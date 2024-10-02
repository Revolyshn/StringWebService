namespace StringProcessingWebAPI.Sorting
{
    public record RandomNumberData(int Number);
    class Treesort
    {
        private class TreeNode
        {
            public char val;
            public TreeNode? left;
            public TreeNode? right;

            public TreeNode(char c)
            {
                val = c;
                left = null;
                right = null;
            }
        }

        private TreeNode? root;

        private TreeNode Insert(TreeNode node, char c)
        {
            if (node == null)
            {
                node = new TreeNode(c);
            }
            else
            {
                if (c < node.val)
                {
                    node.left = Insert(node.left, c);
                }
                else
                {
                    node.right = Insert(node.right, c);
                }
            }
            return node;
        }

        public void BuildTree(string str)
        {
            foreach (char c in str)
            {
                root = Insert(root, c);
            }
        }

        private void InOrderTraversal(TreeNode node, List<char> sortedList)
        {
            if (node != null)
            {
                InOrderTraversal(node.left, sortedList);
                sortedList.Add(node.val);
                InOrderTraversal(node.right, sortedList);
            }
        }

        public string GetSortedString()
        {
            List<char> sortedList = new List<char>();
            InOrderTraversal(root, sortedList);
            return new string(sortedList.ToArray());
        }
    }

}