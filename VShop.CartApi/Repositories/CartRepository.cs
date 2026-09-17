using AutoMapper;
using Microsoft.EntityFrameworkCore;
using VShop.CartApi.Context;
using VShop.CartApi.DTOs;
using VShop.CartApi.Models;

namespace VShop.CartApi.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly AppDbContext _context;
        private IMapper _mapper;

        public CartRepository(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CartDTO> GetCartByUserIdAsync(string userId)
        {
            Cart cart = new Cart
            {
                CartHeader = await _context.CartHeaders.FirstOrDefaultAsync(ch => ch.UserId == userId)
            };

            cart.CartItems = _context.CartItems
                .Where(ci => ci.CartHeaderId == cart.CartHeader.Id)
                .Include(ci => ci.Product);

            return _mapper.Map<CartDTO>(cart);
        }

        public async Task<bool> DeleteItemCartAsync(int cartItemId)
        {
            try
            {
                var cartItem = await _context.CartItems.FirstOrDefaultAsync(ci => ci.Id == cartItemId);

                int totalItems = _context.CartItems.Count(ci => ci.CartHeaderId == cartItem.CartHeaderId);

                _context.CartItems.Remove(cartItem);

                if (totalItems == 1)
                {
                    var cartHeaderToRemove = await _context.CartHeaders.FirstOrDefaultAsync(ch => ch.Id == cartItem.CartHeaderId);
                    _context.CartHeaders.Remove(cartHeaderToRemove);
                }

                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message, "Erro ao remover item do carrinho!");
                return false;
            }
        }

        public async Task<bool> CleanCartAsync(string userId)
        {
            try
            {
                var cartHeader = await _context.CartHeaders.FirstOrDefaultAsync(ch => ch.UserId == userId);

                if (cartHeader is not null)
                {
                    _context.CartItems.RemoveRange(
                        _context.CartItems.Where(ci => ci.CartHeaderId == cartHeader.Id)
                    );

                    _context.CartHeaders.Remove(cartHeader);

                    await _context.SaveChangesAsync();
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message, "Erro ao limpar o carrinho!");

                return false;
            }
        }

        public async Task<CartDTO> UpdateCartAsync(CartDTO cartDTO)
        {
            Cart cart = _mapper.Map<Cart>(cartDTO);

            await SaveProductInDataBase(cartDTO, cart);

            var cartHeaderFromDb = await _context.CartHeaders.AsNoTracking()
                .FirstOrDefaultAsync(ch => ch.UserId == cart.CartHeader.UserId);

            if (cartHeaderFromDb is null)
            {
                await CreateCartHeaderAndItems(cart);
            }
            else
            {
                await UpdateQuantityAndItems(cartDTO, cartHeaderFromDb, cart);
            }

            return _mapper.Map<CartDTO>(cart);
        }

        private async Task UpdateQuantityAndItems(CartDTO cartDTO, CartHeader? cartHeaderFromDb, Cart cart)
        {
            try
            {
                var cartDetail = await _context.CartItems.AsNoTracking()
                .FirstOrDefaultAsync(p => p.ProductId == cartDTO.CartItems.FirstOrDefault().ProductId
                && p.CartHeaderId == cartHeaderFromDb.Id);

                if (cartDetail is null)
                {
                    cart.CartItems.FirstOrDefault().CartHeaderId = cartHeaderFromDb.Id;
                    cart.CartItems.FirstOrDefault().Product = null;
                    _context.CartItems.Add(cart.CartItems.FirstOrDefault());
                    await _context.SaveChangesAsync();
                }
                else
                {
                    cart.CartItems.FirstOrDefault().Product = null;
                    cart.CartItems.FirstOrDefault().Quantity += cartDetail.Quantity;
                    cart.CartItems.FirstOrDefault().Id = cartDetail.Id;
                    cart.CartItems.FirstOrDefault().CartHeaderId = cartDetail.CartHeaderId;

                    _context.CartItems.Update(cart.CartItems.FirstOrDefault());

                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}, Erro ao atualizar o carrinho");
            }
        }

        private async Task CreateCartHeaderAndItems(Cart cart)
        {
            _context.CartHeaders.Add(cart.CartHeader);
            await _context.SaveChangesAsync();

            cart.CartItems.FirstOrDefault().CartHeaderId = cart.CartHeader.Id;
            cart.CartItems.FirstOrDefault().Product = null;

            _context.CartItems.Add(cart.CartItems.FirstOrDefault());

            await _context.SaveChangesAsync();
        }

        private async Task SaveProductInDataBase(CartDTO cartDTO, Cart cart)
        {
            try
            {
                var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == cartDTO.CartItems.FirstOrDefault().ProductId);

                if (product is null)
                {
                    _context.Products.Add(cart.CartItems.FirstOrDefault().Product);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message, "Erro ao salvar produto no banco!");
            }
        }

        public Task<bool> ApplyCouponAsync(string userId, string couponCode)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveCouponAsync(string userId)
        {
            throw new NotImplementedException();
        }
    }
}
