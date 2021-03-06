﻿using System;
using System.Data.Common;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Reflection;
using Moq;

using Trifolia.Shared;
using Trifolia.DB;
using Trifolia.Authorization;
using System.Collections.ObjectModel;
using System.Data.Entity.Infrastructure;

namespace Trifolia.Test
{
    [Serializable]
    public class MockObjectRepository : IObjectRepository
    {
        public const string DEFAULT_ORGANIZATION = "LCG";
        public const string DEFAULT_FHIR_CURRENT_BUILD_IG_TYPE_NAME = "FHIR Current Build";
        public const string DEFAULT_USERNAME = "admin";

        public const string DEFAULT_CDA_DOC_TYPE = "Document";
        public const string DEFAULT_CDA_SECTION_TYPE = "Section";
        public const string DEFAULT_CDA_ENTRY_TYPE = "Entry";
        public const string DEFAULT_CDA_SUBENTRY_TYPE = "Subentry";
        public const string DEFAULT_CDA_UNSPECIFIED_TYPE = "Unspecified";

        public const string ADMIN_ROLE = "admin";

        public MockObjectRepository()
        {
            this.PublishStatuses.Add(new PublishStatus()
            {
                Id = 1,
                Status = "Draft"
            });
            this.PublishStatuses.Add(new PublishStatus()
            {
                Id = 2,
                Status = "Ballot"
            });
            this.PublishStatuses.Add(new PublishStatus()
            {
                Id = 3,
                Status = "Published"
            });
            this.PublishStatuses.Add(new PublishStatus()
            {
                Id = 4,
                Status = "Deprecated"
            });
            this.PublishStatuses.Add(new PublishStatus()
            {
                Id = 5,
                Status = "Retired"
            });

            // Add the default admin role
            var adminRole = this.FindOrCreateRole(ADMIN_ROLE);
            adminRole.IsAdmin = true;

            // Add all securables to the system
            this.FindOrCreateSecurables(SecurableNames.ADMIN, SecurableNames.CODESYSTEM_EDIT, SecurableNames.CODESYSTEM_LIST, SecurableNames.EXPORT_GREEN, SecurableNames.EXPORT_SCHEMATRON, 
                SecurableNames.EXPORT_VOCAB, SecurableNames.EXPORT_WORD, SecurableNames.EXPORT_XML, SecurableNames.GREEN_MODEL, SecurableNames.IMPLEMENTATIONGUIDE_AUDIT_TRAIL, 
                SecurableNames.IMPLEMENTATIONGUIDE_EDIT, SecurableNames.IMPLEMENTATIONGUIDE_EDIT_BOOKMARKS, SecurableNames.IMPLEMENTATIONGUIDE_FILE_MANAGEMENT, SecurableNames.IMPLEMENTATIONGUIDE_FILE_VIEW, 
                SecurableNames.IMPLEMENTATIONGUIDE_LIST, SecurableNames.IMPLEMENTATIONGUIDE_NOTES, SecurableNames.IMPLEMENTATIONGUIDE_PRIMITIVES, SecurableNames.LANDING_PAGE, SecurableNames.ORGANIZATION_DETAILS, 
                SecurableNames.ORGANIZATION_LIST, SecurableNames.PUBLISH_SETTINGS, SecurableNames.REPORT_TEMPLATE_COMPLIANCE, SecurableNames.REPORT_TEMPLATE_REVIEW, SecurableNames.TEMPLATE_COPY, 
                SecurableNames.TEMPLATE_DELETE, SecurableNames.TEMPLATE_EDIT, SecurableNames.TEMPLATE_LIST, SecurableNames.TEMPLATE_MOVE, SecurableNames.TERMINOLOGY_OVERRIDE, SecurableNames.VALUESET_EDIT, 
                SecurableNames.VALUESET_LIST, SecurableNames.WEB_IG);
        }

        /// <summary>
        /// Creates the ImplementationGuideType for CDA, as well as 5 template types (Document, Section, Entry, Subentry, Unspecified)
        /// </summary>
        public void InitializeCDARepository()
        {
            ImplementationGuideType cdaType = this.FindOrCreateImplementationGuideType(Constants.IGTypeNames.CDA, Constants.IGTypeSchemaLocations.CDA, Constants.IGTypePrefixes.CDA, Constants.IGTypeNamespaces.CDA);

            this.FindOrCreateTemplateType(cdaType, DEFAULT_CDA_DOC_TYPE, "ClinicalDocument", "ClinicalDocument", 1);
            this.FindOrCreateTemplateType(cdaType, DEFAULT_CDA_SECTION_TYPE, "section", "Section", 2);
            this.FindOrCreateTemplateType(cdaType, DEFAULT_CDA_ENTRY_TYPE, "entry", "Entry", 3);
            this.FindOrCreateTemplateType(cdaType, DEFAULT_CDA_SUBENTRY_TYPE, "entry", "Entry", 4);
            this.FindOrCreateTemplateType(cdaType, DEFAULT_CDA_UNSPECIFIED_TYPE, "", "", 4);
        }

        public void InitializeFHIR2Repository()
        {
            ImplementationGuideType fhirType = this.FindOrCreateImplementationGuideType(Constants.IGTypeNames.FHIR_DSTU2, "fhir-all.xsd", "fhir", "http://hl7.org/fhir");

            this.FindOrCreateTemplateType(fhirType, "Composition", "Composition", "Composition", 1);
            this.FindOrCreateTemplateType(fhirType, "Patient", "Patient", "Patient", 2);
            this.FindOrCreateTemplateType(fhirType, "Practitioner", "Practitioner", "Practitioner", 3);
            this.FindOrCreateTemplateType(fhirType, "StructureDefinition", "StructureDefinition", "StructureDefinition", 4);
            this.FindOrCreateTemplateType(fhirType, "ImplementationGuide", "ImplementationGuide", "ImplementationGuide", 5);
            this.FindOrCreateTemplateType(fhirType, "ValueSet", "ValueSet", "ValueSet", 6);

            this.FindOrCreateImplementationGuide(fhirType, "Unowned FHIR DSTU2 Profiles");
        }

        public void InitializeFHIR3Repository()
        {
            ImplementationGuideType fhirType = this.FindOrCreateImplementationGuideType(Constants.IGTypeNames.FHIR_STU3, "fhir-all.xsd", "fhir", "http://hl7.org/fhir");

            this.FindOrCreateTemplateType(fhirType, "Account");
            this.FindOrCreateTemplateType(fhirType, "ActivityDefinition");
            this.FindOrCreateTemplateType(fhirType, "AllergyIntolerance");
            this.FindOrCreateTemplateType(fhirType, "Appointment");
            this.FindOrCreateTemplateType(fhirType, "AppointmentResponse");
            this.FindOrCreateTemplateType(fhirType, "AuditEvent");
            this.FindOrCreateTemplateType(fhirType, "Basic");
            this.FindOrCreateTemplateType(fhirType, "Binary");
            this.FindOrCreateTemplateType(fhirType, "BodySite");
            this.FindOrCreateTemplateType(fhirType, "Bundle");
            this.FindOrCreateTemplateType(fhirType, "CarePlan");
            this.FindOrCreateTemplateType(fhirType, "CareTeam");
            this.FindOrCreateTemplateType(fhirType, "Claim");
            this.FindOrCreateTemplateType(fhirType, "ClaimResponse");
            this.FindOrCreateTemplateType(fhirType, "ClinicalImpression");
            this.FindOrCreateTemplateType(fhirType, "CodeSystem");
            this.FindOrCreateTemplateType(fhirType, "Communication");
            this.FindOrCreateTemplateType(fhirType, "CommunicationRequest");
            this.FindOrCreateTemplateType(fhirType, "CompartmentDefinition");
            this.FindOrCreateTemplateType(fhirType, "Composition");
            this.FindOrCreateTemplateType(fhirType, "ConceptMap");
            this.FindOrCreateTemplateType(fhirType, "Condition");
            this.FindOrCreateTemplateType(fhirType, "Conformance");
            this.FindOrCreateTemplateType(fhirType, "Consent");
            this.FindOrCreateTemplateType(fhirType, "Contract");
            this.FindOrCreateTemplateType(fhirType, "Coverage");
            this.FindOrCreateTemplateType(fhirType, "DataElement");
            this.FindOrCreateTemplateType(fhirType, "DecisionSupportServiceModule");
            this.FindOrCreateTemplateType(fhirType, "DetectedIssue");
            this.FindOrCreateTemplateType(fhirType, "Device");
            this.FindOrCreateTemplateType(fhirType, "DeviceComponent");
            this.FindOrCreateTemplateType(fhirType, "DeviceMetric");
            this.FindOrCreateTemplateType(fhirType, "DeviceUseRequest");
            this.FindOrCreateTemplateType(fhirType, "DeviceUseStatement");
            this.FindOrCreateTemplateType(fhirType, "DiagnosticRequest");
            this.FindOrCreateTemplateType(fhirType, "DiagnosticReport");
            this.FindOrCreateTemplateType(fhirType, "DocumentManifest");
            this.FindOrCreateTemplateType(fhirType, "DocumentReference");
            this.FindOrCreateTemplateType(fhirType, "EligibilityRequest");
            this.FindOrCreateTemplateType(fhirType, "EligibilityResponse");
            this.FindOrCreateTemplateType(fhirType, "Encounter");
            this.FindOrCreateTemplateType(fhirType, "Endpoint");
            this.FindOrCreateTemplateType(fhirType, "EnrollmentRequest");
            this.FindOrCreateTemplateType(fhirType, "EnrollmentResponse");
            this.FindOrCreateTemplateType(fhirType, "EpisodeOfCare");
            this.FindOrCreateTemplateType(fhirType, "ExpansionProfile");
            this.FindOrCreateTemplateType(fhirType, "ExplanationOfBenefit");
            this.FindOrCreateTemplateType(fhirType, "Extension");
            this.FindOrCreateTemplateType(fhirType, "FamilyMemberHistory");
            this.FindOrCreateTemplateType(fhirType, "Flag");
            this.FindOrCreateTemplateType(fhirType, "Goal");
            this.FindOrCreateTemplateType(fhirType, "Group");
            this.FindOrCreateTemplateType(fhirType, "GuidanceResponse");
            this.FindOrCreateTemplateType(fhirType, "HealthcareService");
            this.FindOrCreateTemplateType(fhirType, "ImagingManifest");
            this.FindOrCreateTemplateType(fhirType, "ImagingStudy");
            this.FindOrCreateTemplateType(fhirType, "Immunization");
            this.FindOrCreateTemplateType(fhirType, "ImmunizationRecommendation");
            this.FindOrCreateTemplateType(fhirType, "ImplementationGuide");
            this.FindOrCreateTemplateType(fhirType, "Library");
            this.FindOrCreateTemplateType(fhirType, "Linkage");
            this.FindOrCreateTemplateType(fhirType, "List");
            this.FindOrCreateTemplateType(fhirType, "Location");
            this.FindOrCreateTemplateType(fhirType, "Measure");
            this.FindOrCreateTemplateType(fhirType, "MeasureReport");
            this.FindOrCreateTemplateType(fhirType, "Media");
            this.FindOrCreateTemplateType(fhirType, "Medication");
            this.FindOrCreateTemplateType(fhirType, "MedicationAdministration");
            this.FindOrCreateTemplateType(fhirType, "MedicationDispense");
            this.FindOrCreateTemplateType(fhirType, "MedicationOrder");
            this.FindOrCreateTemplateType(fhirType, "MedicationStatement");
            this.FindOrCreateTemplateType(fhirType, "MessageHeader");
            this.FindOrCreateTemplateType(fhirType, "NamingSystem");
            this.FindOrCreateTemplateType(fhirType, "NutritionRequest");
            this.FindOrCreateTemplateType(fhirType, "Observation");
            this.FindOrCreateTemplateType(fhirType, "OperationDefinition");
            this.FindOrCreateTemplateType(fhirType, "OperationOutcome");
            this.FindOrCreateTemplateType(fhirType, "Organization");
            this.FindOrCreateTemplateType(fhirType, "Parameters");
            this.FindOrCreateTemplateType(fhirType, "Patient");
            this.FindOrCreateTemplateType(fhirType, "PaymentNotice");
            this.FindOrCreateTemplateType(fhirType, "PaymentReconciliation");
            this.FindOrCreateTemplateType(fhirType, "Person");
            this.FindOrCreateTemplateType(fhirType, "PlanDefinition");
            this.FindOrCreateTemplateType(fhirType, "Practitioner");
            this.FindOrCreateTemplateType(fhirType, "PractitionerRole");
            this.FindOrCreateTemplateType(fhirType, "Procedure");
            this.FindOrCreateTemplateType(fhirType, "ProcedureRequest");
            this.FindOrCreateTemplateType(fhirType, "ProcessRequest");
            this.FindOrCreateTemplateType(fhirType, "ProcessResponse");
            this.FindOrCreateTemplateType(fhirType, "Provenance");
            this.FindOrCreateTemplateType(fhirType, "Questionnaire");
            this.FindOrCreateTemplateType(fhirType, "QuestionnaireResponse");
            this.FindOrCreateTemplateType(fhirType, "ReferralRequest");
            this.FindOrCreateTemplateType(fhirType, "RelatedPerson");
            this.FindOrCreateTemplateType(fhirType, "RiskAssessment");
            this.FindOrCreateTemplateType(fhirType, "Schedule");
            this.FindOrCreateTemplateType(fhirType, "SearchParameter");
            this.FindOrCreateTemplateType(fhirType, "Sequence");
            this.FindOrCreateTemplateType(fhirType, "Slot");
            this.FindOrCreateTemplateType(fhirType, "Specimen");
            this.FindOrCreateTemplateType(fhirType, "StructureDefinition");
            this.FindOrCreateTemplateType(fhirType, "StructureMap");
            this.FindOrCreateTemplateType(fhirType, "Subscription");
            this.FindOrCreateTemplateType(fhirType, "Substance");
            this.FindOrCreateTemplateType(fhirType, "SupplyDelivery");
            this.FindOrCreateTemplateType(fhirType, "SupplyRequest");
            this.FindOrCreateTemplateType(fhirType, "Task");
            this.FindOrCreateTemplateType(fhirType, "TestScript");
            this.FindOrCreateTemplateType(fhirType, "ValueSet");
            this.FindOrCreateTemplateType(fhirType, "VisionPrescription");

            this.FindOrCreateImplementationGuide(fhirType, "Unowned FHIR STU3 Profiles");
        }

