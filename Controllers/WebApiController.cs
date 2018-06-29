
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using WaterCool.Models;

namespace WaterCool.Controllers{
    public class WebApiController : Controller
    {
        [HttpGet("/WebApi/getHtml/{page}")]
        [EnableCors("CorsPolicy")] 
        public List<EpriceNewsModel>  getHtml(int page)
        {
            //URL少了前面的網址
            string urlHead = "http://www.eprice.com.tw";
            List<EpriceNewsModel> lsEnm = new List<EpriceNewsModel>();
            var htmlWeb = new HtmlWeb();
            var doc = htmlWeb.Load("http://www.eprice.com.tw/news/all/"+page+"/");
            //選擇的ul整個區塊
            var items = doc.DocumentNode.SelectNodes("//li[contains(@class, 'news')]").ToList();
            foreach(var item in items){
                EpriceNewsModel enm = new EpriceNewsModel();
                enm.title = item.ChildNodes[1].Attributes["title"].Value.ToString();
                enm.url = urlHead + item.ChildNodes[1].Attributes["href"].Value.ToString();
                enm.imgUrl = item.ChildNodes[1].ChildNodes[1].Attributes["src"].Value.ToString();
                enm.postDate = DateTime.Parse(item.ChildNodes[5].ChildNodes[3].ChildNodes[1].ChildNodes[0].InnerText.ToString());
                lsEnm.Add(enm);
            }
            return lsEnm;
        }
        
    }    
}