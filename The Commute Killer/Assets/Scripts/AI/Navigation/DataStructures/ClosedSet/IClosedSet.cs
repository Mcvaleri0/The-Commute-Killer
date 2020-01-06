﻿using System.Collections.Generic;

public interface IClosedSet
{
    void Initialize();
    void AddToClosed(NodeRecord nodeRecord);
    void RemoveFromClosed(NodeRecord nodeRecord);
    //should return null if the node is not found
    NodeRecord SearchInClosed(NodeRecord nodeRecord);
    ICollection<NodeRecord> All();
}
