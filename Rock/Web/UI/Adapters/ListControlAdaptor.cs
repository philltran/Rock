﻿// <copyright>
// Copyright by the Spark Development Network
//
// Licensed under the Rock Community License (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.rockrms.com/license
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
//
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.Adapters;

namespace Rock.Web.UI.Adapters
{
    /// <summary>
    /// Abstract adapter for checkboxlist and radiobuttonlist adaptors
    /// </summary>
    public abstract class ListControlAdaptor : WebControlAdapter
    {
        /// <summary>
        /// Creates the beginning tag for the Web control in the markup that is transmitted to the target browser.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Web.UI.HtmlTextWriter" /> containing methods to render the target-specific output.</param>
        protected override void RenderBeginTag( System.Web.UI.HtmlTextWriter writer )
        {
            // Preserve any classes that a developer put on the control (such as a "well") by wrapping it in a <div>.
            ListControl listControl = Control as ListControl;
            if ( listControl != null && !string.IsNullOrEmpty( listControl.CssClass ) )
            {
                writer.AddAttribute( "class", listControl.CssClass );
                writer.RenderBeginTag( HtmlTextWriterTag.Div );
            }
        }

        /// <summary>
        /// Creates the ending tag for the Web control in the markup that is transmitted to the target browser.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Web.UI.HtmlTextWriter" /> containing methods to render the target-specific output.</param>
        protected override void RenderEndTag( System.Web.UI.HtmlTextWriter writer )
        {
            // Close the <div> tag we may have started in the BeginTag above.
            ListControl listControl = Control as ListControl;
            if ( listControl != null && !string.IsNullOrEmpty( listControl.CssClass ) )
            {
                writer.RenderEndTag();
            }
        }

        /// <summary>
        /// Gets the repeat direction.
        /// </summary>
        /// <param name="listControl">The list control.</param>
        /// <returns></returns>
        protected abstract RepeatDirection GetRepeatDirection( ListControl listControl );

        /// <summary>
        /// Gets the repeat columns.
        /// </summary>
        /// <param name="listControl">The list control.</param>
        /// <returns></returns>
        public abstract int GetRepeatColumns( ListControl listControl );

        /// <summary>
        /// Gets the type of the input tag.
        /// </summary>
        /// <param name="listControl">The list control.</param>
        /// <returns></returns>
        public abstract string GetInputTagType( ListControl listControl );

        /// <summary>
        /// Gets the name of the input.
        /// </summary>
        /// <param name="listControl">The list control.</param>
        /// <param name="itemIndex">Index of the item.</param>
        /// <returns></returns>
        public abstract string GetInputName( ListControl listControl, int itemIndex );

