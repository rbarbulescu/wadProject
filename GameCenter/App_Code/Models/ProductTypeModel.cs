using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ProductTypeModel
/// </summary>
public class ProductTypeModel
{
    public string InsertProductType(ProductType productType)
    {
        try
        {
            GameCenterDBEntities db = new GameCenterDBEntities();
            db.ProductTypes.Add(productType);
            db.SaveChanges();

            return productType.Name + " was succesfully inserted.";
        }
        catch (Exception e)
        {
            return "Error" + e;
        }
    }

    public string UpdateProductType(int id, ProductType productType)
    {
        try
        {
            GameCenterDBEntities db = new GameCenterDBEntities();

            //Fetch object from database
            ProductType p = db.ProductTypes.Find(id);

            p.Name = productType.Name;

            db.SaveChanges();
            return productType.Name + " was succesfully updated.";
        }
        catch (Exception e)
        {
            return "Error" + e;
        }
    }


    public string DeleteProductType(int id)
    {
        try
        {
            GameCenterDBEntities db = new GameCenterDBEntities();

            //Finding the object that needs to be deleted
            ProductType productType = db.ProductTypes.Find(id);

            db.ProductTypes.Attach(productType);
            db.ProductTypes.Remove(productType);
            db.SaveChanges();

            return productType.Name + " was succesfully updated.";

        }
        catch (Exception e)
        {
            return "Error" + e;
        }
    }
}