        public void InitializeFHIRLatestRepository()
        {
            ImplementationGuideType fhirType = this.FindOrCreateImplementationGuideType(DEFAULT_FHIR_CURRENT_BUILD_IG_TYPE_NAME, "fhir-all.xsd", "fhir", "http://hl7.org/fhir");

            this.FindOrCreateTemplateType(fhirType, "Account");
            this.FindOrCreateTemplateType(fhirType, "ActivityDefinition");
            this.FindOrCreateTemplateType(fhirType, "AllergyIntolerance");
            this.FindOrCreateTemplateType(fhirType, "AdverseEvent");
            this.FindOrCreateTemplateType(fhirType, "Appointment");
            this.FindOrCreateTemplateType(fhirType, "AppointmentResponse");
            this.FindOrCreateTemplateType(fhirType, "AuditEvent");
            this.FindOrCreateTemplateType(fhirType, "Basic");
            this.FindOrCreateTemplateType(fhirType, "Binary");
            this.FindOrCreateTemplateType(fhirType, "BodyStructure");
            this.FindOrCreateTemplateType(fhirType, "Bundle");
            this.FindOrCreateTemplateType(fhirType, "CapabilityStatement");
            this.FindOrCreateTemplateType(fhirType, "CarePlan");
            this.FindOrCreateTemplateType(fhirType, "CareTeam");
            this.FindOrCreateTemplateType(fhirType, "ChargeItem");
            this.FindOrCreateTemplateType(fhirType, "Claim");
            this.FindOrCreateTemplateType(fhirType, "ClaimResponse");
            this.FindOrCreateTemplateType(fhirType, "ClinicalImpression");
            this.FindOrCreateTemplateType(fhirType, "CodeSystem");
            this.FindOrCreateTemplateType(fhirType, "Communication");
            this.FindOrCreateTemplateType(fhirType, "CommunicationRequest");
            this.FindOrCreateTemplateType(fhirType, "CompartmentDefinition");
            this.FindOrCreateTemplateType(fhirType, "Composition");
            this.FindOrCreateTemplateType(fhirType, "ConceptMap");
            this.FindOrCreateTemplateType(fhirType, "Condition");
            this.FindOrCreateTemplateType(fhirType, "Consent");
            this.FindOrCreateTemplateType(fhirType, "Contract");
            this.FindOrCreateTemplateType(fhirType, "Coverage");
            this.FindOrCreateTemplateType(fhirType, "DetectedIssue");
            this.FindOrCreateTemplateType(fhirType, "Device");
            this.FindOrCreateTemplateType(fhirType, "DeviceComponent");
            this.FindOrCreateTemplateType(fhirType, "DeviceMetric");
            this.FindOrCreateTemplateType(fhirType, "DeviceRequest");
            this.FindOrCreateTemplateType(fhirType, "DeviceUseStatement");
            this.FindOrCreateTemplateType(fhirType, "DiagnosticReport");
            this.FindOrCreateTemplateType(fhirType, "DocumentManifest");
            this.FindOrCreateTemplateType(fhirType, "DocumentReference");
            this.FindOrCreateTemplateType(fhirType, "EligibilityRequest");
            this.FindOrCreateTemplateType(fhirType, "EligibilityResponse");
            this.FindOrCreateTemplateType(fhirType, "Encounter");
            this.FindOrCreateTemplateType(fhirType, "Endpoint");
            this.FindOrCreateTemplateType(fhirType, "EnrollmentRequest");
            this.FindOrCreateTemplateType(fhirType, "EnrollmentResponse");
            this.FindOrCreateTemplateType(fhirType, "EpisodeOfCare");
            this.FindOrCreateTemplateType(fhirType, "EventDefinition");
            this.FindOrCreateTemplateType(fhirType, "ExpansionProfile");
            this.FindOrCreateTemplateType(fhirType, "ExplanationOfBenefit");
            this.FindOrCreateTemplateType(fhirType, "FamilyMemberHistory");
            this.FindOrCreateTemplateType(fhirType, "Flag");
            this.FindOrCreateTemplateType(fhirType, "Goal");
            this.FindOrCreateTemplateType(fhirType, "GraphDefinition");
            this.FindOrCreateTemplateType(fhirType, "Group");
            this.FindOrCreateTemplateType(fhirType, "GuidanceResponse");
            this.FindOrCreateTemplateType(fhirType, "HealthcareService");
            this.FindOrCreateTemplateType(fhirType, "ImagingManifest");
            this.FindOrCreateTemplateType(fhirType, "ImagingStudy");
            this.FindOrCreateTemplateType(fhirType, "Immunization");
            this.FindOrCreateTemplateType(fhirType, "ImmunizationRecommendation");
            this.FindOrCreateTemplateType(fhirType, "ImplementationGuide");
            this.FindOrCreateTemplateType(fhirType, "Library");
            this.FindOrCreateTemplateType(fhirType, "Linkage");
            this.FindOrCreateTemplateType(fhirType, "List");
            this.FindOrCreateTemplateType(fhirType, "Location");
            this.FindOrCreateTemplateType(fhirType, "Measure");
            this.FindOrCreateTemplateType(fhirType, "MeasureReport");
            this.FindOrCreateTemplateType(fhirType, "Media");
            this.FindOrCreateTemplateType(fhirType, "Medication");
            this.FindOrCreateTemplateType(fhirType, "MedicationAdministration");
            this.FindOrCreateTemplateType(fhirType, "MedicationDispense");
            this.FindOrCreateTemplateType(fhirType, "MedicationRequest");
            this.FindOrCreateTemplateType(fhirType, "MedicationStatement");
            this.FindOrCreateTemplateType(fhirType, "MessageDefinition");
            this.FindOrCreateTemplateType(fhirType, "MessageHeader");
            this.FindOrCreateTemplateType(fhirType, "NamingSystem");
            this.FindOrCreateTemplateType(fhirType, "NutritionOrder");
            this.FindOrCreateTemplateType(fhirType, "Observation");
            this.FindOrCreateTemplateType(fhirType, "OperationDefinition");
            this.FindOrCreateTemplateType(fhirType, "OperationOutcome");
            this.FindOrCreateTemplateType(fhirType, "Organization");
            this.FindOrCreateTemplateType(fhirType, "Parameters");
            this.FindOrCreateTemplateType(fhirType, "Patient");
            this.FindOrCreateTemplateType(fhirType, "PaymentNotice");
            this.FindOrCreateTemplateType(fhirType, "PaymentReconciliation");
            this.FindOrCreateTemplateType(fhirType, "Person");
            this.FindOrCreateTemplateType(fhirType, "PlanDefinition");
            this.FindOrCreateTemplateType(fhirType, "Practitioner");
            this.FindOrCreateTemplateType(fhirType, "PractitionerRole");
            this.FindOrCreateTemplateType(fhirType, "Procedure");
            this.FindOrCreateTemplateType(fhirType, "ProcedureRequest");
            this.FindOrCreateTemplateType(fhirType, "ProcessRequest");
            this.FindOrCreateTemplateType(fhirType, "ProcessResponse");
            this.FindOrCreateTemplateType(fhirType, "Provenance");
            this.FindOrCreateTemplateType(fhirType, "Questionnaire");
            this.FindOrCreateTemplateType(fhirType, "QuestionnaireResponse");
            this.FindOrCreateTemplateType(fhirType, "RelatedPerson");
            this.FindOrCreateTemplateType(fhirType, "RequestGroup");
            this.FindOrCreateTemplateType(fhirType, "ResearchStudy");
            this.FindOrCreateTemplateType(fhirType, "ResearchSubject");
            this.FindOrCreateTemplateType(fhirType, "RiskAssessment");
            this.FindOrCreateTemplateType(fhirType, "Schedule");
            this.FindOrCreateTemplateType(fhirType, "SearchParameter");
            this.FindOrCreateTemplateType(fhirType, "Sequence");
            this.FindOrCreateTemplateType(fhirType, "ServiceDefinition");
            this.FindOrCreateTemplateType(fhirType, "Slot");
            this.FindOrCreateTemplateType(fhirType, "Specimen");
            this.FindOrCreateTemplateType(fhirType, "StructureDefinition");
            this.FindOrCreateTemplateType(fhirType, "StructureMap");
            this.FindOrCreateTemplateType(fhirType, "Subscription");
            this.FindOrCreateTemplateType(fhirType, "Substance");
            this.FindOrCreateTemplateType(fhirType, "SupplyDelivery");
            this.FindOrCreateTemplateType(fhirType, "SupplyRequest");
            this.FindOrCreateTemplateType(fhirType, "Task");
            this.FindOrCreateTemplateType(fhirType, "TestScript");
            this.FindOrCreateTemplateType(fhirType, "TestReport");
            this.FindOrCreateTemplateType(fhirType, "ValueSet");
            this.FindOrCreateTemplateType(fhirType, "VisionPrescription");

            this.FindOrCreateImplementationGuide(fhirType, "Unowned FHIR Current Build Profiles");
        }