        /// <summary>
        /// Generates the target-specific inner markup for the Web control to which the control adapter is attached.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Web.UI.HtmlTextWriter" /> containing methods to render the target-specific output.</param>
        protected override void RenderContents( System.Web.UI.HtmlTextWriter writer )
        {
            ListControl listControl = Control as ListControl;
            if ( listControl != null )
            {
                string postBackEventReference = null;

                if ( listControl.AutoPostBack )
                {
                    var postBackOption = new PostBackOptions( listControl, string.Empty );
                    if ( listControl.CausesValidation && this.Page.GetValidators( listControl.ValidationGroup ).Count > 0 )
                    {
                        postBackOption.PerformValidation = true;
                        postBackOption.ValidationGroup = listControl.ValidationGroup;
                    }

                    if ( this.Page.Form != null )
                    {
                        postBackOption.AutoPostBack = true;
                    }

                    postBackEventReference = Page.ClientScript.GetPostBackEventReference( postBackOption, true );
                }

                string labelClass;
                bool createInputDivClass;
                string inputTagType = GetInputTagType( listControl );

                if ( GetRepeatDirection(listControl) == RepeatDirection.Horizontal )
                {
                    // if Horizontal, put checkbox/radio-inline on the label tag, and don't create a <div class="checkbox/radio"> wrapper
                    labelClass = $"{inputTagType}-inline";
                    createInputDivClass = false;
                }
                else
                {
                    // if Vertical, leave the label class empty, and create a <div class="checkbox/radio"> wrapper
                    labelClass = string.Empty;
                    createInputDivClass = true;
                }

                if ( !listControl.Enabled )
                {
                    labelClass += " text-muted";
                }

                int repeatColumns = GetRepeatColumns( listControl );
                
                bool wrapInRow = repeatColumns > 1;

                if ( wrapInRow )
                {
                    // if there are multiple columns, RepeatDirection doesn't matter, either way we want it to Left To Right as A | B | C | D |.  
                    writer.WriteLine();
                    writer.Indent++;
                    writer.AddAttribute( "class", "row" );
                    writer.RenderBeginTag( HtmlTextWriterTag.Div );
                }

                string columnClass = string.Empty;

                if ( wrapInRow )
                {
                    switch ( repeatColumns )
                    {
                        case 2:
                            columnClass = "col-md-6";
                            break;
                        case 3:
                            columnClass = "col-sm-6 col-md-4";
                            break;
                        case 4:
                            columnClass = "col-sm-6 col-md-3";
                            break;
                        case 6:
                            columnClass = "col-sm-4 col-md-2";
                            break;
                        default:
                            columnClass = "col-sm-4 col-md-2";
                            break;
                    }
                }

                int itemIndex = 0;
                foreach ( ListItem li in listControl.Items )
                {
                    if ( wrapInRow )
                    {
                        writer.WriteLine();
                        writer.Indent++;
                        writer.AddAttribute( "class", columnClass );
                        writer.RenderBeginTag( HtmlTextWriterTag.Div );
                    }

                    if ( createInputDivClass )
                    {
                        writer.WriteLine();
                        writer.Indent++;
                        writer.AddAttribute( "class", inputTagType );
                        writer.RenderBeginTag( HtmlTextWriterTag.Div );
                    }

                    // render checkbox/radio label tag which will contain the input and label text
                    writer.WriteLine();
                    writer.Indent++;
                    writer.AddAttribute( "class", labelClass );
                    writer.RenderBeginTag( HtmlTextWriterTag.Label );

                    string itemId = $"{listControl.ClientID}_{itemIndex}";
                    writer.AddAttribute( "id", itemId );
                    writer.AddAttribute( "type", inputTagType );
                    var inputName = GetInputName( listControl, itemIndex );
                    itemIndex++;
                    writer.AddAttribute( "name", inputName );
                    writer.AddAttribute( "value", li.Value );
                    if ( li.Selected )
                    {
                        writer.AddAttribute( "checked", "checked" );
                    }

                    if ( !listControl.Enabled )
                    {
                        writer.AddAttribute( "disabled", string.Empty );
                    }

                    foreach ( var attributeKey in li.Attributes.Keys )
                    {
                        var key = attributeKey as string;
                        writer.AddAttribute( key, li.Attributes[key] );
                    }

                    if ( postBackEventReference != null )
                    {
                        string itemPostBackEventReference = postBackEventReference.Replace( listControl.UniqueID, inputName );
                        writer.AddAttribute( HtmlTextWriterAttribute.Onclick, itemPostBackEventReference );
                    }

                    writer.WriteLine();
                    writer.Indent++;
                    writer.RenderBeginTag( HtmlTextWriterTag.Input );
                    writer.RenderEndTag();
                    writer.Indent--;

                    writer.WriteLine();
                    writer.Indent++;
                    writer.AddAttribute( HtmlTextWriterAttribute.Class, "label-text" );
                    writer.RenderBeginTag( HtmlTextWriterTag.Span );

                    writer.Write( li.Text );

                    writer.RenderEndTag();      // Span
                    writer.Indent--;

                    writer.RenderEndTag();      // Label
                    writer.Indent--;

                    if ( createInputDivClass )
                    {
                        writer.RenderEndTag();   // checkbox/radio div
                        writer.Indent--;
                    }

                    if ( wrapInRow )
                    {
                        writer.RenderEndTag();   // col div
                        writer.Indent--;
                    }

                    if ( Page != null && Page.ClientScript != null )
                    {
                        Page.ClientScript.RegisterForEventValidation( listControl.UniqueID, li.Value );
                    }
                }

                if ( wrapInRow )
                {
                    writer.RenderEndTag();   // row div
                    writer.Indent--;
                }

                if ( Page != null && Page.ClientScript != null )
                {
                    Page.ClientScript.RegisterForEventValidation( listControl.UniqueID );
                }
            }
        }
    }
}