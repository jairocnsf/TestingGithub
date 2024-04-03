using Octokit;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        await Octokit();
        await Github();
        
    }

    static async Task Github()
    {
        try
        {
            var token = "token"; // NOTA: Tu token de GitHub
            var owner = "jairocnsf"; // NOTA: Nnombre del propietario del repositorio
            var repo = "TestingGithub"; // NOTA: Nombre del repositorio
            var branchName = "production";

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("User-Agent", "TestingGitHub");
                client.DefaultRequestHeaders.Add("Authorization", $"token {token}");

                // Obtén la referencia de la rama 'master'
                var masterRefResponse = await client.GetStringAsync($"https://api.github.com/repos/{owner}/{repo}/git/refs/heads/master");
                var masterRefSha = JObject.Parse(masterRefResponse)["object"]["sha"].ToString();

                // Crea una nueva referencia de rama apuntando al último commit
                var newRefContent = $"{{\"ref\": \"refs/heads/{branchName}\", \"sha\": \"{masterRefSha}\"}}";
                var newRefResponse = await client.PostAsync($"https://api.github.com/repos/{owner}/{repo}/git/refs", new StringContent(newRefContent, Encoding.UTF8, "application/json"));
            }

            Console.WriteLine("La rama ha sido creada");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Se produjo un error: {ex.Message}");
        }
    }

    static async Task Octokit()
    {
        try
        {
            var github = new GitHubClient(new ProductHeaderValue("TestingGithub"));
            github.Credentials = new Credentials("token"); // NOTA: Tu token de GitHub

            var branchName = "issues";
            var owner = "jairocnsf"; // NOTA: Nombre del propietario del repositorio
            var repoName = "TestingGithub"; // NOTA: Nombre del repositorio

            // Obtén la referencia de la rama 'master'
            var masterReference = await github.Git.Reference.Get(owner, repoName, "heads/master");
            var latestCommit = masterReference.Object.Sha;

            // Crea una nueva referencia de rama apuntando al último commit
            await github.Git.Reference.Create(owner, repoName, new NewReference($"refs/heads/{branchName}", latestCommit));

            Console.WriteLine("La rama ha sido creada");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Se produjo un error: {ex.Message}");
        }
    }
}