namespace BookStore.DTOs.BookDTOs
{
    public class BookDetailsDTOs
    {
        public string Book_title { get; set; }
        public decimal Book_price { get; set; }
        public string Book_photo { get; set; }
        public DateOnly Book_publishDate { get; set; }
    }
}
