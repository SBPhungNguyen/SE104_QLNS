﻿#pragma checksum "..\..\BookInfoPopup.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "9EEFA24A9A663C8EB1F668026FDA20C96267FD5E6F169DFABD39A549230E3FED"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using SE104_QLNS;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace SE104_QLNS {
    
    
    /// <summary>
    /// BookInfoPopup
    /// </summary>
    public partial class BookInfoPopup : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 27 "..\..\BookInfoPopup.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Border bdr_BookImage;
        
        #line default
        #line hidden
        
        
        #line 29 "..\..\BookInfoPopup.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Media.ImageBrush img_BookImage;
        
        #line default
        #line hidden
        
        
        #line 48 "..\..\BookInfoPopup.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock tbl_BookID;
        
        #line default
        #line hidden
        
        
        #line 52 "..\..\BookInfoPopup.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock tbl_BookName;
        
        #line default
        #line hidden
        
        
        #line 56 "..\..\BookInfoPopup.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock tbl_Gerne;
        
        #line default
        #line hidden
        
        
        #line 60 "..\..\BookInfoPopup.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock tbl_Author;
        
        #line default
        #line hidden
        
        
        #line 64 "..\..\BookInfoPopup.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock tbl_ImportPrice;
        
        #line default
        #line hidden
        
        
        #line 68 "..\..\BookInfoPopup.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock tbl_ExportPrice;
        
        #line default
        #line hidden
        
        
        #line 72 "..\..\BookInfoPopup.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock tbl_Quantity;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/SE104_QLNS;component/bookinfopopup.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\BookInfoPopup.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.bdr_BookImage = ((System.Windows.Controls.Border)(target));
            return;
            case 2:
            this.img_BookImage = ((System.Windows.Media.ImageBrush)(target));
            return;
            case 3:
            this.tbl_BookID = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 4:
            this.tbl_BookName = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 5:
            this.tbl_Gerne = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 6:
            this.tbl_Author = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 7:
            this.tbl_ImportPrice = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 8:
            this.tbl_ExportPrice = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 9:
            this.tbl_Quantity = ((System.Windows.Controls.TextBlock)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}
