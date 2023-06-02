using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection.Emit;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using GeneticsChallenge;

class Program
{
    static async Task Main(string[] args)
    {
        var httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri("https://gene.lacuna.cc/");

        var user = new User
        {
            Username = "test",
            Email = "test@test.com",
            Password = "testpassword"
        };

        var userJson = JsonConvert.SerializeObject(user);

        var request = new HttpRequestMessage(HttpMethod.Post, "api/users/create");
        request.Content = new StringContent(userJson, System.Text.Encoding.UTF8, "application/json");

        var response = await httpClient.SendAsync(request);

        var responseContent = await response.Content.ReadAsStringAsync();

        User loginData = new User
        {
            Username = "test",
            Password = "testpassword"
        };

        var loginJson = JsonConvert.SerializeObject(loginData);

        var loginRequest = new HttpRequestMessage(HttpMethod.Post, "api/users/login");
        loginRequest.Content = new StringContent(loginJson, System.Text.Encoding.UTF8, "application/json");

        var loginResponse = await httpClient.SendAsync(loginRequest);

        var loginResponseContent = await loginResponse.Content.ReadAsStringAsync();

        var token = JsonConvert.DeserializeObject<Token>(loginResponseContent);

        var jobRequest = new HttpRequestMessage(HttpMethod.Get, "api/dna/jobs");

        jobRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);

        var jobResponse = await httpClient.SendAsync(jobRequest);

        var jobResponseContent = await jobResponse.Content.ReadAsStringAsync();

        var job = JsonConvert.DeserializeObject<JobResponse>(jobResponseContent);


        if (job.Job.Type == "DecodeStrand")
        {
            Console.WriteLine($"Job requested successfully. Job ID: {job.Job.Id}, Type: {job.Job.Type}, StrandEncoded: {job.Job.StrandEncoded}");

            string dnaStrand = DnaDecodeStrand.DecodeStrand(job.Job.StrandEncoded);

            Job strand = new Job
            {
                Strand = dnaStrand
            };
            var submitJson = JsonConvert.SerializeObject(strand);
            var submitRequest = new HttpRequestMessage(HttpMethod.Post, $"api/dna/jobs/{job.Job.Id}/decode");
            submitRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);
            submitRequest.Content = new StringContent(submitJson, Encoding.UTF8, "application/json");

            var submitResponse = await httpClient.SendAsync(submitRequest);

            var submitResponseContent = await submitResponse.Content.ReadAsStringAsync();

            var jobResult = JsonConvert.DeserializeObject<JobResult>(submitResponseContent);

            Console.WriteLine(submitResponseContent);

        }
        else if (job.Job.Type == "EncodeStrand")
        {
            Console.WriteLine($"Job requested successfully. Job ID: {job.Job.Id}, Type: {job.Job.Type}, Strand: {job.Job.Strand}");

            string base64Strand = DnaEncodeStrand.EncodeStrand(job.Job.Strand);

            Job StrandEncoded = new Job
            {
                StrandEncoded = base64Strand
            };
            var submitJson = JsonConvert.SerializeObject(StrandEncoded);
            var submitRequest = new HttpRequestMessage(HttpMethod.Post, $"api/dna/jobs/{job.Job.Id}/encode");
            submitRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);
            submitRequest.Content = new StringContent(submitJson, Encoding.UTF8, "application/json");

            var submitResponse = await httpClient.SendAsync(submitRequest);

            var submitResponseContent = await submitResponse.Content.ReadAsStringAsync();

            var jobResult = JsonConvert.DeserializeObject<JobResult>(submitResponseContent);

            Console.WriteLine(submitResponseContent);

        }
        else
        {
            Console.WriteLine($"Job requested successfully. Job ID: {job.Job.Id}, Type: {job.Job.Type}, Gene: {job.Job.GeneEncoded}, DNA Template Strand: {job.Job.StrandEncoded}");

            string strand = DnaDecodeStrand.DecodeStrand(job.Job.StrandEncoded);
            string geneStrand = DnaDecodeStrand.DecodeStrand(job.Job.GeneEncoded);

            string templateString;

            if (strand.StartsWith("CAT"))
            {
                templateString = strand;
                Console.WriteLine($"Template Strand: {templateString}");
            }
            else
            {
                templateString = new string(strand.Select(nucleotide =>
                {
                    switch (nucleotide)
                    {
                        case 'A':
                            return 'T';
                        case 'C':
                            return 'G';
                        case 'G':
                            return 'C';
                        case 'T':
                            return 'A';
                        default:
                            return nucleotide;
                    }
                }).ToArray());

                Console.WriteLine($"Template Strand: {templateString}");
            }
            char[] geneArray = geneStrand.ToCharArray();
            char[] strandArray = templateString.ToCharArray();

            int geneLength = geneArray.Length;
            int strandLength = strandArray.Length;
            int minimumMatchLength = geneLength / 2 + 1;

            bool isActivated = false;

            for (int geneIndex = 0; geneIndex <= geneLength - minimumMatchLength; geneIndex++)
            {
                int matchCount = 0;
                int strandIndex = 0;

                while (strandIndex < strandLength)
                {
                    if (geneArray[geneIndex] == strandArray[strandIndex])
                    {
                        int tempGeneIndex = geneIndex;
                        int tempStrandIndex = strandIndex;
                        int tempMatchCount = 0;

                        while (tempGeneIndex < geneLength && tempStrandIndex < strandLength && geneArray[tempGeneIndex] == strandArray[tempStrandIndex])
                        {
                            tempMatchCount++;
                            tempGeneIndex++;
                            tempStrandIndex++;
                        }

                        if (tempMatchCount > matchCount)
                        {
                            matchCount = tempMatchCount;
                        }
                    }

                    strandIndex++;
                }

                if (matchCount >= geneLength / 2)
                {
                    isActivated = true;
                    break;
                }
            }

            Console.WriteLine($"Gene Activation: {(isActivated ? "Activated" : "Not Activated")}");

            var requestBody = new
            {
                isActivated = isActivated
            };

            var submitJson = JsonConvert.SerializeObject(requestBody);

            var submitRequest = new HttpRequestMessage(HttpMethod.Post, $"api/dna/jobs/{job.Job.Id}/gene");
            submitRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);
            submitRequest.Content = new StringContent(submitJson, Encoding.UTF8, "application/json");

            var submitResponse = await httpClient.SendAsync(submitRequest);

            var submitResponseContent = await submitResponse.Content.ReadAsStringAsync();

            var jobResult = JsonConvert.DeserializeObject<JobResult>(submitResponseContent);

            Console.WriteLine(submitResponseContent);
        }



    }


}