using CourseBusinessWebsite.DataContext;
using CourseBusinessWebsite.Entities;

namespace CourseBusinessWebsite.Handle.EmailHandle
{
    public class BillEmailTemplate
    {
        public static string GenerateNotificationBillEmail(Bill bill, string message = "")
        {
            AppDbContext context = new AppDbContext();
            string htmlContent = $@"
            <html>
            <head>
                <style>
                    body {{
                        font-family: Arial, sans-serif;
                    }}
                    image {{
                        width: 60px;
                        height: 70px;
                    }}
                    h1 {{
                        color: #333;
                    }}
                    
                    table {{
                        border-collapse: collapse;
                        width: 100%;
                    }}
                    
                    th, td {{
                        border: 1px solid #ddd;
                        padding: 8px;
                    }}
                    
                    th {{
                        background-color: #f2f2f2;
                        font-weight: bold;
                    }}
                    
                    .footer {{
                        margin-top: 20px;
                        font-size: 14px;
                    }}
                </style>
            </head>
            <body>
                <h1>Thông tin hóa đơn mua khóa học</h1>
                <h2 style=""color: red; font-size: 20px; font-weight: bold;"">{(string.IsNullOrEmpty(message) ? "" : message)}</h2>

                <table>
                    <tr>
                        <th>Mã giao dịch</th>
                        <th>Tổng tiền</th>
                        <th>Trạng thái hóa đơn</th>
                        <th>Tên khách hàng</th>
                        <th>Ngày tạo</th>
                    </tr>
                    <tr>
                        <td>{bill.TradingCode}</td>
                        <td>{bill.TotalPrice}</td>
                        <td>{bill.BillStatus}</td>
                        <td>{context.users.SingleOrDefault(x => x.ID == bill.UserID).Username}</td>
                        <td>{bill.CreateAt}</td>
                    </tr>
                </table>
                
                
                
                <div class=""footer"">
                    <p>Trân trọng,</p>
                    <p>MyBugs Cinema</p>
                </div>
            </body>
            </html>";

            return htmlContent;
        }
    }
}