        public void InitializeLCG()
        {
            var org = this.FindOrCreateOrganization(DEFAULT_ORGANIZATION);
            this.FindOrCreateUser(DEFAULT_USERNAME);
            this.AssociateUserWithRole(DEFAULT_USERNAME, ADMIN_ROLE);
        }

        public void InitializeLCGAndLogin()
        {
            this.InitializeLCG();
            this.Login();
        }

        /// <summary>
        /// Login for the user. This results in associating the thread with an identity.
        /// This needs to be called for each thread that will be running against the mockRepo.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="organization"></param>
        public void Login(string username = null)
        {
            if (username == null)
                username = DEFAULT_USERNAME;

            Helper.AuthLogin(this, username);
        }

        #region IObjectRepository Collections

        public void AuditChanges(string auditUserName, string auditIP)
        {

        }

        private static DbSet<T> CreateMockDbSet<T>() where T: class
        {
            var data = new List<T>();
            var dataQueryable = data.AsQueryable();
            var mockSet = new Mock<DbSet<T>>();
            mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(dataQueryable.Provider);
            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(dataQueryable.Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(dataQueryable.ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => dataQueryable.GetEnumerator());
            mockSet.Setup(m => m.AsNoTracking()).Returns(() => mockSet.Object);
            mockSet.Setup(d => d.Remove(It.IsAny<T>())).Callback<T>((s) => data.Remove(s));
            mockSet.Setup(d => d.Include(It.IsAny<string>())).Returns(mockSet.Object);
            
            mockSet.Setup(d => d.Add(It.IsAny<T>())).Callback<T>((s) =>
            {
                Template template = s as Template;
                TemplateConstraint constraint = s as TemplateConstraint;
                ImplementationGuide ig = s as ImplementationGuide;
                ValueSet vs = s as ValueSet;
                ValueSetMember vsm = s as ValueSetMember;
                TemplateConstraintReference constraintRef = s as TemplateConstraintReference;

                if (constraintRef != null)
                {
                    if (constraintRef.Id == 0)
                        constraintRef.Id = data.Select(y => (y as TemplateConstraintReference).Id).DefaultIfEmpty(0).Max() + 1;

                    if (constraintRef.Constraint != null && !constraintRef.Constraint.References.Contains(constraintRef))
                        constraintRef.Constraint.References.Add(constraintRef);

                    if (constraintRef.Constraint != null && constraintRef.TemplateConstraintId != constraintRef.Constraint.Id)
                        constraintRef.TemplateConstraintId = constraintRef.Constraint.Id;
                }

                if (template != null)
                {
                    if (template.Id == 0)
                        template.Id = data.Select(y => (y as Template).Id).DefaultIfEmpty(0).Max() + 1;

                    if (template.OwningImplementationGuide != null && !template.OwningImplementationGuide.ChildTemplates.Contains(template))
                        template.OwningImplementationGuide.ChildTemplates.Add(template);

                    if (template.ImplementationGuideType != null && !template.ImplementationGuideType.Templates.Contains(template))
                        template.ImplementationGuideType.Templates.Add(template);

                    if (template.Status != null && !template.Status.Templates.Contains(template))
                        template.Status.Templates.Add(template);

                    if (template.ImpliedTemplate != null && !template.ImpliedTemplate.ImplyingTemplates.Contains(template))
                        template.ImpliedTemplate.ImplyingTemplates.Add(template);

                    if (template.PreviousVersion != null && !template.PreviousVersion.NextVersions.Contains(template))
                        template.PreviousVersion.NextVersions.Add(template);

                    if (template.Author != null && !template.Author.Templates.Contains(template))
                        template.Author.Templates.Add(template);
                }
                
                if (constraint != null)
                {
                    if (constraint.Id == 0)
                        constraint.Id = data.Select(y => (y as TemplateConstraint).Id).DefaultIfEmpty(0).Max() + 1;

                    if (constraint.CodeSystem != null && !constraint.CodeSystem.Constraints.Contains(constraint))
                        constraint.CodeSystem.Constraints.Add(constraint);

                    if (constraint.ParentConstraint != null && !constraint.ParentConstraint.ChildConstraints.Contains(constraint))
                        constraint.ParentConstraint.ChildConstraints.Add(constraint);

                    if (constraint.Template != null && !constraint.Template.ChildConstraints.Contains(constraint))
                        constraint.Template.ChildConstraints.Add(constraint);

                    if (constraint.ValueSet != null && !constraint.ValueSet.Constraints.Contains(constraint))
                        constraint.ValueSet.Constraints.Add(constraint);
                }

                if (ig != null)
                {
                    if (ig.Id == 0)
                        ig.Id = data.Select(y => (y as ImplementationGuide).Id).DefaultIfEmpty(0).Max() + 1;

                    if (ig.AccessManager != null && !ig.AccessManager.AccessManagerImplemntationGuides.Contains(ig))
                        ig.AccessManager.AccessManagerImplemntationGuides.Add(ig);

                    if (ig.ImplementationGuideType != null && !ig.ImplementationGuideType.ImplementationGuides.Contains(ig))
                        ig.ImplementationGuideType.ImplementationGuides.Add(ig);

                    if (ig.Organization != null && !ig.Organization.ImplementationGuides.Contains(ig))
                        ig.Organization.ImplementationGuides.Add(ig);

                    if (ig.PreviousVersion != null && !ig.PreviousVersion.NextVersions.Contains(ig))
                        ig.PreviousVersion.NextVersions.Add(ig);

                    if (ig.Organization != null && !ig.Organization.ImplementationGuides.Contains(ig))
                        ig.Organization.ImplementationGuides.Add(ig);

                    if (ig.PublishStatus != null && !ig.PublishStatus.ImplementationGuides.Contains(ig))
                        ig.PublishStatus.ImplementationGuides.Add(ig);
                }
                
                if (vs != null)
                {
                    if (vs.Id == 0)
                        vs.Id = data.Select(y => (y as ValueSet).Id).DefaultIfEmpty(0).Max() + 1;
                }

                if (vsm != null)
                {
                    if (vsm.Id == 0)
                        vsm.Id = data.Select(y => (y as ValueSetMember).Id).DefaultIfEmpty(0).Max() + 1;

                    if (vsm.CodeSystem != null && !vsm.CodeSystem.Members.Contains(vsm))
                        vsm.CodeSystem.Members.Add(vsm);

                    if (vsm.ValueSet != null && !vsm.ValueSet.Members.Contains(vsm))
                        vsm.ValueSet.Members.Add(vsm);
                }

                data.Add(s);
            });
            mockSet.Setup(d => d.AddRange(It.IsAny<IEnumerable<T>>())).Callback<IEnumerable<T>>((s) => 
            {
                foreach (var a in s)
                    mockSet.Object.Add(a);
            });

            return mockSet.Object;
        }

        DbSet<AuditEntry> auditEntries = null;
        DbSet<Template> templates = null;
        DbSet<CodeSystem> codeSystems = null;
        DbSet<GreenConstraint> greenConstraints = null;
        DbSet<GreenTemplate> greenTemplates = null;
        DbSet<ImplementationGuide> implementationGuides = null;
        DbSet<ImplementationGuideSetting> implementationGuideSettings = null;
        DbSet<TemplateConstraint> constraints = null;
        DbSet<TemplateType> templateTypes = null;
        DbSet<ValueSet> valuesets = null;
        DbSet<ValueSetIdentifier> valueSetIdentifiers = null;
        DbSet<ValueSetMember> valuesetMembers = null;
        DbSet<ImplementationGuideTemplateType> implementationGuideTemplateTypes = null;
        DbSet<ImplementationGuideTypeDataType> dataTypes = null;
        DbSet<ImplementationGuideType> implementationGuideTypes = null;
        DbSet<ImplementationGuideFile> implementationGuideFiles = null;
        DbSet<ImplementationGuideFileData> implementationGuideFileDatas = null;
        DbSet<Organization> organizations = null;
        DbSet<ImplementationGuideSchematronPattern> implementationGuideSchematronPatterns = null;
        DbSet<PublishStatus> publishStatuses = null;
        DbSet<Role> roles = null;
        DbSet<AppSecurable> appSecurables = null;
        DbSet<RoleAppSecurable> roleAppSecurables = null;
        DbSet<UserRole> userRoles = null;
        DbSet<User> users = null;
        DbSet<RoleRestriction> roleRestrictions = null;
        DbSet<Group> groups = null;
        DbSet<UserGroup> userGroups = null;
        DbSet<GroupManager> groupManagers = null;
        DbSet<ImplementationGuidePermission> implementationGuidePermissions = null;
        DbSet<TemplateConstraintSample> templateConstraintSamples = null;
        DbSet<TemplateSample> templateSamples = null;
        DbSet<ImplementationGuideSection> implementationGuideSections = null;
        DbSet<TemplateExtension> templateExtensions = null;
        DbSet<ImplementationGuideAccessRequest> implementationGuideAccessRequests = null;
        DbSet<TemplateConstraintReference> templateConstraintReferences = null;

        public DbSet<TemplateConstraintReference> TemplateConstraintReferences
        {
            get
            {
                if (this.templateConstraintReferences == null)
                    this.templateConstraintReferences = CreateMockDbSet<TemplateConstraintReference>();

                return this.templateConstraintReferences;
            }
        }

        public DbSet<AuditEntry> AuditEntries
        {
            get
            {
                if (this.auditEntries == null)
                    this.auditEntries = CreateMockDbSet<AuditEntry>();

                return auditEntries;
            }
        }

        public DbSet<CodeSystem> CodeSystems
        {
            get
            {
                if (this.codeSystems == null)
                    this.codeSystems = CreateMockDbSet<CodeSystem>();

                return codeSystems;
            }
        }

        public DbSet<GreenConstraint> GreenConstraints
        {
            get
            {
                if (this.greenConstraints == null)
                    this.greenConstraints = CreateMockDbSet<GreenConstraint>();

                return greenConstraints;
            }
        }

        public DbSet<GreenTemplate> GreenTemplates
        {
            get
            {
                if (this.greenTemplates == null)
                    this.greenTemplates = CreateMockDbSet<GreenTemplate>();

                return greenTemplates;
            }
        }

        public DbSet<ImplementationGuide> ImplementationGuides
        {
            get
            {
                if (this.implementationGuides == null)
                    this.implementationGuides = CreateMockDbSet<ImplementationGuide>();

                return implementationGuides;
            }
        }

        public DbSet<ImplementationGuideSetting> ImplementationGuideSettings
        {
            get
            {
                if (this.implementationGuideSettings == null)
                    this.implementationGuideSettings = CreateMockDbSet<ImplementationGuideSetting>();

                return implementationGuideSettings;
            }
        }

        public DbSet<Template> Templates
        {
            get
            {
                if (this.templates == null)
                    this.templates = CreateMockDbSet<Template>();

                return templates;
            }
        }

        public DbSet<TemplateConstraint> TemplateConstraints
        {
            get
            {
                if (this.constraints == null)
                    this.constraints = CreateMockDbSet<TemplateConstraint>();

                return constraints;
            }
        }

        public DbSet<TemplateType> TemplateTypes
        {
            get
            {
                if (this.templateTypes == null)
                    this.templateTypes = CreateMockDbSet<TemplateType>();

                return templateTypes;
            }
        }

        public DbSet<ValueSet> ValueSets
        {
            get
            {
                if (this.valuesets == null)
                    this.valuesets = CreateMockDbSet<ValueSet>();

                return valuesets;
            }
        }

        public DbSet<ValueSetIdentifier> ValueSetIdentifiers
        {
            get
            {
                if (this.valueSetIdentifiers == null)
                    this.valueSetIdentifiers = CreateMockDbSet<ValueSetIdentifier>();

                return this.valueSetIdentifiers;
            }
        }

        public DbSet<ValueSetMember> ValueSetMembers
        {
            get
            {
                if (this.valuesetMembers == null)
                    this.valuesetMembers = CreateMockDbSet<ValueSetMember>();

                return valuesetMembers;
            }
        }

        public DbSet<ImplementationGuideAccessRequest> ImplementationGuideAccessRequests
        {
            get
            {
                if (this.implementationGuideAccessRequests == null)
                    this.implementationGuideAccessRequests = CreateMockDbSet<ImplementationGuideAccessRequest>();

                return this.implementationGuideAccessRequests;
            }
        }

        public DbSet<ImplementationGuideTemplateType> ImplementationGuideTemplateTypes
        {
            get
            {
                if (this.implementationGuideTemplateTypes == null)
                    this.implementationGuideTemplateTypes = CreateMockDbSet<ImplementationGuideTemplateType>();

                return implementationGuideTemplateTypes;
            }
        }

        public DbSet<ImplementationGuideTypeDataType> ImplementationGuideTypeDataTypes
        {
            get
            {
                if (this.dataTypes == null)
                    this.dataTypes = CreateMockDbSet<ImplementationGuideTypeDataType>();

                return dataTypes;
            }
        }

        public DbSet<ImplementationGuideType> ImplementationGuideTypes
        {
            get
            {
                if (this.implementationGuideTypes == null)
                    this.implementationGuideTypes = CreateMockDbSet<ImplementationGuideType>();

                return implementationGuideTypes;
            }
        }

        public DbSet<ImplementationGuideFile> ImplementationGuideFiles
        {
            get
            {
                if (this.implementationGuideFiles == null)
                    this.implementationGuideFiles = CreateMockDbSet<ImplementationGuideFile>();

                return implementationGuideFiles;
            }
        }

        public DbSet<ImplementationGuideFileData> ImplementationGuideFileDatas
        {
            get
            {
                if (this.implementationGuideFileDatas == null)
                    this.implementationGuideFileDatas = CreateMockDbSet<ImplementationGuideFileData>();

                return implementationGuideFileDatas;
            }
        }

        public DbSet<Organization> Organizations
        {
            get
            {
                if (this.organizations == null)
                    this.organizations = CreateMockDbSet<Organization>();
                
                return organizations;
            }
        }

        public DbSet<ImplementationGuideSchematronPattern> ImplementationGuideSchematronPatterns
        {
            get
            {
                if (this.implementationGuideSchematronPatterns == null)
                    this.implementationGuideSchematronPatterns = CreateMockDbSet<ImplementationGuideSchematronPattern>();

                return implementationGuideSchematronPatterns;
            }
        }

        public DbSet<PublishStatus> PublishStatuses
        {
            get
            {
                if (this.publishStatuses == null)
                    this.publishStatuses = CreateMockDbSet<PublishStatus>();

                return publishStatuses;
            }
        }

        public DbSet<Role> Roles
        {
            get
            {
                if (this.roles == null)
                    this.roles = CreateMockDbSet<Role>();

                return roles;
            }
        }

        public DbSet<AppSecurable> AppSecurables
        {
            get
            {
                if (this.appSecurables == null)
                    this.appSecurables = CreateMockDbSet<AppSecurable>();

                return appSecurables;
            }
        }

        public DbSet<RoleAppSecurable> RoleAppSecurables
        {
            get
            {
                if (this.roleAppSecurables == null)
                    this.roleAppSecurables = CreateMockDbSet<RoleAppSecurable>();

                return roleAppSecurables;
            }
        }

        public DbSet<UserRole> UserRoles
        {
            get
            {
                if (this.userRoles == null)
                    this.userRoles = CreateMockDbSet<UserRole>();

                return userRoles;
            }
        }

        public DbSet<User> Users
        {
            get
            {
                if (this.users == null)
                    this.users = CreateMockDbSet<User>();

                return users;
            }
        }

        public DbSet<RoleRestriction> RoleRestrictions
        {
            get
            {
                if (this.roleRestrictions == null)
                    this.roleRestrictions = CreateMockDbSet<RoleRestriction>();

                return roleRestrictions;
            }
        }

        public DbSet<Group> Groups
        {
            get
            {
                if (this.groups == null)
                    this.groups = CreateMockDbSet<Group>();

                return groups;
            }
        }

        public DbSet<UserGroup> UserGroups
        {
            get
            {
                if (this.userGroups == null)
                    this.userGroups = CreateMockDbSet<UserGroup>();

                return userGroups;
            }
        }

        public DbSet<GroupManager> GroupManagers
        {
            get
            {
                if (this.groupManagers == null)
                    this.groupManagers = CreateMockDbSet<GroupManager>();

                return this.groupManagers;
            }
        }

        public DbSet<ImplementationGuidePermission> ImplementationGuidePermissions
        {
            get
            {
                if (this.implementationGuidePermissions == null)
                    this.implementationGuidePermissions = CreateMockDbSet<ImplementationGuidePermission>();

                return implementationGuidePermissions;
            }
        }

        public DbSet<TemplateConstraintSample> TemplateConstraintSamples
        {
            get
            {
                if (this.templateConstraintSamples == null)
                    this.templateConstraintSamples = CreateMockDbSet<TemplateConstraintSample>();

                return templateConstraintSamples;
            }
        }

        public DbSet<TemplateSample> TemplateSamples
        {
            get
            {
                if (this.templateSamples == null)
                    this.templateSamples = CreateMockDbSet<TemplateSample>();

                return templateSamples;
            }
        }

        public DbSet<ImplementationGuideSection> ImplementationGuideSections
        {
            get
            {
                if (this.implementationGuideSections == null)
                    this.implementationGuideSections = CreateMockDbSet<ImplementationGuideSection>();

                return implementationGuideSections;
            }
        }

        public DbSet<TemplateExtension> TemplateExtensions
        {
            get
            {
                if (this.templateExtensions == null)
                    this.templateExtensions = CreateMockDbSet<TemplateExtension>();

                return templateExtensions;
            }
        }

        #endregion

        #region IObjectRepository View Collections

        public DbSet<ViewCodeSystemUsage> ViewCodeSystemUsages
        {
            get
            {
                return null;
            }
        }

        public DbSet<ViewTemplateRelationship> ViewTemplateRelationships
        {
            get
            {
                var results = (from tcr in this.TemplateConstraintReferences
                               join tc in this.TemplateConstraints on tcr.TemplateConstraintId equals tc.Id
                               join pt in this.Templates on tc.TemplateId equals pt.Id
                               join ct in this.Templates on tcr.ReferenceIdentifier equals ct.Oid
                               where tcr.ReferenceType == ConstraintReferenceTypes.Template
                               select new ViewTemplateRelationship()
                               {
                                   ParentTemplateBookmark = pt.Bookmark,
                                   ParentTemplateId = pt.Id,
                                   ParentTemplateIdentifier = pt.Oid,
                                   ParentTemplateName = pt.Name,
                                   ChildTemplateBookmark = ct.Bookmark,
                                   ChildTemplateId = ct.Id,
                                   ChildTemplateIdentifier = ct.Oid,
                                   ChildTemplateName = ct.Name,
                                   Required = this.IsConstraintRequired(tc.Id)
                               });

                var mockDbSet = CreateMockDbSet<ViewTemplateRelationship>();
                mockDbSet.AddRange(results);
                return mockDbSet;
            }
        }

        private bool IsConstraintRequired(int templateConstraintId)
        {
            int? currentTemplateConstraintId = templateConstraintId;
            bool isRequired = false;

            while (currentTemplateConstraintId != null)
            {
                TemplateConstraint currentTemplateConstraint = this.TemplateConstraints.Single(y => y.Id == currentTemplateConstraintId);
                isRequired = currentTemplateConstraint.Conformance == "SHALL" || currentTemplateConstraint.Conformance == "SHALL NOT";

                if (!isRequired)
                    break;
                else
                    currentTemplateConstraintId = currentTemplateConstraint.ParentConstraintId;
            }

            return isRequired;
        }

        public DbSet<ViewImplementationGuideCodeSystem> ViewImplementationGuideCodeSystems
        {
            get
            {
                var results = (from t in this.Templates
                               join tc in this.TemplateConstraints on t.Id equals tc.TemplateId
                               join cs in this.CodeSystems on tc.CodeSystemId equals cs.Id
                               select new ViewImplementationGuideCodeSystem()
                               {
                                   CodeSystemId = cs.Id,
                                   Description = cs.Description,
                                   Identifier = cs.Oid,
                                   ImplementationGuideId = t.OwningImplementationGuideId,
                                   Name = cs.Name
                               }).Distinct()
                        .Union(from t in this.Templates
                               join tc in this.TemplateConstraints on t.Id equals tc.TemplateId
                               join vsm in this.ValueSetMembers on tc.ValueSetId equals vsm.ValueSetId
                               join cs in this.CodeSystems on vsm.CodeSystemId equals cs.Id
                               select new ViewImplementationGuideCodeSystem()
                               {
                                   CodeSystemId = cs.Id,
                                   Description = cs.Description,
                                   Identifier = cs.Oid,
                                   ImplementationGuideId = t.OwningImplementationGuideId,
                                   Name = cs.Name
                               })
                               .Distinct();

                var mockDbSet = CreateMockDbSet<ViewImplementationGuideCodeSystem>();
                mockDbSet.AddRange(results);
                return mockDbSet;
            }
        }

        public DbSet<ViewIGAuditTrail> ViewIGAuditTrails
        {
            get
            {
                return null;
            }
        }

        public DbSet<ViewTemplatePermission> ViewTemplatePermissions
        {
            get
            {
                var results = (from ig in this.ImplementationGuides
                               join igp in this.ImplementationGuidePermissions on ig.Id equals igp.ImplementationGuideId
                               join t in this.Templates on ig.Id equals t.OwningImplementationGuideId
                               join u in this.Users on igp.UserId equals u.Id
                               select new ViewTemplatePermission()
                               {
                                   Permission = igp.Permission,
                                   TemplateId = t.Id,
                                   UserId = u.Id
                               })
                               .Union(from ig in this.ImplementationGuides
                                      join igp in this.ImplementationGuidePermissions on ig.Id equals igp.ImplementationGuideId
                                      join t in this.Templates on ig.Id equals t.OwningImplementationGuideId
                                      join ug in this.UserGroups on igp.GroupId equals ug.GroupId
                                      select new ViewTemplatePermission()
                                      {
                                          Permission = igp.Permission,
                                          TemplateId = t.Id,
                                          UserId = ug.UserId
                                      });

                var mockDbSet = CreateMockDbSet<ViewTemplatePermission>();
                mockDbSet.AddRange(results);
                return mockDbSet;
            }
        }

        public DbSet<ViewImplementationGuidePermission> ViewImplementationGuidePermissions
        {
            get
            {
                var results = (from igp in this.ImplementationGuidePermissions
                               join u in this.Users on igp.UserId equals u.Id
                               select new ViewImplementationGuidePermission()
                               {
                                   Permission = igp.Permission,
                                   ImplementationGuideId = igp.ImplementationGuideId,
                                   UserId = u.Id
                               })
                               .Union(from igp in this.ImplementationGuidePermissions
                                      join ug in this.UserGroups on igp.GroupId equals ug.GroupId
                                      select new ViewImplementationGuidePermission()
                                      {
                                          Permission = igp.Permission,
                                          ImplementationGuideId = igp.ImplementationGuideId,
                                          UserId = ug.UserId
                                      });

                var mockDbSet = CreateMockDbSet<ViewImplementationGuidePermission>();
                mockDbSet.AddRange(results);
                return mockDbSet;
            }
        }

        public DbSet<ViewImplementationGuideTemplate> ViewImplementationGuideTemplates
        {
            get
            {
                var results = (from ig in this.ImplementationGuides
                               join t in this.Templates on ig.Id equals t.OwningImplementationGuideId
                               select new ViewImplementationGuideTemplate()
                               {
                                   ImplementationGuideId = ig.Id,
                                   TemplateId = t.Id
                               })
                               .Union(
                               from ig in this.ImplementationGuides
                               join t in this.Templates on ig.PreviousVersionImplementationGuideId equals t.OwningImplementationGuideId
                               where this.Templates.Count(y => y.PreviousVersionTemplateId == t.Id) == 0
                               select new ViewImplementationGuideTemplate()
                               {
                                   ImplementationGuideId = ig.Id,
                                   TemplateId = t.Id
                               });

                var mockDbSet = CreateMockDbSet<ViewImplementationGuideTemplate>();
                mockDbSet.AddRange(results);
                return mockDbSet;
            }
        }

        public DbSet<ViewUserSecurable> ViewUserSecurables
        {
            get
            {
                var results = (from ur in this.UserRoles
                               join asr in this.RoleAppSecurables on ur.RoleId equals asr.RoleId
                               join aps in this.AppSecurables on asr.AppSecurableId equals aps.Id
                               select new ViewUserSecurable()
                               {
                                   UserId = ur.UserId,
                                   SecurableName = aps.Name
                               }).Distinct();

                var mockDbSet = CreateMockDbSet<ViewUserSecurable>();
                mockDbSet.AddRange(results);
                return mockDbSet;
            }
        }

        public DbSet<ViewImplementationGuideFile> ViewImplementationGuideFiles
        {
            get
            {
                var maxFileDataDates = (from ifgd in this.ImplementationGuideFileDatas
                                        group ifgd by ifgd.ImplementationGuideFileId into g
                                        select new
                                        {
                                            ImplementationGuideFileId = g.Key,
                                            LastUpdateDate = g.Max(y => y.UpdatedDate)
                                        });

                var results = (from igf in this.ImplementationGuideFiles
                               join mfdd in maxFileDataDates on igf.Id equals mfdd.ImplementationGuideFileId
                               join igfd in this.ImplementationGuideFileDatas on mfdd.ImplementationGuideFileId equals igfd.ImplementationGuideFileId
                               where igfd.UpdatedDate == mfdd.LastUpdateDate
                               select new ViewImplementationGuideFile()
                               {
                                   ContentType = igf.ContentType,
                                   Data = igfd.Data,
                                   ExpectedErrorCount = igf.ExpectedErrorCount,
                                   FileName = igf.FileName,
                                   Id = igfd.Id,
                                   ImplementationGuideId = igf.ImplementationGuideId,
                                   MimeType = igf.MimeType,
                                   Note = igfd.Note,
                                   UpdatedBy = igfd.UpdatedBy,
                                   UpdatedDate = igfd.UpdatedDate
                               });

                var mockDbSet = CreateMockDbSet<ViewImplementationGuideFile>();
                mockDbSet.AddRange(results);
                return mockDbSet;
            }
        }

        public DbSet<ViewTemplate> ViewTemplates
        {
            get
            {
                var results = (from t in this.Templates
                               select new ViewTemplate()
                               {
                                   ConstraintCount = t.ChildConstraints.Count,
                                   ContainedTemplateCount = 0,  // TODO: Fix
                                   Id = t.Id,
                                   ImplementationGuideTypeName = t.ImplementationGuideType.Name,
                                   ImpliedTemplateCount = t.ImplyingTemplates.Count,
                                   ImpliedTemplateOid = t.ImpliedTemplate != null ? t.ImpliedTemplate.Oid : null,
                                   ImpliedTemplateTitle = t.ImpliedTemplate != null ? t.ImpliedTemplate.Name : null,
                                   IsOpen = t.IsOpen,
                                   Name = t.Name,
                                   Oid = t.Oid,
                                   OrganizationName = t.OwningImplementationGuide != null && t.OwningImplementationGuide.Organization != null ? t.OwningImplementationGuide.Organization.Name : null,
                                   OwningImplementationGuideId = t.OwningImplementationGuideId,
                                   OwningImplementationGuideTitle = t.OwningImplementationGuide != null ? t.OwningImplementationGuide.Name : null,
                                   PrimaryContext = t.PrimaryContext,
                                   PublishDate = t.OwningImplementationGuide != null ? t.OwningImplementationGuide.PublishDate : null,
                                   TemplateTypeDisplay = t.TemplateType != null ? t.TemplateType.Name : null,
                                   TemplateTypeId = t.TemplateTypeId,
                                   TemplateTypeName = t.TemplateType != null ? t.TemplateType.Name : null
                               }).Distinct();

                var mockDbSet = CreateMockDbSet<ViewTemplate>();
                mockDbSet.AddRange(results);
                return mockDbSet;
            }
        }

        public DbSet<ViewTemplateList> ViewTemplateLists
        {
            get
            {
                var results = (from t in this.Templates
                               select new ViewTemplateList()
                               {
                                   Id = t.Id,
                                   Oid = t.Oid,
                                   Name = t.Name,
                                   Open = t.IsOpen ? "Yes" : "No",
                                   Organization = t.OwningImplementationGuide != null && t.OwningImplementationGuide.Organization != null ? t.OwningImplementationGuide.Organization.Name : null,
                                   ImplementationGuide = t.OwningImplementationGuide != null ? t.OwningImplementationGuide.Name : null,
                                   PublishDate = t.OwningImplementationGuide != null ? t.OwningImplementationGuide.PublishDate : null,
                                   TemplateType = t.TemplateType.Name + " (" + t.TemplateType.ImplementationGuideType.Name + ")",
                                   ImpliedTemplateName = t.ImpliedTemplate != null ? t.ImpliedTemplate.Name : null,
                                   ImpliedTemplateOid = t.ImpliedTemplate != null ? t.ImpliedTemplate.Oid : null
                                   // TODO: Fill in other fields
                               }).Distinct();

                var mockDbSet = CreateMockDbSet<ViewTemplateList>();
                mockDbSet.AddRange(results);
                return mockDbSet;
            }
        }

        public DbSet<ViewValueSetMemberWhiteSpace> ViewValueSetMemberWhiteSpaces
        {
            get
            {
                var results = (from vs in this.ValueSets
                               join vsm in this.ValueSetMembers on vs.Id equals vsm.ValueSetId
                               where vsm.Code.Trim() != vsm.Code || vsm.DisplayName.Trim() != vsm.DisplayName
                               select new ViewValueSetMemberWhiteSpace()
                               {
                                   ValueSetId = vs.Id,
                                   ValueSetName = vs.Name,
                                   Code = vsm.Code,
                                   DisplayName = vsm.DisplayName
                               });

                var mockDbSet = CreateMockDbSet<ViewValueSetMemberWhiteSpace>();
                mockDbSet.AddRange(results);
                return mockDbSet;
            }
        }

        #endregion

        #region Generic Test Data Generation

        public Organization FindOrCreateOrganization(string name, string contactName = null, string contactEmail = null, string contactPhone = null)
        {
            Organization foundOrganization = this.Organizations.SingleOrDefault(y => y.Name == name);

            if (foundOrganization == null)
            {
                foundOrganization = new Organization()
                {
                    Id = this.Organizations.DefaultIfEmpty().Max(y => y != null ? y.Id : 0) + 1,
                    Name = name,
                    ContactName = contactName,
                    ContactEmail = contactEmail,
                    ContactPhone = contactPhone
                };
                this.Organizations.Add(foundOrganization);
            }

            return foundOrganization;
        }

        public Template CreateTemplate(
            string oid, 
            string typeName, 
            string title, 
            ImplementationGuide owningImplementationGuide, 
            string primaryContext = null, 
            string primaryContextType = null, 
            string description = null, 
            string notes = null, 
            Organization organization = null, 
            Template impliedTemplate = null,
            Template previousVersion = null,
            string status = null)
        {
            TemplateType templateType = this.FindTemplateType(owningImplementationGuide.ImplementationGuideType.Name, typeName);
            PublishStatus publishStatus = null;

            if (!string.IsNullOrEmpty(status))
                publishStatus = this.PublishStatuses.Single(y => y.Status == status);

            var template = CreateTemplate(oid, templateType, title, owningImplementationGuide, primaryContext, primaryContextType, description, notes, impliedTemplate, publishStatus);

            if (previousVersion != null)
                template.SetPreviousVersion(previousVersion);

            return template;
        }
        
        /// <summary>
        /// Creates a new instance of a template with the specified parameters.
        /// </summary>
        /// <remarks>Generates a bookmark automatically based on the title and type of the template.</remarks>
        /// <param name="oid">Required. The oid of the template</param>
        /// <param name="type">Required. The type of template</param>
        /// <param name="title">Required. The title of the template</param>
        /// <param name="description">The description of the template</param>
        /// <param name="notes">The notes of the template</param>
        /// <param name="owningImplementationGuide">The implementation guide that owns the template.</param>
        /// <returns>A new instance of Template that has been appropriately added to the mock object repository.</returns>
        public Template CreateTemplate(string oid, TemplateType type, string title, ImplementationGuide owningImplementationGuide, string primaryContext = null, string primaryContextType = null, string description = null, string notes = null, Template impliedTemplate = null, PublishStatus status = null, string bookmark = null)
        {
            if (string.IsNullOrEmpty(oid))
                throw new ArgumentNullException("oid");

            if (type == null)
                throw new ArgumentNullException("type");

            if (string.IsNullOrEmpty("title"))
                throw new ArgumentNullException("title");

            if (status == null)
                status = PublishStatus.GetDraftStatus(this);

            Template template = new Template()
            {
                Id = this.Templates.DefaultIfEmpty().Max(y => y != null ? y.Id : 0) + 1,
                Bookmark = string.IsNullOrEmpty(bookmark) ? bookmark : null,
                OwningImplementationGuideId = owningImplementationGuide.Id,
                OwningImplementationGuide = owningImplementationGuide,
                ImplementationGuideTypeId = type.ImplementationGuideTypeId,
                ImplementationGuideType = type.ImplementationGuideType,
                ImpliedTemplateId = impliedTemplate != null ? (int?)impliedTemplate.Id : null,
                ImpliedTemplate = impliedTemplate,
                PrimaryContext = string.IsNullOrEmpty(primaryContext) ? type.RootContext : primaryContext,
                PrimaryContextType = string.IsNullOrEmpty(primaryContextType) ? type.RootContextType : primaryContextType,
                TemplateType = type,
                TemplateTypeId = type.Id,
                Name = title,
                Oid = oid,
                Description = description,
                Notes = notes,
                Status = status,
                StatusId = status.Id
            };

            template.Bookmark = title
                .Replace(" ", "_")
                .Replace("\t", "_");

            switch (type.Id)
            {
                case 1:
                    template.Bookmark = "D_" + template.Bookmark;
                    break;
                case 2:
                    template.Bookmark = "S_" + template.Bookmark;
                    break;
                case 3:
                    template.Bookmark = "E_" + template.Bookmark;
                    break;
                case 4:
                    template.Bookmark = "SE_" + template.Bookmark;
                    break;
                case 5:
                    template.Bookmark = "O_" + template.Bookmark;
                    break;
            }

            this.Templates.Add(template);

            if (owningImplementationGuide != null)
                owningImplementationGuide.ChildTemplates.Add(template);

            return template;
        }

        /// <summary>
        /// Generates a data-type for the specified name that can be used by the green libraries.
        /// </summary>
        /// <param name="igType">The implementation guide type for the data type</param>
        /// <param name="name">The name of the data-type</param>
        /// <returns>A new instance of DataType that has been appropriately added to the mock object repository.</returns>
        public ImplementationGuideTypeDataType FindOrCreateDataType(ImplementationGuideType igType, string name)
        {
            ImplementationGuideTypeDataType dt = this.ImplementationGuideTypeDataTypes.SingleOrDefault(y => y.ImplementationGuideTypeId == igType.Id && y.DataTypeName == name);

            if (dt == null)
            {
                dt = new ImplementationGuideTypeDataType()
                {
                    Id = this.ImplementationGuideTypeDataTypes.DefaultIfEmpty().Max(y => y != null ? y.Id : 0) + 1,
                    ImplementationGuideType = igType,
                    ImplementationGuideTypeId = igType.Id,
                    DataTypeName = name
                };

                this.ImplementationGuideTypeDataTypes.Add(dt);
                igType.DataTypes.Add(dt);
            }

            return dt;
        }

        /// <summary>
        /// Attempts to find the implementation guide type based on the name specified.
        /// If none can be found, a new implementation guide type is created and added to the mock object repository.
        /// </summary>
        /// <param name="name">Required. The name of the implementation guide type. This value is used to perform the search.</param>
        /// <param name="schemaLocation">Required. The location of the schema for the implementation guide type.</param>
        /// <param name="prefix">The prefix of the schema for the implementation guide type.</param>
        /// <param name="uri">The namespace uri of the schema for the implementation guide type.</param>
        /// <returns>Either the found implementation guide type, or a new implementation guide tpye that has been appropriately added to the mock object repository.</returns>
        public ImplementationGuideType FindOrCreateImplementationGuideType(string name, string schemaLocation, string prefix, string uri)
        {
            ImplementationGuideType igType = this.ImplementationGuideTypes.SingleOrDefault(y => y.Name == name);

            if (igType == null)
            {
                igType = new ImplementationGuideType()
                {
                    Id = this.ImplementationGuideTypes.DefaultIfEmpty().Max(y => y != null ? y.Id : 0) + 1,
                    Name = name,
                    SchemaLocation = schemaLocation,
                    SchemaPrefix = prefix,
                    SchemaURI = uri
                };

                this.ImplementationGuideTypes.Add(igType);
            }

            return igType;
        }

        public TemplateType FindOrCreateTemplateType(ImplementationGuideType igType, string name)
        {
            int nextOrder = igType.TemplateTypes.DefaultIfEmpty().Max(y => y != null ? y.Id : 1) + 1;
            return this.FindOrCreateTemplateType(igType, name, name, name, nextOrder);
        }

        /// <summary>
        /// Attempts to find a template type based on the implementation guide type and name specified.
        /// If none can be found, a new instance is created and added to the mock object repository and ig type.
        /// </summary>
        /// <param name="igType">Required. The implementation guide type. This value is used to perform the search.</param>
        /// <param name="name">The name of the template type. This value is used to perform the search.</param>
        /// <param name="context">The default contetx of the template type.</param>
        /// <returns>Either the found template type, or a new one which has been added to the mock object repository and the ig type.</returns>
        public TemplateType FindOrCreateTemplateType(ImplementationGuideType igType, string name, string context, string contextType, int outputOrder)
        {
            if (igType == null)
                throw new ArgumentNullException("igType");

            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            TemplateType type = this.TemplateTypes.SingleOrDefault(y => y.ImplementationGuideTypeId == igType.Id && y.Name == name);

            if (type == null)
            {
                type = new TemplateType()
                {
                    Id = this.TemplateTypes.DefaultIfEmpty().Max(y => y != null ? y.Id : 0) + 1,
                    Name = name,
                    ImplementationGuideType = igType,
                    ImplementationGuideTypeId = igType.Id,
                    RootContext = context,
                    RootContextType = contextType,
                    OutputOrder = outputOrder
                };

                igType.TemplateTypes.Add(type);
                this.TemplateTypes.Add(type);
            }

            return type;
        }

        public ImplementationGuide FindOrCreateImplementationGuide(string igTypeName, string title, Organization organization = null, DateTime? publishDate = null, ImplementationGuide previousVersion = null)
        {
            ImplementationGuideType igType = this.FindImplementationGuideType(igTypeName);

            var ig = FindOrCreateImplementationGuide(igType, title, organization, publishDate);

            if (previousVersion != null)
                ig.SetPreviousVersion(previousVersion);

            return ig;
        }

        /// <summary>
        /// Creates an implementation guide section
        /// </summary>
        /// <param name="ig">The ig to add the section to</param>
        /// <param name="heading">The text representation of the heading of the section</param>
        /// <param name="content">The content of the section</param>
        /// <param name="order">The order in which this section is output amongst all sections</param>
        /// <param name="level">The level/depth of the heading (h1, h2, h3, h4, etc.)</param>
        public ImplementationGuideSection CreateImplementationGuideSection(ImplementationGuide ig, string heading, string content, int order, int level = 1)
        {
            ImplementationGuideSection section = new ImplementationGuideSection()
            {
                ImplementationGuide = ig,
                ImplementationGuideId = ig.Id,
                Heading = heading,
                Content = content,
                Order = order,
                Level = level
            };

            this.ImplementationGuideSections.Add(section);
            ig.Sections.Add(section);

            return section;
        }

        /// <summary>
        /// Craetes a new implementation guide for the specified implementation guide type.
        /// The new implementation guide is added to the mock object repository and the implementation guide type.
        /// </summary>
        /// <param name="igType">Required. The implementation guide type for the implementation guide (ex: CDA vs eMeasure)</param>
        /// <param name="title">Required. The title of the implementation guide.</param>
        /// <param name="organization">The organization for the implementation guide.</param>
        /// <param name="publishDate">The publishing date for the implementation guide.</param>
        /// <returns>A new instance of an implementation guide that has been added to the mock object repository.</returns>
        public ImplementationGuide FindOrCreateImplementationGuide(ImplementationGuideType igType, string title, Organization organization = null, DateTime? publishDate = null)
        {
            if (igType == null)
                throw new ArgumentNullException("igType");

            if (string.IsNullOrEmpty(title))
                throw new ArgumentNullException("title");

            ImplementationGuide ig = this.ImplementationGuides.SingleOrDefault(y => y.ImplementationGuideType == igType && y.Name.ToLower() == title.ToLower());

            if (organization == null)
                organization = this.FindOrCreateOrganization(DEFAULT_ORGANIZATION);

            if (ig == null)
            {
                ig = new ImplementationGuide()
                {
                    Id = this.ImplementationGuides.DefaultIfEmpty().Max(y => y != null ? y.Id : 0) + 1,
                    Name = title,
                    ImplementationGuideType = igType,
                    ImplementationGuideTypeId = igType.Id,
                    Organization = organization,
                    OrganizationId = organization.Id,
                    PublishDate = publishDate,
                    Version = 1
                };

                igType.ImplementationGuides.Add(ig);
                this.ImplementationGuides.Add(ig);
            }

            return ig;
        }

        public TemplateConstraint AddPrimitiveToTemplate(
            Template template, 
            TemplateConstraint parent, 
            string conformanceText, 
            string narrative, 
            string schematronTest = null, 
            bool? isBranch = null, 
            bool isPrimitiveSchRooted=false, 
            bool isInheritable=false, 
            int? number = null)
        {
            TemplateConstraint constraint = new TemplateConstraint()
            {
                Id = this.TemplateConstraints.DefaultIfEmpty().Max(y => y != null ? y.Id : 0) + 1,
                IsPrimitive = true,
                Template = template,
                TemplateId = template.Id,
                Conformance = conformanceText,
                PrimitiveText = narrative,
                Schematron = schematronTest,
                IsBranch = isBranch != null ? isBranch.Value : false,
                IsSchRooted = isPrimitiveSchRooted,
                IsInheritable = isInheritable
            };

            if (number == null)
                constraint.Number = constraint.Id;
            else
                constraint.Number = number;

            if (parent != null)
            {
                constraint.ParentConstraint = parent;
                constraint.ParentConstraintId = parent.Id;
            }
            
            template.ChildConstraints.Add(constraint);
            this.TemplateConstraints.Add(constraint);

            return constraint;
        }

        /// <summary>
        /// Creates a new constraint for the specified template.
        /// The constraint is added to the mock object repository and the template.
        /// </summary>
        /// <param name="template">Required. The template for the constraint.</param>
        /// <param name="parentConstraint">The parent constraint.</param>
        /// <param name="containedTemplate">The contained template.</param>
        /// <param name="context">The context of the constraint.</param>
        /// <param name="conformance">The conformance of the constraint (SHALL, SHOULD, MAY, etc)</param>
        /// <param name="cardinality">The cardinality of the constraint ("1..1", "0..1", etc)</param>
        /// <param name="dataType">The data type of the constraint ("CD")</param>
        /// <param name="valueConformance">The value conformance of the constraint.</param>
        /// <param name="value">The value of the constraint (ex: "23423-X")</param>
        /// <param name="displayName">The display name for the value of the constraint.</param>
        /// <param name="valueSet">The valueset associated with the constraint.</param>
        /// <param name="codeSystem">The code system associated with the constraint.</param>
        /// <param name="description">The description of the constraint.</param>
        /// <returns>A new instance of TemplateConstraint that has been added to the specified Template's child constraints and to the mock object repository.</returns>
        public TemplateConstraint AddConstraintToTemplate(
            Template template, 
            TemplateConstraint parentConstraint = null, 
            Template containedTemplate = null, 
            string context = null, 
            string conformance = null, 
            string cardinality = null, 
            string dataType = null, 
            string valueConformance = null, 
            string value = null, 
            string displayName = null, 
            ValueSet valueSet = null, 
            CodeSystem codeSystem = null,
            string description = null,
            bool? isBranch = null,
            bool? isBranchIdentifier = null,
            bool isPrimitiveSchRooted = false,
            int? number = null,
            string category = null,
            bool isChoice = false)
        {
            if (template == null)
                throw new ArgumentNullException("template");

            TemplateConstraint constraint = new TemplateConstraint()
            {
                Id = this.TemplateConstraints.DefaultIfEmpty().Max(y => y != null ? y.Id : 0) + 1,
                Template = template,
                Context = context,
                Conformance = conformance,
                ValueConformance = valueConformance,
                TemplateId = template.Id,
                IsSchRooted = isPrimitiveSchRooted,
                Category = category,
                IsChoice = isChoice
            };

            if (number == null)
                constraint.Number = constraint.Id;
            else
                constraint.Number = number;

            if (parentConstraint != null)
            {
                parentConstraint.ChildConstraints.Add(constraint);
                constraint.ParentConstraint = parentConstraint;
                constraint.ParentConstraintId = parentConstraint.Id;
            }

            if (containedTemplate != null)
            {
                TemplateConstraintReference reference = new TemplateConstraintReference();
                reference.Constraint = constraint;
                reference.ReferenceIdentifier = containedTemplate.Oid;
                reference.ReferenceType = ConstraintReferenceTypes.Template;
                constraint.References.Add(reference);
                this.TemplateConstraintReferences.Add(reference);
            }

            if (!string.IsNullOrEmpty(cardinality))
                constraint.Cardinality = cardinality;

            if (!string.IsNullOrEmpty(dataType))
                constraint.DataType = dataType;

            if (!string.IsNullOrEmpty(value))
                constraint.Value = value;

            if (!string.IsNullOrEmpty(displayName))
                constraint.DisplayName = displayName;

            if (valueSet != null)
            {
                valueSet.Constraints.Add(constraint);
                constraint.ValueSet = valueSet;
                constraint.ValueSetId = valueSet.Id;
            }

            if (codeSystem != null)
            {
                codeSystem.Constraints.Add(constraint);
                constraint.CodeSystem = codeSystem;
                constraint.CodeSystemId = codeSystem.Id;
            }

            if (!string.IsNullOrEmpty(description))
                constraint.Description = description;

            if (isBranch != null)
                constraint.IsBranch = isBranch.Value;

            if (isBranchIdentifier != null)
                constraint.IsBranchIdentifier = isBranchIdentifier.Value;

            template.ChildConstraints.Add(constraint);
            this.TemplateConstraints.Add(constraint);

            return constraint;
        }

        /// <summary>
        /// Attempts to find a Code System based on the oid specified.
        /// If none is found, a new instance is created and added to the mock object repository.
        /// </summary>
        /// <param name="name">Required. The name of the code system (Ex: "SNOMED CT")</param>
        /// <param name="oid">Required. The oid of the code system. This value is used to perform the search.</param>
        /// <param name="description">The description of the code system.</param>
        /// <param name="lastUpdate">The last update date of the code system.</param>
        /// <returns></returns>
        public CodeSystem FindOrCreateCodeSystem(string name, string oid, string description = null, DateTime? lastUpdate = null)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            if (string.IsNullOrEmpty(oid))
                throw new ArgumentNullException("oid");

            CodeSystem codeSystem = this.CodeSystems.SingleOrDefault(y => y.Oid == oid);

            if (codeSystem == null)
            {
                codeSystem = new CodeSystem()
                {
                    Id = this.CodeSystems.Count() > 0 ? this.CodeSystems.Max(y => y.Id) + 1 : 1,
                    Name = name,
                    Oid = oid,
                    Description = description,
                    LastUpdate = lastUpdate != null ? lastUpdate.Value : DateTime.Now
                };

                this.CodeSystems.Add(codeSystem);
            }

            return codeSystem;
        }

