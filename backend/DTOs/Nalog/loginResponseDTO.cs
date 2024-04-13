namespace backend.DTOs.Nalog
{
	public class loginResponseDTO
	{
        public bool is2faEnabled { get; set; }
        public string token { get; set; }
    }
}
