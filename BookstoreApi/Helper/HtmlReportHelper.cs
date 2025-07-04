using BookstoreApi.Dtos;
using System.Text;

public static class HtmlReportHelper
{
    public static string GenerateBooksReportHtml(List<BookDto> books)
    {
        var html = new StringBuilder();
        html.Append("<html><head><title>Books Report</title>");
        html.Append(@"
        <style>
            body {
                font-family: Arial, sans-serif;
                padding: 20px;
                background-color: #f5f5f5;
            }
            h1 {
                color: #333;
            }
            table {
                width: 100%;
                border-collapse: collapse;
                margin-top: 20px;
                background-color: #fff;
                box-shadow: 0 0 10px rgba(0,0,0,0.1);
            }
            th, td {
                border: 1px solid #ccc;
                padding: 10px;
                text-align: left;
            }
            th {
                background-color: #4CAF50;
                color: white;
            }
            tr:nth-child(even) {
                background-color: #f9f9f9;
            }
        </style>
        ");
        html.Append("</head><body>");
        html.Append("<h1>Books Report</h1>");
        html.Append("<table>");
        html.Append("<tr><th>Title</th><th>Authors</th><th>Category</th><th>Year</th><th>Price</th></tr>");

        foreach (var book in books)
        {
            var authors = string.Join(", ", book.Authors);
            html.Append($"<tr>" +
                        $"<td>{book.Title}</td>" +
                        $"<td>{authors}</td>" +
                        $"<td>{book.Category}</td>" +
                        $"<td>{book.Year}</td>" +
                        $"<td>{book.Price:C}</td>" +
                        $"</tr>");
        }

        html.Append("</table></body></html>");

        return html.ToString();
    }
}

