class Quicksort
{
    public string Sort(string str)
    {
        char[] charArray = str.ToCharArray();
        Sort(charArray, 0, charArray.Length - 1);
        return new string(charArray);
    }

    private void Sort(char[] arr, int left, int right)
    {
        if (left < right)
        {
            int pivotIndex = Partition(arr, left, right);

            Sort(arr, left, pivotIndex - 1);
            Sort(arr, pivotIndex + 1, right);
        }
    }

    private int Partition(char[] arr, int left, int right)
    {
        char pivot = arr[right];
        int i = left - 1;

        for (int j = left; j < right; j++)
        {
            if (arr[j] < pivot)
            {
                i++;
                Swap(arr, i, j);
            }
        }

        Swap(arr, i + 1, right);
        return i + 1;
    }

    private void Swap(char[] arr, int i, int j)
    {
        char temp = arr[i];
        arr[i] = arr[j];
        arr[j] = temp;
    }
}
