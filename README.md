# Todos Program
 
 Api para cadastro e visualização de tarefas, do qual contempla 5 endpoints

 - GET  `/v1/todos`

 Esse endpoint retorna uma lista com todas as tarefas cadastrados

- Post `/v1/todos`

Para cadastro de uma nova tarefa, deve-se passar os seguintes dados: 

`{
  "title": "string",
  "description": "stringstri",
  "daysToFinish": 0
}`

Regras : 
 * title conter mais de 3 caracteres
 * description conter mais de 10 caracteres
 * nenhum campo pode ser vazio ou nulo

 - Put `/v1/todos/{id}/update`

** Substituir {id} pelo id da tarefa

 Para atualização dos dados cadastrados.
`{
  "title": "string",
  "description": "stringstri",
  "daysToFinish": 0
}`
 
Regras : 
 * title conter mais de 3 caracteres
 * description conter mais de 10 caracteres
 * Não é necessário enviar todos os campos
 * não é possível atualizar tarefa com status done

 - Put `/v1/todos/{id}/update/status`

 ** Substituir {id} pelo id da tarefa

Para atualização do status da tarefa.

{
  "status": "string"
}

Regras : 
 * status deve ser preenchido com um dos seguintes valores: "todo", "doing", "done"
 * campo não pode ser vazio ou nulo
 * não é possível atualizar tarefa com status done

- Delete `/v1/todos/{id}`

 ** Substituir {id} pelo id da tarefa

 Para excluir tarefa registrada.


### Informações técnicas

#### Execução de testes unitários
Executar o comando: 
`dotnet test --verbosity minimal --collect:"XPlat Code Coverage"`

O qual irá executar os testes unitários e criar uma pasta com o nome de um GUID dentro do diretorio Todo.Tests/TestResult e dentro dessa pasta o arquivo coverage.cobertura.xml. Para converter esse arquivo e obter de forma visual é necessário instalar uma ferramenta com o seguinte comando:
`dotnet tool install --global dotnet-reportgenerator-globaltool --version 4.8.6`

Com a ferramenta instalada, basta entrar no diretorio onde o arquivo coverage.cobertura.xml se encontra e executar o seguinte comando
`reportgenerator "-reports:coverage.cobertura.xml" "-targetdir:coveragereport" -reporttypes:Html`

Com isso será gerado uma pasta chamada coveragereport, que contem um arquivo index.html, basta selecionar e clicar duas vezes nesse arquivo para visualiza-lo.

#### Configuração de banco de dados
  Nas configurações relacionadas a dados foi deixado na classe Program.cs a opção de conexão com banco de dados SQL, o qual encontra-se comentado, e a opção do banco de dados InMemory que encontra-se ativo, para que seja utilizado conforme necessidade. 
 Para uso do banco de dados InMemory basta executar a aplicação.
 Para uso de banco de dados SQL é necessário seguir os passos abaixo: 

 1 - ajuste das propriedades de ambiente encontradas no arquivo "settings.json", fornecendo suas credenciais (user, senha)

 2 - Abrir o terminal de Package-Manager e adicionar uma nova migration do banco de dados, utilizando o seguinte comando, alterando {nome da migration} pelo nome que deseja colocar na sua migration: 

  `Add-Migration {nome da migration}`

  3 - Ainda no Package-Manager Realizar update da base de dados para que as alterações das migration sejam aplicadas

  `Update-Database`

Após esses comandos é possivel executar o projeto utilizando o F5 normalmente.