        /// <summary>
        /// Attempts to find a Value Set based on the oid specified.
        /// If none is found, a new instance is created and added to the mock object repository.
        /// </summary>
        /// <param name="name">Required. The name of the value set.</param>
        /// <param name="identifier">Required. The oid of the value set. This value is used to perform the search.</param>
        /// <param name="code">The code of the value set.</param>
        /// <param name="description">The description of the valueset.</param>
        /// <param name="intensional">Indicates whether the value set is intensional or extensional.</param>
        /// <param name="intensionalDefinition">Describes the algorithm used for intensional value sets.</param>
        /// <param name="lastUpdate">The last update date of the value set.</param>
        /// <returns>Either the found ValueSet or a new instance of one, which has been added to the mock object repository.</returns>
        public ValueSet FindOrCreateValueSet(string name, string identifier, string code = null, string description = null, bool? intensional = null, string intensionalDefinition = null, DateTime? lastUpdate = null)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            if (string.IsNullOrEmpty(identifier))
                throw new ArgumentNullException("oid");

            ValueSet valueSet = (from v in this.ValueSets
                                 join vsi in this.ValueSetIdentifiers on v.Id equals vsi.ValueSetId
                                 where vsi.Identifier.ToLower().Trim() == identifier.ToLower().Trim()
                                 select v)
                                 .FirstOrDefault();

