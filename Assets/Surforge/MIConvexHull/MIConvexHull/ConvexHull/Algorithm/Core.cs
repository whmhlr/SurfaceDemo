namespace MIConvexHull // 定义MIConvexHull命名空间
//TODO：以后命名空间修改
{
    /*
    * 算法的主要部分
    * 基本思路：
    * - 创建初始包围壳（在 Initialize.cs 文件中完成）*
    对于每个面而言，都存在一些“超出该面的顶点”，这些顶点从该面本身是可见的。
    如果没有这样的顶点，那么该面就位于外层轮廓上。*
    * - 而且至少存在一个具有至少一个“超出端点”的面：
    *   * 选取距离该端点最远的那个端点
    *   * 对于该端点：
    *     > 找出从它能够看到的所有面（标记为“受影响面”）
    *     > 移除这些面，并用由该端点和受影响面的边界所形成的“锥体”来替换它们
    *       对于每个新形成的面，计算“超出端点的端点”（创建锥体 + 提交锥体）*
    * + 以一种快速实现但难以理解且难以维护的方式来进行。
    */

    internal partial class ConvexHullInternal // 定义ConvexHullInternal内部类，这是一个部分类
    {                
        /// <summary>
        /// 将当前顶点所能看到的所有面都标记为1
        /// </summary>
        /// <param name="currentFace"></param>
        void TagAffectedFaces(ConvexFaceInternal currentFace) // 标记当前顶点可见的所有面
        {
            AffectedFaceBuffer.Clear(); // 清空受影响面的缓冲区
            AffectedFaceBuffer.Add(currentFace.Index); // 将当前面的索引添加到缓冲区
            TraverseAffectedFaces(currentFace.Index); // 遍历受影响的面
        }
        
        /// <summary>
        /// 遍历递归所有受影响的面
        /// </summary>
        void TraverseAffectedFaces(int currentFace) // 递归遍历所有相关的面
        {
            TraverseStack.Clear(); // 清空遍历栈
            TraverseStack.Push(currentFace); // 将当前面压入栈中
            AffectedFaceFlags[currentFace] = true; // 标记当前面为受影响

            while (TraverseStack.Count > 0) // 当栈不为空时继续遍历
            {
                var top = FacePool[TraverseStack.Pop()]; // 弹出栈顶的面
                for (int i = 0; i < Dimension; i++) // 遍历面的所有维度
                {
                    var adjFace = top.AdjacentFaces[i]; // 获取相邻的面

                    // 如果相邻面未被标记且当前顶点到该面的距离大于等于平面距离容差
                    if (!AffectedFaceFlags[adjFace] && MathHelper.GetVertexDistance(CurrentVertex, FacePool[adjFace]) >= PlaneDistanceTolerance)
                    {
                        AffectedFaceBuffer.Add(adjFace); // 将相邻面添加到受影响面缓冲区
                        AffectedFaceFlags[adjFace] = true; // 标记相邻面为受影响
                        TraverseStack.Push(adjFace); // 将相邻面压入栈中，继续遍历
                    }
                }
            }
        }
                
        /// <summary>
        /// 创建一个新的绘制面
        /// </summary>
        /// <param name="face"></param>
        /// <param name="faceIndex"></param>
        /// <param name="pivot"></param>
        /// <param name="pivotIndex"></param>
        /// <param name="oldFace"></param>
        /// <returns></returns>
        DeferredFace MakeDeferredFace(ConvexFaceInternal face, int faceIndex, ConvexFaceInternal pivot, int pivotIndex, ConvexFaceInternal oldFace) // 创建一个新的延迟面
        {
            var ret = ObjectManager.GetDeferredFace(); // 从对象管理器获取一个延迟面
            
            ret.Face = face; // 设置延迟面的面引用
            ret.FaceIndex = faceIndex; // 设置面索引
            ret.Pivot = pivot; // 设置枢轴面
            ret.PivotIndex = pivotIndex; // 设置枢轴索引
            ret.OldFace = oldFace; // 设置旧面

            return ret; // 返回创建的延迟面
        }

        /// <summary>
        /// 使用连接器连接面
        /// </summary>
        /// <param name="connector"></param>
        void ConnectFace(FaceConnector connector) // 使用连接器连接面
        {
            var index = connector.HashCode % ConnectorTableSize; // 计算连接器在表中的索引
            var list = ConnectorTable[index]; // 获取连接器列表

            for (var current = list.First; current != null; current = current.Next) // 遍历连接器列表
            {
                if (FaceConnector.AreConnectable(connector, current, Dimension)) // 检查两个连接器是否可以连接
                {
                    list.Remove(current); // 从列表中移除当前连接器
                    FaceConnector.Connect(current, connector); // 连接两个连接器
                    current.Face = null; // 清空当前连接器的面引用
                    connector.Face = null; // 清空连接器的面引用
                    ObjectManager.DepositConnector(current); // 将当前连接器归还对象管理器
                    ObjectManager.DepositConnector(connector); // 将连接器归还对象管理器
                    return; // 返回
                }
            }

            list.Add(connector); // 如果没有找到可连接的连接器，则将其添加到列表中
        }

        /// <summary>
        /// 移除当前顶点所“覆盖”的面部，并添加新创建的面部
        /// </summary>
        private bool CreateCone() // 移除被当前顶点"覆盖"的面并添加新创建的面
        {
            var currentVertexIndex = CurrentVertex; // 获取当前顶点索引
            ConeFaceBuffer.Clear(); // 清空锥面缓冲区

            for (int fIndex = 0; fIndex < AffectedFaceBuffer.Count; fIndex++) // 遍历所有受影响的面
            {
                var oldFaceIndex = AffectedFaceBuffer[fIndex]; // 获取旧面索引
                var oldFace = FacePool[oldFaceIndex]; // 获取旧面

                // Find the faces that need to be updated
                int updateCount = 0; // 需要更新的面计数
                for (int i = 0; i < Dimension; i++) // 遍历面的所有维度
                {
                    var af = oldFace.AdjacentFaces[i]; // 获取相邻的面
                    if (!AffectedFaceFlags[af]) // 如果相邻面未被标记为受影响
                    { // Tag == false when oldFaces does not contain af
                        UpdateBuffer[updateCount] = af; // 将相邻面添加到更新缓冲区
                        UpdateIndices[updateCount] = i; // 记录相邻面的索引
                        ++updateCount; // 增加计数
                    }
                }

                for (int i = 0; i < updateCount; i++) // 遍历所有需要更新的面
                {
                    var adjacentFace = FacePool[UpdateBuffer[i]]; // 获取相邻面

                    int oldFaceAdjacentIndex = 0; // 旧面在相邻面中的索引
                    var adjFaceAdjacency = adjacentFace.AdjacentFaces; // 相邻面的邻接数组
                    for (int j = 0; j < adjFaceAdjacency.Length; j++) // 遍历邻接数组
                    {
                        if (oldFaceIndex == adjFaceAdjacency[j]) // 如果找到旧面
                        {
                            oldFaceAdjacentIndex = j; // 记录索引
                            break; // 跳出循环
                        }
                    }

                    var forbidden = UpdateIndices[i]; // 禁止的索引（对应于该相邻面的面索引）

                    int oldVertexIndex; // 旧顶点索引
                    int[] vertices; // 顶点数组

                    var newFaceIndex = ObjectManager.GetFace(); // 获取一个新面的索引
                    var newFace = FacePool[newFaceIndex]; // 获取新面
                    vertices = newFace.Vertices; // 获取新面的顶点数组
                    for (int j = 0; j < Dimension; j++) vertices[j] = oldFace.Vertices[j]; // 复制旧面的顶点到新面
                    oldVertexIndex = vertices[forbidden]; // 获取禁止位置的旧顶点索引

                    int orderedPivotIndex; // 有序的枢轴索引

                    // correct the ordering
                    if (currentVertexIndex < oldVertexIndex) // 如果当前顶点索引小于旧顶点索引
                    {
                        orderedPivotIndex = 0; // 设置有序枢轴索引为0
                        for (int j = forbidden - 1; j >= 0; j--) // 从禁止位置向前遍历
                        {
                            if (vertices[j] > currentVertexIndex) vertices[j + 1] = vertices[j]; // 如果顶点大于当前顶点，则后移
                            else
                            {
                                orderedPivotIndex = j + 1; // 设置有序枢轴索引
                                break; // 跳出循环
                            }
                        }
                    }
                    else // 如果当前顶点索引大于等于旧顶点索引
                    {
                        orderedPivotIndex = Dimension - 1; // 设置有序枢轴索引为维度减1
                        for (int j = forbidden + 1; j < Dimension; j++) // 从禁止位置向后遍历
                        {
                            if (vertices[j] < currentVertexIndex) vertices[j - 1] = vertices[j]; // 如果顶点小于当前顶点，则前移
                            else
                            {
                                orderedPivotIndex = j - 1; // 设置有序枢轴索引
                                break; // 跳出循环
                            }
                        }
                    }
                    
                    vertices[orderedPivotIndex] = CurrentVertex; // 在有序枢轴索引位置设置当前顶点

                    if (!MathHelper.CalculateFacePlane(newFace, Center)) // 计算新面的平面，如果失败
                    {
                        return false; // 返回false
                    }

                    ConeFaceBuffer.Add(MakeDeferredFace(newFace, orderedPivotIndex, adjacentFace, oldFaceAdjacentIndex, oldFace)); // 添加新创建的延迟面到锥面缓冲区
                }
            }
            
            return true; // 返回true表示成功创建锥面
        }

        /// <summary>
        /// 构建一个圆锥体，并向凸包中添加一个顶点
        /// </summary>
        void CommitCone() // 提交锥面并将顶点添加到凸包
        {            
            // Fill the adjacency.
            for (int i = 0; i < ConeFaceBuffer.Count; i++) // 遍历所有锥面
            {
                var face = ConeFaceBuffer[i]; // 获取当前锥面

                var newFace = face.Face; // 获取新面
                var adjacentFace = face.Pivot; // 获取相邻面
                var oldFace = face.OldFace; // 获取旧面
                var orderedPivotIndex = face.FaceIndex; // 获取有序枢轴索引

                newFace.AdjacentFaces[orderedPivotIndex] = adjacentFace.Index; // 设置新面在有序枢轴索引位置的邻接面
                adjacentFace.AdjacentFaces[face.PivotIndex] = newFace.Index; // 设置相邻面在枢轴索引位置的邻接面

                // let there be a connection.
                for (int j = 0; j < Dimension; j++) // 遍历所有维度
                {
                    if (j == orderedPivotIndex) continue; // 跳过有序枢轴索引位置
                    var connector = ObjectManager.GetConnector(); // 获取一个连接器
                    connector.Update(newFace, j, Dimension); // 更新连接器
                    ConnectFace(connector); // 连接面
                }
                
                // the id adjacent face on the hull? If so, we can use simple method to find beyond vertices.
                if (adjacentFace.VerticesBeyond.Count == 0) // 如果相邻面没有超出顶点
                {
                    FindBeyondVertices(newFace, oldFace.VerticesBeyond); // 查找新面的超出顶点
                }
                // it is slightly more effective if the face with the lower number of beyond vertices comes first.
                else if (adjacentFace.VerticesBeyond.Count < oldFace.VerticesBeyond.Count) // 如果相邻面的超出顶点数量少于旧面
                {
                    FindBeyondVertices(newFace, adjacentFace.VerticesBeyond, oldFace.VerticesBeyond); // 查找新面的超出顶点，优先使用相邻面
                }
                else // 否则
                {
                    FindBeyondVertices(newFace, oldFace.VerticesBeyond, adjacentFace.VerticesBeyond); // 查找新面的超出顶点，优先使用旧面
                }

                // This face will definitely lie on the hull
                if (newFace.VerticesBeyond.Count == 0) // 如果新面没有超出顶点，说明它在凸包上
                {
                    ConvexFaces.Add(newFace.Index); // 将新面添加到凸包面集合
                    UnprocessedFaces.Remove(newFace); // 从未处理面集合中移除新面
                    ObjectManager.DepositVertexBuffer(newFace.VerticesBeyond); // 归还顶点缓冲区
                    newFace.VerticesBeyond = EmptyBuffer; // 设置新面的超出顶点为空缓冲区
                }
                else // Add the face to the list
                {
                    UnprocessedFaces.Add(newFace); // 否则将新面添加到未处理面集合
                }

                // recycle the object.
                ObjectManager.DepositDeferredFace(face); // 归还延迟面对象
            }

            // Recycle the affected faces.
            for (int fIndex = 0; fIndex < AffectedFaceBuffer.Count; fIndex++) // 遍历所有受影响的面
            {
                var face = AffectedFaceBuffer[fIndex]; // 获取面索引
                UnprocessedFaces.Remove(FacePool[face]); // 从未处理面集合中移除该面
                ObjectManager.DepositFace(face); // 归还面对象
            }
        }
        
        /// <summary>
        /// 检查顶点 v 是否位于指定面之外。如果是，则将其添加到 beyondVertices 中
        /// </summary>
        /// <param name="face"></param>
        /// <param name="beyondVertices"></param>
        /// <param name="v"></param>
        void IsBeyond(ConvexFaceInternal face, IndexBuffer beyondVertices, int v) // 检查顶点v是否在给定面之外，如果是，则添加到beyondVertices
        {
            double distance = MathHelper.GetVertexDistance(v, face); // 计算顶点v到面的距离
            if (distance >= PlaneDistanceTolerance) // 如果距离大于等于平面距离容差
            {
                if (distance > MaxDistance) // 如果距离大于最大距离
                {
                    // If it's within the tolerance distance, use the lex. larger point
                    if (distance - MaxDistance < PlaneDistanceTolerance) // 如果在容差范围内
                    {
                        if (LexCompare(v, FurthestVertex) > 0) // 如果顶点v的字典序大于最远顶点
                        {
                            MaxDistance = distance; // 更新最大距离
                            FurthestVertex = v; // 更新最远顶点
                        }
                    }
                    else // 否则
                    {
                        MaxDistance = distance; // 更新最大距离
                        FurthestVertex = v; // 更新最远顶点
                    }
                }
                beyondVertices.Add(v); // 将顶点v添加到超出顶点集合
            }
        }
        
        /// <summary>
        /// 查找面的超出顶点，用于更新面
        /// </summary>
        void FindBeyondVertices(ConvexFaceInternal face, IndexBuffer beyond, IndexBuffer beyond1) // 查找面的超出顶点，用于更新面
        {
            var beyondVertices = BeyondBuffer; // 获取超出顶点缓冲区

            MaxDistance = double.NegativeInfinity; // 初始化最大距离为负无穷
            FurthestVertex = 0; // 初始化最远顶点为0
            int v; // 顶点变量

            for (int i = 0; i < beyond1.Count; i++) VertexMarks[beyond1[i]] = true; // 标记beyond1中的顶点
            VertexMarks[CurrentVertex] = false; // 取消标记当前顶点
            for (int i = 0; i < beyond.Count; i++) // 遍历beyond中的顶点
            {
                v = beyond[i]; // 获取顶点
                if (v == CurrentVertex) continue; // 跳过当前顶点
                VertexMarks[v] = false; // 取消标记顶点
                IsBeyond(face, beyondVertices, v); // 检查顶点是否在面之外
            }

            for (int i = 0; i < beyond1.Count; i++) // 遍历beyond1中的顶点
            {
                v = beyond1[i]; // 获取顶点
                if (VertexMarks[v]) IsBeyond(face, beyondVertices, v); // 如果顶点仍被标记，检查是否在面之外
            }

            face.FurthestVertex = FurthestVertex; // 设置面的最远顶点

            // Pull the old switch a roo (switch the face beyond buffers)
            var temp = face.VerticesBeyond; // 临时保存面的超出顶点
            face.VerticesBeyond = beyondVertices; // 设置面的超出顶点为新计算的缓冲区
            if (temp.Count > 0) temp.Clear(); // 如果临时缓冲区非空，则清空
            BeyondBuffer = temp; // 更新BeyondBuffer为临时缓冲区
        }

        void FindBeyondVertices(ConvexFaceInternal face, IndexBuffer beyond) // 查找面的超出顶点（重载版本）
        {
            var beyondVertices = BeyondBuffer; // 获取超出顶点缓冲区

            MaxDistance = double.NegativeInfinity; // 初始化最大距离为负无穷
            FurthestVertex = 0; // 初始化最远顶点为0
            int v; // 顶点变量

            for (int i = 0; i < beyond.Count; i++) // 遍历beyond中的顶点
            {
                v = beyond[i]; // 获取顶点
                if (v == CurrentVertex) continue; // 跳过当前顶点
                IsBeyond(face, beyondVertices, v); // 检查顶点是否在面之外
            }
            
            face.FurthestVertex = FurthestVertex; // 设置面的最远顶点

            // Pull the old switch a roo (switch the face beyond buffers)
            var temp = face.VerticesBeyond; // 临时保存面的超出顶点
            face.VerticesBeyond = beyondVertices; // 设置面的超出顶点为新计算的缓冲区
            if (temp.Count > 0) temp.Clear(); // 如果临时缓冲区非空，则清空
            BeyondBuffer = temp; // 更新BeyondBuffer为临时缓冲区
        }
                        
        /// <summary>
        /// 重新计算当前凸包的重心位置
        /// </summary>
        void UpdateCenter() // 重新计算当前凸包的质心
        {
            for (int i = 0; i < Dimension; i++) Center[i] *= ConvexHullSize; // 质心坐标乘以凸包大小
            ConvexHullSize += 1; // 凸包大小加1
            double f = 1.0 / ConvexHullSize; // 计算比例因子
            var co = CurrentVertex * Dimension; // 计算当前顶点的坐标偏移
            for (int i = 0; i < Dimension; i++) Center[i] = f * (Center[i] + Positions[co + i]); // 更新质心坐标
        }

        /// <summary>
        /// 从质心中移除最后一个顶点
        /// </summary>
        void RollbackCenter() // 从质心中移除最后一个顶点
        {
            for (int i = 0; i < Dimension; i++) Center[i] *= ConvexHullSize; // 质心坐标乘以凸包大小
            ConvexHullSize -= 1; // 凸包大小减1
            double f = ConvexHullSize > 0 ? 1.0 / ConvexHullSize : 0.0; // 计算比例因子，如果凸包大小为0则为0
            var co = CurrentVertex * Dimension; // 计算当前顶点的坐标偏移
            for (int i = 0; i < Dimension; i++) Center[i] = f * (Center[i] - Positions[co + i]); // 更新质心坐标
        }

        /// <summary>
        /// Handles singular vertex.
        /// </summary>
        void HandleSingular() // 处理奇异顶点
        {
            RollbackCenter(); // 回滚质心
            SingularVertices.Add(CurrentVertex); // 将当前顶点添加到奇异顶点集合

            // This means that all the affected faces must be on the hull and that all their "vertices beyond" are singular.
            for (int fIndex = 0; fIndex < AffectedFaceBuffer.Count; fIndex++) // 遍历所有受影响的面
            {
                var face = FacePool[AffectedFaceBuffer[fIndex]]; // 获取面
                var vb = face.VerticesBeyond; // 获取面的超出顶点
                for (int i = 0; i < vb.Count; i++) // 遍历超出顶点
                {
                    SingularVertices.Add(vb[i]); // 将超出顶点添加到奇异顶点集合
                }

                ConvexFaces.Add(face.Index); // 将面添加到凸包面集合
                UnprocessedFaces.Remove(face); // 从未处理面集合中移除面
                ObjectManager.DepositVertexBuffer(face.VerticesBeyond); // 归还顶点缓冲区
                face.VerticesBeyond = EmptyBuffer; // 设置面的超出顶点为空缓冲区
            }
        }

        /// <summary>
        /// Fins the convex hull.
        /// </summary>
        void FindConvexHull() // 查找凸包
        {
            // Find the (dimension+1) initial points and create the simplexes.
            InitConvexHull(); // 初始化凸包，找到(dimension+1)个初始点并创建单纯形

            // Expand the convex hull and faces.
            while (UnprocessedFaces.First != null) // 当有未处理的面时继续循环
            {
                var currentFace = UnprocessedFaces.First; // 获取第一个未处理的面
                CurrentVertex = currentFace.FurthestVertex; // 设置当前顶点为面的最远顶点
                                                
                UpdateCenter(); // 更新质心

                // The affected faces get tagged
                TagAffectedFaces(currentFace); // 标记受影响的面

                // Create the cone from the currentVertex and the affected faces horizon.
                if (!SingularVertices.Contains(CurrentVertex) && CreateCone()) CommitCone(); // 如果当前顶点不是奇异顶点且成功创建锥面，则提交锥面
                else HandleSingular(); // 否则处理奇异顶点

                // Need to reset the tags
                int count = AffectedFaceBuffer.Count; // 获取受影响面的数量
                for (int i = 0; i < count; i++) AffectedFaceFlags[AffectedFaceBuffer[i]] = false; // 重置受影响面的标记
            }
        }
    }
}