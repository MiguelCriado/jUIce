using System.Collections.Generic;
using UnityEngine;

namespace Juice
{
	public class ViewModelComponentTreeNode
	{
		public Transform Transform { get; }
		public Transform ParentObject { get; set; }
		public ViewModelComponentTreeNode ParentNode { get; private set; }
		public Dictionary<string, ViewModelComponentInfo> Components { get; }
		public LinkedList<ViewModelComponentTreeNode> Children { get; }

		public ViewModelComponentTreeNode(Transform transform)
		{
			Transform = transform;
			Components = new Dictionary<string, ViewModelComponentInfo>();
			Children = new LinkedList<ViewModelComponentTreeNode>();
		}

		public void SetParentNode(ViewModelComponentTreeNode parent)
		{
			ParentNode = parent;
			ParentNode?.Children.AddLast(this);
		}

		public void AddComponent(ViewModelComponent component)
		{
			Components[component.Id] = new ViewModelComponentInfo(component);
		}
	}
}
