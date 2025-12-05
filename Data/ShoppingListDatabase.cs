using DamianIonutLab7.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DamianIonutLab7.Data
{
    public class ShoppingListDatabase
    {
        readonly SQLiteAsyncConnection _database; 
        public ShoppingListDatabase(string dbPath) {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<ShopList>().Wait();
            _database.CreateTableAsync<Product>().Wait();
            _database.CreateTableAsync<ListProduct>().Wait();
            _database.CreateTableAsync<Shop>().Wait();
        }
        public Task<int> SaveProductAsync(Product product) { 
            if (product.ID != 0) {
                return _database.UpdateAsync(product); 
            } 
            else
            { 
                return _database.InsertAsync(product); 
            }
        }
        public Task<int> DeleteProductAsync(Product product) { 
            return _database.DeleteAsync(product);
        }
        public Task<List<Product>> GetProductsAsync() {
            return _database.Table<Product>().ToListAsync();
        }
        public Task<int> SaveListProductAsync(ListProduct listp) { 
            if (listp.ID != 0) { return _database.UpdateAsync(listp); 
            } 
            else 
            {
                return _database.InsertAsync(listp);
            } 
        }
        public Task<List<Product>> GetListProductsAsync(int shoplistid) { 
            return _database.QueryAsync<Product>(
                "select P.ID, P.Description from Product P" 
                + " inner join ListProduct LP" 
                + " on P.ID = LP.ProductID where LP.ShopListID = ?", 
                shoplistid); 
        }
        public Task<int> DeleteShopListAsync(ShopList list)
        {
            return _database.DeleteAsync(list);
        }
        public Task<int> SaveShopListAsync(ShopList list)
        {
            if (list.ID != 0)
            {
                return _database.UpdateAsync(list);
            }
            else
            {
                return _database.InsertAsync(list);
            }
        }
        public Task<List<ShopList>> GetShopListsAsync()
        {
            return _database.Table<ShopList>().ToListAsync();
        }

        // Șterge un produs dintr-o listă specifică (șterge legătura din tabela ListProduct)
        public async Task<int> DeleteProductFromListAsync(int shopListId, int productId)
        {
            // Căutăm legătura exactă dintre lista curentă și produsul selectat
            var listProduct = await _database.Table<ListProduct>()
                                    .Where(lp => lp.ShopListID == shopListId && lp.ProductID == productId)
                                    .FirstOrDefaultAsync();

            if (listProduct != null)
            {
                return await _database.DeleteAsync(listProduct);
            }
            return 0;
        }

        public Task<List<Shop>> GetShopsAsync()
        {
            return _database.Table<Shop>().ToListAsync();
        }

        public Task<int> SaveShopAsync(Shop shop)
        {
            if (shop.ID != 0)
            {
                // Actualizează un magazin existent (ID != 0)
                return _database.UpdateAsync(shop);
            }
            else
            {
                // Inserează un magazin nou (ID == 0)
                return _database.InsertAsync(shop);
            }
        }

        public Task<int> DeleteShopAsync(Shop shop)
        {
            // Utilizează metoda DeleteAsync a conexiunii SQLite pentru a șterge magazinul
            return _database.DeleteAsync(shop);
        }
    }
}
