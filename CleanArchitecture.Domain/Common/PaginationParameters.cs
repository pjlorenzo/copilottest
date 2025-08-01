namespace CleanArchitecture.Domain.Common;

public class PaginationParameters
{
    private const int MaxPageSize = 50;
    private int _pageSize = 10;
    private int _pageNumber = 1;

    public int PageNumber
    {
        get => _pageNumber;
        set => _pageNumber = value < 1 ? 1 : value;
    }

    public int PageSize
    {
        get => _pageSize;
        set
        {
            if (value > MaxPageSize)
                _pageSize = MaxPageSize;
            else if (value < 1)
                _pageSize = 1;
            else
                _pageSize = value;
        }
    }
}
