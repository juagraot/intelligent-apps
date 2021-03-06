﻿using AdatumTaxCorpKnowledgeService.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace AdatumTaxCorpKnowledgeService.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            FaqContext db = new FaqContext();
            var allFaqs = db.Faqs.ToList();
            ViewBag.FaqsList = allFaqs;
            return View();
        }
        
        [HttpPost]
        public JsonResult FindTaxAnswer(QnaMakerQuestion inquiry)
        {
            JsonResult result;
            string kbIdName = "knowledgeBaseID";
            string keyName = "Ocp-Apim-Subscription-Key";
            string contentTypeName = "Content-Type";
            string contentTypeValue = "application/json";

            var kbIdValue = WebConfigurationManager.AppSettings[kbIdName];
            var keyValue = WebConfigurationManager.AppSettings[keyName];

            if (inquiry.Question is null || inquiry.Question == "")
            {
                result = Json("Please provide a question");
            }
            else
            {
                string url = $"https://westus.api.cognitive.microsoft.com/qnamaker/v2.0/knowledgebases/{kbIdValue}/generateAnswer";
                string body = JsonConvert.SerializeObject(inquiry);

                using (WebClient client = new WebClient())
                {
                    client.Headers.Add($"{keyName}:{keyValue}");
                    client.Headers.Add($"{contentTypeName}:{contentTypeValue}");
                    result = Json(client.UploadString(url, body));
                }
                
            }

            return result;
        }
    }
}