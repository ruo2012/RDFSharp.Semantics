﻿/*
   Copyright 2012-2016 Marco De Salvo

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

     http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RDFSharp.Model;

namespace RDFSharp.Semantics
{

    /// <summary>
    /// RDFOntologyData represents the data component (A-BOX) of an ontology.
    /// </summary>
    public class RDFOntologyData: IEnumerable<RDFOntologyFact> {

        #region Properties
        /// <summary>
        /// Count of the facts composing the data
        /// </summary>
        public Int64 FactsCount {
            get { return this.Facts.Count; }
        }

        /// <summary>
        /// Count of the literals composing the data
        /// </summary>
        public Int64 LiteralsCount {
            get { return this.Literals.Count; }
        }

        /// <summary>
        /// Gets the enumerator on the facts of the data for iteration
        /// </summary>
        public IEnumerator<RDFOntologyFact> FactsEnumerator {
            get { return this.Facts.Values.GetEnumerator(); }
        }

        /// <summary>
        /// Gets the enumerator on the literals of the data for iteration
        /// </summary>
        public IEnumerator<RDFOntologyLiteral> LiteralsEnumerator {
            get { return this.Literals.Values.GetEnumerator(); }
        }

        /// <summary>
        /// Annotations describing facts of the ontology data
        /// </summary>
        public RDFOntologyAnnotationsMetadata Annotations { get; internal set; }

        /// <summary>
        /// Relations describing facts of the ontology data
        /// </summary>
        public RDFOntologyDataMetadata Relations { get; internal set; }

        /// <summary>
        /// Dictionary of facts composing the data
        /// </summary>
        internal Dictionary<Int64, RDFOntologyFact> Facts { get; set; }

        /// <summary>
        /// Dictionary of literals composing the data
        /// </summary>
        internal Dictionary<Int64, RDFOntologyLiteral> Literals { get; set; }
        #endregion

        #region Ctors
        /// <summary>
        /// Default-ctor to build an empty ontology data
        /// </summary>
        public RDFOntologyData() {
            this.Facts       = new Dictionary<Int64, RDFOntologyFact>();
            this.Literals    = new Dictionary<Int64, RDFOntologyLiteral>();
            this.Annotations = new RDFOntologyAnnotationsMetadata();
            this.Relations   = new RDFOntologyDataMetadata();
        }
        #endregion

        #region Interfaces
        /// <summary>
        /// Exposes a typed enumerator on the data's facts
        /// </summary>
        IEnumerator<RDFOntologyFact> IEnumerable<RDFOntologyFact>.GetEnumerator() {
            return this.Facts.Values.GetEnumerator();
        }

        /// <summary>
        /// Exposes an untyped enumerator on the data's facts
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator() {
            return this.Facts.Values.GetEnumerator();
        }
        #endregion

        #region Methods

        #region Add
        /// <summary>
        /// Adds the given fact to the data
        /// </summary>
        public RDFOntologyData AddFact(RDFOntologyFact ontologyFact) {
            if (ontologyFact  != null) {
                if (!this.Facts.ContainsKey(ontologyFact.PatternMemberID)) {
                     this.Facts.Add(ontologyFact.PatternMemberID, ontologyFact);
                }
            }
            return this;
        }

        /// <summary>
        /// Adds the given literal to the data
        /// </summary>
        internal RDFOntologyData AddLiteral(RDFOntologyLiteral ontologyLiteral) {
            if (ontologyLiteral  != null) {
                if (!this.Literals.ContainsKey(ontologyLiteral.PatternMemberID)) {
                     this.Literals.Add(ontologyLiteral.PatternMemberID, ontologyLiteral);
                }
            }
            return this;
        }

        /// <summary>
        /// Adds the "ontologyFact -> owl:VersionInfo -> ontologyLiteral" annotation to the data
        /// </summary>
        public RDFOntologyData AddVersionInfoAnnotation(RDFOntologyFact ontologyFact, 
                                                        RDFOntologyLiteral ontologyLiteral) {
            if (ontologyFact   != null && ontologyLiteral != null) {
                this.Annotations.VersionInfo.AddEntry(new RDFOntologyTaxonomyEntry(ontologyFact, RDFBASEOntology.Instance.Model.PropertyModel.SelectProperty(RDFVocabulary.OWL.VERSION_INFO.ToString()), ontologyLiteral));
                this.AddLiteral(ontologyLiteral);
            }
            return this;
        }

        /// <summary>
        /// Adds the "ontologyFact -> rdfs:comment -> ontologyLiteral" annotation to the data
        /// </summary>
        public RDFOntologyData AddCommentAnnotation(RDFOntologyFact ontologyFact, 
                                                    RDFOntologyLiteral ontologyLiteral) {
            if (ontologyFact   != null && ontologyLiteral != null) {
                this.Annotations.Comment.AddEntry(new RDFOntologyTaxonomyEntry(ontologyFact, RDFBASEOntology.Instance.Model.PropertyModel.SelectProperty(RDFVocabulary.RDFS.COMMENT.ToString()), ontologyLiteral));
                this.AddLiteral(ontologyLiteral);
            }
            return this;
        }

        /// <summary>
        /// Adds the "ontologyFact -> rdfs:label -> ontologyLiteral" annotation to the data
        /// </summary>
        public RDFOntologyData AddLabelAnnotation(RDFOntologyFact ontologyFact, 
                                                  RDFOntologyLiteral ontologyLiteral) {
            if (ontologyFact   != null && ontologyLiteral != null) {
                this.Annotations.Label.AddEntry(new RDFOntologyTaxonomyEntry(ontologyFact, RDFBASEOntology.Instance.Model.PropertyModel.SelectProperty(RDFVocabulary.RDFS.LABEL.ToString()), ontologyLiteral));
                this.AddLiteral(ontologyLiteral);
            }
            return this;
        }

        /// <summary>
        /// Adds the "ontologyFact -> rdfs:seeAlso -> ontologyResource" annotation to the data
        /// </summary>
        public RDFOntologyData AddSeeAlsoAnnotation(RDFOntologyFact ontologyFact, 
                                                    RDFOntologyResource ontologyResource) {
            if (ontologyFact   != null && ontologyResource != null) {
                this.Annotations.SeeAlso.AddEntry(new RDFOntologyTaxonomyEntry(ontologyFact, RDFBASEOntology.Instance.Model.PropertyModel.SelectProperty(RDFVocabulary.RDFS.SEE_ALSO.ToString()), ontologyResource));
                if (ontologyResource.IsLiteral()) {
                    this.AddLiteral((RDFOntologyLiteral)ontologyResource);
                }
            }
            return this;
        }

        /// <summary>
        /// Adds the "ontologyFact -> rdfs:isDefinedBy -> ontologyResource" annotation to the data
        /// </summary>
        public RDFOntologyData AddIsDefinedByAnnotation(RDFOntologyFact ontologyFact, 
                                                        RDFOntologyResource ontologyResource) {
            if (ontologyFact   != null && ontologyResource != null) {
                this.Annotations.IsDefinedBy.AddEntry(new RDFOntologyTaxonomyEntry(ontologyFact, RDFBASEOntology.Instance.Model.PropertyModel.SelectProperty(RDFVocabulary.RDFS.IS_DEFINED_BY.ToString()), ontologyResource));
                if (ontologyResource.IsLiteral()) {
                    this.AddLiteral((RDFOntologyLiteral)ontologyResource);
                }
            }
            return this;
        }

        /// <summary>
        /// Adds the "ontologyFact -> ontologyAnnotationProperty -> ontologyResource" annotation to the data
        /// </summary>
        public RDFOntologyData AddCustomAnnotation(RDFOntologyFact ontologyFact, 
                                                   RDFOntologyAnnotationProperty ontologyAnnotationProperty, 
                                                   RDFOntologyResource ontologyResource) {
            if (ontologyFact   != null && ontologyAnnotationProperty != null && ontologyResource != null) {

                //owl:versionInfo
                if (ontologyAnnotationProperty.Equals(RDFBASEOntology.Instance.Model.PropertyModel.SelectProperty(RDFVocabulary.OWL.VERSION_INFO.ToString()))) {
                    if (ontologyResource.IsLiteral()) {
                        this.AddVersionInfoAnnotation(ontologyFact, (RDFOntologyLiteral)ontologyResource);
                    }
                }

                //vs:term_status
                else if(ontologyAnnotationProperty.Equals(RDFBASEOntology.Instance.Model.PropertyModel.SelectProperty(RDFVocabulary.VS.TERM_STATUS.ToString()))) {

                    //Raise warning event to inform the user: vs:term_status annotation property cannot be used for facts
                    RDFSemanticsEvents.RaiseSemanticsWarning("vs:term_status annotation property cannot be used for facts.");

                }

                //rdfs:comment
                else if(ontologyAnnotationProperty.Equals(RDFBASEOntology.Instance.Model.PropertyModel.SelectProperty(RDFVocabulary.RDFS.COMMENT.ToString()))) {
                     if(ontologyResource.IsLiteral()) {
                        this.AddCommentAnnotation(ontologyFact, (RDFOntologyLiteral)ontologyResource);
                     }
                }

                //rdfs:label
                else if(ontologyAnnotationProperty.Equals(RDFBASEOntology.Instance.Model.PropertyModel.SelectProperty(RDFVocabulary.RDFS.LABEL.ToString()))) {
                     if(ontologyResource.IsLiteral()) {
                        this.AddLabelAnnotation(ontologyFact, (RDFOntologyLiteral)ontologyResource);
                     }
                }

                //rdfs:seeAlso
                else if(ontologyAnnotationProperty.Equals(RDFBASEOntology.Instance.Model.PropertyModel.SelectProperty(RDFVocabulary.RDFS.SEE_ALSO.ToString()))) {
                     this.AddSeeAlsoAnnotation(ontologyFact, ontologyResource);
                }

                //rdfs:isDefinedBy
                else if(ontologyAnnotationProperty.Equals(RDFBASEOntology.Instance.Model.PropertyModel.SelectProperty(RDFVocabulary.RDFS.IS_DEFINED_BY.ToString()))) {
                     this.AddIsDefinedByAnnotation(ontologyFact, ontologyResource);
                }

                //ontology-specific
                else if(ontologyAnnotationProperty.Equals(RDFBASEOntology.Instance.Model.PropertyModel.SelectProperty(RDFVocabulary.OWL.VERSION_IRI.ToString()))              ||
                        ontologyAnnotationProperty.Equals(RDFBASEOntology.Instance.Model.PropertyModel.SelectProperty(RDFVocabulary.OWL.IMPORTS.ToString()))                  ||
                        ontologyAnnotationProperty.Equals(RDFBASEOntology.Instance.Model.PropertyModel.SelectProperty(RDFVocabulary.OWL.BACKWARD_COMPATIBLE_WITH.ToString())) ||
                        ontologyAnnotationProperty.Equals(RDFBASEOntology.Instance.Model.PropertyModel.SelectProperty(RDFVocabulary.OWL.INCOMPATIBLE_WITH.ToString()))        ||
                        ontologyAnnotationProperty.Equals(RDFBASEOntology.Instance.Model.PropertyModel.SelectProperty(RDFVocabulary.OWL.PRIOR_VERSION.ToString()))) {

                     //Raise warning event to inform the user: Ontology-specific annotation properties cannot be used for facts
                     RDFSemanticsEvents.RaiseSemanticsWarning("Ontology-specific annotation properties cannot be used for facts.");

                }

                //custom
                else {
                    this.Annotations.CustomAnnotations.AddEntry(new RDFOntologyTaxonomyEntry(ontologyFact, ontologyAnnotationProperty, ontologyResource));
                    if (ontologyResource.IsLiteral()) {
                        this.AddLiteral((RDFOntologyLiteral)ontologyResource);
                    }
                }

            }
            return this;
        }

        /// <summary>
        /// Adds the "ontologyFact -> rdf:type -> ontologyClass" relation to the data.
        /// </summary>
        public RDFOntologyData AddClassTypeRelation(RDFOntologyFact ontologyFact, 
                                                    RDFOntologyClass ontologyClass) {
            if (ontologyFact != null && ontologyClass != null) {

                //Enforce taxonomy checks before adding the classType relation
                //Only plain classes can be explicitly assigned as classtypes of facts
                if (!ontologyClass.IsRestrictionClass() && 
                    !ontologyClass.IsCompositeClass()   &&
                    !ontologyClass.IsEnumerateClass()   && 
                    !ontologyClass.IsDataRangeClass()   &&
                    //owl:Nothing cannot be assigned as classtype of facts
                    !ontologyClass.Equals(RDFBASEOntology.Instance.Model.ClassModel.SelectClass(RDFVocabulary.OWL.NOTHING.ToString()))) {
                     this.Relations.ClassType.AddEntry(new RDFOntologyTaxonomyEntry(ontologyFact, RDFBASEOntology.Instance.Model.PropertyModel.SelectProperty(RDFVocabulary.RDF.TYPE.ToString()), ontologyClass));
                }
                else {

                     //Raise warning event to inform the user: ClassType relation cannot be added to the data because only plain classes can be explicitly assigned as class types of facts
                     RDFSemanticsEvents.RaiseSemanticsWarning(String.Format("ClassType relation between fact '{0}' and class '{1}' cannot be added to the data because only plain classes can be explicitly assigned as class types of facts.", ontologyFact, ontologyClass));
                     
                }

            }
            return this;
        }

        /// <summary>
        /// Adds the "aFact -> owl:sameAs -> bFact" relation to the data
        /// </summary>
        public RDFOntologyData AddSameAsRelation(RDFOntologyFact aFact, 
                                                 RDFOntologyFact bFact) {
            if (aFact != null && bFact != null && !aFact.Equals(bFact)) {

                //Enforce taxonomy checks before adding the sameAs relation
                if (!RDFBASEOntologyReasoningHelper.IsDifferentFactFrom(aFact, bFact, this)) {
                     this.Relations.SameAs.AddEntry(new RDFOntologyTaxonomyEntry(aFact, RDFBASEOntology.Instance.Model.PropertyModel.SelectProperty(RDFVocabulary.OWL.SAME_AS.ToString()), bFact));
                     this.Relations.SameAs.AddEntry(new RDFOntologyTaxonomyEntry(bFact, RDFBASEOntology.Instance.Model.PropertyModel.SelectProperty(RDFVocabulary.OWL.SAME_AS.ToString()), aFact).SetInference(RDFSemanticsEnums.RDFOntologyInferenceType.API));
                }
                else {

                     //Raise warning event to inform the user: SameAs relation cannot be added to the data because it violates the taxonomy consistency
                     RDFSemanticsEvents.RaiseSemanticsWarning(String.Format("SameAs relation between fact '{0}' and fact '{1}' cannot be added to the data because it violates the taxonomy consistency.", aFact, bFact));

                }

            }
            return this;
        }

        /// <summary>
        /// Adds the "aFact -> owl:differentFrom -> bFact" relation to the data
        /// </summary>
        public RDFOntologyData AddDifferentFromRelation(RDFOntologyFact aFact, 
                                                        RDFOntologyFact bFact) {
            if (aFact != null && bFact != null && !aFact.Equals(bFact)) {

                //Enforce taxonomy checks before adding the differentFrom relation
                if (!RDFBASEOntologyReasoningHelper.IsSameFactAs(aFact, bFact, this)) {
                     this.Relations.DifferentFrom.AddEntry(new RDFOntologyTaxonomyEntry(aFact, RDFBASEOntology.Instance.Model.PropertyModel.SelectProperty(RDFVocabulary.OWL.DIFFERENT_FROM.ToString()), bFact));
                     this.Relations.DifferentFrom.AddEntry(new RDFOntologyTaxonomyEntry(bFact, RDFBASEOntology.Instance.Model.PropertyModel.SelectProperty(RDFVocabulary.OWL.DIFFERENT_FROM.ToString()), aFact).SetInference(RDFSemanticsEnums.RDFOntologyInferenceType.API));
                }
                else {
                     
                     //Raise warning event to inform the user: DifferentFrom relation cannot be added to the data because it violates the taxonomy consistency
                     RDFSemanticsEvents.RaiseSemanticsWarning(String.Format("DifferentFrom relation between fact '{0}' and fact '{1}' cannot be added to the data because it violates the taxonomy consistency.", aFact, bFact));

                }

            }
            return this;
        }

        /// <summary>
        /// Adds the "aFact -> objectProperty -> bFact" relation to the data
        /// </summary>
        public RDFOntologyData AddAssertionRelation(RDFOntologyFact aFact, 
                                                    RDFOntologyObjectProperty objectProperty, 
                                                    RDFOntologyFact bFact) {
            if (aFact != null && objectProperty != null && bFact != null) {

                //Enforce taxonomy checks before adding the assertion
                //Even if legal, we don't permit creation of transitive cycles!!
                if(!RDFBASEOntologyReasoningHelper.IsTransitiveAssertionOf(bFact, objectProperty, aFact, this)) {
                    this.Relations.Assertions.AddEntry(new RDFOntologyTaxonomyEntry(aFact, objectProperty, bFact));
                }
                else {

                    //Raise warning event to inform the user: Assertion relation cannot be added to the data because it violates the taxonomy transitive consistency
                    RDFSemanticsEvents.RaiseSemanticsWarning(String.Format("Assertion relation between fact '{0}' and fact '{1}' with transitive property '{2}' cannot be added to the data because it would violate the taxonomy consistency (transitive cycle detected).", aFact, bFact, objectProperty));

                }

            }
            return this;
        }

        /// <summary>
        /// Adds the "ontologyFact -> datatypeProperty -> ontologyLiteral" relation to the data
        /// </summary>
        public RDFOntologyData AddAssertionRelation(RDFOntologyFact ontologyFact, 
                                                    RDFOntologyDatatypeProperty datatypeProperty, 
                                                    RDFOntologyLiteral ontologyLiteral) {
            if (ontologyFact != null && datatypeProperty != null && ontologyLiteral != null) {
                this.Relations.Assertions.AddEntry(new RDFOntologyTaxonomyEntry(ontologyFact, datatypeProperty, ontologyLiteral));
                this.AddLiteral(ontologyLiteral);
            }
            return this;
        }
        #endregion

        #region Remove
        /// <summary>
        /// Removes the given fact from the data (it also deletes its orphaned taxonomies and annotations)
        /// </summary>
        public RDFOntologyData RemoveFact(RDFOntologyFact ontologyFact) {
            if (ontologyFact != null) {
                if (this.Facts.ContainsKey(ontologyFact.PatternMemberID)) {
                    this.Facts.Remove(ontologyFact.PatternMemberID);

                    //Delete fact's orphaned taxonomies
                    foreach(var tEntry in this.Relations.ClassType.SelectEntriesBySubject(ontologyFact)) {
                        this.Relations.ClassType.RemoveEntry(tEntry);
                    }
                    foreach(var tEntry in this.Relations.SameAs.SelectEntriesBySubject(ontologyFact)
                                              .UnionWith(this.Relations.SameAs.SelectEntriesByObject(ontologyFact))) {
                        this.Relations.SameAs.RemoveEntry(tEntry);
                    }
                    foreach(var tEntry in this.Relations.DifferentFrom.SelectEntriesBySubject(ontologyFact)
                                              .UnionWith(this.Relations.DifferentFrom.SelectEntriesByObject(ontologyFact))) {
                        this.Relations.DifferentFrom.RemoveEntry(tEntry);
                    }
                    foreach(var tEntry in this.Relations.Assertions.SelectEntriesBySubject(ontologyFact)
                                              .UnionWith(this.Relations.Assertions.SelectEntriesByObject(ontologyFact))) {
                        this.Relations.Assertions.RemoveEntry(tEntry);
                    }

                    //Delete fact's orphaned annotations
                    foreach(var tEntry in this.Annotations.VersionInfo.SelectEntriesBySubject(ontologyFact)) {
                        this.Annotations.VersionInfo.RemoveEntry(tEntry);
                    }
                    foreach(var tEntry in this.Annotations.VersionIRI.SelectEntriesBySubject(ontologyFact)) {
                        this.Annotations.VersionIRI.RemoveEntry(tEntry);
                    }
                    foreach(var tEntry in this.Annotations.Comment.SelectEntriesBySubject(ontologyFact)) {
                        this.Annotations.Comment.RemoveEntry(tEntry);
                    }
                    foreach(var tEntry in this.Annotations.Label.SelectEntriesBySubject(ontologyFact)) {
                        this.Annotations.Label.RemoveEntry(tEntry);
                    }
                    foreach(var tEntry in this.Annotations.SeeAlso.SelectEntriesBySubject(ontologyFact)
                                              .UnionWith(this.Annotations.SeeAlso.SelectEntriesByObject(ontologyFact))) {
                        this.Annotations.SeeAlso.RemoveEntry(tEntry);
                    }
                    foreach(var tEntry in this.Annotations.IsDefinedBy.SelectEntriesBySubject(ontologyFact)
                                              .UnionWith(this.Annotations.IsDefinedBy.SelectEntriesByObject(ontologyFact))) {
                        this.Annotations.IsDefinedBy.RemoveEntry(tEntry);
                    }
                    foreach(var tEntry in this.Annotations.CustomAnnotations.SelectEntriesBySubject(ontologyFact)
                                              .UnionWith(this.Annotations.CustomAnnotations.SelectEntriesByObject(ontologyFact))) {
                        this.Annotations.CustomAnnotations.RemoveEntry(tEntry);
                    }
                }
            }
            return this;
        }

        /// <summary>
        /// Removes the given literal from the data
        /// </summary>
        internal RDFOntologyData RemoveLiteral(RDFOntologyLiteral ontologyLiteral) {
            if (ontologyLiteral != null) {
                if (this.Literals.ContainsKey(ontologyLiteral.PatternMemberID)) {
                    this.Literals.Remove(ontologyLiteral.PatternMemberID);
                }
            }
            return this;
        }

        /// <summary>
        /// Removes the "ontologyFact -> owl:VersionInfo -> ontologyLiteral" annotation from the data
        /// </summary>
        public RDFOntologyData RemoveVersionInfoAnnotation(RDFOntologyFact ontologyFact, RDFOntologyLiteral ontologyLiteral) {
            if (ontologyFact != null && ontologyLiteral != null) {
                this.Annotations.VersionInfo.RemoveEntry(new RDFOntologyTaxonomyEntry(ontologyFact, RDFBASEOntology.Instance.Model.PropertyModel.SelectProperty(RDFVocabulary.OWL.VERSION_INFO.ToString()), ontologyLiteral));
            }
            return this;
        }

        /// <summary>
        /// Removes the "ontologyFact -> rdfs:comment -> ontologyLiteral" annotation from the data
        /// </summary>
        public RDFOntologyData RemoveCommentAnnotation(RDFOntologyFact ontologyFact, RDFOntologyLiteral ontologyLiteral) {
            if (ontologyFact != null && ontologyLiteral != null) {
                this.Annotations.Comment.RemoveEntry(new RDFOntologyTaxonomyEntry(ontologyFact, RDFBASEOntology.Instance.Model.PropertyModel.SelectProperty(RDFVocabulary.RDFS.COMMENT.ToString()), ontologyLiteral));
            }
            return this;
        }

        /// <summary>
        /// Removes the "ontologyFact -> rdfs:label -> ontologyLiteral" annotation from the data
        /// </summary>
        public RDFOntologyData RemoveLabelAnnotation(RDFOntologyFact ontologyFact, RDFOntologyLiteral ontologyLiteral) {
            if (ontologyFact != null && ontologyLiteral != null) {
                this.Annotations.Label.RemoveEntry(new RDFOntologyTaxonomyEntry(ontologyFact, RDFBASEOntology.Instance.Model.PropertyModel.SelectProperty(RDFVocabulary.RDFS.LABEL.ToString()), ontologyLiteral));
            }
            return this;
        }

        /// <summary>
        /// Removes the "ontologyFact -> rdfs:seeAlso -> ontologyResource" annotation from the data
        /// </summary>
        public RDFOntologyData RemoveSeeAlsoAnnotation(RDFOntologyFact ontologyFact, RDFOntologyResource ontologyResource) {
            if (ontologyFact != null && ontologyResource != null) {
                this.Annotations.SeeAlso.RemoveEntry(new RDFOntologyTaxonomyEntry(ontologyFact, RDFBASEOntology.Instance.Model.PropertyModel.SelectProperty(RDFVocabulary.RDFS.SEE_ALSO.ToString()), ontologyResource));
            }
            return this;
        }

        /// <summary>
        /// Removes the "ontologyFact -> rdfs:isDefinedBy -> ontologyResource" annotation from the data
        /// </summary>
        public RDFOntologyData RemoveIsDefinedByAnnotation(RDFOntologyFact ontologyFact, RDFOntologyResource ontologyResource) {
            if (ontologyFact != null && ontologyResource != null) {
                this.Annotations.IsDefinedBy.RemoveEntry(new RDFOntologyTaxonomyEntry(ontologyFact, RDFBASEOntology.Instance.Model.PropertyModel.SelectProperty(RDFVocabulary.RDFS.IS_DEFINED_BY.ToString()), ontologyResource));
            }
            return this;
        }

        /// <summary>
        /// Removes the "ontologyFact -> ontologyAnnotationProperty -> ontologyResource" annotation from the data
        /// </summary>
        public RDFOntologyData RemoveCustomAnnotation(RDFOntologyFact ontologyFact, RDFOntologyAnnotationProperty ontologyAnnotationProperty, RDFOntologyResource ontologyResource) {
            if (ontologyFact != null && ontologyAnnotationProperty != null && ontologyResource != null) {

                //owl:versionInfo
                if (ontologyAnnotationProperty.Equals(RDFBASEOntology.Instance.Model.PropertyModel.SelectProperty(RDFVocabulary.OWL.VERSION_INFO.ToString()))) {
                    if (ontologyResource.IsLiteral()) {
                        this.RemoveVersionInfoAnnotation(ontologyFact, (RDFOntologyLiteral)ontologyResource);
                    }
                }

                //rdfs:comment
                else if(ontologyAnnotationProperty.Equals(RDFBASEOntology.Instance.Model.PropertyModel.SelectProperty(RDFVocabulary.RDFS.COMMENT.ToString()))) {
                     if(ontologyResource.IsLiteral()) {
                        this.RemoveCommentAnnotation(ontologyFact, (RDFOntologyLiteral)ontologyResource);
                     }
                }

                //rdfs:label
                else if(ontologyAnnotationProperty.Equals(RDFBASEOntology.Instance.Model.PropertyModel.SelectProperty(RDFVocabulary.RDFS.LABEL.ToString()))) {
                     if(ontologyResource.IsLiteral()) {
                        this.RemoveLabelAnnotation(ontologyFact, (RDFOntologyLiteral)ontologyResource);
                     }
                }

                //rdfs:seeAlso
                else if(ontologyAnnotationProperty.Equals(RDFBASEOntology.Instance.Model.PropertyModel.SelectProperty(RDFVocabulary.RDFS.SEE_ALSO.ToString()))) {
                     this.RemoveSeeAlsoAnnotation(ontologyFact, ontologyResource);
                }

                //rdfs:isDefinedBy
                else if(ontologyAnnotationProperty.Equals(RDFBASEOntology.Instance.Model.PropertyModel.SelectProperty(RDFVocabulary.RDFS.IS_DEFINED_BY.ToString()))) {
                     this.RemoveIsDefinedByAnnotation(ontologyFact, ontologyResource);
                }

                //custom
                else {
                     this.Annotations.CustomAnnotations.RemoveEntry(new RDFOntologyTaxonomyEntry(ontologyFact, ontologyAnnotationProperty, ontologyResource));
                }

            }
            return this;
        }

        /// <summary>
        /// Removes the "ontologyFact -> rdf:type -> ontologyClass" relation from the data
        /// </summary>
        public RDFOntologyData RemoveClassTypeRelation(RDFOntologyFact ontologyFact, RDFOntologyClass ontologyClass) {
            if (ontologyFact != null && ontologyClass != null) {
                this.Relations.ClassType.RemoveEntry(new RDFOntologyTaxonomyEntry(ontologyFact, RDFBASEOntology.Instance.Model.PropertyModel.SelectProperty(RDFVocabulary.RDF.TYPE.ToString()), ontologyClass));
            }
            return this;
        }

        /// <summary>
        /// Removes the "aFact -> owl:sameAs -> bFact" relation from the data
        /// </summary>
        public RDFOntologyData RemoveSameAsRelation(RDFOntologyFact aFact, RDFOntologyFact bFact) {
            if (aFact != null && bFact != null) {
                this.Relations.SameAs.RemoveEntry(new RDFOntologyTaxonomyEntry(aFact, RDFBASEOntology.Instance.Model.PropertyModel.SelectProperty(RDFVocabulary.OWL.SAME_AS.ToString()), bFact));
                this.Relations.SameAs.RemoveEntry(new RDFOntologyTaxonomyEntry(bFact, RDFBASEOntology.Instance.Model.PropertyModel.SelectProperty(RDFVocabulary.OWL.SAME_AS.ToString()), aFact));
            }
            return this;
        }

        /// <summary>
        /// Removes the "aFact -> owl:differentFrom -> bFact" relation from the data
        /// </summary>
        public RDFOntologyData RemoveDifferentFromRelation(RDFOntologyFact aFact, RDFOntologyFact bFact) {
            if (aFact != null && bFact != null) {
                this.Relations.DifferentFrom.RemoveEntry(new RDFOntologyTaxonomyEntry(aFact, RDFBASEOntology.Instance.Model.PropertyModel.SelectProperty(RDFVocabulary.OWL.DIFFERENT_FROM.ToString()), bFact));
                this.Relations.DifferentFrom.RemoveEntry(new RDFOntologyTaxonomyEntry(bFact, RDFBASEOntology.Instance.Model.PropertyModel.SelectProperty(RDFVocabulary.OWL.DIFFERENT_FROM.ToString()), aFact));
            }
            return this;
        }

        /// <summary>
        /// Removes the "aFact -> objectProperty -> bFact" relation from the data
        /// </summary>
        public RDFOntologyData RemoveAssertionRelation(RDFOntologyFact aFact, RDFOntologyObjectProperty objectProperty, RDFOntologyFact bFact) {
            if (aFact != null && objectProperty != null && bFact != null) {
                this.Relations.Assertions.RemoveEntry(new RDFOntologyTaxonomyEntry(aFact, objectProperty, bFact));
            }
            return this;
        }

        /// <summary>
        /// Removes the "ontologyFact -> datatypeProperty -> ontologyLiteral" relation from the data
        /// </summary>
        public RDFOntologyData RemoveAssertionRelation(RDFOntologyFact ontologyFact, RDFOntologyDatatypeProperty datatypeProperty, RDFOntologyLiteral ontologyLiteral) {
            if (ontologyFact != null && datatypeProperty != null && ontologyLiteral != null) {
                this.Relations.Assertions.RemoveEntry(new RDFOntologyTaxonomyEntry(ontologyFact, datatypeProperty, ontologyLiteral));
            }
            return this;
        }
        #endregion

        #region Select
        /// <summary>
        /// Selects the fact represented by the given string from the data
        /// </summary>
        public RDFOntologyFact SelectFact(String fact) {
            if (fact         != null) {
                Int64 factID  = RDFModelUtilities.CreateHash(fact);
                if (this.Facts.ContainsKey(factID)) {
                    return this.Facts[factID];
                }
            }
            return null;
        }

        /// <summary>
        /// Selects the literal represented by the given string from the data
        /// </summary>
        public RDFOntologyLiteral SelectLiteral(String literal) {
            if (literal         != null) {
                Int64 literalID  = RDFModelUtilities.CreateHash(literal);
                if (this.Literals.ContainsKey(literalID)) {
                    return this.Literals[literalID];
                }
            }
            return null;
        }
        #endregion

        #region Set
        /// <summary>
        /// Builds a new intersection data from this data and a given one
        /// </summary>
        public RDFOntologyData IntersectWith(RDFOntologyData ontologyData) {
            var result        = new RDFOntologyData();
            if (ontologyData != null) {

                //Add intersection facts
                foreach (var  f in this) {
                    if  (ontologyData.Facts.ContainsKey(f.PatternMemberID)) {
                         result.AddFact(f);
                    }
                }

                //Add intersection literals
                foreach (var  l in this.Literals.Values) {
                    if  (ontologyData.Literals.ContainsKey(l.PatternMemberID)) {
                         result.AddLiteral(l);
                    }
                }

                //Add intersection relations
                result.Relations.ClassType           = this.Relations.ClassType.IntersectWith(ontologyData.Relations.ClassType);
                result.Relations.SameAs              = this.Relations.SameAs.IntersectWith(ontologyData.Relations.SameAs);
                result.Relations.DifferentFrom       = this.Relations.DifferentFrom.IntersectWith(ontologyData.Relations.DifferentFrom);
                result.Relations.Assertions          = this.Relations.Assertions.IntersectWith(ontologyData.Relations.Assertions);

                //Add intersection annotations
                result.Annotations.VersionInfo       = this.Annotations.VersionInfo.IntersectWith(ontologyData.Annotations.VersionInfo);
                result.Annotations.Comment           = this.Annotations.Comment.IntersectWith(ontologyData.Annotations.Comment);
                result.Annotations.Label             = this.Annotations.Label.IntersectWith(ontologyData.Annotations.Label);
                result.Annotations.SeeAlso           = this.Annotations.SeeAlso.IntersectWith(ontologyData.Annotations.SeeAlso);
                result.Annotations.IsDefinedBy       = this.Annotations.IsDefinedBy.IntersectWith(ontologyData.Annotations.IsDefinedBy);
                result.Annotations.CustomAnnotations = this.Annotations.CustomAnnotations.IntersectWith(ontologyData.Annotations.CustomAnnotations);

            }
            return result;
        }

        /// <summary>
        /// Builds a new union data from this data and a given one
        /// </summary>
        public RDFOntologyData UnionWith(RDFOntologyData ontologyData) {
            var result   = new RDFOntologyData();

            //Add facts from this data
            foreach (var f in this) {
                result.AddFact(f);
            }

            //Add literals from this data
            foreach (var l in this.Literals.Values) {
                result.AddLiteral(l);
            }

            //Add relations from this data
            result.Relations.ClassType           = result.Relations.ClassType.UnionWith(this.Relations.ClassType);
            result.Relations.SameAs              = result.Relations.SameAs.UnionWith(this.Relations.SameAs);
            result.Relations.DifferentFrom       = result.Relations.DifferentFrom.UnionWith(this.Relations.DifferentFrom);
            result.Relations.Assertions          = result.Relations.Assertions.UnionWith(this.Relations.Assertions);

            //Add annotations from this data
            result.Annotations.VersionInfo       = result.Annotations.VersionInfo.UnionWith(this.Annotations.VersionInfo);
            result.Annotations.Comment           = result.Annotations.Comment.UnionWith(this.Annotations.Comment);
            result.Annotations.Label             = result.Annotations.Label.UnionWith(this.Annotations.Label);
            result.Annotations.SeeAlso           = result.Annotations.SeeAlso.UnionWith(this.Annotations.SeeAlso);
            result.Annotations.IsDefinedBy       = result.Annotations.IsDefinedBy.UnionWith(this.Annotations.IsDefinedBy);
            result.Annotations.CustomAnnotations = result.Annotations.CustomAnnotations.UnionWith(this.Annotations.CustomAnnotations);

            //Manage the given data
            if (ontologyData != null) {

                //Add facts from the given data
                foreach (var  f in ontologyData) {
                    result.AddFact(f);
                }

                //Add literals from the given data
                foreach (var  l in ontologyData.Literals.Values) {
                    result.AddLiteral(l);
                }

                //Add relations from the given data
                result.Relations.ClassType           = result.Relations.ClassType.UnionWith(ontologyData.Relations.ClassType);
                result.Relations.SameAs              = result.Relations.SameAs.UnionWith(ontologyData.Relations.SameAs);
                result.Relations.DifferentFrom       = result.Relations.DifferentFrom.UnionWith(ontologyData.Relations.DifferentFrom);
                result.Relations.Assertions          = result.Relations.Assertions.UnionWith(ontologyData.Relations.Assertions);

                //Add annotations from the given data
                result.Annotations.VersionInfo       = result.Annotations.VersionInfo.UnionWith(ontologyData.Annotations.VersionInfo);
                result.Annotations.Comment           = result.Annotations.Comment.UnionWith(ontologyData.Annotations.Comment);
                result.Annotations.Label             = result.Annotations.Label.UnionWith(ontologyData.Annotations.Label);
                result.Annotations.SeeAlso           = result.Annotations.SeeAlso.UnionWith(ontologyData.Annotations.SeeAlso);
                result.Annotations.IsDefinedBy       = result.Annotations.IsDefinedBy.UnionWith(ontologyData.Annotations.IsDefinedBy);
                result.Annotations.CustomAnnotations = result.Annotations.CustomAnnotations.UnionWith(ontologyData.Annotations.CustomAnnotations);

            }
            return result;
        }

        /// <summary>
        /// Builds a new difference data from this data and a given one
        /// </summary>
        public RDFOntologyData DifferenceWith(RDFOntologyData ontologyData) {
            var result        = new RDFOntologyData();
            if (ontologyData != null) {

                //Add difference facts
                foreach (var  f in this) {
                    if  (!ontologyData.Facts.ContainsKey(f.PatternMemberID)) {
                          result.AddFact(f);
                    }
                }

                //Add difference literals
                foreach (var  l in this.Literals.Values) {
                    if  (!ontologyData.Literals.ContainsKey(l.PatternMemberID)) {
                          result.AddLiteral(l);
                    }
                }

                //Add difference relations
                result.Relations.ClassType           = this.Relations.ClassType.DifferenceWith(ontologyData.Relations.ClassType);
                result.Relations.SameAs              = this.Relations.SameAs.DifferenceWith(ontologyData.Relations.SameAs);
                result.Relations.DifferentFrom       = this.Relations.DifferentFrom.DifferenceWith(ontologyData.Relations.DifferentFrom);
                result.Relations.Assertions          = this.Relations.Assertions.DifferenceWith(ontologyData.Relations.Assertions);

                //Add difference annotations
                result.Annotations.VersionInfo       = this.Annotations.VersionInfo.DifferenceWith(ontologyData.Annotations.VersionInfo);
                result.Annotations.Comment           = this.Annotations.Comment.DifferenceWith(ontologyData.Annotations.Comment);
                result.Annotations.Label             = this.Annotations.Label.DifferenceWith(ontologyData.Annotations.Label);
                result.Annotations.SeeAlso           = this.Annotations.SeeAlso.DifferenceWith(ontologyData.Annotations.SeeAlso);
                result.Annotations.IsDefinedBy       = this.Annotations.IsDefinedBy.DifferenceWith(ontologyData.Annotations.IsDefinedBy);
                result.Annotations.CustomAnnotations = this.Annotations.CustomAnnotations.DifferenceWith(ontologyData.Annotations.CustomAnnotations);

            }
            else {

                //Add facts from this data
                foreach (var f in this) {
                    result.AddFact(f);
                }

                //Add literals from this data
                foreach (var l in this.Literals.Values) {
                    result.AddLiteral(l);
                }

                //Add relations from this data
                result.Relations.ClassType           = result.Relations.ClassType.UnionWith(this.Relations.ClassType);
                result.Relations.SameAs              = result.Relations.SameAs.UnionWith(this.Relations.SameAs);
                result.Relations.DifferentFrom       = result.Relations.DifferentFrom.UnionWith(this.Relations.DifferentFrom);
                result.Relations.Assertions          = result.Relations.Assertions.UnionWith(this.Relations.Assertions);

                //Add annotations from this data
                result.Annotations.VersionInfo       = result.Annotations.VersionInfo.UnionWith(this.Annotations.VersionInfo);
                result.Annotations.Comment           = result.Annotations.Comment.UnionWith(this.Annotations.Comment);
                result.Annotations.Label             = result.Annotations.Label.UnionWith(this.Annotations.Label);
                result.Annotations.SeeAlso           = result.Annotations.SeeAlso.UnionWith(this.Annotations.SeeAlso);
                result.Annotations.IsDefinedBy       = result.Annotations.IsDefinedBy.UnionWith(this.Annotations.IsDefinedBy);
                result.Annotations.CustomAnnotations = result.Annotations.CustomAnnotations.UnionWith(this.Annotations.CustomAnnotations);

            }
            return result;
        }
        #endregion

        #region Convert
        /// <summary>
        /// Gets a graph representation of this ontology data, exporting inferences according to the selected behavior
        /// </summary>
        public RDFGraph ToRDFGraph(RDFSemanticsEnums.RDFOntologyInferenceExportBehavior infexpBehavior) {
            var result = new RDFGraph();

            //Relations
            result     = result.UnionWith(this.Relations.SameAs.ToRDFGraph(infexpBehavior))
                               .UnionWith(this.Relations.DifferentFrom.ToRDFGraph(infexpBehavior))
                               .UnionWith(this.Relations.ClassType.ToRDFGraph(infexpBehavior))
                               .UnionWith(this.Relations.Assertions.ToRDFGraph(infexpBehavior));

            //Annotations
            result     = result.UnionWith(this.Annotations.VersionInfo.ToRDFGraph(infexpBehavior))
                               .UnionWith(this.Annotations.Comment.ToRDFGraph(infexpBehavior))
                               .UnionWith(this.Annotations.Label.ToRDFGraph(infexpBehavior))
                               .UnionWith(this.Annotations.SeeAlso.ToRDFGraph(infexpBehavior))
                               .UnionWith(this.Annotations.IsDefinedBy.ToRDFGraph(infexpBehavior))
                               .UnionWith(this.Annotations.CustomAnnotations.ToRDFGraph(infexpBehavior));

            return result;        
        }
        #endregion

        #region Reasoner
        /// <summary>
        /// Clears all the taxonomy entries marked as true semantic inferences (=RDFSemanticsEnums.RDFOntologyInferenceType.Reasoner)
        /// </summary>
        public RDFOntologyData ClearInferences() {
            var cacheRemove = new Dictionary<Int64, Object>();

            //ClassType
            foreach (var t in this.Relations.ClassType.Where(tEntry => tEntry.InferenceType == RDFSemanticsEnums.RDFOntologyInferenceType.Reasoner)) {
                cacheRemove.Add(t.TaxonomyEntryID, null);
            }
            foreach (var c in cacheRemove.Keys) { this.Relations.ClassType.Entries.Remove(c); }
            cacheRemove.Clear();

            //SameAs
            foreach (var t in this.Relations.SameAs.Where(tEntry => tEntry.InferenceType == RDFSemanticsEnums.RDFOntologyInferenceType.Reasoner)) {
                cacheRemove.Add(t.TaxonomyEntryID, null);
            }
            foreach (var c in cacheRemove.Keys) { this.Relations.SameAs.Entries.Remove(c); }
            cacheRemove.Clear();

            //DifferentFrom
            foreach (var t in this.Relations.DifferentFrom.Where(tEntry => tEntry.InferenceType == RDFSemanticsEnums.RDFOntologyInferenceType.Reasoner)) {
                cacheRemove.Add(t.TaxonomyEntryID, null);
            }
            foreach (var c in cacheRemove.Keys) { this.Relations.DifferentFrom.Entries.Remove(c); }
            cacheRemove.Clear();           

            //Assertions
            foreach (var t in this.Relations.Assertions.Where(tEntry => tEntry.InferenceType == RDFSemanticsEnums.RDFOntologyInferenceType.Reasoner)) {
                cacheRemove.Add(t.TaxonomyEntryID, null);
            }
            foreach (var c in cacheRemove.Keys) { this.Relations.Assertions.Entries.Remove(c); }
            cacheRemove.Clear();

            return this;
        }
        #endregion

        #endregion

    }

}