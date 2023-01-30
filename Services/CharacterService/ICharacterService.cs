public interface ICharacterService
{
    Task<ServiceResponse<List<ReadCharacterDto>>> GetCharacters(int userId);
    Task<ServiceResponse<ReadCharacterDto>> GetCharacterById(int id);
    Task<ServiceResponse<List<ReadCharacterDto>>> CreateCharacter(CreateCharacterDto newCharacter);
    Task<ServiceResponse<ReadCharacterDto>> UpdateCharacter(int id, UpdateCharacterDto newCharacter);
    Task<ServiceResponse<ReadCharacterDto>> DeleteCharacter(int id);

    bool SaveChanges();
}