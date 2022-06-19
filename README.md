# Como salvar imagem utilizando o blob no azure? 🤔

Sejam bem-vindos ao meu GITHUB em parceria com o canal 👉🏻 [UniversoTI](https://www.youtube.com/c/UniversoTi), inscreva-se lá para obter dicas, tutoriais e tirar dúvidas sobre o mundo da tecnologia.

-------
### Problema
Hoje em dia ainda vemos muitas empresas tem adotado em salvar imagens no banco de dados, mas sabemos que isso não é uma boa prática pois podemos prejudicar nossa base com esses métodos antigos.

O blob veio para salvar nossas vídas e armazenar apenas uma string que pode renderizar uma imagem ou mais em nossas aplicações web.
Não apenas o blob mas hoje temos soluções como servidor FTP dentre outros que podemos fugir em salvar esses tipos de arquivos em nossas bases. 😁

### Solução
Como solução em não salvar imagem diretamente no banco de dados, vamos salvar nossas imagens no blob do Azure, o mesmo retornara uma url e podemos salvar em nossa base a string retornada da URL sendo assim temos em nossa base apenas a string da url e não a imagem em sí.

------

# Processo para salvar imagem
### Configuração no Azure.
1. O primeiro passo é a gente ter uma conta no Azure, caso você já tenha basta entrar no link [portal.azure.com](https://portal.azure.com/#home) e efetuar seu login.
2. Feito o login vamos nos deperar com uma dashboard do Azure, vamos clicar em *Conta de Armazenamento*.
![image](https://user-images.githubusercontent.com/99252640/174483965-e9485d80-0f8a-4a70-a3e1-c6e6c0685e04.png)
3. Caso você ainda não tenha nenhuma conta de armazenamento, vamos criar um seguindo os passos abaixo.
4. Na página de armazenamento, vamos clicar no link Criar conforme a imagem.
![image](https://user-images.githubusercontent.com/99252640/174484066-4354e7cf-1860-4556-bc70-b726ce784ab3.png)
5. No processo de criar uma conta de armazenamento, precisamos obrigatóriamente informar um grupo de recurso e caso não tenha por favor, crie uma.
![image](https://user-images.githubusercontent.com/99252640/174484119-42a41db2-35c6-40a7-a511-64c80736e178.png)
6.Logo abaixo vamos informar o nome da nossa conta de armazenamento, a região e o desempenho que vou deixar standar.
![image](https://user-images.githubusercontent.com/99252640/174484195-7483ac04-6e53-4128-beaf-11d9ca983813.png)
7. Feito os passos acima, as demais abas deixei conforme veio pré configurado e mandei criar.

------

# O projeto
### Criando nosso projeto.
1. Para esse exemplo criei um projeto ASP.NET CORE 6
2. Depois de criado, a primeira coisa que devemos configurar é o nosso _appsettings.json_ 
![image](https://user-images.githubusercontent.com/99252640/174484425-f1a465b2-2d94-4d5e-8b35-3a1989d878a5.png)

```csharp
    "StorageConfiguration": {
    "NomeArmazenamento": "****",
    "ChaveAcesso": "*****",
    "NomeContainer": "****"
    }
  ```
  
  > NomeArmazenamento: Nome que foi dado a sua conta de armazenamento quando foi criado.
  > Chave de acesso da conta de armazenamento que você criou.
  > Nome do seu container.

### Como pegar a chave de acesso? 🤔
- Para pegar sua chave de acesso, entre na conta de armazenamento que você criou no menu ao lado clica e escolha a opção _chave de acesso_.
![image](https://user-images.githubusercontent.com/99252640/174484631-25b95cb1-3fc9-4a45-8472-0b3091bba901.png)

- Feito isso você terá sua chave de acesso, para visualiza-la basta clicar no ícone de olho.
![image](https://user-images.githubusercontent.com/99252640/174484679-b0edaac2-ab42-4e20-8560-66462dff9ca5.png)

### Como criar um container?
- Para criar um container você vai entrar na sua conta de armazenamento, e clicar no menu containeres.
![image](https://user-images.githubusercontent.com/99252640/174484752-ddb30b8f-acc4-4ae5-9897-9e8ae6449062.png)
- Acessando esse menu, basta clicar em cima da opção + Container, dar um nome e permitir que esse container seja publico.
![image](https://user-images.githubusercontent.com/99252640/174484809-b4d0844f-bcb8-499b-a6dc-8832fa1af46e.png)

3. Agora com todas essas informações, vamos inserir conforme solicitado em nosso appsettings.
4. Com nosso appsettings.json devidamente criado, vamos criar uma **classe** que iremos salvar a imagem. No meu exemplo criei uma classe simulando um produto.
Para criar uma classe entre na pasta model - botão direito - Nova classe.
![image](https://user-images.githubusercontent.com/99252640/174484951-489e73df-05f0-4998-8d76-5bcda9d28a97.png)
![image](https://user-images.githubusercontent.com/99252640/174484977-f5a6407f-63a9-479c-beef-78d80be5a88b.png)

5. Dentro da classe podemos definir as propriedades que vai compor nossa classe, nesse exemplo utilizei apenas 3 campos, mas para nosso exemplo no blob vou usar apenas a propriedade responsavel para armazenar a url.
```csharp
      public class Produto
      {
        public string Nome { get; set; }
        public decimal Preco { get; set; }
        public string ImagemUrl { get; set; }
      }
```
6. Depois de ter criada a classe, vamos agora criar um controller chamado ProdutoController, é o mesmo processo de criação da classe o que vai mudar é que na hora de criar uma classe o tipo vai ser controller conforme a imagem abaixo:
![image](https://user-images.githubusercontent.com/99252640/174485122-1390ab1c-03a0-403a-ad6e-90e21e46d351.png)

7. Depois de criado vamos criar o código conforme abaixo:
```csharp
    public class ProdutoController : Controller
    {

        private readonly IConfiguration _configuration;

        public ProdutoController(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }


        [HttpGet]
        public IActionResult Salvar()
        {
            return View();
        }

        #region Operações Post

        /// <summary>
        /// Método responsável por salvar os arquivos no blob azure.
        /// </summary>
        /// <param name="file">Assinatura para receber o arquivo que esta sendo enviado diretamente da web.</param>
        /// <param name="">Assinatura da nossa classe de produtos</param>
        [HttpPost]
        public async Task<IActionResult> SalvarFoto([FromForm] Produto produto, IFormFile file )
        {
            var urlImagem = await Upload(file);
            produto.ImagemUrl = urlImagem;
            return Ok(produto);

        }

        /// <summary>
        /// Método responsável por realizar o processo de upload em nosso blob.
        /// </summary>
        /// <param name="file">Assinatura que vai receber o arquivo que o usuário nos enviou pela web.</param>
        private async Task<string> Upload(IFormFile file)
        {
            // Recupera as credenciais de conexão do azure, via injeção de dependência do appsettings.json
            var nomeArmazenamento = _configuration["StorageConfiguration:NomeArmazenamento"];
            var chaveAcesso = _configuration["StorageConfiguration:ChaveAcesso"];
            var nomeContainer = _configuration["StorageConfiguration:NomeContainer"];
            // Acima criamos um objeto contendo as credenciais de acesso do Azure Blob Storage e abrimos uma conexão com suas APIs.


            var storageCredentials = new StorageCredentials(nomeArmazenamento, chaveAcesso);
            var storageAccount = new CloudStorageAccount(storageCredentials, true);
            var blobAzure = storageAccount.CreateCloudBlobClient();
            var container = blobAzure.GetContainerReference(nomeContainer); // Pegamos a referência do container que vamos utilizar para realizar o upload

            var blob = container.GetBlockBlobReference(file.FileName); // Atribuimos um nome de arquivo para o nosso blob, ou seja, podemos manter o próprio nome ou atribuir um novo nome.
            blob.Properties.ContentType = file.ContentType; // Aqui é definido o tipo do arquivo | Nesse trecho é definido o tipo do arquivo, ou seja, sua extensão e quando especificamos isso, podemos abrir a imagem via navegador sem realizar um download, porém, quando não especificamos essa informação, ao acessar pelo navegador, o download do arquivo é realizado.
            await blob.UploadFromStreamAsync(file.OpenReadStream()); // Realizamos o upload do arquivo para o servidor do azure em nosso blob

            return blob.SnapshotQualifiedStorageUri.PrimaryUri.ToString(); // Obtemos a url de referência do arquivo no blob no qual acabamos de realizar o upload.

        }

        #endregion
    }
```

8. Agora com nosso controller criado, devemos criar a view responsavel por mandar nosso arquivo e para isso clique com o botão direito em cima do método _Salvar - Adicionar Exibição..._
![image](https://user-images.githubusercontent.com/99252640/174485272-7735a955-d79c-4321-a353-42b05fa60e23.png)
9. Depois de ter feito esse passo, em nossa pasta _view - produto_ vamos ter um documento HTML pasta colocar como utilizei o exemplo:
```csharp
    <form class="form-labels-on-top" method="post" asp-action="SalvarFoto" enctype="multipart/form-data" >
    <div class="form-title-row">
        <h1>Testando salvar imagem</h1>
    </div>
    <div class="form-row">
        <label>
            <span>Foto</span>
            <input type="file" name="file" />
        </label>
    </div>
    <div class="form-row">
        <button type="submit">Salvar</button>
    </div>
</form>

```
10. Pronto agora so testar. 😁

