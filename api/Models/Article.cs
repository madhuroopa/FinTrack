using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    
public class Article
{
    public string id { get; set; }
    public string ticker { get; set; }
    public string title { get; set; }
    public string description { get; set; }
    public string url { get; set; }
    public string content { get; set; }
    public DateTime publishedAt { get; set; }
    public string rid { get; set; }
}

}