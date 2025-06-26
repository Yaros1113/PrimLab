using Core.Models;
using DAL.Data;
using Microsoft.EntityFrameworkCore;

public class ClientService
{
    private readonly AppDbContext _context;

    public ClientService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ClientResponseDTO> CreateClientAsync(ClientCreateDTO dto)
    {
        var client = new Client
        {
            Name = dto.Name,
            Email = dto.Email,
            RegistrationDate = DateTime.UtcNow,
            UserId = dto.UserId
        };

        await _context.Clients.AddAsync(client);
        await _context.SaveChangesAsync();

        // Добавляем телефоны
        foreach (var phone in dto.PhoneNumbers)
        {
            _context.ClientPhones.Add(new ClientPhone
            {
                ClientId = client.Id,
                PhoneNumber = phone
            });
        }
        
        await _context.SaveChangesAsync();

        return MapToDTO(client);
    }

    public async Task<PagedList<ClientResponseDTO>> GetAllClientsAsync(
        int page = 1, 
        int pageSize = 10,
        string search = "",
        string sortBy = "Name",
        bool ascending = true)
    {
        var query = _context.Clients
            .Include(c => c.Phones)
            .AsQueryable();

        // Фильтрация (поиск)
        if (!string.IsNullOrEmpty(search))
        {
            query = query.Where(c => 
                c.Name.Contains(search) || 
                c.Email.Contains(search));
        }

        // Сортировка
        switch (sortBy.ToLower())
        {
            case "name":
                query = ascending ? 
                    query.OrderBy(c => c.Name) : 
                    query.OrderByDescending(c => c.Name);
                break;
            case "email":
                query = ascending ? 
                    query.OrderBy(c => c.Email) : 
                    query.OrderByDescending(c => c.Email);
                break;
            case "registrationdate":
                query = ascending ? 
                    query.OrderBy(c => c.RegistrationDate) : 
                    query.OrderByDescending(c => c.RegistrationDate);
                break;
            default:
                query = query.OrderBy(c => c.Name);
                break;
        }

        // Пагинация
        return await PagedList<ClientResponseDTO>.CreateAsync(
            query.Select(c => MapToDTO(c)),
            page,
            pageSize
        );
    }

    // Get By Id
    public async Task<ClientResponseDTO?> GetClientByIdAsync(int id)
    {
        return await _context.Clients
            .Include(c => c.Phones)
            .Where(c => c.Id == id)
            .Select(c => MapToDTO(c))
            .FirstOrDefaultAsync();
    }

    // Update
    public async Task<ClientResponseDTO?> UpdateClientAsync(int id, ClientUpdateDTO dto)
    {
        var client = await _context.Clients
            .Include(c => c.Phones)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (client == null) return null;

        // Обновляем основные поля
        client.Name = dto.Name;
        client.Email = dto.Email;
        client.UserId = dto.UserId;

        // Обновляем телефоны
        var existingPhones = client.Phones.ToList();
        _context.ClientPhones.RemoveRange(existingPhones);
        
        foreach (var phone in dto.PhoneNumbers)
        {
            _context.ClientPhones.Add(new ClientPhone
            {
                ClientId = client.Id,
                PhoneNumber = phone
            });
        }

        await _context.SaveChangesAsync();
        return MapToDTO(client);
    }

    // Delete
    public async Task<bool> DeleteClientAsync(int id)
    {
        var client = await _context.Clients.FindAsync(id);
        if (client == null) return false;

        _context.Clients.Remove(client);
        await _context.SaveChangesAsync();
        return true;
    }

    private static ClientResponseDTO MapToDTO(Client client)
    {
        return new ClientResponseDTO
        {
            Id = client.Id,
            Name = client.Name,
            Email = client.Email,
            RegistrationDate = client.RegistrationDate,
            UserId = client.UserId,
            PhoneNumbers = client.Phones.Select(p => p.PhoneNumber).ToList()
        };
    }
}