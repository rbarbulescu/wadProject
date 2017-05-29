using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for CartModel
/// </summary>
public class CartModel
{
    public string InsertCart(Cart cart)
    {
        try
        {
            GameCenterDBEntities db = new GameCenterDBEntities();

            //Adding a cart
            db.Carts.Add(cart);
            db.SaveChanges();

            return cart.DatePurchased + " was succesfully inserted.";
        }
        catch (Exception e)
        {
            return "Error" + e;
        }
    }

    public string UpdateCart(int id, Cart cart)
    {
        try
        {
            GameCenterDBEntities db = new GameCenterDBEntities();

            //Fetch object from database
            Cart p = db.Carts.Find(id);

            p.DatePurchased = cart.DatePurchased;
            p.ClientID = cart.ClientID;
            p.Amount = cart.Amount;
            p.IsInCart = cart.IsInCart;
            p.ProductID = cart.ProductID;
            
            db.SaveChanges();
            return cart.DatePurchased + " was succesfully updated.";
        }
        catch (Exception e)
        {
            return "Error" + e;
        }
    }


    public string DeleteCart(int id)
    {
        try
        {
            GameCenterDBEntities db = new GameCenterDBEntities();

            //Finding the object that needs to be deleted
            Cart cart = db.Carts.Find(id);

            db.Carts.Attach(cart);
            db.Carts.Remove(cart);
            db.SaveChanges();

            return cart.DatePurchased + " was succesfully updated.";

        }
        catch (Exception e)
        {
            return "Error" + e;
        }
    }

    public List<Cart>  GetOrdersInCart(string userId)
    {
        GameCenterDBEntities db = new GameCenterDBEntities();
        List<Cart> orders = (from x in db.Carts
                             where x.ClientID == userId 
                             && x.IsInCart
                             orderby x.DatePurchased
                             select x).ToList();

        return orders;
    }

    public int GetAmountOfOrders(string userId)
    {
        try
        {
            GameCenterDBEntities db = new GameCenterDBEntities();
            int amount = (from x in db.Carts
                          where x.ClientID == userId 
                          && x.IsInCart
                          select x.Amount).Sum();

            return amount;
        }
        catch
        {
            return 0;
        }
    }

    public void UPdateQuantity(int id, int quantity)
    {
        GameCenterDBEntities db = new GameCenterDBEntities();
        Cart cart = db.Carts.Find(id);
        cart.Amount = quantity;

        db.SaveChanges();
    }

    public void MarkOrdersAsPaid(List<Cart> carts)
    {
        GameCenterDBEntities db = new GameCenterDBEntities();

        if(carts != null)
        {
            foreach(Cart cart in carts)
            {
                Cart oldCart = db.Carts.Find(cart.ID);
                oldCart.DatePurchased = DateTime.Now;
                oldCart.IsInCart = false;
            }

            db.SaveChanges();
        }
    }
}