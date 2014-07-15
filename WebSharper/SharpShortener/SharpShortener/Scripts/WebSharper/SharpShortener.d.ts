declare module SharpShortener {
    module Skin {
        interface Page {
            Title: string;
            Body: __ABBREV.__List.T<any>;
        }
    }
    module Controls {
        interface Shorten {
            get_Body(): __ABBREV.__Html.IPagelet;
        }
        interface Stats {
            get_Body(): __ABBREV.__Html.IPagelet;
        }
    }
    module StatsPagelet {
        var getStatsData : {
            (drawTo: __ABBREV.__visualization.ColumnChart, options: __ABBREV.__Visualization.ColumnChartOptions): void;
        };
        var StatsChart : {
            (): __ABBREV.__Html.Element;
        };
        var Main : {
            (): __ABBREV.__Html.Element;
        };
    }
    module ShortenPagelet {
        var requestShortenedUrl : {
            (cont: {
                (x: __ABBREV.__WebSharper.OptionProxy<__ABBREV.__Url.T>): void;
            }, url: __ABBREV.__Url.T): void;
        };
        var enableElement : {
            (e: __ABBREV.__Html.Element, enable: boolean): void;
        };
        var setText : {
            (e: __ABBREV.__Html.Element, text: string): void;
        };
        var setValue : {
            <_M1>(f: {
                (x: _M1): string;
            }, e: __ABBREV.__Html.Element, value: _M1): void;
        };
        var getValue : {
            <_M1>(f: {
                (x: string): _M1;
            }, e: __ABBREV.__Html.Element, unitVar0: void): _M1;
        };
        var wireUp : {
            (button: __ABBREV.__Html.Element, input: __ABBREV.__Html.Element): void;
        };
        var Main : {
            (): __ABBREV.__Html.Element;
        };
    }
    module Url {
        interface T {
            toString(): string;
        }
        var isValidUrl : {
            (url: string): boolean;
        };
        var withProtocol : {
            (url: string): string;
        };
        var withoutProtocol : {
            (url: string): string;
        };
        var withoutWWW : {
            (url: string): string;
        };
        var fromString : {
            (url: string): __ABBREV.__WebSharper.OptionProxy<__ABBREV.__Url.T>;
        };
        var nameOnly : {
            (_arg1: __ABBREV.__Url.T): string;
        };
        var pattern : {
            (): string;
        };
    }
    module Addresse {
        interface Link {
        }
        interface Extern {
        }
        var link : {
            (id: number): __ABBREV.__Addresse.Link;
        };
        var getId : {
            (_arg1: __ABBREV.__Addresse.Link): number;
        };
        var tryGetHash : {
            (baseAdr: string, url: __ABBREV.__Url.T): __ABBREV.__WebSharper.OptionProxy<string>;
        };
        var linkFromUrl : {
            (baseAdr: string, url: __ABBREV.__Url.T): __ABBREV.__WebSharper.OptionProxy<__ABBREV.__Addresse.Link>;
        };
        var linkFromString : {
            (baseAdr: string, url: string): __ABBREV.__WebSharper.OptionProxy<__ABBREV.__Addresse.Link>;
        };
        var linkToUrl : {
            (baseUri: string, _arg1: __ABBREV.__Addresse.Link): __ABBREV.__Url.T;
        };
        var isLink : {
            (baseAdr: string, url: __ABBREV.__Url.T): boolean;
        };
        var externFromUrl : {
            (baseAdr: string, url: __ABBREV.__Url.T): __ABBREV.__WebSharper.OptionProxy<__ABBREV.__Addresse.Extern>;
        };
        var externFromString : {
            (baseAdr: string, url: string): __ABBREV.__WebSharper.OptionProxy<__ABBREV.__Addresse.Extern>;
        };
        var externToUrl : {
            (_arg1: __ABBREV.__Addresse.Extern): __ABBREV.__Url.T;
        };
    }
    module Hash {
        interface T {
            toString(): string;
        }
        var fromId : {
            (id: number): __ABBREV.__Hash.T;
        };
        var stringToId : {
            (hash: string): __ABBREV.__WebSharper.OptionProxy<number>;
        };
        var toId : {
            (_arg1: __ABBREV.__Hash.T): number;
        };
        var alphabet : {
            (): __ABBREV.__List.T<number>;
        };
        var alphabetLen : {
            (): number;
        };
        var indexOf : {
            (): {
                (x: number): number;
            };
        };
    }
    interface Aktion {
    }
    interface Website {
    }
}
declare module __ABBREV {
    
    export import __List = IntelliFactory.WebSharper.List;
    export import __Html = IntelliFactory.WebSharper.Html;
    export import __visualization = google.visualization;
    export import __Visualization = IntelliFactory.WebSharper.Google.Visualization;
    export import __WebSharper = IntelliFactory.WebSharper;
    export import __Url = SharpShortener.Url;
    export import __Addresse = SharpShortener.Addresse;
    export import __Hash = SharpShortener.Hash;
}
