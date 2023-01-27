using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using DenscreenMap.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DenscreenMap.Controllers
{
    [Route("api")]
    [ApiController]
    public class BaseController : Controller
    {
        [HttpPost("check")]
        public async Task<IActionResult> CheckNewForm([FromBody] CheckNewFormRequest checkNewFormRequest)
        {
            var client = new HttpClient();
            
            var oldId = checkNewFormRequest.oldId;
            var allSubsRequest = new HttpRequestMessage
            {
                RequestUri = new Uri($"https://www.formstack.com/api/v2/form/3887663/submission.json?per_page=2&sort=DESC&data=true&expand_data=true"),
                Method = HttpMethod.Get
            };
            
            allSubsRequest.Headers.Add("Authorization", "APIKEY";

            var allSubsResponse = await client.SendAsync(allSubsRequest);
            var allSubsResponseContent = await allSubsResponse.Content.ReadAsStringAsync();

            var allSubs = JsonConvert.DeserializeObject<AllSubmissions>(allSubsResponseContent);
            
            var newId = allSubs.Submissions[0].Id;
            if (oldId != newId)
            {
                var latestSubRequest = new HttpRequestMessage
                {
                    RequestUri = new Uri($"https://www.formstack.com/api/v2/submission/{newId}.json?"),
                    Method = HttpMethod.Get
                };
                
                latestSubRequest.Headers.Add("Authorization", "APIKEY");
                
                var latestSubResponse  = await client.SendAsync(latestSubRequest);
                var latestSubResponseContent = await latestSubResponse.Content.ReadAsStringAsync();
                
                var latestSub = JsonConvert.DeserializeObject<SpecificSubmission>(latestSubResponseContent);
                
                var postcode = string.Empty;
                var controlLine = string.Empty;
                var IgG = string.Empty;
                var IgM = string.Empty;

                var counter = 0;
                
                while (postcode == string.Empty || controlLine == string.Empty || IgG == string.Empty || IgM == string.Empty)
                {
                    var fieldcontents = latestSub.Data[counter].Field;
                    switch (fieldcontents)
                    {
                        case 92817547:
                            postcode = latestSub.Data[counter].Value;
                            break;
                        case 92824785:
                            controlLine = latestSub.Data[counter].Value;
                            break;
                        case 92825822:
                            IgG = latestSub.Data[counter].Value;
                            break;
                        case 92825971:
                            IgM = latestSub.Data[counter].Value;
                            break;
                    }
                    counter++;
                }
                var timestamp = allSubs.Submissions[0].Timestamp;
                
                var geocodeRequest = new HttpRequestMessage
                {
                    RequestUri = new Uri($"https://maps.googleapis.com/maps/api/geocode/json?address={postcode}&region=uk&key=APIKEY2"),
                    Method = HttpMethod.Get
                };
                
                var geocodeResponse  = await client.SendAsync(geocodeRequest);
                var geocodeResponseContent = await geocodeResponse.Content.ReadAsStringAsync();

                var geocodeObject = JsonConvert.DeserializeObject<GoogleGeocodeResponse>(geocodeResponseContent);

                if (geocodeObject.Status != "OK")
                    return BadRequest(new {newForm = true, status = "fail", message = "new form found but geocoding failed"});

                var newLocation = geocodeObject.Results[0].Geometry.Location;
                
                string colour;
                if (controlLine == "Negative")
                    colour = "https://images.squarespace-cdn.com/content/v1/5ea969a9c6ec5a41d0623807/1597755860815-6HX50VZ0IUQ29JCAFDXX/ke17ZwdGBToddI8pDm48kAf-OpKpNsh_OjjU8JOdDKBZw-zPPgdn4jUwVcJE1ZvWQUxwkmyExglNqGp0IvTJZUJFbgE-7XRK3dMEBRBhUpxq_kr5JmUSWzdbeQhVZ8KGVEAfgr0ybhloMHNIqvT8SMftTfHgE5YY4gbHxAHibYY/DenScreen-Map-GIF-grey.gif";
                else
                {
                    if (IgM == "Negative" && IgG == "Negative")
                        colour = "https://images.squarespace-cdn.com/content/v1/5ea969a9c6ec5a41d0623807/1597755831591-1ELAZS11YMW6USQODWFP/ke17ZwdGBToddI8pDm48kAf-OpKpNsh_OjjU8JOdDKBZw-zPPgdn4jUwVcJE1ZvWQUxwkmyExglNqGp0IvTJZUJFbgE-7XRK3dMEBRBhUpxq_kr5JmUSWzdbeQhVZ8KGVEAfgr0ybhloMHNIqvT8SMftTfHgE5YY4gbHxAHibYY/DenScreen-Map-GIF-amber.gif";
                    else
                    {
                        if ((IgM == "Positive" || IgM == "Weak Positive" || IgM == "Strong Positive") && (IgG == "Postive" || IgG == "Weak Positive" || IgG == "Strong Positive"))
                            colour = "https://images.squarespace-cdn.com/content/v1/5ea969a9c6ec5a41d0623807/1597755800745-MHN0I1AN6X6QZHR7BEXG/ke17ZwdGBToddI8pDm48kAf-OpKpNsh_OjjU8JOdDKBZw-zPPgdn4jUwVcJE1ZvWQUxwkmyExglNqGp0IvTJZUJFbgE-7XRK3dMEBRBhUpxq_kr5JmUSWzdbeQhVZ8KGVEAfgr0ybhloMHNIqvT8SMftTfHgE5YY4gbHxAHibYY/DenScreen-Map-GIF-green.gif";
                        else
                        {
                            if (IgG == "Positive" || IgG == "Weak Positive" || IgM == "Strong Positive")
                                colour = "https://images.squarespace-cdn.com/content/v1/5ea969a9c6ec5a41d0623807/1597755800745-MHN0I1AN6X6QZHR7BEXG/ke17ZwdGBToddI8pDm48kAf-OpKpNsh_OjjU8JOdDKBZw-zPPgdn4jUwVcJE1ZvWQUxwkmyExglNqGp0IvTJZUJFbgE-7XRK3dMEBRBhUpxq_kr5JmUSWzdbeQhVZ8KGVEAfgr0ybhloMHNIqvT8SMftTfHgE5YY4gbHxAHibYY/DenScreen-Map-GIF-green.gif";
                            else
                                colour = "https://images.squarespace-cdn.com/content/v1/5ea969a9c6ec5a41d0623807/1597755387587-85SSSTGQB6636UZTZBEH/ke17ZwdGBToddI8pDm48kAf-OpKpNsh_OjjU8JOdDKBZw-zPPgdn4jUwVcJE1ZvWQUxwkmyExglNqGp0IvTJZUJFbgE-7XRK3dMEBRBhUpxq_kr5JmUSWzdbeQhVZ8KGVEAfgr0ybhloMHNIqvT8SMftTfHgE5YY4gbHxAHibYY/DenScreen-Map-GIF-red5.gif";
                        }
                    }
                }

                var marker = new Marker
                {
                    Id = latestSub.Id,
                    Lat = newLocation.Lat,
                    Lng = newLocation.Lng,
                    TimeStamp = latestSub.Timestamp.ToString("dd/MM/yyyy HH:mm:ss"),
                    Colour = colour,
                };
                
                return Ok(new {newForm = true, status = "found", message = "new form found and returned position successfully",
                    marker = marker});
            }
            else
            {
                return Ok(new {newForm = false, status = "fail", message = "no new form"});
            }
        }

    [HttpGet("startup")]
        public async Task<IActionResult> Startup()
        {
            var client = new HttpClient();
            
            var allSubsDataRequest = new HttpRequestMessage
            {
                RequestUri = new Uri($"	https://www.formstack.com/api/v2/form/3887663/submission.json?per_page=100&sort=DESC&data=true&expand_data=true"),
                Method = HttpMethod.Get
            };
            
            allSubsDataRequest.Headers.Add("Authorization", "Bearer APIKEY");

            var allSubsDataResponse = await client.SendAsync(allSubsDataRequest);
            var allSubsDataResponseContent = await allSubsDataResponse.Content.ReadAsStringAsync();

            var allSubsData = JsonConvert.DeserializeObject<Welcome>(allSubsDataResponseContent);
            
            var markers =  new List<Marker>();

            for (var i = 99; i >= 0; i--)
            {
                var postcode = allSubsData.Submissions[i].Data["92817547"].FlatValue;
                var controlLine = allSubsData.Submissions[i].Data["92824785"].FlatValue;
                var IgG = allSubsData.Submissions[i].Data["92825822"].FlatValue;
                var IgM = allSubsData.Submissions[i].Data["92825971"].FlatValue;

                var geocodeRequest = new HttpRequestMessage
                {
                    RequestUri = new Uri($"https://maps.googleapis.com/maps/api/geocode/json?address={postcode}&region=uk&key=APIKEY2"),
                    Method = HttpMethod.Get
                };
                
                var geocodeResponse  = await client.SendAsync(geocodeRequest);
                var geocodeResponseContent = await geocodeResponse.Content.ReadAsStringAsync();

                var geocodeObject = JsonConvert.DeserializeObject<GoogleGeocodeResponse>(geocodeResponseContent);

                if (geocodeObject.Status != "OK")
                {
                    
                }
                else
                {
                    var newLocation = geocodeObject.Results[0].Geometry.Location;
                
                    string colour;
                    if (controlLine == "Negative")
                        colour = "https://images.squarespace-cdn.com/content/v1/5ea969a9c6ec5a41d0623807/1597755860815-6HX50VZ0IUQ29JCAFDXX/ke17ZwdGBToddI8pDm48kAf-OpKpNsh_OjjU8JOdDKBZw-zPPgdn4jUwVcJE1ZvWQUxwkmyExglNqGp0IvTJZUJFbgE-7XRK3dMEBRBhUpxq_kr5JmUSWzdbeQhVZ8KGVEAfgr0ybhloMHNIqvT8SMftTfHgE5YY4gbHxAHibYY/DenScreen-Map-GIF-grey.gif";
                    else
                    {
                        if (IgM == "Negative" && IgG == "Negative")
                            colour = "https://images.squarespace-cdn.com/content/v1/5ea969a9c6ec5a41d0623807/1597755831591-1ELAZS11YMW6USQODWFP/ke17ZwdGBToddI8pDm48kAf-OpKpNsh_OjjU8JOdDKBZw-zPPgdn4jUwVcJE1ZvWQUxwkmyExglNqGp0IvTJZUJFbgE-7XRK3dMEBRBhUpxq_kr5JmUSWzdbeQhVZ8KGVEAfgr0ybhloMHNIqvT8SMftTfHgE5YY4gbHxAHibYY/DenScreen-Map-GIF-amber.gif";
                        else
                        {
                            if ((IgM == "Positive" || IgM == "Weak Positive" || IgM == "Strong Positive") && (IgG == "Postive" || IgG == "Weak Positive" || IgG == "Strong Positive"))
                                colour = "https://images.squarespace-cdn.com/content/v1/5ea969a9c6ec5a41d0623807/1597755800745-MHN0I1AN6X6QZHR7BEXG/ke17ZwdGBToddI8pDm48kAf-OpKpNsh_OjjU8JOdDKBZw-zPPgdn4jUwVcJE1ZvWQUxwkmyExglNqGp0IvTJZUJFbgE-7XRK3dMEBRBhUpxq_kr5JmUSWzdbeQhVZ8KGVEAfgr0ybhloMHNIqvT8SMftTfHgE5YY4gbHxAHibYY/DenScreen-Map-GIF-green.gif";
                            else
                            {
                                if (IgG == "Positive" || IgG == "Weak Positive" || IgG == "Strong Positive")
                                    colour = "https://images.squarespace-cdn.com/content/v1/5ea969a9c6ec5a41d0623807/1597755800745-MHN0I1AN6X6QZHR7BEXG/ke17ZwdGBToddI8pDm48kAf-OpKpNsh_OjjU8JOdDKBZw-zPPgdn4jUwVcJE1ZvWQUxwkmyExglNqGp0IvTJZUJFbgE-7XRK3dMEBRBhUpxq_kr5JmUSWzdbeQhVZ8KGVEAfgr0ybhloMHNIqvT8SMftTfHgE5YY4gbHxAHibYY/DenScreen-Map-GIF-green.gif";
                                else
                                    colour = "https://images.squarespace-cdn.com/content/v1/5ea969a9c6ec5a41d0623807/1597755387587-85SSSTGQB6636UZTZBEH/ke17ZwdGBToddI8pDm48kAf-OpKpNsh_OjjU8JOdDKBZw-zPPgdn4jUwVcJE1ZvWQUxwkmyExglNqGp0IvTJZUJFbgE-7XRK3dMEBRBhUpxq_kr5JmUSWzdbeQhVZ8KGVEAfgr0ybhloMHNIqvT8SMftTfHgE5YY4gbHxAHibYY/DenScreen-Map-GIF-red5.gif";
                            }
                        }
                    }
                    
                    markers.Add(new Marker
                    {
                        Id = allSubsData.Submissions[i].Id,
                        Lat = newLocation.Lat,
                        Lng = newLocation.Lng,
                        TimeStamp = allSubsData.Submissions[i].Timestamp.ToString("dd/MM/yyyy HH:mm:ss"),
                        Colour = colour,
                    });
                }
                  
            }

            return Ok(new
                {status = "success", message = "successfully found the latest submissions", markers = markers});
        }

        public class CheckNewFormRequest
        {
            public long oldId { get; set; }
        }
    }
}