            if (valueSet == null)
            {
                valueSet = new ValueSet()
                {
                    Id = this.ValueSets.DefaultIfEmpty().Max(y => y != null ? y.Id : 0) + 1,
                    Name = name,
                    Description = intensionalDefinition,
                    Intensional = intensional,
                    IntensionalDefinition = intensionalDefinition,
                    Code = code,
                    LastUpdate = lastUpdate != null ? lastUpdate.Value : DateTime.Now
                };

                this.ValueSets.Add(valueSet);

                // Add the identifier
                ValueSetIdentifier vsi = new ValueSetIdentifier();
                vsi.ValueSetId = valueSet.Id;
                vsi.ValueSet = valueSet;
                vsi.Identifier = identifier;

                valueSet.Identifiers.Add(vsi);
                this.ValueSetIdentifiers.Add(vsi);
            }

            return valueSet;
        }

        /// <summary>
        /// Attempts to find a ValueSetMember based on the valueset and code specified.
        /// If none is found, a new instance is created and added to the mock object repository.
        /// </summary>
        /// <param name="valueSet">Required. The valueset owning the member. This value is used to perform the search.</param>
        /// <param name="codeSystem">Required. The code system for the member.</param>
        /// <param name="code">Required. The code for the member. This value is used to perform the search.</param>
        /// <param name="displayName">The display name of the member.</param>
        /// <param name="valueSetStatus">The status of the member (ex: "active" or "inactive").</param>
        /// <param name="dateOfValueSetStatus">The date that the status of the member was set.</param>
        /// <param name="lastUpdate">The last update date of the value set member.</param>
        /// <returns>Either the found ValueSetMember, or a new instance of one which has been added to the valueset and the mock object repository.</returns>
        public ValueSetMember FindOrCreateValueSetMember(ValueSet valueSet, CodeSystem codeSystem, string code, string displayName, string valueSetStatus = null, string dateOfValueSetStatus = null, DateTime? lastUpdate = null)
        {
            if (valueSet == null)
                throw new ArgumentNullException("valueSet");

            if (codeSystem == null)
                throw new ArgumentNullException("codeSystem");

            if (string.IsNullOrEmpty(code))
                throw new ArgumentNullException("code");
            
            DateTime? statusDate = !string.IsNullOrEmpty(dateOfValueSetStatus) ? new DateTime?(DateTime.Parse(dateOfValueSetStatus)) : null;
            ValueSetMember member = this.ValueSetMembers.SingleOrDefault(y => y.ValueSetId == valueSet.Id && y.Code == code && y.Status == valueSetStatus && y.StatusDate == statusDate);

            if (member == null)
            {
                member = new ValueSetMember()
                {
                    Id = this.ValueSetMembers.DefaultIfEmpty().Max(y => y != null ? y.Id : 0) + 1,
                    ValueSet = valueSet,
                    ValueSetId = valueSet.Id,
                    CodeSystem = codeSystem,
                    CodeSystemId = codeSystem.Id,
                    Code = code,
                    DisplayName = displayName,
                    Status = valueSetStatus,
                    StatusDate = statusDate
                };

                this.ValueSetMembers.Add(member);
                valueSet.Members.Add(member);
            }

            return member;
        }

