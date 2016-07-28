﻿extern alias fhir_stu3;
using System;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Trifolia.Authorization;
using Trifolia.DB;
using Trifolia.Export.FHIR.STU3;
using Trifolia.Shared;
using FhirImplementationGuide = fhir_stu3.Hl7.Fhir.Model.ImplementationGuide;

namespace Trifolia.Web.Controllers.API.FHIR.STU3
{
    [STU3Config]
    [RoutePrefix("api/FHIR3")]
    public class FHIR3ImplementationGuideController : ApiController
    {
        private IObjectRepository tdb;
        private ImplementationGuideType implementationGuideType;

        #region Constructors

        public FHIR3ImplementationGuideController(IObjectRepository tdb)
        {
            this.tdb = tdb;
            this.implementationGuideType = Trifolia.Export.FHIR.STU3.Shared.GetImplementationGuideType(this.tdb, true);
        }

        public FHIR3ImplementationGuideController()
            : this(new TemplateDatabaseDataSource())
        {

        }

        private string BaseProfilePath
        {
            get
            {
                return HttpContext.Current.Server.MapPath("~/App_Data/FHIR/DSTU2/");
            }
        }

        #endregion

        /// <summary>
        /// Get the specified implementation guide in FHIR DSTU2 format
        /// </summary>
        /// <param name="implementationGuideId">The id of the implementation guide</param>
        /// <param name="format">Optional. The format to respond with (ex: "application/fhir+xml" or "application/fhir+json")</param>
        /// <param name="summary">Optional. The type of summary to respond with.</param>
        /// <returns>Hl7.Fhir.Model.ImplementationGuide</returns>
        [HttpGet]
        [Route("ImplementationGuide/{implementationGuideId}")]
        [SecurableAction(SecurableNames.IMPLEMENTATIONGUIDE_LIST)]
        public HttpResponseMessage GetImplementationGuide(
            int implementationGuideId,
            [FromUri(Name = "_format")] string format = null,
            [FromUri(Name = "_summary")] fhir_stu3.Hl7.Fhir.Rest.SummaryType? summary = null)
        {
            var implementationGuide = this.tdb.ImplementationGuides.Single(y => y.Id == implementationGuideId);
            SimpleSchema schema = SimplifiedSchemaContext.GetSimplifiedSchema(HttpContext.Current.Application, implementationGuide.ImplementationGuideType);
            ImplementationGuideExporter exporter = new ImplementationGuideExporter(this.tdb, schema, this.BaseProfilePath, this.Request.RequestUri.Scheme, this.Request.RequestUri.Authority);
            FhirImplementationGuide response = exporter.Convert(implementationGuide, summary);
            return Shared.GetResponseMessage(this.Request, format, response);
        }

        /// <summary>
        /// Gets implementation guides in FHIR DSTU2 format. Can specify search information, such as the name of the implementation guide and the id of the implementation guide.
        /// </summary>
        /// <param name="format">Optional. The format to respond with (ex: "application/fhir+xml" or "application/fhir+json")</param>
        /// <param name="summary">Optional. The type of summary to respond with.</param>
        /// <param name="include">Indicate what additional information should be included with the implementation guide (such as "ImplementationGuide:resource")</param>
        /// <param name="implementationGuideId">Specify the id of the implementation guide to search for.</param>
        /// <param name="name">Specifies the name of the implementation guide to search for. Implementation guides whose name *contains* this value will be returned.</param>
        /// <returns>Hl7.Fhir.Model.Bundle</returns>
        [HttpGet]
        [Route("ImplementationGuide")]
        [Route("ImplementationGuide/_search")]
        [SecurableAction(SecurableNames.IMPLEMENTATIONGUIDE_LIST)]
        public HttpResponseMessage GetImplementationGuides(
            [FromUri(Name = "_format")] string format = null,
            [FromUri(Name = "_summary")] fhir_stu3.Hl7.Fhir.Rest.SummaryType? summary = null,
            [FromUri(Name = "_include")] string include = null,
            [FromUri(Name = "_id")] int? implementationGuideId = null,
            [FromUri(Name = "name")] string name = null)
        {
            SimpleSchema schema = SimplifiedSchemaContext.GetSimplifiedSchema(HttpContext.Current.Application, this.implementationGuideType);
            ImplementationGuideExporter exporter = new ImplementationGuideExporter(this.tdb, schema, this.BaseProfilePath, this.Request.RequestUri.Scheme, this.Request.RequestUri.Authority);
            var bundle = exporter.GetImplementationGuides(summary, include, implementationGuideId, name);
            return Shared.GetResponseMessage(this.Request, format, bundle);
        }

        [HttpPost]
        [Route("ImplementationGuide")]
        [SecurableAction(SecurableNames.IMPLEMENTATIONGUIDE_EDIT)]
        public HttpResponseMessage CreateImplementationGuide(
            [FromBody] FhirImplementationGuide implementationGuide,
            [FromUri(Name = "_format")] string format = null)
        {
            throw new NotImplementedException();
        }

        [HttpPut]
        [Route("ImplementationGuide/{implementationGuideId}")]
        [SecurableAction(SecurableNames.IMPLEMENTATIONGUIDE_EDIT)]
        public HttpResponseMessage UpdateImplementationGuide(
            [FromUri] int implementationGuideId,
            [FromBody] FhirImplementationGuide implementationGuide)
        {
            throw new NotImplementedException();
        }

        [HttpDelete]
        [Route("ImplementationGuide/{implementationGuideId}")]
        [SecurableAction(SecurableNames.IMPLEMENTATIONGUIDE_EDIT)]
        public HttpResponseMessage DeleteImplementationGuide([FromUri] int implementationGuideId)
        {
            throw new NotImplementedException();
        }
    }
}