using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity;


public partial class Pages_ShoppingCart : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //get id of current logged in user and display items in cart
        string userId = User.Identity.GetUserId();
        GetPurchasesInCart(userId);
    }

    private void GetPurchasesInCart(string userId)
    {
        CartModel model = new CartModel();
        double subTotal = 0;

        //generate HTML for each element in purchaseList
        List<Cart> purchaseList = model.GetOrdersInCart(userId);
        CreateShopTable(purchaseList, out subTotal);

        //add totals to webpage
        double taxes = subTotal * 0.25;
        double totalAmount = subTotal + taxes + 10;

        //display values on page
        litTotal.Text = "$ " + subTotal;
        litVat.Text = "$ " + taxes;
        litTotalAmount.Text = "$ " + totalAmount;
    }

    private void CreateShopTable(List<Cart> purchaseList, out double subTotal)
    {
        subTotal = new Double();
        
        ProductModel model = new ProductModel();

        foreach(Cart cart in purchaseList)
        {
            Product product = model.GetProduct(cart.ProductID);

            //create the delete link 
            ImageButton btnImage = new ImageButton
            {
                ImageUrl = string.Format("~/images/Products{0}", product.Image),
                PostBackUrl = string.Format("~/Pages/Product.aspx?id={0}", product.Id)

            };

            //create the delete link
            LinkButton lnkDelete = new LinkButton
            {
                PostBackUrl = string.Format("~/Pages/ShoppingCart.aspx?productId={0}", cart.ID),
                Text = "Delete Item",
                ID = "del" + cart.ID
            };

            //add an OnClick Event
            lnkDelete.Click += Delete_Item;

            //create the quantity dropdown list
            //generate list numbers from 1 to 20
            int[] amount = Enumerable.Range(1, 20).ToArray();
            DropDownList ddlAmount = new DropDownList
            {
                DataSource = amount,
                AppendDataBoundItems = true,
                AutoPostBack = true,
                ID = cart.ID.ToString()
            };
            ddlAmount.DataBind();
            ddlAmount.SelectedValue = cart.Amount.ToString();
            ddlAmount.SelectedIndexChanged += ddlAmount_SelectedIndexChanged;

            //create HTML table with 2 rows
            Table table = new Table { CssClass = "cartTable" };
            TableRow a = new TableRow();
            TableRow b = new TableRow();

            //create 6 cells for row a
            TableCell a1 = new TableCell { RowSpan =2 , Width = 50};
            TableCell a2 = new TableCell { Text = string.Format("<h4>{0}</h4><br/>{1}<br/>In Stock",
                product.Name, "Item No: " + product.Id),
                HorizontalAlign = HorizontalAlign.Left, Width = 350};
            TableCell a3 = new TableCell { Text = "Unit Price<hr/>"};
            TableCell a4 = new TableCell { Text = "Quantity<hr/>"};
            TableCell a5 = new TableCell { Text = "Item Total<hr/>"};
            TableCell a6 = new TableCell { };

            //create 6 cells for row b
            TableCell b1 = new TableCell { };
            TableCell b2 = new TableCell { Text = "$ " + product.Price};
            TableCell b3 = new TableCell { };
            TableCell b4 = new TableCell { Text = "$ " + Math.Round(Convert.ToDouble(cart.Amount * product.Price), 2)};
            TableCell b5 = new TableCell { };
            TableCell b6 = new TableCell { };

            //set special controls
            a1.Controls.Add(btnImage);
            a6.Controls.Add(lnkDelete);
            b3.Controls.Add(ddlAmount);

            //add cells to rows
            a.Cells.Add(a1);
            a.Cells.Add(a2);
            a.Cells.Add(a3);
            a.Cells.Add(a4);
            a.Cells.Add(a5);
            a.Cells.Add(a6);

            a.Cells.Add(b1);
            a.Cells.Add(b2);
            a.Cells.Add(b3);
            a.Cells.Add(b4);
            a.Cells.Add(b5);
            a.Cells.Add(b6);

            //add row to tables
            table.Rows.Add(a);
            table.Rows.Add(b);

            //add tablew to pnl shoppingCart
            pnlShoppingCart.Controls.Add(table);

            //add total amount of item in cart to subtotal
            
            subTotal += Convert.ToDouble(cart.Amount * product.Price);
        }

        //add current user's shopping cart to user specific session value
        Session[User.Identity.GetUserId()] = purchaseList;
    }

    private void ddlAmount_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList selectedList = (DropDownList)sender;
        int quantity = Convert.ToInt32(selectedList.SelectedValue);
        int cartId = Convert.ToInt32(selectedList.ID);

        CartModel model = new CartModel();
        model.UPdateQuantity(cartId, quantity);

        Response.Redirect("~/Pages/ShoppingCart.aspx");
    }

    private void Delete_Item(object sender, EventArgs e)
    {
        LinkButton selectedLink = (LinkButton)sender;
        string link = selectedLink.ID.Replace("del", "");
        int cartId = Convert.ToInt32(link);

        CartModel model = new CartModel();
        model.DeleteCart(cartId);

        Response.Redirect("~/Pages/ShoppingCart.aspx");
    }
}