        public ImplementationGuideFile CreateImplementationGuideFile(ImplementationGuide ig, string fileName, string contentType, string mimeType, int? expectedErrorCount=null, byte[] content=null)
        {
            ImplementationGuideFile igf = new ImplementationGuideFile()
            {
                Id = this.ImplementationGuideFiles.DefaultIfEmpty().Max(y => y != null ? y.Id : 0) + 1,
                FileName = fileName,
                ContentType = contentType,
                MimeType = mimeType,
                ExpectedErrorCount = expectedErrorCount,
                ImplementationGuide = ig,
                ImplementationGuideId = ig.Id
            };

            if (content != null)
            {
                this.CreateImplementationGuideFileData(igf, content);
            }

            this.ImplementationGuideFiles.Add(igf);
            ig.Files.Add(igf);

            return igf;
        }

        public ImplementationGuideFileData CreateImplementationGuideFileData(ImplementationGuideFile igFile, byte[] content, string note=null, DateTime? updatedDate = null)
        {
            ImplementationGuideFileData igFileData = new ImplementationGuideFileData()
            {
                Id = this.ImplementationGuideFileDatas.DefaultIfEmpty().Max(y => y != null ? y.Id : 0) + 1,
                ImplementationGuideFile = igFile,
                ImplementationGuideFileId = igFile.Id,
                Data = content,
                Note = note,
                UpdatedDate = updatedDate != null ? updatedDate.Value : DateTime.Now,
                UpdatedBy = "unit test"
            };

            this.ImplementationGuideFileDatas.Add(igFileData);
            igFile.Versions.Add(igFileData);

            return igFileData;
        }

