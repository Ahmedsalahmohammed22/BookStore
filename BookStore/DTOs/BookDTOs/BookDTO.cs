namespace BookStore.DTOs.BookDTOs
{
    public class BookDTO
    {
        public int id {  get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public int stock { get; set; }
        public string photo { get; set; }
        public DateOnly publishDate { get; set; }
        public string author_name { get; set; }
        public string catalog_name { get; set;}
    }
}
