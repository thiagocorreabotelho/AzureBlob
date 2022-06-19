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