        public User FindOrCreateUser(string username)
        {
            User foundUser = this.Users.SingleOrDefault(y => y.UserName.ToLower() == username.ToLower());

            if (foundUser != null)
                return foundUser;

            User newUser = new User()
            {
                Id = this.Users.DefaultIfEmpty().Max(y => y != null ? y.Id : 0) + 1,
                UserName = username
            };

            this.Users.Add(newUser);

            return newUser;
        }

        public Role FindOrCreateRole(string roleName)
        {
            Role role = this.Roles.SingleOrDefault(y => y.Name.ToLower() == roleName.ToLower());

            if (role != null)
                return role;

            role = new Role()
            {
                Id = this.Roles.DefaultIfEmpty().Max(y => y != null ? y.Id : 0) + 1,
                Name = roleName
            };

            this.Roles.Add(role);

            return role;
        }

        public void CreateRole(string roleName, params string[] securables)
        {
            Role role = FindOrCreateRole(roleName);

            AssociateSecurableToRole(roleName, securables);
        }

        public void AssociateSecurableToRole(string roleName, params string[] securables)
        {
            Role role = FindOrCreateRole(roleName);

            foreach (string cSecurable in securables)
            {
                AppSecurable securable = FindOrCreateSecurable(cSecurable);

                RoleAppSecurable newRoleAppSecurable = new RoleAppSecurable()
                {
                    Id = this.RoleAppSecurables.DefaultIfEmpty().Max(y => y != null ? y.Id : 0) + 1,
                    AppSecurableId = securable.Id,
                    AppSecurable = securable,
                    RoleId = role.Id,
                    Role = role
                };

                this.RoleAppSecurables.Add(newRoleAppSecurable);
                role.AppSecurables.Add(newRoleAppSecurable);
            }
        }

