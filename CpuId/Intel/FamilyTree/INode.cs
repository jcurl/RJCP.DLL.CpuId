namespace RJCP.Diagnostics.Intel.FamilyTree
{
    using System.Collections.Generic;

    internal interface INode : ICollection<INode>
    {
        int Key { get; }

        string Value { get; set; }

        INode this[int key] { get; }
    }
}
