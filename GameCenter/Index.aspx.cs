using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Index : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        FillPage();
    }

    private void FillPage()
    {
        //get a list of all products
        ProductModel productModel = new ProductModel();
        List<Product> products = productModel.GetAllProducts();

        //make sure products exist in the db
        if(products != null)
        {
            //create a new panel with ImageButton and 2 labels for each product
            foreach(Product product in products)
            {
                Panel productPanel = new Panel();
                ImageButton imageButton = new ImageButton();
                Label lblName = new Label();
                Label lblPrice = new Label();

                //set childcontrol properties
                imageButton.ImageUrl = "~/images/Products/" + product.Image;
                imageButton.CssClass = "productImage";
                imageButton.PostBackUrl = "~/Pages/Product.aspx?id=" + product.Id;

                lblName.Text = product.Name;
                lblName.CssClass = "productName";

                lblPrice.Text = "$ " + product.Price;
                lblPrice.CssClass = "productPrice";

                //add child control to panel
                productPanel.Controls.Add(imageButton);
                productPanel.Controls.Add(new Literal { Text = "<br />" });
                productPanel.Controls.Add(lblName);
                productPanel.Controls.Add(new Literal { Text = "<br />" });
                productPanel.Controls.Add(lblPrice);

                //add dynamic panels to static parent panel
                pnlProducts.Controls.Add(productPanel);
            }
        }
        else
        {
            //No products found
            pnlProducts.Controls.Add(new Literal { Text = "No products found! " });
        }
    }


    protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
}