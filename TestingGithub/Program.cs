using Octokit;

static async Task Main(string[] args)
{
    var github = new GitHubClient(new ProductHeaderValue("MyAmazingApp"));
    github.Credentials = new Credentials("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"); // NOTA: Reemplaza "your-github-token" con tu token de GitHub

    var branchName = "testing";
    var owner = "jairocnsf"; // NOTA: Reemplaza "owner-name" con el nombre del propietario del repositorio
    var repoName = "testing"; // NOTA: Reemplaza "repo-name" con el nombre del repositorio

    // Obtén la referencia de la rama 'master'
    var masterReference = await github.Git.Reference.Get(owner, repoName, "heads/master");
    var latestCommit = masterReference.Object.Sha;

    // Crea una nueva referencia de rama apuntando al último commit
    await github.Git.Reference.Create(owner, repoName, new NewReference($"refs/heads/{branchName}", latestCommit));
}