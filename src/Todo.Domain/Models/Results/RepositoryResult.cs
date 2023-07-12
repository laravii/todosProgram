namespace Todo.Domain.Models.Results
{
    public class RepositoryResult
    {
        public bool Success { get; }
        public string Message { get; }

        private RepositoryResult(bool success, string message)
        {
            Success = success;
            Message = message;
        }

        public static readonly RepositoryResult Sucess = new(true, "Operação realizada com sucesso");
        public static readonly RepositoryResult NotFound = new(false, "Item não encontrado, revise os dados e tente novamente");
        public static readonly RepositoryResult AlreadyDone = new(false, "Não é possível atualizar itens concluídos");
    }
}
