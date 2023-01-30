using System.Collections.Generic;
using System.Threading.Tasks;

public interface ICharacterService
{
    Task<ServiceResponse<List<ReadCharacterDto>>> GetCharacters();
    Task<ServiceResponse<ReadCharacterDto>> GetCharacterById(int id);
    Task<ServiceResponse<List<ReadCharacterDto>>> CreateCharacter(CreateCharacterDto newCharacter);
    Task<ServiceResponse<ReadCharacterDto>> UpdateCharacter(int id, UpdateCharacterDto newCharacter);
    Task<ServiceResponse<ReadCharacterDto>> DeleteCharacter(int id);

    bool SaveChanges();
}