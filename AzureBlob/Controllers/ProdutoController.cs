using AzureBlob.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;

namespace AzureBlob.Controllers
{
    public class ProdutoController : Controller
    {

        private readonly IConfiguration _configuration;

        public ProdutoController(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public IActionResult Index()
        {
            return View();
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
}