        public void RemoveSecurableFromRole(string roleName, params string[] securables)
        {
            Role role = FindOrCreateRole(roleName);

            foreach (string cSecurable in securables)
            {
                AppSecurable securable = FindOrCreateSecurable(cSecurable);

                RoleAppSecurable foundRoleSecurable = this.RoleAppSecurables.Single(y => y.Role == role && y.AppSecurable == securable);

                if (foundRoleSecurable != null)
                {
                    role.AppSecurables.Remove(foundRoleSecurable);
                    this.RoleAppSecurables.Remove(foundRoleSecurable);
                }
            }
        }

        public void AssociateUserWithRole(string userName, string roleName)
        {
            User foundUser = this.Users.Single(y => y.UserName.ToLower() == userName.ToLower());
            Role foundRole = this.Roles.Single(y => y.Name.ToLower() == roleName.ToLower());

            if (foundUser.Roles.Count(y => y.RoleId == foundRole.Id) > 0)
                return;

            UserRole newUserRole = new UserRole()
            {
                Id = this.UserRoles.DefaultIfEmpty().Max(y => y != null ? y.Id : 0) + 1,
                UserId = foundUser.Id,
                User = foundUser,
                RoleId = foundRole.Id,
                Role = foundRole
            };

            foundUser.Roles.Add(newUserRole);
            this.UserRoles.Add(newUserRole);
        }

        public AppSecurable FindOrCreateSecurable(string securableName, string description = null)
        {
            AppSecurable securable = this.AppSecurables.SingleOrDefault(y => y.Name.ToLower() == securableName.ToLower());

            if (securable != null)
                return securable;

            securable = new AppSecurable()
            {
                Id = this.AppSecurables.DefaultIfEmpty().Max(y => y != null ? y.Id : 0) + 1,
                Name = securableName,
                Description = description
            };

            this.AppSecurables.Add(securable);

            return securable;
        }

        public void FindOrCreateSecurables(params string[] securableNames)
        {
            foreach (string securableName in securableNames)
            {
                this.FindOrCreateSecurable(securableName);
            }
        }

        public void GrantImplementationGuidePermission(User user, ImplementationGuide ig, string permission)
        {
            var newPerm = new ImplementationGuidePermission()
            {
                ImplementationGuide = ig,
                ImplementationGuideId = ig.Id,
                User = user,
                UserId = user.Id,
                Permission = permission,
                Type = "User"
            };

            this.ImplementationGuidePermissions.Add(newPerm);
            ig.Permissions.Add(newPerm);
        }

        public ImplementationGuideTypeDataType FindOrCreateDataType(string igTypeName, string dataTypeName)
        {
            var igType = this.FindImplementationGuideType(igTypeName);
            var dataType = igType.DataTypes.SingleOrDefault(y => y.DataTypeName == dataTypeName);

            if (dataType == null)
            {
                dataType = new ImplementationGuideTypeDataType()
                {
                    DataTypeName = dataTypeName,
                    ImplementationGuideType = igType
                };

                this.ImplementationGuideTypeDataTypes.Add(dataType);
            }

            return dataType;
        }

        #endregion

        #region Simple Find Methods

        public ImplementationGuideType FindImplementationGuideType(string igType)
        {
            return this.ImplementationGuideTypes.SingleOrDefault(y => y.Name == igType);
        }

        public TemplateType FindTemplateType(string igType, string templateType)
        {
            return this.TemplateTypes.SingleOrDefault(y => y.ImplementationGuideType.Name == igType && y.Name == templateType);
        }

        #endregion

        #region Unimplemented Methods

        public DbConnection Connection
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public void ChangeObjectState(object entity, System.Data.Entity.EntityState entityState)
        {
            
        }

        public DbContextTransaction BeginTransaction()
        {
            throw new NotImplementedException();
        }

        public DbContextTransaction BeginTransaction(System.Data.IsolationLevel isolationLevel)
        {
            throw new NotImplementedException();
        }


        public int SaveChanges()
        {
            foreach (var template in this.Templates)
            {
                // Mock the association between template and child constraints
                foreach (var constraint in template.ChildConstraints.Where(y => y.TemplateId == 0 || y.Template == null))
                {
                    if (!this.TemplateConstraints.Contains(constraint))
                        this.TemplateConstraints.Add(constraint);

                    constraint.Template = template;
                    constraint.TemplateId = template.Id;
                }
            }

            return 0;
        }

        public void Dispose()
        {

        }

        #endregion

        #region Stored Procedures

        private IEnumerable<Template> GetTemplateReferences(List<Template> checkedTemplates, Template template, bool includeInferred)
        {
            List<Template> templates = new List<Template>();

            if (checkedTemplates.Contains(template))
                return templates;

            // Add the template to the list of checked templates right away, because we know we are going to check it
            // and in case one of the contained or implied template references itself, directly.
            checkedTemplates.Add(template);

            foreach (var reference in template.ChildConstraints.SelectMany(y => y.References).Where(y => y.ReferenceType == ConstraintReferenceTypes.Template))
            {
                var containedTemplate = this.Templates.Single(y => y.Oid == reference.ReferenceIdentifier);

                var templateAlreadyIncluded = templates.Contains(containedTemplate);
                var externalTemplate = containedTemplate.OwningImplementationGuideId != template.OwningImplementationGuideId;

                if ((!externalTemplate || includeInferred) && !templateAlreadyIncluded)
                {
                    templates.Add(containedTemplate);

                    templates.AddRange(
                        GetTemplateReferences(checkedTemplates, containedTemplate, includeInferred));
                }
            }

            if (template.ImpliedTemplate != null)
            {
                var externalTemplate = template.ImpliedTemplate.OwningImplementationGuideId != template.OwningImplementationGuideId;
                var templateAlreadyIncluded = templates.Contains(template.ImpliedTemplate);

                if ((!externalTemplate || includeInferred) && !templateAlreadyIncluded)
                {
                    templates.Add(template.ImpliedTemplate);

                    templates.AddRange(
                        GetTemplateReferences(checkedTemplates, template.ImpliedTemplate, includeInferred));
                }
            }

            return templates.Distinct();
        }

        public IEnumerable<SearchValueSetResult> SearchValueSet(Nullable<global::System.Int32> userId, global::System.String searchText, Nullable<global::System.Int32> count, Nullable<global::System.Int32> page, global::System.String orderProperty, Nullable<global::System.Boolean> orderDesc)
        {
            throw new NotImplementedException();
        }

        IEnumerable<Nullable<global::System.Int32>> IObjectRepository.GetImplementationGuideTemplates(Nullable<global::System.Int32> implementationGuideId, Nullable<global::System.Boolean> inferred, Nullable<global::System.Int32> parentTemplateId, string[] categories)
        {
            var implementationGuide = this.ImplementationGuides.Single(y => y.Id == implementationGuideId);

            // A list of templates that will be used by GetTemplateReferences() to determine
            // if a template has already been checked, so that an endless loop does not occur
            List<Template> checkedTemplates = new List<Template>();

            List<Template> templates = new List<Template>();
            var currentVersionId = (Nullable<int>)implementationGuide.Id;
            var retiredStatus = PublishStatus.GetRetiredStatus(this);

            while (currentVersionId.HasValue)
            {
                ImplementationGuide currentVersion = this.ImplementationGuides.Single(ig => ig.Id == currentVersionId);

                // Ignore previous version IG templates that have been retired
                var childTemplates = currentVersion.ChildTemplates.Where(y => y.OwningImplementationGuideId == implementationGuide.Id || y.Status != retiredStatus);

                // Loop through all templates in the current version
                foreach (Template lTemplate in childTemplates)
                {
                    List<Template> nextVersionTemplates = new List<Template>();
                    var nextCurrent = lTemplate;

                    // Build a list of all this template's next ids
                    while (nextCurrent != null)
                    {
                        nextVersionTemplates.Add(nextCurrent);

                        // Make sure we stop at this version of the IG, and don't look at future versions of the IG
                        if (nextCurrent.OwningImplementationGuideId != implementationGuide.Id)
                            nextCurrent = nextCurrent.NextVersions.FirstOrDefault();
                        else
                            nextCurrent = null;
                    }

                    // Skip the tempalte if it has been retired in a future version
                    if (nextVersionTemplates.Count(y => y.Status == retiredStatus && y.OwningImplementationGuideId != implementationGuide.Id) > 0)
                        continue;

                    // Look if the previous version has a newer version already in the list
                    IEnumerable<Template> lNewerVersions = from t in templates
                                                           join nt in nextVersionTemplates on t.PreviousVersion equals nt
                                                           select t;

                    // If there are no newer versions of the template (which means it is not included yet), add it to the list
                    if (lNewerVersions.Count() == 0)
                    {
                        templates.Add(lTemplate);

                        templates.AddRange(
                            GetTemplateReferences(checkedTemplates, lTemplate, inferred == null ? true : inferred.Value));
                    }
                }

                // Move to the previous version of the IG
                currentVersionId = currentVersion.PreviousVersionImplementationGuideId;
            }

            return templates.Select(y => (int?)y.Id);
        }

        IEnumerable<Nullable<global::System.Int32>> IObjectRepository.SearchTemplates(Nullable<global::System.Int32> userId, Nullable<global::System.Int32> filterImplementationGuideId, global::System.String filterName, global::System.String filterIdentifier, Nullable<global::System.Int32> filterTemplateTypeId, Nullable<global::System.Int32> filterOrganizationId, global::System.String filterContextType, global::System.String queryText)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public static class TemplateConstraintExtensions
    {
        public static TemplateConstraint AddChildConstraintToTemplate(
            this TemplateConstraint parentConstraint,
            MockObjectRepository tdb,
            Template template,
            Template containedTemplate = null,
            string context = null,
            string conformance = null,
            string cardinality = null,
            string dataType = null,
            string valueConformance = null,
            string value = null,
            string displayName = null,
            ValueSet valueSet = null,
            CodeSystem codeSystem = null,
            string description = null,
            bool? isBranch = null,
            bool? isBranchIdentifier = null,
            bool isPrimitiveSchRooted = false,
            int? number = null,
            string category = null)
        {
            return tdb.AddConstraintToTemplate(template, parentConstraint, containedTemplate, context, conformance, cardinality, dataType, valueConformance, value, displayName, valueSet, codeSystem, description, isBranch, isBranchIdentifier, isPrimitiveSchRooted, number, category);
        }
    }
}