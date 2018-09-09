using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using devRantNetCore;

namespace devrant_cli
{
    public class RantProducer
    {
        public Sort SortType {get;set;}
        private DevRantClient Client {get;set;}
        private Queue<Rant> RantBuffer {get;set;}
        private int CursorPosition {get; set;} = 0;

        public RantProducer(Sort sort = Sort.Algo, int bufferSize = 20) {
            SortType = sort;
            Client = DevRantClient.Create(new HttpClient());
            RantBuffer = new Queue<Rant>();
        }

        public async Task<Rant> GetNextRantAsync() {
            if (RantBuffer.Count < 5)
                await Task.Run(async () => {
                    var newRants = await Client.GetRants(SortType, limit:15, skip:CursorPosition);
                    CursorPosition += 15;
                    foreach (var rant in newRants.Rants)
                        RantBuffer.Enqueue(rant);
                });
            return RantBuffer.Dequeue();
        }

        public async Task<IList<Comment>> GetComments(Rant rant) {
            return (await Client.GetRant(rant.Id)).Comments;

        }


    }
}