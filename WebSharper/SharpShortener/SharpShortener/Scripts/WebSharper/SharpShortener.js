(function()
{
 var Global=this,Runtime=this.IntelliFactory.Runtime,WebSharper,Option,SharpShortener,Addresse,Url,Hash,Strings,ShortenPagelet,StatsPagelet,List,Seq,Operators,String,T,Collections,MapModule,Html,Default,Operators1,Concurrency,Remoting,EventsPervasives,alert,T1,google,visualization,ColumnChart,DataTable,Arrays,T2,RegExp;
 Runtime.Define(Global,{
  SharpShortener:{
   Addresse:{
    externFromString:function(baseAdr,url)
    {
     return Option.bind(function(url1)
     {
      return Addresse.externFromUrl(baseAdr,url1);
     },Url.fromString(url));
    },
    externFromUrl:function(baseAdr,url)
    {
     return Addresse.isLink(baseAdr,url)?{
      $:0
     }:{
      $:1,
      $0:{
       $:0,
       $0:url
      }
     };
    },
    externToUrl:function(_arg1)
    {
     return _arg1.$0;
    },
    getId:function(_arg1)
    {
     return _arg1.$0;
    },
    isLink:function(baseAdr,url)
    {
     return Addresse.linkFromUrl(baseAdr,url).$==1;
    },
    link:function(id)
    {
     return{
      $:0,
      $0:id
     };
    },
    linkFromString:function(baseAdr,url)
    {
     return Option.bind(function(url1)
     {
      return Addresse.linkFromUrl(baseAdr,url1);
     },Url.fromString(url));
    },
    linkFromUrl:function(baseAdr,url)
    {
     return Option.map(function(arg0)
     {
      return{
       $:0,
       $0:arg0
      };
     },Option.bind(function(hash)
     {
      return Hash.stringToId(hash);
     },Addresse.tryGetHash(baseAdr,url)));
    },
    linkToUrl:function(baseUri,_arg1)
    {
     return Url.fromString(baseUri+Global.String(Hash.fromId(_arg1.$0))).$0;
    },
    tryGetHash:function(baseAdr,url)
    {
     var url1,url2;
     url1=Global.String(url);
     if(Strings.StartsWith(url1,baseAdr))
      {
       return{
        $:1,
        $0:url1.substring(baseAdr.length)
       };
      }
     else
      {
       url2="http://"+url1;
       return Strings.StartsWith(url2,baseAdr)?{
        $:1,
        $0:url2.substring(baseAdr.length)
       }:{
        $:0
       };
      }
    }
   },
   Controls:{
    Shorten:Runtime.Class({
     get_Body:function()
     {
      return ShortenPagelet.Main();
     }
    }),
    Stats:Runtime.Class({
     get_Body:function()
     {
      return StatsPagelet.Main();
     }
    })
   },
   Hash:{
    T:Runtime.Class({
     toString:function()
     {
      return this.$0;
     }
    }),
    alphabet:Runtime.Field(function()
    {
     return List.concat(List.ofArray([Seq.toList(Operators.range(65,90)),Seq.toList(Operators.range(48,57))]));
    }),
    alphabetLen:Runtime.Field(function()
    {
     return Hash.alphabet().get_Length();
    }),
    fromId:function(id)
    {
     var output,n;
     output="";
     n=id;
     while(n>0)
      {
       output=output+String.fromCharCode(Hash.alphabet().get_Item(n%Hash.alphabetLen()<<0));
       n=n/Hash.alphabetLen()>>0;
      }
     return Runtime.New(T,{
      $:0,
      $0:output
     });
    },
    indexOf:Runtime.Field(function()
    {
     var map;
     map=MapModule.OfArray(Seq.toArray(List.zip(Hash.alphabet(),Seq.toList(Operators.range(0,Hash.alphabet().get_Length()-1)))));
     return function(c)
     {
      return map.get_Item(c);
     };
    }),
    stringToId:function(hash)
    {
     var matchValue;
     if(hash.indexOf("/")!=-1)
      {
       return{
        $:0
       };
      }
     else
      {
       try
       {
        return{
         $:1,
         $0:List.foldBack(function(c)
         {
          return function(n)
          {
           return n*Hash.alphabetLen()+(Hash.indexOf())(c);
          };
         },List.ofSeq(hash),0)
        };
       }
       catch(matchValue)
       {
        return{
         $:0
        };
       }
      }
    },
    toId:function(_arg1)
    {
     return Hash.stringToId(_arg1.$0).$0;
    }
   },
   ShortenPagelet:{
    Main:function()
    {
     var input,button;
     input=Default.Input(List.ofArray([Default.Text("")]));
     button=Default.Button(List.ofArray([Default.Text("Kürzen")]));
     ShortenPagelet.wireUp(button,input);
     return Operators1.add(Default.Div(List.ofArray([input,button])),List.ofArray([Default.Attr().Class("eingabe")]));
    },
    enableElement:function(e,enable)
    {
     return!enable?e["HtmlProvider@32"].SetAttribute(e.Body,"disabled",""):e["HtmlProvider@32"].RemoveAttribute(e.Body,"disabled");
    },
    getValue:function(f,e)
    {
     return f(e.get_Value());
    },
    requestShortenedUrl:function(cont,url)
    {
     return Concurrency.Start(Concurrency.Delay(function()
     {
      return Concurrency.Bind(Remoting.Async("SharpShortener:0",[url]),function(arg101)
      {
       return Concurrency.Return(cont(arg101));
      });
     }));
    },
    setText:function(e,text)
    {
     return e.set_Text(text);
    },
    setValue:function(f,e,value)
    {
     return e.set_Value(f(value));
    },
    wireUp:function(button,input)
    {
     var getUrl,f,f1,onTextChange,arg00,arg001,arg002;
     getUrl=function(_arg20_)
     {
      return ShortenPagelet.getValue(function(url)
      {
       return Url.fromString(url);
      },input,_arg20_);
     };
     f=function(u)
     {
      return Global.String(u);
     };
     f1=function()
     {
      return"";
     };
     onTextChange=function()
     {
      if(getUrl(null).$==0)
       {
        ShortenPagelet.enableElement(button,false);
        return ShortenPagelet.setText(button,"...");
       }
      else
       {
        ShortenPagelet.enableElement(button,true);
        return ShortenPagelet.setText(button,"kürzen");
       }
     };
     arg00=function()
     {
      return function()
      {
       var matchValue,url,cont;
       matchValue=getUrl(null);
       if(matchValue.$==0)
        {
         return null;
        }
       else
        {
         url=matchValue.$0;
         cont=function(linkUrl)
         {
          ShortenPagelet.enableElement(button,true);
          ShortenPagelet.enableElement(input,true);
          if(linkUrl.$==0)
           {
            ShortenPagelet.setValue(f1,input,null);
           }
          else
           {
            ShortenPagelet.setValue(f,input,linkUrl.$0);
            input.Body.select();
           }
          return onTextChange(null);
         };
         ShortenPagelet.enableElement(button,false);
         ShortenPagelet.enableElement(input,false);
         return ShortenPagelet.requestShortenedUrl(cont,url);
        }
      };
     };
     EventsPervasives.Events().OnClick(arg00,button);
     arg001=function()
     {
      return onTextChange(null);
     };
     EventsPervasives.Events().OnChange(arg001,input);
     arg002=function()
     {
      return function()
      {
       return onTextChange(null);
      };
     };
     EventsPervasives.Events().OnKeyUp(arg002,input);
     return ShortenPagelet.enableElement(button,false);
    }
   },
   StatsPagelet:{
    Main:function()
    {
     var x,btn;
     x=Default.Button(List.ofArray([Default.Text("Test")]));
     Operators1.OnAfterRender(function(el)
     {
      return el.Body.addEventListener("click",function()
      {
       return alert("Hallo");
      },false);
     },x);
     btn=x;
     btn.Body.addEventListener("click",function()
     {
      return alert("Welt");
     },false);
     return Operators1.add(Default.Div(List.ofArray([Default.H2(List.ofArray([Default.Text("Besucherzähler der registrierten Seiten")])),StatsPagelet.StatsChart(),btn])),List.ofArray([Default.Attr().Class("statistik")]));
    },
    StatsChart:function()
    {
     var x;
     x=Default.Div(Runtime.New(T1,{
      $:0
     }));
     Operators1.OnAfterRender(function(container)
     {
      var returnVal;
      returnVal={};
      returnVal.position="bottom";
      return StatsPagelet.getStatsData(new ColumnChart(container.Body),{
       width:600,
       height:300,
       legend:returnVal,
       title:"Hitcounter"
      });
     },x);
     return x;
    },
    getStatsData:function(drawTo,options)
    {
     return Concurrency.Start(Concurrency.Delay(function()
     {
      var stats,data;
      stats=Remoting.Call("SharpShortener:1",[]);
      data=new DataTable();
      data.addColumn("string","url");
      data.addColumn("number","#hits");
      Arrays.iter(Runtime.Tupled(function(tupledArg)
      {
       var url,anz,i;
       url=tupledArg[0];
       anz=tupledArg[1];
       i=data.addRow();
       data.setValue(i,0,Url.nameOnly(url));
       return data.setValue(i,1,anz);
      }),stats);
      drawTo.draw(data,options);
      return Concurrency.Return(null);
     }));
    }
   },
   Url:{
    T:Runtime.Class({
     toString:function()
     {
      return this.$0;
     }
    }),
    fromString:function(url)
    {
     return Url.isValidUrl(url.toLowerCase())?{
      $:1,
      $0:Runtime.New(T2,{
       $:0,
       $0:Url.withProtocol(url)
      })
     }:{
      $:0
     };
    },
    isValidUrl:function(url)
    {
     return(new RegExp(Url.pattern())).test(url);
    },
    nameOnly:function(_arg1)
    {
     return Url.withoutWWW(Url.withoutProtocol(Global.String(_arg1.$0).toLowerCase()));
    },
    pattern:Runtime.Field(function()
    {
     return"^(https?:\\/\\/)?(www\\.)?(\\w+.?)+$";
    }),
    withProtocol:function(url)
    {
     return(Strings.StartsWith(url,"http://")?true:Strings.StartsWith(url,"https://"))?url:"http://"+url;
    },
    withoutProtocol:function(url)
    {
     return Strings.Replace(Strings.Replace(url,"http://",""),"https://","");
    },
    withoutWWW:function(url)
    {
     return Strings.Replace(url,"www.","");
    }
   }
  }
 });
 Runtime.OnInit(function()
 {
  WebSharper=Runtime.Safe(Global.IntelliFactory.WebSharper);
  Option=Runtime.Safe(WebSharper.Option);
  SharpShortener=Runtime.Safe(Global.SharpShortener);
  Addresse=Runtime.Safe(SharpShortener.Addresse);
  Url=Runtime.Safe(SharpShortener.Url);
  Hash=Runtime.Safe(SharpShortener.Hash);
  Strings=Runtime.Safe(WebSharper.Strings);
  ShortenPagelet=Runtime.Safe(SharpShortener.ShortenPagelet);
  StatsPagelet=Runtime.Safe(SharpShortener.StatsPagelet);
  List=Runtime.Safe(WebSharper.List);
  Seq=Runtime.Safe(WebSharper.Seq);
  Operators=Runtime.Safe(WebSharper.Operators);
  String=Runtime.Safe(Global.String);
  T=Runtime.Safe(Hash.T);
  Collections=Runtime.Safe(WebSharper.Collections);
  MapModule=Runtime.Safe(Collections.MapModule);
  Html=Runtime.Safe(WebSharper.Html);
  Default=Runtime.Safe(Html.Default);
  Operators1=Runtime.Safe(Html.Operators);
  Concurrency=Runtime.Safe(WebSharper.Concurrency);
  Remoting=Runtime.Safe(WebSharper.Remoting);
  EventsPervasives=Runtime.Safe(Html.EventsPervasives);
  alert=Runtime.Safe(Global.alert);
  T1=Runtime.Safe(List.T);
  google=Runtime.Safe(Global.google);
  visualization=Runtime.Safe(google.visualization);
  ColumnChart=Runtime.Safe(visualization.ColumnChart);
  DataTable=Runtime.Safe(visualization.DataTable);
  Arrays=Runtime.Safe(WebSharper.Arrays);
  T2=Runtime.Safe(Url.T);
  return RegExp=Runtime.Safe(Global.RegExp);
 });
 Runtime.OnLoad(function()
 {
  Url.pattern();
  Hash.indexOf();
  Hash.alphabetLen();
  Hash.alphabet();
  return;
 });
}());
