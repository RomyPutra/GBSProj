/* Start jason.js */

/*
json2.js
2007-12-02

Public Domain

No warranty expressed or implied. Use at your own risk.

See http://www.JSON.org/js.html

This file creates a global JSON object containing two methods:

JSON.stringify(value, whitelist)
value       any JavaScript value, usually an object or array.

whitelist   an optional array prameter that determines how object
values are stringified.

This method produces a JSON text from a JavaScript value.
There are three possible ways to stringify an object, depending
on the optional whitelist parameter.

If an object has a toJSON method, then the toJSON() method will be
called. The value returned from the toJSON method will be
stringified.

Otherwise, if the optional whitelist parameter is an array, then
the elements of the array will be used to select members of the
object for stringification.

Otherwise, if there is no whitelist parameter, then all of the
members of the object will be stringified.

Values that do not have JSON representaions, such as undefined or
functions, will not be serialized. Such values in objects will be
dropped; in arrays will be replaced with null.
JSON.stringify(undefined) returns undefined. Dates will be
stringified as quoted ISO dates.

Example:

var text = JSON.stringify(['e', {pluribus: 'unum'}]);
// text is '["e",{"pluribus":"unum"}]'

JSON.parse(text, filter)
This method parses a JSON text to produce an object or
array. It can throw a SyntaxError exception.

The optional filter parameter is a function that can filter and
transform the results. It receives each of the keys and values, and
its return value is used instead of the original value. If it
returns what it received, then structure is not modified. If it
returns undefined then the member is deleted.

Example:

// Parse the text. If a key contains the string 'date' then
// convert the value to a date.

myData = JSON.parse(text, function (key, value) {
return key.indexOf('date') >= 0 ? new Date(value) : value;
});

This is a reference implementation. You are free to copy, modify, or
redistribute.

Use your own copy. It is extremely unwise to load third party
code into your pages.
*/

/*jslint evil: true */

/*global JSON */

/*members "\b", "\t", "\n", "\f", "\r", "\"", JSON, "\\", apply,
charCodeAt, floor, getUTCDate, getUTCFullYear, getUTCHours,
getUTCMinutes, getUTCMonth, getUTCSeconds, hasOwnProperty, join, length,
parse, propertyIsEnumerable, prototype, push, replace, stringify, test,
toJSON, toString
*/

if (!this.JSON) {

    JSON = function() {

        function f(n) {    // Format integers to have at least two digits.
            return n < 10 ? '0' + n : n;
        }

        Date.prototype.toJSON = function() {

            // Eventually, this method will be based on the date.toISOString method.

            return this.getUTCFullYear() + '-' +
                 f(this.getUTCMonth() + 1) + '-' +
                 f(this.getUTCDate()) + 'T' +
                 f(this.getUTCHours()) + ':' +
                 f(this.getUTCMinutes()) + ':' +
                 f(this.getUTCSeconds()) + 'Z';
        };


        var m = {    // table of character substitutions
            '\b': '\\b',
            '\t': '\\t',
            '\n': '\\n',
            '\f': '\\f',
            '\r': '\\r',
            '"': '\\"',
            '\\': '\\\\'
        };

        function stringify(value, whitelist) {
            var a,          // The array holding the partial texts.
                i,          // The loop counter.
                k,          // The member key.
                l,          // Length.
                r = /["\\\x00-\x1f\x7f-\x9f]/g,
                v;          // The member value.

            switch (typeof value) {
                case 'string':

                    // If the string contains no control characters, no quote characters, and no
                    // backslash characters, then we can safely slap some quotes around it.
                    // Otherwise we must also replace the offending characters with safe sequences.

                    return r.test(value) ?
                    '"' + value.replace(r, function(a) {
                        var c = m[a];
                        if (c) {
                            return c;
                        }
                        c = a.charCodeAt();
                        return '\\u00' + Math.floor(c / 16).toString(16) +
                                                   (c % 16).toString(16);
                    }) + '"' :
                    '"' + value + '"';

                case 'number':

                    // JSON numbers must be finite. Encode non-finite numbers as null.

                    return isFinite(value) ? String(value) : 'null';

                case 'boolean':
                case 'null':
                    return String(value);

                case 'object':

                    // Due to a specification blunder in ECMAScript,
                    // typeof null is 'object', so watch out for that case.

                    if (!value) {
                        return 'null';
                    }

                    // If the object has a toJSON method, call it, and stringify the result.

                    if (typeof value.toJSON === 'function') {
                        return stringify(value.toJSON());
                    }
                    a = [];
                    if (typeof value.length === 'number' &&
                        !(value.propertyIsEnumerable('length'))) {

                        // The object is an array. Stringify every element. Use null as a placeholder
                        // for non-JSON values.

                        l = value.length;
                        for (i = 0; i < l; i += 1) {
                            a.push(stringify(value[i], whitelist) || 'null');
                        }

                        // Join all of the elements together and wrap them in brackets.

                        return '[' + a.join(',') + ']';
                    }
                    if (whitelist) {

                        // If a whitelist (array of keys) is provided, use it to select the components
                        // of the object.

                        l = whitelist.length;
                        for (i = 0; i < l; i += 1) {
                            k = whitelist[i];
                            if (typeof k === 'string') {
                                v = stringify(value[k], whitelist);
                                if (v) {
                                    a.push(stringify(k) + ':' + v);
                                }
                            }
                        }
                    } else {

                        // Otherwise, iterate through all of the keys in the object.

                        for (k in value) {
                            if (typeof k === 'string') {
                                v = stringify(value[k], whitelist);
                                if (v) {
                                    a.push(stringify(k) + ':' + v);
                                }
                            }
                        }
                    }

                    // Join all of the member texts together and wrap them in braces.

                    return '{' + a.join(',') + '}';
            }
        }

        return {
            stringify: stringify,
            parse: function(text, filter) {
                var j;

                function walk(k, v) {
                    var i, n;
                    if (v && typeof v === 'object') {
                        for (i in v) {
                            if (Object.prototype.hasOwnProperty.apply(v, [i])) {
                                n = walk(i, v[i]);
                                if (n !== undefined) {
                                    v[i] = n;
                                }
                            }
                        }
                    }
                    return filter(k, v);
                }


                // Parsing happens in three stages. In the first stage, we run the text against
                // regular expressions that look for non-JSON patterns. We are especially
                // concerned with '()' and 'new' because they can cause invocation, and '='
                // because it can cause mutation. But just to be safe, we want to reject all
                // unexpected forms.

                // We split the first stage into 4 regexp operations in order to work around
                // crippling inefficiencies in IE's and Safari's regexp engines. First we
                // replace all backslash pairs with '@' (a non-JSON character). Second, we
                // replace all simple value tokens with ']' characters. Third, we delete all
                // open brackets that follow a colon or comma or that begin the text. Finally,
                // we look to see that the remaining characters are only whitespace or ']' or
                // ',' or ':' or '{' or '}'. If that is so, then the text is safe for eval.

                if (/^[\],:{}\s]*$/.test(text.replace(/\\./g, '@').
replace(/"[^"\\\n\r]*"|true|false|null|-?\d+(?:\.\d*)?(:?[eE][+\-]?\d+)?/g, ']').
replace(/(?:^|:|,)(?:\s*\[)+/g, ''))) {

                    // In the second stage we use the eval function to compile the text into a
                    // JavaScript structure. The '{' operator is subject to a syntactic ambiguity
                    // in JavaScript: it can begin a block or an object literal. We wrap the text
                    // in parens to eliminate the ambiguity.

                    j = eval('(' + text + ')');

                    // In the optional third stage, we recursively walk the new structure, passing
                    // each name/value pair to a filter function for possible transformation.

                    return typeof filter === 'function' ? walk('', j) : j;
                }

                // If the text is not JSON parseable, then a SyntaxError is thrown.

                throw new SyntaxError('parseJSON');
            }
        };
    } ();
}
/* End jason.js */
/* Start jquery.js */
(function() {
    /*
    * jQuery 1.2.1 - New Wave Javascript
    *
    * Copyright (c) 2007 John Resig (jquery.com)
    * Dual licensed under the MIT (MIT-LICENSE.txt)
    * and GPL (GPL-LICENSE.txt) licenses.
    *
    * $Date: 2007-09-16 23:42:06 -0400 (Sun, 16 Sep 2007) $
    * $Rev: 3353 $
    */

    // Map over jQuery in case of overwrite
    if (typeof jQuery != "undefined")
        var _jQuery = jQuery;

    var jQuery = window.jQuery = function(selector, context) {
        // If the context is a namespace object, return a new object
        return this instanceof jQuery ?
		this.init(selector, context) :
		new jQuery(selector, context);
    };

    // Map over the $ in case of overwrite
    if (typeof $ != "undefined")
        var _$ = $;

    // Map the jQuery namespace to the '$' one
    window.$ = jQuery;

    var quickExpr = /^[^<]*(<(.|\s)+>)[^>]*$|^#(\w+)$/;

    jQuery.fn = jQuery.prototype = {
        init: function(selector, context) {
            // Make sure that a selection was provided
            selector = selector || document;

            // Handle HTML strings
            if (typeof selector == "string") {
                var m = quickExpr.exec(selector);
                if (m && (m[1] || !context)) {
                    // HANDLE: $(html) -> $(array)
                    if (m[1])
                        selector = jQuery.clean([m[1]], context);

                    // HANDLE: $("#id")
                    else {
                        var tmp = document.getElementById(m[3]);
                        if (tmp)
                        // Handle the case where IE and Opera return items
                        // by name instead of ID
                            if (tmp.id != m[3])
                            return jQuery().find(selector);
                        else {
                            this[0] = tmp;
                            this.length = 1;
                            return this;
                        }
                        else
                            selector = [];
                    }

                    // HANDLE: $(expr)
                } else
                    return new jQuery(context).find(selector);

                // HANDLE: $(function)
                // Shortcut for document ready
            } else if (jQuery.isFunction(selector))
                return new jQuery(document)[jQuery.fn.ready ? "ready" : "load"](selector);

            return this.setArray(
            // HANDLE: $(array)
			selector.constructor == Array && selector ||

            // HANDLE: $(arraylike)
            // Watch for when an array-like object is passed as the selector
			(selector.jquery || selector.length && selector != window && !selector.nodeType && selector[0] != undefined && selector[0].nodeType) && jQuery.makeArray(selector) ||

            // HANDLE: $(*)
			[selector]);
        },

        jquery: "1.2.1",

        size: function() {
            return this.length;
        },

        length: 0,

        get: function(num) {
            return num == undefined ?

            // Return a 'clean' array
			jQuery.makeArray(this) :

            // Return just the object
			this[num];
        },

        pushStack: function(a) {
            var ret = jQuery(a);
            ret.prevObject = this;
            return ret;
        },

        setArray: function(a) {
            this.length = 0;
            Array.prototype.push.apply(this, a);
            return this;
        },

        each: function(fn, args) {
            return jQuery.each(this, fn, args);
        },

        index: function(obj) {
            var pos = -1;
            this.each(function(i) {
                if (this == obj) pos = i;
            });
            return pos;
        },

        attr: function(key, value, type) {
            var obj = key;

            // Look for the case where we're accessing a style value
            if (key.constructor == String)
                if (value == undefined)
                return this.length && jQuery[type || "attr"](this[0], key) || undefined;
            else {
                obj = {};
                obj[key] = value;
            }

            // Check to see if we're setting style values
            return this.each(function(index) {
                // Set all the styles
                for (var prop in obj)
                    jQuery.attr(
					type ? this.style : this,
					prop, jQuery.prop(this, obj[prop], type, index, prop)
				);
            });
        },

        css: function(key, value) {
            return this.attr(key, value, "curCSS");
        },

        text: function(e) {
            if (typeof e != "object" && e != null)
                return this.empty().append(document.createTextNode(e));

            var t = "";
            jQuery.each(e || this, function() {
                jQuery.each(this.childNodes, function() {
                    if (this.nodeType != 8)
                        t += this.nodeType != 1 ?
						this.nodeValue : jQuery.fn.text([this]);
                });
            });
            return t;
        },

        wrapAll: function(html) {
            if (this[0])
            // The elements to wrap the target around
                jQuery(html, this[0].ownerDocument)
				.clone()
				.insertBefore(this[0])
				.map(function() {
				    var elem = this;
				    while (elem.firstChild)
				        elem = elem.firstChild;
				    return elem;
				})
				.append(this);

            return this;
        },

        wrapInner: function(html) {
            return this.each(function() {
                jQuery(this).contents().wrapAll(html);
            });
        },

        wrap: function(html) {
            return this.each(function() {
                jQuery(this).wrapAll(html);
            });
        },

        append: function() {
            return this.domManip(arguments, true, 1, function(a) {
                this.appendChild(a);
            });
        },

        prepend: function() {
            return this.domManip(arguments, true, -1, function(a) {
                this.insertBefore(a, this.firstChild);
            });
        },

        before: function() {
            return this.domManip(arguments, false, 1, function(a) {
                this.parentNode.insertBefore(a, this);
            });
        },

        after: function() {
            return this.domManip(arguments, false, -1, function(a) {
                this.parentNode.insertBefore(a, this.nextSibling);
            });
        },

        end: function() {
            return this.prevObject || jQuery([]);
        },

        find: function(t) {
            var data = jQuery.map(this, function(a) { return jQuery.find(t, a); });
            return this.pushStack(/[^+>] [^+>]/.test(t) || t.indexOf("..") > -1 ?
			jQuery.unique(data) : data);
        },

        clone: function(events) {
            // Do the clone
            var ret = this.map(function() {
                return this.outerHTML ? jQuery(this.outerHTML)[0] : this.cloneNode(true);
            });

            // Need to set the expando to null on the cloned set if it exists
            // removeData doesn't work here, IE removes it from the original as well
            // this is primarily for IE but the data expando shouldn't be copied over in any browser
            var clone = ret.find("*").andSelf().each(function() {
                if (this[expando] != undefined)
                    this[expando] = null;
            });

            // Copy the events from the original to the clone
            if (events === true)
                this.find("*").andSelf().each(function(i) {
                    var events = jQuery.data(this, "events");
                    for (var type in events)
                        for (var handler in events[type])
                        jQuery.event.add(clone[i], type, events[type][handler], events[type][handler].data);
                });

            // Return the cloned set
            return ret;
        },

        filter: function(t) {
            return this.pushStack(
			jQuery.isFunction(t) &&
			jQuery.grep(this, function(el, index) {
			    return t.apply(el, [index]);
			}) ||

			jQuery.multiFilter(t, this));
        },

        not: function(t) {
            return this.pushStack(
			t.constructor == String &&
			jQuery.multiFilter(t, this, true) ||

			jQuery.grep(this, function(a) {
			    return (t.constructor == Array || t.jquery)
					? jQuery.inArray(a, t) < 0
					: a != t;
			})
		);
        },

        add: function(t) {
            return this.pushStack(jQuery.merge(
			this.get(),
			t.constructor == String ?
				jQuery(t).get() :
				t.length != undefined && (!t.nodeName || jQuery.nodeName(t, "form")) ?
					t : [t])
		);
        },

        is: function(expr) {
            return expr ? jQuery.multiFilter(expr, this).length > 0 : false;
        },

        hasClass: function(expr) {
            return this.is("." + expr);
        },

        val: function(val) {
            if (val == undefined) {
                if (this.length) {
                    var elem = this[0];

                    // We need to handle select boxes special
                    if (jQuery.nodeName(elem, "select")) {
                        var index = elem.selectedIndex,
						a = [],
						options = elem.options,
						one = elem.type == "select-one";

                        // Nothing was selected
                        if (index < 0)
                            return null;

                        // Loop through all the selected options
                        for (var i = one ? index : 0, max = one ? index + 1 : options.length; i < max; i++) {
                            var option = options[i];
                            if (option.selected) {
                                // Get the specifc value for the option
                                var val = jQuery.browser.msie && !option.attributes["value"].specified ? option.text : option.value;

                                // We don't need an array for one selects
                                if (one)
                                    return val;

                                // Multi-Selects return an array
                                a.push(val);
                            }
                        }

                        return a;

                        // Everything else, we just grab the value
                    } else
                        return this[0].value.replace(/\r/g, "");
                }
            } else
                return this.each(function() {
                    if (val.constructor == Array && /radio|checkbox/.test(this.type))
                        this.checked = (jQuery.inArray(this.value, val) >= 0 ||
						jQuery.inArray(this.name, val) >= 0);
                    else if (jQuery.nodeName(this, "select")) {
                        var tmp = val.constructor == Array ? val : [val];

                        jQuery("option", this).each(function() {
                            this.selected = (jQuery.inArray(this.value, tmp) >= 0 ||
						jQuery.inArray(this.text, tmp) >= 0);
                        });

                        if (!tmp.length)
                            this.selectedIndex = -1;
                    } else
                        this.value = val;
                });
        },

        html: function(val) {
            return val == undefined ?
			(this.length ? this[0].innerHTML : null) :
			this.empty().append(val);
        },

        replaceWith: function(val) {
            return this.after(val).remove();
        },

        eq: function(i) {
            return this.slice(i, i + 1);
        },

        slice: function() {
            return this.pushStack(Array.prototype.slice.apply(this, arguments));
        },

        map: function(fn) {
            return this.pushStack(jQuery.map(this, function(elem, i) {
                return fn.call(elem, i, elem);
            }));
        },

        andSelf: function() {
            return this.add(this.prevObject);
        },

        domManip: function(args, table, dir, fn) {
            var clone = this.length > 1, a;

            return this.each(function() {
                if (!a) {
                    a = jQuery.clean(args, this.ownerDocument);
                    if (dir < 0)
                        a.reverse();
                }

                var obj = this;

                if (table && jQuery.nodeName(this, "table") && jQuery.nodeName(a[0], "tr"))
                    obj = this.getElementsByTagName("tbody")[0] || this.appendChild(document.createElement("tbody"));

                jQuery.each(a, function() {
                    var elem = clone ? this.cloneNode(true) : this;
                    if (!evalScript(0, elem))
                        fn.call(obj, elem);
                });
            });
        }
    };

    function evalScript(i, elem) {
        var script = jQuery.nodeName(elem, "script");

        if (script) {
            if (elem.src)
                jQuery.ajax({ url: elem.src, async: false, dataType: "script" });
            else
                jQuery.globalEval(elem.text || elem.textContent || elem.innerHTML || "");

            if (elem.parentNode)
                elem.parentNode.removeChild(elem);

        } else if (elem.nodeType == 1)
            jQuery("script", elem).each(evalScript);

        return script;
    }

    jQuery.extend = jQuery.fn.extend = function() {
        // copy reference to target object
        var target = arguments[0] || {}, a = 1, al = arguments.length, deep = false;

        // Handle a deep copy situation
        if (target.constructor == Boolean) {
            deep = target;
            target = arguments[1] || {};
        }

        // extend jQuery itself if only one argument is passed
        if (al == 1) {
            target = this;
            a = 0;
        }

        var prop;

        for (; a < al; a++)
        // Only deal with non-null/undefined values
            if ((prop = arguments[a]) != null)
        // Extend the base object
            for (var i in prop) {
            // Prevent never-ending loop
            if (target == prop[i])
                continue;

            // Recurse if we're merging object values
            if (deep && typeof prop[i] == 'object' && target[i])
                jQuery.extend(target[i], prop[i]);

            // Don't bring in undefined values
            else if (prop[i] != undefined)
                target[i] = prop[i];
        }

        // Return the modified object
        return target;
    };

    var expando = "jQuery" + (new Date()).getTime(), uuid = 0, win = {};

    jQuery.extend({
        noConflict: function(deep) {
            window.$ = _$;
            if (deep)
                window.jQuery = _jQuery;
            return jQuery;
        },

        // This may seem like some crazy code, but trust me when I say that this
        // is the only cross-browser way to do this. --John
        isFunction: function(fn) {
            return !!fn && typeof fn != "string" && !fn.nodeName &&
			fn.constructor != Array && /function/i.test(fn + "");
        },

        // check if an element is in a XML document
        isXMLDoc: function(elem) {
            if (elem) {
                return elem.documentElement && !elem.body ||
			    elem.tagName && elem.ownerDocument && !elem.ownerDocument.body;
            }
        },

        // Evalulates a script in a global context
        // Evaluates Async. in Safari 2 :-(
        globalEval: function(data) {
            data = jQuery.trim(data);
            if (data) {
                if (window.execScript)
                    window.execScript(data);
                else if (jQuery.browser.safari)
                // safari doesn't provide a synchronous global eval
                    window.setTimeout(data, 0);
                else
                    eval.call(window, data);
            }
        },

        nodeName: function(elem, name) {
            return elem.nodeName && elem.nodeName.toUpperCase() == name.toUpperCase();
        },

        cache: {},

        data: function(elem, name, data) {
            elem = elem == window ? win : elem;

            var id = elem[expando];

            // Compute a unique ID for the element
            if (!id)
                id = elem[expando] = ++uuid;

            // Only generate the data cache if we're
            // trying to access or manipulate it
            if (name && !jQuery.cache[id])
                jQuery.cache[id] = {};

            // Prevent overriding the named cache with undefined values
            if (data != undefined)
                jQuery.cache[id][name] = data;

            // Return the named cache data, or the ID for the element	
            return name ? jQuery.cache[id][name] : id;
        },

        removeData: function(elem, name) {
            elem = elem == window ? win : elem;

            var id = elem[expando];

            // If we want to remove a specific section of the element's data
            if (name) {
                if (jQuery.cache[id]) {
                    // Remove the section of cache data
                    delete jQuery.cache[id][name];

                    // If we've removed all the data, remove the element's cache
                    name = "";
                    for (name in jQuery.cache[id]) break;
                    if (!name)
                        jQuery.removeData(elem);
                }

                // Otherwise, we want to remove all of the element's data
            } else {
                // Clean up the element expando
                try {
                    delete elem[expando];
                } catch (e) {
                    // IE has trouble directly removing the expando
                    // but it's ok with using removeAttribute
                    if (elem.removeAttribute)
                        elem.removeAttribute(expando);
                }

                // Completely remove the data cache
                delete jQuery.cache[id];
            }
        },

        // args is for internal usage only
        each: function(obj, fn, args) {
            if (args) {
                if (obj.length == undefined)
                    for (var i in obj)
                    fn.apply(obj[i], args);
                else
                    for (var i = 0, ol = obj.length; i < ol; i++)
                    if (fn.apply(obj[i], args) === false) break;

                // A special, fast, case for the most common use of each
            } else {
                if (obj.length == undefined)
                    for (var i in obj)
                    fn.call(obj[i], i, obj[i]);
                else
                    for (var i = 0, ol = obj.length, val = obj[0];
					i < ol && fn.call(val, i, val) !== false; val = obj[++i]) { }
            }

            return obj;
        },

        prop: function(elem, value, type, index, prop) {
            // Handle executable functions
            if (jQuery.isFunction(value))
                value = value.call(elem, [index]);

            // exclude the following css properties to add px
            var exclude = /z-?index|font-?weight|opacity|zoom|line-?height/i;

            // Handle passing in a number to a CSS property
            return value && value.constructor == Number && type == "curCSS" && !exclude.test(prop) ?
				value + "px" :
				value;
        },

        className: {
            // internal only, use addClass("class")
            add: function(elem, c) {
                jQuery.each((c || "").split(/\s+/), function(i, cur) {
                    if (!jQuery.className.has(elem.className, cur))
                        elem.className += (elem.className ? " " : "") + cur;
                });
            },

            // internal only, use removeClass("class")
            remove: function(elem, c) {
                elem.className = c != undefined ?
				jQuery.grep(elem.className.split(/\s+/), function(cur) {
				    return !jQuery.className.has(c, cur);
				}).join(" ") : "";
            },

            // internal only, use is(".class")
            has: function(t, c) {
                return jQuery.inArray(c, (t.className || t).toString().split(/\s+/)) > -1;
            }
        },

        swap: function(e, o, f) {
            for (var i in o) {
                e.style["old" + i] = e.style[i];
                e.style[i] = o[i];
            }
            f.apply(e, []);
            for (var i in o)
                e.style[i] = e.style["old" + i];
        },

        css: function(e, p) {
            if (p == "height" || p == "width") {
                var old = {}, oHeight, oWidth, d = ["Top", "Bottom", "Right", "Left"];

                jQuery.each(d, function() {
                    old["padding" + this] = 0;
                    old["border" + this + "Width"] = 0;
                });

                jQuery.swap(e, old, function() {
                    if (jQuery(e).is(':visible')) {
                        oHeight = e.offsetHeight;
                        oWidth = e.offsetWidth;
                    } else {
                        e = jQuery(e.cloneNode(true))
						.find(":radio").removeAttr("checked").end()
						.css({
						    visibility: "hidden", position: "absolute", display: "block", right: "0", left: "0"
						}).appendTo(e.parentNode)[0];

                        var parPos = jQuery.css(e.parentNode, "position") || "static";
                        if (parPos == "static")
                            e.parentNode.style.position = "relative";

                        oHeight = e.clientHeight;
                        oWidth = e.clientWidth;

                        if (parPos == "static")
                            e.parentNode.style.position = "static";

                        e.parentNode.removeChild(e);
                    }
                });

                return p == "height" ? oHeight : oWidth;
            }

            return jQuery.curCSS(e, p);
        },

        curCSS: function(elem, prop, force) {
            var ret, stack = [], swap = [];

            // A helper method for determining if an element's values are broken
            function color(a) {
                if (!jQuery.browser.safari)
                    return false;

                var ret = document.defaultView.getComputedStyle(a, null);
                return !ret || ret.getPropertyValue("color") == "";
            }

            if (prop == "opacity" && jQuery.browser.msie) {
                ret = jQuery.attr(elem.style, "opacity");
                return ret == "" ? "1" : ret;
            }

            if (prop.match(/float/i))
                prop = styleFloat;

            if (!force && elem.style && elem.style[prop])
                ret = elem.style[prop];

            else if (document.defaultView && document.defaultView.getComputedStyle) {

                if (prop.match(/float/i))
                    prop = "float";

                prop = prop.replace(/([A-Z])/g, "-$1").toLowerCase();
                var cur = document.defaultView.getComputedStyle(elem, null);

                if (cur && !color(elem))
                    ret = cur.getPropertyValue(prop);

                // If the element isn't reporting its values properly in Safari
                // then some display: none elements are involved
                else {
                    // Locate all of the parent display: none elements
                    for (var a = elem; a && color(a); a = a.parentNode)
                        stack.unshift(a);

                    // Go through and make them visible, but in reverse
                    // (It would be better if we knew the exact display type that they had)
                    for (a = 0; a < stack.length; a++)
                        if (color(stack[a])) {
                        swap[a] = stack[a].style.display;
                        stack[a].style.display = "block";
                    }

                    // Since we flip the display style, we have to handle that
                    // one special, otherwise get the value
                    ret = prop == "display" && swap[stack.length - 1] != null ?
					"none" :
					document.defaultView.getComputedStyle(elem, null).getPropertyValue(prop) || "";

                    // Finally, revert the display styles back
                    for (a = 0; a < swap.length; a++)
                        if (swap[a] != null)
                        stack[a].style.display = swap[a];
                }

                if (prop == "opacity" && ret == "")
                    ret = "1";

            } else if (elem.currentStyle) {
                var newProp = prop.replace(/\-(\w)/g, function(m, c) { return c.toUpperCase(); });
                ret = elem.currentStyle[prop] || elem.currentStyle[newProp];

                // From the awesome hack by Dean Edwards
                // http://erik.eae.net/archives/2007/07/27/18.54.15/#comment-102291

                // If we're not dealing with a regular pixel number
                // but a number that has a weird ending, we need to convert it to pixels
                if (!/^\d+(px)?$/i.test(ret) && /^\d/.test(ret)) {
                    var style = elem.style.left;
                    var runtimeStyle = elem.runtimeStyle.left;
                    elem.runtimeStyle.left = elem.currentStyle.left;
                    elem.style.left = ret || 0;
                    ret = elem.style.pixelLeft + "px";
                    elem.style.left = style;
                    elem.runtimeStyle.left = runtimeStyle;
                }
            }

            return ret;
        },

        clean: function(a, doc) {
            var r = [];
            doc = doc || document;

            jQuery.each(a, function(i, arg) {
                if (!arg) return;

                if (arg.constructor == Number)
                    arg = arg.toString();

                // Convert html string into DOM nodes
                if (typeof arg == "string") {
                    // Fix "XHTML"-style tags in all browsers
                    arg = arg.replace(/(<(\w+)[^>]*?)\/>/g, function(m, all, tag) {
                        return tag.match(/^(abbr|br|col|img|input|link|meta|param|hr|area)$/i) ? m : all + "></" + tag + ">";
                    });

                    // Trim whitespace, otherwise indexOf won't work as expected
                    var s = jQuery.trim(arg).toLowerCase(), div = doc.createElement("div"), tb = [];

                    var wrap =
                    // option or optgroup
					!s.indexOf("<opt") &&
					[1, "<select>", "</select>"] ||

					!s.indexOf("<leg") &&
					[1, "<fieldset>", "</fieldset>"] ||

					s.match(/^<(thead|tbody|tfoot|colg|cap)/) &&
					[1, "<table>", "</table>"] ||

					!s.indexOf("<tr") &&
					[2, "<table><tbody>", "</tbody></table>"] ||

                    // <thead> matched above
					(!s.indexOf("<td") || !s.indexOf("<th")) &&
					[3, "<table><tbody><tr>", "</tr></tbody></table>"] ||

					!s.indexOf("<col") &&
					[2, "<table><tbody></tbody><colgroup>", "</colgroup></table>"] ||

                    // IE can't serialize <link> and <script> tags normally
					jQuery.browser.msie &&
					[1, "div<div>", "</div>"] ||

					[0, "", ""];

                    // Go to html and back, then peel off extra wrappers
                    div.innerHTML = wrap[1] + arg + wrap[2];

                    // Move to the right depth
                    while (wrap[0]--)
                        div = div.lastChild;

                    // Remove IE's autoinserted <tbody> from table fragments
                    if (jQuery.browser.msie) {

                        // String was a <table>, *may* have spurious <tbody>
                        if (!s.indexOf("<table") && s.indexOf("<tbody") < 0)
                            tb = div.firstChild && div.firstChild.childNodes;

                        // String was a bare <thead> or <tfoot>
                        else if (wrap[1] == "<table>" && s.indexOf("<tbody") < 0)
                            tb = div.childNodes;

                        for (var n = tb.length - 1; n >= 0; --n)
                            if (jQuery.nodeName(tb[n], "tbody") && !tb[n].childNodes.length)
                            tb[n].parentNode.removeChild(tb[n]);

                        // IE completely kills leading whitespace when innerHTML is used	
                        if (/^\s/.test(arg))
                            div.insertBefore(doc.createTextNode(arg.match(/^\s*/)[0]), div.firstChild);

                    }

                    arg = jQuery.makeArray(div.childNodes);
                }

                if (0 === arg.length && (!jQuery.nodeName(arg, "form") && !jQuery.nodeName(arg, "select")))
                    return;

                if (arg[0] == undefined || jQuery.nodeName(arg, "form") || arg.options)
                    r.push(arg);
                else
                    r = jQuery.merge(r, arg);

            });

            return r;
        },

        attr: function(elem, name, value) {
            var fix = jQuery.isXMLDoc(elem) ? {} : jQuery.props;

            // Safari mis-reports the default selected property of a hidden option
            // Accessing the parent's selectedIndex property fixes it
            if (name == "selected" && jQuery.browser.safari)
                elem.parentNode.selectedIndex;

            // Certain attributes only work when accessed via the old DOM 0 way
            if (fix[name]) {
                if (value != undefined) elem[fix[name]] = value;
                return elem[fix[name]];
            } else if (jQuery.browser.msie && name == "style")
                return jQuery.attr(elem.style, "cssText", value);

            else if (value == undefined && jQuery.browser.msie && jQuery.nodeName(elem, "form") && (name == "action" || name == "method"))
                return elem.getAttributeNode(name).nodeValue;

            // IE elem.getAttribute passes even for style
            else if (elem && elem.tagName) {

                if (value != undefined) {
                    if (name == "type" && jQuery.nodeName(elem, "input") && elem.parentNode)
                        throw "type property can't be changed";
                    elem.setAttribute(name, value);
                }

                if (jQuery.browser.msie && /href|src/.test(name) && !jQuery.isXMLDoc(elem))
                    return elem.getAttribute(name, 2);

                return elem.getAttribute(name);

                // elem is actually elem.style ... set the style
            } else {
                // IE actually uses filters for opacity
                if (name == "opacity" && jQuery.browser.msie) {
                    if (value != undefined) {
                        // IE has trouble with opacity if it does not have layout
                        // Force it by setting the zoom level
                        elem.zoom = 1;

                        // Set the alpha filter to set the opacity
                        elem.filter = (elem.filter || "").replace(/alpha\([^)]*\)/, "") +
						(parseFloat(value).toString() == "NaN" ? "" : "alpha(opacity=" + value * 100 + ")");
                    }

                    return elem.filter ?
					(parseFloat(elem.filter.match(/opacity=([^)]*)/)[1]) / 100).toString() : "";
                }
                name = name.replace(/-([a-z])/ig, function(z, b) { return b.toUpperCase(); });
                if (value != undefined) elem[name] = value;
                return elem[name];
            }
        },

        trim: function(t) {
            return (t || "").replace(/^\s+|\s+$/g, "");
        },

        makeArray: function(a) {
            var r = [];

            // Need to use typeof to fight Safari childNodes crashes
            if (typeof a != "array")
                for (var i = 0, al = a.length; i < al; i++)
                r.push(a[i]);
            else
                r = a.slice(0);

            return r;
        },

        inArray: function(b, a) {
            for (var i = 0, al = a.length; i < al; i++)
                if (a[i] == b)
                return i;
            return -1;
        },

        merge: function(first, second) {
            // We have to loop this way because IE & Opera overwrite the length
            // expando of getElementsByTagName

            // Also, we need to make sure that the correct elements are being returned
            // (IE returns comment nodes in a '*' query)
            if (jQuery.browser.msie) {
                for (var i = 0; second[i]; i++)
                    if (second[i].nodeType != 8)
                    first.push(second[i]);
            } else
                for (var i = 0; second[i]; i++)
                first.push(second[i]);

            return first;
        },

        unique: function(first) {
            var r = [], done = {};

            try {
                for (var i = 0, fl = first.length; i < fl; i++) {
                    var id = jQuery.data(first[i]);
                    if (!done[id]) {
                        done[id] = true;
                        r.push(first[i]);
                    }
                }
            } catch (e) {
                r = first;
            }

            return r;
        },

        grep: function(elems, fn, inv) {
            // If a string is passed in for the function, make a function
            // for it (a handy shortcut)
            if (typeof fn == "string")
                fn = eval("false||function(a,i){return " + fn + "}");

            var result = [];

            // Go through the array, only saving the items
            // that pass the validator function
            for (var i = 0, el = elems.length; i < el; i++)
                if (!inv && fn(elems[i], i) || inv && !fn(elems[i], i))
                result.push(elems[i]);

            return result;
        },

        map: function(elems, fn) {
            // If a string is passed in for the function, make a function
            // for it (a handy shortcut)
            if (typeof fn == "string")
                fn = eval("false||function(a){return " + fn + "}");

            var result = [];

            // Go through the array, translating each of the items to their
            // new value (or values).
            for (var i = 0, el = elems.length; i < el; i++) {
                var val = fn(elems[i], i);

                if (val !== null && val != undefined) {
                    if (val.constructor != Array) val = [val];
                    result = result.concat(val);
                }
            }

            return result;
        }
    });

    var userAgent = navigator.userAgent.toLowerCase();

    // Figure out what browser is being used
    jQuery.browser = {
        version: (userAgent.match(/.+(?:rv|it|ra|ie)[\/: ]([\d.]+)/) || [])[1],
        safari: /webkit/.test(userAgent),
        opera: /opera/.test(userAgent),
        msie: /msie/.test(userAgent) && !/opera/.test(userAgent),
        mozilla: /mozilla/.test(userAgent) && !/(compatible|webkit)/.test(userAgent)
    };

    var styleFloat = jQuery.browser.msie ? "styleFloat" : "cssFloat";

    jQuery.extend({
        // Check to see if the W3C box model is being used
        boxModel: !jQuery.browser.msie || document.compatMode == "CSS1Compat",

        styleFloat: jQuery.browser.msie ? "styleFloat" : "cssFloat",

        props: {
            "for": "htmlFor",
            "class": "className",
            "float": styleFloat,
            cssFloat: styleFloat,
            styleFloat: styleFloat,
            innerHTML: "innerHTML",
            className: "className",
            value: "value",
            disabled: "disabled",
            checked: "checked",
            readonly: "readOnly",
            selected: "selected",
            maxlength: "maxLength"
        }
    });

    jQuery.each({
        parent: "a.parentNode",
        parents: "jQuery.dir(a,'parentNode')",
        next: "jQuery.nth(a,2,'nextSibling')",
        prev: "jQuery.nth(a,2,'previousSibling')",
        nextAll: "jQuery.dir(a,'nextSibling')",
        prevAll: "jQuery.dir(a,'previousSibling')",
        siblings: "jQuery.sibling(a.parentNode.firstChild,a)",
        children: "jQuery.sibling(a.firstChild)",
        contents: "jQuery.nodeName(a,'iframe')?a.contentDocument||a.contentWindow.document:jQuery.makeArray(a.childNodes)"
    }, function(i, n) {
        jQuery.fn[i] = function(a) {
            var ret = jQuery.map(this, n);
            if (a && typeof a == "string")
                ret = jQuery.multiFilter(a, ret);
            return this.pushStack(jQuery.unique(ret));
        };
    });

    jQuery.each({
        appendTo: "append",
        prependTo: "prepend",
        insertBefore: "before",
        insertAfter: "after",
        replaceAll: "replaceWith"
    }, function(i, n) {
        jQuery.fn[i] = function() {
            var a = arguments;
            return this.each(function() {
                for (var j = 0, al = a.length; j < al; j++)
                    jQuery(a[j])[n](this);
            });
        };
    });

    jQuery.each({
        removeAttr: function(key) {
            jQuery.attr(this, key, "");
            this.removeAttribute(key);
        },
        addClass: function(c) {
            jQuery.className.add(this, c);
        },
        removeClass: function(c) {
            jQuery.className.remove(this, c);
        },
        toggleClass: function(c) {
            jQuery.className[jQuery.className.has(this, c) ? "remove" : "add"](this, c);
        },
        remove: function(a) {
            if (!a || jQuery.filter(a, [this]).r.length) {
                jQuery.removeData(this);
                this.parentNode.removeChild(this);
            }
        },
        empty: function() {
            // Clean up the cache
            jQuery("*", this).each(function() { jQuery.removeData(this); });

            while (this.firstChild)
                this.removeChild(this.firstChild);
        }
    }, function(i, n) {
        jQuery.fn[i] = function() {
            return this.each(n, arguments);
        };
    });

    jQuery.each(["Height", "Width"], function(i, name) {
        var n = name.toLowerCase();

        jQuery.fn[n] = function(h) {
            return this[0] == window ?
			jQuery.browser.safari && self["inner" + name] ||
			jQuery.boxModel && Math.max(document.documentElement["client" + name], document.body["client" + name]) ||
			document.body["client" + name] :

			this[0] == document ?
				Math.max(document.body["scroll" + name], document.body["offset" + name]) :

				h == undefined ?
					(this.length ? jQuery.css(this[0], n) : null) :
					this.css(n, h.constructor == String ? h : h + "px");
        };
    });

    var chars = jQuery.browser.safari && parseInt(jQuery.browser.version) < 417 ?
		"(?:[\\w*_-]|\\\\.)" :
		"(?:[\\w\u0128-\uFFFF*_-]|\\\\.)",
	quickChild = new RegExp("^>\\s*(" + chars + "+)"),
	quickID = new RegExp("^(" + chars + "+)(#)(" + chars + "+)"),
	quickClass = new RegExp("^([#.]?)(" + chars + "*)");

    jQuery.extend({
        expr: {
            "": "m[2]=='*'||jQuery.nodeName(a,m[2])",
            "#": "a.getAttribute('id')==m[2]",
            ":": {
                // Position Checks
                lt: "i<m[3]-0",
                gt: "i>m[3]-0",
                nth: "m[3]-0==i",
                eq: "m[3]-0==i",
                first: "i==0",
                last: "i==r.length-1",
                even: "i%2==0",
                odd: "i%2",

                // Child Checks
                "first-child": "a.parentNode.getElementsByTagName('*')[0]==a",
                "last-child": "jQuery.nth(a.parentNode.lastChild,1,'previousSibling')==a",
                "only-child": "!jQuery.nth(a.parentNode.lastChild,2,'previousSibling')",

                // Parent Checks
                parent: "a.firstChild",
                empty: "!a.firstChild",

                // Text Check
                contains: "(a.textContent||a.innerText||jQuery(a).text()||'').indexOf(m[3])>=0",

                // Visibility
                visible: '"hidden"!=a.type&&jQuery.css(a,"display")!="none"&&jQuery.css(a,"visibility")!="hidden"',
                hidden: '"hidden"==a.type||jQuery.css(a,"display")=="none"||jQuery.css(a,"visibility")=="hidden"',

                // Form attributes
                enabled: "!a.disabled",
                disabled: "a.disabled",
                checked: "a.checked",
                selected: "a.selected||jQuery.attr(a,'selected')",

                // Form elements
                text: "'text'==a.type",
                radio: "'radio'==a.type",
                checkbox: "'checkbox'==a.type",
                file: "'file'==a.type",
                password: "'password'==a.type",
                submit: "'submit'==a.type",
                image: "'image'==a.type",
                reset: "'reset'==a.type",
                button: '"button"==a.type||jQuery.nodeName(a,"button")',
                input: "/input|select|textarea|button/i.test(a.nodeName)",

                // :has()
                has: "jQuery.find(m[3],a).length",

                // :header
                header: "/h\\d/i.test(a.nodeName)",

                // :animated
                animated: "jQuery.grep(jQuery.timers,function(fn){return a==fn.elem;}).length"
            }
        },

        // The regular expressions that power the parsing engine
        parse: [
        // Match: [@value='test'], [@foo]
		/^(\[) *@?([\w-]+) *([!*$^~=]*) *('?"?)(.*?)\4 *\]/,

        // Match: :contains('foo')
		/^(:)([\w-]+)\("?'?(.*?(\(.*?\))?[^(]*?)"?'?\)/,

        // Match: :even, :last-chlid, #id, .class
		new RegExp("^([:.#]*)(" + chars + "+)")
	],

        multiFilter: function(expr, elems, not) {
            var old, cur = [];

            while (expr && expr != old) {
                old = expr;
                var f = jQuery.filter(expr, elems, not);
                expr = f.t.replace(/^\s*,\s*/, "");
                cur = not ? elems = f.r : jQuery.merge(cur, f.r);
            }

            return cur;
        },

        find: function(t, context) {
            // Quickly handle non-string expressions
            if (typeof t != "string")
                return [t];

            // Make sure that the context is a DOM Element
            if (context && !context.nodeType)
                context = null;

            // Set the correct context (if none is provided)
            context = context || document;

            // Initialize the search
            var ret = [context], done = [], last;

            // Continue while a selector expression exists, and while
            // we're no longer looping upon ourselves
            while (t && last != t) {
                var r = [];
                last = t;

                t = jQuery.trim(t);

                var foundToken = false;

                // An attempt at speeding up child selectors that
                // point to a specific element tag
                var re = quickChild;
                var m = re.exec(t);

                if (m) {
                    var nodeName = m[1].toUpperCase();

                    // Perform our own iteration and filter
                    for (var i = 0; ret[i]; i++)
                        for (var c = ret[i].firstChild; c; c = c.nextSibling)
                        if (c.nodeType == 1 && (nodeName == "*" || c.nodeName.toUpperCase() == nodeName.toUpperCase()))
                        r.push(c);

                    ret = r;
                    t = t.replace(re, "");
                    if (t.indexOf(" ") == 0) continue;
                    foundToken = true;
                } else {
                    re = /^([>+~])\s*(\w*)/i;

                    if ((m = re.exec(t)) != null) {
                        r = [];

                        var nodeName = m[2], merge = {};
                        m = m[1];

                        for (var j = 0, rl = ret.length; j < rl; j++) {
                            var n = m == "~" || m == "+" ? ret[j].nextSibling : ret[j].firstChild;
                            for (; n; n = n.nextSibling)
                                if (n.nodeType == 1) {
                                var id = jQuery.data(n);

                                if (m == "~" && merge[id]) break;

                                if (!nodeName || n.nodeName.toUpperCase() == nodeName.toUpperCase()) {
                                    if (m == "~") merge[id] = true;
                                    r.push(n);
                                }

                                if (m == "+") break;
                            }
                        }

                        ret = r;

                        // And remove the token
                        t = jQuery.trim(t.replace(re, ""));
                        foundToken = true;
                    }
                }

                // See if there's still an expression, and that we haven't already
                // matched a token
                if (t && !foundToken) {
                    // Handle multiple expressions
                    if (!t.indexOf(",")) {
                        // Clean the result set
                        if (context == ret[0]) ret.shift();

                        // Merge the result sets
                        done = jQuery.merge(done, ret);

                        // Reset the context
                        r = ret = [context];

                        // Touch up the selector string
                        t = " " + t.substr(1, t.length);

                    } else {
                        // Optimize for the case nodeName#idName
                        var re2 = quickID;
                        var m = re2.exec(t);

                        // Re-organize the results, so that they're consistent
                        if (m) {
                            m = [0, m[2], m[3], m[1]];

                        } else {
                            // Otherwise, do a traditional filter check for
                            // ID, class, and element selectors
                            re2 = quickClass;
                            m = re2.exec(t);
                        }

                        m[2] = m[2].replace(/\\/g, "");

                        var elem = ret[ret.length - 1];

                        // Try to do a global search by ID, where we can
                        if (m[1] == "#" && elem && elem.getElementById && !jQuery.isXMLDoc(elem)) {
                            // Optimization for HTML document case
                            var oid = elem.getElementById(m[2]);

                            // Do a quick check for the existence of the actual ID attribute
                            // to avoid selecting by the name attribute in IE
                            // also check to insure id is a string to avoid selecting an element with the name of 'id' inside a form
                            if ((jQuery.browser.msie || jQuery.browser.opera) && oid && typeof oid.id == "string" && oid.id != m[2])
                                oid = jQuery('[@id="' + m[2] + '"]', elem)[0];

                            // Do a quick check for node name (where applicable) so
                            // that div#foo searches will be really fast
                            ret = r = oid && (!m[3] || jQuery.nodeName(oid, m[3])) ? [oid] : [];
                        } else {
                            // We need to find all descendant elements
                            for (var i = 0; ret[i]; i++) {
                                // Grab the tag name being searched for
                                var tag = m[1] == "#" && m[3] ? m[3] : m[1] != "" || m[0] == "" ? "*" : m[2];

                                // Handle IE7 being really dumb about <object>s
                                if (tag == "*" && ret[i].nodeName.toLowerCase() == "object")
                                    tag = "param";

                                r = jQuery.merge(r, ret[i].getElementsByTagName(tag));
                            }

                            // It's faster to filter by class and be done with it
                            if (m[1] == ".")
                                r = jQuery.classFilter(r, m[2]);

                            // Same with ID filtering
                            if (m[1] == "#") {
                                var tmp = [];

                                // Try to find the element with the ID
                                for (var i = 0; r[i]; i++)
                                    if (r[i].getAttribute("id") == m[2]) {
                                    tmp = [r[i]];
                                    break;
                                }

                                r = tmp;
                            }

                            ret = r;
                        }

                        t = t.replace(re2, "");
                    }

                }

                // If a selector string still exists
                if (t) {
                    // Attempt to filter it
                    var val = jQuery.filter(t, r);
                    ret = r = val.r;
                    t = jQuery.trim(val.t);
                }
            }

            // An error occurred with the selector;
            // just return an empty set instead
            if (t)
                ret = [];

            // Remove the root context
            if (ret && context == ret[0])
                ret.shift();

            // And combine the results
            done = jQuery.merge(done, ret);

            return done;
        },

        classFilter: function(r, m, not) {
            m = " " + m + " ";
            var tmp = [];
            for (var i = 0; r[i]; i++) {
                var pass = (" " + r[i].className + " ").indexOf(m) >= 0;
                if (!not && pass || not && !pass)
                    tmp.push(r[i]);
            }
            return tmp;
        },

        filter: function(t, r, not) {
            var last;

            // Look for common filter expressions
            while (t && t != last) {
                last = t;

                var p = jQuery.parse, m;

                for (var i = 0; p[i]; i++) {
                    m = p[i].exec(t);

                    if (m) {
                        // Remove what we just matched
                        t = t.substring(m[0].length);

                        m[2] = m[2].replace(/\\/g, "");
                        break;
                    }
                }

                if (!m)
                    break;

                // :not() is a special case that can be optimized by
                // keeping it out of the expression list
                if (m[1] == ":" && m[2] == "not")
                    r = jQuery.filter(m[3], r, true).r;

                // We can get a big speed boost by filtering by class here
                else if (m[1] == ".")
                    r = jQuery.classFilter(r, m[2], not);

                else if (m[1] == "[") {
                    var tmp = [], type = m[3];

                    for (var i = 0, rl = r.length; i < rl; i++) {
                        var a = r[i], z = a[jQuery.props[m[2]] || m[2]];

                        if (z == null || /href|src|selected/.test(m[2]))
                            z = jQuery.attr(a, m[2]) || '';

                        if ((type == "" && !!z ||
						 type == "=" && z == m[5] ||
						 type == "!=" && z != m[5] ||
						 type == "^=" && z && !z.indexOf(m[5]) ||
						 type == "$=" && z.substr(z.length - m[5].length) == m[5] ||
						 (type == "*=" || type == "~=") && z.indexOf(m[5]) >= 0) ^ not)
                            tmp.push(a);
                    }

                    r = tmp;

                    // We can get a speed boost by handling nth-child here
                } else if (m[1] == ":" && m[2] == "nth-child") {
                    var merge = {}, tmp = [],
					test = /(\d*)n\+?(\d*)/.exec(
						m[3] == "even" && "2n" || m[3] == "odd" && "2n+1" ||
						!/\D/.test(m[3]) && "n+" + m[3] || m[3]),
					first = (test[1] || 1) - 0, last = test[2] - 0;

                    for (var i = 0, rl = r.length; i < rl; i++) {
                        var node = r[i], parentNode = node.parentNode, id = jQuery.data(parentNode);

                        if (!merge[id]) {
                            var c = 1;

                            for (var n = parentNode.firstChild; n; n = n.nextSibling)
                                if (n.nodeType == 1)
                                n.nodeIndex = c++;

                            merge[id] = true;
                        }

                        var add = false;

                        if (first == 1) {
                            if (last == 0 || node.nodeIndex == last)
                                add = true;
                        } else if ((node.nodeIndex + last) % first == 0)
                            add = true;

                        if (add ^ not)
                            tmp.push(node);
                    }

                    r = tmp;

                    // Otherwise, find the expression to execute
                } else {
                    var f = jQuery.expr[m[1]];
                    if (typeof f != "string")
                        f = jQuery.expr[m[1]][m[2]];

                    // Build a custom macro to enclose it
                    f = eval("false||function(a,i){return " + f + "}");

                    // Execute it against the current filter
                    r = jQuery.grep(r, f, not);
                }
            }

            // Return an array of filtered elements (r)
            // and the modified expression string (t)
            return { r: r, t: t };
        },

        dir: function(elem, dir) {
            var matched = [];
            var cur = elem[dir];
            while (cur && cur != document) {
                if (cur.nodeType == 1)
                    matched.push(cur);
                cur = cur[dir];
            }
            return matched;
        },

        nth: function(cur, result, dir, elem) {
            result = result || 1;
            var num = 0;

            for (; cur; cur = cur[dir])
                if (cur.nodeType == 1 && ++num == result)
                break;

            return cur;
        },

        sibling: function(n, elem) {
            var r = [];

            for (; n; n = n.nextSibling) {
                if (n.nodeType == 1 && (!elem || n != elem))
                    r.push(n);
            }

            return r;
        }
    });
    /*
    * A number of helper functions used for managing events.
    * Many of the ideas behind this code orignated from 
    * Dean Edwards' addEvent library.
    */
    jQuery.event = {

        // Bind an event to an element
        // Original by Dean Edwards
        add: function(element, type, handler, data) {
            // For whatever reason, IE has trouble passing the window object
            // around, causing it to be cloned in the process
            if (jQuery.browser.msie && element.setInterval != undefined)
                element = window;

            // Make sure that the function being executed has a unique ID
            if (!handler.guid)
                handler.guid = this.guid++;

            // if data is passed, bind to handler 
            if (data != undefined) {
                // Create temporary function pointer to original handler 
                var fn = handler;

                // Create unique handler function, wrapped around original handler 
                handler = function() {
                    // Pass arguments and context to original handler 
                    return fn.apply(this, arguments);
                };

                // Store data in unique handler 
                handler.data = data;

                // Set the guid of unique handler to the same of original handler, so it can be removed 
                handler.guid = fn.guid;
            }

            // Namespaced event handlers
            var parts = type.split(".");
            type = parts[0];
            handler.type = parts[1];

            // Init the element's event structure
            var events = jQuery.data(element, "events") || jQuery.data(element, "events", {});

            var handle = jQuery.data(element, "handle", function() {
                // returned undefined or false
                var val;

                // Handle the second event of a trigger and when
                // an event is called after a page has unloaded
                if (typeof jQuery == "undefined" || jQuery.event.triggered)
                    return val;

                val = jQuery.event.handle.apply(element, arguments);
                // added by Linson at 2011-12-31    begin
                if (element.type != "submit" && val == false) {
                    SKYSALES.RemoveLoadingBar();
                }
                // added by Linson at 2011-12-31    end
                return val;
            });

            // Get the current list of functions bound to this event
            var handlers = events[type];

            // Init the event handler queue
            if (!handlers) {
                handlers = events[type] = {};

                // And bind the global event handler to the element
                if (element.addEventListener)
                    element.addEventListener(type, handle, false);
                else
                    element.attachEvent("on" + type, handle);
            }

            // Add the function to the element's handler list
            handlers[handler.guid] = handler;

            // Keep track of which events have been used, for global triggering
            this.global[type] = true;
        },

        guid: 1,
        global: {},

        // Detach an event or set of events from an element
        remove: function(element, type, handler) {
            var events = jQuery.data(element, "events"), ret, index;

            // Namespaced event handlers
            if (typeof type == "string") {
                var parts = type.split(".");
                type = parts[0];
            }

            if (events) {
                // type is actually an event object here
                if (type && type.type) {
                    handler = type.handler;
                    type = type.type;
                }

                if (!type) {
                    for (type in events)
                        this.remove(element, type);

                } else if (events[type]) {
                    // remove the given handler for the given type
                    if (handler)
                        delete events[type][handler.guid];

                    // remove all handlers for the given type
                    else
                        for (handler in events[type])
                    // Handle the removal of namespaced events
                        if (!parts[1] || events[type][handler].type == parts[1])
                        delete events[type][handler];

                    // remove generic event handler if no more handlers exist
                    for (ret in events[type]) break;
                    if (!ret) {
                        if (element.removeEventListener)
                            element.removeEventListener(type, jQuery.data(element, "handle"), false);
                        else
                            element.detachEvent("on" + type, jQuery.data(element, "handle"));
                        ret = null;
                        delete events[type];
                    }
                }

                // Remove the expando if it's no longer used
                for (ret in events) break;
                if (!ret) {
                    jQuery.removeData(element, "events");
                    jQuery.removeData(element, "handle");
                }
            }
        },

        trigger: function(type, data, element, donative, extra) {
            // Clone the incoming data, if any
            data = jQuery.makeArray(data || []);

            // Handle a global trigger
            if (!element) {
                // Only trigger if we've ever bound an event for it
                if (this.global[type])
                    jQuery("*").add([window, document]).trigger(type, data);

                // Handle triggering a single element
            } else {
                var val, ret, fn = jQuery.isFunction(element[type] || null),
                // Check to see if we need to provide a fake event, or not
				evt = !data[0] || !data[0].preventDefault;

                // Pass along a fake event
                if (evt)
                    data.unshift(this.fix({ type: type, target: element }));

                // Enforce the right trigger type
                data[0].type = type;

                // Trigger the event
                if (jQuery.isFunction(jQuery.data(element, "handle")))
                    val = jQuery.data(element, "handle").apply(element, data);

                // Handle triggering native .onfoo handlers
                if (!fn && element["on" + type] && element["on" + type].apply(element, data) === false)
                    val = false;

                // Extra functions don't get the custom event object
                if (evt)
                    data.shift();

                // Handle triggering of extra function
                if (extra && extra.apply(element, data) === false)
                    val = false;

                // Trigger the native events (except for clicks on links)
                if (fn && donative !== false && val !== false && !(jQuery.nodeName(element, 'a') && type == "click")) {
                    this.triggered = true;
                    element[type]();
                }

                this.triggered = false;
            }

            return val;
        },

        handle: function(event) {
            // returned undefined or false
            var val;

            // Empty object is for triggered events with no data
            event = jQuery.event.fix(event || window.event || {});

            // Namespaced event handlers
            var parts = event.type.split(".");
            event.type = parts[0];

            var c = jQuery.data(this, "events") && jQuery.data(this, "events")[event.type], args = Array.prototype.slice.call(arguments, 1);
            args.unshift(event);

            for (var j in c) {
                // Pass in a reference to the handler function itself
                // So that we can later remove it
                args[0].handler = c[j];
                args[0].data = c[j].data;

                // Filter the functions by class
                if (!parts[1] || c[j].type == parts[1]) {
                    var tmp = c[j].apply(this, args);

                    if (val !== false)
                        val = tmp;

                    if (tmp === false) {
                        event.preventDefault();
                        event.stopPropagation();
                    }
                }
            }

            // Clean up added properties in IE to prevent memory leak
            if (jQuery.browser.msie)
                event.target = event.preventDefault = event.stopPropagation =
				event.handler = event.data = null;

            return val;
        },

        fix: function(event) {
            // store a copy of the original event object 
            // and clone to set read-only properties
            var originalEvent = event;
            event = jQuery.extend({}, originalEvent);

            // add preventDefault and stopPropagation since 
            // they will not work on the clone
            event.preventDefault = function() {
                // if preventDefault exists run it on the original event
                if (originalEvent.preventDefault)
                    originalEvent.preventDefault();
                // otherwise set the returnValue property of the original event to false (IE)
                originalEvent.returnValue = false;
            };
            event.stopPropagation = function() {
                // if stopPropagation exists run it on the original event
                if (originalEvent.stopPropagation)
                    originalEvent.stopPropagation();
                // otherwise set the cancelBubble property of the original event to true (IE)
                originalEvent.cancelBubble = true;
            };

            // Fix target property, if necessary
            if (!event.target && event.srcElement)
                event.target = event.srcElement;

            // check if target is a textnode (safari)
            if (jQuery.browser.safari && event.target.nodeType == 3)
                event.target = originalEvent.target.parentNode;

            // Add relatedTarget, if necessary
            if (!event.relatedTarget && event.fromElement)
                event.relatedTarget = event.fromElement == event.target ? event.toElement : event.fromElement;

            // Calculate pageX/Y if missing and clientX/Y available
            if (event.pageX == null && event.clientX != null) {
                var e = document.documentElement, b = document.body;
                event.pageX = event.clientX + (e && e.scrollLeft || b.scrollLeft || 0);
                event.pageY = event.clientY + (e && e.scrollTop || b.scrollTop || 0);
            }

            // Add which for key events
            if (!event.which && (event.charCode || event.keyCode))
                event.which = event.charCode || event.keyCode;

            // Add metaKey to non-Mac browsers (use ctrl for PC's and Meta for Macs)
            if (!event.metaKey && event.ctrlKey)
                event.metaKey = event.ctrlKey;

            // Add which for click: 1 == left; 2 == middle; 3 == right
            // Note: button is not normalized, so don't use it
            if (!event.which && event.button)
                event.which = (event.button & 1 ? 1 : (event.button & 2 ? 3 : (event.button & 4 ? 2 : 0)));

            return event;
        }
    };

    jQuery.fn.extend({
        bind: function(type, data, fn) {
            return type == "unload" ? this.one(type, data, fn) : this.each(function() {
                jQuery.event.add(this, type, fn || data, fn && data);
            });
        },

        one: function(type, data, fn) {
            return this.each(function() {
                jQuery.event.add(this, type, function(event) {
                    jQuery(this).unbind(event);
                    return (fn || data).apply(this, arguments);
                }, fn && data);
            });
        },

        unbind: function(type, fn) {
            return this.each(function() {
                jQuery.event.remove(this, type, fn);
            });
        },

        trigger: function(type, data, fn) {
            return this.each(function() {
                jQuery.event.trigger(type, data, this, true, fn);
            });
        },

        triggerHandler: function(type, data, fn) {
            if (this[0])
                return jQuery.event.trigger(type, data, this[0], false, fn);
        },

        toggle: function() {
            // Save reference to arguments for access in closure
            var a = arguments;

            return this.click(function(e) {
                // Figure out which function to execute
                this.lastToggle = 0 == this.lastToggle ? 1 : 0;

                // Make sure that clicks stop
                e.preventDefault();

                // and execute the function
                return a[this.lastToggle].apply(this, [e]) || false;
            });
        },

        hover: function(f, g) {

            // A private function for handling mouse 'hovering'
            function handleHover(e) {
                // Check if mouse(over|out) are still within the same parent element
                var p = e.relatedTarget;

                // Traverse up the tree
                while (p && p != this) try { p = p.parentNode; } catch (e) { p = this; };

                // If we actually just moused on to a sub-element, ignore it
                if (p == this) return false;

                // Execute the right function
                return (e.type == "mouseover" ? f : g).apply(this, [e]);
            }

            // Bind the function to the two event listeners
            return this.mouseover(handleHover).mouseout(handleHover);
        },

        ready: function(f) {
            // Attach the listeners
            bindReady();

            // If the DOM is already ready
            if (jQuery.isReady)
            // Execute the function immediately
                f.apply(document, [jQuery]);

            // Otherwise, remember the function for later
            else
            // Add the function to the wait list
                jQuery.readyList.push(function() { return f.apply(this, [jQuery]); });

            return this;
        }
    });

    jQuery.extend({
        /*
        * All the code that makes DOM Ready work nicely.
        */
        isReady: false,
        readyList: [],

        // Handle when the DOM is ready
        ready: function() {
            // Make sure that the DOM is not already loaded
            if (!jQuery.isReady) {
                // Remember that the DOM is ready
                jQuery.isReady = true;

                // If there are functions bound, to execute
                if (jQuery.readyList) {
                    // Execute all of them
                    jQuery.each(jQuery.readyList, function() {
                        this.apply(document);
                    });

                    // Reset the list of functions
                    jQuery.readyList = null;
                }
                // Remove event listener to avoid memory leak
                if (jQuery.browser.mozilla || jQuery.browser.opera)
                    document.removeEventListener("DOMContentLoaded", jQuery.ready, false);

                // Remove script element used by IE hack
                if (!window.frames.length) // don't remove if frames are present (#1187)
                    jQuery(window).load(function() { jQuery("#__ie_init").remove(); });
            }
        }
    });

    jQuery.each(("blur,focus,load,resize,scroll,unload,click,dblclick," +
	"mousedown,mouseup,mousemove,mouseover,mouseout,change,select," +
	"submit,keydown,keypress,keyup,error").split(","), function(i, o) {

	    // Handle event binding
	    jQuery.fn[o] = function(f) {
	        return f ? this.bind(o, f) : this.trigger(o);
	    };
	});

    var readyBound = false;

    function bindReady() {
        if (readyBound) return;
        readyBound = true;

        // If Mozilla is used
        if (jQuery.browser.mozilla || jQuery.browser.opera)
        // Use the handy event callback
            document.addEventListener("DOMContentLoaded", jQuery.ready, false);

        // If IE is used, use the excellent hack by Matthias Miller
        // http://www.outofhanwell.com/blog/index.php?title=the_window_onload_problem_revisited
        else if (jQuery.browser.msie) {

            // Only works if you document.write() it
            document.write("<scr" + "ipt id=__ie_init defer=true " +
			"src=//:><\/script>");

            // Use the defer script hack
            var script = document.getElementById("__ie_init");

            // script does not exist if jQuery is loaded dynamically
            if (script)
                script.onreadystatechange = function() {
                    if (this.readyState != "complete") return;
                    jQuery.ready();
                };

            // Clear from memory
            script = null;

            // If Safari  is used
        } else if (jQuery.browser.safari)
        // Continually check to see if the document.readyState is valid
            jQuery.safariTimer = setInterval(function() {
                // loaded and complete are both valid states
                if (document.readyState == "loaded" ||
				document.readyState == "complete") {

                    // If either one are found, remove the timer
                    clearInterval(jQuery.safariTimer);
                    jQuery.safariTimer = null;

                    // and execute any waiting functions
                    jQuery.ready();
                }
            }, 10);

        // A fallback to window.onload, that will always work
        jQuery.event.add(window, "load", jQuery.ready);
    }
    jQuery.fn.extend({
        load: function(url, params, callback) {
            if (jQuery.isFunction(url))
                return this.bind("load", url);

            var off = url.indexOf(" ");
            if (off >= 0) {
                var selector = url.slice(off, url.length);
                url = url.slice(0, off);
            }

            callback = callback || function() { };

            // Default to a GET request
            var type = "GET";

            // If the second parameter was provided
            if (params)
            // If it's a function
                if (jQuery.isFunction(params)) {
                // We assume that it's the callback
                callback = params;
                params = null;

                // Otherwise, build a param string
            } else {
                params = jQuery.param(params);
                type = "POST";
            }

            var self = this;

            // Request the remote document
            jQuery.ajax({
                url: url,
                type: type,
                data: params,
                complete: function(res, status) {
                    // If successful, inject the HTML into all the matched elements
                    if (status == "success" || status == "notmodified")
                    // See if a selector was specified
                        self.html(selector ?
                    // Create a dummy div to hold the results
						jQuery("<div/>")
                    // inject the contents of the document in, removing the scripts
                    // to avoid any 'Permission Denied' errors in IE
							.append(res.responseText.replace(/<script(.|\s)*?\/script>/g, ""))

                    // Locate the specified elements
							.find(selector) :

                    // If not, just inject the full result
						res.responseText);

                    // Add delay to account for Safari's delay in globalEval
                    setTimeout(function() {
                        self.each(callback, [res.responseText, status, res]);
                    }, 13);
                }
            });
            return this;
        },

        serialize: function() {
            return jQuery.param(this.serializeArray());
        },
        serializeArray: function() {
            return this.map(function() {
                return jQuery.nodeName(this, "form") ?
				jQuery.makeArray(this.elements) : this;
            })
		.filter(function() {
		    return this.name && !this.disabled &&
				(this.checked || /select|textarea/i.test(this.nodeName) ||
					/text|hidden|password/i.test(this.type));
		})
		.map(function(i, elem) {
		    var val = jQuery(this).val();
		    return val == null ? null :
				val.constructor == Array ?
					jQuery.map(val, function(val, i) {
					    return { name: elem.name, value: val };
					}) :
					{ name: elem.name, value: val };
		}).get();
        }
    });

    // Attach a bunch of functions for handling common AJAX events
    jQuery.each("ajaxStart,ajaxStop,ajaxComplete,ajaxError,ajaxSuccess,ajaxSend".split(","), function(i, o) {
        jQuery.fn[o] = function(f) {
            return this.bind(o, f);
        };
    });

    var jsc = (new Date).getTime();

    jQuery.extend({
        get: function(url, data, callback, type) {
            // shift arguments if data argument was ommited
            if (jQuery.isFunction(data)) {
                callback = data;
                data = null;
            }

            return jQuery.ajax({
                type: "GET",
                url: url,
                data: data,
                success: callback,
                dataType: type
            });
        },

        getScript: function(url, callback) {
            return jQuery.get(url, null, callback, "script");
        },

        getJSON: function(url, data, callback) {
            return jQuery.get(url, data, callback, "json");
        },

        post: function(url, data, callback, type) {
            if (jQuery.isFunction(data)) {
                callback = data;
                data = {};
            }

            return jQuery.ajax({
                type: "POST",
                url: url,
                data: data,
                success: callback,
                dataType: type
            });
        },

        ajaxSetup: function(settings) {
            jQuery.extend(jQuery.ajaxSettings, settings);
        },

        ajaxSettings: {
            global: true,
            type: "GET",
            timeout: 0,
            contentType: "application/x-www-form-urlencoded",
            processData: true,
            async: true,
            data: null
        },

        // Last-Modified header cache for next request
        lastModified: {},

        ajax: function(s) {
            var jsonp, jsre = /=(\?|%3F)/g, status, data;

            // Extend the settings, but re-extend 's' so that it can be
            // checked again later (in the test suite, specifically)
            s = jQuery.extend(true, s, jQuery.extend(true, {}, jQuery.ajaxSettings, s));

            // convert data if not already a string
            if (s.data && s.processData && typeof s.data != "string")
                s.data = jQuery.param(s.data);

            // Handle JSONP Parameter Callbacks
            if (s.dataType == "jsonp") {
                if (s.type.toLowerCase() == "get") {
                    if (!s.url.match(jsre))
                        s.url += (s.url.match(/\?/) ? "&" : "?") + (s.jsonp || "callback") + "=?";
                } else if (!s.data || !s.data.match(jsre))
                    s.data = (s.data ? s.data + "&" : "") + (s.jsonp || "callback") + "=?";
                s.dataType = "json";
            }

            // Build temporary JSONP function
            if (s.dataType == "json" && (s.data && s.data.match(jsre) || s.url.match(jsre))) {
                jsonp = "jsonp" + jsc++;

                // Replace the =? sequence both in the query string and the data
                if (s.data)
                    s.data = s.data.replace(jsre, "=" + jsonp);
                s.url = s.url.replace(jsre, "=" + jsonp);

                // We need to make sure
                // that a JSONP style response is executed properly
                s.dataType = "script";

                // Handle JSONP-style loading
                window[jsonp] = function(tmp) {
                    data = tmp;
                    success();
                    complete();
                    // Garbage collect
                    window[jsonp] = undefined;
                    try { delete window[jsonp]; } catch (e) { }
                };
            }

            if (s.dataType == "script" && s.cache == null)
                s.cache = false;

            if (s.cache === false && s.type.toLowerCase() == "get")
                s.url += (s.url.match(/\?/) ? "&" : "?") + "_=" + (new Date()).getTime();

            // If data is available, append data to url for get requests
            if (s.data && s.type.toLowerCase() == "get") {
                s.url += (s.url.match(/\?/) ? "&" : "?") + s.data;

                // IE likes to send both get and post data, prevent this
                s.data = null;
            }

            // Watch for a new set of requests
            if (s.global && !jQuery.active++)
                jQuery.event.trigger("ajaxStart");

            // If we're requesting a remote document
            // and trying to load JSON or Script
            if (!s.url.indexOf("http") && s.dataType == "script") {
                var head = document.getElementsByTagName("head")[0];
                var script = document.createElement("script");
                script.src = s.url;

                // Handle Script loading
                if (!jsonp && (s.success || s.complete)) {
                    var done = false;

                    // Attach handlers for all browsers
                    script.onload = script.onreadystatechange = function() {
                        if (!done && (!this.readyState ||
							this.readyState == "loaded" || this.readyState == "complete")) {
                            done = true;
                            success();
                            complete();
                            head.removeChild(script);
                        }
                    };
                }

                head.appendChild(script);

                // We handle everything using the script element injection
                return;
            }

            var requestDone = false;

            // Create the request object; Microsoft failed to properly
            // implement the XMLHttpRequest in IE7, so we use the ActiveXObject when it is available
            var xml = window.ActiveXObject ? new ActiveXObject("Microsoft.XMLHTTP") : new XMLHttpRequest();

            // Open the socket
            xml.open(s.type, s.url, s.async);

            // Set the correct header, if data is being sent
            if (s.data)
                xml.setRequestHeader("Content-Type", s.contentType);

            // Set the If-Modified-Since header, if ifModified mode.
            if (s.ifModified)
                xml.setRequestHeader("If-Modified-Since",
				jQuery.lastModified[s.url] || "Thu, 01 Jan 1970 00:00:00 GMT");

            // Set header so the called script knows that it's an XMLHttpRequest
            xml.setRequestHeader("X-Requested-With", "XMLHttpRequest");

            // Allow custom headers/mimetypes
            if (s.beforeSend)
                s.beforeSend(xml);

            if (s.global)
                jQuery.event.trigger("ajaxSend", [xml, s]);

            // Wait for a response to come back
            var onreadystatechange = function(isTimeout) {
                // The transfer is complete and the data is available, or the request timed out
                if (!requestDone && xml && (xml.readyState == 4 || isTimeout == "timeout")) {
                    requestDone = true;

                    // clear poll interval
                    if (ival) {
                        clearInterval(ival);
                        ival = null;
                    }

                    status = isTimeout == "timeout" && "timeout" ||
					!jQuery.httpSuccess(xml) && "error" ||
					s.ifModified && jQuery.httpNotModified(xml, s.url) && "notmodified" ||
					"success";

                    if (status == "success") {
                        // Watch for, and catch, XML document parse errors
                        try {
                            // process the data (runs the xml through httpData regardless of callback)
                            data = jQuery.httpData(xml, s.dataType);
                        } catch (e) {
                            status = "parsererror";
                        }
                    }

                    // Make sure that the request was successful or notmodified
                    if (status == "success") {
                        // Cache Last-Modified header, if ifModified mode.
                        var modRes;
                        try {
                            modRes = xml.getResponseHeader("Last-Modified");
                        } catch (e) { } // swallow exception thrown by FF if header is not available

                        if (s.ifModified && modRes)
                            jQuery.lastModified[s.url] = modRes;

                        // JSONP handles its own success callback
                        if (!jsonp)
                            success();
                    } else
                        jQuery.handleError(s, xml, status);

                    // Fire the complete handlers
                    complete();

                    // Stop memory leaks
                    if (s.async)
                        xml = null;
                }
            };

            if (s.async) {
                // don't attach the handler to the request, just poll it instead
                var ival = setInterval(onreadystatechange, 13);

                // Timeout checker
                if (s.timeout > 0)
                    setTimeout(function() {
                        // Check to see if the request is still happening
                        if (xml) {
                            // Cancel the request
                            xml.abort();

                            if (!requestDone)
                                onreadystatechange("timeout");
                        }
                    }, s.timeout);
            }

            // Send the data
            try {
                xml.send(s.data);
            } catch (e) {
                jQuery.handleError(s, xml, null, e);
            }

            // firefox 1.5 doesn't fire statechange for sync requests
            if (!s.async)
                onreadystatechange();

            // return XMLHttpRequest to allow aborting the request etc.
            return xml;

            function success() {
                // If a local callback was specified, fire it and pass it the data
                if (s.success)
                    s.success(data, status);

                // Fire the global callback
                if (s.global)
                    jQuery.event.trigger("ajaxSuccess", [xml, s]);
            }

            function complete() {
                // Process result
                if (s.complete)
                    s.complete(xml, status);

                // The request was completed
                if (s.global)
                    jQuery.event.trigger("ajaxComplete", [xml, s]);

                // Handle the global AJAX counter
                if (s.global && ! --jQuery.active)
                    jQuery.event.trigger("ajaxStop");
            }
        },

        handleError: function(s, xml, status, e) {
            // If a local callback was specified, fire it
            if (s.error) s.error(xml, status, e);

            // Fire the global callback
            if (s.global)
                jQuery.event.trigger("ajaxError", [xml, s, e]);
        },

        // Counter for holding the number of active queries
        active: 0,

        // Determines if an XMLHttpRequest was successful or not
        httpSuccess: function(r) {
            try {
                return !r.status && location.protocol == "file:" ||
				(r.status >= 200 && r.status < 300) || r.status == 304 ||
				jQuery.browser.safari && r.status == undefined;
            } catch (e) { }
            return false;
        },

        // Determines if an XMLHttpRequest returns NotModified
        httpNotModified: function(xml, url) {
            try {
                var xmlRes = xml.getResponseHeader("Last-Modified");

                // Firefox always returns 200. check Last-Modified date
                return xml.status == 304 || xmlRes == jQuery.lastModified[url] ||
				jQuery.browser.safari && xml.status == undefined;
            } catch (e) { }
            return false;
        },

        httpData: function(r, type) {
            var ct = r.getResponseHeader("content-type");
            var xml = type == "xml" || !type && ct && ct.indexOf("xml") >= 0;
            var data = xml ? r.responseXML : r.responseText;

            if (xml && data.documentElement.tagName == "parsererror")
                throw "parsererror";

            // If the type is "script", eval it in global context
            if (type == "script")
                jQuery.globalEval(data);

            // Get the JavaScript object, if JSON is used.
            if (type == "json")
                data = eval("(" + data + ")");

            return data;
        },

        // Serialize an array of form elements or a set of
        // key/values into a query string
        param: function(a) {
            var s = [];

            // If an array was passed in, assume that it is an array
            // of form elements
            if (a.constructor == Array || a.jquery)
            // Serialize the form elements
                jQuery.each(a, function() {
                    s.push(encodeURIComponent(this.name) + "=" + encodeURIComponent(this.value));
                });

            // Otherwise, assume that it's an object of key/value pairs
            else
            // Serialize the key/values
                for (var j in a)
            // If the value is an array then the key names need to be repeated
                if (a[j] && a[j].constructor == Array)
                jQuery.each(a[j], function() {
                    s.push(encodeURIComponent(j) + "=" + encodeURIComponent(this));
                });
            else
                s.push(encodeURIComponent(j) + "=" + encodeURIComponent(a[j]));

            // Return the resulting serialization
            return s.join("&").replace(/%20/g, "+");
        }

    });
    jQuery.fn.extend({
        show: function(speed, callback) {
            return speed ?
			this.animate({
			    height: "show", width: "show", opacity: "show"
			}, speed, callback) :

			this.filter(":hidden").each(function() {
			    this.style.display = this.oldblock ? this.oldblock : "";
			    if (jQuery.css(this, "display") == "none")
			        this.style.display = "block";
			}).end();
        },

        hide: function(speed, callback) {
            return speed ?
			this.animate({
			    height: "hide", width: "hide", opacity: "hide"
			}, speed, callback) :

			this.filter(":visible").each(function() {
			    this.oldblock = this.oldblock || jQuery.css(this, "display");
			    if (this.oldblock == "none")
			        this.oldblock = "block";
			    if (this.style) {
			        this.style.display = "none";
			    }
			}).end();
        },

        // Save the old toggle function
        _toggle: jQuery.fn.toggle,

        toggle: function(fn, fn2) {
            return jQuery.isFunction(fn) && jQuery.isFunction(fn2) ?
			this._toggle(fn, fn2) :
			fn ?
				this.animate({
				    height: "toggle", width: "toggle", opacity: "toggle"
				}, fn, fn2) :
				this.each(function() {
				    jQuery(this)[jQuery(this).is(":hidden") ? "show" : "hide"]();
				});
        },

        slideDown: function(speed, callback) {
            return this.animate({ height: "show" }, speed, callback);
        },

        slideUp: function(speed, callback) {
            return this.animate({ height: "hide" }, speed, callback);
        },

        slideToggle: function(speed, callback) {
            return this.animate({ height: "toggle" }, speed, callback);
        },

        fadeIn: function(speed, callback) {
            return this.animate({ opacity: "show" }, speed, callback);
        },

        fadeOut: function(speed, callback) {
            return this.animate({ opacity: "hide" }, speed, callback);
        },

        fadeTo: function(speed, to, callback) {
            return this.animate({ opacity: to }, speed, callback);
        },

        animate: function(prop, speed, easing, callback) {
            var opt = jQuery.speed(speed, easing, callback);

            return this[opt.queue === false ? "each" : "queue"](function() {
                opt = jQuery.extend({}, opt);
                var hidden = jQuery(this).is(":hidden"), self = this;

                for (var p in prop) {
                    if (prop[p] == "hide" && hidden || prop[p] == "show" && !hidden)
                        return jQuery.isFunction(opt.complete) && opt.complete.apply(this);

                    if (p == "height" || p == "width") {
                        // Store display property
                        opt.display = jQuery.css(this, "display");

                        // Make sure that nothing sneaks out
                        opt.overflow = this.style.overflow;
                    }
                }

                if (opt.overflow != null)
                    this.style.overflow = "hidden";

                opt.curAnim = jQuery.extend({}, prop);

                jQuery.each(prop, function(name, val) {
                    var e = new jQuery.fx(self, opt, name);

                    if (/toggle|show|hide/.test(val))
                        e[val == "toggle" ? hidden ? "show" : "hide" : val](prop);
                    else {
                        var parts = val.toString().match(/^([+-]=)?([\d+-.]+)(.*)$/),
						start = e.cur(true) || 0;

                        if (parts) {
                            var end = parseFloat(parts[2]),
							unit = parts[3] || "px";

                            // We need to compute starting value
                            if (unit != "px") {
                                self.style[name] = (end || 1) + unit;
                                start = ((end || 1) / e.cur(true)) * start;
                                self.style[name] = start + unit;
                            }

                            // If a +=/-= token was provided, we're doing a relative animation
                            if (parts[1])
                                end = ((parts[1] == "-=" ? -1 : 1) * end) + start;

                            e.custom(start, end, unit);
                        } else
                            e.custom(start, val, "");
                    }
                });

                // For JS strict compliance
                return true;
            });
        },

        queue: function(type, fn) {
            if (jQuery.isFunction(type)) {
                fn = type;
                type = "fx";
            }

            if (!type || (typeof type == "string" && !fn))
                return queue(this[0], type);

            return this.each(function() {
                if (fn.constructor == Array)
                    queue(this, type, fn);
                else {
                    queue(this, type).push(fn);

                    if (queue(this, type).length == 1)
                        fn.apply(this);
                }
            });
        },

        stop: function() {
            var timers = jQuery.timers;

            return this.each(function() {
                for (var i = 0; i < timers.length; i++)
                    if (timers[i].elem == this)
                    timers.splice(i--, 1);
            }).dequeue();
        }

    });

    var queue = function(elem, type, array) {
        if (!elem)
            return;

        var q = jQuery.data(elem, type + "queue");

        if (!q || array)
            q = jQuery.data(elem, type + "queue",
			array ? jQuery.makeArray(array) : []);

        return q;
    };

    jQuery.fn.dequeue = function(type) {
        type = type || "fx";

        return this.each(function() {
            var q = queue(this, type);

            q.shift();

            if (q.length)
                q[0].apply(this);
        });
    };

    jQuery.extend({

        speed: function(speed, easing, fn) {
            var opt = speed && speed.constructor == Object ? speed : {
                complete: fn || !fn && easing ||
				jQuery.isFunction(speed) && speed,
                duration: speed,
                easing: fn && easing || easing && easing.constructor != Function && easing
            };

            opt.duration = (opt.duration && opt.duration.constructor == Number ?
			opt.duration :
			{ slow: 600, fast: 200}[opt.duration]) || 400;

            // Queueing
            opt.old = opt.complete;
            opt.complete = function() {
                jQuery(this).dequeue();
                if (jQuery.isFunction(opt.old))
                    opt.old.apply(this);
            };

            return opt;
        },

        easing: {
            linear: function(p, n, firstNum, diff) {
                return firstNum + diff * p;
            },
            swing: function(p, n, firstNum, diff) {
                return ((-Math.cos(p * Math.PI) / 2) + 0.5) * diff + firstNum;
            }
        },

        timers: [],

        fx: function(elem, options, prop) {
            this.options = options;
            this.elem = elem;
            this.prop = prop;

            if (!options.orig)
                options.orig = {};
        }

    });

    jQuery.fx.prototype = {

        // Simple function for setting a style value
        update: function() {
            if (this.options.step)
                this.options.step.apply(this.elem, [this.now, this]);

            (jQuery.fx.step[this.prop] || jQuery.fx.step._default)(this);

            // Set display property to block for height/width animations
            if (this.prop == "height" || this.prop == "width")
                this.elem.style.display = "block";
        },

        // Get the current size
        cur: function(force) {
            if (this.elem[this.prop] != null && this.elem.style[this.prop] == null)
                return this.elem[this.prop];

            var r = parseFloat(jQuery.curCSS(this.elem, this.prop, force));
            return r && r > -10000 ? r : parseFloat(jQuery.css(this.elem, this.prop)) || 0;
        },

        // Start an animation from one number to another
        custom: function(from, to, unit) {
            this.startTime = (new Date()).getTime();
            this.start = from;
            this.end = to;
            this.unit = unit || this.unit || "px";
            this.now = this.start;
            this.pos = this.state = 0;
            this.update();

            var self = this;
            function t() {
                return self.step();
            }

            t.elem = this.elem;

            jQuery.timers.push(t);

            if (jQuery.timers.length == 1) {
                var timer = setInterval(function() {
                    var timers = jQuery.timers;

                    for (var i = 0; i < timers.length; i++)
                        if (!timers[i]())
                        timers.splice(i--, 1);

                    if (!timers.length)
                        clearInterval(timer);
                }, 13);
            }
        },

        // Simple 'show' function
        show: function() {
            // Remember where we started, so that we can go back to it later
            this.options.orig[this.prop] = jQuery.attr(this.elem.style, this.prop);
            this.options.show = true;

            // Begin the animation
            this.custom(0, this.cur());

            // Make sure that we start at a small width/height to avoid any
            // flash of content
            if (this.prop == "width" || this.prop == "height")
                this.elem.style[this.prop] = "1px";

            // Start by showing the element
            jQuery(this.elem).show();
        },

        // Simple 'hide' function
        hide: function() {
            // Remember where we started, so that we can go back to it later
            this.options.orig[this.prop] = jQuery.attr(this.elem.style, this.prop);
            this.options.hide = true;

            // Begin the animation
            this.custom(this.cur(), 0);
        },

        // Each step of an animation
        step: function() {
            var t = (new Date()).getTime();

            if (t > this.options.duration + this.startTime) {
                this.now = this.end;
                this.pos = this.state = 1;
                this.update();

                this.options.curAnim[this.prop] = true;

                var done = true;
                for (var i in this.options.curAnim)
                    if (this.options.curAnim[i] !== true)
                    done = false;

                if (done) {
                    if (this.options.display != null) {
                        // Reset the overflow
                        this.elem.style.overflow = this.options.overflow;

                        // Reset the display
                        this.elem.style.display = this.options.display;
                        if (jQuery.css(this.elem, "display") == "none")
                            this.elem.style.display = "block";
                    }

                    // Hide the element if the "hide" operation was done
                    if (this.options.hide)
                        this.elem.style.display = "none";

                    // Reset the properties, if the item has been hidden or shown
                    if (this.options.hide || this.options.show)
                        for (var p in this.options.curAnim)
                        jQuery.attr(this.elem.style, p, this.options.orig[p]);
                }

                // If a callback was provided, execute it
                if (done && jQuery.isFunction(this.options.complete))
                // Execute the complete function
                    this.options.complete.apply(this.elem);

                return false;
            } else {
                var n = t - this.startTime;
                this.state = n / this.options.duration;

                // Perform the easing function, defaults to swing
                this.pos = jQuery.easing[this.options.easing || (jQuery.easing.swing ? "swing" : "linear")](this.state, n, 0, 1, this.options.duration);
                this.now = this.start + ((this.end - this.start) * this.pos);

                // Perform the next step of the animation
                this.update();
            }

            return true;
        }

    };

    jQuery.fx.step = {
        scrollLeft: function(fx) {
            fx.elem.scrollLeft = fx.now;
        },

        scrollTop: function(fx) {
            fx.elem.scrollTop = fx.now;
        },

        opacity: function(fx) {
            jQuery.attr(fx.elem.style, "opacity", fx.now);
        },

        _default: function(fx) {
            fx.elem.style[fx.prop] = fx.now + fx.unit;
        }
    };
    // The Offset Method
    // Originally By Brandon Aaron, part of the Dimension Plugin
    // http://jquery.com/plugins/project/dimensions
    jQuery.fn.offset = function() {
        var left = 0, top = 0, elem = this[0], results;

        if (elem) with (jQuery.browser) {
            var absolute = jQuery.css(elem, "position") == "absolute",
		    parent = elem.parentNode,
		    offsetParent = elem.offsetParent,
		    doc = elem.ownerDocument,
		    safari2 = safari && parseInt(version) < 522;

            // Use getBoundingClientRect if available
            if (elem.getBoundingClientRect) {
                box = elem.getBoundingClientRect();

                // Add the document scroll offsets
                add(
				box.left + Math.max(doc.documentElement.scrollLeft, doc.body.scrollLeft),
				box.top + Math.max(doc.documentElement.scrollTop, doc.body.scrollTop)
			);

                // IE adds the HTML element's border, by default it is medium which is 2px
                // IE 6 and IE 7 quirks mode the border width is overwritable by the following css html { border: 0; }
                // IE 7 standards mode, the border is always 2px
                if (msie) {
                    var border = jQuery("html").css("borderWidth");
                    border = (border == "medium" || jQuery.boxModel && parseInt(version) >= 7) && 2 || border;
                    add(-border, -border);
                }

                // Otherwise loop through the offsetParents and parentNodes
            } else {

                // Initial element offsets
                add(elem.offsetLeft, elem.offsetTop);

                // Get parent offsets
                while (offsetParent) {
                    // Add offsetParent offsets
                    add(offsetParent.offsetLeft, offsetParent.offsetTop);

                    // Mozilla and Safari > 2 does not include the border on offset parents
                    // However Mozilla adds the border for table cells
                    if (mozilla && /^t[d|h]$/i.test(parent.tagName) || !safari2)
                        border(offsetParent);

                    // Safari <= 2 doubles body offsets with an absolutely positioned element or parent
                    if (safari2 && !absolute && jQuery.css(offsetParent, "position") == "absolute")
                        absolute = true;

                    // Get next offsetParent
                    offsetParent = offsetParent.offsetParent;
                }

                // Get parent scroll offsets
                while (parent.tagName && !/^body|html$/i.test(parent.tagName)) {
                    // Work around opera inline/table scrollLeft/Top bug
                    if (!/^inline|table-row.*$/i.test(jQuery.css(parent, "display")))
                    // Subtract parent scroll offsets
                        add(-parent.scrollLeft, -parent.scrollTop);

                    // Mozilla does not add the border for a parent that has overflow != visible
                    if (mozilla && jQuery.css(parent, "overflow") != "visible")
                        border(parent);

                    // Get next parent
                    parent = parent.parentNode;
                }

                // Safari doubles body offsets with an absolutely positioned element or parent
                if (safari2 && absolute)
                    add(-doc.body.offsetLeft, -doc.body.offsetTop);
            }

            // Return an object with top and left properties
            results = { top: top, left: left };
        }

        return results;

        function border(elem) {
            add(jQuery.css(elem, "borderLeftWidth"), jQuery.css(elem, "borderTopWidth"));
        }

        function add(l, t) {
            left += parseInt(l) || 0;
            top += parseInt(t) || 0;
        }
    };
})();
/* End json.js */
/* Start ui.datePicker.js */
/* jQuery UI Date Picker v3.4.3 (previously jQuery Calendar)
Written by Marc Grabanski (m@marcgrabanski.com) and Keith Wood (kbwood@virginbroadband.com.au).

Copyright (c) 2007 Marc Grabanski (http://marcgrabanski.com/code/ui-datepicker)
Dual licensed under the MIT (MIT-LICENSE.txt)
and GPL (GPL-LICENSE.txt) licenses.
Date: 09-03-2007  */

; (function($) { // hide the namespace

    /* Date picker manager.
    Use the singleton instance of this class, $.datepicker, to interact with the date picker.
    Settings for (groups of) date pickers are maintained in an instance object
    (DatepickerInstance), allowing multiple different settings on the same page. */

    function Datepicker() {
        this.debug = false; // Change this to true to start debugging
        this._nextId = 0; // Next ID for a date picker instance
        this._inst = []; // List of instances indexed by ID
        this._curInst = null; // The current instance in use
        this._disabledInputs = []; // List of date picker inputs that have been disabled
        this._datepickerShowing = false; // True if the popup picker is showing , false if not
        this._inDialog = false; // True if showing within a "dialog", false if not
        this.regional = []; // Available regional settings, indexed by language code
        this.regional[''] = { // Default regional settings
            clearText: 'Clear', // Display text for clear link
            clearStatus: 'Erase the current date', // Status text for clear link
            closeText: 'Close', // Display text for close link
            closeStatus: 'Close without change', // Status text for close link
            prevText: '&#x3c;Prev', // Display text for previous month link
            prevStatus: 'Show the previous month', // Status text for previous month link
            nextText: 'Next&#x3e;', // Display text for next month link
            nextStatus: 'Show the next month', // Status text for next month link
            currentText: 'Today', // Display text for current month link
            currentStatus: 'Show the current month', // Status text for current month link
            monthNames: ['January', 'February', 'March', 'April', 'May', 'June',
			'July', 'August', 'September', 'October', 'November', 'December'], // Names of months for drop-down and formatting
            monthNamesShort: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'], // For formatting
            monthStatus: 'Show a different month', // Status text for selecting a month
            yearStatus: 'Show a different year', // Status text for selecting a year
            weekHeader: 'Wk', // Header for the week of the year column
            weekStatus: 'Week of the year', // Status text for the week of the year column
            dayNames: ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'], // For formatting
            dayNamesShort: ['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'], // For formatting
            dayNamesMin: ['Su', 'Mo', 'Tu', 'We', 'Th', 'Fr', 'Sa'], // Column headings for days starting at Sunday
            dayStatus: 'Set DD as first week day', // Status text for the day of the week selection
            dateStatus: 'Select DD, M d', // Status text for the date selection
            dateFormat: 'mm/dd/yy', // See format options on parseDate
            firstDay: 0, // The first day of the week, Sun = 0, Mon = 1, ...
            initStatus: 'Select a date', // Initial Status text on opening
            isRTL: false // True if right-to-left language, false if left-to-right
        };
        this._defaults = { // Global defaults for all the date picker instances
            showOn: 'focus', // 'focus' for popup on focus,
            // 'button' for trigger button, or 'both' for either
            showAnim: 'show', // Name of jQuery animation for popup
            defaultDate: null, // Used when field is blank: actual date,
            // +/-number for offset from today, null for today
            appendText: '', // Display text following the input box, e.g. showing the format
            buttonText: '...', // Text for trigger button
            buttonImage: '', // URL for trigger button image
            buttonImageOnly: false, // True if the image appears alone, false if it appears on a button
            closeAtTop: true, // True to have the clear/close at the top,
            // false to have them at the bottom
            mandatory: false, // True to hide the Clear link, false to include it
            hideIfNoPrevNext: false, // True to hide next/previous month links
            // if not applicable, false to just disable them
            changeMonth: true, // True if month can be selected directly, false if only prev/next
            changeYear: true, // True if year can be selected directly, false if only prev/next
            yearRange: '-10:+10', // Range of years to display in drop-down,
            // either relative to current year (-nn:+nn) or absolute (nnnn:nnnn)
            changeFirstDay: false, // True to click on day name to change, false to remain as set
            showOtherMonths: false, // True to show dates in other months, false to leave blank
            showWeeks: false, // True to show week of the year, false to omit
            calculateWeek: this.iso8601Week, // How to calculate the week of the year,
            // takes a Date and returns the number of the week for it
            shortYearCutoff: '+10', // Short year values < this are in the current century,
            // > this are in the previous century, 
            // string value starting with '+' for current year + value
            showStatus: false, // True to show status bar at bottom, false to not show it
            statusForDate: this.dateStatus, // Function to provide status text for a date -
            // takes date and instance as parameters, returns display text
            minDate: null, // The earliest selectable date, or null for no limit
            maxDate: null, // The latest selectable date, or null for no limit
            speed: 'normal', // Speed of display/closure
            beforeShowDay: null, // Function that takes a date and returns an array with
            // [0] = true if selectable, false if not,
            // [1] = custom CSS class name(s) or '', e.g. $.datepicker.noWeekends
            beforeShow: null, // Function that takes an input field and
            // returns a set of custom settings for the date picker
            onSelect: null, // Define a callback function when a date is selected
            onClose: null, // Define a callback function when the datepicker is closed
            numberOfMonths: 1, // Number of months to show at a time
            stepMonths: 1, // Number of months to step back/forward
            rangeSelect: false, // Allows for selecting a date range on one date picker
            rangeSeparator: ' - ' // Text between two dates in a range
        };
        $.extend(this._defaults, this.regional['']);
        this._datepickerDiv = $('<div id="datepicker_div">');
    }

    $.extend(Datepicker.prototype, {
        /* Class name added to elements to indicate already configured with a date picker. */
        markerClassName: 'hasDatepicker',

        /* Debug logging (if enabled). */
        log: function() {
            if (this.debug)
                console.log.apply('', arguments);
        },

        /* Register a new date picker instance - with custom settings. */
        _register: function(inst) {
            var id = this._nextId++;
            this._inst[id] = inst;
            return id;
        },

        /* Retrieve a particular date picker instance based on its ID. */
        _getInst: function(id) {
            return this._inst[id] || id;
        },

        /* Override the default settings for all instances of the date picker. 
        @param  settings  object - the new settings to use as defaults (anonymous object)
        @return the manager object */
        setDefaults: function(settings) {
            extendRemove(this._defaults, settings || {});
            return this;
        },

        /* Attach the date picker to a jQuery selection.
        @param  target    element - the target input field or division or span
        @param  settings  object - the new settings to use for this date picker instance (anonymous) */
        _attachDatepicker: function(target, settings) {
            // check for settings on the control itself - in namespace 'date:'
            var inlineSettings = null;
            for (attrName in this._defaults) {
                var attrValue = target.getAttribute('date:' + attrName);
                if (attrValue) {
                    inlineSettings = inlineSettings || {};
                    try {
                        inlineSettings[attrName] = eval(attrValue);
                    } catch (err) {
                        inlineSettings[attrName] = attrValue;
                    }
                }
            }
            var nodeName = target.nodeName.toLowerCase();
            var instSettings = (inlineSettings ?
			$.extend(settings || {}, inlineSettings || {}) : settings);
            if (nodeName == 'input') {
                var inst = (inst && !inlineSettings ? inst :
				new DatepickerInstance(instSettings, false));
                this._connectDatepicker(target, inst);
            } else if (nodeName == 'div' || nodeName == 'span') {
                var inst = new DatepickerInstance(instSettings, true);
                this._inlineDatepicker(target, inst);
            }
        },

        /* Detach a datepicker from its control.
        @param  target    element - the target input field or division or span */
        _destroyDatepicker: function(target) {
            var nodeName = target.nodeName.toLowerCase();
            var calId = target._calId;
            target._calId = null;
            var $target = $(target);
            if (nodeName == 'input') {
                $target.siblings('.datepicker_append').replaceWith('').end()
				.siblings('.datepicker_trigger').replaceWith('').end()
				.removeClass(this.markerClassName)
				.unbind('focus', this._showDatepicker)
				.unbind('keydown', this._doKeyDown)
				.unbind('keypress', this._doKeyPress);
                var wrapper = $target.parents('.datepicker_wrap');
                if (wrapper)
                    wrapper.replaceWith(wrapper.html());
            } else if (nodeName == 'div' || nodeName == 'span')
                $target.removeClass(this.markerClassName).empty();
            if ($('input[_calId=' + calId + ']').length == 0)
            // clean up if last for this ID
                this._inst[calId] = null;
        },

        /* Enable the date picker to a jQuery selection.
        @param  target    element - the target input field or division or span */
        _enableDatepicker: function(target) {
            target.disabled = false;
            $(target).siblings('button.datepicker_trigger').each(function() { this.disabled = false; }).end()
			.siblings('img.datepicker_trigger').css({ opacity: '1.0', cursor: '' });
            this._disabledInputs = $.map(this._disabledInputs,
			function(value) { return (value == target ? null : value); }); // delete entry
        },

        /* Disable the date picker to a jQuery selection.
        @param  target    element - the target input field or division or span */
        _disableDatepicker: function(target) {
            target.disabled = true;
            $(target).siblings('button.datepicker_trigger').each(function() { this.disabled = true; }).end()
			.siblings('img.datepicker_trigger').css({ opacity: '0.5', cursor: 'default' });
            this._disabledInputs = $.map($.datepicker._disabledInputs,
			function(value) { return (value == target ? null : value); }); // delete entry
            this._disabledInputs[$.datepicker._disabledInputs.length] = target;
        },

        /* Is the first field in a jQuery collection disabled as a datepicker?
        @param  target    element - the target input field or division or span
        @return boolean - true if disabled, false if enabled */
        _isDisabledDatepicker: function(target) {
            if (!target)
                return false;
            for (var i = 0; i < this._disabledInputs.length; i++) {
                if (this._disabledInputs[i] == target)
                    return true;
            }
            return false;
        },

        /* Update the settings for a date picker attached to an input field or division.
        @param  target  element - the target input field or division or span
        @param  name    string - the name of the setting to change or
        object - the new settings to update
        @param  value   any - the new value for the setting (omit if above is an object) */
        _changeDatepicker: function(target, name, value) {
            var settings = name || {};
            if (typeof name == 'string') {
                settings = {};
                settings[name] = value;
            }
            if (inst = this._getInst(target._calId)) {
                extendRemove(inst._settings, settings);
                this._updateDatepicker(inst);
            }
        },

        /* Set the dates for a jQuery selection.
        @param  target   element - the target input field or division or span
        @param  date     Date - the new date
        @param  endDate  Date - the new end date for a range (optional) */
        _setDateDatepicker: function(target, date, endDate) {
            if (inst = this._getInst(target._calId)) {
                inst._setDate(date, endDate);
                this._updateDatepicker(inst);
            }
        },

        /* Get the date(s) for the first entry in a jQuery selection.
        @param  target  element - the target input field or division or span
        @return Date - the current date or
        Date[2] - the current dates for a range */
        _getDateDatepicker: function(target) {
            var inst = this._getInst(target._calId);
            return (inst ? inst._getDate() : null);
        },

        /* Handle keystrokes. */
        _doKeyDown: function(e) {
            var inst = $.datepicker._getInst(this._calId);
            if ($.datepicker._datepickerShowing)
                switch (e.keyCode) {
                case 9: $.datepicker._hideDatepicker(null, '');
                    break; // hide on tab out
                case 13: $.datepicker._selectDay(inst, inst._selectedMonth, inst._selectedYear,
							$('td.datepicker_daysCellOver', inst._datepickerDiv)[0]);
                    return false; // don't submit the form
                    break; // select the value on enter
                case 27: $.datepicker._hideDatepicker(null, inst._get('speed'));
                    break; // hide on escape
                case 33: $.datepicker._adjustDate(inst,
							(e.ctrlKey ? -1 : -inst._get('stepMonths')), (e.ctrlKey ? 'Y' : 'M'));
                    break; // previous month/year on page up/+ ctrl
                case 34: $.datepicker._adjustDate(inst,
							(e.ctrlKey ? +1 : +inst._get('stepMonths')), (e.ctrlKey ? 'Y' : 'M'));
                    break; // next month/year on page down/+ ctrl
                case 35: if (e.ctrlKey) $.datepicker._clearDate(inst);
                    break; // clear on ctrl+end
                case 36: if (e.ctrlKey) $.datepicker._gotoToday(inst);
                    break; // current on ctrl+home
                case 37: if (e.ctrlKey) $.datepicker._adjustDate(inst, -1, 'D');
                    break; // -1 day on ctrl+left
                case 38: if (e.ctrlKey) $.datepicker._adjustDate(inst, -7, 'D');
                    break; // -1 week on ctrl+up
                case 39: if (e.ctrlKey) $.datepicker._adjustDate(inst, +1, 'D');
                    break; // +1 day on ctrl+right
                case 40: if (e.ctrlKey) $.datepicker._adjustDate(inst, +7, 'D');
                    break; // +1 week on ctrl+down
            }
            else if (e.keyCode == 36 && e.ctrlKey) // display the date picker on ctrl+home
                $.datepicker._showDatepicker(this);
        },

        /* Filter entered characters - based on date format. */
        _doKeyPress: function(e) {
            var inst = $.datepicker._getInst(this._calId);
            var chars = $.datepicker._possibleChars(inst._get('dateFormat'));
            var chr = String.fromCharCode(e.charCode == undefined ? e.keyCode : e.charCode);
            return e.ctrlKey || (chr < ' ' || !chars || chars.indexOf(chr) > -1);
        },

        /* Attach the date picker to an input field. */
        _connectDatepicker: function(target, inst) {
            var input = $(target);
            if (input.is('.' + this.markerClassName))
                return;
            var appendText = inst._get('appendText');
            var isRTL = inst._get('isRTL');
            if (appendText) {
                if (isRTL)
                    input.before('<span class="datepicker_append">' + appendText);
                else
                    input.after('<span class="datepicker_append">' + appendText);
            }
            var showOn = inst._get('showOn');
            if (showOn == 'focus' || showOn == 'both') // pop-up date picker when in the marked field
                input.focus(this._showDatepicker);
            if (showOn == 'button' || showOn == 'both') { // pop-up date picker when button clicked
                input.wrap('<span id="calDiv" class="datepicker_wrap">');
                var buttonText = inst._get('buttonText');
                var buttonImage = inst._get('buttonImage');
                var trigger = $(inst._get('buttonImageOnly') ?
				$('<img>').addClass('datepicker_trigger').attr({ src: buttonImage, alt: buttonText, title: buttonText }) :
				$('<button>').addClass('datepicker_trigger').attr({ type: 'button' }).html(buttonImage != '' ?
						$('<img>').attr({ src: buttonImage, alt: buttonText, title: buttonText }) : buttonText));
                if (isRTL)
                    input.before(trigger);
                else
                    input.after(trigger);
                trigger.click(function() {
                    if ($.datepicker._datepickerShowing && $.datepicker._lastInput == target)
                        $.datepicker._hideDatepicker();
                    else
                        $.datepicker._showDatepicker(target);
                });
            }
            input.addClass(this.markerClassName).keydown(this._doKeyDown).keypress(this._doKeyPress)
			.bind("setData.datepicker", function(event, key, value) {
			    inst._settings[key] = value;
			}).bind("getData.datepicker", function(event, key) {
			    return inst._get(key);
			});
            input[0]._calId = inst._id;
        },

        /* Attach an inline date picker to a div. */
        _inlineDatepicker: function(target, inst) {
            var input = $(target);
            if (input.is('.' + this.markerClassName))
                return;
            input.addClass(this.markerClassName).append(inst._datepickerDiv)
			.bind("setData.datepicker", function(event, key, value) {
			    inst._settings[key] = value;
			}).bind("getData.datepicker", function(event, key) {
			    return inst._get(key);
			});
            input[0]._calId = inst._id;
            this._updateDatepicker(inst);
        },

        /* Tidy up after displaying the date picker. */
        _inlineShow: function(inst) {
            var numMonths = inst._getNumberOfMonths(); // fix width for dynamic number of date pickers
            //inst._datepickerDiv.width(numMonths[1] * $('.datepicker', inst._datepickerDiv[0]).width());
        },

        /* Pop-up the date picker in a "dialog" box.
        @param  input     element - ignored
        @param  dateText  string - the initial date to display (in the current format)
        @param  onSelect  function - the function(dateText) to call when a date is selected
        @param  settings  object - update the dialog date picker instance's settings (anonymous object)
        @param  pos       int[2] - coordinates for the dialog's position within the screen or
        event - with x/y coordinates or
        leave empty for default (screen centre)
        @return the manager object */
        _dialogDatepicker: function(input, dateText, onSelect, settings, pos) {
            var inst = this._dialogInst; // internal instance
            if (!inst) {
                inst = this._dialogInst = new DatepickerInstance({}, false);
                this._dialogInput = $('<input type="text" size="1" style="position: absolute; top: -100px;"/>');
                this._dialogInput.keydown(this._doKeyDown);
                $('body').append(this._dialogInput);
                this._dialogInput[0]._calId = inst._id;
            }
            extendRemove(inst._settings, settings || {});
            this._dialogInput.val(dateText);

            this._pos = (pos ? (pos.length ? pos : [pos.pageX, pos.pageY]) : null);
            if (!this._pos) {
                var browserWidth = window.innerWidth || document.documentElement.clientWidth || document.body.clientWidth;
                var browserHeight = window.innerHeight || document.documentElement.clientHeight || document.body.clientHeight;
                var scrollX = document.documentElement.scrollLeft || document.body.scrollLeft;
                var scrollY = document.documentElement.scrollTop || document.body.scrollTop;
                this._pos = // should use actual width/height below
				[(browserWidth / 2) - 100 + scrollX, (browserHeight / 2) - 150 + scrollY];
            }

            // move input on screen for focus, but hidden behind dialog
            this._dialogInput.css('left', this._pos[0] + 'px').css('top', this._pos[1] + 'px');
            inst._settings.onSelect = onSelect;
            this._inDialog = true;
            this._datepickerDiv.addClass('datepicker_dialog');
            this._showDatepicker(this._dialogInput[0]);
            if ($.blockUI)
                $.blockUI(this._datepickerDiv);
            return this;
        },

        /* Pop-up the date picker for a given input field.
        @param  input  element - the input field attached to the date picker or
        event - if triggered by focus */
        _showDatepicker: function(input) {
            input = input.target || input;
            if (input.nodeName.toLowerCase() != 'input') // find from button/image trigger
                input = $('input', input.parentNode)[0];
            if ($.datepicker._isDisabledDatepicker(input) || $.datepicker._lastInput == input) // already here
                return;
            var inst = $.datepicker._getInst(input._calId);
            var beforeShow = inst._get('beforeShow');
            extendRemove(inst._settings, (beforeShow ? beforeShow.apply(input, [input, inst]) : {}));
            $.datepicker._hideDatepicker(null, '');
            $.datepicker._lastInput = input;
            inst._setDateFromField(input);
            if ($.datepicker._inDialog) // hide cursor
                input.value = '';
            if (!$.datepicker._pos) { // position below input
                $.datepicker._pos = $.datepicker._findPos(input);
                $.datepicker._pos[1] += input.offsetHeight; // add the height
            }
            var isFixed = false;
            $(input).parents().each(function() {
                isFixed |= $(this).css('position') == 'fixed';
            });
            if (isFixed && $.browser.opera) { // correction for Opera when fixed and scrolled
                $.datepicker._pos[0] -= document.documentElement.scrollLeft;
                $.datepicker._pos[1] -= document.documentElement.scrollTop;
            }
            inst._datepickerDiv.css('position', ($.datepicker._inDialog && $.blockUI ?
			'static' : (isFixed ? 'fixed' : 'absolute')))
			.css({ left: $.datepicker._pos[0] + 'px', top: $.datepicker._pos[1] + 'px' });
            $.datepicker._pos = null;
            inst._rangeStart = null;
            $.datepicker._updateDatepicker(inst);
            if (!inst._inline) {
                var speed = inst._get('speed');
                var postProcess = function() {
                    $.datepicker._datepickerShowing = true;
                    $.datepicker._afterShow(inst);
                };
                var showAnim = inst._get('showAnim') || 'show';
                inst._datepickerDiv[showAnim](speed, postProcess);
                if (speed == '')
                    postProcess();
                //if (inst._input[0].type != 'hidden')
                var isVisible = $(':visible', $(inst._input[0])).length > 0;
                if (isVisible) {
                    inst._input[0].focus();
                }
                $.datepicker._curInst = inst;
            }
        },

        /* Generate the date picker content. */
        _updateDatepicker: function(inst) {
            inst._datepickerDiv.empty().append(inst._generateDatepicker());
            var numMonths = inst._getNumberOfMonths();
            if (numMonths[0] != 1 || numMonths[1] != 1)
                inst._datepickerDiv.addClass('datepicker_multi');
            else
                inst._datepickerDiv.removeClass('datepicker_multi');

            if (inst._get('isRTL'))
                inst._datepickerDiv.addClass('datepicker_rtl');
            else
                inst._datepickerDiv.removeClass('datepicker_rtl');

            //if (inst._input && inst._input[0].type != 'hidden')

            if (inst._input) {
                var isVisible = $(':visible', $(inst._input[0])).length > 0;
                if (isVisible) {
                    inst._input[0].focus();
                }
            }
        },

        /* Tidy up after displaying the date picker. */
        _afterShow: function(inst) {
            //mortonj: Comment out next 2 lines to make the months stack on top of each other.
            var numMonths = inst._getNumberOfMonths(); // fix width for dynamic number of date pickers
            //inst._datepickerDiv.width(numMonths[1] * $('.datepicker', inst._datepickerDiv[0])[0].offsetWidth);
            if ($.browser.msie && parseInt($.browser.version) < 7) { // fix IE < 7 select problems
                $('#datepicker_cover').css({ width: inst._datepickerDiv.width() + 4,
                    height: inst._datepickerDiv.height() + 4
                });
            }
            // re-position on screen if necessary
            var isFixed = inst._datepickerDiv.css('position') == 'fixed';
            var pos = inst._input ? $.datepicker._findPos(inst._input[0]) : null;
            var browserWidth = window.innerWidth || document.documentElement.clientWidth || document.body.clientWidth;
            var browserHeight = window.innerHeight || document.documentElement.clientHeight || document.body.clientHeight;
            var scrollX = (isFixed ? 0 : document.documentElement.scrollLeft || document.body.scrollLeft);
            var scrollY = (isFixed ? 0 : document.documentElement.scrollTop || document.body.scrollTop);
            // reposition date picker horizontally if outside the browser window
            if ((inst._datepickerDiv.offset().left + inst._datepickerDiv.width() -
				(isFixed && $.browser.msie ? document.documentElement.scrollLeft : 0)) >
				(browserWidth + scrollX)) {
                inst._datepickerDiv.css('left', Math.max(scrollX,
				pos[0] + (inst._input ? $(inst._input[0]).width() : null) - inst._datepickerDiv.width() -
				(isFixed && $.browser.opera ? document.documentElement.scrollLeft : 0)) + 'px');
            }
            // reposition date picker vertically if outside the browser window
            if ((inst._datepickerDiv.offset().top + inst._datepickerDiv.height() -
				(isFixed && $.browser.msie ? document.documentElement.scrollTop : 0)) >
				(browserHeight + scrollY)) {
                inst._datepickerDiv.css('top', Math.max(scrollY,
				pos[1] - (this._inDialog ? 0 : inst._datepickerDiv.height()) -
				(isFixed && $.browser.opera ? document.documentElement.scrollTop : 0)) + 'px');
            }
        },

        /* Find an object's position on the screen. */
        _findPos: function(obj) {
            var notTypeHidden = (obj.type != 'hidden');
            while (obj && (obj.type == 'hidden' || obj.nodeType != 1)) {
                obj = obj.nextSibling;
            }
            var jObj = $(obj);
            if (notTypeHidden) {
                jObj.show();
            }
            var position = jObj.offset();
            if (notTypeHidden) {
                jObj.hide();
            }
            return [position.left, position.top];
        },

        /* Hide the date picker from view.
        @param  input  element - the input field attached to the date picker
        @param  speed  string - the speed at which to close the date picker */
        _hideDatepicker: function(input, speed) {
            //added so that second datepicker will close
            var inst = this._curInst ? this._curInst : $.datepicker._inst[1];
            if (!inst)
                return;
            var rangeSelect = inst._get('rangeSelect');
            if (rangeSelect && this._stayOpen) {
                this._selectDate(inst, inst._formatDate(
				inst._currentDay, inst._currentMonth, inst._currentYear));
            }
            this._stayOpen = false;
            if (this._datepickerShowing) {
                speed = (speed != null ? speed : inst._get('speed'));
                var showAnim = inst._get('showAnim');
                inst._datepickerDiv[(showAnim == 'slideDown' ? 'slideUp' :
				(showAnim == 'fadeIn' ? 'fadeOut' : 'hide'))](speed, function() {
				    $.datepicker._tidyDialog(inst);
				});
                if (speed == '')
                    this._tidyDialog(inst);
                var onClose = inst._get('onClose');
                if (onClose) {
                    onClose.apply((inst._input ? inst._input[0] : null),
					[inst._getDate(), inst]);  // trigger custom callback
                }
                this._datepickerShowing = false;
                this._lastInput = null;
                inst._settings.prompt = null;
                if (this._inDialog) {
                    this._dialogInput.css({ position: 'absolute', left: '0', top: '-100px' });
                    if ($.blockUI) {
                        $.unblockUI();
                        $('body').append(this._datepickerDiv);
                    }
                }
                this._inDialog = false;
            }
            this._curInst = null;
        },

        /* Tidy up after a dialog display. */
        _tidyDialog: function(inst) {
            inst._datepickerDiv.removeClass('datepicker_dialog').unbind('.datepicker');
            $('.datepicker_prompt', inst._datepickerDiv).remove();
        },

        /* Close date picker if clicked elsewhere. */
        _checkExternalClick: function(event) {
            //added such that it detects the second date picker when it pops up
            if (!$.datepicker._curInst && !$.datepicker._inst[1])
                return;
            var $target = $(event.target);
            if (($target.parents("#datepicker_div").length == 0) &&
				($target.attr('class') != 'datepicker_trigger') &&
				$.datepicker._datepickerShowing && !($.datepicker._inDialog && $.blockUI)) {
                $.datepicker._hideDatepicker(null, '');
            }
        },

        /* Adjust one of the date sub-fields. */
        _adjustDate: function(id, offset, period) {
            var inst = this._getInst(id);
            inst._adjustDate(offset, period);
            this._updateDatepicker(inst);
        },

        /* Action for current link. */
        _gotoToday: function(id) {
            var date = new Date();
            var inst = this._getInst(id);
            inst._selectedDay = date.getDate();
            inst._drawMonth = inst._selectedMonth = date.getMonth();
            inst._drawYear = inst._selectedYear = date.getFullYear();
            this._adjustDate(inst);
        },

        /* Action for selecting a new month/year. */
        _selectMonthYear: function(id, select, period) {
            var inst = this._getInst(id);
            inst._selectingMonthYear = false;
            inst[period == 'M' ? '_drawMonth' : '_drawYear'] =
			select.options[select.selectedIndex].value - 0;
            this._adjustDate(inst);
        },

        /* Restore input focus after not changing month/year. */
        _clickMonthYear: function(id) {
            var inst = this._getInst(id);
            if (inst._input && inst._selectingMonthYear && !$.browser.msie)
                inst._input[0].focus();
            inst._selectingMonthYear = !inst._selectingMonthYear;
        },

        /* Action for changing the first week day. */
        _changeFirstDay: function(id, day) {
            var inst = this._getInst(id);
            inst._settings.firstDay = day;
            this._updateDatepicker(inst);
        },

        /* Action for selecting a day. */
        _selectDay: function(id, month, year, td) {
            if ($(td).is('.datepicker_unselectable'))
                return;
            var inst = this._getInst(id);
            var rangeSelect = inst._get('rangeSelect');
            if (rangeSelect) {
                if (!this._stayOpen) {
                    $('.datepicker td').removeClass('datepicker_currentDay');
                    $(td).addClass('datepicker_currentDay');
                }
                this._stayOpen = !this._stayOpen;
            }
            inst._selectedDay = inst._currentDay = $('a', td).html();
            inst._selectedMonth = inst._currentMonth = month;
            inst._selectedYear = inst._currentYear = year;
            this._selectDate(id, inst._formatDate(
			inst._currentDay, inst._currentMonth, inst._currentYear));
            if (this._stayOpen) {
                inst._endDay = inst._endMonth = inst._endYear = null;
                inst._rangeStart = new Date(inst._currentYear, inst._currentMonth, inst._currentDay);
                this._updateDatepicker(inst);
            }
            else if (rangeSelect) {
                inst._endDay = inst._currentDay;
                inst._endMonth = inst._currentMonth;
                inst._endYear = inst._currentYear;
                inst._selectedDay = inst._currentDay = inst._rangeStart.getDate();
                inst._selectedMonth = inst._currentMonth = inst._rangeStart.getMonth();
                inst._selectedYear = inst._currentYear = inst._rangeStart.getFullYear();
                inst._rangeStart = null;
                if (inst._inline)
                    this._updateDatepicker(inst);
            }
        },

        /* Erase the input field and hide the date picker. */
        _clearDate: function(id) {
            var inst = this._getInst(id);
            if (inst._get('mandatory'))
                return;
            this._stayOpen = false;
            inst._endDay = inst._endMonth = inst._endYear = inst._rangeStart = null;
            this._selectDate(inst, '');
        },

        /* Update the input field with the selected date. */
        _selectDate: function(id, dateStr) {
            var inst = this._getInst(id);
            dateStr = (dateStr != null ? dateStr : inst._formatDate());
            if (inst._rangeStart)
                dateStr = inst._formatDate(inst._rangeStart) + inst._get('rangeSeparator') + dateStr;
            if (inst._input)
                inst._input.val(dateStr);
            var onSelect = inst._get('onSelect');
            if (onSelect)
                onSelect.apply((inst._input ? inst._input[0] : null), [dateStr, inst]);  // trigger custom callback
            else if (inst._input)
                inst._input.trigger('change'); // fire the change event
            if (inst._inline)
                this._updateDatepicker(inst);
            else if (!this._stayOpen) {
                this._hideDatepicker(null, inst._get('speed'));
                this._lastInput = inst._input[0];
                if (typeof (inst._input[0]) != 'object')
                    inst._input[0].focus(); // restore focus
                this._lastInput = null;
            }
        },

        /* Set as beforeShowDay function to prevent selection of weekends.
        @param  date  Date - the date to customise
        @return [boolean, string] - is this date selectable?, what is its CSS class? */
        noWeekends: function(date) {
            var day = date.getDay();
            return [(day > 0 && day < 6), ''];
        },

        /* Set as calculateWeek to determine the week of the year based on the ISO 8601 definition.
        @param  date  Date - the date to get the week for
        @return  number - the number of the week within the year that contains this date */
        iso8601Week: function(date) {
            var checkDate = new Date(date.getFullYear(), date.getMonth(), date.getDate(), (date.getTimezoneOffset() / -60));
            var firstMon = new Date(checkDate.getFullYear(), 1 - 1, 4); // First week always contains 4 Jan
            var firstDay = firstMon.getDay() || 7; // Day of week: Mon = 1, ..., Sun = 7
            firstMon.setDate(firstMon.getDate() + 1 - firstDay); // Preceding Monday
            if (firstDay < 4 && checkDate < firstMon) { // Adjust first three days in year if necessary
                checkDate.setDate(checkDate.getDate() - 3); // Generate for previous year
                return $.datepicker.iso8601Week(checkDate);
            } else if (checkDate > new Date(checkDate.getFullYear(), 12 - 1, 28)) { // Check last three days in year
                firstDay = new Date(checkDate.getFullYear() + 1, 1 - 1, 4).getDay() || 7;
                if (firstDay > 4 && (checkDate.getDay() || 7) < firstDay - 3) { // Adjust if necessary
                    checkDate.setDate(checkDate.getDate() + 3); // Generate for next year
                    return $.datepicker.iso8601Week(checkDate);
                }
            }
            return Math.floor(((checkDate - firstMon) / 86400000) / 7) + 1; // Weeks to given date
        },

        /* Provide status text for a particular date.
        @param  date  the date to get the status for
        @param  inst  the current datepicker instance
        @return  the status display text for this date */
        dateStatus: function(date, inst) {
            return $.datepicker.formatDate(inst._get('dateStatus'), date, inst._getFormatConfig());
        },

        /* Parse a string value into a date object.
        The format can be combinations of the following:
        d  - day of month (no leading zero)
        dd - day of month (two digit)
        D  - day name short
        DD - day name long
        m  - month of year (no leading zero)
        mm - month of year (two digit)
        M  - month name short
        MM - month name long
        y  - year (two digit)
        yy - year (four digit)
        '...' - literal text
        '' - single quote

	   @param  format           String - the expected format of the date
        @param  value            String - the date in the above format
        @param  settings  Object - attributes include:
        shortYearCutoff  Number - the cutoff year for determining the century (optional)
        dayNamesShort    String[7] - abbreviated names of the days from Sunday (optional)
        dayNames         String[7] - names of the days from Sunday (optional)
        monthNamesShort  String[12] - abbreviated names of the months (optional)
        monthNames       String[12] - names of the months (optional)
        @return  Date - the extracted date value or null if value is blank */
        parseDate: function(format, value, settings) {
            if (format == null || value == null)
                throw 'Invalid arguments';
            value = (typeof value == 'object' ? value.toString() : value + '');
            if (value == '')
                return null;
            var shortYearCutoff = (settings ? settings.shortYearCutoff : null) || this._defaults.shortYearCutoff;
            var dayNamesShort = (settings ? settings.dayNamesShort : null) || this._defaults.dayNamesShort;
            var dayNames = (settings ? settings.dayNames : null) || this._defaults.dayNames;
            var monthNamesShort = (settings ? settings.monthNamesShort : null) || this._defaults.monthNamesShort;
            var monthNames = (settings ? settings.monthNames : null) || this._defaults.monthNames;
            var year = -1;
            var month = -1;
            var day = -1;
            var literal = false;
            // Check whether a format character is doubled
            var lookAhead = function(match) {
                var matches = (iFormat + 1 < format.length && format.charAt(iFormat + 1) == match);
                if (matches)
                    iFormat++;
                return matches;
            };
            // Extract a number from the string value
            var getNumber = function(match) {
                lookAhead(match);
                var size = (match == 'y' ? 4 : 2);
                var num = 0;
                while (size > 0 && iValue < value.length &&
					value.charAt(iValue) >= '0' && value.charAt(iValue) <= '9') {
                    num = num * 10 + (value.charAt(iValue++) - 0);
                    size--;
                }
                if (size == (match == 'y' ? 4 : 2))
                    throw 'Missing number at position ' + iValue;
                return num;
            };
            // Extract a name from the string value and convert to an index
            var getName = function(match, shortNames, longNames) {
                var names = (lookAhead(match) ? longNames : shortNames);
                var size = 0;
                for (var j = 0; j < names.length; j++)
                    size = Math.max(size, names[j].length);
                var name = '';
                var iInit = iValue;
                while (size > 0 && iValue < value.length) {
                    name += value.charAt(iValue++);
                    for (var i = 0; i < names.length; i++)
                        if (name == names[i])
                        return i + 1;
                    size--;
                }
                throw 'Unknown name at position ' + iInit;
            };
            // Confirm that a literal character matches the string value
            var checkLiteral = function() {
                if (value.charAt(iValue) != format.charAt(iFormat))
                    throw 'Unexpected literal at position ' + iValue;
                iValue++;
            };
            var iValue = 0;
            for (var iFormat = 0; iFormat < format.length; iFormat++) {
                if (literal)
                    if (format.charAt(iFormat) == "'" && !lookAhead("'"))
                    literal = false;
                else
                    checkLiteral();
                else
                    switch (format.charAt(iFormat)) {
                    case 'd':
                        day = getNumber('d');
                        break;
                    case 'D':
                        getName('D', dayNamesShort, dayNames);
                        break;
                    case 'm':
                        month = getNumber('m');
                        break;
                    case 'M':
                        month = getName('M', monthNamesShort, monthNames);
                        break;
                    case 'y':
                        year = getNumber('y');
                        break;
                    case "'":
                        if (lookAhead("'"))
                            checkLiteral();
                        else
                            literal = true;
                        break;
                    default:
                        checkLiteral();
                }
            }
            if (year < 100) {
                year += new Date().getFullYear() - new Date().getFullYear() % 100 +
				(year <= shortYearCutoff ? 0 : -100);
            }
            var date = new Date(year, month - 1, day);
            if (date.getFullYear() != year || date.getMonth() + 1 != month || date.getDate() != day) {
                throw 'Invalid date'; // E.g. 31/02/*
            }
            return date;
        },

        /* Format a date object into a string value.
        The format can be combinations of the following:
        d  - day of month (no leading zero)
        dd - day of month (two digit)
        D  - day name short
        DD - day name long
        m  - month of year (no leading zero)
        mm - month of year (two digit)
        M  - month name short
        MM - month name long
        y  - year (two digit)
        yy - year (four digit)
        '...' - literal text
        '' - single quote

	   @param  format    String - the desired format of the date
        @param  date      Date - the date value to format
        @param  settings  Object - attributes include:
        dayNamesShort    String[7] - abbreviated names of the days from Sunday (optional)
        dayNames         String[7] - names of the days from Sunday (optional)
        monthNamesShort  String[12] - abbreviated names of the months (optional)
        monthNames       String[12] - names of the months (optional)
        @return  String - the date in the above format */
        formatDate: function(format, date, settings) {
            if (!date)
                return '';
            var dayNamesShort = (settings ? settings.dayNamesShort : null) || this._defaults.dayNamesShort;
            var dayNames = (settings ? settings.dayNames : null) || this._defaults.dayNames;
            var monthNamesShort = (settings ? settings.monthNamesShort : null) || this._defaults.monthNamesShort;
            var monthNames = (settings ? settings.monthNames : null) || this._defaults.monthNames;
            // Check whether a format character is doubled
            var lookAhead = function(match) {
                var matches = (iFormat + 1 < format.length && format.charAt(iFormat + 1) == match);
                if (matches)
                    iFormat++;
                return matches;
            };
            // Format a number, with leading zero if necessary
            var formatNumber = function(match, value) {
                return (lookAhead(match) && value < 10 ? '0' : '') + value;
            };
            // Format a name, short or long as requested
            var formatName = function(match, value, shortNames, longNames) {
                return (lookAhead(match) ? longNames[value] : shortNames[value]);
            };
            var output = '';
            var literal = false;
            if (date) {
                for (var iFormat = 0; iFormat < format.length; iFormat++) {
                    if (literal)
                        if (format.charAt(iFormat) == "'" && !lookAhead("'"))
                        literal = false;
                    else
                        output += format.charAt(iFormat);
                    else
                        switch (format.charAt(iFormat)) {
                        case 'd':
                            output += formatNumber('d', date.getDate());
                            break;
                        case 'D':
                            output += formatName('D', date.getDay(), dayNamesShort, dayNames);
                            break;
                        case 'm':
                            output += formatNumber('m', date.getMonth() + 1);
                            break;
                        case 'M':
                            output += formatName('M', date.getMonth(), monthNamesShort, monthNames);
                            break;
                        case 'y':
                            output += (lookAhead('y') ? date.getFullYear() :
								(date.getYear() % 100 < 10 ? '0' : '') + date.getYear() % 100);
                            break;
                        case "'":
                            if (lookAhead("'"))
                                output += "'";
                            else
                                literal = true;
                            break;
                        default:
                            output += format.charAt(iFormat);
                    }
                }
            }
            return output;
        },

        /* Extract all possible characters from the date format. */
        _possibleChars: function(format) {
            var chars = '';
            var literal = false;
            for (var iFormat = 0; iFormat < format.length; iFormat++)
                if (literal)
                if (format.charAt(iFormat) == "'" && !lookAhead("'"))
                literal = false;
            else
                chars += format.charAt(iFormat);
            else
                switch (format.charAt(iFormat)) {
                case 'd' || 'm' || 'y':
                    chars += '0123456789';
                    break;
                case 'D' || 'M':
                    return null; // Accept anything
                case "'":
                    if (lookAhead("'"))
                        chars += "'";
                    else
                        literal = true;
                    break;
                default:
                    chars += format.charAt(iFormat);
            }
            return chars;
        }
    });

    /* Individualised settings for date picker functionality applied to one or more related inputs.
    Instances are managed and manipulated through the Datepicker manager. */
    function DatepickerInstance(settings, inline) {
        this._id = $.datepicker._register(this);
        this._selectedDay = 0; // Current date for selection
        this._selectedMonth = 0; // 0-11
        this._selectedYear = 0; // 4-digit year
        this._drawMonth = 0; // Current month at start of datepicker
        this._drawYear = 0;
        this._input = null; // The attached input field
        this._inline = inline; // True if showing inline, false if used in a popup
        this._datepickerDiv = (!inline ? $.datepicker._datepickerDiv :
		$('<div id="datepicker_div_' + this._id + '" class="datepicker_inline">'));
        // customise the date picker object - uses manager defaults if not overridden
        this._settings = extendRemove(settings || {}); // clone
        if (inline)
            this._setDate(this._getDefaultDate());
    }

    $.extend(DatepickerInstance.prototype, {
        /* Get a setting value, defaulting if necessary. */
        _get: function(name) {
            return this._settings[name] || $.datepicker._defaults[name];
        },

        /* Parse existing date and initialise date picker. */
        _setDateFromField: function(input) {
            this._input = $(input);
            var dateFormat = this._get('dateFormat');
            var dates = this._input ? this._input.val().split(this._get('rangeSeparator')) : null;
            this._endDay = this._endMonth = this._endYear = null;
            var date = defaultDate = this._getDefaultDate();
            if (dates.length > 0) {
                var settings = this._getFormatConfig();
                if (dates.length > 1) {
                    date = $.datepicker.parseDate(dateFormat, dates[1], settings) || defaultDate;
                    this._endDay = date.getDate();
                    this._endMonth = date.getMonth();
                    this._endYear = date.getFullYear();
                }
                try {
                    date = $.datepicker.parseDate(dateFormat, dates[0], settings) || defaultDate;
                } catch (e) {
                    $.datepicker.log(e);
                    date = defaultDate;
                }
            }
            this._selectedDay = date.getDate();
            this._drawMonth = this._selectedMonth = date.getMonth();
            this._drawYear = this._selectedYear = date.getFullYear();
            this._currentDay = (dates[0] ? date.getDate() : 0);
            this._currentMonth = (dates[0] ? date.getMonth() : 0);
            this._currentYear = (dates[0] ? date.getFullYear() : 0);
            this._adjustDate();
        },

        /* Retrieve the default date shown on opening. */
        _getDefaultDate: function() {
            var date = this._determineDate('defaultDate', new Date());
            var minDate = this._getMinMaxDate('min', true);
            var maxDate = this._getMinMaxDate('max');
            date = (minDate && date < minDate ? minDate : date);
            date = (maxDate && date > maxDate ? maxDate : date);
            return date;
        },

        /* A date may be specified as an exact value or a relative one. */
        _determineDate: function(name, defaultDate) {
            var offsetNumeric = function(offset) {
                var date = new Date();
                date.setDate(date.getDate() + offset);
                return date;
            };
            var offsetString = function(offset, getDaysInMonth) {
                var date = new Date();
                var matches = /^([+-]?[0-9]+)\s*(d|D|w|W|m|M|y|Y)?$/.exec(offset);
                if (matches) {
                    var year = date.getFullYear();
                    var month = date.getMonth();
                    var day = date.getDate();
                    switch (matches[2] || 'd') {
                        case 'd': case 'D':
                            day += (matches[1] - 0); break;
                        case 'w': case 'W':
                            day += (matches[1] * 7); break;
                        case 'm': case 'M':
                            month += (matches[1] - 0);
                            day = Math.min(day, getDaysInMonth(year, month));
                            break;
                        case 'y': case 'Y':
                            year += (matches[1] - 0);
                            day = Math.min(day, getDaysInMonth(year, month));
                            break;
                    }
                    date = new Date(year, month, day);
                }
                return date;
            };
            var date = this._get(name);
            return (date == null ? defaultDate :
			(typeof date == 'string' ? offsetString(date, this._getDaysInMonth) :
			(typeof date == 'number' ? offsetNumeric(date) : date)));
        },

        /* Set the date(s) directly. */
        _setDate: function(date, endDate) {
            this._selectedDay = this._currentDay = date.getDate();
            this._drawMonth = this._selectedMonth = this._currentMonth = date.getMonth();
            this._drawYear = this._selectedYear = this._currentYear = date.getFullYear();
            if (this._get('rangeSelect')) {
                if (endDate) {
                    this._endDay = endDate.getDate();
                    this._endMonth = endDate.getMonth();
                    this._endYear = endDate.getFullYear();
                } else {
                    this._endDay = this._currentDay;
                    this._endMonth = this._currentMonth;
                    this._endYear = this._currentYear;
                }
            }
            this._adjustDate();
        },

        /* Retrieve the date(s) directly. */
        _getDate: function() {
            var startDate = (!this._currentYear || (this._input && this._input.val() == '') ? null :
			new Date(this._currentYear, this._currentMonth, this._currentDay));
            if (this._get('rangeSelect')) {
                return [startDate, (!this._endYear ? null :
				new Date(this._endYear, this._endMonth, this._endDay))];
            } else
                return startDate;
        },

        /* Generate the HTML for the current state of the date picker. */
        _generateDatepicker: function() {
            var today = new Date();
            today = new Date(today.getFullYear(), today.getMonth(), today.getDate()); // clear time
            var showStatus = this._get('showStatus');
            var isRTL = this._get('isRTL');
            // build the date picker HTML
            var clear = (this._get('mandatory') ? '' :
			'<div class="datepicker_clear"><a onclick="jQuery.datepicker._clearDate(' + this._id + ');"' +
			(showStatus ? this._addStatus(this._get('clearStatus') || '&#xa0;') : '') + '>' +
			this._get('clearText') + '</a></div>');
            //            var controls = '<div class="datepicker_control"><div class="datepicker_controlHeader"><span>' +
            //            (this._get('controlHeaderText') ? this._get('controlHeaderText') : '') + '</span></div>' + (isRTL ? '' : clear) +
            //			'<div class="datepicker_close"><a onclick="jQuery.datepicker._hideDatepicker();"' +
            //			(showStatus ? this._addStatus(this._get('closeStatus') || '&#xa0;') : '') + '>' +
            //			this._get('closeText') + '</a></div>' + (isRTL ? clear : '') + '</div>';
            var controls = '';
            var prompt = this._get('prompt');
            var closeAtTop = this._get('closeAtTop');
            var hideIfNoPrevNext = this._get('hideIfNoPrevNext');
            var numMonths = this._getNumberOfMonths();
            var stepMonths = this._get('stepMonths');
            var isMultiMonth = (numMonths[0] != 1 || numMonths[1] != 1);
            var minDate = this._getMinMaxDate('min', true);
            var maxDate = this._getMinMaxDate('max');
            var drawMonth = this._drawMonth;
            var drawYear = this._drawYear;
            if (maxDate) {
                var maxDraw = new Date(maxDate.getFullYear(),
				maxDate.getMonth() - numMonths[1] + 1, maxDate.getDate());
                maxDraw = (minDate && maxDraw < minDate ? minDate : maxDraw);
                while (new Date(drawYear, drawMonth, 1) > maxDraw) {
                    drawMonth--;
                    if (drawMonth < 0) {
                        drawMonth = 11;
                        drawYear--;
                    }
                }
            }
            // controls and links
            var prev = '<div class="datepicker_prev">' + (this._canAdjustMonth(-1, drawYear, drawMonth) ?
			'<a onclick="jQuery.datepicker._adjustDate(' + this._id + ', -' + stepMonths + ', \'M\');"' +
			(showStatus ? this._addStatus(this._get('prevStatus') || '&#xa0;') : '') + '>' +
			this._get('prevText') + '</a>' :
			(hideIfNoPrevNext ? '' : '<label>' + this._get('prevText') + '</label>')) + '</div>';
            var next = '<div class="datepicker_next">' + (this._canAdjustMonth(+1, drawYear, drawMonth) ?
			'<a onclick="jQuery.datepicker._adjustDate(' + this._id + ', +' + stepMonths + ', \'M\');"' +
			(showStatus ? this._addStatus(this._get('nextStatus') || '&#xa0;') : '') + '>' +
			this._get('nextText') + '</a>' :
			(hideIfNoPrevNext ? '>' : '<label>' + this._get('nextText') + '</label>')) + '</div>';
            var html = (prompt ? '<div class="datepicker_prompt">' + prompt + '</div>' : '') +
			(closeAtTop && !this._inline ? controls : '') +
			'<div class="datepicker_links">' + (isRTL ? next : prev) +
			(this._isInRange(today) ? '<div class="datepicker_current">' +
			'<a onclick="jQuery.datepicker._gotoToday(' + this._id + ');"' +
			(showStatus ? this._addStatus(this._get('currentStatus') || '&#xa0;') : '') + '>' +
			this._get('currentText') + '</a></div>' : '') + (isRTL ? prev : next) + '</div>';
            var showWeeks = this._get('showWeeks');
            for (var row = 0; row < numMonths[0]; row++)
                for (var col = 0; col < numMonths[1]; col++) {
                var selectedDate = new Date(drawYear, drawMonth, this._selectedDay);
                html += '<div class="datepicker_oneMonth' + (col == 0 ? ' datepicker_newRow' : '') + '">' +
					this._generateMonthYearHeader(drawMonth, drawYear, minDate, maxDate,
					selectedDate, row > 0 || col > 0) + // draw month headers
					'<table class="datepicker" cellpadding="0" cellspacing="0"><thead>' +
					'<tr class="datepicker_titleRow">' +
					(showWeeks ? '<td>' + this._get('weekHeader') + '</td>' : '');
                var firstDay = this._get('firstDay');
                var changeFirstDay = this._get('changeFirstDay');
                var dayNames = this._get('dayNames');
                var dayNamesShort = this._get('dayNamesShort');
                var dayNamesMin = this._get('dayNamesMin');
                for (var dow = 0; dow < 7; dow++) { // days of the week
                    var day = (dow + firstDay) % 7;
                    var status = this._get('dayStatus') || '&#xa0;';
                    status = (status.indexOf('DD') > -1 ? status.replace(/DD/, dayNames[day]) :
						status.replace(/D/, dayNamesShort[day]));
                    html += '<td' + ((dow + firstDay + 6) % 7 >= 5 ? ' class="datepicker_weekEndCell"' : '') + '>' +
						(!changeFirstDay ? '<span' :
						'<a onclick="jQuery.datepicker._changeFirstDay(' + this._id + ', ' + day + ');"') +
						(showStatus ? this._addStatus(status) : '') + ' title="' + dayNames[day] + '">' +
						dayNamesMin[day] + (changeFirstDay ? '</a>' : '</span>') + '</td>';
                }
                html += '</tr></thead><tbody>';
                var daysInMonth = this._getDaysInMonth(drawYear, drawMonth);
                if (drawYear == this._selectedYear && drawMonth == this._selectedMonth) {
                    this._selectedDay = Math.min(this._selectedDay, daysInMonth);
                }
                var leadDays = (this._getFirstDayOfMonth(drawYear, drawMonth) - firstDay + 7) % 7;
                var currentDate = (!this._currentDay ? new Date(9999, 9, 9) :
					new Date(this._currentYear, this._currentMonth, this._currentDay));
                var endDate = this._endDay ? new Date(this._endYear, this._endMonth, this._endDay) : currentDate;
                var printDate = new Date(drawYear, drawMonth, 1 - leadDays);
                var numRows = (isMultiMonth ? 6 : Math.ceil((leadDays + daysInMonth) / 7)); // calculate the number of rows to generate
                var beforeShowDay = this._get('beforeShowDay');
                var showOtherMonths = this._get('showOtherMonths');
                var calculateWeek = this._get('calculateWeek') || $.datepicker.iso8601Week;
                var dateStatus = this._get('statusForDate') || $.datepicker.dateStatus;
                for (var dRow = 0; dRow < numRows; dRow++) { // create date picker rows
                    html += '<tr class="datepicker_daysRow">' +
						(showWeeks ? '<td class="datepicker_weekCol">' + calculateWeek(printDate) + '</td>' : '');
                    for (var dow = 0; dow < 7; dow++) { // create date picker days
                        var daySettings = (beforeShowDay ?
							beforeShowDay.apply((this._input ? this._input[0] : null), [printDate]) : [true, '']);
                        var otherMonth = (printDate.getMonth() != drawMonth);
                        var unselectable = otherMonth || !daySettings[0] ||
							(minDate && printDate < minDate) || (maxDate && printDate > maxDate);
                        html += '<td class="datepicker_daysCell' +
							((dow + firstDay + 6) % 7 >= 5 ? ' datepicker_weekEndCell' : '') + // highlight weekends
							(otherMonth ? ' datepicker_otherMonth' : '') + // highlight days from other months
                        //START: removed this to prevent highlight of days in other months
                        //(printDate.getTime() == selectedDate.getTime() && drawMonth == this._selectedMonth ?
                        //' datepicker_daysCellOver' : '') + // highlight selected day
                        //END: removed this to prevent highlight of days in other months
							(unselectable ? ' datepicker_unselectable' : '') +  // highlight unselectable days
							(otherMonth && !showOtherMonths ? '' : ' ' + daySettings[1] + // highlight custom dates
							(printDate.getTime() >= currentDate.getTime() && printDate.getTime() <= endDate.getTime() ?  // in current range
							' datepicker_currentDay' : '') + // highlight selected day
							(printDate.getTime() == today.getTime() ? ' datepicker_today' : '')) + '"' + // highlight today (if different)
							(unselectable ? '' : ' onmouseover="jQuery(this).addClass(\'datepicker_daysCellOver\');' +
							(!showStatus || (otherMonth && !showOtherMonths) ? '' : 'jQuery(\'#datepicker_status_' +
							this._id + '\').html(\'' + (dateStatus.apply((this._input ? this._input[0] : null),
							[printDate, this]) || '&#xa0;') + '\');') + '"' +
							' onmouseout="jQuery(this).removeClass(\'datepicker_daysCellOver\');' +
							(!showStatus || (otherMonth && !showOtherMonths) ? '' : 'jQuery(\'#datepicker_status_' +
							this._id + '\').html(\'&#xa0;\');') + '" onclick="jQuery.datepicker._selectDay(' +
							this._id + ',' + drawMonth + ',' + drawYear + ', this);"') + '>' + // actions
							(otherMonth ? (showOtherMonths ? printDate.getDate() : '&#xa0;') : // display for other months
							(unselectable ? printDate.getDate() : '<a>' + printDate.getDate() + '</a>')) + '</td>'; // display for this month
                        printDate.setDate(printDate.getDate() + 1);
                    }
                    html += '</tr>';
                }
                drawMonth++;
                if (drawMonth > 11) {
                    drawMonth = 0;
                    drawYear++;
                }
                html += '</tbody></table></div>';
            }
            html += (showStatus ? '<div id="datepicker_status_' + this._id +
			'" class="datepicker_status">' + (this._get('initStatus') || '&#xa0;') + '</div>' : '') +
			(!closeAtTop && !this._inline ? controls : '') +
			'<div style="clear: both;"></div>' +
			($.browser.msie && parseInt($.browser.version) < 7 && !this._inline ?
			'<iframe src="javascript:false;" class="datepicker_cover"></iframe>' : '');
            return html;
        },

        /* Generate the month and year header. */
        _generateMonthYearHeader: function(drawMonth, drawYear, minDate, maxDate, selectedDate, secondary) {
            minDate = (this._rangeStart && minDate && selectedDate < minDate ? selectedDate : minDate);
            var showStatus = this._get('showStatus');
            var html = '<div class="datepicker_header">';
            // month selection
            var monthNames = this._get('monthNames');
            if (secondary || !this._get('changeMonth'))
                html += monthNames[drawMonth] + '&#xa0;';

            else {
                var inMinYear = (minDate && minDate.getFullYear() == drawYear);
                var inMaxYear = (maxDate && maxDate.getFullYear() == drawYear);
                html += '<select class="datepicker_newMonth" ' +
				'onchange="jQuery.datepicker._selectMonthYear(' + this._id + ', this, \'M\');" ' +
				'onclick="jQuery.datepicker._clickMonthYear(' + this._id + ');"' +
				(showStatus ? this._addStatus(this._get('monthStatus') || '&#xa0;') : '') + '>';
                for (var month = 0; month < 12; month++) {
                    if ((!inMinYear || month >= minDate.getMonth()) &&
						(!inMaxYear || month <= maxDate.getMonth())) {
                        html += '<option value="' + month + '"' +
						(month == drawMonth ? ' selected="selected"' : '') +
						'>' + monthNames[month] + '</option>';
                    }
                }
                html += '</select>';
            }
            // year selection
            if (secondary || !this._get('changeYear'))
                html += drawYear;
            else {
                // determine range of years to display
                var years = this._get('yearRange').split(':');
                var year = 0;
                var endYear = 0;
                if (years.length != 2) {
                    year = drawYear - 10;
                    endYear = drawYear + 10;
                } else if (years[0].charAt(0) == '+' || years[0].charAt(0) == '-') {
                    year = drawYear + parseInt(years[0], 10);
                    endYear = drawYear + parseInt(years[1], 10);
                } else {
                    year = parseInt(years[0], 10);
                    endYear = parseInt(years[1], 10);
                }
                year = (minDate ? Math.max(year, minDate.getFullYear()) : year);
                endYear = (maxDate ? Math.min(endYear, maxDate.getFullYear()) : endYear);
                html += '<select class="datepicker_newYear" ' +
				'onchange="jQuery.datepicker._selectMonthYear(' + this._id + ', this, \'Y\');" ' +
				'onclick="jQuery.datepicker._clickMonthYear(' + this._id + ');"' +
				(showStatus ? this._addStatus(this._get('yearStatus') || '&#xa0;') : '') + '>';
                for (; year <= endYear; year++) {
                    html += '<option value="' + year + '"' +
					(year == drawYear ? ' selected="selected"' : '') +
					'>' + year + '</option>';
                }
                html += '</select>';
            }
            html += '</div>'; // Close datepicker_header
            return html;
        },

        /* Provide code to set and clear the status panel. */
        _addStatus: function(text) {
            return ' onmouseover="jQuery(\'#datepicker_status_' + this._id + '\').html(\'' + text + '\');" ' +
			'onmouseout="jQuery(\'#datepicker_status_' + this._id + '\').html(\'&#xa0;\');"';
        },

        /* Adjust one of the date sub-fields. */
        _adjustDate: function(offset, period) {
            var year = this._drawYear + (period == 'Y' ? offset : 0);
            var month = this._drawMonth + (period == 'M' ? offset : 0);
            var day = Math.min(this._selectedDay, this._getDaysInMonth(year, month)) +
			(period == 'D' ? offset : 0);
            var date = new Date(year, month, day);
            // ensure it is within the bounds set
            var minDate = this._getMinMaxDate('min', true);
            var maxDate = this._getMinMaxDate('max');
            date = (minDate && date < minDate ? minDate : date);
            date = (maxDate && date > maxDate ? maxDate : date);
            this._selectedDay = date.getDate();
            this._drawMonth = this._selectedMonth = date.getMonth();
            this._drawYear = this._selectedYear = date.getFullYear();
        },

        /* Determine the number of months to show. */
        _getNumberOfMonths: function() {
            var numMonths = this._get('numberOfMonths');
            return (numMonths == null ? [1, 1] : (typeof numMonths == 'number' ? [1, numMonths] : numMonths));
        },

        /* Determine the current maximum date - ensure no time components are set - may be overridden for a range. */
        _getMinMaxDate: function(minMax, checkRange) {
            var date = this._determineDate(minMax + 'Date', null);
            if (date) {
                date.setHours(0);
                date.setMinutes(0);
                date.setSeconds(0);
                date.setMilliseconds(0);
            }
            return date || (checkRange ? this._rangeStart : null);
        },

        /* Find the number of days in a given month. */
        _getDaysInMonth: function(year, month) {
            return 32 - new Date(year, month, 32).getDate();
        },

        /* Find the day of the week of the first of a month. */
        _getFirstDayOfMonth: function(year, month) {
            return new Date(year, month, 1).getDay();
        },

        /* Determines if we should allow a "next/prev" month display change. */
        _canAdjustMonth: function(offset, curYear, curMonth) {
            var numMonths = this._getNumberOfMonths();
            var date = new Date(curYear, curMonth + (offset < 0 ? offset : numMonths[1]), 1);
            if (offset < 0)
                date.setDate(this._getDaysInMonth(date.getFullYear(), date.getMonth()));
            return this._isInRange(date);
        },

        /* Is the given date in the accepted range? */
        _isInRange: function(date) {
            // during range selection, use minimum of selected date and range start
            var newMinDate = (!this._rangeStart ? null :
			new Date(this._selectedYear, this._selectedMonth, this._selectedDay));
            newMinDate = (newMinDate && this._rangeStart < newMinDate ? this._rangeStart : newMinDate);
            var minDate = newMinDate || this._getMinMaxDate('min');
            var maxDate = this._getMinMaxDate('max');
            return ((!minDate || date >= minDate) && (!maxDate || date <= maxDate));
        },

        /* Provide the configuration settings for formatting/parsing. */
        _getFormatConfig: function() {
            var shortYearCutoff = this._get('shortYearCutoff');
            shortYearCutoff = (typeof shortYearCutoff != 'string' ? shortYearCutoff :
			new Date().getFullYear() % 100 + parseInt(shortYearCutoff, 10));
            return { shortYearCutoff: shortYearCutoff,
                dayNamesShort: this._get('dayNamesShort'), dayNames: this._get('dayNames'),
                monthNamesShort: this._get('monthNamesShort'), monthNames: this._get('monthNames')
            };
        },

        /* Format the given date for display. */
        _formatDate: function(day, month, year) {
            if (!day) {
                this._currentDay = this._selectedDay;
                this._currentMonth = this._selectedMonth;
                this._currentYear = this._selectedYear;
            }
            var date = (day ? (typeof day == 'object' ? day : new Date(year, month, day)) :
			new Date(this._currentYear, this._currentMonth, this._currentDay));
            return $.datepicker.formatDate(this._get('dateFormat'), date, this._getFormatConfig());
        }
    });

    /* jQuery extend now ignores nulls! */
    function extendRemove(target, props) {
        $.extend(target, props);
        for (var name in props)
            if (props[name] == null)
            target[name] = null;
        return target;
    };

    /* Invoke the datepicker functionality.
    @param  options  String - a command, optionally followed by additional parameters or
    Object - settings for attaching new datepicker functionality
    @return  jQuery object */
    $.fn.datepicker = function(options) {
        var otherArgs = Array.prototype.slice.call(arguments, 1);
        if (typeof options == 'string' && (options == 'isDisabled' || options == 'getDate')) {
            return $.datepicker['_' + options + 'Datepicker'].apply($.datepicker, [this[0]].concat(otherArgs));
        }
        return this.each(function() {
            typeof options == 'string' ?
			$.datepicker['_' + options + 'Datepicker'].apply($.datepicker, [this].concat(otherArgs)) :
			$.datepicker._attachDatepicker(this, options);
        });
    };

    /* Initialise the date picker. */
    $(document).ready(function() {
        $(document.body).append($.datepicker._datepickerDiv)
		.mousedown($.datepicker._checkExternalClick);
    });

    $.datepicker = new Datepicker(); // singleton instance

})(jQuery);
/* Start ui.datePicker.js */

/* Start jquery.metaobjects.js */
/*  
*Disclaimer: This license only applies to the code within this file. 
*Navitaire Inc. reserves rights to all other code within this web application
===============================================================================  
Metaobjects is a jQuery plugin for setting properties of DOM elements  by means  
of metaobjects (OBJECT elements with a 'metaobject' class)
...............................................................................                                                 
Copyright 2007 / Andrea Ercolino  
-------------------------------------------------------------------------------  
LICENSE: http://www.opensource.org/licenses/mit-license.php 
MANUAL:  http://www.mondotondo.com/aercolino/noteslog/?page_id=105 
UPDATES: http://www.mondotondo.com/aercolino/noteslog/?cat=20  
===============================================================================  
*/

(function($) {
    $.metaobjects = function(options) {

        options = $.extend({
            context: document
        , clean: true
        , selector: 'object.metaobject'
        }, options);

        function jsValue(value, name) {
            if (name == "regex") {
                value = escapeRegex(value);
            } else {
                value = escapeValue(value);
            }
            eval('value = ' + value + ";");
            return value;
        }

        function escapeValue(value) {
            if (value.match(/^'.*'$/)) {
                value = value.replace(/'/g, "\\'");
                value = value.replace(/^\\\'/, "'");
                value = value.replace(/\\\'$/, "'");
            }
            return value;
        }

        function escapeRegex(value) {
            if (value.match(/^'.*'$/)) {
                value = value.replace(/^'/, "/");
                value = value.replace(/'$/, "/");
            }
            return value;
        }

        return $(options.selector, options.context)
    .each(function() {

        var settings = { target: this.parentNode };
        $('> param[@name=metaparam]', this)
        .each(function() {
            $.extend(settings, jsValue(this.value));
        });

        $('> param', this)
        .not('[@name=metaparam]')
        .each(function() {
            var type = $(this).attr('type');
            var name = this.name;
            var value = jsValue(this.value, name);
            $(settings.target)
            .each(function() {
                this[name] = value;
            });
        });

        if (options.clean) {
            $(this).remove();
        }
    });
    }
})(jQuery);

/*
*Disclaimer: This license only applies to the code within this file. 
*Navitaire Inc. reserves rights to all other code within this web application
*/
/* End jquery.metaobjects.js */

/* Start common.js */
/*!
This file is part of the Navitaire NewSkies application.
Copyright (C) Navitaire.  All rights reserved.
*/

/*
Dependencies:
This file depends on other JavaScript files to be there at run time.
        
jquery.js:
$ is a jquery variable

Standards:
JavaScript file names are camelCase, starting with a lower case letter
Put all code in the appropriate namespace such as all object definitions go in SKYSALES.Class
Objects take no parameters when they are constructed, but implement an init method for initialization of their data members
        
Every object definition has some basic methods
init(jsonObject) - calls the initializing methods
setSettingsByObject(jsonObject) - initializes the object by matching the jsonObject key name with the public member variable name
setVars - accesses nodes on the dom
addEvents - adds dom events to the object, and sets event handlers
supplant - swaps out the objects member value names in [] with there actual values
            
Event handler method names end in Handler, for example clickEventHandler, 
this is to identify that the this variable will be the dom object and not the object instance.
        
You pass string ids of dom nodes to objects and they handle finding that node on the dom, and wiring up its own events
        
Do not write HTML in JavaScript, use a template node from the XSLT, and swap out the object values with a supplant method.
Array brackets [] are used to tell the supplant method to replace the member name with the member value.
[name] is replaced with this.name, Hello [title] [name], becomes Hello Mr. Anderson
      
Inheritance
Note that I have to instantiate an instance of the base class. And keep it around to be able to call base class methods.
var parent = new GLOBAL.Class.Base();
                
You must make a copy of the this object, to be used when the this object turns into the window or a dom object.
var thisChildObject = GLOBAL.extendObject(parent);
                
To call a parent method, you must use the build in call function
parent.setSettingsByObject.call(this);
            
The child class must override event handler methods to set the this variable correctly.
// The event is added in a addEvents method
thisChildObject.domButton.click(this.updateHandler);

// The event is handeled by the correct method, and has thisChildObject available to use via closure
thisChildObject.updateHandler = function ()
{
thisChildObject.update();
};
            
How to know when to use the "this" keyword, or the copied this object (thisChildObject)
            
You should always use the copied this variable on the left side of an assignment operator.
thisChildObject.type = 'childObject';
                    
Inside of an event handler method where you know the the this variable will be set to the dom object.
thisChildObject.updateHandler = function ()
{
thisChildObject.update();
};
                    
Use the "this" keyword in every other scenario.
this.setSettingsByObject(jsonObject);
var name = this.name;
                    
        
Follow all of the JsLint rules that apply when you click good parts.
        
It is highly recommended that you use a JavaScript code compressor to decrease the size of the JavaScript files in production.
Be sure to keep the original file around, because making edits to a compressed file is very difficult.
JavaScript Compressors: 
YUI Compressor: http://developer.yahoo.com/yui/compressor/
JsMin: http://crockford.com/javascript/jsmin.html
We highly recommend turning on gzip compression on your web server.
        

General Notes:
The common.js file is where JavaScript that is used in multiple places goes,
or JavaScript that is commonly used. Such as code for a control that is on many views.
        
All of the SkySales JavaScript should be behind the SKYSALES object defined in this file.
You can think of the SKYSALES object as a namespace.
        
ToDo:
+ Move all of the JavaScript that is dynamically output from the server to external JavaScript files 
+ Put all code behind the SKYSALES namespace, there are currently some global functions that can not be moved yet
+ Make the SkySales class smaller, most objects do not need most of its features.
+ Make the setSettingsByObject the same as in unitMap.js, so all other classes can use it.
+ Call delete after you are finished with json data, such as all of the ResourceInfo Json.
+ Change all of the if (SKYSALES.Class.ClassName === undefined) to just if (!SKYSALES.Class.ClassName) is is shorter
+ Design a way where the object tags do not get searched forunless that control is on the page, or that they are all searched for in a single pass
+ Make all event handler function names end in Handler, such as updateDomHandler. This way you will be able to easily tell what the this variable is set to.
+ Fix all of the code in this file to follow the SkySales JavaScript coding standards
+ Take out the eval calls - eval is evil
        
        
JsLint Status:
JsLint Status:
Pass - JsLint Edition 2008-05-31
        
+ Strict whitespace
+ Assume a browser 
+ Disallow undefined variables
+ Disallow leading _ in identifiers
+ Disallow == and !=
+ Disallow ++ and --
+ Disallow bitwise operators
+ Tolerate eval
Indentation 4
        
*/

/*
Initialize SKYSALES namespace
All javascript in skysales should be behind the SKYSALES namespace
This prevents naming collisions
*/

/*global window: false, SKYSALES: true, $: false */

SKYSALES = {};

//The JSON parser, and serializer
SKYSALES.Json = window.JSON;

//A pointer to the active resource object instance
SKYSALES.Resource = {};

//A static helper class
SKYSALES.Util = {};

//A namespace for class definitions
SKYSALES.Class = {};

/*
A namespace for instances, 
this is used for instances of objects that are auto generated from object tags. 
*/
SKYSALES.Instance = {};
SKYSALES.Instance.index = 0;
SKYSALES.Instance.getNextIndex = function() {
    SKYSALES.Instance.index += 1;
    return SKYSALES.Instance.index;
};

/*
Name: 
Class LocaleCurrency
Param:
num
Return: 
An instance of LocaleCurrency
Functionality:
This class is used by Util.convertToLocaleCurrency(num)
Notes:
This class provides the ability to convert a number to the local currency format
Class Hierarchy:
LocaleCurrency
*/
if (!SKYSALES.Class.LocaleCurrency) {
    SKYSALES.Class.LocaleCurrency = function() {
        var parent = new SKYSALES.Class.SkySales();
        var thisLocaleCurrency = SKYSALES.Util.extendObject(parent);

        thisLocaleCurrency.num = null;
        thisLocaleCurrency.localeCurrency = null;

        var resource = SKYSALES.Util.getResource();
        var currencyCultureInfo = resource.currencyCultureInfo;
        var integerPartNum = 0;
        var integerPartString = '';
        var decimalPartString = '';
        var number = '';
        var positive = true;

        var getCurrencyPattern = function() {
            //var pattern = currencyCultureInfo.positivePattern;
            //if (!positive) {
            //    pattern = currencyCultureInfo.negativePattern;
            //}
            var pattern = 'n';
            if (!positive) {
                pattern = '(n)';
            }
            return pattern;
        };

        var getIntegerPart = function(numVal) {
            var groupSizes = currencyCultureInfo.groupSizes || [];
            var groupSeparator = currencyCultureInfo.groupSeparator;
            var groupSizesIndex = 0;
            var index = 0;
            var currentGroupSize = 3;
            if (groupSizesIndex > groupSizes.length) {
                currentGroupSize = groupSizes[groupSizesIndex];
            }
            var currentGroupEndIndex = currentGroupSize - 1;
            integerPartNum = Math.floor(numVal);
            var localString = integerPartNum.toString();
            var array = localString.split('');
            var reverseArray = array.reverse();
            var reverseArrayOutput = [];

            var getNextGroupSize = function() {
                var nextGroupSize = 3;
                //Increment group sizes index if necessary
                if (groupSizesIndex <= groupSizes.length - 2) {
                    groupSizesIndex += 1;
                    nextGroupSize = groupSizes[groupSizesIndex];
                }
                else {
                    nextGroupSize = currentGroupSize;
                }
                currentGroupEndIndex += nextGroupSize;
                return nextGroupSize;
            };
            for (index = 0; index < reverseArray.length; index += 1) {
                if (index > currentGroupEndIndex) {
                    currentGroupSize = getNextGroupSize();
                    reverseArrayOutput.push(groupSeparator);
                }
                reverseArrayOutput.push(reverseArray[index]);
            }

            array = reverseArrayOutput.reverse();
            var outputString = array.join('');
            return outputString;
        };

        var getDecimalPart = function(numVal) {
            var decimalPart = numVal - integerPartNum;
            var decimalPartTrimmed = decimalPart.toFixed(currencyCultureInfo.decimalDigits);
            var decimalPartString = decimalPartTrimmed.substring(2);
            return decimalPartString;
        };

        var applyPattern = function() {
            var pattern = getCurrencyPattern() || '';
            var replaceNumber = pattern.replace('n', number);
            return replaceNumber;
        };

        var invariantNumberToLocaleCurrency = function() {
            thisLocaleCurrency.currency = thisLocaleCurrency.num.toString();
            positive = thisLocaleCurrency.num >= 0;
            // Make the number positive. The applyPattern will reestablish the sign.
            thisLocaleCurrency.num = Math.abs(thisLocaleCurrency.num);
            integerPartString = getIntegerPart(thisLocaleCurrency.num);

            decimalPartString = getDecimalPart(thisLocaleCurrency.num);
            number = integerPartString;
            if (0 < currencyCultureInfo.decimalDigits) {
                number += currencyCultureInfo.decimalSeparator;
            }
            number += decimalPartString;
            thisLocaleCurrency.currency = applyPattern();
        };

        thisLocaleCurrency.init = function(json) {
            this.setSettingsByObject(json);
            if (null !== this.num) {
                invariantNumberToLocaleCurrency();
            }
        };
        return thisLocaleCurrency;
    };
}

/*
Name: 
Class Resource
Param:
None
Return: 
An instance of Resource
Functionality:
Used to hold any common data that multiple controls use such as
CountryInfo, MacInfo, StationInfo, and BookingInfo
Notes:
Right now there is one resource object instance.
It is accessed in the JavaScript by calling SKYSALES.Util.getResource()
It is created in the common.xslt file, and populated by resource data
that is written to JSON.
The resources that come down to the browser are configured at a view level in the naml file.
To get a list of stations in JSON you add the node of
<bind link="StationResource" property="ResourceContainer"/>
as a child node of the view node.
        
This class also contains a way to access cookie data. 
Such as the contact info that is stored in a cookie to populate the contact view.
Class Hierarchy:
SkySales -> Resource
*/
SKYSALES.Class.Resource = function() {
    var parent = new SKYSALES.Class.SkySales();
    var thisResource = SKYSALES.Util.extendObject(parent);

    thisResource.carLocationInfo = {};
    thisResource.carLocationArray = [];
    thisResource.carLocationHash = {};
    thisResource.activityLocationInfo = {};
    thisResource.activityLocationArray = [];
    thisResource.activityLocationHash = {};
    thisResource.hotelLocationInfo = {};
    thisResource.hotelLocationArray = [];
    thisResource.hotelLocationHash = {};
    thisResource.countryInfo = {};
    thisResource.titleInfo = {};
    thisResource.stateInfo = {};
    thisResource.stationInfo = {};
    thisResource.macInfo = {};
    thisResource.marketInfo = {};
    thisResource.macHash = {};
    thisResource.stationHash = {};
    thisResource.macStationHash = {};
    thisResource.countryStationHash = {};
    thisResource.countryHash = {};
    thisResource.marketHash = {};
    thisResource.sourceInfo = {};
    thisResource.clientHash = {};
    thisResource.dateCultureInfo = {};
    thisResource.currencyCultureInfo = {};
    thisResource.carrierInfo = {};
    thisResource.carrierHash = {};
    thisResource.datePickerInfo = {};
    thisResource.passengerInfo = {};
    thisResource.passengerHash = {};
    thisResource.carInfo = {};
    thisResource.externalRateInfo = {};
    thisResource.currencyInfo = {};
    thisResource.currencyHash = {};

    /*
    Turns the macInfo into a hash for quick lookups.
    Keying into the macHash with the mac code you will get back 
    an object that contains an array of station codes that the mac code is associated with.
    macHash[macCode] = { "code": "stationCode", "stations": [ "stationCode1", "stationCode2" ] };
    */
    thisResource.populateMacHash = function() {
        var i = 0;
        var macArray = [];
        var macHash = {};
        var mac = null;
        if (thisResource.macInfo && thisResource.macInfo.MacList) {
            macArray = thisResource.macInfo.MacList;
            for (i = 0; i < macArray.length; i += 1) {
                mac = macArray[i];
                macHash[mac.code] = mac;
            }
        }
        thisResource.macHash = macHash;
    };

    thisResource.populateStationList = function() {
        var i = 0,
                stationArray = [],
                stationTemp = [],
                countrySort = 'country',
                stationNameSort = 'name',
                len = -1;

        if (thisResource.stationInfo && thisResource.stationInfo.StationList) {
            stationArray = thisResource.stationInfo.StationList;
            len = stationArray.length;
            for (i = 0; i < len; i += 1) {
                stationArray[i].country = thisResource.countryHash[stationArray[i].countryCode];
                stationArray[i].index = i;
            }
            stationTemp = stationArray;
        }
        // sort stations through country or name
//        stationTemp.sort(thisResource.sortStationList(countrySort, thisResource.sortStationList(stationNameSort)));
//        thisResource.stationInfo.StationList = stationTemp;
    };

    thisResource.sortStationList = function(country, functionStationSort) {
        return function(stationA, stationB) {
            var countryA = stationA[country], countryB = stationB[country];
            //if countries are the same,sort through names
            if (countryA === countryB) {
                return typeof functionStationSort === 'function' ? functionStationSort(stationA, stationB) : 0;
            }
            return countryA < countryB ? -1 : 1;
        };
    };

    /*
    Turns the stationInfo into a hash for quick lookups.
    Keying into the stationHash with the station code you will get back a station object
    stationHash[stationCode] = { "macCode": "", "name":"", "code": "" };
    
    This method also turns the stationInfo and countryInfo into a hash for quick lookups.
    Keying into the countryStationHash with the country code you will get back an array of station codes
    countryStationHash[countryCode] = { "countryCode": [{"macCode": "", "name":"", "code": "" }] };
    Requires the stationInfo JSON resource to include the countryCode field. See Settings\StationInfo.xml.
    */
    thisResource.populateStationHash = function() {
        var i = 0;
        var stationArray = [];
        var stationHash = {};
        var station = null;
        var countryStationHash = {};
        var countryHash = thisResource.countryHash;
        if (thisResource.stationInfo && thisResource.stationInfo.StationList) {
            stationArray = thisResource.stationInfo.StationList;
            for (i = 0; i < stationArray.length; i += 1) {
                station = stationArray[i];
                stationHash[station.code] = station;
                // This is where the countryStationHash is populated
                stationCountryCode = station.countryCode || "";
                if (stationCountryCode !== "") {
                    countryStationHash[stationCountryCode] = countryStationHash[stationCountryCode] || [];
                    countryStationHash[stationCountryCode][countryStationHash[stationCountryCode].length] = station;
                    countryStationHash[stationCountryCode].name = countryHash[stationCountryCode];
                }
            }
        }
        thisResource.stationHash = stationHash;
        thisResource.countryStationHash = countryStationHash;
    };

    /*
    Turns the passengerInfo into a hash for quick lookups.
    Keying into the passengerHash with the passenger number you will get back a passenger object
    passengerHash[passengerNumber] = { "name": {}, "Nationality":"US", "Gender": "Male" };
    */
    thisResource.populatePassengerHash = function() {
        var i = 0,
                passengerArray = [],
                passengerHash = {},
                passenger = null,
                len = -1;

        if (thisResource.passengerInfo && thisResource.passengerInfo.PassengerList) {
            passengerArray = thisResource.passengerInfo.PassengerList;
            len = passengerArray.length;
            for (i = 0; i < len; i += 1) {
                passenger = passengerArray[i];
                passengerHash[passenger.PassengerNumber] = passenger;
            }
        }
        thisResource.passengerHash = passengerHash;
    };

    /*
    Turns the carrierInfo into a hash for quick lookups.
    Keying into the carrierHash with the carrier code you will get back a carrier object
    carrierHash[carrierCode] = { "macCode": "", "name":"", "code": "" };
    */
    thisResource.populateCarrierHash = function() {
        var i = 0,
                carrierArray = [],
                carrierArrayLength = 0,
                carrierHash = {},
                carrier = null,
                carrierInfo = this.carrierInfo;

        if (carrierInfo) {
            carrierArray = carrierInfo.carrierList;
            if (carrierArray) {
                carrierArrayLength = carrierArray.length;
                if (carrierArrayLength > 0) {
                    for (i = 0; i < carrierArrayLength; i += 1) {
                        carrier = carrierArray[i];
                        carrierHash[carrier.code] = carrier;
                    }
                }
            }
            thisResource.carrierHash = carrierHash;
        }
    };

    /*
    Populate the country hash for quick country name lookups based on country code
    */
    thisResource.populateCountryHash = function() {
        var i = 0;
        var countryHash = {};
        var countryArray = [];
        var country = null;
        if (thisResource.countryInfo && thisResource.countryInfo.CountryList) {
            countryArray = thisResource.countryInfo.CountryList;
            for (i = 0; i < countryArray.length; i += 1) {
                country = countryArray[i];
                countryHash[country.code] = country.name;
            }
        }
        thisResource.countryHash = countryHash;
    };
    /*
    Turns the marketInfo into a hash for quick lookups.
    Keying into the marketHash with the orgin station code you will get back an array of objects.
    Each object contains a destination station code that can be mapped to a station object using the stationHash.
    marketHash[originStationCode] = [ { "code": "destinationStationCode1", "name": "destinationStationCode2" } ]

           The station codes that do not match up to a station object are removed from the destinationArray with the
    JavaScript array method splice. Array.splice(startIndex, lengthOfElementsToRemove);
    The splice method takes the element out of the array completely, and reorders the indexes of the array
    */
    thisResource.populateMarketHash = function() {
        var i = 0,
                destinationArray = [],
                destination = {},
                station = {},
                originCode = "",
                marketHash = {},
                stationHash = thisResource.stationHash;

        if (thisResource.marketInfo && thisResource.marketInfo.MarketList) {
            marketHash = thisResource.marketInfo.MarketList;

            for (originCode in marketHash) {
                if (marketHash.hasOwnProperty(originCode)) {
                    destinationArray = marketHash[originCode];

                    for (i = destinationArray.length - 1; i >= 0; i -= 1) {
                        destination = destinationArray[i];
                        station = stationHash[destination.code];
                        if (station) {
                            destination.name = station.name;
                            destination.index = station.index;
                        } else {
                            destinationArray.splice(i, 1);
                        }
                    }
                    destinationArray.sort(this.destinationSort);
                }
            }
            thisResource.marketHash = marketHash;
        }
    };

    thisResource.destinationSort = function(stationOne, stationTwo) {
        var stationOneIndex = stationOne.index,
                stationTwoIndex = stationTwo.index;

        return stationOneIndex - stationTwoIndex;
    };

    /*
    The clientHash is data that has been stored in a cookie
    */
    thisResource.populateClientHash = function() {
        var cookie = window.document.cookie;
        var nameValueArray = [];
        var i = 0;
        var singleNameValue = '';
        var key = '';
        var value = '';
        var eqIndex = -1;
        if (cookie) {
            nameValueArray = document.cookie.split('; ');
            for (i = 0; i < nameValueArray.length; i += 1) {
                singleNameValue = nameValueArray[i];
                eqIndex = singleNameValue.indexOf('=');
                if (eqIndex > -1) {
                    key = singleNameValue.substring(0, eqIndex);
                    value = singleNameValue.substring(eqIndex + 1, singleNameValue.length);
                    if (key) {
                        value = SKYSALES.Util.decodeUriComponent(value);
                        thisResource.clientHash[key] = value;
                    }
                }
            }
        }
    };

    /*
    Turns the currencyInfo into a hash for quick lookups.
    Keying into the currencyHash with the currency code you will get back a currency object
    currencyHash[currencyCode] = { "currencyCode": "", "Description":"" };
    */
    thisResource.populateCurrencyHash = function() {
        var i = 0,
                currencyArray = [],
                currencyHash = {},
                currency = null,
                len = -1;

        if (thisResource.currencyInfo && thisResource.currencyInfo.CurrencyList) {
            currencyArray = thisResource.currencyInfo.CurrencyList;
            len = currencyArray.length;
            for (i = 0; i < len; i += 1) {
                currency = currencyArray[i];
                currencyHash[currency.code] = currency;
            }
        }
        thisResource.currencyHash = currencyHash;
    };

    thisResource.sortCountryStationHash = function(a, b) {
        return (a.name > b.name);
    }

    thisResource.joinMacStations = function() {
        if (SKYSALES.joinMacStations != "false") {
            for (sIdx in thisResource.stationHash) {
                if (thisResource.stationHash[sIdx].macCode != "") {
                    // let's create a station entry for this MAC!
                    if (!thisResource.stationHash[thisResource.stationHash[sIdx].macCode]) {
                        macStation = {
                            "countryCode": thisResource.stationHash[sIdx].countryCode,
                            "macCode": "",
                            "name": thisResource.macHash[thisResource.stationHash[sIdx].macCode].name,
                            "shortName": thisResource.macHash[thisResource.stationHash[sIdx].macCode].name,
                            "code": thisResource.macHash[thisResource.stationHash[sIdx].macCode].code
                        }
                        thisResource.stationHash[thisResource.stationHash[sIdx].macCode] = macStation;
                        thisResource.countryStationHash[thisResource.stationHash[sIdx].countryCode].push(macStation);
                        thisResource.countryStationHash[thisResource.stationHash[sIdx].countryCode].sort(thisResource.sortCountryStationHash);
                        thisResource.marketHash[thisResource.stationHash[sIdx].macCode] = [];
                    }
                    // let's add all the markets for this station on the new MAC station
                    myMarketHash = thisResource.marketHash[sIdx];
                    macMarketHash = thisResource.marketHash[thisResource.stationHash[sIdx].macCode];
                    for (dIdx in myMarketHash) {
                        alreadyInMarketHash = false;
                        for (mdIdx in macMarketHash) {
                            if (myMarketHash[dIdx].code == macMarketHash[mdIdx].code) {
                                alreadyInMarketHash = true;
                                break;
                            }
                        }
                        if (alreadyInMarketHash == false) {
                            thisResource.marketHash[thisResource.stationHash[sIdx].macCode].push(myMarketHash[dIdx]);
                        }
                    }
                    // scour all markets with destinations as this station and update
                    macStationMarket = {
                        "code": thisResource.stationHash[sIdx].macCode,
                        "name": thisResource.macHash[thisResource.stationHash[sIdx].macCode].name
                    }
                    for (mIndex in thisResource.marketHash) {
                        for (dIdx in thisResource.marketHash[mIndex]) {
                            if (thisResource.marketHash[mIndex][dIdx].code == thisResource.stationHash[sIdx].code) {
                                thisResource.marketHash[mIndex].push(macStationMarket);
                                delete thisResource.marketHash[mIndex][dIdx];
                            }
                        }
                    }
                    thisResource.macStationHash[sIdx] = thisResource.stationHash[sIdx];
                    delete thisResource.stationHash[sIdx];
                }
            }
        }
    };

    /*
    Turns the locationInfo into an array
    */
    thisResource.populateAOSLocationInfoArray = function(locationInfo, locationHash) {
        var i = 0,
                locationArray = [],
                len = 0,
                location = null,
                parentCode = '',
                parentLocation = null;

        locationInfo = locationInfo || {};
        locationArray = locationInfo.LocationList || [];
        len = locationArray.length;
        for (i = 0; i < len; i += 1) {
            location = locationArray[i];
            parentCode = location.parent;
            if (parentCode) {
                parentLocation = locationHash[parentCode];
                if (parentLocation) {
                    // need to add spaces for indentation to work in Internet Explorer
                    location.name = '\xa0\xa0\xa0\xa0' + parentLocation.name + ' - ' + location.name;
                    location.optionClass = 'subLocation';
                }
            }
        }

        return locationArray;
    };

    /*
    Turns the locationInfo into an array
    */
    thisResource.getAOSLocationHash = function(aosLocationInfo) {
        var i = 0,
                locationArray = null,
                locationHash = {},
                len = 0,
                location = null,
                locationInfo = aosLocationInfo || {};

        locationArray = locationInfo.LocationList || [];
        len = locationArray.length;
        for (i = 0; i < len; i += 1) {
            location = locationArray[i];
            locationHash[location.code] = location;
        }
        locationHash = locationHash || {};
        return locationHash;
    };

    /*
    Populate the object instance.
    This is accomplished by matching the name of the public menber 
    with the name of the key in the key: value pair of the JSON object that is passed in.
    It then turns the data into hash lists for quick lookups.
    */
    thisResource.setSettingsByObject = function(jsonObject) {
        parent.setSettingsByObject.call(this, jsonObject);
        SKYSALES.datepicker = this.datePickerInfo;
        thisResource.populateCountryHash();
        thisResource.populateStationList();
        thisResource.populateStationHash();
        thisResource.populateCarrierHash();
        thisResource.populateMacHash();
        thisResource.populateMarketHash();
        thisResource.joinMacStations();
        thisResource.populateClientHash();
        thisResource.populatePassengerHash();
        thisResource.populateCurrencyHash();
        thisResource.carLocationHash = this.getAOSLocationHash(this.carLocationInfo);
        thisResource.carLocationArray = this.populateAOSLocationInfoArray(this.carLocationInfo, this.carLocationHash);
        thisResource.activityLocationHash = this.getAOSLocationHash(this.activityLocationInfo);
        thisResource.activityLocationArray = this.populateAOSLocationInfoArray(this.activityLocationInfo, this.activityLocationHash);
        thisResource.hotelLocationHash = this.getAOSLocationHash(this.hotelLocationInfo);
        thisResource.hotelLocationArray = this.populateAOSLocationInfoArray(this.hotelLocationInfo, this.hotelLocationHash);
    };
    return thisResource;
};

/*
Name: 
Class Util
Param:
None
Return: 
None
Functionality:
Represents a Static Util object
Notes:
Provides common methods.
Used for inheritance - for example
var parent = new SKYSALES.Class.SkySales();
var theObject = SKYSALES.Util.extendObject(parent);
This class reads in the JSON object tags and instantiates them into running JavaScript
Class Hierarchy:
SkySales -> Resource
*/

SKYSALES.Util.createObjectArray = [];
SKYSALES.Util.createObject = function(objNameBase, objType, json) {
    var createObjectArray = SKYSALES.Util.createObjectArray;
    createObjectArray[createObjectArray.length] = {
        'objNameBase': objNameBase,
        'objType': objType,
        'json': json
    };
};

SKYSALES.Util.initObjects = function() {
    var i = 0;
    var createObjectArray = SKYSALES.Util.createObjectArray;
    var objName = '';
    var objectType = '';
    var json = null;
    var createObject = null;
    for (i = 0; i < createObjectArray.length; i += 1) {
        createObject = createObjectArray[i];
        objName = createObject.objNameBase + SKYSALES.Instance.getNextIndex();
        objectType = createObject.objType;
        json = createObject.json || {};
        if (SKYSALES.Class[objectType]) {
            SKYSALES.Instance[objName] = new SKYSALES.Class[objectType]();
            SKYSALES.Instance[objName].init(json);
        }
    }
    SKYSALES.Util.createObjectArray = [];
};

//Replace characters that could not be stored in a cookie
SKYSALES.Util.decodeUriComponent = function(str) {
    str = str || '';
    if (window.decodeURIComponent) {
        str = window.decodeURIComponent(str);
    }
    str = str.replace(/\+/g, ' ');
    return str;
};

//Replace characters for cookie storage
SKYSALES.Util.encodeUriComponent = function(str) {
    str = str || '';
    if (window.encodeURIComponent) {
        str = window.encodeURIComponent(str);
    }
    return str;
};

//Return the main resource instance, this object is instantiated in the common.xslt
SKYSALES.Util.getResource = function() {
    return SKYSALES.Resource;
};

//Douglas Crockford's inheritance method
SKYSALES.Util.extendObject = function(o) {
    var F = function() { };
    F.prototype = o;
    return new F();
};

//Instantiates an object from an html object tag
SKYSALES.Util.initializeNewObject = function(paramObject) {
    var objName = "";

    var defaultSetting = {
        objNameBase: '',
        objType: '',
        selector: ''
    };
    var validateParamObject = function() {
        var retVal = true;
        $().extend(defaultSetting, paramObject);
        var propName = null;
        for (propName in defaultSetting) {
            if (defaultSetting.hasOwnProperty(propName)) {
                if (defaultSetting[propName] === undefined) {
                    retVal = false;
                    break;
                }
            }
        }
        return retVal;
    };
    var paramNodeFunction = function(index) {
        var paramNodeValue = $(this).val();
        var parsedJsonObject = SKYSALES.Json.parse(paramNodeValue);
        var funRef = null;
        var refName = '';
        var refArray = [];
        var i = 0;
        var refIndex = 0;
        var arrayRegex = /^([a-zA-Z0-9]+)\[(\d+)\]$/;
        var matchArray = [];
        if (parsedJsonObject.method !== undefined) {
            funRef = SKYSALES.Instance[objName];
            if (parsedJsonObject.method.name.indexOf('.') > -1) {
                refArray = parsedJsonObject.method.name.split('.');
                for (i = 0; i < refArray.length; i += 1) {
                    refName = refArray[i];
                    matchArray = refName.match(arrayRegex);
                    if ((matchArray) && (matchArray.length > 0)) {
                        refName = matchArray[1];
                        refIndex = matchArray[2];
                        refIndex = parseInt(refIndex, 10);
                        funRef = funRef[refName][refIndex];
                    }
                    else {
                        funRef = funRef[refName];
                    }
                }
            }
            else {
                funRef = funRef[parsedJsonObject.method.name];
            }

            if (funRef) {
                funRef(parsedJsonObject.method.paramJsonObject);
            }
        }
    };
    var objectNodeFunction = function() {
        objName = paramObject.objNameBase + SKYSALES.Instance.getNextIndex();
        if (SKYSALES.Class[paramObject.objType]) {
            SKYSALES.Instance[objName] = new SKYSALES.Class[paramObject.objType]();
            $("object.jsObject > param", this).each(paramNodeFunction);
        }
        else {
            alert("Object Type Not Found: " + paramObject.objType);
        }
    };
    var containerFunction = function() {
        var isValid = validateParamObject();
        if (isValid) {
            $(paramObject.selector).each(objectNodeFunction);
        }
        else {
            alert("\nthere has been an error");
        }
    };
    containerFunction();
    return false;
};

/*
Populates a html select box
An Option object should always be used instead of writing <option> nodes to the dom.
Writing <option> nodes to the dom has issues in IE6
*/
SKYSALES.Util.populateSelect = function(paramObj) {
    var selectedItem = paramObj.selectedItem || null;
    var objectArray = paramObj.objectArray || null;
    var selectBox = paramObj.selectBox || null;
    var showCode = paramObj.showCode || false;
    var clearOptions = paramObj.clearOptions || false;
    var text = '';
    var value = '';
    var selectBoxObj = null;
    var obj = null;
    var prop = '';

    if (selectBox) {
        selectBoxObj = selectBox.get(0);
        if (selectBoxObj && selectBoxObj.options) {
            if (clearOptions) {
                selectBoxObj.options.length = 0;
            }
            else {
                if (!selectBoxObj.originalOptionLength) {
                    selectBoxObj.originalOptionLength = selectBoxObj.options.length;
                }
                selectBoxObj.options.length = selectBoxObj.originalOptionLength;
            }
            if (objectArray) {
                for (prop in objectArray) {
                    if (objectArray.hasOwnProperty(prop)) {
                        obj = objectArray[prop];
                        if (showCode) {
                            text = obj.name + ' (' + obj.code + ')';
                        }
                        else {
                            text = obj.name || obj.Name;
                        }
                        value = obj.code || obj.ProvinceStateCode || '';
                        selectBoxObj.options[selectBoxObj.options.length] = new window.Option(text, value, false, false);
                    }
                }
                if (selectedItem !== null) {

                    selectBox.val(selectedItem);

                }
            }
        }
    }
};
/*
Populates a html select box using option groups
An Option object should always be used instead of writing <option> nodes to the dom.
Writing <option> nodes to the dom has issues in IE6
*/
SKYSALES.Util.populateSelectWithGroups = function(paramObj) {
    var selectedItem = paramObj.selectedItem || null;
    var objectArray = paramObj.objectArray || null;
    var groupArray = paramObj.groupArray || null;
    var selectBox = paramObj.selectBox || null;
    var showCode = paramObj.showCode || false;
    var clearOptions = paramObj.clearOptions || false;
    var promoStationsArray = paramObj.promoStationsArray || [];
    var restrictedStationCategory = paramObj.restrictedStationCategory || [];
    var restrictedPairStationCategory = paramObj.restrictedPairStationCategory || [];
    var originObject = paramObj.originObject || null;
    var text = '';
    var value = null;
    var selectBoxObj = null;
    var groupProp = null;
    var station = null;
    var prop = null;
    var opt = null;
    var optGroup = null;
    var stationCategories = [];
    var isStationAllowed = true;
    var isMarketPairAllowed = true;
    var index = 0;

    var isIn = false;
    var isInGroup = false;
    var optionHeaderText = paramObj.optionHeaderText || ["Origin", "Destination"]; // Todo: Create localized string data source

    if (selectBox) {
        selectBoxObj = selectBox.get(0);
        if (selectBoxObj && selectBoxObj.options) {
            if (clearOptions) {
                selectBoxObj.options.length = 0;
            }
            else {
                if (!selectBoxObj.originalOptionLength) {
                    selectBoxObj.originalOptionLength = selectBoxObj.options.length;
                }

                try {
                    selectBoxObj.options.length = selectBoxObj.originalOptionLength;
                }
                catch (e) { }

                // clear select menu and append optgroups - Todo: rework to use "clearOptions" correctly
                while (selectBoxObj.hasChildNodes()) {
                    selectBoxObj.removeChild(selectBoxObj.firstChild);
                }
            }
            if (objectArray && groupArray) {
                // Filter out non-destinations                
                for (groupProp in groupArray) {
                    if (groupArray.hasOwnProperty(groupProp)) {
                        for (prop in groupArray[groupProp]) {
                            if (groupArray[groupProp].hasOwnProperty(prop) && prop !== "name") {
                                station = groupArray[groupProp][prop] || {};
                                isStationAllowed = true;

                                if (station.stationCategories) {
                                    for (index = 0; index < restrictedStationCategory.length; index = index + 1) {
                                        if (station.stationCategories.indexOf(restrictedStationCategory[index]) > -1) {
                                            isStationAllowed = false;
                                            break;
                                        }
                                    }
                                }
                                if (isStationAllowed) {
                                    for (value in objectArray) {
                                        isMarketPairAllowed = true;
                                        var isInPromo = true;
                                        // Filter out non-promo stations
                                        if (promoStationsArray.length > 0) {
                                            isInPromo = false;
                                            for (arrayIndex in promoStationsArray) {
                                                if (promoStationsArray[arrayIndex] == value || promoStationsArray[arrayIndex] == objectArray[value].code) {
                                                    isInPromo = true;
                                                    break;
                                                }
                                            }
                                        }
                                        if (originObject && station.stationCategories && originObject.stationCategories) {
                                            for (index = 0; index < restrictedPairStationCategory.length; index = index + 1) {
                                                if (station.stationCategories.indexOf(restrictedPairStationCategory[index]) > -1 &&
                                                originObject.stationCategories.indexOf(restrictedPairStationCategory[index]) > -1 &&
                                                station.stationCategories[station.stationCategories.indexOf(restrictedPairStationCategory[index])] ==
                                                originObject.stationCategories[originObject.stationCategories.indexOf(restrictedPairStationCategory[index])]) {
                                                    isMarketPairAllowed = false;
                                                    break;
                                                }
                                            }
                                            if (!isMarketPairAllowed) {
                                                break;
                                            }
                                        }
                                        if ((value == station.code || objectArray[value].code == station.code) && isInPromo) {
                                            isIn = true;
                                            isInGroup = true;
                                            break;
                                        }
                                    }
                                }
                                groupArray[groupProp][prop].show = (isIn) ? true : false;
                                isIn = false;
                            }
                        }
                        groupArray[groupProp].show = (isInGroup) ? true : false;
                        isInGroup = false;
                    }
                }
                // Insert option header into select box (ie "Origin")
                opt = document.createElement("option");
                opt.value = "";
                selectBoxName = selectBoxObj.name.toLowerCase();
                opt.appendChild(document.createTextNode(optionHeaderText[(selectBoxName.indexOf("origin") > 0) ? 0 : 1]));
                selectBoxObj.appendChild(opt);
                for (groupProp in groupArray) {
                    if (groupArray.hasOwnProperty(groupProp) && groupArray[groupProp].show && groupProp !== "show") {
                        if (groupArray[groupProp].show) {
                            optGroup = document.createElement("optgroup");
                            optGroup.label = groupArray[groupProp].name || groupProp;
                        }
                        for (prop in groupArray[groupProp]) {
                            if (groupArray[groupProp].hasOwnProperty(prop) && prop !== "show" && prop !== "name" && groupArray[groupProp][prop].show) { // Filter out "name" and "show" property to avoid "undefined" item in list
                                station = groupArray[groupProp][prop];
                                opt = document.createElement("option");
                                opt.value = station.code;
                                text = (showCode) ? station.name + ' (' + station.code + ')' : station.name;
                                opt.appendChild(document.createTextNode(text));
                                optGroup.appendChild(opt);
                            }
                        }
                        selectBoxObj.appendChild(optGroup);
                    }
                }
                if (selectedItem !== null) {

                    selectBox.val(selectedItem);
                }
            }
        }
    }
};


SKYSALES.Util.cloneArray = function(array) {
    return array.concat();
};

SKYSALES.Util.convertToLocaleCurrency = function(num) {
    var json = {
        'num': num
    };
    var localeCurrency = new SKYSALES.Class.LocaleCurrency();
    localeCurrency.init(json);
    return localeCurrency.currency;
};

/*
Retrieves the text representation (HHMMSS) of the time.
*/
SKYSALES.Util.getTime = function(hour, minutes, seconds) {
    var time = '';

    hour = Number(hour);
    minutes = Number(minutes);
    seconds = Number(seconds);

    if (isNaN(hour) === false) {
        hour += 1;

        if (hour > 12) {
            hour = hour - 12;
        }

        if (hour.toString().length === 1) {
            hour = '0' + hour;
        }

        time = hour;

        if (isNaN(minutes) === false) {
            if (minutes.toString().length === 1) {
                minutes = '0' + minutes;
            }

            time = time + ':' + minutes;

            if (isNaN(seconds) === false) {
                if (seconds.toString().length === 1) {
                    seconds = '0' + seconds;
                }

                time = time + ':' + seconds;
            }
        }

        if (hour > 12) {
            time = time + ' PM';
        }
        else {
            time = time + ' AM';
        }
    }

    return time;
};

/*
Retrieves the text representation (YYYYMMDD) of the date.
*/
SKYSALES.Util.getDate = function(year, month, day) {
    var date = '';

    year = Number(year);
    month = Number(month);
    day = Number(day);

    if (isNaN(year) === false) {
        date = year;

        if (isNaN(month) === false) {
            if (month.toString().length === 1) {
                month = '0' + month;
            }

            date = date.toString() + month.toString();

            if (isNaN(day) === false) {
                if (day.toString().length === 1) {
                    day = '0' + day;
                }

                date = date.toString() + day.toString();

            }
        }
    }

    return date;
};

/*
Returns the text representation of the date depending on the dateFormat indicated
usually format will come from Default.xml
Parameters: 
dateText: "mm/dd/yyyy"
dateFormat: format used to display date,
    D - DayNameShort            DD - DayNameLong
    M - MonthNameShort          MM - MonthNameLong
    d - dayNum short format     dd - dayNum long format
    y - yearNum short format    yy - yearNum long format
    ex: 'D, M d y'    --> Mon, Jan 1 00 / Mon Jan 25 00
        'D, MM dd yy' --> Mon, January 01 2000
        'DD, MM dd yy'--> Monday, January 01 2000
*/
SKYSALES.Util.dateDisplayFormat = function(dateText, dateFormat) {
	/*Added null checking by Sean on 11 July 2012*/
	if (dateText != null) {
		var resource = SKYSALES.Util.getResource(),
		    dateParams = dateText.split('/'),
		    dateObject = new Date(dateParams[2], dateParams[0] - 1, dateParams[1]);
		if (dateText) {
			return ($.datepicker.formatDate(dateFormat, dateObject,
				{
					dayNamesShort: resource.dateCultureInfo.dayNamesShort,
					dayNames: resource.dateCultureInfo.dayNames,
					monthNamesShort: resource.dateCultureInfo.monthNamesShort,
					monthNames: resource.dateCultureInfo.monthNames
				}));
		}
	}
}

/*
Name: 
Class SkySales
Param:
None
Return: 
An instance of SkySales
Functionality:
This is the SkySales base class that most objects inherit from
Notes:
This class provides the ability to show and hide objects based on their container.
Class Hierarchy:
SkySales
*/
if (!SKYSALES.Class.SkySales) {
    SKYSALES.Class.SkySales = function() {
        var thisSkySales = this;

        thisSkySales.containerId = '';
        thisSkySales.container = null;

        thisSkySales.init = SKYSALES.Class.SkySales.prototype.init;
        thisSkySales.getById = SKYSALES.Class.SkySales.prototype.getById;
        thisSkySales.setSettingsByObject = SKYSALES.Class.SkySales.prototype.setSettingsByObject;
        thisSkySales.addEvents = SKYSALES.Class.SkySales.prototype.addEvents;
        thisSkySales.setVars = SKYSALES.Class.SkySales.prototype.setVars;
        thisSkySales.hide = SKYSALES.Class.SkySales.prototype.hide;
        thisSkySales.show = SKYSALES.Class.SkySales.prototype.show;
        return thisSkySales;
    };
    SKYSALES.Class.SkySales.prototype.init = function(json) {
        this.setSettingsByObject(json);
        this.setVars();
    };
    SKYSALES.Class.SkySales.prototype.getById = function(id) {
        var retVal = null;
        if (id) {
            retVal = window.document.getElementById(id);
        }
        if (retVal) {
            retVal = $(retVal);
        }
        else {
            retVal = $([]);
        }
        return retVal;
    };
    SKYSALES.Class.SkySales.prototype.setSettingsByObject = function(json) {
        var propName = '';
        for (propName in json) {
            if (json.hasOwnProperty(propName)) {
                if (this[propName] !== undefined) {
                    this[propName] = json[propName];
                }
            }
        }
    };
    SKYSALES.Class.SkySales.prototype.addEvents = function() { };
    SKYSALES.Class.SkySales.prototype.setVars = function() {
        this.container = $('#' + this.containerId);
    };
    SKYSALES.Class.SkySales.prototype.hide = function() {
        this.container.hide('slow');
    };
    SKYSALES.Class.SkySales.prototype.show = function() {
        this.container.show('slow');
    };
}

/*
Name: 
Class BaseToggleView
Param:
None
Return: 
An instance of BaseToggleView
Functionality:
This is the Base class for any class that can show or hide itself.
The containerId is the id of the domelement that you wish to show and hide.
Notes:
Class Hierarchy:
SkySales -> BaseToggleView
*/
if (!SKYSALES.Class.BaseToggleView) {
    SKYSALES.Class.BaseToggleView = function() {
        var parent = SKYSALES.Class.SkySales();
        var thisBaseToggleView = SKYSALES.Util.extendObject(parent);

        thisBaseToggleView.toggleViewIdArray = [];
        thisBaseToggleView.toggleViewArray = [];

        thisBaseToggleView.addToggleView = function(json) {
            if (json.toggleViewIdArray) {
                json = json.toggleViewIdArray;
            }
            var toggleViewIdArray = json || [];
            var toggleViewIdObj = null;
            var i = 0;
            var toggleView = null;
            if (toggleViewIdArray.length === undefined) {
                toggleViewIdArray = [];
                toggleViewIdArray[0] = json;
            }
            for (i = 0; i < toggleViewIdArray.length; i += 1) {
                toggleViewIdObj = toggleViewIdArray[i];
                toggleView = new SKYSALES.Class.ToggleView();
                toggleView.init(toggleViewIdObj);
                thisBaseToggleView.toggleViewArray[thisBaseToggleView.toggleViewArray.length] = toggleView;
            }
        };
        return thisBaseToggleView;
    };
}

/*
Name: 
Class Date
Param:
None
Return: 
An instance of Date
Functionality:
The object that represents the DateTime object.
Notes:
This class contains the month, day and year with hours, minutes and seconds of a DateTime object.
SkySales -> Date
*/
SKYSALES.Class.Date = function() {
    var parent = new SKYSALES.Class.SkySales(),
    thisDate = SKYSALES.Util.extendObject(parent);

    thisDate.Day = '';
    thisDate.Month = '';
    thisDate.Hour = '';
    thisDate.Minute = '';
    thisDate.Second = '';
    thisDate.Year = '';
    thisDate.date = null;

    thisDate.init = function(json) {
        this.setSettingsByObject(json);
        this.initDateTime();
    };

    thisDate.initDateTime = function() {
        this.date = new Date();
        this.date.setHours(this.Hour, this.Minute, this.Second, 0);
        this.date.setFullYear(this.Year, this.Month - 1, this.Day);
    };

    thisDate.getTime = function() {
        var time = SKYSALES.Util.getTime(this.Hour, this.Minute);
        return time;
    };

    thisDate.getDate = function() {
        var date = SKYSALES.Util.getDate(this.Year, this.Month, this.Day);
        return date;
    };

    return thisDate;
};

/*
Name: 
Class BaggagePrompt
Param:
None
Return: 
An instance of BaggagePrompt
Functionality:
The object that alerts a baggage prompt after all images are .
Notes:
Class Hierarchy:
SkySales -> BaggagePrompt
*/
if (!SKYSALES.Class.BaggagePrompt) {
    SKYSALES.Class.BaggagePrompt = function() {
        var parent = new SKYSALES.Class.SkySales(),
    thisBaggagePrompt = SKYSALES.Util.extendObject(parent);

        thisBaggagePrompt.baggagePrompt = '';

        thisBaggagePrompt.init = function(json) {
            this.setSettingsByObject(json);
            thisBaggagePrompt.displayPrompt();
        };

        thisBaggagePrompt.displayPrompt = function() {
            $(window).load(function() {
                if (thisBaggagePrompt.baggagePrompt != '')
                {
                    alert(thisBaggagePrompt.baggagePrompt);
                }
            });
        };

        return thisBaggagePrompt;
    };
    SKYSALES.Class.BaggagePrompt.createObject = function(json) {
        SKYSALES.Util.createObject('baggagePrompt', 'BaggagePrompt', json);
    };
}

/*
Name: 
Class redCarpetLegInput
Param:
None
Return: 
An instance of redCarpetLegInput
Functionality:
This class represents the redCarpetLegInput control in the traveler page.
Notes:
Class Hierarchy:
SkySales -> redCarpetLegInput
*/
SKYSALES.Class.RedCarpetLegInput = function() {
    var parent = new SKYSALES.Class.SkySales(),
    thisRedCarpetLegInput = SKYSALES.Util.extendObject(parent);

    thisRedCarpetLegInput.clientId = '';
    thisRedCarpetLegInput.ssrAvailabilityArray = null;
    thisRedCarpetLegInput.checkBoxRedCarpetArray = [];
    //thisRedCarpetLegInput.isFlightChange = false;
    thisRedCarpetLegInput.disableMarket = null;

    thisRedCarpetLegInput.init = function(json) {
        this.setSettingsByObject(json);
        this.setVars();
        this.addEvents();
        this.initSsrAvailabilityArray();
        this.initCheckBoxRedCarpetLegInputArray();
    };
    
    thisRedCarpetLegInput.initSsrAvailabilityArray = function() {
        var i = 0,
        ssrAvailabilityArrayObject = this.ssrAvailabilityArray || [],
        len = ssrAvailabilityArrayObject.length,
        ssrAvailability = null;
        for (i = 0; i < len; i += 1) {
            ssrAvailability = new SKYSALES.Class.SSRAvailability();
            ssrAvailability.init(ssrAvailabilityArrayObject[i]);
            this.ssrAvailabilityArray[i] = ssrAvailability;
        }
    };
    
 thisRedCarpetLegInput.initCheckBoxRedCarpetLegInputArray = function() {
        var i = 0,
        checkBoxRedCarpetArrayObject = this.checkBoxRedCarpetArray || [],
        len = checkBoxRedCarpetArrayObject.length,
        checkBoxRedCarpet = null;
        
        for (i = 0; i < len; i += 1) {
            checkBoxRedCarpet = new SKYSALES.Class.CheckBoxRedCarpetLegInput();
            if (this.disableMarket != "none" && i < parseInt(this.disableMarket)) {
                    checkBoxRedCarpet.isDisabled = true;
            }
            //checkBoxRedCarpet.isFlightChange = this.isFlightChange;
            checkBoxRedCarpet.init(checkBoxRedCarpetArrayObject[i], this.ssrAvailabilityArray[0]);
            this.checkBoxRedCarpetArray[i] = checkBoxRedCarpet;
        }
    };

    return thisRedCarpetLegInput;

}

/*
Creates the RedCarpetLegInput class.
*/
SKYSALES.Class.RedCarpetLegInput.createObject = function(json) {
    SKYSALES.Util.createObject('redCarpetLegInput', 'RedCarpetLegInput', json);
};

/*
Name: 
Class CheckBoxRedCarpetLegInput
Param:
None
Return: 
An instance of CheckBoxRedCarpetLegInput
Functionality:
This class represents the checkbox portable media input web control in the traveler page.
Notes:
Class Hierarchy:
SkySales -> CheckBoxRedCarpetLegInput
*/
SKYSALES.Class.CheckBoxRedCarpetLegInput = function() {
    var parent = new SKYSALES.Class.SkySales(),
    thisCheckBoxRedCarpetLegInput = SKYSALES.Util.extendObject(parent);

    thisCheckBoxRedCarpetLegInput.checkBoxRedCarpetId = '';
    thisCheckBoxRedCarpetLegInput.checkBoxRedCarpet = {};
    thisCheckBoxRedCarpetLegInput.quantityPrefix = '';
    thisCheckBoxRedCarpetLegInput.del = '';
    thisCheckBoxRedCarpetLegInput.passengerNumber = 0;
    thisCheckBoxRedCarpetLegInput.flightReference = '';
    thisCheckBoxRedCarpetLegInput.isFlightChange = false;
    thisCheckBoxRedCarpetLegInput.isDisabled = false;
    thisCheckBoxRedCarpetLegInput.checkBoxCounterCheckinId = '';
    thisCheckBoxRedCarpetLegInput.price = '';
    thisCheckBoxRedCarpetLegInput.disableJourney = false;
   
    thisCheckBoxRedCarpetLegInput.init = function(json, ssrAvailability) {
        this.setSettingsByObject(json);
        this.setVars();
        this.initCheckBoxRedCarpetLegInput();
        this.addEvents();
    };
    
     thisCheckBoxRedCarpetLegInput.setVars = function() {
        if (this.price.substring(0, 1) != 0)
        {
            this.disableJourney = true;
        }
        thisCheckBoxRedCarpetLegInput.checkBoxCounterCheckinId = thisCheckBoxRedCarpetLegInput.checkBoxRedCarpetId.replace('RedCarpet','CounterCheckin');
    };
    
    thisCheckBoxRedCarpetLegInput.initCheckBoxRedCarpetLegInput = function() {
       thisCheckBoxRedCarpetLegInput.disableCounterCheckin();
    };
   
    thisCheckBoxRedCarpetLegInput.addEvents = function(){
        if (this.disableJourney == true){
            $('#' + this.checkBoxRedCarpetId).click(thisCheckBoxRedCarpetLegInput.disableCounterCheckin);
           
        }
    }
    
    thisCheckBoxRedCarpetLegInput.disableCounterCheckin = function(){
        if ($('#' + thisCheckBoxRedCarpetLegInput.checkBoxRedCarpetId).attr('checked'))
        {
            $('#' + thisCheckBoxRedCarpetLegInput.checkBoxCounterCheckinId).attr('checked', false);
            $('#' + thisCheckBoxRedCarpetLegInput.checkBoxCounterCheckinId).attr('disabled', true);
        }
        else
        {
            $('#' + thisCheckBoxRedCarpetLegInput.checkBoxCounterCheckinId).attr('disabled', false);
        }
    }
    
    return thisCheckBoxRedCarpetLegInput;
}
/*
Name: 
Class BookingDetail
Param:
None
Return: 
An instance of BookingDetail
Functionality:

Notes:
Class Hierarchy:
SkySales -> BookingDetail
*/
if (!SKYSALES.Class.BookingDetail) {
    SKYSALES.Class.BookingDetail = function() {
        var parent = new SKYSALES.Class.SkySales(),
    thisBookingDetail = SKYSALES.Util.extendObject(parent);

        thisBookingDetail.ssrChangePrompt = '';
        thisBookingDetail.showSsrPrompter = '';
        thisBookingDetail.confirmButtonId = '';

        thisBookingDetail.init = function(json) {
            this.setSettingsByObject(json);
            this.addEvent();
        };

        thisBookingDetail.addEvent = function() {
            var confirmButton = '';
            $('#' + this.confirmButtonId).click(this.displayPromptHandler);
            confirmButton = this.confirmButtonId.split('_');
            $('#' + confirmButton[0] + '_' + confirmButton[2]).click(this.displayPromptHandler);

        };

        thisBookingDetail.displayPromptHandler = function() {
            return thisBookingDetail.displayPrompt();
        }

        thisBookingDetail.displayPrompt = function() {
            if (this.showSsrPrompter === 'true') {
                return confirm(this.ssrChangePrompt);
            }
        };

        return thisBookingDetail;
    };
    SKYSALES.Class.BookingDetail.createObject = function(json) {
        SKYSALES.Util.createObject('bookingDetail', 'BookingDetail', json);
    };
}

/*

/*
Name: 
Class FlightSearch
Param:
None
Return: 
An instance of FlightSearch
Functionality:
The object that initializes the AvailabilitySearchInput control
Notes:
This class contains and creates all of the objects necessary to add functionality to the AvailabilitySearchInput control
Class Hierarchy:
SkySales -> FlightSearch
*/
if (!SKYSALES.Class.FlightSearch) {
    SKYSALES.Class.FlightSearch = function() {
        var parent = new SKYSALES.Class.SkySales();
        var thisFlightSearch = SKYSALES.Util.extendObject(parent);

        thisFlightSearch.optionHeaderText = null;

        thisFlightSearch.promoStationsArray = null;
        thisFlightSearch.marketArray = null;
        thisFlightSearch.flightTypeInputIdArray = null;
        thisFlightSearch.countryInputIdArray = null;
        thisFlightSearch.confirmTextArray = null;
        thisFlightSearch.actionId = "";
        thisFlightSearch.action = null;

        thisFlightSearch.dropDownAdtId = "";
        thisFlightSearch.dropDownInfId = "";
        thisFlightSearch.dropDownChdId = "";
        thisFlightSearch.tooManyInfantsText = "";
        thisFlightSearch.tooManyPassengersTextPre = "";
        thisFlightSearch.tooManyPassengersTextPost = "";
        thisFlightSearch.noPassengersText = "";
        thisFlightSearch.infantArray = null;
        thisFlightSearch.infantAgeText = "";

        thisFlightSearch.updateButtonId = "";
        thisFlightSearch.updateButton = null;
        thisFlightSearch.updateButtonClicked = false;

        thisFlightSearch.dateRangeError = "";
        thisFlightSearch.dateSameText = "";
        thisFlightSearch.originalDate = [];
        thisFlightSearch.disableInputId = "";
        thisFlightSearch.insuranceControlId = "";
        thisFlightSearch.checkboxNotification = "";
        thisFlightSearch.maxpaxnum = "";
        thisFlightSearch.autoMacArray = [];
        thisFlightSearch.autoMacMessage = "";
        thisFlightSearch.autoMacShown = ' ';

        thisFlightSearch.restrictedStationCategories = [];
        thisFlightSearch.restrictedPairStationCategory = [];
        thisFlightSearch.localizedMac = [];

        thisFlightSearch.jsDateFormat = "";
        thisFlightSearch.clientID = "";

        var countryInputArray = [];
        var flightTypeInputArray = [];

        thisFlightSearch.init = function(paramObject) {
            this.setSettingsByObject(paramObject);
            this.setVars();
            this.addEvents();
            this.initFlightTypeInputIdArray();
            this.initCountryInputIdArray();
            this.populateFlightType();
            this.initAutoMacArray();
            this.initDateDisplay();
        };

        thisFlightSearch.initDateDisplay = function() {
            $(document).ready(function() {
                $('#' + thisFlightSearch.clientID + 'date_picker_display_id_1').click(function() {
                    $(this).parent().find('.datepicker_trigger').click();
                });
                $('#' + thisFlightSearch.clientID + 'date_picker_display_id_2').click(function() {
                    $(this).parent().find('.datepicker_trigger').click();
                });
                $('#' + thisFlightSearch.clientID + 'date_picker_display_id_1').focus(function() {
                    $('#' + thisFlightSearch.clientID + 'date_picker_display_id_1').click();
                });
                $('#' + thisFlightSearch.clientID + 'date_picker_display_id_2').focus(function() {
                    $('#' + thisFlightSearch.clientID + 'date_picker_display_id_2').click();
                });

                $('#' + thisFlightSearch.clientID + 'date_picker_display_id_1').val(SKYSALES.Util.dateDisplayFormat($('#' + thisFlightSearch.clientID + 'date_picker_id_1').val(), thisFlightSearch.jsDateFormat));
                $('#' + thisFlightSearch.clientID + 'date_picker_display_id_2').val(SKYSALES.Util.dateDisplayFormat($('#' + thisFlightSearch.clientID + 'date_picker_id_2').val(), thisFlightSearch.jsDateFormat));
            });
        }

        thisFlightSearch.initAutoMacArray = function() {
            for (amIdx in thisFlightSearch.autoMacArray) {
                if (SKYSALES.Resource.macHash[thisFlightSearch.autoMacArray[amIdx].MacStation]) {
                    thisFlightSearch.autoMacArray[amIdx].Stations = SKYSALES.Resource.macHash[thisFlightSearch.autoMacArray[amIdx].MacStation].stations;
                }
            }
        };


        thisFlightSearch.setSettingsByObject = function(json) {
            parent.setSettingsByObject.call(this, json);

            var i = 0;
            var marketArray = this.marketArray || [];
            var market = null;
            for (i = 0; i < marketArray.length; i += 1) {
                market = new SKYSALES.Class.FlightSearchMarket();
                market.optionHeaderText = this.optionHeaderText;
                market.flightSearch = this;
                market.index = i;
                market.autoMacArray = thisFlightSearch.autoMacArray;
                market.restrictedStationCategories = this.restrictedStationCategories;
                market.restrictedPairStationCategory = this.restrictedPairStationCategory;
                market.localizedMac = thisFlightSearch.localizedMac;
                market.init(marketArray[i]);
                this.marketArray[i] = market;
            }
        };

        thisFlightSearch.setVars = function() {
            parent.setVars.call(this);
            thisFlightSearch.action = $('#' + this.actionId);
            thisFlightSearch.updateButton = $("input[type='submit'][id^='" + this.updateButtonId + "']");
        };

        thisFlightSearch.addEvents = function() {
            parent.addEvents.call(this);
            if (thisFlightSearch.actionId) {
                thisFlightSearch.action.click(thisFlightSearch.searchHandler);
            }
            if (thisFlightSearch.updateButtonId) {
                thisFlightSearch.updateButton.click(thisFlightSearch.searchUpdateHandler);
            }
        };

        thisFlightSearch.searchUpdateHandler = function() {
            thisFlightSearch.updateButtonClicked = true;
            return thisFlightSearch.searchHandler();
        };

        thisFlightSearch.searchHandler = function() {

            // uncomment these lines if you want to prevent the prompter to
            // appear when transitioning from Select to Traveler page.
            //
            //if (!(/SelectView/i.test(thisFlightSearch.actionId)) ||
            //(/SelectView/i.test(thisFlightSearch.actionId) && thisFlightSearch.updateButtonClicked == true)) {
            thisFlightSearch.checkAutoMac();
            //}
            //thisFlightSearch.updateButtonClicked = false;

            if (/SearchView/i.test(thisFlightSearch.actionId)
                || /SelectView/i.test(thisFlightSearch.actionId)
                || /CompactView/i.test(thisFlightSearch.actionId)) {
                if (thisFlightSearch.tooManyInfantsText != "") {
                    if (thisFlightSearch.checkInfantPax() == false) { SKYSALES.RemoveLoadingBar(); return false; } //modified by Linson at 2011-12-31
                }
                if (thisFlightSearch.tooManyPassengersTextPre != "" || thisFlightSearch.noPassengersText != "") {
                    if (thisFlightSearch.checkNumPax() == false) { SKYSALES.RemoveLoadingBar(); return false; } //modified by Linson at 2011-12-31
                }
                if (thisFlightSearch.checkSameDate() == false) { SKYSALES.RemoveLoadingBar(); return false; } //modified by Linson at 2011-12-31
            }
            else if (/SearchChangeView/i.test(thisFlightSearch.actionId)) {
            //modified by Linson at 2011-12-31 begin
            var ifDisplayLockLayer = true;
            ifDisplayLockLayer = thisFlightSearch.searchChangeHandler();
            if (ifDisplayLockLayer == false) SKYSALES.RemoveLoadingBar();
            return ifDisplayLockLayer; //thisFlightSearch.searchChangeHandler();
            //modified by Linson at 2011-12-31 end
            }
        };

        thisFlightSearch.checkAutoMac = function() {
            for (j = 0; j < thisFlightSearch.marketArray.length; j += 1) {
                isAutoMac = false;
                autoMacStations = '';
                for (i = 0; i < thisFlightSearch.autoMacArray.length; i += 1) {
                    if (thisFlightSearch.autoMacShown.indexOf(thisFlightSearch.autoMacArray[i].MacStation) < 1) {
                        for (h = 0; h < thisFlightSearch.autoMacArray[i].Stations.length; h += 1) {
                            if ($('#' + thisFlightSearch.marketArray[j].marketInputIdArray[0].originId).val() ==
                            thisFlightSearch.autoMacArray[i].Stations[h] ||
                            $('#' + thisFlightSearch.marketArray[j].marketInputIdArray[0].destinationId).val() ==
                            thisFlightSearch.autoMacArray[i].Stations[h] ||
                            $('#' + thisFlightSearch.marketArray[j].marketInputIdArray[0].originId).val() ==
                            thisFlightSearch.autoMacArray[i].MacStation ||
                            $('#' + thisFlightSearch.marketArray[j].marketInputIdArray[0].destinationId).val() ==
                            thisFlightSearch.autoMacArray[i].MacStation
                            ) {
                                for (g = 0; g < thisFlightSearch.autoMacArray[i].Stations.length; g += 1) {
                                    stationCode = thisFlightSearch.autoMacArray[i].Stations[g];
                                    if (SKYSALES.Resource.macStationHash[stationCode]) {
                                        autoMacStations += "\n" + SKYSALES.Resource.macStationHash[stationCode].name + ' (' + stationCode + ')';
                                    }
                                    else {
                                        if (SKYSALES.Resource.stationHash[stationCode]) {
                                            autoMacStations += "\n" + SKYSALES.Resource.stationHash[stationCode].name + ' (' + stationCode + ')';
                                        }
                                    }
                                }
                                thisFlightSearch.autoMacShown += ' ' + thisFlightSearch.autoMacArray[i].MacStation + ' ';
                                thisFlightSearch.autoMacMessage = thisFlightSearch.autoMacArray[i].MacPrompter;
                                isAutoMac = true;
                                break;
                            }
                        }
                    }
                    if (isAutoMac == true) { break; }
                }
                if ((autoMacStations != '') && (thisFlightSearch.autoMacMessage)) {
                    alert(thisFlightSearch.autoMacMessage);
                    SKYSALES.RemoveLoadingBar();  //added by Linson at 2011-12-31
                }
            }
        }

        thisFlightSearch.checkSameDate = function() {
            if (thisFlightSearch.dateSameText != "" &&
            $('#' + thisFlightSearch.flightTypeInputIdArray[1].checkBoxId).is(':checked') == false) {
                if ($('#' + thisFlightSearch.marketArray[0].marketDateIdArray[0].marketDateId).val() ==
                $('#' + thisFlightSearch.marketArray[1].marketDateIdArray[0].marketDateId).val()) {
                    return confirm(thisFlightSearch.dateSameText)
                }
            }
        }

        thisFlightSearch.searchChangeHandler = function() {
            thisFlightSearch = this;
            var i = 0;
            var market = null;
            var insuranceCheckbox = $('#' + this.insuranceControlId).is(':checked');
            if ($('#' + this.insuranceControlId).length == 0) { insuranceCheckbox = true; }
            if (thisFlightSearch.marketArray != null && insuranceCheckbox == true) {
                if (thisFlightSearch.marketArray.length > 1) {
                    var dateRange = thisFlightSearch.checkDateRange();
                    if (!dateRange) {
                        return false;
                    }
                }
                else {
                    if ($('#' + this.disableInputId).is(':checked') == false) {
                        alert(this.checkboxNotification);
                        return false;
                    }
                }
                for (i = 0; i < thisFlightSearch.marketArray.length; i++) {
                    market = thisFlightSearch.marketArray[i];
                    if (market.isToChange() && market.compareDates() && !confirm(thisFlightSearch.confirmTextArray[i]))
                        return false;
                }
            }
            return thisFlightSearch.checkInfantAge();
        };

        thisFlightSearch.checkInfantAge = function() {
            if (thisFlightSearch.marketArray != null) {
                var returnMarket = thisFlightSearch.marketArray[thisFlightSearch.marketArray.length - 1];
                var departMarket = thisFlightSearch.marketArray[0];
                if (returnMarket.isToChange()) {
                    var returnDate = returnMarket.getSearchDate();
                    var departDate = departMarket.getSearchDate();
                    var infantAge = new SKYSALES.Class.TravelerAge();
                    var i = 0;
                    var birthday = null;

                    for (i = 0; i < thisFlightSearch.infantArray.length; i += 1) {
                        birthday = thisFlightSearch.infantArray[i];
                        if (!infantAge.isInfant(returnDate, birthday, departDate)) {
                            alert(thisFlightSearch.infantAgeText);
                            return false;
                        }
                    }
                }
            }
        };

        thisFlightSearch.checkDateRange = function() {
            var marketDate, marketDateValue, date, disableInput;
            marketDate = new Array();
            marketDateValue = new Array();
            date = new Array();
            disableInput = new Array();
            var dateDeparture = (this.originalDate[0]).split("/");
            var dateArrival = (this.originalDate[1]).split("/");
            //var validateObj = new SKYSALES.Validate(null, '', '', null);

            for (var i = 0; i < thisFlightSearch.marketArray.length; i++) {
                marketDate[i] = this.marketArray[i].marketDateIdArray[0].marketDateId;
                marketDateValue[i] = $('#' + marketDate[i]).val().split("/");
                date[i] = new Date(marketDateValue[i][2], marketDateValue[i][0] - 1, marketDateValue[i][1]);
                disableInput[i] = $('#' + this.disableInputId[i]).is(':checked');
            }
            //Prompt user to check atleast one checkbox
            if (disableInput[0] == false && disableInput[1] == false) {
                alert(this.checkboxNotification);
                return false;
            }
            //set default value for departure date if departure checkbox is unticked
            if (disableInput[0] == false) {
                date[0] = new Date(dateDeparture[2], dateDeparture[0] - 1, dateDeparture[1]);
            }
            //set default value for return date if return checkbox is unticked
            if (disableInput[1] == false) {
                date[1] = new Date(dateArrival[2], dateArrival[0] - 1, dateArrival[1]);
            }
            if (date[1] < date[0]) {
                alert(this.dateRangeError);
                return false;
            }
            //check if same dates
            if (date[1].toString() == date[0].toString()) {
                if (!confirm(this.dateSameText))
                    return false;
            }
            return true;
        }

        thisFlightSearch.checkInfantPax = function() {
            var ddAdultPax = $('#' + thisFlightSearch.dropDownAdtId);
            var ddInfPax = $('#' + thisFlightSearch.dropDownInfId);

            if (ddAdultPax.val() < ddInfPax.val()) {
                alert(thisFlightSearch.tooManyInfantsText);
                return false;
            }

        };

        thisFlightSearch.checkNumPax = function() {
            var ddAdultPax = $('#' + thisFlightSearch.dropDownAdtId);
            var ddChildPax = $('#' + thisFlightSearch.dropDownChdId);
            var totalpaxBookingAssigned = parseInt(ddAdultPax.val()) + parseInt(ddChildPax.val()),
                totalAdultPaxAssigned = parseInt(ddAdultPax.val());

            if (totalAdultPaxAssigned <= 0) {
                alert(thisFlightSearch.noPassengersText);
                return false;
            }
            else if (totalpaxBookingAssigned > thisFlightSearch.maxpaxnum) {
                alert(thisFlightSearch.tooManyPassengersTextPre + thisFlightSearch.maxpaxnum + thisFlightSearch.tooManyPassengersTextPost);
                return false;
            }

        };

        thisFlightSearch.initCountryInputIdArray = function() {
            var i = 0;
            var countryInputId = null;
            var countryInput = {};
            var countryInputIdArray = this.countryInputIdArray || [];
            for (i = 0; i < countryInputIdArray.length; i += 1) {
                countryInputId = countryInputIdArray[i];
                countryInput = new SKYSALES.Class.CountryInput();
                countryInput.init(countryInputId);
                countryInputArray[countryInputArray.length] = countryInput;
            }
        };

        thisFlightSearch.initFlightTypeInputIdArray = function() {
            var i = 0;
            var flightTypeInputId = null;
            var flightTypeInput = {};
            var flightTypeInputIdArray = this.flightTypeInputIdArray || [];
            for (i = 0; i < flightTypeInputIdArray.length; i += 1) {
                flightTypeInputId = flightTypeInputIdArray[i];
                flightTypeInput = new SKYSALES.Class.FlightTypeInput();
                flightTypeInput.flightSearch = this;
                flightTypeInput.index = i;
                flightTypeInput.checkBoxId = flightTypeInputId.checkBoxId;
                flightTypeInput.init(flightTypeInputId);
                flightTypeInputArray[flightTypeInputArray.length] = flightTypeInput;
            }
        };

        thisFlightSearch.populateFlightType = function() {
            var flightTypeIndex = 0;
            var flightType = null;
            for (flightTypeIndex = 0; flightTypeIndex < flightTypeInputArray.length; flightTypeIndex += 1) {
                flightType = flightTypeInputArray[flightTypeIndex];
                if (flightType.input.attr('checked')) {
                    flightType.input.click();
                    if ($('#' + flightType.checkBoxId).length > 0) {
                        $('#' + flightType.checkBoxId).attr('checked', true);
                    }
                    break;
                }
            }
        };

        thisFlightSearch.updateFlightTypeShow = function(activeflightType) {
            var flightTypeIndex = 0;
            var flightType = null;

            for (flightTypeIndex = 0; flightTypeIndex < flightTypeInputArray.length; flightTypeIndex += 1) {
                flightType = flightTypeInputArray[flightTypeIndex];
                flightType.hideInputArray.show();
            }
        };

        thisFlightSearch.updateFlightTypeHide = function(activeflightType) {
            this.updateFlightTypeShow();
            activeflightType.hideInputArray.hide();
        };


        return thisFlightSearch;
    };
    SKYSALES.Class.FlightSearch.createObject = function(json) {
        SKYSALES.Util.createObject('flightSearch', 'FlightSearch', json);
    };
}

/*
Name: 
Class FlightSearchMarket
Param:
None
Return: 
An instance of FlightSearchMarket
Functionality:
The object that initializes the market data for the AvailabilitySearchInput control
Notes:
        
Class Hierarchy:
SkySales -> FlightSearchMarket
*/
if (!SKYSALES.Class.FlightSearchMarket) {
    SKYSALES.Class.FlightSearchMarket = function() {
        var parent = new SKYSALES.Class.SkySales();
        var thisFlightSearchMarket = SKYSALES.Util.extendObject(parent);

        thisFlightSearchMarket.flightSearch = null;
        thisFlightSearchMarket.index = -1;
        //thisFlightSearchMarket.validationInputId = '';
        thisFlightSearchMarket.validationMessageObject = {};

        thisFlightSearchMarket.validationObjectIdArray = [];
        thisFlightSearchMarket.stationInputIdArray = [];
        thisFlightSearchMarket.stationDropDownIdArray = [];
        thisFlightSearchMarket.marketInputIdArray = [];
        thisFlightSearchMarket.macInputIdArray = [];
        thisFlightSearchMarket.marketDateIdArray = [];
        thisFlightSearchMarket.autoMacArray = [];
        thisFlightSearchMarket.disableMarket = false;
        thisFlightSearchMarket.localizedMac = [];

        thisFlightSearchMarket.restrictedStationCategories = [];
        thisFlightSearchMarket.restrictedPairStationCategory = [];

        thisFlightSearchMarket.optionHeaderText = null;
        //var validationInput = null;
        var marketInputArray = [];
        var stationInputArray = [];
        var stationDropDownArray = [];
        var macInputArray = [];
        var marketDateArray = [];

        thisFlightSearchMarket.init = function(json) {
            this.setSettingsByObject(json);
            this.setVars();
            this.addEvents();

            this.initMarketInputIdArray();
            this.initStationInputIdArray();
            this.initStationDropDownIdArray();
            this.initMacInputIdArray();
            this.initMarketDateIdArray();
            this.initValidationObjectRedirect();
        };

        thisFlightSearchMarket.initMacInputIdArray = function() {
            var i = 0;
            var macInputId = null;
            var macInput = {};
            var macInputIdArray = this.macInputIdArray || [];
            for (i = 0; i < macInputIdArray.length; i += 1) {
                macInputId = macInputIdArray[i];
                macInput = new SKYSALES.Class.MacInput();
                macInput.init(macInputId);
                macInput.autoMacArray = thisFlightSearchMarket.autoMacArray;
                macInputArray[macInputArray.length] = macInput;
                macInput.showMac.call(macInput.stationInput);
            }
        };

        thisFlightSearchMarket.initMarketDateIdArray = function() {
            var i = 0;
            var marketDateId = null;
            var marketDate = {};
            var marketDateIdArray = this.marketDateIdArray || [];
            for (i = 0; i < marketDateIdArray.length; i += 1) {
                marketDateId = marketDateIdArray[i];
                marketDate = new SKYSALES.Class.MarketDate();
                marketDate.marketDateCount = this.flightSearch.marketArray.length;
                marketDate.init(marketDateId);
                marketDateArray[marketDateArray.length] = marketDate;
            }
        };

        thisFlightSearchMarket.compareDates = function() {
            return marketDateArray[0].compareDates();
        };

        thisFlightSearchMarket.getSearchDate = function() {
            return marketDateArray[0].marketDate.val();
        };

        thisFlightSearchMarket.initMarketInputIdArray = function() {
            var i = 0;
            var marketInputId = null;
            var marketInput = {};
            var marketInputIdArray = this.marketInputIdArray || [];
            for (i = 0; i < marketInputIdArray.length; i += 1) {
                marketInputId = marketInputIdArray[i];
                marketInput = new SKYSALES.Class.MarketInput();
                marketOriginalId = this.flightSearch.marketArray[0].stationDropDownIdArray[0].inputId;
                marketInput.promoStationsArray = this.flightSearch.promoStationsArray;
                marketInput.optionHeaderText = thisFlightSearchMarket.optionHeaderText;
                marketInput.restrictedStationCategories = this.restrictedStationCategories;
                marketInput.restrictedPairStationCategory = this.restrictedPairStationCategory;
                marketInput.localizedMac = thisFlightSearchMarket.localizedMac;
                marketInput.init(marketInputId);
                marketInputArray[marketInputArray.length] = marketInput;
                if (this.disableMarket)
                    marketInput.disableInput.attr('disabled', true);

            }
        };

        thisFlightSearchMarket.isToChange = function() {
            if (marketInputArray[1].disableInput.attr('checked'))
                return true;
            return false;
        };

        thisFlightSearchMarket.initStationInputIdArray = function() {
            var i = 0;
            var stationInputId = null;
            var stationInput = {};
            var stationInputIdArray = this.stationInputIdArray;
            for (i = 0; i < stationInputIdArray.length; i += 1) {
                stationInputId = stationInputIdArray[i];
                stationInput = new SKYSALES.Class.StationInput();
                stationInput.init(stationInputId);
                stationInputArray[stationInputArray.length] = stationInput;
            }
        };

        thisFlightSearchMarket.initStationDropDownIdArray = function() {
            var i = 0;
            var stationDropDownId = null;
            var stationDropDown = {};
            var stationDropDownIdArray = this.stationDropDownIdArray;
            for (i = 0; i < stationDropDownIdArray.length; i += 1) {
                stationDropDownId = stationDropDownIdArray[i];
                stationDropDown = new SKYSALES.Class.StationDropDown();
                stationDropDown.init(stationDropDownId);
                stationDropDownArray[stationDropDownArray.length] = stationDropDown;
                // this will trigger the update of the destination dropdown list, before
                // the actual value is assigned to the destination dropdown.
                if (i == 0) {
                    stationDropDown.selectBox.change();
                }
            }

        };

        //This function will redirect validation to a new object if the view has
        //been configured to use drop down lists but will tolerate a misconfiguration
        //if the dropdownlist mapping is not removed from the view
        thisFlightSearchMarket.initValidationObjectRedirect = function() {
            //find the metaobject used to validate the first item inthe hash
            var validationObjectIdArray = this.validationObjectIdArray || [];
            var i = 0;
            var validationObjectId = '';
            var key = '';
            var value = '';
            var metaObjectParamToModify = null;
            var dropDownListToValidate = null;
            var metaObjectParam = null;
            for (i = 0; i < validationObjectIdArray.length; i += 1) {
                validationObjectId = validationObjectIdArray[i];
                key = validationObjectId.key || '';
                value = validationObjectId.value || '';
                metaObjectParamToModify = $("object.metaobject>param[@value*='" + key + "']");
                if (metaObjectParamToModify.length > 0) {
                    //before we do the work check to see if the validation target even exists                        
                    dropDownListToValidate = $(":input#" + value);
                    if (dropDownListToValidate.length > 0) {
                        metaObjectParam = metaObjectParamToModify[0];
                        if ('value' in metaObjectParam) {
                            var newStr = metaObjectParam.value;
                            newStr = newStr.replace(key, value);
                            metaObjectParam.value = newStr;
                        }
                    }
                }
            }
        };
        return thisFlightSearchMarket;
    };
}


/*
Name: 
Class FlightStatusSearchContainer
Param:
None
Return: 
An instance of FlightStatusSearchContainer
Functionality:
The object that initializes the FlightFollowing control
Notes:
A FlightStatusSearchContainer object is created every time a div appears in the dom that has a class of flightStatusSearchContainer
<div class="flightStatusSearchContainer" ></div>
There should be one instance of this object for every FlightFollowing control in the view.
Class Hierarchy:
SkySales -> FlightSearchContainer -> FlightStatusSearchContainer
*/
//if (!SKYSALES.Class.FlightStatusSearchContainer)
//{ 
//    SKYSALES.Class.FlightStatusSearchContainer = function ()
//    {   
//        var flightStatusSearchContainer = new SKYSALES.Class.FlightSearchContainer();
//        var thisFlightStatusSearchContainer = SKYSALES.Util.extendObject(flightStatusSearchContainer);
//        return thisFlightStatusSearchContainer;
//    };
//    SKYSALES.Class.FlightStatusSearchContainer.initializeFlightStatusSearchContainers = function ()
//    {
//        var paramObject = {
//            objNameBase: 'flightStatusSearchContainer',
//            objType: 'FlightStatusSearchContainer',
//            selector: 'div.flightStatusSearchContainer'
//        };
//        SKYSALES.Util.initializeNewObject(paramObject);
//    };
//}

/*
Name: 
Class MacInput
Param:
None
Return: 
An instance of MacInput
Functionality:
Handels the functionality of the macs on the AvailabilitySearchInput control
Notes:
This class controls the mac html input controls
Class Hierarchy:
SkySales -> MacInput
*/
if (!SKYSALES.Class.MacInput) {
    SKYSALES.Class.MacInput = function() {
        var macInputBase = new SKYSALES.Class.SkySales();
        var thisMacInput = SKYSALES.Util.extendObject(macInputBase);

        thisMacInput.macHash = SKYSALES.Util.getResource().macHash;
        thisMacInput.stationHash = SKYSALES.Util.getResource().stationHash;
        thisMacInput.stationInputId = '';
        thisMacInput.macContainerId = '';
        thisMacInput.macLabelId = '';
        thisMacInput.macInputId = '';
        thisMacInput.macContainer = {};
        thisMacInput.stationInput = {};
        thisMacInput.macInput = {};
        thisMacInput.macLabel = {};
        thisMacInput.autoMacArray = [];

        thisMacInput.showMac = function() {
            var stationCode = $(this).val();
            stationCode = stationCode || '';
            stationCode = stationCode.toUpperCase();
            var station = null;
            var macCode = '';
            var macText = '';
            var mac = null;

            thisMacInput.macInput.removeAttr('checked');
            thisMacInput.macContainer.hide();
            station = thisMacInput.stationHash[stationCode];
            if (station) {
                macCode = station.macCode;
                isAutoMac = false;

                if (thisMacInput.autoMacArray.length > 0) {
                    for (i = 0; i < thisMacInput.autoMacArray.length; i += 1) {
                        if (thisMacInput.autoMacArray[i].MacStation == macCode) {
                            isAutoMac = true;
                        }
                    }
                }


                mac = thisMacInput.macHash[macCode];
                if ((mac) && (mac.stations.length > 0)) {
                    macText = mac.stations.join();
                    thisMacInput.macLabel.html(macText);

                    if (isAutoMac) {
                        thisMacInput.macInput.attr('checked', true);
                    }
                    else {
                        thisMacInput.macContainer.show();
                    }
                }
            }
        };

        thisMacInput.addEvents = function() {
            thisMacInput.stationInput.change(thisMacInput.showMac);
        };

        thisMacInput.setVars = function() {
            thisMacInput.stationInput = $('#' + thisMacInput.stationInputId);
            thisMacInput.macContainer = $('#' + thisMacInput.macContainerId);
            thisMacInput.macLabel = $('#' + thisMacInput.macLabelId);
            thisMacInput.macInput = $('#' + thisMacInput.macInputId);
        };

        thisMacInput.init = function(paramObject) {
            macInputBase.init.call(this, paramObject);
            thisMacInput.macContainer.hide();
            this.addEvents();
        };
        return thisMacInput;
    };
}

/*
Name: 
Class MarketDate
Param:
None
Return: 
An instance of MarketDate
Functionality:
Handels the functionality of the dates on the AvailabilitySearchInput control
Notes:
The dates also effect the date selection calendar
Class Hierarchy:
SkySales -> MarketDate
*/
if (!SKYSALES.Class.MarketDate) {
    SKYSALES.Class.MarketDate = function() {
        var marketDateBase = new SKYSALES.Class.SkySales();
        var thisMarketDate = SKYSALES.Util.extendObject(marketDateBase);

        thisMarketDate.dateFormat = SKYSALES.datepicker.datePickerFormat;
        thisMarketDate.dateDelimiter = SKYSALES.datepicker.datePickerDelimiter;
        thisMarketDate.marketDateId = '';
        thisMarketDate.marketDate = null;
        thisMarketDate.marketAltDateId = "";
        thisMarketDate.marketDayId = '';
        thisMarketDate.marketDay = null;
        thisMarketDate.marketMonthYearId = '';
        thisMarketDate.marketMonthYear = null;
        thisMarketDate.origDate = null;
        thisMarketDate.marketDateCount = '';
        thisMarketDate.marketDateFormat = '';

        thisMarketDate.setSettingsByObject = function(jsonObject) {
            marketDateBase.setSettingsByObject.call(this, jsonObject);
            var propName = '';
            for (propName in jsonObject) {
                if (thisMarketDate.hasOwnProperty(propName)) {
                    thisMarketDate[propName] = jsonObject[propName];
                }
            }
        };

        thisMarketDate.parseDate = function(dateStr) {
            var day = '';
            var month = '';
            var year = '';
            var date = new Date();
            var dateData = '';
            var formatChar = '';
            var datePickerArray = [];
            var i = 0;
            if (dateStr.indexOf(thisMarketDate.dateDelimiter) > -1) {
                datePickerArray = dateStr.split(thisMarketDate.dateDelimiter);
                for (i = 0; i < thisMarketDate.dateFormat.length; i += 1) {
                    dateData = datePickerArray[i];
                    if (dateData.charAt(0) === '0') {
                        dateData = dateData.substring(1);
                    }
                    formatChar = thisMarketDate.dateFormat.charAt(i);
                    switch (formatChar) {
                    case 'm':
                        month = dateData;
                        break;
                    case 'd':
                        day = dateData;
                        break;
                    case 'y':
                        year = dateData;
                        break;
                    }
                }

                date = new Date(year, month - 1, day);
            }
        return date;
        };

        thisMarketDate.addEvents = function() {
            var datePickerManager = new SKYSALES.Class.DatePickerManager();
            datePickerManager.marketCount = thisMarketDate.marketDateCount;
            datePickerManager.isAOS = false;
            datePickerManager.yearMonth = thisMarketDate.marketMonthYear;
            datePickerManager.day = thisMarketDate.marketDay;
            datePickerManager.linkedDate = thisMarketDate.marketDate;
            datePickerManager.linkedAltDateId = thisMarketDate.marketAltDateId;
            datePickerManager.jsDateFormat = thisMarketDate.marketDateFormat;
            datePickerManager.init();
        };

        thisMarketDate.setVars = function() {
            thisMarketDate.marketDate = $('#' + thisMarketDate.marketDateId);
            thisMarketDate.marketDay = $('#' + thisMarketDate.marketDayId);
            thisMarketDate.marketMonthYear = $('#' + thisMarketDate.marketMonthYearId);
        };

        thisMarketDate.init = function(paramObject) {
            marketDateBase.init.call(this, paramObject);
            this.addEvents();
            thisMarketDate.origDate = thisMarketDate.marketDate.val();

        };

        thisMarketDate.compareDates = function() {
            if (thisMarketDate.origDate != thisMarketDate.marketDate.val())
                return false;
            return true;
        };

        // Checks if the 1st date is before the 2nd date.
        thisMarketDate.datesInOrder = function(dateArray) {
            var retVal = true;
            var dateA = null;
            var dateB = null;

            dateA = this.parseDate(dateArray[0]);
            dateB = this.parseDate(dateArray[1]);

            if (dateA > dateB) {
                retVal = false;
            }
            return retVal;
        };

        thisMarketDate.isOneDayOnly = function(dateArray) {
            var retVal = true;
            var dateA = null;
            var dateB = null;

            dateA = this.parseDate(dateArray[0]);
            dateB = this.parseDate(dateArray[1]);

            if (dateA > dateB || dateA < dateB) {
                retVal = false;
            }
            return retVal;
        };

        return thisMarketDate;
    };
}

/*
Name: 
Class CountryInput
Param:
None
Return: 
An instance of CountryInput
Functionality:
Handels the functionality of the resident country on the AvailabilitySearchInput control
Notes:
The list of countries comes from the resource object
Class Hierarchy:
SkySales -> CountryInput
*/
if (!SKYSALES.Class.CountryInput) {
    SKYSALES.Class.CountryInput = function() {
        var countryInputBase = new SKYSALES.Class.SkySales();
        var thisCountryInput = SKYSALES.Util.extendObject(countryInputBase);

        thisCountryInput.countryInfo = SKYSALES.Util.getResource().countryInfo;
        thisCountryInput.countryInputId = '';
        thisCountryInput.input = {};
        thisCountryInput.defaultCountry = '';
        thisCountryInput.countryArray = [];

        thisCountryInput.populateCountryInput = function() {
            var selectParamObj = {
                'selectBox': thisCountryInput.input,
                'objectArray': thisCountryInput.countryArray,
                'selectedItem': thisCountryInput.defaultCountry,
                'showCode': true
            };
            SKYSALES.Util.populateSelect(selectParamObj);
        };

        thisCountryInput.addEvents = function() {
        };

        thisCountryInput.setVars = function() {
            thisCountryInput.input = $('#' + thisCountryInput.countryInputId);
            var countryInfo = thisCountryInput.countryInfo;
            if (countryInfo) {
                if (countryInfo.CountryList) {
                    thisCountryInput.countryArray = countryInfo.CountryList;
                }
                if (countryInfo.DefaultValue) {
                    thisCountryInput.defaultCountry = countryInfo.DefaultValue;
                }
            }
        };

        thisCountryInput.init = function(paramObject) {
            countryInputBase.init.call(this, paramObject);
            thisCountryInput.populateCountryInput();
            this.addEvents();
        };
        return thisCountryInput;
    };
}

/*
Name: 
Class FlightTypeInput
Param:
None
Return: 
An instance of CountryInput
Functionality:
Handels the functionality of the flight type on the AvailabilitySearchInput control
Notes:
Flight type is the type of flight, as in Round Trip, One Way, or Open Jaw
When you select a flight type the correct html fields are shown.
Class Hierarchy:
SkySales -> FlightTypeInput
*/
if (!SKYSALES.Class.FlightTypeInput) {
    SKYSALES.Class.FlightTypeInput = function() {
        var parent = new SKYSALES.Class.SkySales();
        var thisFlightTypeInput = SKYSALES.Util.extendObject(parent);

        thisFlightTypeInput.flightSearch = null;
        thisFlightTypeInput.index = -1;

        thisFlightTypeInput.flightTypeId = '';
        thisFlightTypeInput.hideInputIdArray = [];
        thisFlightTypeInput.hideInputArray = [];
        thisFlightTypeInput.input = {};

        var hideShowCount = 1;

        thisFlightTypeInput.updateFlightTypeHandler = function() {
            //if (hideShowCount % 2 === 0) {
            //    thisFlightTypeInput.flightSearch.updateFlightTypeShow(thisFlightTypeInput);
            //}
            //else {
            //hideShowCount = 2;
            thisFlightTypeInput.flightSearch.updateFlightTypeHide(thisFlightTypeInput);
            //}
            //hideShowCount += 1;
        };

        thisFlightTypeInput.addEvents = function() {
            parent.addEvents.call(this);
            this.input.click(this.updateFlightTypeHandler);
        };

        thisFlightTypeInput.getById = function(id) {
            var retVal = null;
            if (id) {
                retVal = window.document.getElementById(id);
            }
            return retVal;
        };

        thisFlightTypeInput.setVars = function() {
            parent.setVars.call(this);

            var hideInputIndex = 0;
            var hideInput = null;
            var hideInputArray = [];

            thisFlightTypeInput.input = $('#' + this.flightTypeId);
            for (hideInputIndex = 0; hideInputIndex < this.hideInputIdArray.length; hideInputIndex += 1) {
                hideInput = thisFlightTypeInput.getById(this.hideInputIdArray[hideInputIndex]);
                if (hideInput) {
                    hideInputArray[hideInputArray.length] = hideInput;
                }
            }
            thisFlightTypeInput.hideInputArray = $(hideInputArray);
        };

        thisFlightTypeInput.init = function(json) {
            this.setSettingsByObject(json);
            this.setVars();
            this.addEvents();
        };
        return thisFlightTypeInput;
    };
}

/*
Name: 
Class MarketInput
Param:
None
Return: 
An instance of MarketInput
Functionality:
Handels the functionality of the markets on the AvailabilitySearchInput control
Notes:
Markets are a link between stations. 
When you select an orgin station only the valid destionation stations should be available for selection.
Class Hierarchy:
SkySales -> MarketInput
*/
if (!SKYSALES.Class.MarketInput) {
    SKYSALES.Class.MarketInput = function() {
        var parent = new SKYSALES.Class.SkySales();
        var thisMarketInput = SKYSALES.Util.extendObject(parent);

        thisMarketInput.marketHash = SKYSALES.Util.getResource().marketHash;
        thisMarketInput.stationHash = SKYSALES.Util.getResource().stationHash;
        thisMarketInput.countryStationHash = SKYSALES.Util.getResource().countryStationHash;

        thisMarketInput.optionHeaderText = null;

        thisMarketInput.containerId = '';
        thisMarketInput.container = null;
        thisMarketInput.containerTextId = '';
        thisMarketInput.containerText = null;
        //added by Bryan, 06 Jul 2012
        thisMarketInput.row1Id = '';
        thisMarketInput.row2Id = '';
        thisMarketInput.row3Id = '';
        
        thisMarketInput.disableInputId = '';
        thisMarketInput.disableInput = null;
        thisMarketInput.originId = '';
        thisMarketInput.origin = null;
        thisMarketInput.destinationId = '';
        thisMarketInput.destination = null;
        thisMarketInput.toggleMarketCount = 0;
        thisMarketInput.marketOriginalId = '';
        thisMarketInput.promoStationsArray = [];
        thisMarketInput.localizedMac = [];

        thisMarketInput.initializeMacStationNames = function() {
            if (this.localizedMac) {
                for (mac in this.localizedMac) {
                    if (this.stationHash[this.localizedMac[mac].StationCode]) {
                        this.stationHash[this.localizedMac[mac].StationCode].name = this.localizedMac[mac].StationName;
                    }
                }
            }
        };

        thisMarketInput.restrictedStationCategories = [];
        thisMarketInput.restrictedPairStationCategory = [];

//Edited by Bryan, 06 Jul 2012
        thisMarketInput.toggleMarket = function() {
            if ((thisMarketInput.toggleMarketCount % 2) === 0) {
                $(thisMarketInput.container).hide();
//                $(thisMarketInput.containerText).show();
//                $(thisMarketInput.row1Id).show();
                $(thisMarketInput.row2Id).show();
                $(thisMarketInput.row3Id).show();
            }
            else {
                $(thisMarketInput.container).show();
//                $(thisMarketInput.containerText).hide();
//                $(thisMarketInput.row1Id).hide();
                $(thisMarketInput.row2Id).hide();
                $(thisMarketInput.row3Id).hide();
            }
            thisMarketInput.toggleMarketCount += 1;
        };

        thisMarketInput.useComboBox = function(input) {
            var retVal = true;
            if (input && input.get(0) && input.get(0).options) {
                retVal = false;
            }
            return retVal;
        };

        thisMarketInput.updateMarketOrigin = function() {
            var origin = $(this).val();
            if (origin == "") {
                var origin = $('#' + marketOriginalId).val();
            }
            origin = origin.toUpperCase();
            var destinationArray = thisMarketInput.marketHash[origin];
            destinationArray = destinationArray || [];
            var selectParamObj = null;
            var useCombo = true;
            var originObject = thisMarketInput.stationHash[origin];

            useCombo = thisMarketInput.useComboBox(thisMarketInput.destination);
            if (useCombo) {
                selectParamObj = {
                    'input': thisMarketInput.destination,
                    'options': destinationArray,
                    'optionHeaderText': thisMarketInput.optionHeaderText
                };
                SKYSALES.Class.DropDown.getDropDown(selectParamObj);
            }
            else {

                var myPromoStationsArray = [];
                if (thisMarketInput.promoStationsArray && thisMarketInput.promoStationsArray.length > 0) {
                    for (stationArrayIndex in thisMarketInput.promoStationsArray) {
                        if (origin == thisMarketInput.promoStationsArray[stationArrayIndex].originStation) {
                            myPromoStationsArray = thisMarketInput.promoStationsArray[stationArrayIndex].destStationArray;
                            break;
                        }
                    }
                }

                selectParamObj = {
                    'selectBox': thisMarketInput.destination,
                    'promoStationsArray': myPromoStationsArray,
                    'objectArray': destinationArray,
                    'groupArray': thisMarketInput.countryStationHash,
                    'showCode': true,
                    'optionHeaderText': thisMarketInput.optionHeaderText,
                    'restrictedStationCategory': thisMarketInput.restrictedStationCategories,
                    'restrictedPairStationCategory': thisMarketInput.restrictedPairStationCategory,
                    'originObject': originObject
                };
                SKYSALES.Util.populateSelectWithGroups(selectParamObj);
                //SKYSALES.Util.populateSelect(selectParamObj);
            }
        };

        thisMarketInput.addEvents = function() {
            parent.addEvents.call(this);
            thisMarketInput.origin.change(thisMarketInput.updateMarketOrigin);
            thisMarketInput.disableInput.click(thisMarketInput.toggleMarket);
        };

        thisMarketInput.setVars = function() {
            parent.setVars.call(this);
            thisMarketInput.container = $('#' + thisMarketInput.containerId);
            thisMarketInput.containerText = $('#' + thisMarketInput.containerTextId);
            //added by Bryan, 06 Jul 2012
            thisMarketInput.row1Id = $('#' + thisMarketInput.row1Id);
            thisMarketInput.row2Id = $('#' + thisMarketInput.row2Id);
            thisMarketInput.row3Id = $('#' + thisMarketInput.row3Id);
            
            thisMarketInput.disableInput = $('#' + thisMarketInput.disableInputId);
            thisMarketInput.origin = $('#' + thisMarketInput.originId);
            thisMarketInput.destination = $('#' + thisMarketInput.destinationId);
        };

        thisMarketInput.populateMarketInput = function(input) {
            //            var stationHash = this.FilterStationHash(thisMarketInput.stationHash);
            var useCombo = true;
            var selectParamObj = {};
            if ((input) && (input.length > 0)) {
                useCombo = thisMarketInput.useComboBox(input);
                if (useCombo) {
                    selectParamObj = {
                        'input': input,
                        'options': this.stationHash,
                        'optionHeaderText': thisMarketInput.optionHeaderText,
                        'restrictedStationCategory': this.restrictedStationCategories
                    };
                    SKYSALES.Class.DropDown.getDropDown(selectParamObj);
                }
                else {

                    var myPromoStationsArray = [];
                    if (thisMarketInput.promoStationsArray && thisMarketInput.promoStationsArray.length > 0) {
                        for (stationArrayIndex in thisMarketInput.promoStationsArray) {
                            myPromoStationsArray.push(thisMarketInput.promoStationsArray[stationArrayIndex].originStation);
                        }
                    }

                    selectParamObj = {
                        'selectBox': input,
                        'promoStationsArray': myPromoStationsArray,
                        'objectArray': this.stationHash,
                        'groupArray': thisMarketInput.countryStationHash,
                        'showCode': true,
                        'optionHeaderText': thisMarketInput.optionHeaderText,
                        'restrictedStationCategory': this.restrictedStationCategories
                    };
                    SKYSALES.Util.populateSelectWithGroups(selectParamObj);
                    //SKYSALES.Util.populateSelect(selectParamObj);
                }
            }
        };

        thisMarketInput.init = function(paramObject) {
            parent.init.call(this, paramObject);
            this.addEvents();

            thisMarketInput.initializeMacStationNames();

            thisMarketInput.populateMarketInput(thisMarketInput.origin);
            thisMarketInput.populateMarketInput(thisMarketInput.destination);

            thisMarketInput.disableInput.click();
            thisMarketInput.disableInput.removeAttr('checked');

        };
        return thisMarketInput;
    };
}


/*
Name: 
Class StationInput
Param:
None
Return: 
An instance of StationInput
Functionality:
Handels the functionality of the stations on the AvailabilitySearchInput control
Notes:
StationInput is the html form element where you type the orgin or destination station
Class Hierarchy:
SkySales -> StationInput
*/
if (!SKYSALES.Class.StationInput) {
    SKYSALES.Class.StationInput = function() {
        var parent = new SKYSALES.Class.SkySales();
        var thisStationInput = SKYSALES.Util.extendObject(parent);

        thisStationInput.stationInputId = '';
        thisStationInput.stationInput = null;

        thisStationInput.setVars = function() {
            parent.setVars.call(this);
            thisStationInput.stationInput = $('#' + this.stationInputId);
        };

        thisStationInput.init = function(json) {
            parent.init.call(this, json);
            this.addEvents();
        };
        return thisStationInput;
    };
}

/*
Name: 
Class StationDropDown
Param:
None
Return: 
An instance of StationDropDown
Functionality:
Handels the functionality of the stations on the AvailabilitySearchInput control
Notes:
StationDropDown is the html form element where you select the orgin or destination station
Class Hierarchy:
SkySales -> StationDropDown
*/
if (!SKYSALES.Class.StationDropDown) {
    SKYSALES.Class.StationDropDown = function() {
        var stationDropDownBase = new SKYSALES.Class.SkySales();
        var thisStationDropDown = SKYSALES.Util.extendObject(stationDropDownBase);

        thisStationDropDown.selectBoxId = '';
        thisStationDropDown.selectBox = null;
        thisStationDropDown.inputId = '';
        thisStationDropDown.input = null;

        thisStationDropDown.updateStationDropDown = function() {
            var stationCode = $(this).val();
            if (stationCode != "") {
                try { thisStationDropDown.selectBox.val(stationCode) } catch (e) { };
            }
            else {
                // assign hidden textbox value for IE6
                $(this).val(this.defaultValue);
            }
        };

        thisStationDropDown.updateStationInput = function() {
            var stationCode = $(this).val();
            thisStationDropDown.input.val(stationCode);
            thisStationDropDown.input.change();
        };

        thisStationDropDown.addEvents = function() {
            thisStationDropDown.input.change(thisStationDropDown.updateStationDropDown);
            thisStationDropDown.selectBox.change(thisStationDropDown.updateStationInput);
        };

        thisStationDropDown.setVars = function() {
            thisStationDropDown.selectBox = $('#' + thisStationDropDown.selectBoxId);
            thisStationDropDown.input = $('#' + thisStationDropDown.inputId);
        };

        thisStationDropDown.init = function(paramObject) {
            stationDropDownBase.init.call(this, paramObject);
            this.addEvents();
            thisStationDropDown.input.change();
        };
        return thisStationDropDown;
    };
}

/*global $ SKYSALES window */

/*
Name: 
TravelDocumentInput
Param:
None
Return: 
An instance of TravelDocumentInput
Functionality:
The object that sets the travel document information of the TravelDocumentInput control for registration
Class Hierarchy:
TravelDocumentInput
*/
if (!SKYSALES.Class.TravelDocumentInput) {
    SKYSALES.Class.TravelDocumentInput = function() {
        var parent = new SKYSALES.Class.SkySales();
        var thisTravelDocumentInput = SKYSALES.Util.extendObject(parent);

        thisTravelDocumentInput.instanceName = '';
        thisTravelDocumentInput.delimiter = '_';

        thisTravelDocumentInput.travelDocumentInfoId = '';
        thisTravelDocumentInput.travelDocumentInfo = null;
        thisTravelDocumentInput.documentNumberId = '';
        thisTravelDocumentInput.documentNumber = null;
        thisTravelDocumentInput.documentTypeId = '';
        thisTravelDocumentInput.documentType = null;
        thisTravelDocumentInput.documentIssuingCountryId = '';
        thisTravelDocumentInput.documentIssuingCountry = null;
        thisTravelDocumentInput.documentExpYearId = '';
        thisTravelDocumentInput.documentExpYear = null;
        thisTravelDocumentInput.documentExpMonthId = '';
        thisTravelDocumentInput.documentExpMonth = null;
        thisTravelDocumentInput.documentExpDayId = '';
        thisTravelDocumentInput.documentExpDay = null;
        thisTravelDocumentInput.actionId = '';
        thisTravelDocumentInput.action = null;
        thisTravelDocumentInput.travelDocumentKey = '';

        thisTravelDocumentInput.missingDocumentText = '';
        thisTravelDocumentInput.missingDocumentTypeText = '';
        thisTravelDocumentInput.invalidExpDateText = '';
        thisTravelDocumentInput.emptyExpDateText = '';
        thisTravelDocumentInput.invalidDaysOfMonthTextPre = '';
        thisTravelDocumentInput.invalidDaysOfMonthTextMid = '';
        thisTravelDocumentInput.invalidDaysOfMonthTextPost = '';
        thisTravelDocumentInput.missingDocumentNumberText = '';
        thisTravelDocumentInput.missingDocumentCountryText = '';

        thisTravelDocumentInput.init = function(json) {
            this.setSettingsByObject(json);
            this.setVars();
            this.addEvents();
        };

        thisTravelDocumentInput.setVars = function() {
            thisTravelDocumentInput.travelDocumentInfo = this.getById(this.travelDocumentInfoId);
            thisTravelDocumentInput.documentType = this.getById(this.documentTypeId);
            thisTravelDocumentInput.documentNumber = this.getById(this.documentNumberId);
            thisTravelDocumentInput.documentIssuingCountry = this.getById(this.documentIssuingCountryId);
            thisTravelDocumentInput.documentExpYear = this.getById(this.documentExpYearId);
            thisTravelDocumentInput.documentExpMonth = this.getById(this.documentExpMonthId);
            thisTravelDocumentInput.documentExpDay = this.getById(this.documentExpDayId);
            thisTravelDocumentInput.action = this.getById(this.actionId);
        };

        thisTravelDocumentInput.setTravelDocumentInfo = function() {
            var travelDocumentKey = '';
            var documentType = this.documentType.val();
            var documentNumber = this.documentNumber.val();
            var documentIssuingCountry = this.documentIssuingCountry.val();

            if (documentType && documentNumber && documentIssuingCountry) {
                //travelDocumentKey should always start with the delimiter.
                //travelDocumentKey format is as follows: _<DOC TYPE>_<DOC NUMBER>_<ISSUING COUNTRY>
                travelDocumentKey = this.delimiter + documentType + this.delimiter + documentNumber + this.delimiter + documentIssuingCountry;
                this.travelDocumentInfo.val(travelDocumentKey);
            }
            return true;
        };

        thisTravelDocumentInput.validateTravelDocumentHandler = function() {
            var result = thisTravelDocumentInput.validateTravelDocument();
            return result;
        };

        thisTravelDocumentInput.validateTravelDocument = function() {
            this.setTravelDocumentInfo();
            var action = this.action.get(0);
            var result = window.validate(action) && this.validateInput();
            return result;
        };

        thisTravelDocumentInput.addEvents = function() {
            this.action.click(this.validateTravelDocumentHandler);
        };

        thisTravelDocumentInput.validateInput = function() {
            var retVal = true;
            var msg = '';
            var invalidDateMsg = '';
            var documentNumberValue = this.documentNumber.val() || '';
            var documentExpYearValue = this.documentExpYear.val() || '';
            var documentExpMonthValue = this.documentExpMonth.val() || '';
            var documentExpDayValue = this.documentExpDay.val() || '';
            var documentTypeValue = this.documentType.val() || '';
            var documentIssuingCountryValue = this.documentIssuingCountry.val() || '';
            var isPassedDate = false;
            var isValidDate = false;
            var documentExpMonthText = '';

            if (documentNumberValue || documentTypeValue || documentIssuingCountryValue || documentExpYearValue || documentExpMonthValue || documentExpDayValue) {
                if (!documentNumberValue) {
                    msg = msg + this.missingDocumentNumberText + "\n";
                }
                if (!documentTypeValue) {
                    msg = msg + this.missingDocumentTypeText + "\n";
                }
                if (!documentIssuingCountryValue) {
                    msg = msg + this.missingDocumentCountryText + "\n";
                }

                isValidDate = this.checkDaysOfMonth(documentExpDayValue, documentExpMonthValue, documentExpYearValue);
                isPassedDate = this.isPastDate(documentExpDayValue, documentExpMonthValue, documentExpYearValue);
                if (documentExpDayValue && documentExpMonthValue && documentExpYearValue) {
                    if (!isValidDate) {
                        documentExpMonthText = this.documentExpMonth.find(':selected').text();
                        invalidDateMsg = this.invalidDaysOfMonthTextPre + documentExpDayValue;
                        invalidDateMsg += this.invalidDaysOfMonthTextMid + documentExpMonthText + this.invalidDaysOfMonthTextPost;
                        msg = msg + invalidDateMsg + "\n";
                    }
                    else if (!isPassedDate) {
                        msg = msg + this.invalidExpDateText + "\n";
                    }
                }
                else {
                    msg = msg + this.emptyExpDateText + "\n";
                }

                if (msg) {
                    window.alert(this.missingDocumentText + "\n" + msg);
                    retVal = false;
                }
            }
            return retVal;
        };

        thisTravelDocumentInput.checkDaysOfMonth = function(day, month, year) {
            year = window.parseInt(year, 10);
            month = window.parseInt(month, 10);
            day = window.parseInt(day, 10);
            var retVal = false;
            var lastDayInFeb = null;
            var daysInFeb = -1;
            var daysInMonth = null;
            if (year && month && day) {
                month -= 1;
                lastDayInFeb = new Date();
                lastDayInFeb.setMonth(2);
                lastDayInFeb.setDate(1);
                lastDayInFeb.setDate(lastDayInFeb.getDate() - 1);
                daysInFeb = lastDayInFeb.getDate();
                daysInMonth = [31, daysInFeb, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];
                if (day <= daysInMonth[month]) {
                    retVal = true;
                }
            }
            return retVal;
        };

        thisTravelDocumentInput.isPastDate = function(day, month, year) {
            year = window.parseInt(year, 10);
            month = window.parseInt(month, 10);
            day = window.parseInt(day, 10);
            var retVal = false;
            var today = null;
            var compareDate = null;
            if (year && month && day) {
                month -= 1;
                today = new Date();
                compareDate = new Date(year, month, day);
                if (compareDate > today) {
                    retVal = true;
                }
            }
            return retVal;
        };

        return thisTravelDocumentInput;
    };
}

/*
Name: 
Class ControlGroup
Param:
None
Return: 
An instance of ControlGroup
Functionality:
Handels a ControlGroup validation
Notes:
        
Class Hierarchy:
SkySales -> ControlGroup
*/
if (!SKYSALES.Class.ControlGroup) {
    SKYSALES.Class.ControlGroup = function() {
        var parent = new SKYSALES.Class.SkySales();
        var thisControlGroup = SKYSALES.Util.extendObject(parent);

        thisControlGroup.actionId = 'SkySales';
        thisControlGroup.action = null;

        thisControlGroup.init = function(json) {
            this.setSettingsByObject(json);
            this.setVars();
            this.addEvents();
        };
        thisControlGroup.setVars = function() {
            parent.setVars.call(this);
            thisControlGroup.action = $('#' + this.actionId);
        };
        thisControlGroup.addEvents = function() {
            parent.addEvents.call(this);
            this.action.click(this.validateHandler);
        };
        thisControlGroup.validateHandler = function() {
            var retVal = thisControlGroup.validate();
            return retVal;
        };
        thisControlGroup.validate = function() {
            var actionDom = this.action.get(0);
            var retVal = window.validate(actionDom);
            return retVal;
        };
        return thisControlGroup;
    };
    SKYSALES.Class.ControlGroup.createObject = function(json) {
        SKYSALES.Util.createObject('controlGroup', 'ControlGroup', json);
    };
}

/*
Name: 
Class ControlGroupRegister
Param:
None
Return: 
An instance of ControlGroupRegister
Functionality:
Handels a ControlGroupRegister validation
Notes:
        
Class Hierarchy:
SkySales -> ControlGroupRegister
*/
if (!SKYSALES.Class.ControlGroupRegister) {
    SKYSALES.Class.ControlGroupRegister = function() {
        var parent = new SKYSALES.Class.ControlGroup();
        var thisControlGroupRegister = SKYSALES.Util.extendObject(parent);

        thisControlGroupRegister.travelDocumentInput = null;

        thisControlGroupRegister.setSettingsByObject = function(json) {
            parent.setSettingsByObject.call(this, json);
            var travelDocumentInput = new SKYSALES.Class.TravelDocumentInput();
            travelDocumentInput.init(this.travelDocumentInput);
            thisControlGroupRegister.travelDocumentInput = travelDocumentInput;
        };
        thisControlGroupRegister.validateHandler = function() {
            var retVal = thisControlGroupRegister.validate();
            return retVal;
        };
        thisControlGroupRegister.validate = function() {
            var retVal = false;
            retVal = (this.travelDocumentInput.setTravelDocumentInfo() && this.travelDocumentInput.validateExpDate());
            if (retVal) {
                retVal = parent.validate.call(this);
            }
            return retVal;
        };
        return thisControlGroupRegister;
    };
    SKYSALES.Class.ControlGroupRegister.createObject = function(json) {
        SKYSALES.Util.createObject('controlGroupRegister', 'ControlGroupRegister', json);
    };
}

/*
Name: 
Class ContactInput
Param:
None
Return: 
An instance of ContactInput
Functionality:
Handles the ContactInput control
Notes:
Auto populates from the clientHash cookie
when you enter in a name that matches the one on the cookie.
Class Hierarchy:
SkySales -> ContactInput
*/
if (!SKYSALES.Class.ContactInput) {
    SKYSALES.Class.ContactInput = function() {
        var contactInput = new SKYSALES.Class.SkySales();
        var thisContactInput = SKYSALES.Util.extendObject(contactInput);

        thisContactInput.clientId = '';
        thisContactInput.keyIdArray = [];
        thisContactInput.keyArray = [];
        thisContactInput.clientStoreIdHash = null;
        thisContactInput.countryInputId = '';
        thisContactInput.countryInput = null;
        thisContactInput.stateInputId = '';
        thisContactInput.stateInput = null;
        thisContactInput.countryStateHash = null;
        thisContactInput.imContactId = '';
        thisContactInput.imContact = null;
        thisContactInput.currentContactData = {};
        thisContactInput.logOutButton = null;
        thisContactInput.homePhone = null;
        thisContactInput.workPhone = null;
        thisContactInput.otherPhone = null;
        thisContactInput.faxPhone = null;
        thisContactInput.homePhoneId = '';
        thisContactInput.workPhoneId = '';
        thisContactInput.otherPhoneId = '';
        thisContactInput.faxPhoneId = '';
        thisContactInput.dropdownTitleId = "";
        thisContactInput.dropdownGenderId = "";
        thisContactInput.dropdownTitle = null;
        thisContactInput.dropdownGender = null;
        thisContactInput.selectCountryText = '';
        thisContactInput.selectStateText = '';
        thisContactInput.provinceStateCode = '';
        thisContactInput.stateArray = [];

        thisContactInput.clientHash = SKYSALES.Util.getResource().clientHash;

        thisContactInput.setSettingsByObject = function(jsonObject) {
            contactInput.setSettingsByObject.call(this, jsonObject);
            var propName = '';
            for (propName in jsonObject) {
                if (thisContactInput.hasOwnProperty(propName)) {
                    thisContactInput[propName] = jsonObject[propName];
                }
            }
        };

        thisContactInput.clearCurrentContact = function() {
            // there are fields with default value/s
            $("#" + thisContactInput.clientId + "_DropDownListTitle").val("");

            //do not clear the name
            //$("#" + thisContactInput.clientId + "_TextBoxFirstName").val("first");
            //$("#" + thisContactInput.clientId + "_TextBoxMiddleName").val("");
            //$("#" + thisContactInput.clientId + "_TextBoxLastName").val("last");

            $("#" + thisContactInput.clientId + "_TextBoxAddressLine1").val("");
            $("#" + thisContactInput.clientId + "_TextBoxAddressLine2").val("");
            $("#" + thisContactInput.clientId + "_TextBoxAddressLine3").val("");
            $("#" + thisContactInput.clientId + "_TextBoxCity").val("");
            $("#" + thisContactInput.clientId + "_DropDownListStateProvince").val("");
            $("#" + thisContactInput.clientId + "_DropDownListCountry").val(thisContactInput.selectCountryText);
            $("#" + thisContactInput.clientId + "_TextBoxPostalCode").val("");
            $("#" + thisContactInput.clientId + "_TextBoxHomePhone").val("");
            $("#" + thisContactInput.clientId + "_TextBoxWorkPhone").val("");
            $("#" + thisContactInput.clientId + "_TextBoxOtherPhone").val("");
            $("#" + thisContactInput.clientId + "_TextBoxFax").val("");
            $("#" + thisContactInput.clientId + "_TextBoxEmailAddress").val("");
        };

        thisContactInput.populateCurrentContact = function() {
            if (thisContactInput.currentContactData) {
                if (thisContactInput.imContact.attr("checked") === true) {

                    $("#" + thisContactInput.clientId + "_DropDownListTitle").val(thisContactInput.currentContactData.title);

                    if (thisContactInput.currentContactData.firstName != "") {
                        $("#" + thisContactInput.clientId + "_TextBoxFirstName").val(thisContactInput.currentContactData.firstName);
                    }
                    else {
                        $("#" + thisContactInput.clientId + "_TextBoxFirstName").val(thisContactInput.currentContactData.lastName);
                    }

                    $("#" + thisContactInput.clientId + "_TextBoxMiddleName").val(thisContactInput.currentContactData.middleName);
                    $("#" + thisContactInput.clientId + "_TextBoxLastName").val(thisContactInput.currentContactData.lastName);
                    $("#" + thisContactInput.clientId + "_TextBoxAddressLine1").val(thisContactInput.currentContactData.streetAddressOne);
                    $("#" + thisContactInput.clientId + "_TextBoxAddressLine2").val(thisContactInput.currentContactData.streetAddressTwo);
                    $("#" + thisContactInput.clientId + "_TextBoxAddressLine3").val(thisContactInput.currentContactData.streetAddressThree);
                    $("#" + thisContactInput.clientId + "_TextBoxCity").val(thisContactInput.currentContactData.city);
                    $("#" + thisContactInput.clientId + "_DropDownListCountry").val(thisContactInput.currentContactData.country);

                    thisContactInput.updateState();

                    $("#" + thisContactInput.clientId + "_DropDownListStateProvince").val(thisContactInput.currentContactData.stateProvince);
                    $("#" + thisContactInput.clientId + "_TextBoxPostalCode").val(thisContactInput.currentContactData.postalCode);
                    $("#" + thisContactInput.clientId + "_TextBoxHomePhone").val(thisContactInput.currentContactData.eveningPhone);
                    $("#" + thisContactInput.clientId + "_TextBoxWorkPhone").val(thisContactInput.currentContactData.dayPhone);
                    $("#" + thisContactInput.clientId + "_TextBoxOtherPhone").val(thisContactInput.currentContactData.mobilePhone);
                    $("#" + thisContactInput.clientId + "_TextBoxFax").val(thisContactInput.currentContactData.faxPhone);
                    $("#" + thisContactInput.clientId + "_TextBoxEmailAddress").val(thisContactInput.currentContactData.email);
                }
                else {
                    thisContactInput.clearCurrentContact();
                }
            }
        };

        thisContactInput.populateCountrySelect = function() {
            var country = "";
            if ($(thisContactInput.imContact).is(':checked') ||
            (!$(thisContactInput.imContact).is(':visible') &&
            $('#' + thisContactInput.countryInputId + '_value').val() != ""))
                country = $('#' + thisContactInput.countryInputId + '_value').val();
            else
                country = $('#' + thisContactInput.countryInputId).val();

            var countryList = SKYSALES.Util.getResource().countryInfo.CountryList;

            countryList.unshift({ InternationalDialCode: '', code: '', name: thisContactInput.selectCountryText });

            var selectParamObj = {
                'selectBox': $('#' + thisContactInput.countryInputId),
                'objectArray': countryList,
                'selectedItem': country,
                'showCode': false
            };

            SKYSALES.Util.populateSelect(selectParamObj);
        }

        thisContactInput.updateCountry = function() {
            if (thisContactInput.stateArray.length == 0) {
                var stateList = SKYSALES.Util.getResource().stateInfo.StateList;
                var countryState = thisContactInput.stateInput.val();

                for (stateIndex in stateList) {
                    if (stateList[stateIndex].ProvinceStateCode == countryState) {
                        thisContactInput.countryInput.val(stateList[stateIndex].CountryCode);
                        break;
                    }
                }
            }
        };

        thisContactInput.updateStateAndPhoneNumbers = function() {
            thisContactInput.updatePhoneNumbers();
            thisContactInput.updateState();
        };

        thisContactInput.updatePhoneNumbers = function() {
            var selectedCountryCode = thisContactInput.countryInput.val();
            var countryList = SKYSALES.Util.getResource().countryInfo.CountryList;

            for (i = 0; i < countryList.length; i += 1) {
                country = countryList[i];

                if (selectedCountryCode == country.code) {
                    thisContactInput.homePhone.val(country.InternationalDialCode);
                    thisContactInput.workPhone.val(country.InternationalDialCode);
                    thisContactInput.otherPhone.val(country.InternationalDialCode);
                    thisContactInput.faxPhone.val(country.InternationalDialCode);

                    break;
                }
            }
        };

        thisContactInput.updateState = function() {
            var stateList = SKYSALES.Util.getResource().stateInfo.StateList;
            var country = thisContactInput.countryInput.val();
            var stateArray = [];
            var stateObject = {};
            var stateObjectArray = [];

            if (country !== '') {
                var i = 0;

                for (stateIndex in stateList) {
                    if (stateList[stateIndex].CountryCode == country) {
                        stateArray.push(stateList[stateIndex]);
                    }
                }

                stateArray = stateArray || [];
                thisContactInput.stateArray = stateArray;
                if (stateArray.length === 0) {
                    stateArray = stateList;
                }
            }
            if (stateArray.length == 0 || stateArray[0].code != '') {
                stateArray.unshift({ InternationalDialCode: '', code: '', name: thisContactInput.selectStateText || 'Select State' });
            }

            var paramObject = {
                'objectArray': stateArray,
                'selectBox': thisContactInput.stateInput,
                'showCode': false,
                'clearOptions': true
            };
            SKYSALES.Util.populateSelect(paramObject);
        };

        thisContactInput.getKey = function() {
            var i = 0;
            var keyArray = thisContactInput.keyArray;
            var keyObject = null;
            var key = '';
            for (i = 0; i < keyArray.length; i += 1) {
                keyObject = keyArray[i];
                key += keyObject.val();
            }
            key = key.replace(/\s+/g, '+');
            key = thisContactInput.clientId + '_' + (key.toLowerCase());
            return key;
        };

        thisContactInput.populateClientStoreIdHash = function() {
            var clientHash = thisContactInput.clientHash;
            var i = 0;
            var keyValueStr = '';
            var keyValueArray = [];
            var singleKeyValueStr = '';
            var eqIndex = -1;
            var key = thisContactInput.getKey();

            var value = null;
            thisContactInput.clientStoreIdHash = {};
            if (key && clientHash && clientHash[key]) {
                thisContactInput.clientStoreIdHash = thisContactInput.clientStoreIdHash || {};
                keyValueStr = clientHash[key];
                keyValueArray = keyValueStr.split('&');
                for (i = 0; i < keyValueArray.length; i += 1) {
                    singleKeyValueStr = keyValueArray[i];
                    eqIndex = singleKeyValueStr.indexOf('=');
                    if (eqIndex > -1) {
                        key = singleKeyValueStr.substring(0, eqIndex);
                        value = singleKeyValueStr.substring(eqIndex + 1, singleKeyValueStr.length);
                        if (key) {
                            thisContactInput.clientStoreIdHash[key] = value;
                        }
                    }
                }
            }
        };

        thisContactInput.autoPopulateForm = function() {
            thisContactInput.populateClientStoreIdHash();
            var clientStoreIdHash = thisContactInput.clientStoreIdHash;
            var key = '';
            var value = '';
            for (key in clientStoreIdHash) {
                if (clientStoreIdHash.hasOwnProperty(key)) {

                    value = clientStoreIdHash[key];
                    $('#' + key).val(value);

                }
            }
            //filter states by country
            thisContactInput.initializeState();

        };

        thisContactInput.initializeState = function() {
            if ($('#' + thisContactInput.countryInputId + ' option').length < 1) {
                thisContactInput.populateCountrySelect();
            }
            if (this.countryInput.val() !== '') {
                var stateValue = "";
                var countryStateArray = [];

                if ($(thisContactInput.imContact).is(':checked') ||
                (!$(thisContactInput.imContact).is(':visible') &&
                $('#' + thisContactInput.stateInputId + '_value').val() != "") &&
                $('#' + thisContactInput.stateInputId + '_value').val() != undefined) {
                    stateValue = thisContactInput.provinceStateCode || $('#' + thisContactInput.stateInputId + '_value').val() || '';
                }
                else {
                    stateValue = thisContactInput.provinceStateCode || $('#' + thisContactInput.stateInputId).val() || '';

                    if (stateValue == '' && thisContactInput.clientStoreIdHash) {
                        stateValue = thisContactInput.clientStoreIdHash[thisContactInput.stateInputId];
                    }
                }
                if (stateValue) {
                    countryStateArray = stateValue.split('|');

                    if (countryStateArray.length == 2) {
                        stateValue = countryStateArray[1];
                    }
                }

                thisContactInput.updateState();

                if (stateValue != '') {
                    thisContactInput.stateInput.val(stateValue);
                }
            }
            else {
                thisContactInput.updateState();
            }
        };

        thisContactInput.updateGender = function() {
            var selectedTitle = thisContactInput.dropdownTitle.val();
            var titleList = SKYSALES.Util.getResource().titleInfo.TitleList;

            for (i = 0; i < titleList.length; i += 1) {
                title = titleList[i];

                if (selectedTitle == title.TitleKey) {
                    thisContactInput.dropdownGender.val(title.GenderCode);

                    break;
                }
            }
        };

        thisContactInput.updateTitle = function() {
            var selectedGender = thisContactInput.dropdownGender.val();
            var selectedTitle = thisContactInput.dropdownTitle.val();
            var titleList = SKYSALES.Util.getResource().titleInfo.TitleList;

            for (i = 0; i < titleList.length; i += 1) {
                title = titleList[i];

                if (selectedTitle != "CHD" &&
                    selectedGender == title.GenderCode) {
                    thisContactInput.dropdownTitle.val(title.TitleKey);

                    break;
                }
            }
        };

        thisContactInput.addEvents = function() {
            contactInput.addEvents.call(this);
            var i = 0;
            var keyArray = thisContactInput.keyArray;
            var key = null;
            for (i = 0; i < keyArray.length; i += 1) {
                key = keyArray[i];
                key.change(thisContactInput.autoPopulateForm);
            }
            thisContactInput.countryInput.change(thisContactInput.updateStateAndPhoneNumbers);
            thisContactInput.stateInput.change(thisContactInput.updateCountry);
            thisContactInput.imContact.click(thisContactInput.populateCurrentContact);
            thisContactInput.logOutButton.click(thisContactInput.clearCurrentContact);
            thisContactInput.dropdownTitle.change(this.updateGender);
            thisContactInput.dropdownGender.change(this.updateTitle);
        };

        thisContactInput.setVars = function() {
            contactInput.setVars.call(this);
            var i = 0;
            var keyIdArray = thisContactInput.keyIdArray;
            var keyArray = thisContactInput.keyArray;
            var keyId = '';
            for (i = 0; i < keyIdArray.length; i += 1) {
                keyId = keyIdArray[i];
                keyArray[keyArray.length] = $('#' + keyId);
            }
            thisContactInput.countryInput = $('#' + thisContactInput.countryInputId);
            thisContactInput.stateInput = $('#' + thisContactInput.stateInputId);
            thisContactInput.imContact = $('#' + thisContactInput.imContactId);
            thisContactInput.logOutButton = $('#MemberLoginContactView_ButtonLogOut');
            thisContactInput.homePhone = $("#" + thisContactInput.homePhoneId);
            thisContactInput.workPhone = $("#" + thisContactInput.workPhoneId);
            thisContactInput.otherPhone = $("#" + thisContactInput.otherPhoneId);
            thisContactInput.faxPhone = $("#" + thisContactInput.faxPhoneId);
            thisContactInput.dropdownTitle = $('#' + this.dropdownTitleId);
            thisContactInput.dropdownGender = $('#' + this.dropdownGenderId);

            if ($("#" + thisContactInput.clientId + "_TextBoxFirstName").val() == "") {
                $("#" + thisContactInput.clientId + "_TextBoxFirstName").val(thisContactInput.currentContactData.lastName);
            }
        };

        thisContactInput.init = function(paramObj) {
            this.setSettingsByObject(paramObj);
            this.setVars();
            this.addEvents();

            this.initializeState();
        };
        return thisContactInput;
    };
    SKYSALES.Class.ContactInput.createObject = function(json) {
        SKYSALES.Util.createObject('contactInput', 'ContactInput', json);
    };
}

/*
Name: 
Class ToggleView
Param:
None
Return: 
An instance of ToggleView
Functionality:
The ToggleView class is used to show and hide dom elements.
Notes:
It is set up so that you can click different elements to show and hide the dom object.
You can have a link that you click to show the element, and anothe that you click to hide it.
showId is the id of the html element that you click to show the element
hideId is the id of the html element that you click to hide the element
elementId is the id of the element you are showing and hiding
Class Hierarchy:
SkySales -> ToggleView
*/
if (!SKYSALES.Class.ToggleView) {
    SKYSALES.Class.ToggleView = function() {
        var toggleView = new SKYSALES.Class.SkySales();
        var thisToggleView = SKYSALES.Util.extendObject(toggleView);

        thisToggleView.showId = '';
        thisToggleView.hideId = '';
        thisToggleView.elementId = '';

        thisToggleView.show = null;
        thisToggleView.hide = null;
        thisToggleView.element = null;

        thisToggleView.setVars = function() {
            toggleView.setVars.call(this);
            thisToggleView.show = $('#' + thisToggleView.showId);
            thisToggleView.hide = $('#' + thisToggleView.hideId);
            thisToggleView.element = $('#' + thisToggleView.elementId);
        };

        thisToggleView.init = function(paramObj) {
            this.setSettingsByObject(paramObj);
            this.setVars();
            this.addEvents();
        };

        thisToggleView.updateShowHandler = function() {
            thisToggleView.element.show('slow');
        };

        thisToggleView.updateHideHandler = function() {
            thisToggleView.element.hide();
        };

        thisToggleView.addEvents = function() {
            toggleView.addEvents.call(this);
            thisToggleView.show.click(thisToggleView.updateShowHandler);
            thisToggleView.hide.click(thisToggleView.updateHideHandler);
        };
        return thisToggleView;
    };
}


/*
Name: 
Class PaymentInput
Param:
None
Return: 
An instance of PaymentInput
Functionality:
This class represents a PaymentInput
Notes:
Class Hierarchy:
SkySales -> PaymentInput
*/
if (!SKYSALES.Class.PaymentInputContainer) {
    SKYSALES.Class.PaymentInputContainer = function() {
        var parent = SKYSALES.Class.SkySales();
        thisPaymentInputContainer = SKYSALES.Util.extendObject(parent);

        thisPaymentInputContainer.labelConvenienceFeeId = '';
        thisPaymentInputContainer.voucherNumberTextBoxId = '';
        thisPaymentInputContainer.voucherNumberTextBox = '';
        thisPaymentInputContainer.labelConvenienceFee = null;
        thisPaymentInputContainer.validBinRange = '';
        thisPaymentInputContainer.invalidBinRangeMsg = '';
        thisPaymentInputContainer.validBigCardBinRange = '';
        thisPaymentInputContainer.invalidBigCardBinRangeMsg = '';
        thisPaymentInputContainer.buttonSubmitProxyId = 'ButtonSubmitProxy';
        thisPaymentInputContainer.buttonSubmitProxy = null;
        thisPaymentInputContainer.buttonSubmitId = 'CONTROLGROUPPAYMENTBOTTOM_ButtonSubmit';

        thisPaymentInputContainer.defaultCreditCardsArray = null;
        thisPaymentInputContainer.dropDownListPaymentMethodId = '';
        thisPaymentInputContainer.dropDownListPaymentMethod = null;
        thisPaymentInputContainer.registeredCCArray = null;

        thisPaymentInputContainer.ccPaymentFeesArray = null;
        thisPaymentInputContainer.ccPaymentControlName = '';
        thisPaymentInputContainer.ccPaymentFeeLabel = '';
        thisPaymentInputContainer.ccPaymentFeePaxLabel = '';
        thisPaymentInputContainer.ccPaymentFeeWayLabel = '';
        thisPaymentInputContainer.ccPaymentFeeSegmentCount = '';
        thisPaymentInputContainer.ccPaymentFeePaxCount = '';

        //Bryan, 21 May 2012, bypass CC Checking
        thisPaymentInputContainer.PaymentCount = '';
        thisPaymentInputContainer.PNR = '';
        
        thisPaymentInputContainer.selectedCreditCard = '';

        thisPaymentInputContainer.dropDownMCCId = '';
        thisPaymentInputContainer.buttonApplyMCCId = '';
        thisPaymentInputContainer.hiddenUpdatedMCCId = '';
        thisPaymentInputContainer.dropDownMCC = null;
        thisPaymentInputContainer.buttonApplyMCC = null;
        thisPaymentInputContainer.hiddenUpdatedMCC = null;

        thisPaymentInputContainer.upSellPrompt = '';
        thisPaymentInputContainer.mccNoticePrompt = '';
        thisPaymentInputContainer.mccCurrencyCode = '';
        thisPaymentInputContainer.mccEnabled = '';
        thisPaymentInputContainer.issuingCountrySelectId = '';
        thisPaymentInputContainer.countryCurrencyHash = {};

        thisPaymentInputContainer.isModifiedBooking = '';
        thisPaymentInputContainer.manageMyBookingPmtAlert = '';
        thisPaymentInputContainer.WAKObjDefault = '';
        thisPaymentInputContainer.WAKGuest = '';

        var selectedBillingContent = 0;

        thisPaymentInputContainer.setSettingsByObject = function(json) {
            parent.setSettingsByObject.call(this, json);
            if (thisPaymentInputContainer.WAKObjDefault != '') {
                for (i = 0; i < eval(thisPaymentInputContainer.WAKGuest); i++) {
                    $("#CONTROLGROUPPAYMENTBOTTOM_PaymentInputViewPaymentView_objectNoDropdownList" + i + " option[text=" + thisPaymentInputContainer.WAKObjDefault + "]").attr("selected", "selected");
                }
            }

        };

        thisPaymentInputContainer.setVars = function() {
            thisPaymentInputContainer.labelConvenienceFee = this.getById(this.labelConvenienceFeeId);
            thisPaymentInputContainer.voucherNumberTextBox = this.getById(this.voucherNumberTextBoxId);
            thisPaymentInputContainer.buttonSubmitProxy = this.getById(this.buttonSubmitProxyId);
            thisPaymentInputContainer.dropDownListPaymentMethod = this.getById(this.dropDownListPaymentMethodId);
            thisPaymentInputContainer.dropDownMCC = this.getById(this.dropDownMCCId);
            thisPaymentInputContainer.buttonApplyMCC = this.getById(this.buttonApplyMCCId);
            thisPaymentInputContainer.hiddenUpdatedMCC = this.getById(this.hiddenUpdatedMCCId);
        };

        thisPaymentInputContainer.previewConvenienceFeeDivHandler = function() {
            $('#convenienceFeePreview').show();
        };

        thisPaymentInputContainer.hideConvenienceFeeDivHandler = function() {
            $('#convenienceFeePreview').hide();
        };

        thisPaymentInputContainer.addEvents = function() {
            this.labelConvenienceFee.hover(thisPaymentInputContainer.previewConvenienceFeeDivHandler, thisPaymentInputContainer.hideConvenienceFeeDivHandler);
            this.buttonSubmitProxy.click(thisPaymentInputContainer.validateCC);
            this.dropDownListPaymentMethod.change(thisPaymentInputContainer.showSelectedCC);
            $('#' + this.buttonSubmitId).click(this.prompterHandler);

            $("input:radio[name='" + this.ccPaymentControlName + "']").click(
            function() {
                var selectedPayment = $(this).val();
                thisPaymentInputContainer.selectCC(selectedPayment);
            }
            );
            
            if (thisPaymentInputContainer.hiddenUpdatedMCC)
                thisPaymentInputContainer.hiddenUpdatedMCC.val('');
            if (this.hiddenUpdatedMCC.length > 0)
                $('#CONTROLGROUPPAYMENTBOTTOM_MultiCurrencyConversionViewPaymentView_ButtonApplyMCC').click(thisPaymentInputContainer.updateCurrency);
        };

        thisPaymentInputContainer.prompterHandler = function() {
            return thisPaymentInputContainer.showCurrencyPrompter();
        };

        thisPaymentInputContainer.showCurrencyPrompter = function() {
            if (this.defaultCreditCardsArray.length > 0) {
                var activeCC = this.defaultCreditCardsArray[this.getActivePayment()],
                    countrySelected = $('#' + activeCC.issuingCountrySelectId).val() || '',
                    activeBilling = this.defaultCreditCardsArray[this.getActivePayment()].billingContentId,
                    contentCC = this.defaultCreditCardsArray[this.getActivePayment()].contentId,
                    virtualNo = $("input:radio[name='" + this.ccPaymentControlName + "']:checked").val(),
                    countryCurrencySelected = this.countryCurrencyHash[countrySelected] || null;

                if (virtualNo) {
                    result = validateBySelector("td[id='" + this.selectedCreditCard + "_CVV_" + virtualNo + "']") && validateBySelector("div[id='" + activeBilling + "']");
                }
                else {
                    result = validateBySelector("div[id='" + contentCC + "']") || validateBySelector("div[id='" + activeBilling + "']");
                }

                if (result && this.mccEnabled === 'true' && this.selectedCreditCard === '') {
                    if (this.mccCurrencyCode != '') {
                        return confirm(this.mccNoticePrompt) ? SKYSALES.DisplayLoadingBar() : false;  //added by Linson at 2012-01-05
                    }
                    else if (countryCurrencySelected != null && activeCC.currencyCode != countryCurrencySelected.currencyCode) {
                         return confirm(this.upSellPrompt) ? SKYSALES.DisplayLoadingBar() : false;  //added by Linson at 2012-01-05
                }
                }
            }
            SKYSALES.DisplayLoadingBar(); //added by Linson at 2012-01-05
            return true;
        };

        thisPaymentInputContainer.updateCurrency = function() {
            var updatedMCC = thisPaymentInputContainer.dropDownMCC.val();
            thisPaymentInputContainer.hiddenUpdatedMCC.val(updatedMCC);
        };

        thisPaymentInputContainer.init = function(json) {
            this.setSettingsByObject(json);
            this.setVars();
            this.addEvents();

            if (this.voucherNumberTextBox != null) {
                this.voucherNumberTextBox.val("");
            }

            var i = 0;
            var defaultCreditCardsArray = this.defaultCreditCardsArray || [];
            var paymentInput = null;
            for (i = 0; i < defaultCreditCardsArray.length; i += 1) {
                paymentInput = new SKYSALES.Class.PaymentInput();
                paymentInput.init(defaultCreditCardsArray[i]);

                this.defaultCreditCardsArray[i] = paymentInput;
                paymentInput.hideContent();
            }

            if (this.defaultCreditCardsArray.length > 0) {
                var activeDiv = this.getActivePayment();
                this.defaultCreditCardsArray[activeDiv].showContent();
                this.displayConvenienceFee(this.defaultCreditCardsArray[activeDiv].ccId);
            }

            var countryList = SKYSALES.Util.getResource().countryInfo.CountryList;
            for (i = 0; i < countryList.length; i = i + 1) {
                this.countryCurrencyHash[countryList[i].code] = { "currencyCode": countryList[i].currency };
            }

            //by default, choose the first option (be sure to only select the enabled)
            var ccRadio = $("input:radio[name='" + thisPaymentInputContainer.ccPaymentControlName + "']:enabled");
            if (ccRadio.length > 0) {
                if (ccRadio.length > 1) {
                    thisPaymentInputContainer.selectedCreditCard = thisPaymentInputContainer.registeredCCArray[0].brand;
                    thisPaymentInputContainer.displayConvenienceFee(thisPaymentInputContainer.selectedCreditCard);
                    $('#' + thisPaymentInputContainer.registeredCCArray[0].CVV).removeAttr("disabled");
                    thisPaymentInputContainer.showBillingContent(thisPaymentInputContainer.registeredCCArray[0].brand);
                    thisPaymentInputContainer.hideDefaultCCs();
                }

                else if (ccRadio.length == 1) {
                    thisPaymentInputContainer.showDefaultCCs();
                    thisPaymentInputContainer.displayConvenienceFee(thisPaymentInputContainer.defaultCreditCardsArray[thisPaymentInputContainer.getActivePayment()].ccId);
                    thisPaymentInputContainer.showBillingContent(thisPaymentInputContainer.defaultCreditCardsArray[thisPaymentInputContainer.getActivePayment()].ccId);
                }
                ccRadio[0].checked = true;
            }
        };


        thisPaymentInputContainer.getActivePayment = function() {
            var i = 0;
            var activeIndex = 0;

            for (i = 0; i < thisPaymentInputContainer.defaultCreditCardsArray.length; i++) {
                if (thisPaymentInputContainer.defaultCreditCardsArray[i].contentId == "content_" + thisPaymentInputContainer.dropDownListPaymentMethod.val()) {
                    activeIndex = i;
                    break;
                }
            }

            return activeIndex;
        };

        //        show selected default CC
        thisPaymentInputContainer.showSelectedCC = function() {
            var i = 0;
            var activeIndex = thisPaymentInputContainer.getActivePayment();
            for (i = 0; i < thisPaymentInputContainer.defaultCreditCardsArray.length; i++) {
                thisPaymentInputContainer.defaultCreditCardsArray[i].hideContent();
            }

            thisPaymentInputContainer.defaultCreditCardsArray[activeIndex].showContent();
            thisPaymentInputContainer.displayConvenienceFee(thisPaymentInputContainer.defaultCreditCardsArray[activeIndex].ccId);
        };

        thisPaymentInputContainer.hideDefaultCCs = function() {
            thisPaymentInputContainer.defaultCreditCardsArray[thisPaymentInputContainer.getActivePayment()].clearCCForm();
            $('#' + thisPaymentInputContainer.registeredCCArray[thisPaymentInputContainer.registeredCCArray.length - 1].brand + "_div").hide();
        };

        thisPaymentInputContainer.showDefaultCCs = function() {
            $('#' + thisPaymentInputContainer.registeredCCArray[thisPaymentInputContainer.registeredCCArray.length - 1].brand + "_div").show();
        };

        thisPaymentInputContainer.validateCC = function() {
            $.post('SessionRefresh.aspx'); // additional code to refresh the session
            var validCC = true;
            //if user chose default cc
            if (thisPaymentInputContainer.selectedCreditCard == '' && thisPaymentInputContainer.defaultCreditCardsArray.length > 0) {
                var activeCC = thisPaymentInputContainer.defaultCreditCardsArray[thisPaymentInputContainer.getActivePayment()];

                var ccNumber = $('#' + activeCC.accountNumberInputId).val();

                var validBigCard = true;
                if (activeCC.contentId == 'content_ExternalAccount:BG') {
                    var validBigCard = false;
                    var bigCardBinRangeArray = thisPaymentInputContainer.validBigCardBinRange.split(',');
                    var bigCardBinRanges = bigCardBinRangeArray.length;
                    for (i = 0; i < bigCardBinRanges; i++) {
                        var bigCardBinRangeItem = bigCardBinRangeArray[i];
                        if (bigCardBinRangeItem == ccNumber.substring(0, bigCardBinRangeItem.length)) {
                            validBigCard = true;
                            break;
                        }
                    }
                    if (validBigCard == false && ccNumber.length > 0) {
                        alert(thisPaymentInputContainer.invalidBigCardBinRangeMsg);
                        validCC = false;
                    }
                }

                if (validBigCard == true) {
                    if (thisPaymentInputContainer.validBinRange != '' && ccNumber != '') {
                        var validCCNumber = false;
                        var binRangeArray = thisPaymentInputContainer.validBinRange.split(',');
                        var binRanges = binRangeArray.length;

                        // Bryan, 21 May 2012, bypass CCCheck
                        if (thisPaymentInputContainer.PNR != "" && thisPaymentInputContainer.PaymentCount > 0) {
                            validCCNumber = true;
                        }
                        for (i = 0; i < binRanges; i++) {
                            var binRangeItem = binRangeArray[i];
                            if (binRangeItem == ccNumber.substring(0, binRangeItem.length)) {
                                validCCNumber = true;
                                break;
                            }
                        }
                        if (validCCNumber == false) {
                            alert(thisPaymentInputContainer.invalidBinRangeMsg);
                            validCC = false;
                        }
                    }
                }
                validCC = validCC && validateBySelector("div[id='" + activeCC.contentId + "']")
                    && validateBySelector("div[id='" + activeCC.billingContentId + "']");
            }

            else if (thisPaymentInputContainer.selectedCreditCard != '') {
                var activeBilling = thisPaymentInputContainer.defaultCreditCardsArray[selectedBillingContent].billingContentId;
                var virtualNo = $("input:radio[name='" + thisPaymentInputContainer.ccPaymentControlName + "']:checked").val();
                validCC = validateBySelector("td[id='" + thisPaymentInputContainer.selectedCreditCard + "_CVV_" + virtualNo + "']") &&
				validateBySelector("div[id='" + activeBilling + "']");
            }
            else if (!thisPaymentInputContainer.defaultCreditCardsArray.length > 0) {
                var submitAction = ($('#' + thisPaymentInputContainer.buttonSubmitId)).get(0);
                validCC = validate(submitAction);
            }

            if (validCC == true) {
                if (thisPaymentInputContainer.isModifiedBooking == "True") {
                    alert(thisPaymentInputContainer.manageMyBookingPmtAlert);
                }
                document.getElementById(thisPaymentInputContainer.buttonSubmitId).click();
            }

            return false;
        }

        //show selected registered CC
        thisPaymentInputContainer.selectCC = function(selectedCC) {
            thisPaymentInputContainer.selectedCreditCard = '';
            var cvvField = null;
            var selectedIndex = 0;
            var i = 0;
            for (i = 0; i < thisPaymentInputContainer.registeredCCArray.length; i++) {
                cvvField = $('#' + thisPaymentInputContainer.registeredCCArray[i].CVV);
                if (cvvField != null && cvvField.length > 0) {
                    cvvField.val('');
                    if (thisPaymentInputContainer.registeredCCArray[i].virtualNo == selectedCC) {
                        selecteddIndex = i;
                        thisPaymentInputContainer.selectedCreditCard = thisPaymentInputContainer.registeredCCArray[i].brand;
                    }
                    cvvField.attr("disabled", "disabled");
                    cvvField.removeClass("validationError");
                }
            }

            if (thisPaymentInputContainer.selectedCreditCard != '') {
                thisPaymentInputContainer.hideDefaultCCs();
                cvvField = $('#' + thisPaymentInputContainer.registeredCCArray[selecteddIndex].CVV);
                cvvField.removeAttr("disabled");
                cvvField.select();
                thisPaymentInputContainer.displayConvenienceFee(thisPaymentInputContainer.selectedCreditCard);
                thisPaymentInputContainer.showBillingContent(thisPaymentInputContainer.registeredCCArray[selecteddIndex].brand);
            }
            else {
                thisPaymentInputContainer.showDefaultCCs();
                thisPaymentInputContainer.displayConvenienceFee(thisPaymentInputContainer.defaultCreditCardsArray[thisPaymentInputContainer.getActivePayment()].ccId);
                thisPaymentInputContainer.showBillingContent(thisPaymentInputContainer.defaultCreditCardsArray[thisPaymentInputContainer.getActivePayment()].ccId);
            }
        };

        thisPaymentInputContainer.displayConvenienceFee = function(ccBrand) {
            var i = 0;
            var hasConvenienceFee = false;
            var convPreview = thisPaymentInputContainer.ccPaymentFeeLabel;
            for (i = 0; i < thisPaymentInputContainer.ccPaymentFeesArray.length; i++) {
                if (ccBrand == thisPaymentInputContainer.ccPaymentFeesArray[i].brand) {
                    hasConvenienceFee = true;
                    $("#paymentFee_Amount").text(thisPaymentInputContainer.ccPaymentFeesArray[i].convenienceFee);
                    $("#totalWithPaymentFee_Amount").text(thisPaymentInputContainer.ccPaymentFeesArray[i].paymentTotal);
                    convPreview = convPreview.concat(thisPaymentInputContainer.ccPaymentFeesArray[i].feeBase, ' x ', thisPaymentInputContainer.ccPaymentFeeSegmentCount, thisPaymentInputContainer.ccPaymentFeeWayLabel, ' x ', thisPaymentInputContainer.ccPaymentFeePaxCount, thisPaymentInputContainer.ccPaymentFeePaxLabel, thisPaymentInputContainer.ccPaymentFeesArray[i].convenienceFee);
                    $("#convenienceFeePreview").text(convPreview);
                }
            }

            if (hasConvenienceFee == false) {
                $("#convenienceFee_Display").hide();
            }
            else {
                $("#convenienceFee_Display").show();
            }
        };

        thisPaymentInputContainer.showBillingContent = function(ccBrand) {
            var i = 0;
            selectedBillingContent = 0;
            for (i = 0; i < thisPaymentInputContainer.defaultCreditCardsArray.length; i++) {
                thisPaymentInputContainer.defaultCreditCardsArray[i].billingContent.addClass('hidden');
                if (ccBrand == thisPaymentInputContainer.defaultCreditCardsArray[i].ccId) {
                    selectedBillingContent = i;
                }
            }

            //show the appropriate billing div
            thisPaymentInputContainer.defaultCreditCardsArray[selectedBillingContent].billingContent.removeClass('hidden');
        };

        return thisPaymentInputContainer;

    };

    SKYSALES.Class.PaymentInputContainer.createObject = function(json) {
        SKYSALES.Util.createObject('paymentInputContainer', 'PaymentInputContainer', json);
    };
}

if (!SKYSALES.Class.PaymentInput) {
    SKYSALES.Class.PaymentInput = function() {
        var parent = SKYSALES.Class.SkySales(),
        thisPaymentInput = SKYSALES.Util.extendObject(parent);

        thisPaymentInput.dccOfferInfoId = '';
        thisPaymentInput.foreignAmountId = '';
        thisPaymentInput.foreignCurrencyId = '';
        thisPaymentInput.foreignCurrencySymbolId = '';
        thisPaymentInput.ownCurrencyAmountId = '';
        thisPaymentInput.ownCurrencyId = '';
        thisPaymentInput.ownCurrencySymbolId = '';
        thisPaymentInput.rejectRadioBtnIdId = '';
        thisPaymentInput.acceptRadioBtnIdId = '';
        thisPaymentInput.doubleOptOutId = '';
        thisPaymentInput.inlineDCCAjaxSucceededId = '';
        thisPaymentInput.dccId = '';
        thisPaymentInput.inlineDCCConversionLabelId = '';
        thisPaymentInput.amountInputId = '';
        thisPaymentInput.accountNumberInputId = '';
        thisPaymentInput.inlineDCCOffer = null;
        thisPaymentInput.currencyCode = null;
        thisPaymentInput.feeAmt = null;
        thisPaymentInput.issuingCountryTextBoxId = '';
        thisPaymentInput.issuingCountrySelectId = '';
        thisPaymentInput.billingCountryTextBoxId = '';
        thisPaymentInput.billingCountrySelectId = '';
        thisPaymentInput.billingStateTextBoxId = '';
        thisPaymentInput.billingStateSelectId = '';
        thisPaymentInput.billingStateSelectBoxInput = '';
        thisPaymentInput.billingStateTextBoxInput = '';
        thisPaymentInput.billingCountryInput = '';
        thisPaymentInput.billingCountryTextBox = null;
        thisPaymentInput.voucherNumberTextBoxId = '';
        thisPaymentInput.voucherNumberTextBox = '';

        thisPaymentInput.stateInfo = SKYSALES.Util.getResource().stateInfo;
        thisPaymentInput.contentId = '';
        thisPaymentInput.content = null;
        thisPaymentInput.billingContentId = '';
        thisPaymentInput.billingContent = null;
        thisPaymentInput.cvvTextBoxId = '';
        thisPaymentInput.cvvTextBox = null;
        thisPaymentInput.acctHolderNameTextBoxId = '';
        thisPaymentInput.acctHolderNameTextBox = null;
        thisPaymentInput.bankNameTextBoxId = '';
        thisPaymentInput.bankNameTextBox = null;
        thisPaymentInput.ccId = '';

        thisPaymentInput.setSettingsByObject = function(json) {
            parent.setSettingsByObject.call(this, json);
        };

        thisPaymentInput.setVars = function() {
            thisPaymentInput.dcc = $('#' + this.dccId);
            thisPaymentInput.inlineDCCConversionLabel = $('#' + this.inlineDCCConversionLabelId);
            thisPaymentInput.accountNoTextBox = $('#' + this.accountNumberInputId);
            thisPaymentInput.amountTextBox = $('#' + this.amountInputId);
            thisPaymentInput.inlineDCCAjaxSucceeded = $('#' + this.inlineDCCAjaxSucceededId);
            thisPaymentInput.billingStateSelectBoxInput = this.getById(this.billingStateSelectId);
            thisPaymentInput.billingStateTextBoxInput = this.getById(this.billingStateTextBoxId);
            thisPaymentInput.billingCountryInput = this.getById(this.billingCountrySelectId);
            thisPaymentInput.content = this.getById(this.contentId);
            thisPaymentInput.billingContent = this.getById(this.billingContentId);
            thisPaymentInput.billingCountryTextBox = this.getById(this.billingCountryTextBoxId);
            thisPaymentInput.cvvTextBox = $('#' + this.cvvTextBoxId);
            thisPaymentInput.acctHolderNameTextBox = $('#' + this.acctHolderNameTextBoxId);
            thisPaymentInput.bankNameTextBox = $('#' + this.bankNameTextBoxId);

        };

        thisPaymentInput.hideTextBoxRef = function() {
            thisPaymentInput.billingStateTextBoxInput.addClass('hidden');
            thisPaymentInput.billingCountryTextBox.addClass('hidden');
        };

        thisPaymentInput.hideContent = function() {
            this.content.addClass('hidden');
            this.clearCCForm();
            this.billingContent.addClass('hidden');
        };

        thisPaymentInput.clearCCForm = function() {
            this.cvvTextBox.val('');
            this.accountNoTextBox.val('');
            this.acctHolderNameTextBox.val('');
            this.bankNameTextBox.val('');
            $('#' + this.issuingCountryTextBoxId).val('');
            $('#' + this.issuingCountrySelectId).val('');
        };

        thisPaymentInput.showContent = function() {
            this.content.removeClass('hidden');
            this.billingContent.removeClass('hidden');
        };

        thisPaymentInput.inlineDCCAjaxRequestHandler = function() {
            thisPaymentInput.getInlineDCC();
        };

        thisPaymentInput.addEvents = function() {
            this.amountTextBox.change(this.inlineDCCAjaxRequestHandler);
            this.accountNoTextBox.change(this.inlineDCCAjaxRequestHandler);
            this.billingCountryInput.change(this.updateState);
            this.billingStateSelectBoxInput.change(this.updateStateTextBoxValue);
        };

        thisPaymentInput.init = function(json) {
            this.setSettingsByObject(json);
            this.setVars();
            this.addEvents();
            this.hideTextBoxRef();
            this.initDropDowns();
            this.billingStateSelectBoxInput.val(this.billingStateTextBoxInput.val());

        };


        thisPaymentInput.initDropDowns = function() {
            if (thisPaymentInput.issuingCountryTextBoxId !== "") {
                var paramObj = {
                    'TextBoxId': thisPaymentInput.issuingCountryTextBoxId,
                    'SelectBoxId': thisPaymentInput.issuingCountrySelectId
                };

                countryDropDown = new SKYSALES.Class.CountryDropDown();
                countryDropDown.init(paramObj);
            }

            if (thisPaymentInput.billingCountryTextBoxId !== "") {
                var paramObj = {
                    'TextBoxId': thisPaymentInput.billingCountryTextBoxId,
                    'SelectBoxId': thisPaymentInput.billingCountrySelectId
                };

                countryDropDown = new SKYSALES.Class.CountryDropDown();
                countryDropDown.init(paramObj);
            }

            thisPaymentInput.initStateDropDown();
        };

        thisPaymentInput.updateState = function() {
            thisPaymentInput.initStateDropDown();
            thisPaymentInput.billingStateTextBoxInput.val('');
        };

        thisPaymentInput.initStateDropDown = function() {
            var countryCode = thisPaymentInput.billingCountryInput.val();
            var stateArray = thisPaymentInput.stateInfo.StateList;
            var selectBox = thisPaymentInput.billingStateSelectBoxInput.get(0);

            if (!(selectBox === undefined)) {
                selectBox.options.length = 1;

                for (i = 0; i < stateArray.length; i += 1) {
                    state = stateArray[i];

                    if (countryCode == state.CountryCode) {
                        selectBox.options[selectBox.options.length] = new window.Option(state.Name, state.ProvinceStateCode, false, false);
                    }
                }
            }
        };

        thisPaymentInput.updateStateTextBoxValue = function() {
            var codeValue = $(this).val();
            thisPaymentInput.billingStateTextBoxInput.val(codeValue);
        };

        thisPaymentInput.getInlineDCC = function(amount, acctNumber) {
            var params = {};
            if ('True' === this.inlineDCCOffer) {
                if (!acctNumber) {
                    //get the account number
                    acctNumber = this.accountNoTextBox.val();
                }
                if (!amount) {
                    //get the amount
                    amount = this.amountTextBox.val();
                }
                params = { 'amount': amount,
                    'paymentFee': this.feeAmt,
                    'currencyCode': this.currencyCode,
                    'accountNumber': acctNumber
                };
                if (this.currencyCode && amount && acctNumber && (0 < parseFloat(amount)) && (12 <= acctNumber.length)) {
                    this.inlineDCCAjaxSucceeded.val('false');
                    $.get('DCCOfferAjax-Resource.aspx', params, this.inlineDCCResponseHandler);
                }
            }
        };

        thisPaymentInput.setVarsAfterAjaxResponse = function(jData) {
            var offerInfo = $('#' + this.dccOfferInfoId, jData);
            thisPaymentInput.foreignAmount = $('#' + this.foreignAmountId, offerInfo).text();
            thisPaymentInput.foreignCurrency = $('#' + this.foreignCurrencyId, offerInfo).text();
            thisPaymentInput.foreignCurrencySymbol = $('#' + this.foreignCurrencySymbolId, offerInfo).text();
            thisPaymentInput.ownCurrencyAmount = $('#' + this.ownCurrencyAmountId, offerInfo).text();
            thisPaymentInput.ownCurrency = $('#' + this.ownCurrencyId, offerInfo).text();
            thisPaymentInput.ownCurrencySymbol = $('#' + this.ownCurrencySymbolId, offerInfo).text();
            thisPaymentInput.acceptRadioBtnID = $('#' + this.acceptRadioBtnIdId, offerInfo).text();
            thisPaymentInput.rejectRadioBtnID = $('#' + this.rejectRadioBtnIdId, offerInfo).text();
            thisPaymentInput.acceptRadioBtn = $('#' + this.acceptRadioBtnID);
            thisPaymentInput.doubleOptOut = $('#' + this.doubleOptOutId, offerInfo).text();
            thisPaymentInput.radioButtonInlineDccStatusOfferAccept = $('#' + this.acceptRadioBtnID);
            thisPaymentInput.radioButtonInlineDccStatusOfferReject = $('#' + this.rejectRadioBtnID);
        };

        thisPaymentInput.foreignUpdateConversionLabel = function() {
            this.inlineDCCConversionLabel.text('(' + ' ' + this.foreignAmount + ' ' + this.foreignCurrency + ')');
        };

        thisPaymentInput.ownUpdateConversionLabel = function() {
            this.inlineDCCConversionLabel.text('');
        };

        thisPaymentInput.noThanks = function() {
            $('#dccCont').show('slow');
        };

        thisPaymentInput.noShowThanks = function() {
            $('#dccCont').hide('slow');
        };

        thisPaymentInput.inlineDccStatusOfferAccept = function() {
            this.foreignUpdateConversionLabel();
            this.noShowThanks();
        };

        thisPaymentInput.inlineDccStatusOfferReject = function() {
            this.ownUpdateConversionLabel();
            this.noThanks();
        };

        thisPaymentInput.inlineDccStatusOfferAcceptHandler = function() {
            thisPaymentInput.inlineDccStatusOfferAccept();
        };

        thisPaymentInput.inlineDccStatusOfferRejectHandler = function() {
            thisPaymentInput.inlineDccStatusOfferReject();
        };

        thisPaymentInput.addEventsAfterAjaxResponse = function() {
            this.radioButtonInlineDccStatusOfferAccept.click(this.inlineDccStatusOfferAcceptHandler);
            this.radioButtonInlineDccStatusOfferReject.click(this.inlineDccStatusOfferRejectHandler);
        };

        thisPaymentInput.updateAcceptRadioBtn = function() {
            var acceptChecked = this.acceptRadioBtn.attr('checked');
            if (acceptChecked) {
                this.foreignUpdateConversionLabel();
            }
        };

        thisPaymentInput.updateInlineDCCOffer = function(data) {
            this.inlineDCCAjaxSucceeded.val('true');
            var responseDCCElement = null;
            if (data) {
                this.dcc.empty();
                var jData = $(data);
                responseDCCElement = $('#' + this.dccId, jData);
                if (responseDCCElement && responseDCCElement.length) {
                    this.dcc.prepend(responseDCCElement.children());
                }
                this.setVarsAfterAjaxResponse(jData);
                this.addEventsAfterAjaxResponse();
                this.updateAcceptRadioBtn();
            }
        };

        thisPaymentInput.inlineDCCResponseHandler = function(data) {
            thisPaymentInput.updateInlineDCCOffer(data);
        };
        return thisPaymentInput;
    };

    SKYSALES.Class.PaymentInput.createObject = function(json) {
        SKYSALES.Util.createObject('paymentInput', 'PaymentInput', json);
    };
}

if (!SKYSALES.Class.CountryDropDown) {
    SKYSALES.Class.CountryDropDown = function() {
        var parent = SKYSALES.Class.SkySales();
        var thisCountryDropDown = SKYSALES.Util.extendObject(parent);

        thisCountryDropDown.SelectBox = null;
        thisCountryDropDown.SelectBoxId = '';
        thisCountryDropDown.TextBox = null;
        thisCountryDropDown.TextBoxId = null;
        thisCountryDropDown.countryInfo = SKYSALES.Util.getResource().countryInfo;

        thisCountryDropDown.init = function(json) {
            this.setSettingsByObject(json);
            this.setVars();
            this.addEvents();

            thisCountryDropDown.populateDropDown();

            thisCountryDropDown.TextBox.val(this.TextBox.val());
        };

        thisCountryDropDown.setVars = function() {
            thisCountryDropDown.TextBox = this.getById(thisCountryDropDown.TextBoxId);
            thisCountryDropDown.Select = this.getById(thisCountryDropDown.SelectBoxId);
        };

        thisCountryDropDown.addEvents = function() {
            thisCountryDropDown.Select.change(thisCountryDropDown.updateTextBoxValue);
        };

        thisCountryDropDown.updateTextBoxValue = function() {
            var codeValue = $(this).val();
            thisCountryDropDown.TextBox.val(codeValue);
        };

        thisCountryDropDown.populateDropDown = function() {
            var selectParamObj = {
                'selectBox': this.Select,
                'objectArray': this.countryInfo.CountryList,
                'selectedItem': this.TextBox.val(),
                'showCode': false
            };

            SKYSALES.Util.populateSelect(selectParamObj);
        };

        return thisCountryDropDown;
    };
}

/*
Name: 
Class PriceDisplay
Param:
None
Return: 
An instance of PriceDisplay
Functionality:
Handels the PriceDisplay control
Notes:
All functionality is inherited from the SkySales Class
Class Hierarchy:
SkySales -> PriceDisplay
*/
if (!SKYSALES.Class.PriceDisplay) {
    SKYSALES.Class.PriceDisplay = function() {
        var parent = new SKYSALES.Class.SkySales();
        var thisPriceDisplay = SKYSALES.Util.extendObject(parent);

        thisPriceDisplay.toggleViewIdArray = null;

        thisPriceDisplay.init = function(json) {
            this.setSettingsByObject(json);

            var toggleViewIdArray = this.toggleViewIdArray || [];
            var i = 0;
            var toggleView = null;
            for (i = 0; i < toggleViewIdArray.length; i += 1) {
                toggleView = new SKYSALES.Class.ToggleView();
                toggleView.init(toggleViewIdArray[i]);
                thisPriceDisplay.toggleViewIdArray[i] = toggleView;
            }
        };
        return thisPriceDisplay;
    };
    SKYSALES.Class.PriceDisplay.createObject = function(json) {
        SKYSALES.Util.createObject('priceDisplay', 'PriceDisplay', json);
    };
}

/*
Name: 
Class FlightDisplay
Param:
None
Return: 
An instance of FlightDisplay
Functionality:
Handels the FlightDisplay control
Notes:
All functionality is inherited from the SkySales Class
Class Hierarchy:
SkySales -> FlightDisplay
*/
if (!SKYSALES.Class.FlightDisplay) {
    SKYSALES.Class.FlightDisplay = function() {
        var parent = new SKYSALES.Class.SkySales();
        var thisFlightDisplay = SKYSALES.Util.extendObject(parent);

        thisFlightDisplay.toggleViewIdArray = null;

        thisFlightDisplay.init = function(json) {
            this.setSettingsByObject(json);

            var toggleViewIdArray = this.toggleViewIdArray || [];
            var i = 0;
            var toggleView = null;
            for (i = 0; i < toggleViewIdArray.length; i += 1) {
                toggleView = new SKYSALES.Class.ToggleView();
                toggleView.init(toggleViewIdArray[i]);
                thisFlightDisplay.toggleViewIdArray[i] = toggleView;
            }
        };
        return thisFlightDisplay;
    };
    SKYSALES.Class.FlightDisplay.createObject = function(json) {
        SKYSALES.Util.createObject('flightDisplay', 'FlightDisplay', json);
    };
}

/*
Name: 
Class RandomImage
Param:
None
Return: 
An instance of RandomImage
Functionality:
Handels the RandomImage on the search view
Notes:
This class can be used to display a random image,
provided it has a list of uri that point to images,
and a dom node to place the image.
Class Hierarchy:
SkySales -> RandomImage
*/
if (!SKYSALES.Class.RandomImage) {
    SKYSALES.Class.RandomImage = function() {
        var parent = new SKYSALES.Class.SkySales();
        var thisRandomImage = SKYSALES.Util.extendObject(parent);

        thisRandomImage.imageUriArray = [];

        thisRandomImage.init = function(json) {
            this.setSettingsByObject(json);
            this.setVars();
            this.setAsBackground();
        };

        thisRandomImage.getRandomNumber = function() {
            var randomNumberMax = this.imageUriArray.length;
            var randomNumber = Math.floor(Math.random() * randomNumberMax);
            return randomNumber;
        };

        thisRandomImage.setAsBackground = function() {
            var randomNumber = this.getRandomNumber();
            var uri = 'url(' + this.imageUriArray[randomNumber] + ')';
            this.container.css('background-image', uri);
        };
        return thisRandomImage;
    };
    SKYSALES.Class.RandomImage.createObject = function(json) {
        SKYSALES.Util.createObject('randomImage', 'RandomImage', json);
    };
}

/*
Name: 
Class DropDown
Param:
None
Return: 
An instance of DropDown
Functionality:
The DropDown class is a combo box.
Notes:
Provided with an input box, and an array of objects that have a code and name property
it will turn the text box into a combo box that attemps to help you auto complete what you are typing
given the data provided.
The way to turn an input box into a combo box is
var selectParamObj = {
'input': $(inputBox),
'options': [ { "code": "", "name": name } ]
};
SKYSALES.Class.DropDown.getDropDown(selectParamObj);
Class Hierarchy:
DropDown
*/
SKYSALES.Class.DropDown = function(paramObject) {
    paramObject = paramObject || {};
    var thisDrop = this;

    thisDrop.container = {};
    thisDrop.name = '';
    thisDrop.options = [];
    thisDrop.dropDownContainer = null;
    thisDrop.dropDownContainerInput = null;
    thisDrop.document = null;
    thisDrop.optionList = null;
    thisDrop.optionActiveClass = 'optionActive';
    thisDrop.timeOutObj = null;
    thisDrop.timeOut = 225;
    thisDrop.minCharLength = 2;
    thisDrop.optionMax = 100;
    thisDrop.html = '<div></div><div class="dropDownContainer"></div>';
    thisDrop.autoComplete = true;

    thisDrop.setSettingsByObject = function(jsonObject) {
        var prop = null;
        for (prop in jsonObject) {
            if (thisDrop.hasOwnProperty(prop)) {
                thisDrop[prop] = jsonObject[prop];
            }
        }
    };

    thisDrop.getOptionHtml = function(search) {
        search = search || '';
        var option = {};
        var prop = '';
        var optionHtml = '';
        var optionCount = 0;
        var optionHash = thisDrop.options;
        var re = new RegExp('^' + search, 'i');
        if (search.length < thisDrop.minCharLength) {
            optionHtml = '';
        }
        else {
            for (prop in optionHash) {
                if (optionHash.hasOwnProperty(prop)) {
                    option = optionHash[prop];
                    option.name = option.name || '';
                    option.code = option.code || '';
                    if (option.name.match(re) || option.code.match(re)) {
                        optionHtml += '<div><span>' + option.code + '</span>' + option.name + ' (' + option.code + ')' + '</div>';
                        optionCount += 1;
                    }
                    if (optionCount >= thisDrop.optionMax) {
                        break;
                    }
                }
            }
        }
        return optionHtml;
    };

    thisDrop.close = function() {
        if (thisDrop.timeOutObj) {
            window.clearTimeout(thisDrop.timeOutObj);
        }
        thisDrop.document.unbind('click', thisDrop.close);
        if (thisDrop.optionList) {
            thisDrop.optionList.unbind('hover');
            thisDrop.optionList.unbind('click');
        }
        thisDrop.optionList = null;
        thisDrop.dropDownContainer.html('');
    };

    thisDrop.getActiveOptionIndex = function() {
        var activeOptionIndex = -1;
        var activeOption = $('.' + thisDrop.optionActiveClass, thisDrop.dropDownContainer);
        if (thisDrop.optionList && (activeOption.length > 0)) {
            activeOptionIndex = thisDrop.optionList.index(activeOption[0]);
        }
        return activeOptionIndex;
    };

    thisDrop.arrowDown = function() {
        var activeOptionIndex = thisDrop.getActiveOptionIndex();
        if (thisDrop.optionList) {
            if ((activeOptionIndex === -1) && (thisDrop.optionList.length > 0)) {
                thisDrop.optionActive.call(thisDrop.optionList[0]);
            }
            else if (thisDrop.optionList.length > activeOptionIndex + 1) {
                thisDrop.optionInActive.call(thisDrop.optionList[activeOptionIndex]);
                thisDrop.optionActive.call(thisDrop.optionList[activeOptionIndex + 1]);
            }
            else {
                thisDrop.arrowDownOpen();
            }
        }
        else {
            thisDrop.arrowDownOpen();
        }
    };

    thisDrop.arrowDownOpen = function() {
        var oldMinCharLength = thisDrop.minCharLength;
        thisDrop.minCharLength = 0;
        thisDrop.open();
        thisDrop.minCharLength = oldMinCharLength;
    };

    thisDrop.arrowUp = function() {
        var activeOptionIndex = thisDrop.getActiveOptionIndex();
        if (thisDrop.optionList) {
            if ((activeOptionIndex === -1) && (thisDrop.optionList.length > 0)) {
                thisDrop.optionActive.call(thisDrop.optionList[0]);
            }
            else if ((activeOptionIndex > 0) && (thisDrop.optionList.length > 0)) {
                thisDrop.optionInActive.call(thisDrop.optionList[activeOptionIndex]);
                thisDrop.optionActive.call(thisDrop.optionList[activeOptionIndex - 1]);
            }
        }
    };

    thisDrop.selectButton = function() {
        var activeOptionIndex = thisDrop.getActiveOptionIndex();
        var oldOptionMax = thisDrop.optionMax;
        if (activeOptionIndex > -1) {
            thisDrop.selectOption.call(thisDrop.optionList[activeOptionIndex]);
        }
        else if (thisDrop.autoComplete === true) {
            thisDrop.optionMax = 1;
            thisDrop.open();
            if (thisDrop.optionList && (thisDrop.optionList.length > 0)) {
                thisDrop.selectOption.call(thisDrop.optionList[0]);
            }
            thisDrop.optionMax = oldOptionMax;
        }
    };

    /*
    40: down arrow
    38: up arrow
    32: space
    9: tab
    13: enter
    */

    thisDrop.keyEvent = function(key) {
        var retVal = true;
        var keyNum = key.which;
        if (keyNum === 40) {
            thisDrop.arrowDown();
            thisDrop.autoComplete = true;
            retVal = false;
        }
        else if (keyNum === 38) {
            thisDrop.arrowUp();
            thisDrop.autoComplete = true;
            retVal = false;
        }
        else if (keyNum === 9) {
            thisDrop.selectButton();
            thisDrop.inputBlur();
        }
        else if (keyNum === 13) {
            thisDrop.selectButton();
            thisDrop.autoComplete = false;
            retVal = false;
        }
        else {
            thisDrop.autoComplete = true;
        }
        return retVal;
    };

    thisDrop.inputKeyEvent = function(key) {
        var retVal = true;
        var keyNum = key.which;
        if ((keyNum !== 40) && (keyNum !== 38) && (keyNum !== 9) && (keyNum !== 13)) {
            if (thisDrop.timeOutObj) {
                window.clearTimeout(thisDrop.timeOutObj);
            }
            thisDrop.timeOutObj = window.setTimeout(thisDrop.open, thisDrop.timeOut);
            retVal = false;
        }
        return retVal;
    };

    thisDrop.catchEvent = function() {
        return false;
    };

    thisDrop.open = function() {
        var iframeHtml = '';
        var iframe = null;
        var search = thisDrop.dropDownContainerInput.val();
        var optionHtml = thisDrop.getOptionHtml(search);
        var height = 0;
        var containerWidth = 0;
        thisDrop.dropDownContainer.html(optionHtml);
        thisDrop.addOptionEvents();
        thisDrop.dropDownContainer.click(thisDrop.catchEvent);
        thisDrop.document.click(thisDrop.close);

        thisDrop.dropDownContainer.show();
        if (thisDrop.optionList && (thisDrop.optionList.length > 0) && thisDrop.optionActive) {
            thisDrop.optionActive.call(thisDrop.optionList[0]);
        }
        containerWidth = thisDrop.dropDownContainer.width();

        if ($.browser.msie) {
            height = thisDrop.dropDownContainer.height();
            iframeHtml = '<iframe src="#"></iframe>';
            thisDrop.dropDownContainer.prepend(iframeHtml);
            iframe = $('iframe', thisDrop.dropDownContainer);
            iframe.width(containerWidth);
            iframe.height(height);
        }
    };

    thisDrop.optionActive = function() {
        var option = $(this);
        thisDrop.optionList.removeClass(thisDrop.optionActiveClass);
        option.addClass(thisDrop.optionActiveClass);
    };

    thisDrop.optionInActive = function() {
        var option = $(this);
        option.removeClass(thisDrop.optionActiveClass);
    };

    thisDrop.selectOption = function() {
        var text = $('span', this).text();
        thisDrop.dropDownContainerInput.val(text);
        thisDrop.close();
        thisDrop.dropDownContainerInput.change();
    };

    thisDrop.addOptionEvents = function() {
        thisDrop.optionList = $('div', thisDrop.dropDownContainer);
        thisDrop.optionList.hover(thisDrop.optionActive, thisDrop.optionInActive);
        thisDrop.optionList.click(thisDrop.selectOption);
    };

    thisDrop.inputBlur = function() {
        thisDrop.close();
    };

    thisDrop.addEvents = function(paramObject) {
        thisDrop.dropDownContainerInput = paramObject.input;
        thisDrop.dropDownContainer = $('div.dropDownContainer', thisDrop.container);
        thisDrop.document = $(document);
        thisDrop.dropDownContainerInput.keyup(thisDrop.inputKeyEvent);
        thisDrop.dropDownContainerInput.keydown(thisDrop.keyEvent);
    };

    thisDrop.init = function(paramObject) {
        thisDrop.setSettingsByObject(paramObject);
        var html = thisDrop.html;
        paramObject.input.attr('autocomplete', 'off');
        paramObject.input.wrap('<span class="dropDownOuterContainer"></span>');
        paramObject.input.after(html);
        thisDrop.container = paramObject.input.parent('span.dropDownOuterContainer');
        thisDrop.addEvents(paramObject);
        SKYSALES.Class.DropDown.dropDownArray[SKYSALES.Class.DropDown.dropDownArray.length] = thisDrop;
    };
    thisDrop.init(paramObject);
    return thisDrop;
};

SKYSALES.Class.DropDown.dropDownArray = [];

SKYSALES.Class.DropDown.getDropDown = function(selectParamObj) {
    var retVal = null;
    var i = 0;
    var dropDown = null;
    var dropDownArray = SKYSALES.Class.DropDown.dropDownArray;
    var input = null;
    var inputCompare = selectParamObj.input.get(0);
    for (i = 0; i < dropDownArray.length; i += 1) {
        dropDown = dropDownArray[i];
        input = dropDown.dropDownContainerInput.get(0);
        if ((input) && (inputCompare) && (input === inputCompare)) {
            retVal = dropDownArray[i];
            if (selectParamObj.options) {
                retVal.options = selectParamObj.options;
            }
        }
    }
    if (!retVal) {
        retVal = new SKYSALES.Class.DropDown(selectParamObj);
    }
    return retVal;
};

if (!SKYSALES.Class.DatePickerManager) {
    SKYSALES.Class.DatePickerManager = function() {
        var thisDatePickerManager = this;
        thisDatePickerManager.isAOS = false;
        thisDatePickerManager.yearMonth = null;
        thisDatePickerManager.day = null;
        thisDatePickerManager.linkedDate = null;
        thisDatePickerManager.linkedAltDateId = "";
        thisDatePickerManager.marketCount = null;
        thisDatePickerManager.jsDateFormat = '';

        var allDayOptionArray = [];
        var yearMonthDelimiter = '-';
        var yearMonthFormatString = 'yy-mm';
        var dayFormatString = 'dd';
        var firstDateOption = 'first';
        var fullDateFormatString = 'mm/dd/yy';

        var validateYearMonthRegExp = new RegExp('\\d{4}-\\d{2}');

        // Accepts a date object to return the number of days in the month
        var getDaysInMonth = function(dateParam) {
            var daysNotInMonthDate = new Date(dateParam.getFullYear(), dateParam.getMonth(), 32);
            var daysNotInMonth = daysNotInMonthDate.getDate();
            return 32 - daysNotInMonth;
        };

        // Checks if the day is in the correct format (2 digit numeric)
        var validateDay = function(dayParam) {
            // Added validation by Sean on 29 May 2012
            if (dayParam == null) {
                return true;
            }
            return dayParam.match(/\d{2}/);
        };

        // Checks if the year-month is in the correct format
        var validateYearMonth = function(yearMonthParam) {
            yearMonthParam = yearMonthParam || '';
            return yearMonthParam.match(validateYearMonthRegExp);
        };

        var getDate = function(yearMonthParam, dayParam) {
            var retDate = new Date();
            var yearMonthArray = yearMonthParam.split(yearMonthDelimiter);
            var yearIndex = 0;
            var monthIndex = 1;
            if (true === thisDatePickerManager.isAOS) {
                yearIndex = 1;
                monthIndex = 0;
            }
            var yearVal = yearMonthArray[yearIndex];
            var monthVal = yearMonthArray[monthIndex] - 1;
            retDate = new Date(yearVal, monthVal, dayParam);
            return retDate;
        };

        // Accepts a year-month and day parameters and returns it as a date object.
        var parseDate = function(yearMonthParam, dayParam) {
            var retDate = new Date();
            var validateDayVal = validateDay(dayParam);
            var validateYearMonthVal = validateYearMonth(yearMonthParam);

            if (validateDayVal && validateYearMonthVal) {
                var date = getDate(yearMonthParam, dayParam);
                var daysInMonth = getDaysInMonth(date);

                var dayVal = dayParam;
                if (dayParam > daysInMonth) {
                    dayVal = daysInMonth;
                }
                retDate = new Date(date.getFullYear(), date.getMonth(), dayVal);
            }
            else {
                retDate = new Date();
            }
            return retDate;
        };

        // Update the selected value of the datepicker to be the same as the value of the dropdown fields when clicked.
        var readLinked = function() {
            var defaultMinDate = new Date();
            defaultMinDate.setDate(defaultMinDate.getDate() - 1);
            var clientID = thisDatePickerManager.linkedAltDateId.indexOf('date_picker_display_id_1') > 0 ?
                            thisDatePickerManager.linkedAltDateId.replace('date_picker_display_id_1', '') :
                            thisDatePickerManager.linkedAltDateId.replace('date_picker_display_id_2', '');

            var firstDate = new Date($('#' + clientID + 'date_picker_id_1').val()),
                date = parseDate(thisDatePickerManager.yearMonth.val(), thisDatePickerManager.day.val());

            var dateString = $.datepicker.formatDate(fullDateFormatString, date);
            thisDatePickerManager.linkedDate.val(dateString);

            //when the return datepicker is shown, set the depart date as minDate value, else the default value
            return { minDate: ((thisDatePickerManager.linkedAltDateId.indexOf("2") > 0 && (getCurrentURL().indexOf('select') < 0 && getCurrentURL().indexOf('searchchange') < 0))
            ? firstDate
            : defaultMinDate)};
        };

        var dayResizeAndSet = function(dateParam, selectBox) {
            var todayDate = new Date();
            var todayDay = todayDate.getDate();
            var todayYearMonth = todayDate.getFullYear() + '-' + todayDate.getMonth();
            var dateParamYearMonth = dateParam.getFullYear() + '-' + dateParam.getMonth();
            var monthYearIsCurrent = todayYearMonth === dateParamYearMonth;
            var todayIsPastSecond = (2 < todayDay);
            var trimInvalidDays = todayIsPastSecond && monthYearIsCurrent;
            var day = dateParam.getDate();
            var daysInMonth = getDaysInMonth(dateParam);
            var daysInMonthDifference = 31 - daysInMonth;
            var dayOptionArray = SKYSALES.Util.cloneArray(allDayOptionArray);
            var removeDaysAfterThisIndex = 31;
            if (daysInMonthDifference > 0) {
                removeDaysAfterThisIndex = 31 - daysInMonthDifference;
                dayOptionArray.splice(removeDaysAfterThisIndex, daysInMonthDifference);
            }
            if (trimInvalidDays) {
                dayOptionArray.splice(0, todayDay - 2);
            }
            var daySelectParams =
            {
                'selectedItem': day,
                'objectArray': dayOptionArray,
                'selectBox': selectBox,
                'clearOptions': true
            };
            SKYSALES.Util.populateSelect(daySelectParams);
        };

        // Ensures that the datepicker and the date fields are in sync when an update was done to the year
        var yearMonthUpdate = function() {
            var dayVal = thisDatePickerManager.day.val();
            var yearMonthVal = thisDatePickerManager.yearMonth.val();
            var dateSelected = getDate(thisDatePickerManager.yearMonth.val(), 1);
            var daysInMonth = getDaysInMonth(dateSelected);
            if (dayVal > daysInMonth) {
                dayVal = daysInMonth;
            }
            dateSelected = new Date(dateSelected.getFullYear(), dateSelected.getMonth(), dayVal);
            dayResizeAndSet(dateSelected, thisDatePickerManager.day);
            thisDatePickerManager.linkedDate.val($.datepicker.formatDate(fullDateFormatString, dateSelected));

            if (this.tagName == 'SELECT' && thisDatePickerManager.marketCount > 1) {
                if (this.id.indexOf('DropDownListMarketMonth1') != -1) {
                    var clientId = this.id.replace('DropDownListMarketMonth1', '');
                    var myMarketDay2 = $('#' + clientId + 'DropDownListMarketDay2');
                    var myMarketMonth2 = document.getElementById(clientId + 'DropDownListMarketMonth2');
                    var myDatePicker2 = document.getElementById(clientId.substring(0, clientId.length - 1) + 'date_picker_id_2');
                } else if (this.id.indexOf('DropDownListMarketMonth2') != -1) {
                    var clientId = this.id.replace('DropDownListMarketMonth2', '');
                    var myMarketDay2 = $('#' + clientId + 'DropDownListMarketDay1');
                    var myMarketMonth2 = document.getElementById(clientId + 'DropDownListMarketMonth1');
                    var myDatePicker2 = document.getElementById(clientId.substring(0, clientId.length - 1) + 'date_picker_id_1');
                }

                var dateCompare = getDate(myMarketMonth2.value, 1);
                dateCompare = new Date(dateCompare.getFullYear(), dateCompare.getMonth(), myMarketDay2.val());
                dayResizeAndSet(dateCompare, myMarketDay2);

                var dayVal = myMarketDay2.val();
                var date = getDate(myMarketMonth2.value, 1);
                var daysInMonth = getDaysInMonth(date);
                if (dayVal > daysInMonth) {
                    dayVal = daysInMonth;
                }

                date = new Date(date.getFullYear(), date.getMonth(), dayVal);

                myDatePicker2.value = $.datepicker.formatDate(fullDateFormatString, date);
            }
        };

        // Ensures that the datepicker and the date fields are in sync when an update was done to the day
        var dateUpdate = function() {
            var yearMonthVal = thisDatePickerManager.yearMonth.val();
            var dayVal = thisDatePickerManager.day.val();
            var date = parseDate(yearMonthVal, dayVal);
            var dateVal = $.datepicker.formatDate(fullDateFormatString, date);
            thisDatePickerManager.linkedDate.val(dateVal);

            if (this.id != null && thisDatePickerManager.marketCount > 1) {
                if (this.id.indexOf('DropDownListMarketDay1') != -1 &&
                    this.tagName == 'SELECT') {
                    var clientId = this.id.replace('DropDownListMarketDay1', '');
                    var myMarketDay2 = document.getElementById(clientId + 'DropDownListMarketDay2');
                    var myMarketMonth2 = document.getElementById(clientId + 'DropDownListMarketMonth2');
                    var myDatePicker2 = document.getElementById(clientId.substring(0, clientId.length - 1) + 'date_picker_id_2');
                    var myDisableInput2 = document.getElementById(clientId + 'CheckBoxChangeMarket_2');

                    if (myDisableInput2 == null || myDisableInput2.checked == true) {
                        //Need to be Commented by siaoshan(remove js date checking)
                        /* if (myMarketDay2.value < dayVal && myMarketMonth2.value == yearMonthVal) {
                        myMarketDay2.value = dayVal;
                        }*/
                    }
                }
                else if (this.id.indexOf('DropDownListMarketDay2') != -1 &&
                    this.tagName == 'SELECT') {
                    var clientId = this.id.replace('DropDownListMarketDay2', '');
                    var myMarketDay2 = document.getElementById(clientId + 'DropDownListMarketDay1');
                    var myMarketMonth2 = document.getElementById(clientId + 'DropDownListMarketMonth1');
                    var myDatePicker2 = document.getElementById(clientId.substring(0, clientId.length - 1) + 'date_picker_id_1');
                    var myDisableInput2 = document.getElementById(clientId + 'CheckBoxChangeMarket_1');

                    if (myDisableInput2 == null || myDisableInput2.checked == true) {
                        //Need to be Commented by siaoshan(remove js date checking)
                        /* if (myMarketDay2.value > dayVal && myMarketMonth2.value == yearMonthVal) {
                        myMarketDay2.value = dayVal;
                        }*/
                    }
                }

                var dayVal = myMarketDay2.value;
                var date = getDate(myMarketMonth2.value, 1);
                var daysInMonth = getDaysInMonth(date);
                //Need to be Commented by siaoshan(remove js date checking)
                /*if (dayVal > daysInMonth) {
                dayVal = daysInMonth;
                }*/

                date = new Date(date.getFullYear(), date.getMonth(), dayVal);

                myDatePicker2.value = $.datepicker.formatDate(fullDateFormatString, date);

            }
        };

        // Create an array of day objects.
        var createAllDayOptionArray = function() {
            var retArray = [];
            var optionIterator = 1;
            var option = {};
            for (optionIterator = 1; optionIterator <= 31; optionIterator += 1) {
                option = {};
                option.name = optionIterator;
                if (optionIterator <= 9) {
                    option.code = '0' + optionIterator;
                }
                else {
                    option.code = optionIterator;
                }
                retArray[optionIterator - 1] = option;
            }
            return retArray;
        };

        // Update select controls to match the date picker selection
        var updateLinked = function(dateString) {
            var match = dateString.match(/\d{2}\/\d{2}\/\d{4}/);
            var date = new Date();
            var yearMonthString = '',
                dayString = '',
                lowerCaseURL = getCurrentURL();
            if (match) {
                date = new Date(dateString);
                yearMonthString = $.datepicker.formatDate(yearMonthFormatString, date);
                thisDatePickerManager.yearMonth.val(yearMonthString);
                dayResizeAndSet(date, thisDatePickerManager.day);
                var thisIdDay = thisDatePickerManager.day[0].id;
                var thisIdMonth = thisDatePickerManager.yearMonth[0].id;

                var myMarketDay2 = $('#' + thisIdDay);
                var myMarketMonth2 = $('#' + thisIdMonth);

                myMarketMonth2.change();
                myMarketDay2.change();

                //set the date selected to the date_display(input control)
                $('#' + thisDatePickerManager.linkedAltDateId).val(SKYSALES.Util.dateDisplayFormat(dateString, thisDatePickerManager.jsDateFormat));

                //check if the depart date is clicked, for return date update
                if (thisDatePickerManager.linkedAltDateId.indexOf("1") > 0) {
                    var firstID = thisDatePickerManager.linkedAltDateId,
                        secondID = thisDatePickerManager.linkedAltDateId.replace("1", "2"),
                        firstDate = new Date($('#' + firstID.replace("display_id", "id")).val()),
                        secondDate = new Date($('#' + secondID.replace("display_id", "id")).val());

                    //check if the depart date is later than the return date
                    //if yes, set the return date to be the same with the depart date
                    if ((lowerCaseURL.indexOf("select") < 0)
                     && (lowerCaseURL.indexOf("searchchange") < 0)
                     && (!$('#oneWayOnly:checked').val())
                     && firstDate > secondDate) {
                        var datePicker1 = firstID.replace("display_id", "id");
                        $('#' + secondID).val(SKYSALES.Util.dateDisplayFormat($('#' + datePicker1).val(), thisDatePickerManager.jsDateFormat));

                        yearMonthString = $.datepicker.formatDate(yearMonthFormatString, firstDate);
                        dayString = $.datepicker.formatDate(dayFormatString, firstDate);
                        thisDatePickerManager.yearMonth.val(yearMonthString);

                        var dayReturnID = thisDatePickerManager.day[0].id.replace("1", "2"),
                            yearMonthReturnID = thisDatePickerManager.yearMonth[0].id.replace("1", "2"),
                            returnMarketDay = $('#' + dayReturnID),
                            returnMarketMonth = $('#' + yearMonthReturnID);

                        returnMarketMonth.val(yearMonthString);
                        dayResizeAndSet(firstDate, returnMarketDay);

                        $('#' + dayReturnID).val(dayString);
                        $('#' + yearMonthReturnID).val(yearMonthString);

                        returnMarketDay.change();
                        returnMarketMonth.change();
            }
                    if(firstDate <= secondDate)
                    {
                        if($('#' + firstID).hasClass('validationError'))
                        {
                            $('#' + firstID).removeClass('validationError');
                        }
                    }
                }

                //check if the page is the select page and searchchange page,
                //and if oneWay checkbox is not checked
                //if true open the return date calendar when depart date has been selected
                if ((lowerCaseURL.indexOf("select") < 0)
                    && (lowerCaseURL.indexOf("searchchange") < 0)
                    && (!$('#oneWayOnly:checked').val())
                    && (thisDatePickerManager.linkedAltDateId.indexOf("2") < 0)) {
                    $('#' + thisDatePickerManager.linkedAltDateId.replace('display_id_1', 'id_2')).datepicker('show');
                }
            }
        };

        var getCurrentURL = function() {
            if ($(this)[0].baseURI) {   //get URL for other browsers,except IE
                return $(this)[0].baseURI.toLowerCase();
            }
            else if ($(this)[0].document) {   //get URL for IE
                return $(this)[0].document.URL.toLowerCase();
            }
        }

        thisDatePickerManager.setSettingsByObject = function(paramObject) {
            var propName = '';
            for (propName in paramObject) {
                if (thisDatePickerManager.hasOwnProperty(propName)) {
                    thisDatePickerManager[propName] = paramObject[propName];
                }
            }
        };

        thisDatePickerManager.setVars = function() {
            if (true === thisDatePickerManager.isAOS) {
                yearMonthDelimiter = '/';
                yearMonthFormatString = 'm/yy';
                validateYearMonthRegExp = new RegExp('\\d{1,2}\\/\\d{4}');
                firstDateOption = 'eq(1)';
            }
        };

        var initFlight = function() {
            if (!thisDatePickerManager.isAOS) {
                dateUpdate();
            }
        };

        //highlights the departure / return date on the calendar
        //parameter: 'date' represents all dates in the calendar
        var highlightDepartReturnDate = function(date) {
            var getDateToHighLight, getSecondDate,
                id = thisDatePickerManager.linkedAltDateId,
                pickerID = id.replace("display_id", "id"),
                oneWay = false,
                lowerCaseURL = '';

            if ($(this)[0].baseURI) {   //get URL for other browsers,except IE
                lowerCaseURL = $(this)[0].baseURI.toLowerCase();
            }
            else if ($(this)[0].document) {   //get URL for IE
                lowerCaseURL = $(this)[0].document.URL.toLowerCase();
            }

            if ($('#oneWayOnly:checked').val()) {
                oneWay = true;
            }
            else if ((lowerCaseURL.indexOf('select') > 0) && ($('div#RoundTripContainer').css('display') != "none")) {
                oneWay = true;
            }

            if ($('#' + id).val() && id.indexOf("1") > 0) { //for depart date calendar
                getDateToHighLight = new Date($('#' + pickerID.replace("1", "2")).val());
            }
            else if ($('#' + id).val() && id.indexOf("2") > 0) { //for return date calendar
                getDateToHighLight = new Date($('#' + pickerID.replace("2", "1")).val());
            }

            //checks if date is not null and if not oneWay journey
            if (date !== null && !oneWay) {
                //if 'date' is same with the getDateToHighLight,
                //it returns a [boolean, string] object
                //boolean, determines if date is selectable
                //string, assigns the CSS Class
                if (date.getDate() == getDateToHighLight.getDate()
                && date.getMonth() == getDateToHighLight.getMonth()
                && date.getFullYear() == getDateToHighLight.getFullYear()) {
                    return [true, "highlight_date"];
                }
            }
            return [true, ""];
        }

        thisDatePickerManager.addEvents = function() {
            thisDatePickerManager.yearMonth.change(yearMonthUpdate);
            thisDatePickerManager.day.change(dateUpdate);

            var minDate = new Date();
            var maxDate = new Date();
            var setDayDate = new Date();
            maxDate.setFullYear(maxDate.getFullYear() + 1);

            // Get the first and last option in the year-month select
            var yearMonthFirst = $('option:' + firstDateOption, thisDatePickerManager.yearMonth).val();
            var yearMonthLast = $('option:last', thisDatePickerManager.yearMonth).val();

            // Create an array of day objects
            allDayOptionArray = createAllDayOptionArray();

            // Get the input where the datepicker is to be linked
            var linkedDate = thisDatePickerManager.linkedDate;

            if (validateYearMonth(yearMonthFirst)) {
                minDate.setDate(minDate.getDate() - 1);
                // Get the default selected date
                if (thisDatePickerManager.isAOS) {
                    setDayDate = new Date(thisDatePickerManager.linkedDate.val());
                }
                else {
                    setDayDate = getDate(thisDatePickerManager.yearMonth.val(), thisDatePickerManager.day.val());
                }
                dayResizeAndSet(setDayDate, thisDatePickerManager.day);
            }

            if (validateYearMonth(yearMonthLast)) {
                maxDate = getDate(yearMonthLast, 1);
                var daysInMonth = getDaysInMonth(maxDate);
                maxDate = new Date(maxDate.getFullYear(), maxDate.getMonth(), daysInMonth);
            }

            var resource = SKYSALES.Util.getResource();
            var dateCultureInfo = resource.dateCultureInfo;
            var datePickerSettings = SKYSALES.datepicker;
            var controlHeaderText = '';

            //Get header text to be used on datepicker
            if (thisDatePickerManager.linkedAltDateId.indexOf("1") > 0) { //if depart datepicker is selected
                controlHeaderText = datePickerSettings.controlHeaderDepartText;
            }
            else if (thisDatePickerManager.linkedAltDateId.indexOf("2") > 0) { //if return datepicker is selected
                controlHeaderText = datePickerSettings.controlHeaderReturnText;
            }

            initFlight();

            var datePickerParams =
            {
                'beforeShow': readLinked,
                'beforeShowDay': highlightDepartReturnDate,
                'onSelect': updateLinked,
                'minDate': minDate,
                'maxDate': maxDate,
                'showOn': 'both',
                'buttonImageOnly': true,
                'buttonImage': './images/AKBase/be_calendar.gif',
                'buttonText': 'Calendar',
                'numberOfMonths': 2,
                'mandatory': true,
                'monthNames': dateCultureInfo.monthNames,
                'monthNamesShort': dateCultureInfo.monthNamesShort,
                'dayNames': dateCultureInfo.dayNames,
                'dayNamesShort': dateCultureInfo.dayNamesShort,
                'dayNamesMin': dateCultureInfo.dayNamesMin,
                'controlHeaderText': controlHeaderText,
                'closeText': datePickerSettings.closeText,
                'prevText': datePickerSettings.prevText,
                'nextText': datePickerSettings.nextText,
                'currentText': datePickerSettings.currentText
            };

            // Attach the input to the datepicker
            linkedDate.datepicker(datePickerParams);
        };

        thisDatePickerManager.init = function(paramObject) {
            this.setSettingsByObject(paramObject);
            this.setVars();
            this.addEvents();
        };
    };
}
//Code to deal with the old way of getting the form on the page
SKYSALES.initializeSkySalesForm = function() {
    document.SkySales = document.forms.SkySales;
};

//Returns the skysales html form
SKYSALES.getSkySalesForm = function() {
    var skySalesForm = $('SkySales').get(0);
    return skySalesForm;
};

/*
Name: 
Class Common
Param:
None
Return: 
An instance of Common
Functionality:
Provide common functionality and events on every view.
Notes:
This should be put in the SKYSALES.Class namespace.
Class Hierarchy:
Common
*/
SKYSALES.Common = function() {
    var thisCommon = this;
    var countryInfo = null;

    thisCommon.allInputObjects = null;

    thisCommon.initializeCommon = function() {
        var hint = new SKYSALES.Hint();
        var inputLabel = new SKYSALES.InputLabel();
        thisCommon.addKeyDownEvents();
        thisCommon.addSetAndEraseEvents();
        thisCommon.setValues();
        hint.addHintEvents();
        inputLabel.formatInputLabel();
        thisCommon.stripeTables();
    };

    thisCommon.setValues = function() {
        var setValue = function(index) {
            if ((this.jsvalue !== null) && (this.jsvalue !== undefined)) {
                this.value = this.jsvalue;
            }
        };
        thisCommon.getAllInputObjects().each(setValue);
    };

    thisCommon.stopSubmit = function() {
        // Remove the submit event so that you can click the submit button
        $('form').unbind('submit', thisCommon.stopSubmit);
        return false;
    };

    thisCommon.addKeyDownEvents = function() {
        var keyFunction = function(e) {
            if (e.keyCode === 13) {
                // Stop the enter key in opera
                $('form').submit(thisCommon.stopSubmit);
                return false;
            }
            return true;
        };
        $(':input').keydown(keyFunction);
    };

    thisCommon.getAllInputObjects = function() {
        if (thisCommon.allInputObjects === null) {
            thisCommon.allInputObjects = $(':input');
        }
        return thisCommon.allInputObjects;
    };

    thisCommon.addSetAndEraseEvents = function() {
        var focusFunction = function() {
            thisCommon.eraseElement(this, this.requiredempty);
        };
        var blurFunction = function() {
            thisCommon.setElement(this, this.requiredempty);
            $(this).change();
        };
        var eventFunction = function(index) {
            var input = $(this);
            if ((this.requiredempty !== null) && (this.requiredempty !== undefined)) {
                //hack prevent focus on hidden elements in IE (which will throw an exception)
                if (input.is(':text') && (input.is(':hidden') === false)) {
                    input.focus(focusFunction);
                    input.blur(blurFunction);
                }
            }
        };
        thisCommon.getAllInputObjects().each(eventFunction);
    };

    thisCommon.eraseElement = function(element, defaultValue) {
        if (element.value === defaultValue) {
            element.value = "";
        }
    };

    thisCommon.setElement = function(element, defaultValue) {
        if (element.value === "") {
            element.value = defaultValue;
        }
    };

    thisCommon.getCountryInfo = function() {
        if (countryInfo === null) {
            countryInfo = window.countryInfo;
        }
        return countryInfo;
    };
    thisCommon.setCountryInfo = function(arg) {
        countryInfo = arg;
        return thisCommon;
    };

    thisCommon.isEmpty = function(element, defaultValue) {
        var val = null;
        var retVal = false;

        if ((element) && (defaultValue === undefined)) {
            if (element.requiredempty) {
                defaultValue = element.requiredempty;
            }
            else {
                defaultValue = '';
            }
        }

        val = SKYSALES.Common.getValue(element);
        if ((val === null) || (val === undefined) || (val.length === 0) || (val === defaultValue)) {
            retVal = true;
        }
        return retVal;
    };

    thisCommon.stripeTables = function() {
        $(".stripeMe tr:even").addClass("alt");
        return thisCommon;
    };
};

//Adds an event to the dom, and sets a function handler
SKYSALES.Common.addEvent = function(obj, eventString, functionRef) {
    $(obj).bind(eventString, functionRef);
};

//Returns the value of an html form element
SKYSALES.Common.getValue = function(e) {
    var val = null;
    if (e) {
        val = $(e).val();
        return val;
    }
    return null;
};

/*
Name: 
Class InputLabel
Param:
None
Return: 
An instance of InputLabel
Functionality:
Adds the * to required fields, and : to the end of labels for input boxes
Notes:
This should be put in the SKYSALES.Class namespace.
Class Hierarchy:
InputLabel
*/
SKYSALES.InputLabel = function() {
    var thisInputLabel = this;
    thisInputLabel.getInputLabelRequiredFlag = function() {
        return '*';
    };

    thisInputLabel.getInputLabelSuffix = function() {
        return ':';
    };

    thisInputLabel.formatInputLabel = function() {
        var requiredFlag = thisInputLabel.getInputLabelRequiredFlag();
        var suffix = thisInputLabel.getInputLabelSuffix();
        var eventFunction = function(index) {
            var label = $("label[@for=" + this.id + "]").eq(0);
            var labelText = $(label).text();
            var inputType = '';
            var required = null;
            if (labelText !== '') {
                inputType = $(this).attr("type");
                if ((inputType !== 'checkbox') && (inputType !== 'radio')) {
                    labelText = labelText;
                }
                required = this.required;
                if (required === undefined) {
                    required = null;
                }
                if (required === null) {
                    required = this.getAttribute('required');
                }
                if (required !== null) {
                    required = required.toString().toLowerCase();
                    if (required === 'true') {
                        labelText = requiredFlag + labelText;
                    }
                }
                $(label).text(labelText);
            }
        };
        SKYSALES.common.getAllInputObjects().each(eventFunction);
    };
};

/*
Name: 
Class Dhtml
Param:
None
Return: 
An instance of Dhtml
Functionality:
Provides methods that return the x and y position of an element on the dom
Notes:
This should be put in the SKYSALES.Class namespace.
Class Hierarchy:
Dhtml
*/
SKYSALES.Dhtml = function() {
    var thisDhtml = this;
    thisDhtml.getX = function(obj) {
        var pos = 0;
        if (obj.x) {
            pos += obj.x;
        }
        else if (obj.offsetParent) {
            while (obj.offsetParent) {
                pos += obj.offsetLeft;
                obj = obj.offsetParent;
            }
        }
        return pos;
    };

    thisDhtml.getY = function(obj) {
        var pos = 0;
        if (obj.y) {
            pos += obj.y;
        }
        else if (obj) {
            while (obj) {
                pos += obj.offsetTop;
                obj = obj.offsetParent;
            }
        }
        return pos;
    };
    return thisDhtml;
};

/*
Name: 
Class Hint
Param:
None
Return: 
An instance of Hint
Functionality:
A hint is shown as a helpful tip to the user about a html form field.
Such as showing the user what the valid characters are for their password, 
when they are registering as a new user
Notes:
This should be put in the SKYSALES.Class namespace.
Class Hierarchy:
Hint
*/
SKYSALES.Hint = function() {
    var thisHint = this;
    thisHint.addHintEvents = function() {
        var eventFunction = function(index) {
            if ((this.hint !== null) && (this.hint !== undefined)) {
                if (this.tagName && (this.tagName.toString().toLowerCase() === 'input')) {
                    thisHint.addHintFocusEvents(this);
                }
                else {
                    thisHint.addHintHoverEvents(this);
                }
            }
        };
        SKYSALES.common.getAllInputObjects().each(eventFunction);
    };

    thisHint.addHintFocusEvents = function(obj, hintText) {
        var focusFunction = function() {
            thisHint.showHint(obj, hintText);
        };
        var blurFunction = function() {
            thisHint.hideHint(obj, hintText);
        };
        if ($(obj).is(':hidden') === false) {
            $(obj).focus(focusFunction);
            $(obj).blur(blurFunction);
        }
    };

    thisHint.addHintHoverEvents = function(obj, hintText) {
        var showFunction = function() {
            thisHint.showHint(obj, hintText);
        };
        var hideFunction = function() {
            thisHint.hideHint(obj, hintText);
        };
        $(obj).hover(showFunction, hideFunction);
    };

    thisHint.getHintDivId = function() {
        return "cssHint";
    };

    thisHint.showHint = function(obj, hintHtml, xOffset, yOffset, referenceId) {
        var hintDivId = thisHint.getHintDivId();
        var jQueryHintDiv = $('#' + hintDivId);
        var x = 0;
        var y = 0;
        var defaultXOffset = 0;
        var defaultYOffset = 0;

        if (xOffset === undefined) {
            xOffset = obj.hintxoffset;
        }
        if (yOffset === undefined) {
            yOffset = obj.hintyoffset;
        }

        if (referenceId === undefined) {
            referenceId = obj.hintReferenceid;
        }
        var referenceObject = $('#' + referenceId).get(0);

        var dhtml = new SKYSALES.Dhtml();
        if (!referenceObject) {
            x = dhtml.getX(obj);
            y = dhtml.getY(obj);
            if (xOffset === undefined) {
                x += obj.offsetWidth + 5;
            }
        }
        else {
            x = dhtml.getX(referenceObject);
            y = dhtml.getY(referenceObject);
            if (xOffset === undefined) {
                x += referenceObject.offsetWidth + 5;
            }
        }

        if (hintHtml === undefined) {
            if (obj.hint !== undefined) {
                hintHtml = obj.hint;
            }
        }
        jQueryHintDiv.html(hintHtml);
        jQueryHintDiv.show();
        xOffset = (xOffset !== undefined) ? xOffset : defaultXOffset;
        yOffset = (yOffset !== undefined) ? yOffset : defaultYOffset;
        var leftX = parseInt(xOffset, 10) + parseInt(x, 10);
        var leftY = parseInt(yOffset, 10) + parseInt(y, 10);
        jQueryHintDiv.css('left', leftX + 'px');
        jQueryHintDiv.css('top', leftY + 'px');
    };

    thisHint.hideHint = function(obj) {
        var hintDivId = thisHint.getHintDivId();
        $('#' + hintDivId).hide();
    };
};

/*
Name: 
Class ValidationErrorReadAlong
Param:
None
Return: 
An instance of ValidationErrorReadAlong
Functionality:
To provide a way of showing the user validation error messages so that they can fix them in a user friendly way 
Notes:
This should be put in the SKYSALES.Class namespace.
Class Hierarchy:
ValidationErrorReadAlong
*/
SKYSALES.ValidationErrorReadAlong = function() {
    var thisReadAlong = this;
    thisReadAlong.objId = '';
    thisReadAlong.obj = null;
    thisReadAlong.errorMessage = '';
    thisReadAlong.isError = false;
    thisReadAlong.hasBeenFixed = false;
    thisReadAlong.hasValidationEvents = false;

    thisReadAlong.getValidationErrorHtml = function() {
        var validatonErrorHtml = '<span id="validationErrorContainerReadAlongIFrame" class="hidden" ><\/span> <div id="validationErrorContainerReadAlong" > <p class="close"> <a id="validationErrorContainerReadAlongCloseButton" \/> <\/p> <div id="validationErrorContainerReadAlongContent" > <h3 class="error">ERROR<\/h3> <div id="validationErrorContainerReadAlongList" > <\/div> <\/div> <\/div>';
        return validatonErrorHtml;
    };
    thisReadAlong.getValidationErrorCloseId = function() {
        return 'validationErrorContainerReadAlongCloseButton';
    };
    thisReadAlong.getValidationErrorListId = function() {
        return 'validationErrorContainerReadAlongList';
    };
    thisReadAlong.getValidationErrorIFrameId = function() {
        return 'validationErrorContainerReadAlongIFrame';
    };
    thisReadAlong.getValidationErrorDivId = function() {
        return 'validationErrorContainerReadAlong';
    };
    thisReadAlong.getFixedClass = function() {
        return 'fixedValidationError';
    };
    thisReadAlong.addCloseEvent = function() {
        var closeId = thisReadAlong.getValidationErrorCloseId();
        var closeFunction = function() {
            thisReadAlong.hide();
        };
        $('#' + closeId).click(closeFunction);
    };
    thisReadAlong.addValidationErrorDiv = function() {
        $('#mainContent').append(thisReadAlong.getValidationErrorHtml());
    };
    thisReadAlong.hide = function() {
        var iFrameId = thisReadAlong.getValidationErrorIFrameId();
        var divId = thisReadAlong.getValidationErrorDivId();
        $('#' + iFrameId).hide();
        $('#' + divId).hide();
    };
    thisReadAlong.addFocusEvent = function(index) {
        var data = { obj: this };
        var eventFunction = function(event) {
            var obj = event.data.obj;
            var hint = null;
            var readAlongDivObj = null;
            var readAlongDivWidth = 0;
            var readAlongDivHeight = 0;
            var x = 0;
            var y = 0;
            var dhtml = null;
            var iFrameObj = null;
            if (obj.isError === true) {
                hint = new SKYSALES.Hint();
                hint.hideHint();
                readAlongDivObj = $('#' + thisReadAlong.getValidationErrorDivId());
                readAlongDivWidth = parseInt(readAlongDivObj.width(), 10) + 5;
                readAlongDivHeight = parseInt(readAlongDivObj.height(), 10) + 5;
                dhtml = new SKYSALES.Dhtml();
                x = dhtml.getX(obj.obj);
                y = dhtml.getY(obj.obj);
                x = x + this.offsetWidth + 5 - (168);//
            	y = y - 72 - (248);//
                /* Start IE 6 Hack */
                if ($.browser.msie) {
                    iFrameObj = $('#' + thisReadAlong.getValidationErrorIFrameId());
                    iFrameObj.css('position', 'absolute');
                    iFrameObj.show();
                    // there are instances when the readAlongDivWidth is less than 25
                    if (readAlongDivWidth > 25) {
                        iFrameObj.width(readAlongDivWidth - 25);
                    }
                    iFrameObj.height(readAlongDivHeight - 5);
                    iFrameObj.css('left', x + 16);
                    iFrameObj.css('top', y);
                }
                /* End IE 6 Hack */
                readAlongDivObj.css('left', x);
                readAlongDivObj.css('top', y);
                readAlongDivObj.css('position', 'absolute');
                readAlongDivObj.show('slow');
                return false;
            }
        };

        if ($(this.obj).is(':hidden') === false) {
            $(this.obj).bind("focus", data, eventFunction);
        }

    };

    thisReadAlong.addBlurEvent = function(index) {
        var data = { obj: this };
        var eventFunction = function(event) {
            var obj = event.data.obj;
            var validateObj = new SKYSALES.Validate(null, '', '', null);
            validateObj.validateSingleElement(this);
            var errorMessage = validateObj.errors;
            var isFixed = false;
            var allFixed = true;
            if (validateObj.validationErrorArray.length > 0) {
                if (validateObj.validationErrorArray[0].isError === false) {
                    isFixed = true;
                }
            }
            var listId = obj.getValidationErrorListId();
            var listObj = $('#' + listId).find('li').eq(index);
            var fixedClass = obj.getFixedClass();
            var fixedFunction = function() {
                if (
                        (allFixed === true) &&
                        ($(this).attr("class").indexOf('hidden') === -1) &&
                        ($(this).attr("class").indexOf(fixedClass) === -1)
                        ) {
                    allFixed = false;
                }
            };
            if (isFixed === true) {
                obj.hasBeenFixed = true;
                listObj.addClass(fixedClass);
                allFixed = true;
                $('#' + listId).find('li').each(fixedFunction);
                if (allFixed === true) {
                    thisReadAlong.hide();
                }
            }
            else {
                obj.hasBeenFixed = false;
                listObj.removeClass(fixedClass);
                listObj.removeClass('hidden');
                obj.isError = true;
                obj.errorMessage = errorMessage;
                listObj.text(errorMessage);
            }
            return false;
        };
        $(this.obj).bind("blur", data, eventFunction);
    };
};

SKYSALES.errorsHeader = 'Please correct the following.\n\n';

/*
Name: 
Class Validate
Param:
None
Return: 
An instance of Validate
Functionality:
Provides a way of validating html form elements before they are submitted to the server
Notes:
This should be put in the SKYSALES.Class namespace.
Class Hierarchy:
Validate
*/
SKYSALES.Validate = function(form, controlID, errorsHeader, regexElementIdFilter) {
    var thisValidate = this;
    if (errorsHeader === undefined) {
        errorsHeader = SKYSALES.errorsHeader;
    }
    // set up properties
    thisValidate.form = form;
    thisValidate.namespace = controlID;
    thisValidate.errors = '';
    thisValidate.validationErrorArray = [];
    thisValidate.setfocus = null;
    thisValidate.clickedObj = null;
    thisValidate.errorDisplayMethod = 'read_along';
    thisValidate.errorsHeader = errorsHeader;
    thisValidate.namedErrors = [];

    //array of date strings
    thisValidate.dateRangeArray = [];

    if (regexElementIdFilter) {
        thisValidate.regexElementIdFilter = regexElementIdFilter;
    }
    // set up attributes
    thisValidate.requiredAttribute = 'required';
    thisValidate.requiredEmptyAttribute = 'requiredempty';
    thisValidate.validationTypeAttribute = 'validationtype';
    thisValidate.regexAttribute = 'regex';
    thisValidate.minLengthAttribute = 'minlength';
    thisValidate.numericMinLengthAttribute = 'numericminlength';
    thisValidate.maxLengthAttribute = 'maxlength';
    thisValidate.numericMaxLengthAttribute = 'numericmaxlength';
    thisValidate.minValueAttribute = 'minvalue';
    thisValidate.maxValueAttribute = 'maxvalue';
    thisValidate.equalsAttribute = 'equals';
    thisValidate.dateRangeAttribute = 'daterange';
    thisValidate.dateRange1HiddenIdAttribute = 'date1hiddenid';
    thisValidate.dateRange2HiddenIdAttribute = 'date2hiddenid';
    thisValidate.validateCreditCardAttribute = 'validatecreditcard';

    // set up error handling attributes
    thisValidate.byPassArrayValidationAttribute = 'bypassarray';
    thisValidate.defaultErrorAttribute = 'error';
    thisValidate.requiredErrorAttribute = 'requirederror';
    thisValidate.validationTypeErrorAttribute = 'validationtypeerror';
    thisValidate.regexErrorAttribute = 'regexerror';
    thisValidate.minLengthErrorAttribute = 'minlengtherror';
    thisValidate.maxLengthErrorAttribute = 'maxlengtherror';
    thisValidate.minValueErrorAttribute = 'minvalueerror';
    thisValidate.maxValueErrorAttribute = 'maxvalueerror';
    thisValidate.equalsErrorAttribute = 'equalserror';
    thisValidate.dateRangeErrorAttribute = 'daterangeerror';
    thisValidate.validationCreditCardErrorAttribute = 'validationcreditcarderror';
    // set up error handling default errors
    thisValidate.defaultError = '{label} is invalid.';
    thisValidate.defaultRequiredError = '{label} is required.';
    thisValidate.defaultValidationTypeError = '{label} is invalid.';
    thisValidate.defaultRegexError = '{label} is invalid.';
    thisValidate.defaultMinLengthError = '{label} is too short in length.';
    thisValidate.defaultMaxLengthError = '{label} is too long in length.';
    thisValidate.defaultMinValueError = '{label} must be greater than {minValue}.';
    thisValidate.defaultMaxValueError = '{label} must be less than {maxValue}.';
    thisValidate.defaultEqualsError = '{label} is not equal to {equals}';
    thisValidate.defaultNotEqualsError = '{label} cannot equal {equals}';
    thisValidate.defaultValidationCreditCardError = '{label} does not match your Card Type, please reconfirm your details before proceeding.';

    //thisValidate.defaultDateSameText = 'Your return date is \nthe same as your departure date.\n\nIs such a short trip intentional?';
    thisValidate.defaultValidationErrorClass = 'validationError';
    thisValidate.defaultValidationErrorLabelClass = 'validationErrorLabel';

    thisValidate.sameDate = false;
    thisValidate.dateRangeErrorAlert = false;
    // add methods to object
    thisValidate.run = function() {
        var nodeArray = $(':input', SKYSALES.getSkySalesForm()).get();
        var e = null;
        // run validation on the form elements
        for (var i = 0; i < nodeArray.length; i += 1) {
            e = nodeArray[i];
            if (!this.isExemptFromValidation(e) && !this.isByPassArrayValidation(e)) {
                thisValidate.validateSingleElement(e);
            }
            if (!this.isExemptFromValidation(e) && this.isByPassArrayValidation(e)) {
                thisValidate.byPassArrayValidation(e)
        }
        }
        return thisValidate.outputErrors();
    };

    //this call by passes the addition of control to the validationErrorArray object
    //such that it doesn't show the error again if the control is updated properly
    thisValidate.byPassArrayValidation = function(e) {
        this.validateRequired(e);
        var value = thisValidate.getValue(e);
        if ((thisValidate.errors.length < 1) && (value !== null) && (value !== "")) {
            thisValidate.validateType(e);
            thisValidate.validateRegex(e);
            thisValidate.validateMinLength(e);
            thisValidate.validateMaxLength(e);
            thisValidate.validateMinValue(e);
            thisValidate.validateMaxValue(e);
            thisValidate.validateEquals(e);
            thisValidate.validateDateRange(e);
        }    
    }

    thisValidate.runBySelector = function(selectorString) {
        var nodeArray = $(selectorString).find(':input').get();
        var node = null;
        var i = 0;
        // run validation on the form elements
        for (i = 0; i < nodeArray.length; i += 1) {
            node = nodeArray[i];
            thisValidate.validateSingleElement(node);
        }
        return false;
    };

    thisValidate.validateSingleElement = function(e) {
        $(e).removeClass(thisValidate.defaultValidationErrorClass);
        $("label[@for=" + e.id + "]").eq(0).removeClass(this.defaultValidationErrorLabelClass);

        var validationError = new SKYSALES.ValidationErrorReadAlong();
        validationError.objId = e.id;
        validationError.obj = e;
        this.validationErrorArray[thisValidate.validationErrorArray.length] = validationError;

        this.validateRequired(e);
        // only validate the rest if they actually have something
        var value = thisValidate.getValue(e);
        if ((thisValidate.errors.length < 1) && (value !== null) && (value !== "")) {
            thisValidate.validateType(e);
            thisValidate.validateRegex(e);
            thisValidate.validateMinLength(e);
            thisValidate.validateMaxLength(e);
            thisValidate.validateMinValue(e);
            thisValidate.validateMaxValue(e);
            thisValidate.validateEquals(e);
            thisValidate.validateDateRange(e);
            thisValidate.validateCreditCard(e);
        }
    };
    thisValidate.outputErrors = function() {
        // if there is an error output it
        var errorDisplayMethod = this.errorDisplayMethod.toString().toLowerCase();
        var errorHtml = '';
        var errorArray = [];
        var i = 0;
        var showDefaultErrorMethod = true;
        if (this.errors) {
            //errorHtml += '<div class="errorSectionHeader" >' + this.errorsHeader + '<\/div>';
            errorArray = thisValidate.errors.split('\n');
            errorHtml += '<ul class="validationErrorList" >';
            for (i = 0; i < errorArray.length; i += 1) {
                if (errorArray[i] !== '') {
                    errorHtml += '<li class="validationErrorListItem" >' + errorArray[i] + '<\/li>';
                }
            }
            errorHtml += '<\/ul>';
            if (errorDisplayMethod.indexOf('read_along') > -1) {
                thisValidate.outputErrorsReadAlong(errorHtml);
                showDefaultErrorMethod = false;
            }
            if (errorDisplayMethod.indexOf('alert') > -1) {
                alert(thisValidate.errorsHeader + thisValidate.errors);
            }
            if (showDefaultErrorMethod === true) {
                alert(thisValidate.errorsHeader + thisValidate.errors);
            }

            if (thisValidate.setfocus) {
                if ($(thisValidate.setfocus).is(':hidden') === false) {
                    try {
                        thisValidate.setfocus.blur();
                        thisValidate.setfocus.focus();
                    }
                    catch (ex) {
                    }
                }
            }
            return false;
        }
        else {
            if (thisValidate.sameDate || thisValidate.dateRangeErrorAlert) {
                return false;
        }
            return true;
        }
    };
    thisValidate.outputErrorsReadAlong = function(errorHtml) {
        var i = 0;
        var errorHtmlLocal = '';
        var validationError = null;
        var validateObject = this;
        var addErrorEventFunction = function(index) {
            this.hasValidationEvents = true;
            this.addFocusEvent(index);
            this.addBlurEvent(index);
        };

        validateObject.validationErrorReadAlong = new SKYSALES.ValidationErrorReadAlong();
        validateObject.readAlongDivId = $('#' + this.validationErrorReadAlong.getValidationErrorDivId()).attr('id');
        if (validateObject.readAlongDivId === undefined) {
            validateObject.validationErrorReadAlong.addValidationErrorDiv();
            validateObject.validationErrorReadAlong.addCloseEvent();
        }
        errorHtmlLocal += '<ul class="validationErrorList" >';
        for (i = 0; i < validateObject.validationErrorArray.length; i += 1) {
            validationError = this.validationErrorArray[i];
            if (validationError.isError === true) {
                errorHtmlLocal += '<li class="validationErrorListItem" >' + validationError.errorMessage + '<\/li>';
            }
            else {
                errorHtmlLocal += '<li class="validationErrorListItem hidden" >' + validationError.errorMessage + '<\/li>';
            }
        }
        $('#' + validateObject.validationErrorReadAlong.getValidationErrorListId()).html(errorHtmlLocal);

        $(validateObject.validationErrorArray).each(addErrorEventFunction);
    };
    thisValidate.checkFocus = function(e) {
        if (!thisValidate.setfocus) {
            thisValidate.setfocus = e;
        }
    };
    thisValidate.setError = function(e, errorAttribute, defaultTypeError) {
        var nameStr = '';
        var error = '';
        var dollarOne = '';
        var i = 0;
        var validationError = null;

        if (e.type === 'radio') {
            nameStr = e.getAttribute('name');
            if (nameStr.length > 0) {
                if (thisValidate.namedErrors[nameStr] !== undefined) {
                    return;
                }
                thisValidate.namedErrors[nameStr] = nameStr;
            }
        }

        error = e[errorAttribute];
        //var error = e.getAttribute(errorAttribute);
        if (!error) {
            if (e.getAttribute(errorAttribute)) {
                error = e.getAttribute(errorAttribute);
            }
            else if (e[thisValidate.defaultErrorAttribute]) {
                error = e[thisValidate.defaultErrorAttribute];
            }
            else if (defaultTypeError) {
                error = defaultTypeError;
            }
            else {
                error = thisValidate.defaultError;
            }
        }
        //alert(errorAttribute + "\n" + error + "\n" + e.requiredError);

        // this would make more sense but it doesn't work
        // so i'll do each explicitly while i make this work
        var results = error.match(/\{\s*(\w+)\s*\}/g);
        if (results) {
            for (i = 0; i < results.length; i += 1) {
                dollarOne = results[i].replace(/\{\s*(\w+)\s*\}/, '$1');
                error = error.replace(/\{\s*\w+\s*\}/, thisValidate.cleanAttributeForErrorDisplay(e, dollarOne));
            }
        }

        $(e).addClass(this.defaultValidationErrorClass);
        $("label[@for=" + e.id + "]").eq(0).addClass(thisValidate.defaultValidationErrorLabelClass);
        this.errors += error + '\n';

        var errorObjId = e.id;
        for (i = 0; i < thisValidate.validationErrorArray.length; i += 1) {
            validationError = thisValidate.validationErrorArray[i];
            if (validationError.objId === errorObjId) {
                validationError.errorMessage = error;
                validationError.isError = true;
                break;
            }
        }
        this.checkFocus(e);

    };
    thisValidate.cleanAttributeForErrorDisplay = function(e, attributeName) {
        var inputLabelObj = null;
        var requiredString = '';
        if (attributeName === undefined) {
            attributeName = '';
        }
        attributeName = attributeName.toLowerCase();
        var attribute = "";
        if (attributeName === 'label') {
            attribute = $("label[@for=" + e.id + "]").eq(0).text();
            inputLabelObj = new SKYSALES.InputLabel();
            requiredString = inputLabelObj.getInputLabelRequiredFlag();
            attribute = attribute.replace(requiredString, '');
        }
        if (!attribute) {
            attribute = e.id;
        }

        if (!attribute) {
            return attributeName;
        }

        if (attributeName.match(/^(minvalue|maxvalue)$/i)) {
            return attribute.replace(/[^\d.,]/g, '');
        }

        return attribute;
    };
    thisValidate.validateRequired = function(e) {
        var requiredAttribute = thisValidate.requiredAttribute;
        var requiredEmptyAttribute = thisValidate.requiredEmptyAttribute;
        var required = e[requiredAttribute] ? e[requiredAttribute] : e.getAttribute(requiredAttribute);
        var requiredEmptyString = e[requiredEmptyAttribute];
        var value = null;
        thisValidate.radioGroupHash = {};
        var radioName = '';
        var isRadioGroupChecked = false;

        if (required !== undefined && required != null) {
            required = required.toString().toLowerCase();
            if (requiredEmptyString) {
                requiredEmptyString = requiredEmptyString.toString().toLowerCase();
            }
            if (required === 'true') {
                value = thisValidate.getValue(e);
                if ((e.type === 'checkbox') && (e.checked === false)) {
                    value = '';
                }
                else if (e.type === 'radio') {
                    radioName = e.getAttribute('name');
                    if (thisValidate.radioGroupHash[radioName] === undefined) {
                        thisValidate.radioGroupHash[radioName] = $("input[@name='" + radioName + "']");
                    }
                    isRadioGroupChecked = thisValidate.radioGroupHash[radioName].is(':checked');
                    if (!isRadioGroupChecked) {
                        value = '';
                    }
                }
                // this will not produce an error if value === 0
                if (
                    (value === undefined) ||
                    (value === 'none') ||
                    (value === null) ||
                    (value === '') ||
                    (value.toLowerCase() === requiredEmptyString)
                    ) {
                    thisValidate.setError(e, thisValidate.requiredErrorAttribute, thisValidate.defaultRequiredError);
                }
            }
        }
    };
    thisValidate.validateType = function(e) {
        var type = e[this.validationTypeAttribute];
        //var type = e.getAttribute(this.validationTypeAttribute);
        var value = this.getValue(e);

        if ((type) && (value !== null)) {
            type = type.toLowerCase();
            if ((type === 'address') && (!value.match(thisValidate.stringPattern))) {
                thisValidate.setValidateTypeError(e);
            }
            else if ((type === 'alphanumeric') && (!value.match(thisValidate.alphaNumericPattern))) {
                thisValidate.setValidateTypeError(e);
            }
            else if ((type === 'amount') && (!thisValidate.validateAmount(value))) {
                thisValidate.setValidateTypeError(e);
            }
            else if ((type === 'country') && (!value.match(thisValidate.stringPattern))) {
                thisValidate.setValidateTypeError(e);
            }
            else if ((type === 'email') && (!value.match(thisValidate.emailPattern))) {
                thisValidate.setValidateTypeError(e);
            }
            else if ((type === 'mod10') && (!thisValidate.validateMod10(value))) {
                thisValidate.setValidateTypeError(e);
            }
            else if ((type === 'name') && (!value.match(thisValidate.stringPattern))) {
                thisValidate.setValidateTypeError(e);
            }
            else if ((type === 'numeric') && (!thisValidate.validateNumeric(value))) {
                thisValidate.setValidateTypeError(e);
            }
            else if ((type.indexOf('date') === 0) && (!thisValidate.validateDate(e, type, value))) {
                thisValidate.setValidateTypeError(e);
            }
            else if ((type === 'state') && (!value.match(thisValidate.stringPattern))) {
                thisValidate.setValidateTypeError(e);
            }
            else if ((type === 'string') && (!value.match(thisValidate.stringPattern))) {
                thisValidate.setValidateTypeError(e);
            }
            else if ((type === 'uppercasestring') && (!value.match(thisValidate.upperCaseStringPattern))) {
                thisValidate.setValidateTypeError(e);
            }
            else if ((type === 'zip') && (!value.match(thisValidate.stringPattern))) {
                thisValidate.setValidateTypeError(e);
            }
        }
    };
    thisValidate.validateRegex = function(e) {
        var regex = e[thisValidate.regexAttribute] != undefined ? e[thisValidate.regexAttribute] : e.getAttribute(thisValidate.regexAttribute);
        //var regex = e.getAttribute(this.regexAttribute);
        var value = thisValidate.getValue(e);
        if ((value !== null) && (regex) && (!value.match(regex))) {
            this.setError(e, thisValidate.regexErrorAttribute, thisValidate.defaultRegexError);
        }
    };
    thisValidate.validateMinLength = function(e) {
        var length = e[thisValidate.minLengthAttribute] != undefined ? e[thisValidate.minLengthAttribute] : e.getAttribute(thisValidate.minLengthAttribute);
        var numericLength = e[thisValidate.numericMinLengthAttribute];
        var value = this.getValue(e);

        if ((0 < length) && (value !== null) && (value.length < length)) {
            thisValidate.setError(e, thisValidate.minLengthErrorAttribute, thisValidate.defaultMinLengthError);
        }
        else if ((0 < numericLength) && (0 < value.length) && (value.replace(thisValidate.numericStripper, '').length < numericLength)) {
            thisValidate.setError(e, thisValidate.minLengthErrorAttribute, thisValidate.defaultMinLengthError);
        }
    };
    thisValidate.validateMaxLength = function(e) {
        var length = e[thisValidate.maxLengthAttribute] != undefined ? e[thisValidate.maxLengthAttribute] : e.getAttribute(thisValidate.maxLengthAttribute);
        var numericLength = e[thisValidate.numericMaxLengthAttribute];
        var value = this.getValue(e);

        if ((0 < length) && (value !== null) && (length < value.length)) {
            thisValidate.setError(e, thisValidate.maxLengthErrorAttribute, thisValidate.defaultMaxLengthError);
        }
        else if ((0 < numericLength) && (0 < value.length) && (numericLength < value.replace(thisValidate.numericStripper, '').length)) {
            thisValidate.setError(e, thisValidate.maxLengthErrorAttribute, thisValidate.defaultMaxLengthError);
        }
    };
    thisValidate.validateMinValue = function(e) {
        var min = e[thisValidate.minValueAttribute];
        var value = thisValidate.getValue(e);

        if ((value !== null) && (min !== undefined) && (0 < min.length)) {
            if ((5 < min.length) && (min.substring(0, 5) === '&gt;=')) {
                if (value < parseFloat(min.substring(5, min.length))) {
                    thisValidate.setError(e, thisValidate.minValueErrorAttribute, thisValidate.defaultMinValueError);
                }
            }
            else if ((4 < min.length) && (min.substring(0, 4) === '&gt;')) {
                if (value <= parseFloat(min.substring(4, min.length))) {
                    thisValidate.setError(e, thisValidate.minValueErrorAttribute, thisValidate.defaultMinValueError);
                }
            }
            else if (value < parseFloat(min)) {
                thisValidate.setError(e, thisValidate.minValueErrorAttribute, thisValidate.defaultMinValueError);
            }
        }
    };
    thisValidate.validateMaxValue = function(e) {
        var max = e[this.maxValueAttribute];
        var value = this.getValue(e);

        if ((value !== null) && (max !== undefined) && (0 < max.length)) {
            if ((5 < max.length) && (max.substring(0, 5) === '&lt;=')) {
                if (value > parseFloat(max.substring(5, max.length))) {
                    thisValidate.setError(e, thisValidate.maxValueErrorAttribute, thisValidate.defaultMaxValueError);
                }
            }
            else if ((4 < max.length) && (max.substring(0, 4) === '&lt;')) {
                if (value >= parseFloat(max.substring(4, max.length))) {
                    thisValidate.setError(e, thisValidate.maxValueErrorAttribute, thisValidate.defaultMaxValueError);
                }
            }
            else if (parseFloat(value) > max) {
                thisValidate.setError(e, thisValidate.maxValueErrorAttribute, thisValidate.defaultMaxValueError);
            }
        }
    };
    thisValidate.validateEquals = function(e) {
        // eventually this should be equipped to do string
        // comparison as well as other element comparisons

        var equal = e[thisValidate.equalsAttribute];
        var value = thisValidate.getValue(e);

        if ((value !== null) && (equal !== undefined) && (0 < equal.length)) {
            if ((2 < equal.length) && (equal.substring(0, 2) === '!=')) {
                if (value === equal.substring(2, equal.length)) {
                    thisValidate.setError(e, thisValidate.equalsErrorAttribute, thisValidate.defaultEqualsError);
                }
            }
        }
    };
    thisValidate.validateCreditCard = function(e) {
        var CCValidation = e[thisValidate.validateCreditCardAttribute];
        var value = thisValidate.getValue(e);
        
        if ((value !== null) && (CCValidation !== undefined) && (0 < CCValidation.length)) {
            var CClen = value.substring(0, CCValidation.length);
            if (CClen != CCValidation)
                thisValidate.setError(e, thisValidate.validationCreditCardErrorAttribute, thisValidate.defaultValidationCreditCardError);
        }
    };
    var checkDateRangeExists = function(dateHidden2) {
        var parent = dateHidden2.parent();
        var parent2 = parent.parent();
        //        var noDateRangeIE = parent.is(':hidden');
        //        var noDateRangeNonIE = parent2.is(':hidden');
        var noDateRangeIE = parent2.is(':hidden');
        var noDateRangeOtherIE = parent.is(':hidden');
        var noDateRangeNonIE = (parent2.parent()).is(':hidden');
        var retVal = !(noDateRangeIE || (noDateRangeNonIE || noDateRangeOtherIE));
        return retVal;
    };
    thisValidate.checkIfValidateDateRangeNeeded = function(e) {
        //this changes are for the calendar enhancement, this gets the attribute set on XSLT for the date_pikcer_display
        var date = e.attributes[thisValidate.dateRangeAttribute] ? e.attributes[thisValidate.dateRangeAttribute].value : e[thisValidate.dateRangeAttribute];
        var date1HiddenId = e.attributes[thisValidate.dateRange1HiddenIdAttribute] ? e.attributes[thisValidate.dateRange1HiddenIdAttribute].value : e[thisValidate.dateRange1HiddenIdAttribute];
        var date2HiddenId = e.attributes[thisValidate.dateRange2HiddenIdAttribute] ? e.attributes[thisValidate.dateRange2HiddenIdAttribute].value : e[thisValidate.dateRange2HiddenIdAttribute];
        var idLastChar = '';
        var idSuffix = '';
        var id = e.id;
        var startValidate = false;
        var dateRangeExists = false;
        var dateHidden1 = null;
        var dateHidden2 = null;

        if ((date !== undefined) && (0 < date.length)) {
            //parse from the id the trailing "count" value
            //e.g. DROPDOWNLISTMARKETDAY2 -> extract 2
            idLastChar = id.charAt(id.length - 1);
            if (this.validateNumeric(idLastChar)) {
                idSuffix = idLastChar;
            }

            //for flight search page only run check on the first month in the range
            if (('1' === idSuffix) || ('' === idSuffix)) {
                // If one of the date range items is hidden then it's one way and the validation shouldn't check a pair of dates.
                dateHidden2 = $('#' + date2HiddenId);
                dateRangeExists = checkDateRangeExists(dateHidden2);
                if (dateRangeExists) {
                    startValidate = true;
                    dateHidden1 = $('#' + date1HiddenId);
                    thisValidate.dateRangeArray[0] = dateHidden1.val();
                    thisValidate.dateRangeArray[1] = dateHidden2.val();
                }
            }
        }
        return startValidate;
    };
    thisValidate.validateDateRange = function(e) {
        var marketDate = null;
        var datesInOrder = false;
        var datesAreSame = false;
        // Determine if date range needs to be validated. If "startValidate" is
        // true, it means that we need to check if the date range is valid.
        //
        var startValidate = thisValidate.checkIfValidateDateRangeNeeded(e);

        if (startValidate) {
            marketDate = new SKYSALES.Class.MarketDate();
            datesInOrder = marketDate.datesInOrder(this.dateRangeArray);
            if (!datesInOrder) {
                if (getCurrentURL().indexOf('select') > -1) {
                    alert(e.attributes[this.dateRangeErrorAttribute].value);
                    if (e.attributes['class'].value.indexOf('validationError') < 0) {
                        $('#' + e.id).addClass('validationError');
                    }
                    thisValidate.dateRangeErrorAlert = true;
                }
                else {
                this.setError(e, this.dateRangeErrorAttribute, this.defaultError);
            }
        }
            else {
                thisValidate.dateRangeErrorAlert = false;
            }
        }
    };
    var getCurrentURL = function() {
        if ($(this)[0].baseURI) {   //get URL for other browsers,except IE
            return $(this)[0].baseURI.toLowerCase();
        }
        else if ($(this)[0].document) {   //get URL for IE
            return $(this)[0].document.URL.toLowerCase();
        }
    }
    thisValidate.isExemptFromValidation = function(e) {
        if (e.id.indexOf(this.namespace) !== 0) {
            return true;
        }

        if (this.regexElementIdFilter && (!e.id.match(this.regexElementIdFilter))) {
            return true;
        }

        return false;
    };
    thisValidate.isByPassArrayValidation = function(e) {
        if (e.attributes[thisValidate.byPassArrayValidationAttribute] &&
            e.attributes[thisValidate.byPassArrayValidationAttribute].value &&
            e.attributes[thisValidate.byPassArrayValidationAttribute].value !== 'false') {
                return true;
            }
        return false;
    }

    // add validation type methods
    thisValidate.setValidateTypeError = function(e) {
        this.setError(e, this.validationTypeErrorAttribute, this.defaultValidationTypeError);
    };
    thisValidate.validateAmount = function(amount) {
        if ((!amount.match(this.amountPattern)) || (amount === 0)) {
            return false;
        }

        return true;
    };
    thisValidate.validateDate = function(e, type, value) {
        var lowerCaseType = '';
        var today = new Date();
        if (type) {
            lowerCaseType = type.toLowerCase();
        }
        if ((lowerCaseType === 'dateyear') && ((value < today.getFullYear()) || (!value.match(thisValidate.dateYearPattern)))) {
            return false;
        }
        //just make sure it is two digits for now
        else if ((lowerCaseType === 'datemonth') && (!value.match(thisValidate.dateMonthPattern))) {
            return false;
        }
        //just make sure it is two digits for now
        else if ((lowerCaseType === 'dateday') && (!value.match(thisValidate.DateDayPattern))) {
            return false;
        }

        return true;
    };
    thisValidate.validateMod10 = function(cardNumber) {
        var ccCheckRegExp = /\D/;
        var cardNumbersOnly = cardNumber.replace(/ /g, "");
        var numberProduct;
        var checkSumTotal = 0;
        var productDigitCounter = 0;
        var digitCounter = 0;

        if (!ccCheckRegExp.test(cardNumbersOnly)) {
            while (cardNumbersOnly.length < 16) {
                cardNumbersOnly = '0' + cardNumbersOnly;
            }

            for (digitCounter = cardNumbersOnly.length - 1; 0 <= digitCounter; digitCounter -= 2) {
                checkSumTotal += parseInt(cardNumbersOnly.charAt(digitCounter), 10);
                numberProduct = String((cardNumbersOnly.charAt(digitCounter - 1) * 2));
                for (productDigitCounter = 0; productDigitCounter < numberProduct.length; productDigitCounter += 1) {
                    checkSumTotal += parseInt(numberProduct.charAt(productDigitCounter), 10);
                }
            }

            return (checkSumTotal % 10 === 0);
        }
        return false;
    };

    // Validates if the data passed is numeric
    thisValidate.validateNumeric = function(number) {
        number = number.replace(/\s/g, '');

        if (!number.match(thisValidate.numericPattern)) {
            return false;
        }

        return true;
    };

    thisValidate.getValue = function(e) {
        return SKYSALES.Common.getValue(e);
    };

    //this.nonePattern = '^\.*$';
    thisValidate.stringPattern = /^.+$/;
    thisValidate.upperCaseStringPattern = /^[A-Z]([A-Z|\s])*$/;
    thisValidate.numericPattern = /^\d+$/;
    thisValidate.numericStripper = /\D/g;
    thisValidate.alphaNumericPattern = /^\w+$/;

    //accepts a period, a comma, a space or a nonbreaking space as delimiter
    thisValidate.amountPattern = /^(\d+((\.|,|\s|\xA0)\d+)*)$/;

    thisValidate.dateYearPattern = /^\d{4}$/;
    thisValidate.dateMonthPattern = /^\d{2}$/;
    thisValidate.dateDayPattern = /^\d{2}$/;

    //thisValidate.emailPattern = /^\w+([\.\-\']?\w+)*@\w+([\.\-\']?\w+)*(\.\w{1,8})$/;
    thisValidate.emailPattern = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,8}$/;
};

var validateBySelector = function(selectorString) {
    var validate = null;
    var clickedObj = null;
    if (selectorString !== undefined) {
        // run validation on the form elements
        validate = new SKYSALES.Validate(null, '', SKYSALES.errorsHeader, null);
        validate.clickedObj = clickedObj;
        validate.runBySelector(selectorString);
        return validate.outputErrors();
    }
    return true;
};

var validate = function(controlID, elementName, filter) {
    var clickedObj = null;
    var validate = null;
    var e = null;
    //make sure we can run this javascript
    if (document.getElementById && document.createTextNode) {
        // check if you can getAttribute if you can it is an element use the id.
        if (controlID.getAttribute) {
            clickedObj = controlID;
            controlID = controlID.getAttribute("id").replace(/_\w+$/, "");
        }
        validate = new SKYSALES.Validate(SKYSALES.getSkySalesForm(), controlID + '_', SKYSALES.errorsHeader, filter);
        validate.clickedObj = clickedObj;

        if (elementName) {
            e = elementName;
            if (!elementName.getAttribute) {
                e = document.getElementById(controlID + "_" + elementName);
            }
            validate.validateSingleElement(e);
            return validate.outputErrors();
        }

        return validate.run();
    }
    // could not use javascript rely on server validation
    return true;
};


var preventDoubleClick = function() {
    return true;
};


var events = [];

var register = function(eventName, functionName) {
    if (events[eventName] === undefined) {
        events[eventName] = [];
    }
    events[eventName][events[eventName].length] = functionName;
};

var raise = function(eventName, eventArgs) {
    var ix = 0;
    if (events[eventName] !== undefined) {
        for (ix = 0; ix < events[eventName].length; ix += 1) {
            if (eval(events[eventName][ix] + "(eventArgs)") === false) {
                return false;
            }
        }
    }
    return true;
};

var WindowInitialize = function() {
    /*extern raise */
    var originalOnLoad = window.onload;
    //fix this up so initialize is synched with everything else
    var windowLoadFunction = function() {
        raise('WindowLoad', {});
        if (originalOnLoad) {
            originalOnLoad();
        }
        //added by Linson at 2012-01-05 begin
//        SKYSALES.LoadLoadingBar();
//        SKYSALES.RemoveLoadingBar();
//        document.onclick=function(e){
//            var _e=e||event||window.event;
//            var _src=_e.srcElement||_e.target;  
//            var ua = navigator.userAgent.toLowerCase(); 
//            if((_src.tagName == "A" && (_e.ctrlKey || _e.shiftKey)) 
//                || (ua.indexOf('safari')>0 && _src.tagName == "INPUT" && (_e.ctrlKey || _e.shiftKey) && _src.type=="submit")){ 
//                SKYSALES.RemoveLoadingBar();
//            }
//        }
//        document.onkeydown=function(e){
//            var _e=e||event||window.event;
//            var _curKey=_e.keyCode||_e.which||_e.charCode;  
//            if(_curKey==27){ 
//                SKYSALES.RemoveLoadingBar();
//            }        
//        }        
       //added by Linson at 2012-01-05 end
    };
    $(window).ready(windowLoadFunction);
};

SKYSALES.Util.displayPopUpConverter = function() {
    var url = 'CurrencyConverter.aspx';
    var converterWindow = window.converterWindow;
    if (!window.converterWindow || converterWindow.closed) {
        converterWindow = window.open(url, 'converter', 'width=360,height=220,toolbar=0,status=0,location=0,menubar=0,scrollbars=0,resizable=0');
    }
    else {
        converterWindow.open(url, 'converter', 'width=360,height=220,toolbar=0,status=0,location=0,menubar=0,scrollbars=0,resizable=0');
        if ($(converterWindow).is(':hidden') === false) {
            converterWindow.focus();
        }
    }
};

//Function to hide/show controls. Inputs are the control you want to hide and the dependent control 
//or which control is interacted with to trigger the hide/show of the other control.
var hideShow = function(hideControl, depControl) {
    var controlHide = hideControl;
    var controlDepend = depControl;

    if (document.getElementById && document.getElementById(hideControl)) {
        if (document.getElementById(controlDepend).checked === true) {
            document.getElementById(controlHide).style.display = "inline";
        }
        else {
            document.getElementById(controlHide).style.display = "none";
        }
    }
};
var jsLoadedCommon = true;

/* Page Behaviors */

SKYSALES.toggleAtAGlanceEvent = function() {
    $(this).next().slideToggle();
};
SKYSALES.toggleAtAGlance = function() {
    $("div.atAGlanceDivHeader").click(SKYSALES.toggleAtAGlanceEvent);
};

SKYSALES.initializeTime = function() {
    var i = 0;
    var timeOptions = "";
    for (i = 0; i < 23; i += 1) {
        timeOptions += "<option value=" + i + ">" + i + "</option>";
    }
    if (timeOptions !== "") {
        $("select.Time").append(timeOptions);
    }
};

$("a.animateMe").animate({
    height: 'toggle',
    opacity: 'toggle'
}, "slow");


SKYSALES.aosAvailabilityShow = function() {
    $(this).parent().find('div.hideShow').show('slow');
    return false;
};

SKYSALES.aosAvailabilityHide = function() {
    $(this).parent().parent('.hideShow').hide('slow');
    return false;
};

SKYSALES.dropDownMenuEvent = function() {
    $("div.slideDownUp").toggle('fast');
    return false;
};

SKYSALES.faqHideShow = function() {
    $(this).parent('dt').next('.accordianSlideContent').slideToggle("slow");
};

SKYSALES.equipHideShow = function() {
    $('div#moreSearchOptions').slideToggle("slow");
    return false;
};

SKYSALES.initializeAosAvailability = function() {
    /* AOS Availability */
    $('.hideShow').hide();
    $('a.showContent').click(SKYSALES.aosAvailabilityShow);
    $('a.hideContent').click(SKYSALES.aosAvailabilityHide);
    /* Drop-down menus */
    $('a.toggleSlideContent').click(SKYSALES.dropDownMenuEvent);

    $('a.accordian').click(SKYSALES.faqHideShow);


    $('a.showEquipOpt').click(SKYSALES.equipHideShow);
    $('a.hideEquipOpt').click(SKYSALES.equipHideShow);
};



//load metaobjects (used for validation)
SKYSALES.initializeMetaObjects = function() {
    $.metaobjects({ clean: false });
};

SKYSALES.common = new SKYSALES.Common();


function formatCurrency(value) {
    var cents = 0;
    value = value.toString();
    if (isNaN(value)) {
        value = '0';
    }
    if (value.indexOf('.') > -1) {
        cents = value.substring(value.indexOf('.') + 1, value.length);

        value = value * 100;
    }
    cents = value % 100;
    if (cents > 0) {
        value = Math.floor(value / 100).toString();
    }
    if (cents < 10) {
        cents = '0' + cents;
    }
    for (var i = 0; i < Math.floor((value.length - (1 + i)) / 3); i += 1) {
        value = value.substring(0, value.length - (4 * i + 3)) + ',' + value.substring(value.length - (4 * i + 3));
    }
    return (value + '.' + cents);
}


SKYSALES.Util.sendAspFormFields = function() {
    var clearAllValidity = null;
    var eventTargetElement = window.document.getElementById('eventTarget');
    var eventArgumentElement = window.document.getElementById('eventArgument');
    var viewStateElement = window.document.getElementById('viewState');
    var theForm = window.theForm;
    if (!theForm.onsubmit || (theForm.onsubmit() !== false)) {
        eventTargetElement.name = '__EVENTTARGET';
        eventArgumentElement.name = '__EVENTARGUMENT';
        viewStateElement.name = '__VIEWSTATE';
        if (theForm.checkValidity) {
            clearAllValidity = function() {
                $(this).removeAttr("required");
            };
            SKYSALES.common.getAllInputObjects().each(clearAllValidity);
        }
    }
    return true;
};

SKYSALES.Util.initStripeTable = function() {
    $('.hotelResult').hide();

    var stripeMeInputHandler = function() {
        $('.stripeMe tr').removeClass("over");
        $(this).parent().parent().addClass("over");
    };
    $('.stripeMe input').click(stripeMeInputHandler);
};

SKYSALES.Util.ready = function() {
    $('form').submit(SKYSALES.Util.sendAspFormFields);

    //Turn validation object tags into javascript, this must run before initializeCommon
    SKYSALES.initializeMetaObjects();

    //Initialize Common
    SKYSALES.common.initializeCommon();

    //Initialize objects
    SKYSALES.Util.initObjects();

    SKYSALES.initializeSkySalesForm();
    SKYSALES.toggleAtAGlance();
    SKYSALES.Util.initStripeTable();
    SKYSALES.initializeAosAvailability();
    //SKYSALES.RemoveLoadingBar();  //added by Linson at 2011-12-31
};
$(document).ready(SKYSALES.Util.ready);


// CUSTOM FUNCTIONS //

SKYSALES.openNewWindow = function(url, windowName, properties) {
    var newWindow = window.open(url, windowName, properties);
}

SKYSALES.tooltip = function() {
    var id = 'tt';
    var top = 0;
    var left = 23;
    var maxw = 700;
    var speed = 10;
    var timer = 20;
    var endalpha = 95;
    var alpha = 0;
    var tt, t, c, b, h;
    var ie = document.all ? true : false;
    return {
    show: function(v, w) {
        document.onmousemove = SKYSALES.tooltip.pos;
            if (tt == null) {
                tt = document.createElement('div');
                tt.setAttribute('id', id);
                t = document.createElement('div');
                t.setAttribute('id', id + 'left');
                c = document.createElement('div');
                c.setAttribute('id', id + 'cont');
                //bryan, set rounded corner
                //                c.setAttribute('style', "-moz-border-radius: 5px;border-radius: 5px;");
                b = document.createElement('div');
                b.setAttribute('id', id + 'right1');
                //                tt.appendChild(t);
                tt.appendChild(c);
                //                tt.appendChild(b);
                document.body.appendChild(tt);
                tt.style.opacity = 0;
                tt.style.filter = 'alpha(opacity=0)';
                
            }
            tt.style.display = 'block';
            c.innerHTML = v;
            tt.style.width = w ? w + 'px' : 'auto';
            c.style.width = w ? (w - 60) + 'px' : 'auto';
            c.style.height = 'auto';
            //            t.style.marginTop = '-' + ((c.style.height / 2)-20) + 'px';
            if (!w && ie) {
                t.style.display = 'none';
                b.style.display = 'none';
                tt.style.width = tt.offsetWidth;
                c.style.width = (tt.offsetWidth - 60);
                t.style.display = 'block';
                b.style.display = 'block';
            }
            if (tt.offsetWidth > maxw) { tt.style.width = maxw + 'px'; c.style.width = (maxw - 60) + 'px'; }
            h = (parseInt(tt.offsetHeight) + top) / 2;
            clearInterval(tt.timer);
            tt.timer = setInterval(function() { SKYSALES.tooltip.fade(1) }, timer);

            //            var abc = ((c.style.height / 2) - 20) + 'px';
            //            t.setAttribute('style', "margin-top:" + abc);
        },
        pos: function(e) {
                        var u = ie ? event.clientY + document.documentElement.scrollTop : e.pageY;
                        var l = ie ? event.clientX + document.documentElement.scrollLeft : e.pageX;
            tt.style.top = (u - h) + 'px';
            tt.style.left = (l + left) + 'px';
        },
        fade: function(d) {
            var a = alpha;
            if ((a != endalpha && d == 1) || (a != 0 && d == -1)) {
                var i = speed;
                if (endalpha - a < speed && d == 1) {
                    i = endalpha - a;
                } else if (alpha < speed && d == -1) {
                    i = a;
                }
                alpha = a + (i * d);
                tt.style.opacity = alpha * .01;
                tt.style.filter = 'alpha(opacity=' + alpha + ')';
            } else {
                clearInterval(tt.timer);
                if (d == -1) { tt.style.display = 'none' }
            }
        },
        hide: function() {
            clearInterval(tt.timer);
            tt.timer = setInterval(function() { SKYSALES.tooltip.fade(-1) }, timer);
        }
    };
} ();

SKYSALES.tooltip2 = function() {
    var id = 'tt2';
    var top = 0;
    var left = -120;
    var maxw = 700;
    var speed = 10;
    var timer = 20;
    var endalpha = 95;
    var alpha = 0;
    var tt2, t2, c2, b2, h2;
    var ie2 = document.all ? true : false;
    return {
    show: function(v2, w2) {
        document.onmousemove = SKYSALES.tooltip2.pos2;
            if (tt2 == null) {
                tt2 = document.createElement('div');
                tt2.setAttribute('id', id);
                //                t2 = document.createElement('div');
                //                t2.setAtt2ribute('id', id + 'left');
                c2 = document.createElement('div');
                c2.setAttribute('id', id + 'cont');
                //bryan, set rounded corner
                //                c2.setAtt2ribute('style', "-moz-border-radius: 5px;border-radius: 5px;");
                //                b2 = document.createElement('div');
                //                b2.setAtt2ribute('id', id + 'right1');
                //                tt2.appendChild(t2);
                tt2.appendChild(c2);
                //                tt2.appendChild(b2);
                document.body.appendChild(tt2);
                tt2.style.opacity = 0;
                tt2.style.filter = 'alpha(opacity=0)';
                
            }
            tt2.style.display = 'block';
            c2.innerHTML = v2;
            tt2.style.width = w2 ? w2 + 'px' : 'auto';
            c2.style.width = w2 ? (w2 - 60) + 'px' : 'auto';
            c2.style.height = 'auto';
            //            t2.style.marginTop = '-' + ((c2.style.height / 2)-20) + 'px';
            if (!w2 && ie2) {
                t2.style.display = 'none';
                b2.style.display = 'none';
                tt2.style.width = tt2.offsetWidth;
                c2.style.width = (tt2.offsetWidth - 60);
                t2.style.display = 'block';
                b2.style.display = 'block';
            }
            if (tt2.offsetWidth > maxw) { tt2.style.width = maxw + 'px'; c2.style.width = (maxw - 60) + 'px'; }
            h2 = (parseInt(tt2.offsetHeight) + top) / 2;
            clearInterval(tt2.timer);
            tt2.timer = setInterval(function() { SKYSALES.tooltip2.fade(1) }, timer);

            //            var abc = ((c2.style.height / 2) - 20) + 'px';
            //            t2.setAtt2ribute('style', "margin-top:" + abc);
        },
        pos2: function(e2) {
                        var u2 = ie2 ? event.clientY + document.documentElement.scrollTop : e2.pageY;
                        var l2 = ie2 ? event.clientX + document.documentElement.scrollLeft : e2.pageX;
            tt2.style.top = (u2 + 30) + 'px';
            tt2.style.left = (l2 + left) + 'px';
        },
        fade: function(d) {
            var a = alpha;
            if ((a != endalpha && d == 1) || (a != 0 && d == -1)) {
                var i = speed;
                if (endalpha - a < speed && d == 1) {
                    i = endalpha - a;
                } else if (alpha < speed && d == -1) {
                    i = a;
                }
                alpha = a + (i * d);
                tt2.style.opacity = alpha * .01;
                tt2.style.filter = 'alpha(opacity=' + alpha + ')';
            } else {
                clearInterval(tt2.timer);
                if (d == -1) { tt2.style.display = 'none' }
            }
        },
        hide: function() {
            clearInterval(tt2.timer);
            tt2.timer = setInterval(function() { SKYSALES.tooltip2.fade(-1) }, timer);
        }
    };
} ();

// SESSION EXPIRY NOTICE //
$(document).ready(function() { SKYSALES.startSessionTracking(); });
window.onbeforeunload = function() {
    // this will stop the session notice when the page has already been submitted.
    if (SKYSALES.sessionTracking) {
        SKYSALES.sessionTracking.sessionExpiry = 0;
    }
    //added by Linson at 2012-01-09 begin
    SKYSALES.ReDisplayLoadingBar();
    //added by Linson at 2012-01-09 end
}
SKYSALES.startSessionTracking = function() {
    if (SKYSALES.sessionTracking && parseInt(SKYSALES.sessionTracking.sessionExpiry) > 0) {
    setTimeout("SKYSALES.launchSessionNotice();", (((parseInt(SKYSALES.sessionTracking.sessionExpiry) * 60) - 30 - SKYSALES.sessionTracking.sessionExpiryCountdown) * 1000));
    }
}

$(window).resize(function() {
    if ($('#dimmer').length > 0) {
        $('#dimmer').css('width', '100%');
        $('#dimmer').css('height', '100%');
    }
    //added by Linson at 2012-01-05 begin
    if ($('#loadingDimmer').length > 0) {
        $('#loadingDimmer').css('width', '100%');
        $('#loadingDimmer').css('height', '100%');
    } 
    //added by Linson at 2012-01-05 end   
	//added by Linson at 2012-06-01 begin
    if ($('#blockDimmer').length > 0) {
        $('#blockDimmer').css('width', '100%');
        $('#blockDimmer').css('height', '100%');
    } 
    //added by Linson at 2012-06-01 end       
});

SKYSALES.launchSessionNotice = function() {
    if (SKYSALES.sessionTracking && parseInt(SKYSALES.sessionTracking.sessionExpiry) > 0) {
        if ($('#dimmer').length > 0) {
            $('#dimmer').remove();
            $('#sessionNoticeBox').remove();
        }
        $('body').append('<div id="dimmer" style="display: block; position: fixed; z-index:998;"></div><div id="sessionNoticeBox"><span id="sessionNoticeText">' + SKYSALES.sessionTracking.title + '<br/>' + SKYSALES.sessionTracking.noticeText1 + '<span id="sessionNoticeCount">' + SKYSALES.sessionTracking.sessionExpiryCountdown + '</span>' + SKYSALES.sessionTracking.noticeText2 + '<br/>' + SKYSALES.sessionTracking.noticeText3 + '</span><br/><br/><input id="sessionNoticeOK" type="button" value=" OK " /></div>');
        $('#dimmer').show();
        $('#sessionNoticeBox').show();
//        $('#dimmer').css('width', ($(document).width()) + 'px');
//        $('#dimmer').css('height', ($(document).height()) + 'px');
    	//Edited by Sean on 27 July 2012 start
    	$('#dimmer').css('width', '100%');
        $('#dimmer').css('height', '100%');
    	//Edited by Sean on 27 July 2012 end
        SKYSALES.sessionNoticeCountdownTick();
        window.focus();
        $('#sessionNoticeOK').focus();
        SKYSALES.windowTitle = document.title;
        document.title = ' ';
        $('#sessionNoticeOK').click(function() {
            sCount = 0;
            if ($('#sessionNoticeCount').length > 0) {
                sCount = parseInt($('#sessionNoticeCount').html());
            }
            $('#dimmer').remove();
            $('#sessionNoticeBox').remove();
            document.title = SKYSALES.windowTitle;
            if (sCount > 0) {
                $.post('SessionRefresh.aspx');
                SKYSALES.startSessionTracking();
            } else {
                showMsg = false; // this gets rid of the popup when leaving the Traveler page.
                window.location = 'Search.aspx';
            }
        });
    }
}

SKYSALES.sessionNoticeCountdown = function() {
    sCount = 0;
    if ($('#sessionNoticeCount').length > 0) {
        sCount = parseInt($('#sessionNoticeCount').html());
    }
    if (sCount > 0) {
        sCount = sCount - 1;
        $('#sessionNoticeCount').html(sCount);
        if (sCount > 0) {
            SKYSALES.sessionNoticeCountdownTick();

            if (document.title != SKYSALES.windowTitle) {
                document.title = SKYSALES.windowTitle;
            }
            else {
                document.title = SKYSALES.sessionTracking.noticeWindowTitle;
            }

            window.focus();
            $('#sessionNoticeOK').focus();
        }
    }
    if (sCount == 0 && $('#sessionNoticeText').length > 0) {
        $('#sessionNoticeText').html(SKYSALES.sessionTracking.expiredText);
    }
}

SKYSALES.sessionNoticeCountdownTick = function() {
    setTimeout("SKYSALES.sessionNoticeCountdown();", 1000);
}
//added by Linson at 2011-12-30 begin   Description:add loading layer for SkySales Booking Pages.  
SKYSALES.LoadLoadingBar=function() {
    //    if ($('#loadingDimmer').length > 0) {
    //        $('#loadingDimmer').remove();
    //        $('#loadingBarBox').remove();
    //    }
    //    //var txtNoticeText = "Please wait while the page loads";
    //    var imgSrc = "images/AKBase/loading_circle.gif";
    //    $('body').append('<div id="loadingDimmer"></div><div id="loadingBarBox"><img id="loadingImage" src="' + imgSrc + '" /></div>');  
    //    $('#loadingDimmer').hide();
    //    $('#loadingBarBox').hide();
    //    $('#loadingDimmer').css('width', '100%');
    //    $('#loadingDimmer').css('height', '100%');
}
 
SKYSALES.DisplayLoadingBar = function() {
    $('#loadingDimmer').show();
    $('#loadingBarBox').show();
    window.focus();
    $('#loadingImage').focus();        
    return true;
}

SKYSALES.RemoveLoadingBar = function() {
    if ($('#loadingDimmer').length > 0) {
        $('#loadingDimmer').hide();
        $('#loadingBarBox').hide();
    }
}

SKYSALES.ReDisplayLoadingBar=function(){
    if ($('#loadingImage').length > 0 && document.getElementById('loadingBarBox').style.display!='none') {
        if(navigator.userAgent.toUpperCase().indexOf("MSIE")>0){
            SKYSALES.LoadLoadingBar();
        }   
        setTimeout("SKYSALES.DisplayLoadingBar();", 10);
    }
    //    if ($('#loadingDimmer').length > 0) {
    //        $('#loadingDimmer').css('width', '100%');
    //        $('#loadingDimmer').css('height', '100%');
    //    }
}
//added by Linson at 2011-12-30 end

// SEARCH PAGE //
SKYSALES.loginDisp = "show";
SKYSALES.toggleLogin = function() {

    if (SKYSALES.loginDisp == "show") {
        $('#loginControl').removeClass('login_expand_icon');
        $('#loginControl').addClass('login_collapse_icon');
        $('#loginContainer').show('slide');
        SKYSALES.loginDisp = "hide";
    }
    else {
        $('#loginControl').removeClass('login_collapse_icon');
        $('#loginControl').addClass('login_expand_icon');
        $('#loginContainer').hide('slide');
        SKYSALES.loginDisp = "show";
    }
};

function toggleOneWayOnly(toggleControlId, oneWayControlId, roundTripControlId) {
    if ($('#' + toggleControlId).length > 0) {
        if ($('#' + toggleControlId).attr('checked')) {
            $('#' + oneWayControlId).click();
        }
        else {
            $('#' + roundTripControlId).click();
        }
    }
};

// SELECT PAGE //
SKYSALES.selectedFareCellId = new Array();
SKYSALES.selectedFareRowId = new Array();
SKYSALES.selectedFareColumnId = new Array();
SKYSALES.selectedFareCellId[1] = '';
SKYSALES.selectedFareCellId[2] = '';
SKYSALES.selectedFareRowId[1] = '';
SKYSALES.selectedFareRowId[2] = '';
SKYSALES.selectedFareColumnId[1] = '';
SKYSALES.selectedFareColumnId[2] = '';
SKYSALES.hiliteFare = function(marketIndex, fareRadioId, fareRowId, fareColumnId, carrierCode, fareClass) {
    if ($('#' + fareRadioId).length > 0) {
        $('#' + fareRadioId).attr('checked', true);
        $('#' + fareRadioId).click();
    }
    fareCellId = fareRadioId + 'CellId';
    if ($('#' + fareCellId).length > 0) {
        if ($('#' + SKYSALES.selectedFareCellId[marketIndex])) {
            $('#' + SKYSALES.selectedFareCellId[marketIndex]).removeClass('resultFareHilite' + fareClass);
            $('#' + SKYSALES.selectedFareCellId[marketIndex]).removeClass('selected');
            $('#' + SKYSALES.selectedFareCellId[marketIndex]).addClass('resultFare' + fareClass);
        }
        $('#' + fareCellId).removeClass('resultFare' + fareClass);
        $('#' + fareCellId).addClass('resultFareHilite' + fareClass);
        $('#' + fareCellId).addClass('selected');
        SKYSALES.selectedFareCellId[marketIndex] = fareCellId;
    }
    if ($('#' + fareRowId).length > 0) {
        if ($('#' + SKYSALES.selectedFareRowId[marketIndex])) {
            $('#' + SKYSALES.selectedFareRowId[marketIndex]).removeClass('selectedAirlineCodeHilite');
            $('#' + SKYSALES.selectedFareRowId[marketIndex]).addClass('selectedAirlineCode');
        }
        $('#' + fareRowId).removeClass('selectedAirlineCode');
        $('#' + fareRowId).addClass('selectedAirlineCodeHilite');
        SKYSALES.selectedFareRowId[marketIndex] = fareRowId;
    }
    if ($('#' + fareColumnId).length > 0) {
        if ($('#' + SKYSALES.selectedFareColumnId[marketIndex])) {
            $('#' + SKYSALES.selectedFareColumnId[marketIndex]).removeClass('resultClassHilite');
            $('#' + SKYSALES.selectedFareColumnId[marketIndex]).addClass('resultClass');
        }
        $('#' + fareColumnId).removeClass('resultClass');
        $('#' + fareColumnId).addClass('resultClassHilite');
        SKYSALES.selectedFareColumnId[marketIndex] = fareColumnId;
    }
}

SKYSALES.changeTandCLink = function(carrierCode) {
    if ($('#termsAndConditionsLink')) {
        $('#termsAndConditionsLink')[0].href = 'tandc_' + carrierCode + '.html';
    }
}

SKYSALES.openTermsAndConditions = function(carrierCode, language) {
    var language_path = SKYSALES.AK_defineLanguagePath(language);
    var URL1 = "http://www.airasia.com/" + language_path + "/tnc_main.html";
    var termsWindow = window.open(URL1, 'TermsAndConditions', 'width=600,height=500,toolbar=0,status=0,menubar=0,scrollbars=1,resizable=No');
}

SKYSALES.openFareRules = function(language) {
    var language_path = SKYSALES.AK_defineLanguagePath(language);
    var URL = 'http://www.airasia.com/' + language_path + '/farerules.html ';
    var termsWindow = window.open(URL, 'FareRules', 'width=600,height=500,toolbar=0,status=0,menubar=0,scrollbars=1,resizable=No');
}

SKYSALES.openPrivacyPolicy = function(language) {
    var language_path = SKYSALES.AK_defineLanguagePath(language);
    var URL = "http://www.airasia.com/" + language_path + "/privacypolicy.page";
    var termsWindow = window.open(URL, 'PrivacyPolicy', 'width=600,height=500,toolbar=0,status=0,menubar=0,scrollbars=1,resizable=No');
}

SKYSALES.AK_defineLanguagePath = function(language) {
    switch (language) {
        case "bm":
            language_path = "my/bm";
            break;
        case "tw":
            language_path = "tw/tw";
            break;
        case "ae":
            language_path = "ae/ae";
            break;
        case "ch":
            language_path = "hk/tc";
            break;
        case "cn":
            language_path = "ch/ch";
            break;
        case "en":
            language_path = "my/en";
            break;
        case "id":
            language_path = "id/id";
            break;
        case "th":
            language_path = "th/th";
            break;
        case "vn":
            language_path = "vn/vn";
            break;
        case "ko":
            language_path = "kr/ko";
            break;
        case "ja":
            language_path = "jp/ja";
            break;
        case "fa":
            language_path = "ir/fa";
            break;
        default:
            language_path = "my/en";
    }
    return language_path;
}

/* PAYMENT PAGE */
SKYSALES.togglePriceSummary = function(priceSummaryType) {

    var priceSummaryId = '';

    if (priceSummaryType == 'OtherTaxesAndFees') {
        priceSummaryId = '#OtherTaxesAndFees';
    }
    else if (priceSummaryType == 'PriceMarket1') {
        priceSummaryId = '#PriceMarket1';
    }
    else if (priceSummaryType == 'PriceMarket2') {
        priceSummaryId = '#PriceMarket2';
    }

    if ($(priceSummaryId).hasClass('expand_icon')) {
        $(priceSummaryId).removeClass('expand_icon');
        $(priceSummaryId).addClass('collapse_icon');
    }
    else {
        $(priceSummaryId).removeClass('collapse_icon');
        $(priceSummaryId).addClass('expand_icon');
    }
};

/* Compact search AAGo link redirection (Added by siaoshan on 19 Aug 2010-Start)*/

SKYSALES.openAAGo = function(AAGoURL) {

    var countryName = "";
    var languageName = "";
    var countryCode = "";
    var languageCode = "";
    
    var myPath = window.location.href;
    if (myPath != null)
    {
        var mySplit = myPath.split("/");
        if( mySplit[3] != null )
        {
          countryCode = mySplit[3];
          countryCode = countryCode.toUpperCase();
        }
        if( mySplit[4] != null )
        {
          languageCode = mySplit[4];
          languageCode = languageCode.toUpperCase();
        }
    }
    
  switch (countryCode) {
        case "AU":
            countryName = "AUSTRALIA";
            break;
        case "BN":
            countryName = "BRUNEI";
            break;
        case "CN":
            countryName = "CHINA"
            break;
        case "HK":
            countryName = "HONGKONG";
            break;
        case "IN":
            countryName = "INDIA";
            break;
        case "ID":
            countryName = "INDONESIA";
            break;
        case "MO":
            countryName = "MACAU";
            break;
        case "MY":
            countryName = "MALAYSIA";
            break;
        case "PH":
            countryName = "PHILLIPPINES";
            break;
        case "SG":
            countryName = "SINGAPORE";
            break;
        case "KR":
            countryName = "KOREA";
            break;
        case "TW":
            countryName = "TAIWAN";
            break;
        case "TH":
            countryName = "THAILAND";
            break;
        case "GB":
            countryName = "UK";
            break;
        case "JP":
            countryName = "JAPAN";
            break;
        case "FR":
            countryName = "FRANCE";
            break;
        case "NZ":
            countryName = "NEWZEALAND";
            break;
        default:
            countryName = "ROW";
    }
    
     switch (languageCode) {
        case "EN":
            languageName = "en";
            break;
        case "ZH":
            languageName = "zz";
            break;
        case "ID":
            languageName = "id";
            break;
        case "MS":
            languageName = "ms";
            break;
        case "KO":
            languageName = "kr";
            break;
        case "TH":
            languageName = "th";
            break;
        case "JA":
            languageName = "jp";
            break;
        default:
            languageName = "en";
     }
    
    if ((countryCode == "CN") && (languageCode == "ZH"))
    {
        languageName = "zh";
    }
    
    var URL1 = AAGoURL + languageName + countryName;

    var termsWindow = window.open(URL1, "_self", '');

};

SKYSALES.get_url_param = function(name){
  name = name.replace(/[\[]/,"\\\[").replace(/[\]]/,"\\\]");
  var regexS = "[\\?&]"+name+"=([^&#]*)";
  var regex = new RegExp( regexS );
  var results = regex.exec( window.location.href );
  
  if( results == null )
  {
    return "" ;
  }
  else
  {
      return results[1];
  }
};

/* Compact search AAGo link redirection (Added by siaoshan on 19 Aug 2010-End)*/

/* Show/Hide DIV (Added by Michelle on 29OCT2010 -start)*/
SKYSALES.ShowContent = function(ShowContent) {
    document.getElementById(d).style.display = "block";
}
/* Show/Hide DIV (Added by Michelle on 29OCT2010 -end)*/

/* End common.js */
/* Start AvailabilityInput.js */
/*!
This file is part of the Navitaire NewSkies application.
Copyright (C) Navitaire.  All rights reserved.
*/

/*
Dependencies:
This file depends on other JavaScript files to be there at run time.
        
jquery.js:
$ is a jquery variable
common.js:
SKYSALES namespace is used to avoid name collisions.
TaxAndFeeInclusiveDisplay.js
Calls taxAndFeeInclusiveDisplayDataRequestHandler
    
General Notes:
This JavaScript is used with the AvailabilityInput control
        
JsLint Status:
Pass - JsLint Edition 2008-06-04
        
+ Strict whitespace
+ Assume a browser 
+ Disallow undefined variables
+ Disallow leading _ in identifiers
+ Disallow == and !=
+ Disallow ++ and --
+ Disallow bitwise operators
+ Disallow . in RegExp literals
+ Disallow global var
Indentation 4
        
*/

/*extern $ jQuery window SKYSALES */

/*
Name: 
Class AvailabilityInput
Param:
None
Return: 
An instance of AvailabilityInput
Functionality:
The object that initializes the AvailabilityInput control
Notes:
The journeyInfoList is set down to the browser via JSON that is created in the XSLT file.
The XSLT creates a new SKYSALES.Class.AvailabilityInput and then sets the SKYSALES.availabilityInput.journeyInfoList
property to an array of JourneyInfo objects. It then calls SKYSALES.availabilityInput.init
This class also attemps to call taxAndFeeInclusiveDisplayDataRequestHandler that is in TaxAndFeeInclusiveDisplay.js
Class Hierarchy:
AvailabilityInput
*/
if (SKYSALES.Class.AvailabilityInput === undefined) {
    SKYSALES.Class.AvailabilityInput = function() {
        var thisAvailabilityInput = this;

        thisAvailabilityInput.detailsLinks = null;
        thisAvailabilityInput.journeyInfoArray = [];
        thisAvailabilityInput.productClasses = [];
        thisAvailabilityInput.fareClasses = [];
        thisAvailabilityInput.fareCategories = [];
        thisAvailabilityInput.changeLimits = [];
        thisAvailabilityInput.departureDates = [null, null];
        thisAvailabilityInput.submitButtonId = '';
        thisAvailabilityInput.restrictSameFareCategory = '';
        thisAvailabilityInput.sameCategoryMessage = '';
        thisAvailabilityInput.agreement24HrMessage = '';
        thisAvailabilityInput.incompleteFareSelection = '';
        thisAvailabilityInput.isNewBooking = 'true';
        thisAvailabilityInput.mostRestrictiveCutOff = 0;
        thisAvailabilityInput.isWithinCutOffHours = [false, false];
        thisAvailabilityInput.WithinTimeLimit = 120; // in minutes
        thisAvailabilityInput.isWithinTimeLimit = [false, false];
        thisAvailabilityInput.cutOffTime = null;
        thisAvailabilityInput.agreement24HrsId = '';
        thisAvailabilityInput.agreement24HrsCheckboxId = '';
        thisAvailabilityInput.agreementCutOffHrsNoteId = '';
        thisAvailabilityInput.taxAndFeeProgressMessage = '';
        thisAvailabilityInput.infantAgeText = '';
        thisAvailabilityInput.submitButton = null;
        thisAvailabilityInput.isExemptedFromCutoff = null;
        thisAvailabilityInput.hideMarketIndex = '';
        thisAvailabilityInput.marketCount = null;
        thisAvailabilityInput.preselectFares = null;
        thisAvailabilityInput.clientId = '';
        thisAvailabilityInput.hiflyerFares = null;
        thisAvailabilityInput.isFlightChange = false;
        thisAvailabilityInput.marketDatesMessage = '';
        thisAvailabilityInput.userSelectedDateTime = [];
        thisAvailabilityInput.infantArray = [];

        thisAvailabilityInput.addEvents = function() {
            thisAvailabilityInput.addGetPriceItineraryInfoEvents();
            thisAvailabilityInput.addEquipmentPropertiesAjaxEvents();
            thisAvailabilityInput.submitButton.click(thisAvailabilityInput.validateEvent);
            thisAvailabilityInput.addFareCategoryDisplayEvents();
            thisAvailabilityInput.agreementCheckBoxEvents();
        };
        thisAvailabilityInput.setVars = function() {
            var clientIdArray = (thisAvailabilityInput.clientId).split("_");
            thisAvailabilityInput.submitButtonId = clientIdArray[0] + '_ButtonSubmit';
            thisAvailabilityInput.detailsLinks = $('.showContent');
            thisAvailabilityInput.submitButton = $('#' + thisAvailabilityInput.submitButtonId);

        };

        thisAvailabilityInput.agreementCheckBoxEvents = function() {
            if ($('#ControlGroupSelectView_AgreementInputSelectView_CheckBoxAgreement').length > 0) {
                $('#ControlGroupSelectView_AgreementInputSelectView_CheckBoxAgreement').click(function() {
                    $('#validationErrorContainerReadAlong').hide();
                    $('#validationErrorContainerReadAlongIFrame').hide();
                });
            }
            if ($('#ControlGroupSelectChangeView_AgreementInputSelectChangeView_CheckBoxAgreement').length > 0) {
                $('#ControlGroupSelectChangeView_AgreementInputSelectChangeView_CheckBoxAgreement').click(function() {
                    $('#validationErrorContainerReadAlong').hide();
                    $('#validationErrorContainerReadAlongIFrame').hide();
                });
            }
        }

        thisAvailabilityInput.addFareCategoryDisplayEvents = function() {
            for (j = 1; j <= 2; j++) {
                for (i = 0; i < this.fareCategories.length; i++) {
                    if ($('#icon' + this.fareCategories[i].fareCategoryName + j).length > 0
                    && $('#description' + this.fareCategories[i].fareCategoryName).length > 0) {
                        $('#icon' + this.fareCategories[i].fareCategoryName + j).addClass('pointer');
                        $('#icon' + this.fareCategories[i].fareCategoryName + j).mouseover(function() {
                            thisAvailabilityInput.FareCategoryDisplayMouseover(this);
                        });
                        $('#icon' + this.fareCategories[i].fareCategoryName + j).mouseout(function() {
                            thisAvailabilityInput.FareCategoryDisplayMouseout(this);
                        });
                    }
                }
            }
        }
        thisAvailabilityInput.FareCategoryDisplayMouseover = function(caller) {
            if (!caller.classList) {
                caller.classList = caller.className.split(' ');
            }
            if ($('#description' + caller.classList[0]).length > 0) {
                var fareDescTop = parseInt($('#icon' + caller.classList[1]).offset().top + 35) + "px";
                var fareDescLeft = parseInt($('#icon' + caller.classList[1]).offset().left - 70) + "px";
                $('#description' + caller.classList[0]).css({
                    "top": fareDescTop,
                    "left": fareDescLeft
                });
                $('#description' + caller.classList[0]).show();
            }

        }
        thisAvailabilityInput.FareCategoryDisplayMouseout = function(caller) {
            if (!caller.classList) {
                caller.classList = caller.className.split(' ');
            }
            if (caller.classList) {
                if ($('#description' + caller.classList[0]).length > 0) {
                    $('#description' + caller.classList[0]).hide();
                }
            }
        }
        thisAvailabilityInput.initJourneyInfoContainers = function() {
            var i = 0;
            var journeyInfoList = this.journeyInfoList;
            var journeyInfo = null;
            for (i = 0; i < journeyInfoList.length; i += 1) {
                journeyInfo = new SKYSALES.Class.JourneyInfo();
                journeyInfo.init(journeyInfoList[i]);
                thisAvailabilityInput.journeyInfoArray[thisAvailabilityInput.journeyInfoArray.length] = journeyInfo;
            }
        };

        thisAvailabilityInput.getPriceItineraryInfo = function() {
            if (undefined !== SKYSALES.taxAndFeeInclusiveDisplayDataRequestHandler) {
                var markets = $("#selectMainBody div[@class^='availabilityInputContent'] .rgMasterTable tr td[@class^='resultFareCell'] div[@class^='resultFare'] div[@id^='fareRadio'] :radio[@checked]");
                var keys = [];
                $(markets).each(function(i) {
                    var marketvars = $(this).val().split('@');
                    keys[i] = marketvars[0];
                });

                SKYSALES.taxAndFeeInclusiveDisplayDataRequestHandler(keys, markets.length, thisAvailabilityInput.marketCount, thisAvailabilityInput.hideMarketIndex, thisAvailabilityInput.taxAndFeeProgressMessage);
                SKYSALES.fareRuleHandler(keys, markets.length);

            }
        };

        thisAvailabilityInput.updateFareInfo = function() {
            var markets = $("#selectMainBody div[@class^='availabilityInputContent'] .rgMasterTable tr td[@class^='resultFareCell'] div[@class^='resultFare'] div[@id^='fareRadio'] :radio[@checked]");
            // If agreementInput div is hidden, let's show it and ensure that the checkbox is validated with the 'required'.
            if ($('#agreementInput').is(":hidden") === true) {
                $('#agreementInput').show();
                if ($('#ControlGroupSelectView_AgreementInputSelectView_CheckBoxAgreement').length > 0) {
                    $('#ControlGroupSelectView_AgreementInputSelectView_CheckBoxAgreement').attr("required", "true");
                }
                else {
                    $('#ControlGroupSelectChangeView_AgreementInputSelectChangeView_CheckBoxAgreement').attr("required", "true");
                }
            }
            $(markets).each(function(i) {
                var UTCvars = $(this).val().split('@');
                var marketvars = UTCvars[0].split('~');
                var rightnow = new Date();
                var tempdate = new Date();

                thisAvailabilityInput.fareClasses[i] = this.getAttribute("productClass");
                //this is for direct flights
                //if(marketvars.length == 14)                     
                //thisAvailabilityInput.departureDates[i] = new Date(marketvars[11]);
                thisAvailabilityInput.departureDates[i] = new Date(UTCvars[1]);
                //this is for connecting flights: length is 27
                //else
                //thisAvailabilityInput.departureDates[i] = new Date(marketvars[26]);
                //tempdate.setDate(rightnow.getDate() + 1); //24hours checking
                //tempdate.setTime(rightnow.getTime() + (1000*3600*12)); //old 12hours checking
                //tempdate.setHours(rightnow.getHours() + 12 + (rightnow.getTimezoneOffset()/60)); //new 12 hours checking


                // Determine the "real" index of the flight in this iteration, taking into consideration cases where only one
                // flight is being changed on a roundtrip booking.
                var currFlightIndex;
                if (thisAvailabilityInput.hideMarketIndex == "") {
                    currFlightIndex = i;
                }
                else if (thisAvailabilityInput.hideMarketIndex == 1) {
                    currFlightIndex = 0;
                }
                else {
                    currFlightIndex = 1;
                }

                // The minimum time before departure (in hours) that a flight should be before it can be selected.
                var flightBookLimit = thisAvailabilityInput.cutOffTime;

                // Apply a 4 hour checking during initial booking. During change booking, use the change limit set in AKSettings.xml
                if (thisAvailabilityInput.changeLimits.length > 0) {
                    flightBookLimit = parseInt(thisAvailabilityInput.changeLimits[currFlightIndex].changeLimit);
                }
                tempdate.setHours(rightnow.getHours() + flightBookLimit + (rightnow.getTimezoneOffset() / 60));

                thisAvailabilityInput.isWithinCutOffHours[i] = false;
                if ((thisAvailabilityInput.departureDates[i] < tempdate) && ((thisAvailabilityInput.hideMarketIndex == "") || (thisAvailabilityInput.hideMarketIndex != i))) {
                    thisAvailabilityInput.isWithinCutOffHours[i] = true;
                    if (flightBookLimit > thisAvailabilityInput.mostRestrictiveCutOff) {
                        thisAvailabilityInput.mostRestrictiveCutOff = flightBookLimit;
                    }
                }
                var timediff = thisAvailabilityInput.departureDates[i].getTime() - rightnow.getTime();
                timediff = Math.ceil(timediff / (1000 * 60));
                thisAvailabilityInput.isWithinTimeLimit[i] = false;
                if (timediff != NaN) {
                    if ((timediff <= thisAvailabilityInput.WithinTimeLimit) && ((thisAvailabilityInput.hideMarketIndex == "") || (thisAvailabilityInput.hideMarketIndex != i))) {
                        thisAvailabilityInput.isWithinTimeLimit[i] = true;
                    }
                }
            });
            thisAvailabilityInput.update24HrCheckbox();
            thisAvailabilityInput.updateCutOffHrNote();
        };

        thisAvailabilityInput.update24HrCheckbox = function() {
            if (thisAvailabilityInput.isNewBooking == 'false') {
                var hideCheckbox = true;
                hideCheckbox = (!thisAvailabilityInput.isWithinCutOffHours[0]) && (!thisAvailabilityInput.isWithinCutOffHours[1]);
                if (hideCheckbox == true) {
                    $('#' + thisAvailabilityInput.agreement24HrsId).hide();
                }
                else {
                    $('#' + thisAvailabilityInput.agreement24HrsId).show();
                }
            }
        };

        checkIfHiFlyer = function(ctr) {
            var hiflyerForModifyBooking = thisAvailabilityInput.isFlightChange == true ? (thisAvailabilityInput.hiflyerFares != null && thisAvailabilityInput.hiflyerFares.length > 0) : false;

            if (!hiflyerForModifyBooking)
                return true;

            for (var i = 0; i < thisAvailabilityInput.hiflyerFares.length; i++) {
                if (thisAvailabilityInput.hiflyerFares[i] == ctr)
                    return true;
            }

            return false;
        };

        thisAvailabilityInput.updateCutOffHrNote = function() {
            var hideNote = true;
            var isHiFlyer = false;
            if (thisAvailabilityInput.hideMarketIndex == '0') {
                hideNote = (!thisAvailabilityInput.isWithinCutOffHours[1]);
                isHiFlyer = checkIfHiFlyer(1);
            }
            else if (thisAvailabilityInput.hideMarketIndex == '1') {
                hideNote = (!thisAvailabilityInput.isWithinCutOffHours[0]);
                isHiFlyer = checkIfHiFlyer(0);
            }
            else {
                hideNote = (!thisAvailabilityInput.isWithinCutOffHours[0]) && (!thisAvailabilityInput.isWithinCutOffHours[1]);
                isHiFlyer = checkIfHiFlyer(0) && checkIfHiFlyer(1);
            }

            var isEligibleWithin24Hrs = thisAvailabilityInput.isExemptedFromCutoff && isHiFlyer;

            // only web anonymous and member roles are not exempted in the 24hour rule
            if (!hideNote && isEligibleWithin24Hrs == false) {
                $('#' + thisAvailabilityInput.submitButtonId).hide();
                $('#agreementInputCheckbox').hide();
                $('#SpecialNeedssection').hide();
                $('#' + thisAvailabilityInput.agreementCutOffHrsNoteId).html($('#' + thisAvailabilityInput.agreementCutOffHrsNoteId).html().replace(/~hours~/g, thisAvailabilityInput.mostRestrictiveCutOff));
                $('#' + thisAvailabilityInput.agreementCutOffHrsNoteId).show();
                thisAvailabilityInput.mostRestrictiveCutOff = 0;
            }

            else {
                $('#' + thisAvailabilityInput.agreementCutOffHrsNoteId).hide();
                $('#agreementInput').show();
                $('#agreementInputCheckbox').show();
                $('#SpecialNeedssection').show();
                $('#' + thisAvailabilityInput.submitButtonId).show();
            }

        };

        thisAvailabilityInput.showPreselectedFares = function(keyArray) {
            for (var keyIndex = 0; keyIndex < keyArray.length; keyIndex += 1) {
                if (keyArray[keyIndex] !== null) {
                    $("#" + keyArray[keyIndex]).click();
                }
            }
        };
        thisAvailabilityInput.setPreselectedFares = function() {
            var fareColumnCells = $("#selectMainBody div[@class^='availabilityInputContent'] .rgMasterTable tr td[@class^='resultFareCell'] div[@class^='resultFare'] div[@id^='fareRadio']");
            var markets = fareColumnCells.find(":radio[@checked]").length;
            if (markets === 0) {
                $("#taxAndFeeInclusiveDivBody").remove();
            }
            else if (markets === 1) {
                $("#taxesAndFeesInclusiveDisplay_2").hide();
            }

            var marketOneLowPrice = -1;
            var marketTwoLowPrice = -1;
            var marketOneID = "";
            var marketTwoID = "";
            var price = -1;
            var tabId1 = $('#' + this.clientId + '_HiddenFieldTabIndex1').val();
            var tabId2 = $('#' + this.clientId + '_HiddenFieldTabIndex2').val();
            var cellID = 0;

            //for one product class
            if ($("td[@class^='resultFareCell1']").length !== 0) {
                cellID = 1;
            } //The class for the fare used is different in initial and retrieve Booking added code to match class.
            else if ($("td[@class^='resultFareCell2']").length !== 0) {
                cellID = 2;
            }
            else if ($("td[@class^='resultFareCell3']").length !== 0) {
                cellID = 3;
            }
            else {
                cellID = 4;
            }

            $("td[@class^='resultFareCell" + cellID + "']", $('#fareTable1_' + tabId1)).each(
                function() {
                    var td = $(this);
                    var radioId = td.find(":radio[@name$='market1']").attr("id");
                    if (radioId) {
                        price = $.trim(td.find("div[@class='price']").find("span").text());
                        //var tempPrice = price.substr(2);
                        // Remove comma to determine the correct numeric value
                        price = parseFloat(price.replace(/[^0-9\.]+/g, ''));
                        //TODO: This only works for the invariant or US culture
                        //Fix it

                        if (!isNaN(price)) {
                            if (price < marketOneLowPrice || marketOneLowPrice < 0) {
                                marketOneLowPrice = price;
                                marketOneID = radioId;
                            }
                        }
                    }
                });

            $("#" + marketOneID).attr("checked", "checked");

            $("td[@class^='resultFareCell" + cellID + "']", $('#fareTable2_' + tabId2)).each(
                function() {
                    var td = $(this);
                    var radioId = td.find(":radio[@name$='market2']").attr("id");
                    if (radioId) {
                        price = $.trim(td.find("div[@class='price']").find("span").text());
                        //var tempPrice = price.substr(2);
                        price = parseFloat(price.replace(/[^0-9\.]+/g, ''));
                        //TODO: This only works for the invariant or US culture
                        //Fix it
                        //DUPLICATED CODE: from above anonymous function
                        //FIX it
                        if (!isNaN(price)) {
                            if (price < marketTwoLowPrice || marketTwoLowPrice < 0) {
                                marketTwoLowPrice = price;
                                marketTwoID = radioId;
                            }
                        }
                    }
                });
            $("#" + marketTwoID).attr("checked", "checked");

            thisAvailabilityInput.getPriceItineraryInfo();
            thisAvailabilityInput.updateFareInfo();

            var keyArray = [];

            keyArray[0] = marketOneID;
            keyArray[1] = marketTwoID;

            thisAvailabilityInput.showPreselectedFares(keyArray);
        };
        thisAvailabilityInput.addGetPriceItineraryInfoEvents = function() {
            $("#selectMainBody div[@class^='availabilityInputContent'] table[@id^='fareTable1'] tr td[@class^='resultFareCell'] div[@class^='resultFare'] div[@id^='fareRadio'] :radio").each(
                function() {
                    $(this).click(
                        function() {
                            thisAvailabilityInput.getPriceItineraryInfo();
                            thisAvailabilityInput.updateFareInfo();
                        });
                });
            $("#selectMainBody div[@class^='availabilityInputContent'] table[@id^='fareTable2'] tr td[@class^='resultFareCell'] div[@class^='resultFare'] div[@id^='fareRadio'] :radio").each(
                function() {
                    $(this).click(
                        function() {
                            thisAvailabilityInput.getPriceItineraryInfo();
                            thisAvailabilityInput.updateFareInfo();
                        });
                });
        };
        thisAvailabilityInput.getFareCategory = function(productclass) {
            var myFareCategoryName = '';
            for (i = 0; i < thisAvailabilityInput.fareCategories.length; i++) {
                if (thisAvailabilityInput.fareCategories[i].productClasses.indexOf('[' + productclass + ']') > -1) {
                    //myFareCategoryName = thisAvailabilityInput.fareCategories[i].fareCategoryName.replace('Economy', '').replace('Premium', '');
                    myFareCategoryName = thisAvailabilityInput.fareCategories[i].fareCategoryName;
                }
            }
            return myFareCategoryName;
        }
        thisAvailabilityInput.checkInfantAge = function() {
            if (thisAvailabilityInput.userSelectedDateTime != null) {
                var returnMarket = thisAvailabilityInput.userSelectedDateTime[thisAvailabilityInput.userSelectedDateTime.length - 1].selectedFareDateTimeSchedSelect,
                departMarket = thisAvailabilityInput.userSelectedDateTime[0].selectedFareDateTimeSchedSelect,
                infantAge = new SKYSALES.Class.TravelerAge(),
                    i = 0,
                    birthday = null;
                if (returnMarket) {
                    for (i = 0; i < thisAvailabilityInput.infantArray.length; i += 1) {
                        birthday = thisAvailabilityInput.infantArray[i];
                        if (birthday) {
                            if (!infantAge.isInfant(returnMarket, birthday, departMarket)) {
                                alert(thisAvailabilityInput.infantAgeText);
                                return false;
                            }
                        }
                    }
                }
            }
        }
        thisAvailabilityInput.validateEvent = function() {
            var validateValue = true;
            var isWithinCutOffHours = false;
            var isWithinTimeLimit = false;
            isWithinCutOffHours = ((thisAvailabilityInput.isWithinCutOffHours[0]) || (thisAvailabilityInput.isWithinCutOffHours[1]));
            isWithinTimeLimit = ((thisAvailabilityInput.isWithinTimeLimit[0]) || (thisAvailabilityInput.isWithinTimeLimit[1]));
            //Added by Cluse Zeng on 6 April 2012(begin)
            //stop user change flight to same flight
            var fareTables = $("#selectMainBody div[@class^='availabilityInputContent'] .rgMasterTable");
            var selectsameflight = false;
            for (fareTableindex = 0; fareTableindex < fareTables.length; fareTableindex++) {
                var fareTable = $(fareTables[fareTableindex]);
                var marketSelected = fareTable.find("tr td[@class^='resultFareCell'] div[@class^='resultFare'] div[@id^='fareRadio'] :radio[@checked]");
                var selectrow = fareTable.find("tr");
                var productClass;
                for (rowindex = 0; rowindex < selectrow.length; rowindex++) {
                    productClass = "";
                    var selectcolumn = $(selectrow[rowindex]).find("td[@class^='resultFareCell']");
                    for (columnindex = 0; columnindex < selectcolumn.length; columnindex++) {
                        if ($(selectcolumn[columnindex]).find("div[@class^='resultFare'] div[@id^='fareRadio'] :radio[@checked]").length > 0 && columnindex < thisAvailabilityInput.fareCategories.length) {
                            productClass = thisAvailabilityInput.fareCategories[columnindex].productClasses;
                            break;
                        }
                    }
                    if (productClass && productClass.length > 0) {
                        break;
                    }
                }

                if (marketSelected && productClass && marketSelected.length > 0 && productClass.length > 0) {
                    var marketSelectedValue = marketSelected[0].value;
                    var marketSelectedClass = productClass;
                    var marketSelectedJourney = marketSelectedValue.substring(marketSelectedValue.indexOf("|") + 1, marketSelectedValue.length);
                    var marketSelectedFilght = marketSelectedJourney.substring(0, marketSelectedJourney.indexOf("~~")).replace(/\~/g, "");
                    if (marketSelectedJourney.indexOf("^") > 0) {
                        var marketSelectedFilght2 = marketSelectedJourney.substring(marketSelectedJourney.indexOf("^") + 1, marketSelectedJourney.length);
                        marketSelectedFilght2 = marketSelectedFilght2.substring(0, marketSelectedFilght2.indexOf("~~")).replace(/\~/g, "");
                        marketSelectedFilght = marketSelectedFilght + "/" + marketSelectedFilght2;
                    }
                    var marketSelectedDeparture = marketSelectedJourney.substring(marketSelectedJourney.indexOf("~~") + 2, marketSelectedJourney.length);
                    var marketSelectedDepartureDate = marketSelectedDeparture.substring(marketSelectedDeparture.indexOf("~") + 1, marketSelectedDeparture.indexOf(" "));
                    var existDepartureDate = new Date(Date.parse(thisAvailabilityInput.existJourney[fareTableindex].departuredate));
                    var selectDepartureDate = new Date(Date.parse(marketSelectedDepartureDate));
                    var existFlight = thisAvailabilityInput.existJourney[fareTableindex].flightdesignator;
                    var existProductClass = "[" + thisAvailabilityInput.existJourney[fareTableindex].productclass + "]";

                    if ((existDepartureDate - selectDepartureDate === 0) && marketSelectedFilght === existFlight && marketSelectedClass.indexOf(existProductClass) >= 0) {
                        selectsameflight = true;
                        break;
                    }
                    else {
                        selectsameflight = false;
                    }
                }
            }
            if (selectsameflight) {
                alert(thisAvailabilityInput.changeSameFlightMessage);
                SKYSALES.RemoveLoadingBar(); //added by Linson at 2012-05-16              
                return false;
            }
            //Added by Cluse Zeng on 6 April 2012(end)
            var marketsSelected = $("#selectMainBody div[@class^='availabilityInputContent'] .rgMasterTable tr td[@class^='resultFareCell'] div[@class^='resultFare'] div[@id^='fareRadio'] :radio[@checked]").length,
                marketVisible = $("#selectMainBody :visible div[@class^='availabilityInputContent']").length;
            if (marketsSelected === 0 || (!thisAvailabilityInput.hideMarketIndex && marketsSelected < thisAvailabilityInput.marketCount) || (marketsSelected < marketVisible)) {
                alert(thisAvailabilityInput.incompleteFareSelection);
                SKYSALES.RemoveLoadingBar(); //added by Linson at 2011-12-31
                //Hijack by Sean on 27 Jun 2012 from false to true, Remember to change it back later
                return false;
            }

            if (thisAvailabilityInput.isExemptedFromCutoff == false) {
                if (thisAvailabilityInput.isNewBooking == 'true') {
                    if (isWithinCutOffHours) {
                        SKYSALES.RemoveLoadingBar(); //added by Linson at 2011-12-31
                        return false;
                    }
                }

                if (thisAvailabilityInput.isNewBooking == 'false') {
                    if (isWithinTimeLimit) {
                        return false;
                        SKYSALES.RemoveLoadingBar(); //added by Linson at 2011-12-31
                    }
                    if (isWithinCutOffHours) {
                        if ($('#' + thisAvailabilityInput.agreement24HrsCheckboxId + ':checked').length == 0) {
                            alert(thisAvailabilityInput.agreement24HrMessage);
                            SKYSALES.RemoveLoadingBar(); //added by Linson at 2011-12-31
                            return false;
                        }
                    }
                }
            }

            if (validateValue == true && thisAvailabilityInput.departureDates.length > 1) {
                departureDates = thisAvailabilityInput.GetDepartureDates(thisAvailabilityInput.departureDates);
                if (departureDates[0] && departureDates[1] && departureDates[0] > departureDates[1]) {
                    validateValue = false;
                    alert(thisAvailabilityInput.marketDatesMessage);
                    SKYSALES.RemoveLoadingBar(); //added by Linson at 2011-12-31
                    return false;
                }
            }

            if (validateValue == true) {
                if (thisAvailabilityInput.restrictSameFareCategory == 'True') {
                    if (thisAvailabilityInput.fareClasses.length > 1) {
                        validateValue = false;
                        if (thisAvailabilityInput.fareClasses[0] == thisAvailabilityInput.fareClasses[1]) {
                            validateValue = true;
                        }
                        else {
                            if (thisAvailabilityInput.getFareCategory(thisAvailabilityInput.fareClasses[0]) == thisAvailabilityInput.getFareCategory(thisAvailabilityInput.fareClasses[1])) {
                                validateValue = true;
                            }
                        }
                    }
                    if (validateValue == false) {
                        alert(thisAvailabilityInput.sameCategoryMessage);
                        SKYSALES.RemoveLoadingBar(); //added by Linson at 2011-12-31
                        return false;
                    }
                }
            }
            return thisAvailabilityInput.checkInfantAge();
        }

        thisAvailabilityInput.GetDepartureDates = function(departureDates) {
            var selectedDepDates = [null, null],
                hiddenFareDate = null,
                fareToChangeDate = null;

            if (this.isFlightChange && this.hideMarketIndex) {
                fareToChangeDate = departureDates[0];
                hiddenFareDate = new Date(this.userSelectedDateTime[this.hideMarketIndex].selectedFareDateTime);
                if (this.hideMarketIndex === "0") {
                    selectedDepDates[1] = fareToChangeDate;
                }
                else {
                    selectedDepDates[0] = fareToChangeDate;
                }
                selectedDepDates[this.hideMarketIndex] = hiddenFareDate;

                return selectedDepDates;
            }
            return departureDates;
        };

        thisAvailabilityInput.ajaxEquipmentProperties = function() {

        };
        thisAvailabilityInput.addEquipmentPropertiesAjaxEvent = function() {
            $(this).click(thisAvailabilityInput.ajaxEquipmentProperties);
        };
        thisAvailabilityInput.addEquipmentPropertiesAjaxEvents = function() {
            thisAvailabilityInput.detailsLinks.each(thisAvailabilityInput.addEquipmentPropertiesAjaxEvent);
        };
        thisAvailabilityInput.init = function() {
            thisAvailabilityInput.initJourneyInfoContainers();
            if (undefined !== SKYSALES.taxAndFeeInclusiveDisplayDataRequestHandler) {
                thisAvailabilityInput.setVars();

                // Determine if we need to preselect the lowest amount fare as per the setting.
                if (thisAvailabilityInput.preselectFares == "true") {
                    thisAvailabilityInput.setPreselectedFares();
                    if ($('#ControlGroupSelectView_AgreementInputSelectView_CheckBoxAgreement').length > 0) {
                        $('#ControlGroupSelectView_AgreementInputSelectView_CheckBoxAgreement').attr("required", "true");
                    }
                    else {
                        $('#ControlGroupSelectChangeView_AgreementInputSelectChangeView_CheckBoxAgreement').attr("required", "true");
                    }
                }
                else {
                    thisAvailabilityInput.updateFareInfo();
                    // During initialization, temporarily hide the agreement section and remove validation on the agreement checkbox
                    // because we'll use our own validation to ensure that fares were selected for all markets.
                    $('#agreementInput').hide();
                    if ($('#ControlGroupSelectView_AgreementInputSelectView_CheckBoxAgreement').length > 0 &&
                        $('#ControlGroupSelectView_AgreementInputSelectView_CheckBoxAgreement')[0].required === 'true') {
                        $('#ControlGroupSelectView_AgreementInputSelectView_CheckBoxAgreement').attr("required", "false");
                    }
                    else if ($('#ControlGroupSelectChangeView_AgreementInputSelectChangeView_CheckBoxAgreement').length > 0 &&
                             $('#ControlGroupSelectChangeView_AgreementInputSelectChangeView_CheckBoxAgreement')[0].required === 'true') {
                        $('#ControlGroupSelectChangeView_AgreementInputSelectChangeView_CheckBoxAgreement').attr("required", "false");
                    }
                }

                thisAvailabilityInput.addEvents();
            }
        };
    };
}

/*
Name: 
Class JourneyInfo
Param:
None
Return: 
An instance of JourneyInfo
Functionality:
The object that represents the journey information on the AvailabilityInput control
Notes:
When the user clicks a link to view more details about the flight it gets the data and shows it in a floating div.
It uses AJAX to get data about the equipment that the journey will use.
The AJAX response sends the equipment data in the form of a JSON object.
Class Hierarchy:
AvailabilityInput
*/
if (SKYSALES.Class.JourneyInfo === undefined) {
    SKYSALES.Class.JourneyInfo = function() {
        var thisJourneyInfo = this;

        thisJourneyInfo.equipmentInfoUri = 'EquipmentPropertiesDisplayAjax-resource.aspx';
        thisJourneyInfo.key = '';
        thisJourneyInfo.journeyContainerId = "";
        thisJourneyInfo.activateJourneyId = "";
        thisJourneyInfo.activateJourney = null;
        thisJourneyInfo.deactivateJourneyId = "";
        thisJourneyInfo.deactivateJourney = null;
        thisJourneyInfo.journeyContainer = null;
        thisJourneyInfo.legInfoArray = [];
        thisJourneyInfo.clientName = 'EquipmentPropertiesDisplayControlAjax';

        thisJourneyInfo.init = function(paramObject) {
            this.setSettingsByObject(paramObject);
            this.setVars();
            this.addEvents();
        };
        thisJourneyInfo.setVars = function() {
            thisJourneyInfo.journeyContainer = $('#' + thisJourneyInfo.journeyContainerId);
            thisJourneyInfo.activateJourney = $('#' + thisJourneyInfo.activateJourneyId);
            thisJourneyInfo.deactivateJourney = $('#' + thisJourneyInfo.deactivateJourneyId);
        };
        thisJourneyInfo.addEvents = function() {
            thisJourneyInfo.activateJourney.click(thisJourneyInfo.show);
            thisJourneyInfo.deactivateJourney.click(thisJourneyInfo.hide);
        };
        thisJourneyInfo.setSettingsByObject = function(jsonObject) {
            var propName = '';
            for (propName in jsonObject) {
                if (thisJourneyInfo.hasOwnProperty(propName)) {
                    thisJourneyInfo[propName] = jsonObject[propName];
                }
            }
        };
        thisJourneyInfo.showWithData = function(data) {
            var legInfoStr = $(data).html();
            var legInfoJson = SKYSALES.Json.parse(legInfoStr);
            var legInfoHash = legInfoJson.legInfo;
            var legInfo = null;
            var prop = '';
            var propertyContainer = null;
            var propertyHtml = '';
            var equipmentPropertyArray = null;
            var i = 0;
            var equipmentProperty = null;
            for (prop in legInfoHash) {
                if (legInfoHash.hasOwnProperty(prop)) {
                    propertyHtml = '';
                    legInfo = legInfoHash[prop];
                    if (legInfo.legIndex !== undefined) {
                        propertyContainer = $('#propertyContainer_' + thisJourneyInfo.key);
                        equipmentPropertyArray = legInfo.equipmentPropertyArray;
                        for (i = 0; i < equipmentPropertyArray.length; i += 1) {
                            equipmentProperty = equipmentPropertyArray[i];
                            propertyHtml += '<div>' + equipmentProperty.name + ': ' + equipmentProperty.value + '<\/div>';
                        }
                        propertyContainer.html(propertyHtml);
                    }
                }
            }
            thisJourneyInfo.journeyContainer.show('slow');
        };
        thisJourneyInfo.show = function() {
            var legInfoArray = thisJourneyInfo.legInfoArray;
            var legInfo = null;
            var postHash = {};
            var prop = '';
            var i = 0;
            var propName = thisJourneyInfo.clientName;
            for (i = 0; i < legInfoArray.length; i += 1) {
                legInfo = legInfoArray[i];
                for (prop in legInfo) {
                    if (legInfo.hasOwnProperty(prop)) {
                        postHash[propName + '$legInfo_' + prop + '_' + i] = legInfo[prop];
                    }
                }
            }
            $.post(thisJourneyInfo.equipmentInfoUri, postHash, thisJourneyInfo.showWithData);
        };
        thisJourneyInfo.hide = function() {
            thisJourneyInfo.journeyContainer.hide();
        };
        return thisJourneyInfo;
    };
}

/* End AvailabilityInput.js */
/* Start SSRPassengerInput.js */

/*!
This file is part of the Navitaire NewSkies application.
Copyright (C) Navitaire.  All rights reserved.
*/
/*


JsLint Status:
Pass - JsLint Edition 2008-07-04
        
+ Strict whitespace
+ Assume a browser
+ Disallow undefined variables
+ Disallow leading _ in identifiers
+ Disallow == and !=
+ Disallow ++ and --
+ Disallow bitwise operators
+ Disallow . in RegExp literals
+ Disallow global var
Indentation 4
*/

/*extern $ jQuery window SKYSALES*/

/*
Name: 
Class SsrPassengerInput
Param:
None
Return: 
An instance of SsrPassengerInput
Functionality:
This class represents an SsrPassengerInput
It is used to sell ssrs at any point in the booking flow
Notes:
Class Hierarchy:
SkySales -> SsrPassengerInput
*/
if (!SKYSALES.Class.SsrPassengerInput) {
    SKYSALES.Class.SsrPassengerInput = function() {
        var parent = SKYSALES.Class.SkySales();
        var thisSsrPassengerInput = SKYSALES.Util.extendObject(parent);

        thisSsrPassengerInput.ssrFormArray = null;
        thisSsrPassengerInput.ssrFeeArray = null;
        thisSsrPassengerInput.errorMsgOverMaxPerPassenger = 'There has been an error';
        thisSsrPassengerInput.ssrButtonIdArray = null;
        thisSsrPassengerInput.ssrButtonArray = null;
        thisSsrPassengerInput.buttonTrackId = "";
        thisSsrPassengerInput.buttonTrack = null;

        thisSsrPassengerInput.init = function(json) {
            this.setSettingsByObject(json);
            this.setVars();
            this.addEvents();
            $('table.ssrSoldContainer :input', this.container).attr("disabled", "disabled");
        };

        thisSsrPassengerInput.setVars = function() {
            thisSsrPassengerInput.buttonTrack = $('#' + this.buttonTrackId);
            thisSsrPassengerInput.ssrButtonIdArray = this.ssrButtonIdArray || [];
            var ssrButtonArray = [];
            var i = 0;
            var ssrButton = null;
            var ssrButtonId = '';
            for (i = 0; i < this.ssrButtonIdArray.length; i += 1) {
                ssrButtonId = this.ssrButtonIdArray[i];
                ssrButton = $('#' + ssrButtonId);
                if (ssrButton.length > 0) {
                    ssrButtonArray[ssrButtonArray.length] = ssrButton;
                }
            }
            thisSsrPassengerInput.ssrButtonArray = ssrButtonArray;
        };

        thisSsrPassengerInput.addEvents = function() {
            this.addButtonClickedEvents();
        };

        thisSsrPassengerInput.addButtonClickedEvents = function() {
            var i = 0;
            var ssrButton = null;
            for (i = 0; i < this.ssrButtonArray.length; i += 1) {
                ssrButton = this.ssrButtonArray[i];
                ssrButton.click(this.updateButtonTrackHandler);
            }
        };

        thisSsrPassengerInput.updateButtonTrackHandler = function() {
            thisSsrPassengerInput.buttonTrack.val(this.id);
        };

        thisSsrPassengerInput.setSettingsByObject = function(json) {
            parent.setSettingsByObject.call(this, json);

            var i = 0;
            var ssrFormArray = this.ssrFormArray || [];
            var ssrForm = null;
            for (i = 0; i < ssrFormArray.length; i += 1) {
                ssrForm = new SKYSALES.Class.SsrForm();
                ssrForm.index = i;
                ssrForm.ssrPassengerInput = this;
                ssrForm.init(ssrFormArray[i]);
                ssrFormArray[i] = ssrForm;
            }

            var ssrFeeArray = this.ssrFeeArray || [];
            var ssrFee = null;
            for (i = 0; i < ssrFeeArray.length; i += 1) {
                ssrFee = new SKYSALES.Class.SsrFormFee();
                ssrFee.index = i;
                ssrFee.ssrPassengerInput = this;
                ssrFee.init(ssrFeeArray[i]);
                ssrFeeArray[i] = ssrFee;
            }
        };

        thisSsrPassengerInput.deactivateSsrFormNotes = function() {
            var i = 0;
            var ssrFormArray = this.ssrFormArray;
            var ssrForm = null;

            for (i = 0; i < ssrFormArray.length; i += 1) {
                ssrForm = ssrFormArray[i];
                ssrForm.deactivateNoteDiv();
            }
        };
        return thisSsrPassengerInput;
    };
    SKYSALES.Class.SsrPassengerInput.createObject = function(json) {
        SKYSALES.Util.createObject('ssrPassengerInput', 'SsrPassengerInput', json);
    };
}


/*
Name: 
Class SsrForm
Param:
None
Return: 
An instance of SsrForm
Functionality:
An SsrForm represents a row on the SsrPassengerInput
Notes:
Class Hierarchy:
SkySales -> SsrForm
*/
SKYSALES.Class.SsrForm = function() {
    var parent = SKYSALES.Class.SkySales();
    var thisSsrForm = SKYSALES.Util.extendObject(parent);

    thisSsrForm.maximumDropDownLimit = 0;
    thisSsrForm.ssrPassengerId = '';
    thisSsrForm.ssrPassenger = null;
    thisSsrForm.ssrCodeId = '';
    thisSsrForm.ssrCode = null;
    thisSsrForm.ssrQuantityId = '';
    thisSsrForm.ssrQuantity = null;
    thisSsrForm.ssrNoteId = '';
    thisSsrForm.ssrNote = null;
    thisSsrForm.ssrNoteIframeId = '';
    thisSsrForm.ssrNoteIframe = null;
    thisSsrForm.ssrNoteCloseId = '';
    thisSsrForm.ssrNoteClose = null;
    thisSsrForm.ssrNoteDivId = '';
    thisSsrForm.ssrNoteDiv = null;
    thisSsrForm.ssrNoteImageId = '';
    thisSsrForm.ssrNoteImage = null;
    thisSsrForm.ssrNoteCancelId = '';
    thisSsrForm.ssrNoteCancel = null;
    thisSsrForm.ssrFlightId = '';
    thisSsrForm.ssrFlight = null;
    thisSsrForm.ssrAmountId = '';
    thisSsrForm.ssrAmount = null;
    thisSsrForm.ssrCurrencyId = '';
    thisSsrForm.ssrCurrency = null;
    thisSsrForm.index = -1;
    thisSsrForm.ssrPassengerInput = null;

    thisSsrForm.init = function(json) {
        this.setSettingsByObject(json);
        this.setVars();
        this.addEvents();
        this.updateSsrAmount();
    };

    thisSsrForm.setVars = function() {
        thisSsrForm.ssrNote = $('#' + this.ssrNoteId);
        thisSsrForm.ssrNoteDiv = $('#' + this.ssrNoteDivId);
        thisSsrForm.ssrNoteClose = $('#' + this.ssrNoteCloseId);
        thisSsrForm.ssrNoteCancel = $('#' + this.ssrNoteCancelId);
        thisSsrForm.ssrNoteImage = $('#' + this.ssrNoteImageId);
        thisSsrForm.ssrNoteIframe = $('#' + this.ssrNoteIframeId);
        thisSsrForm.ssrQuantity = $('#' + this.ssrQuantityId);
        thisSsrForm.ssrPassenger = $('#' + this.ssrPassengerId);
        thisSsrForm.ssrCode = $('#' + this.ssrCodeId);
        thisSsrForm.ssrCurrency = $('#' + this.ssrCurrencyId);
        thisSsrForm.ssrFlight = $('#' + this.ssrFlightId);
        thisSsrForm.ssrAmount = $('#' + this.ssrAmountId);
    };

    thisSsrForm.addEvents = function() {
        this.addNoteEvents();
        this.addQuantityEvents();
        this.addSSRCodeEvents();
        this.addFlightEvents();
    };

    thisSsrForm.addFlightEvents = function() {
        this.ssrFlight.change(this.updateSsrAmountHandler);
    };

    thisSsrForm.updateSsrQuantityHandler = function() {
        thisSsrForm.updateSsrQuantity();
    };

    thisSsrForm.updateSsrAmountHandler = function() {
        thisSsrForm.updateSsrAmount();
    };

    thisSsrForm.addSSRCodeEvents = function() {
        this.ssrCode.change(this.updateSsrQuantityHandler);
        this.ssrCode.change(this.updateSsrAmountHandler);
    };

    thisSsrForm.addQuantityEvents = function() {
        this.ssrQuantity.change(this.updateSsrAmountHandler);
        this.ssrQuantity.blur(this.updateSsrAmountHandler);
    };

    thisSsrForm.updateSsrAmount = function() {
        var ssrAmount = this.ssrAmount;
        var ssrAmountFormatted = SKYSALES.Util.convertToLocaleCurrency('0.00');
        ssrAmount.val(ssrAmountFormatted);
        var ssrPassengerValue = this.ssrPassenger.val();
        var ssrCodeValue = this.ssrCode.val();
        var ssrQuantityValue = this.ssrQuantity.val();
        ssrQuantityValue = $.trim(ssrQuantityValue);
        var re = /^[0-9]+$/;
        var j = 0;
        var ssrPassengerInput = this.ssrPassengerInput;
        var ssrFeeArray = ssrPassengerInput.ssrFeeArray;
        var ssrFee = null;
        var ssrFlightValue = '';
        var amount = 0;

        if (re.test(ssrQuantityValue)) {
            ssrFlightValue = this.ssrFlight.val();

            for (j = 0; j < ssrFeeArray.length; j += 1) {
                ssrFee = ssrFeeArray[j];

                if ((ssrFlightValue === "all") || (ssrFlightValue === ssrFee.segmentKey)) {
                    if ((ssrPassengerValue === ssrFee.passengerNumber) && (ssrCodeValue === ssrFee.ssrCode)) {
                        amount += (ssrFee.amount * ssrQuantityValue);
                    }
                }
            }
            ssrAmountFormatted = SKYSALES.Util.convertToLocaleCurrency(amount);
            ssrAmount.val(ssrAmountFormatted);
        }
        else {
            this.ssrQuantity.val(0);
        }
    };

    thisSsrForm.updateSsrQuantity = function() {
        var maximumDropDownLimit = this.maximumDropDownLimit;
        maximumDropDownLimit = window.parseInt(maximumDropDownLimit, 10);

        var ssrPassengerValue = this.ssrPassenger.val();
        var ssrCodeValue = this.ssrCode.val();
        var ssrFlightValue = this.ssrFlight.val();
        var i = 0;
        var j = 0;
        var ssrPassengerInput = this.ssrPassengerInput;
        var ssrFeeArray = ssrPassengerInput.ssrFeeArray;
        var ssrFee = null;
        var ssrQuantityValue = this.ssrQuantity.val();
        ssrQuantityValue = parseInt(ssrQuantityValue, 10);
        var maxPerPassenger = 0;
        var newOpt = null;
        var ssrQuantityInput = this.ssrQuantity.get(0);

        for (i = 0; i < ssrFeeArray.length; i += 1) {
            ssrFee = ssrFeeArray[i];
            if ((ssrFlightValue === "all") || (ssrFlightValue === ssrFee.segmentKey)) {
                if ((ssrPassengerValue === ssrFee.passengerNumber) && (ssrCodeValue === ssrFee.ssrCode)) {
                    maxPerPassenger = parseInt(ssrFee.maxPerPassenger, 10);
                    if (maxPerPassenger === 0) {
                        maxPerPassenger = maximumDropDownLimit;
                        maxPerPassenger = parseInt(maxPerPassenger, 10);
                        if (ssrQuantityValue >= maxPerPassenger) {
                            maxPerPassenger = ssrQuantityValue;
                            maxPerPassenger = maxPerPassenger + 1;
                        }
                    }

                    if (ssrQuantityInput.options) {

                        while (ssrQuantityInput.options.length > 0) {
                            ssrQuantityInput.options[0] = null;
                        }

                        for (j = 0; j <= maxPerPassenger; j += 1) {
                            newOpt = new window.Option(j, j);
                            ssrQuantityInput.options[j] = newOpt;
                            if (ssrQuantityValue === j) {
                                this.ssrQuantity.val(j);
                            }
                        }
                    }

                    if (ssrQuantityValue > maxPerPassenger) {
                        this.ssrQuantity.val(maxPerPassenger);
                        alert(this.getErrorMsgOverMaxPerPassenger());
                    }
                    else {
                        this.ssrQuantity.val(ssrQuantityValue);
                    }
                }
            }
        }
    };

    thisSsrForm.getErrorMsgOverMaxPerPassenger = function() {
        var retVal = '';
        retVal = this.ssrPassengerInput.errorMsgOverMaxPerPassenger;
        return retVal;
    };

    thisSsrForm.clearAndDeactivateNoteDiv = function() {
        var ssrNote = this.ssrNote;
        var isDisabled = ssrNote.is(':disabled');

        if (isDisabled === false) {
            ssrNote.val('');
        }
        this.deactivateNoteDiv();
    };

    thisSsrForm.deactivateNoteDiv = function() {
        this.ssrNoteDiv.hide();
        this.ssrNoteIframe.hide();
    };

    thisSsrForm.activateNoteDiv = function() {
        // Reset the floating ssrNote divs
        this.ssrPassengerInput.deactivateSsrFormNotes();

        var ssrNoteImage = this.ssrNoteImage.get(0);
        var dhtml = SKYSALES.Dhtml();
        var left = dhtml.getX(ssrNoteImage);
        var top = dhtml.getY(ssrNoteImage);

        this.ssrNoteDiv.css('left', left + 'px');
        this.ssrNoteDiv.css('top', top + 'px');
        this.ssrNoteDiv.show();

        this.ssrNoteIframe.css('left', left + 'px');
        this.ssrNoteIframe.css('top', top + 'px');
        this.ssrNoteIframe.show();

        var isDisabled = this.ssrNote.is(':disabled');
        if (isDisabled === false) {
            this.ssrNote.click();
        }
    };

    thisSsrForm.ssrNoteCancelHandler = function() {
        thisSsrForm.clearAndDeactivateNoteDiv();
    };

    thisSsrForm.ssrNoteCloseHandler = function() {
        thisSsrForm.deactivateNoteDiv();
    };

    thisSsrForm.ssrNoteImageHandler = function() {
        thisSsrForm.activateNoteDiv();
    };

    thisSsrForm.addNoteEvents = function() {
        this.ssrNoteCancel.mouseup(this.ssrNoteCancelHandler);
        this.ssrNoteClose.mouseup(this.ssrNoteCloseHandler);
        this.ssrNoteImage.mouseup(this.ssrNoteImageHandler);
    };
    return thisSsrForm;
};

/*
Name: 
Class SsrFormFee
Param:
None
Return: 
An instance of SsrFormFee
Functionality:
An SsrForm represents a fee that is used to show the amount for an SsrForm
Notes:
Class Hierarchy:
SkySales -> SsrFormFee
*/
SKYSALES.Class.SsrFormFee = function() {
    var parent = SKYSALES.Class.SkySales();
    var thisSsrFormFee = SKYSALES.Util.extendObject(parent);

    thisSsrFormFee.journeyIndex = -1;
    thisSsrFormFee.segmentIndex = -1;
    thisSsrFormFee.segmentKey = '';
    thisSsrFormFee.passengerNumber = -1;
    thisSsrFormFee.ssrCode = '';
    thisSsrFormFee.feeCode = '';
    thisSsrFormFee.amount = 0;
    thisSsrFormFee.currencyCode = '';
    thisSsrFormFee.maxPerPassenger = 0;
    thisSsrFormFee.index = -1;
    thisSsrFormFee.ssrPassengerInput = null;

    return thisSsrFormFee;
};

/* End SSRPassengerInput.js */

/* Start TaxAndFeeInclusiveDisplay.js */

/*global $ SKYSALES window */
SKYSALES.taxAndFeeInclusiveDisplayDataRequestHandler = function taxAndFeeInclusiveDisplayDataRequestHandler(keys, markets, marketCount, hiddenMarketIndex, progressMsg) {
    var keyDelimiter = ',';
    var params = { flightKeys: keys.join(keyDelimiter), numberOfMarkets: markets, keyDelimeter: keyDelimiter };
    var searchedMarketOffset = 0;
    var addAllUpPricingEvents = function() {
        var taxAndFeeInclusiveTotalA = $('taxAndFeeInclusiveTotal');
        var allUpPricingDiv = $('allUpPricing');
        if (SKYSALES.common) {
            SKYSALES.common.stripeTables(allUpPricingDiv);
        }
        var allUpPricingToggleView = new SKYSALES.Class.ToggleView();
        var allUpPricingToggleViewJson = {
            "elementId": "allUpPricing",
            "hideId": "closeTotalPrice",
            "showId": "taxAndFeeInclusiveTotal"
        };
        allUpPricingToggleView.init(allUpPricingToggleViewJson);

        var taxAndFeeInclusiveTotalAHandler = function() {
            allUpPricingDiv.toggle();
        };

        if (taxAndFeeInclusiveTotalA && allUpPricingDiv) {
            taxAndFeeInclusiveTotalA.click(taxAndFeeInclusiveTotalAHandler);
        }
    };

    var updateTaxAndFeeInclusiveDivBodyHandler = function(data) {
        data = "<div>" + data + "</div>";
        if (window.$) {
            $("#taxAndFeeInclusiveDivHeader").before("<div id='tempDiv'></div>");
            $("#taxAndFeeInclusiveDivHeader").remove();
            $("#taxAndFeeInclusiveDivBody").remove();
            $("#tempDiv").after($(data).find("#taxAndFeeInclusiveDivHeader"));
            $("#taxAndFeeInclusiveDivHeader").after($(data).find("#taxAndFeeInclusiveDivBody"));
            $("#tempDiv").remove();

            if (markets === 1) {
                $("table#taxesAndFeesInclusiveDisplay_2").hide();
            }
        }
        addAllUpPricingEvents();
    };

    if (hiddenMarketIndex != "") {
        searchedMarketOffset = 1;
    }

    // "markets" is the number of markets with a selected fare.
    // "marketCount" is the total number of markets being searched. So 1 for oneway 2 for roundtrip.
    if (markets === parseInt(marketCount) - searchedMarketOffset) {
        $.get("TaxAndFeeInclusiveDisplayAjax-resource.aspx",
        params,
        updateTaxAndFeeInclusiveDivBodyHandler);

        $("#taxAndFeeNoFaresSelected").remove();
        inProcessHTMLString = '<div id="taxAndFeeInclusiveDivBody" class="right-col-sidebar" style="padding: 5px 12px;">' + progressMsg + '<div id="waitImg"></div></div>';

        $("#taxAndFeeInclusiveDivBody").remove();
        $("#taxAndFeeInclusiveDivHeader").after(inProcessHTMLString);
    }
};



/* End TaxAndFeeInclusiveDisplay.js */


/* Start TravelerInput.js */
/*!
This file is part of the Navitaire Professional Services customization.
Copyright (C) Navitaire.  All rights reserved.
*/
/*


JsLint Status:
- JsLint Edition 2008-07-04
        
+ Strict whitespace
+ Assume a browser
+ Disallow undefined variables
+ Disallow leading _ in identifiers
+ Disallow == and !=
+ Disallow ++ and --
+ Disallow bitwise operators
+ Disallow . in RegExp literals
+ Disallow global var
Indentation 4
*/

/*extern $ jQuery window SKYSALES*/


/*
Name: 
Class TravelerInput
Param:
None
Return: 
An instance of TravelerInput
Functionality:
This class represents the Link and the Div container of each component
in the traveler page
Notes:
Class Hierarchy:
SkySales -> TravelerInputContainer
*/
if (!SKYSALES.Class.TravelerInputContainer) {
    SKYSALES.Class.TravelerInputContainer = function() {
        var parent = SKYSALES.Class.SkySales(),
        thisTravelerInputContainer = SKYSALES.Util.extendObject(parent);

        thisTravelerInputContainer.menuSectionId = "";
        thisTravelerInputContainer.menuSection = null;
        thisTravelerInputContainer.menuListId = "";
        thisTravelerInputContainer.menuList = null;
        thisTravelerInputContainer.mainTravelerId = "";
        thisTravelerInputContainer.mainTraveler = null;
        thisTravelerInputContainer.travelerInputArray = null;
        thisTravelerInputContainer.submitButtonId = "";
        thisTravelerInputContainer.submitButton = null;

        thisTravelerInputContainer.continueButtonId = "";
        thisTravelerInputContainer.continueButton = null;
        thisTravelerInputContainer.confirmText = "";

        thisTravelerInputContainer.nextButtonId = "";
        thisTravelerInputContainer.nextButton = null;
        thisTravelerInputContainer.nextButtons = null;

        thisTravelerInputContainer.disableNames = null;
        thisTravelerInputContainer.disableContactNames = null;

        thisTravelerInputContainer.infantText = "";
        thisTravelerInputContainer.infantPreText = "";
        thisTravelerInputContainer.childText = "";
        thisTravelerInputContainer.childPreText = "";
        thisTravelerInputContainer.infantFutureBirthDateErrorPreText = "";
        thisTravelerInputContainer.infantFutureBirthDateErrorPostText = "";
        thisTravelerInputContainer.returnDate = "";
        thisTravelerInputContainer.departDate = "";
        thisTravelerInputContainer.passportExpDays = "";
        thisTravelerInputContainer.adultPassportMsgText = "";
        thisTravelerInputContainer.adultPassportMsgPreText = "";
        thisTravelerInputContainer.childPassportMsgText = "";
        thisTravelerInputContainer.childPassportMsgPreText = "";
        thisTravelerInputContainer.infantPassportMsgText = "";
        thisTravelerInputContainer.infantPassportMsgPreText = "";

        thisTravelerInputContainer.duplicateAdultTextPre = "";
        thisTravelerInputContainer.duplicateAdultText = "";

        thisTravelerInputContainer.friendsArray = null;
        thisTravelerInputContainer.addFriendsButtonId = "";
        thisTravelerInputContainer.refreshFriendsButtonId = "";
        thisTravelerInputContainer.addFriendsButton = null;
        thisTravelerInputContainer.refreshFriendsButton = null;
        thisTravelerInputContainer.addFriendsDiv = "";
        thisTravelerInputContainer.addFriendsErrorText = "";
        thisTravelerInputContainer.noTickedFriendsText = "";
        thisTravelerInputContainer.continueButtonText = "";
        thisTravelerInputContainer.submitButtonText = "";
        thisTravelerInputContainer.paxTypesArray = null;
        thisTravelerInputContainer.changeCurrencyPrompter = "";
        thisTravelerInputContainer.unmatchedPaxTypeErrorText = "";

        thisTravelerInputContainer.passportNumber = "";
        thisTravelerInputContainer.passportExpDate = null;
        thisTravelerInputContainer.passportExpMonth = null;
        thisTravelerInputContainer.passportExpYear = null;

        var firstPaxTabIndex = 2;
        thisTravelerInputContainer.init = function(json) {
            this.setSettingsByObject(json);
            this.setVars();
            this.addEvents();

            if (thisTravelerInputContainer.travelerInputArray.length > 0) {
                for (i = 0; i < thisTravelerInputContainer.travelerInputArray.length; i += 1) {
                    //initialize all traveler input
                    thisTravelerInputContainer.travelerInputArray[i].travelerInputArray = this.travelerInputArray;
                    thisTravelerInputContainer.travelerInputArray[i].hideContent(); //resets all

                    if (thisTravelerInputContainer.travelerInputArray[i].contentId == "content_0"
                    || (thisTravelerInputContainer.addFriendsDiv != null &&
                    thisTravelerInputContainer.travelerInputArray[i].contentId == thisTravelerInputContainer.addFriendsDiv)
                    ) {
                        if (this.disableContactNames != null &&
                            this.disableContactNames != "" &&
                            thisTravelerInputContainer.travelerInputArray[i].contentId == "content_0") {
                            thisTravelerInputContainer.travelerInputArray[i].disableTextFields();
                        }
                        else {
                            continue;
                        }
                    }
                    else if (this.disableNames != null && this.disableNames != "") {
                        thisTravelerInputContainer.travelerInputArray[i].disableTextFields();
                    }
                }
                thisTravelerInputContainer.travelerInputArray[0].showContent();
            }

            if (this.changeCurrencyPrompter != "") {
                alert(this.changeCurrencyPrompter);
            }
        };

        thisTravelerInputContainer.setVars = function() {
            thisTravelerInputContainer.menuSection = $('#' + this.menuSectionId);
            thisTravelerInputContainer.menuList = $('#' + this.menuListId);
            thisTravelerInputContainer.mainTraveler = $('#' + this.mainTravelerId);

            thisTravelerInputContainer.submitButton = $("input[id='" + this.submitButtonId + "']");
            if (thisTravelerInputContainer.submitButtonText != "") {
                thisTravelerInputContainer.submitButton.attr("value", thisTravelerInputContainer.submitButtonText);
            }
            thisTravelerInputContainer.addFriendsButton = $("input[id='" + this.addFriendsButtonId + "']");
            thisTravelerInputContainer.continueButton = $('#' + this.continueButtonId);
            thisTravelerInputContainer.nextButton = $("input[type='button'][id='" + this.nextButtonId + "']");
            thisTravelerInputContainer.refreshFriendsButton = $("input[id='" + this.refreshFriendsButtonId + "']");

            thisTravelerInputContainer.passportNumber = $('#' + this.passportNumber + ' input[id*=TextBoxDocumentNumber]');
            thisTravelerInputContainer.passportExpDate = $('#' + this.passportExpDate + ' select[id*=DropDownListDocumentDateDay]');
            thisTravelerInputContainer.passportExpMonth = $('#' + this.passportExpMonth + ' select[id*=DropDownListDocumentDateMonth]');
            thisTravelerInputContainer.passportExpYear = $('#' + this.passportExpYear + ' select[id*=DropDownListDocumentDateYear]');
        };

        thisTravelerInputContainer.addEvents = function() {
            thisTravelerInputContainer.submitButton.click(thisTravelerInputContainer.pickSeatHandler);
            thisTravelerInputContainer.nextButton.click(thisTravelerInputContainer.nextTabHandler);
            thisTravelerInputContainer.addFriendsButton.click(thisTravelerInputContainer.addFriends);
            thisTravelerInputContainer.refreshFriendsButton.click(thisTravelerInputContainer.clearAllFriends);

            if (thisTravelerInputContainer.confirmText == null || thisTravelerInputContainer.confirmText == "") {
                thisTravelerInputContainer.continueButton.attr("value", thisTravelerInputContainer.continueButtonText);
                thisTravelerInputContainer.continueButton.click(thisTravelerInputContainer.pickSeatHandler);
            }

            else {
                thisTravelerInputContainer.continueButton.click(thisTravelerInputContainer.confirmPageHandler);
            }
        };

        thisTravelerInputContainer.pickSeatHandler = function() {
            var results = thisTravelerInputContainer.validateForms();
            if (results != false) SKYSALES.DisplayLoadingBar(); //added by Linson at 2011-12-31
            return results;
        };

        thisTravelerInputContainer.addFriends = function() {
            var friendsToAdd = $('#' + thisTravelerInputContainer.addFriendsDiv + ' :checked').length;
            //revert to former remaining tabs
            var j = 0;
            for (j = 0; j < thisTravelerInputContainer.paxTypesArray.length; j++) {
                thisTravelerInputContainer.paxTypesArray[j].remainingTabs = thisTravelerInputContainer.countPaxType(thisTravelerInputContainer.paxTypesArray[j].paxType);
            }

            var isWithinPaxCount = thisTravelerInputContainer.checkPaxTypeCount();
            if (friendsToAdd > 0 && isWithinPaxCount) {
                if ($('#' + thisTravelerInputContainer.addFriendsDiv + ' :checked').length > 0) {
                    thisTravelerInputContainer.clearAllFriends();
                    SKYSALES.RemoveLoadingBar(); //added by Linson at 2012-01-04
                    var ctrFriend = 0;
                    $('#' + thisTravelerInputContainer.addFriendsDiv + ' :checked').each(
                        function() {
                            var ctr = parseInt($(this).val());
                            var frndTabIndex = thisTravelerInputContainer.uploadFriend(ctr);
                            firstPaxTabIndex = ctrFriend == 0 ? frndTabIndex
                                    : (firstPaxTabIndex > frndTabIndex ? frndTabIndex : firstPaxTabIndex);
                            ctrFriend++;
                        }
                    );
                    thisTravelerInputContainer.travelerInputArray[firstPaxTabIndex].toggleMenuHandler();
                }
            }
            else if (friendsToAdd == 0) {
                alert(thisTravelerInputContainer.noTickedFriendsText);
            }
            else if (friendsToAdd > (thisTravelerInputContainer.travelerInputArray.length - 2)) {
                alert(thisTravelerInputContainer.addFriendsErrorText);
            }
            else if (!isWithinPaxCount) {
                alert(thisTravelerInputContainer.unmatchedPaxTypeErrorText);
            }
        };

        thisTravelerInputContainer.checkPaxTypeCount = function() {
            var counter = 0;
            for (counter = 0; counter < thisTravelerInputContainer.paxTypesArray.length; counter++) {

                var paxType = thisTravelerInputContainer.paxTypesArray[counter].paxType;
                var iteration = 0;
                var isWithinCount = false;
                var friendsCount = $('input[paxType=' + paxType + ']:checked').length;

                if (friendsCount > 0) {
                    var codesToMatch = thisTravelerInputContainer.paxTypesArray[counter].codesMatchArray;
                    for (iteration = 0; iteration < codesToMatch.length; iteration++) {
                        var paxTypeIndex = thisTravelerInputContainer.searchPaxTypeIndex(codesToMatch[iteration].typeCode);
                        if (friendsCount <= thisTravelerInputContainer.paxTypesArray[paxTypeIndex].remainingTabs) {
                            isWithinCount = true;
                            thisTravelerInputContainer.paxTypesArray[paxTypeIndex].remainingTabs -= friendsCount;
                            break;
                        }
                        else if (thisTravelerInputContainer.paxTypesArray[paxTypeIndex].remainingTabs > 0
                    && friendsCount > thisTravelerInputContainer.paxTypesArray[paxTypeIndex].remainingTabs) {
                            friendsCount -= thisTravelerInputContainer.paxTypesArray[paxTypeIndex].remainingTabs;
                            thisTravelerInputContainer.paxTypesArray[paxTypeIndex].remainingTabs = 0;
                        }

                    }


                    if (isWithinCount == false)
                        return false;
                }
            }
            return true;

        };

        thisTravelerInputContainer.searchPaxTypeIndex = function(type) {
            var ctr = 0;
            for (ctr = 0; ctr < thisTravelerInputContainer.paxTypesArray.length; ctr++) {
                if (thisTravelerInputContainer.paxTypesArray[ctr].paxType == type) {
                    return ctr;
                }
            }

        };

        var searchTheNextPax = function(paxType, isFriend) {
            var paxTypeCtr = 0;
            for (paxTypeCtr = 0; paxTypeCtr < thisTravelerInputContainer.paxTypesArray.length; paxTypeCtr++) {
                if (thisTravelerInputContainer.paxTypesArray[paxTypeCtr].paxType == paxType) {
                    var iteration = 0;
                    var codesToMatch = thisTravelerInputContainer.paxTypesArray[paxTypeCtr].codesMatchArray;

                    for (iteration = 0; iteration < codesToMatch.length; iteration++) {
                        var paxTabIndex = thisTravelerInputContainer.findPaxType(codesToMatch[iteration].typeCode, isFriend);
                        if (paxTabIndex != null)
                            return paxTabIndex;
                    }
                    break;
                }
            }
            //by default, return the first pax tab
            return thisTravelerInputContainer.findPaxType(thisTravelerInputContainer.paxTypesArray[0].paxType, isFriend);
        };

        var searchFriend = function(sequence) {
            var friendCtr = 0;
            var chosenFriend = 0;
            for (friendCtr = 0; friendCtr < thisTravelerInputContainer.friendsArray.length; friendCtr++) {
                if (thisTravelerInputContainer.friendsArray[friendCtr].sequence == sequence) {
                    chosenFriend = friendCtr;
                    break;
                }
            }
            return chosenFriend;
        };

        thisTravelerInputContainer.clearAllFriends = function() {
            var i = 0;
            SKYSALES.DisplayLoadingBar(); //added by Linson at 2012-01-04
            for (i = 2; i < thisTravelerInputContainer.travelerInputArray.length; i++) {
                    thisTravelerInputContainer.travelerInputArray[i].clearFriend();
                }
        };

        thisTravelerInputContainer.uploadFriend = function(friendIndex) {

            var friend = friendIndex > 0 ? this.friendsArray[searchFriend(friendIndex)] : contactData;
            var friendTabIndex = friendIndex > 0 ? searchTheNextPax(friend.paxType, false)
                            : searchTheNextPax("ADT", false);
            var guestTab = this.travelerInputArray[friendTabIndex];
            var customerNumber = friendIndex > 0 ? friend.customerNumber : friend.ff;

            guestTab.hasFriend = true;

            //guestTab.customerNumber.val(customerNumber);
            guestTab.travelerName.firstName.val(friend.firstName);
            guestTab.travelerName.lastName.val(friend.lastName);
            guestTab.dropdownTitle.val(friend.title);
            guestTab.travelerName.writeNameHandler();
            guestTab.dropdownNationality.val(friend.nationality);

            guestTab.dropdownGender.val(friend.gender);

            guestTab.dropdownBirthDateMonth.val(friend.bdayMonth);
            guestTab.dropdownBirthDateDay.val(friend.bdayDay);
            guestTab.dropdownBirthDateYear.val(friend.bdayYear);

            guestTab.passportNumber.val(friend.documentNumber);
            guestTab.passportCountry.val(friend.documentCountry);
            guestTab.passportExpDate.val(friend.documentDay);
            guestTab.passportExpMonth.val(friend.documentMonth);
            guestTab.passportExpYear.val(friend.documentYear);

            return friendTabIndex;
        };

        thisTravelerInputContainer.validateForms = function() {
            var action = this.continueButton.get(0);
            if (action == null) {
                action = this.submitButton.get(0);
            }

            if (thisTravelerInputContainer.validateInfants()) {
                var results = validate(action);
                if (!results) {
                    this.customValidateHandler();
                }
                if (thisTravelerInputContainer.validatePassportExps())
                    return results;
                else
                    return false;
            }

            return false;

        };

        thisTravelerInputContainer.validateInfants = function() {
            var i = 0;
            var infant = 0;
            for (i = 0; i < thisTravelerInputContainer.travelerInputArray.length; i++) {
                if ((thisTravelerInputContainer.travelerInputArray[i].paxType == "INF" || thisTravelerInputContainer.travelerInputArray[i].paxType == "CHD") && !validateInfant(i)) {
                    return false;
                }
            }
            return true;
        };

        thisTravelerInputContainer.validatePassportExps = function() {
            var i = 0;
            for (i = 0; i < thisTravelerInputContainer.travelerInputArray.length; i++) {
                if (!validatePassportExpDate(i)) {
                    return false;
                }
            }
            return true;
        };

        thisTravelerInputContainer.confirmPageHandler = function() {
            var cpVal = thisTravelerInputContainer.confirmPage();
            return cpVal;
        };

        thisTravelerInputContainer.confirmPage = function() {
            var confirmAns = window.confirm(this.confirmText);
            if (confirmAns) {
                document.getElementById(this.submitButtonId).click();
                return false;
            }
            else {
                var results = this.validateForms();
                if (results != false) SKYSALES.DisplayLoadingBar(); //added by Linson at 2011-12-31
                return results;
            }
        };

        thisTravelerInputContainer.customValidateHandler = function() {
            // should be done after validation
            //check the container's input if class has validationError then toggle
            var inputWithErrors = null;
            for (i = 0; i < thisTravelerInputContainer.travelerInputArray.length; i += 1) {
                inputWithErrors = $('#' + thisTravelerInputContainer.travelerInputArray[i].contentId + ' :input.validationError, :select.validationError, :radio.validationError');
                if (inputWithErrors != null && inputWithErrors.length > 0) {
                    thisTravelerInputContainer.travelerInputArray[i].toggleMenuHandler();
                    inputWithErrors[0].focus();
                    break;
                }
            }
        };

        thisTravelerInputContainer.nextTabHandler = function() {
            // check and hide which tab is selected, show the next tab
            var currentTab = null;
            var i = 0;
            for (i = 0; i < thisTravelerInputContainer.travelerInputArray.length; i += 1) {
                currentTab = thisTravelerInputContainer.travelerInputArray[i];

                if (currentTab.selectedTab == 0) {
                    if (validateInfant(i) && validateBySelector("div[id='" + currentTab.contentId + "']")) {
                        currentTab.travelerInputArray[i].hideContent();
                        thisTravelerInputContainer.travelerInputArray[i + 1].showContent();
                    }
                    break;
                }

            }
            return false;
        };

        var validatePassportExpDate = function(ctr) {
            if (thisTravelerInputContainer.travelerInputArray[ctr].passportNumber.val() != "" && thisTravelerInputContainer.travelerInputArray[ctr].passportNumber.length > 0) {

                var returnDateArr = thisTravelerInputContainer.returnDate.split("/");
                var returnDay = new Date(returnDateArr[2], returnDateArr[0] - 1, returnDateArr[1]);
                returnDay.setDate(returnDay.getDate() + parseInt(thisTravelerInputContainer.passportExpDays));

                var adultIndex = thisTravelerInputContainer.findPaxType("ADT", null);
                var childIndex = thisTravelerInputContainer.findPaxType("CHD", null);
                var infantIndex = thisTravelerInputContainer.findPaxType("INF", null);
                if (thisTravelerInputContainer.travelerInputArray[ctr].paxType == "ADT") {
                    if (thisTravelerInputContainer.travelerInputArray[ctr].passportExpYear.val() != "" && thisTravelerInputContainer.travelerInputArray[ctr].passportExpMonth.val() != "" && thisTravelerInputContainer.travelerInputArray[ctr].passportExpDate.val() != "") {
                        var expDate = new Date(),
                        year = thisTravelerInputContainer.travelerInputArray[ctr].passportExpYear.val(),
                        month = thisTravelerInputContainer.travelerInputArray[ctr].passportExpMonth.val(),
                        day = thisTravelerInputContainer.travelerInputArray[ctr].passportExpDate.val();
                        expDate.setFullYear(year, month, day);
                        if (returnDay > expDate) {

                            alert(thisTravelerInputContainer.adultPassportMsgPreText + ((ctr - adultIndex) + 1) + thisTravelerInputContainer.adultPassportMsgText);
                            return true;
                        }
                        return true;
                    }
                    return true;
                } else if (thisTravelerInputContainer.travelerInputArray[ctr].paxType == "CHD") {
                    if (thisTravelerInputContainer.travelerInputArray[ctr].passportExpYear.val() != "" && thisTravelerInputContainer.travelerInputArray[ctr].passportExpMonth.val() != "" && thisTravelerInputContainer.travelerInputArray[ctr].passportExpDate.val() != "") {
                        var expDate = new Date();
                        expDate.setFullYear(thisTravelerInputContainer.travelerInputArray[ctr].passportExpYear.val(), (thisTravelerInputContainer.travelerInputArray[ctr].passportExpMonth.val() - 1), thisTravelerInputContainer.travelerInputArray[ctr].passportExpDate.val());
                        if (returnDay > expDate) {

                            alert(thisTravelerInputContainer.childPassportMsgPreText + ((ctr - childIndex) + 1) + thisTravelerInputContainer.childPassportMsgText);
                            return true;
                        }
                        return true;
                    }
                    return true;
                } else if (thisTravelerInputContainer.travelerInputArray[ctr].paxType == "INF") {
                    if (thisTravelerInputContainer.travelerInputArray[ctr].passportExpYear.val() != "" && thisTravelerInputContainer.travelerInputArray[ctr].passportExpMonth.val() != "" && thisTravelerInputContainer.travelerInputArray[ctr].passportExpDate.val() != "") {
                        var expDate = new Date();
                        expDate.setFullYear(thisTravelerInputContainer.travelerInputArray[ctr].passportExpYear.val(), (thisTravelerInputContainer.travelerInputArray[ctr].passportExpMonth.val() - 1), thisTravelerInputContainer.travelerInputArray[ctr].passportExpDate.val());
                        if (returnDay > expDate) {

                            alert(thisTravelerInputContainer.infantPassportMsgPreText + ((ctr - infantIndex) + 1) + thisTravelerInputContainer.infantPassportMsgText);
                            return true;
                        }
                        return true;
                    }
                    return true;
                }
                return true;
            }
            return true;
        };
        var validateInfant = function(ctr) {
            var infant = 0;
            var childs = 0;
            var infantIndex = thisTravelerInputContainer.findPaxType("INF", null);
            var childIndex = thisTravelerInputContainer.findPaxType("CHD", null);
            if (thisTravelerInputContainer.travelerInputArray[ctr].paxType == "INF") {
                if (!thisTravelerInputContainer.travelerInputArray[ctr].travelerAge.isNotFutureDate()) {
                    thisTravelerInputContainer.travelerInputArray[ctr].toggleMenuHandler();
                    infant = (ctr - infantIndex) + 1;
                    alert(thisTravelerInputContainer.infantFutureBirthDateErrorPreText + infant + thisTravelerInputContainer.infantFutureBirthDateErrorPostText);
                    return false;
                }
                else if (!thisTravelerInputContainer.travelerInputArray[ctr].travelerAge.isInfant(thisTravelerInputContainer.returnDate, null, thisTravelerInputContainer.departDate)) {
                    thisTravelerInputContainer.travelerInputArray[ctr].toggleMenuHandler();
                    infant = (ctr - infantIndex) + 1;
                    alert(thisTravelerInputContainer.infantPreText + infant + thisTravelerInputContainer.infantText);
                    return false;
                }


                for (i = 0; i < thisTravelerInputContainer.travelerInputArray.length; i++) {
                    if ((thisTravelerInputContainer.travelerInputArray[i].paxType == "INF") && (i != ctr)
                        && (thisTravelerInputContainer.travelerInputArray[i].travelerCompany.val() == thisTravelerInputContainer.travelerInputArray[ctr].travelerCompany.val())) {
                        alert(thisTravelerInputContainer.duplicateAdultTextPre + (i - infantIndex + 1) + thisTravelerInputContainer.duplicateAdultText);
                        return false;
                    }
                }
            }

            else if (thisTravelerInputContainer.travelerInputArray[ctr].paxType == "CHD") {

                var msg = "";
                var alertAction = true;
                if (!thisTravelerInputContainer.travelerInputArray[ctr].travelerAge.isNotFutureDate()) {
                    thisTravelerInputContainer.travelerInputArray[ctr].toggleMenuHandler();
                    childs = (ctr - childIndex) + 1;
                    alert(thisTravelerInputContainer.childPreText + childs + thisTravelerInputContainer.infantFutureBirthDateErrorPostText);
                    return false;
                } 
                else if (!thisTravelerInputContainer.travelerInputArray[ctr].travelerAge.isChild(thisTravelerInputContainer.returnDate, null)) {
                    thisTravelerInputContainer.travelerInputArray[ctr].toggleMenuHandler();
                    childs = (ctr - childIndex) + 1;
                    alert(thisTravelerInputContainer.childPreText + childs + thisTravelerInputContainer.childText);
                    return false;
                }
            }

            return true;
        };

        thisTravelerInputContainer.findPaxType = function(type, isFriend) {
            var index = null;
            var ctr = 0;
            for (ctr = 0; ctr < thisTravelerInputContainer.travelerInputArray.length; ctr++) {
                var isIndex = isFriend != null
                    ? thisTravelerInputContainer.travelerInputArray[ctr].paxType == type
                            && thisTravelerInputContainer.travelerInputArray[ctr].hasFriend == isFriend
                    : thisTravelerInputContainer.travelerInputArray[ctr].paxType == type;
                if (isIndex == true) {
                    index = ctr;
                    break;
                }

            }
            return index;
        };

        thisTravelerInputContainer.countPaxType = function(type) {
            var index = 0;
            var ctr = 0;
            for (ctr = 2; ctr < thisTravelerInputContainer.travelerInputArray.length; ctr++) {
                if (thisTravelerInputContainer.travelerInputArray[ctr].paxType == type) {
                    index++;
                }
            }
            return index;
        };

        thisTravelerInputContainer.setSettingsByObject = function(json) {
            parent.setSettingsByObject.call(this, json);

            var i = 0;
            var travelerInputArray = this.travelerInputArray || [];
            var travelerInput = null;
            for (i = 0; i < travelerInputArray.length; i += 1) {
                travelerInput = new SKYSALES.Class.TravelerInput();
                travelerInput.init(travelerInputArray[i]);

                travelerInputArray[i] = travelerInput;

            }
            //initializing the friends array
            if (this.friendsArray != null) {
                i = 0;
                var friend = null;
                for (i = 0; i < this.friendsArray.length; i += 1) {
                    friend = new SKYSALES.Class.Friend();
                    friend.init(this.friendsArray[i]);
                }

            }
            thisTravelerInputContainer.travelerInputArray = travelerInputArray;
        };

        return thisTravelerInputContainer;
    };

    SKYSALES.Class.TravelerInputContainer.createObject = function(json) {
        SKYSALES.Util.createObject('travelerInputContainer', 'TravelerInputContainer', json);
    };
}


/*
Name: 
Class TravelerInput
Param:
None
Return: 
An instance of TravelerInput
Functionality:
This class represents the Link and the Div container of each component
in the traveler page
Notes:
Class Hierarchy:
SkySales -> TravelerInput
*/
if (!SKYSALES.Class.TravelerInput) {
    SKYSALES.Class.TravelerInput = function() {
        var parent = SKYSALES.Class.SkySales(),
        thisTravelerInput = SKYSALES.Util.extendObject(parent);

        thisTravelerInput.linkId = "";
        thisTravelerInput.link = null;
        thisTravelerInput.listId = "";
        thisTravelerInput.list = null;
        thisTravelerInput.contentId = "";
        thisTravelerInput.content = null;
        thisTravelerInput.tabId = "";
        thisTravelerInput.tab = null;
        thisTravelerInput.tabStart = 0;
        thisTravelerInput.travelerInputArray = null;  //so as to access the other travelerInput objects
        thisTravelerInput.travelerName = null;
        thisTravelerInput.travelerAge = null;
        thisTravelerInput.travelerCompany = null;
        thisTravelerInput.selectedTab = 0;
        thisTravelerInput.imThisTravellerCheckboxId = "";
        thisTravelerInput.imThisTravellerCheckbox = null;
        thisTravelerInput.paxType = "";
        thisTravelerInput.docType = null;
        thisTravelerInput.passportNumber = null;
        thisTravelerInput.passportCountry = null;

        thisTravelerInput.passportExpDate = null;
        thisTravelerInput.passportExpMonth = null;
        thisTravelerInput.passportExpYear = null;

        thisTravelerInput.dropdownTitleId = "";
        thisTravelerInput.dropdownGenderId = "";
        thisTravelerInput.dropdownTitle = null;
        thisTravelerInput.dropdownGender = null;
        thisTravelerInput.dropdownNationalityId = "";
        thisTravelerInput.dropdownNationality = null;
        thisTravelerInput.dropdownBirthDateMonthId = "";
        thisTravelerInput.dropdownBirthDateMonth = null;
        thisTravelerInput.dropdownBirthDateDayId = "";
        thisTravelerInput.dropdownBirthDateDay = null;
        thisTravelerInput.dropdownBirthDateYearId = "";
        thisTravelerInput.dropdownBirthDateYear = null;

        thisTravelerInput.customerNumberId = "";
        thisTravelerInput.customerNumber = null;
        thisTravelerInput.hasFriend = false;
        thisTravelerInput.setTabIndexes = function() {
            thisTravelerInput.link.attr('tabindex', thisTravelerInput.tabStart);
            var travelerArray = $('#' + thisTravelerInput.contentId + ' :input:visible, :select:visible, :radio:visible');
            for (i = 0; i < travelerArray.length; i++) {
                $(travelerArray[i]).attr('tabindex', thisTravelerInput.tabStart + i + 1)
                travelerArray[i].setAttribute('tabindex', thisTravelerInput.tabStart + i + 1);
            }
        }
        thisTravelerInput.init = function(json) {
            this.setSettingsByObject(json);
            this.setVars();
            this.addEvents();

            //initialize tab index
            //we delay the execution of this to avoid the IE scprit warning
            setTimeout(thisTravelerInput.setTabIndexes, 2000);

            //initialize traveler age object
            var travelerAge = null,
            month = $('#' + thisTravelerInput.contentId + ' :input[id*=DropDownListBirthDateMonth]'),
            day = $('#' + thisTravelerInput.contentId + ' :input[id*=DropDownListBirthDateDay]'),
            year = $('#' + thisTravelerInput.contentId + ' :input[id*=DropDownListBirthDateYear]');
            travelerAge = new SKYSALES.Class.TravelerAge();
            travelerAge.init(month, day, year);
            thisTravelerInput.travelerAge = travelerAge;

            //initialize infant's company
            if (this.paxType == "INF")
                thisTravelerInput.travelerCompany = $('#' + thisTravelerInput.contentId + ' :input[id*=DropDownListAssign]');

            if ($('#' + thisTravelerInput.contentId + ' :input:text[id*=TextBoxFirstName]').val() == "") {
                $('#' + thisTravelerInput.contentId + ' :input:text[id*=TextBoxFirstName]').val($('#' + thisTravelerInput.contentId + ' :input:text[id*=TextBoxLastName]').val());
            }

            //initialize traveler name object
            var travelerName = null,
            firstName = $('#' + thisTravelerInput.contentId + ' :input:text[id*=TextBoxFirstName]'),
            lastName = $('#' + thisTravelerInput.contentId + ' :input:text[id*=TextBoxLastName]'),
            nameLink = thisTravelerInput.tab;

            if (firstName.length > 0 || lastName.length > 0) {
                travelerName = new SKYSALES.Class.TravelerName();
                travelerName.init(firstName, lastName, nameLink);
                thisTravelerInput.travelerName = travelerName;
            }

            var dropDownNationality = $('#' + thisTravelerInput.contentId + ' select[id*=DropDownListNationality]');

            if (dropDownNationality.length > 0) { thisTravelerInput.populateCountryInput(dropDownNationality); }

            thisTravelerInput.passportNumber = $('#' + thisTravelerInput.contentId + ' :input:text[id*=TextBoxDocumentNumber]');

            thisTravelerInput.docType = $('#' + thisTravelerInput.contentId + ' :input:hidden[id*=DropDownListDocumentType]');
            if (thisTravelerInput.passportNumber.length > 0) {
                thisTravelerInput.passportNumber.keyup(this.addPassportHandler);
            }

            thisTravelerInput.passportCountry = $('#' + thisTravelerInput.contentId + ' select[id*=DropDownListDocumentCountry]');
            if (thisTravelerInput.passportCountry.length > 0) {
                thisTravelerInput.populateCountryInput(thisTravelerInput.passportCountry);
            }

            thisTravelerInput.passportExpDate = $('#' + thisTravelerInput.contentId + ' select[id*=DropDownListDocumentDateDay]');
            thisTravelerInput.passportExpMonth = $('#' + thisTravelerInput.contentId + ' select[id*=DropDownListDocumentDateMonth]');
            thisTravelerInput.passportExpYear = $('#' + thisTravelerInput.contentId + ' select[id*=DropDownListDocumentDateYear]');
        }

        thisTravelerInput.disableTextFields = function() {
            thisTravelerInput.travelerName.disable();
        };

        thisTravelerInput.populateCountryInput = function(countrySelect) {

            var country = $('#' + countrySelect[0].id + '_value').val();

            var countryList = SKYSALES.Util.getResource().countryInfo.CountryList;

            var selectParamObj = {
                'selectBox': countrySelect,
                'objectArray': countryList,
                'selectedItem': country,
                'showCode': false
            };

            SKYSALES.Util.populateSelect(selectParamObj);
        }

        thisTravelerInput.setVars = function() {
            thisTravelerInput.link = $('#' + this.linkId);
            thisTravelerInput.list = $('#' + this.listId);
            thisTravelerInput.content = $('#' + this.contentId);
            thisTravelerInput.tab = $('#' + this.tabId);
            thisTravelerInput.imThisTravellerCheckbox = $('#' + this.imThisTravellerCheckboxId);
            thisTravelerInput.dropdownTitle = $('#' + this.dropdownTitleId);
            thisTravelerInput.dropdownGender = $('#' + this.dropdownGenderId);
            thisTravelerInput.dropdownBirthDateMonth = $('#' + this.dropdownBirthDateMonthId);
            thisTravelerInput.dropdownBirthDateDay = $('#' + this.dropdownBirthDateDayId);
            thisTravelerInput.dropdownBirthDateYear = $('#' + this.dropdownBirthDateYearId);
            thisTravelerInput.dropdownNationality = $('#' + this.dropdownNationalityId);
            thisTravelerInput.customerNumber = $('#' + this.customerNumberId);
        };

        thisTravelerInput.addEvents = function() {
            //post here association of action to showing contentDivId            
            thisTravelerInput.link.focus(this.toggleMenuHandler);
            thisTravelerInput.link.click(this.toggleMenuHandler);
            thisTravelerInput.imThisTravellerCheckbox.click(this.populateTravelerTabHandler);
            thisTravelerInput.dropdownTitle.change(this.updateGender);
            thisTravelerInput.dropdownGender.change(this.updateTitle);
            thisTravelerInput.dropdownBirthDateMonth.change(this.updateDays);
            thisTravelerInput.dropdownBirthDateYear.change(this.updateDays);
        };

        thisTravelerInput.updateGender = function() {
            var selectedTitle = thisTravelerInput.dropdownTitle.val();
            var titleList = SKYSALES.Util.getResource().titleInfo.TitleList;

            for (i = 0; i < titleList.length; i += 1) {
                title = titleList[i];

                if (selectedTitle == title.TitleKey) {
                    thisTravelerInput.dropdownGender.val(title.GenderCode);

                    break;
                }
            }
        };

        thisTravelerInput.clearFriend = function() {
            thisTravelerInput.customerNumber.val('');
            thisTravelerInput.travelerName.firstName.val('');
            thisTravelerInput.travelerName.lastName.val('');
            thisTravelerInput.dropdownTitle.val('');
            thisTravelerInput.travelerName.writeNameHandler();
            thisTravelerInput.dropdownNationality.val('');

            thisTravelerInput.dropdownGender.val('');

            thisTravelerInput.dropdownBirthDateMonth.val('');
            thisTravelerInput.dropdownBirthDateDay.val('');
            thisTravelerInput.dropdownBirthDateYear.val('');

            thisTravelerInput.passportNumber.val('');
            thisTravelerInput.passportCountry.val('');
            thisTravelerInput.passportExpDate.val('');
            thisTravelerInput.passportExpMonth.val('');
            thisTravelerInput.passportExpYear.val('');
            thisTravelerInput.hasFriend = false;
        };

        thisTravelerInput.updateTitle = function() {
            var selectedGender = thisTravelerInput.dropdownGender.val();
            var selectedTitle = thisTravelerInput.dropdownTitle.val();
            var titleList = SKYSALES.Util.getResource().titleInfo.TitleList;

            for (i = 0; i < titleList.length; i += 1) {
                title = titleList[i];

                if (selectedTitle != "CHD" &&
                    selectedGender == title.GenderCode) {
                    thisTravelerInput.dropdownTitle.val(title.TitleKey);

                    break;
                }
            }
        };
        thisTravelerInput.updateDays = function() {
            var month = thisTravelerInput.dropdownBirthDateMonth.val() - 1; //months are 0 based
            var year = thisTravelerInput.dropdownBirthDateYear.val();
            var daysInMonth = 32 - new Date(year, month, 32).getDate();
            var selectedDay = thisTravelerInput.dropdownBirthDateDay.val();
            var currentNumDays = thisTravelerInput.dropdownBirthDateDay[0].length - 1;
            if (currentNumDays < daysInMonth) {
                for (i = currentNumDays + 1; i <= daysInMonth; i += 1) {

                    thisTravelerInput.dropdownBirthDateDay.append('<option value="' + i + '">' + i + '</option>');

                }

            }
            else if (currentNumDays > daysInMonth) {
                for (i = currentNumDays; i > daysInMonth; i -= 1) {

                    $('#' + thisTravelerInput.dropdownBirthDateDayId + ' option:eq(' + i + ')').remove();

                }
            }

            if (selectedDay != '' && selectedDay <= daysInMonth) {
                thisTravelerInput.dropdownBirthDateDay.val(selectedDay);
            }
            else {
                thisTravelerInput.dropdownBirthDateDay.val("");
            }




        };

        thisTravelerInput.addPassportHandler = function() {
            if (thisTravelerInput.passportNumber != null && thisTravelerInput.passportNumber.length > 0) {
                if (thisTravelerInput.docType != null && thisTravelerInput.docType.length > 0) {
                    var pnum = thisTravelerInput.passportNumber.val() + '';
                    var trimmed_pnum = pnum.replace(/^\s+|\s+$/g, '');
                    if (trimmed_pnum.length > 0) {
                        thisTravelerInput.docType.val('P');
                    }
                    else {
                        thisTravelerInput.docType.val('');
                    }
                }
            }
        }

        thisTravelerInput.populateTravelerTabHandler = function() {
            thisTravelerInput.travelerName.writeNameHandler();
        }

        thisTravelerInput.toggleMenuHandler = function() {
            //hide other traveler input
            if (thisTravelerInput.travelerInputArray !== null) {
                for (i = 0; i < thisTravelerInput.travelerInputArray.length; i += 1) {
                    thisTravelerInput.travelerInputArray[i].hideContent();
                }
                thisTravelerInput.showContent();
            }
        }

        thisTravelerInput.showContent = function() {
            //show self
            thisTravelerInput.list.addClass("selected");
            thisTravelerInput.content.show();
            thisTravelerInput.selectedTab = 0;
        }

        thisTravelerInput.hideContent = function() {
            thisTravelerInput.list.removeClass("selected");
            thisTravelerInput.content.hide();
            thisTravelerInput.selectedTab = 1;
        }

        thisTravelerInput.setSettingsByObject = function(json) {
            parent.setSettingsByObject.call(this, json);
        };

        return thisTravelerInput;
    };

    SKYSALES.Class.TravelerInput.createObject = function(json) {
        SKYSALES.Util.createObject('travelerInput', 'TravelerInput', json);
    };
}

/*
Name: 
Class TravelerName
Param:
None
Return: 
An instance of TravelerName
Functionality:
This class represents the Traveler Name (whether First Middle or Last Name).
Notes:
Class Hierarchy:
SkySales -> TravelerName
*/
if (!SKYSALES.Class.TravelerName) {
    SKYSALES.Class.TravelerName = function() {
        var parent = SKYSALES.Class.SkySales(),
        thisTravelerName = SKYSALES.Util.extendObject(parent);

        thisTravelerName.firstName = null;
        thisTravelerName.lastName = null;
        thisTravelerName.nameLink = null;
        thisTravelerName.init = function(firstName, lastName, nameLink) {
            //this.setSettingsByObject(json);
            thisTravelerName.firstName = firstName;
            thisTravelerName.lastName = lastName;
            thisTravelerName.nameLink = nameLink;

            this.addEvents();

            // added for displaying the contact and passenger names on load
            this.writeNameHandler();

        };

        thisTravelerName.disable = function() {
            thisTravelerName.firstName.attr("readonly", true);
            thisTravelerName.lastName.attr("readonly", true);
        };

        thisTravelerName.addEvents = function() {
            //post here association of action to showing contentDivId
            thisTravelerName.firstName.change(this.writeNameHandler);
            thisTravelerName.lastName.change(this.writeNameHandler);
        };

        thisTravelerName.writeNameHandler = function() {
            var separator = ", ";
            if (thisTravelerName.lastName.val() === "" || thisTravelerName.firstName.val() === "") {
                separator = "";
            }
            thisTravelerName.nameLink.text(thisTravelerName.lastName.val() + separator + thisTravelerName.firstName.val());
        }
        return thisTravelerName;
    };

    SKYSALES.Class.TravelerName.createObject = function(json) {
        SKYSALES.Util.createObject('travelerName', 'TravelerName', json);
    };
}


/*
Name: 
Class TravelerAge
Param:
None
Return: 
An instance of TravelerAge
Functionality:
This class represents the Traveler Age.
Notes:
Class Hierarchy:
SkySales -> TravelerName
*/
if (!SKYSALES.Class.TravelerAge) {
    SKYSALES.Class.TravelerAge = function() {
        var parent = SKYSALES.Class.SkySales(),
        thisTravelerAge = SKYSALES.Util.extendObject(parent);

        thisTravelerAge.dateMonth = null;
        thisTravelerAge.dateDay = null;
        thisTravelerAge.dateYear = null;

        thisTravelerAge.init = function(dateMonth, dateDay, dateYear) {
            //this.setSettingsByObject(json);
            thisTravelerAge.dateMonth = dateMonth;
            thisTravelerAge.dateDay = dateDay;
            thisTravelerAge.dateYear = dateYear;

        };

        thisTravelerAge.isInfant = function(date1, date2, departDate) {
            var now = new Date(date1);
            var birthday = null;
            if (date2 == null) {
                birthday = new Date(parseInt(thisTravelerAge.dateYear.val()), parseInt(thisTravelerAge.dateMonth.val()) - 1, parseInt(thisTravelerAge.dateDay.val()));
            }
            else
                birthday = new Date(date2);

            if (!isNaN(birthday) && !isNaN(now)) {
                var age = thisTravelerAge.computeAge(now, birthday);
                var depart = new Date(departDate);
                var one_day = 1000 * 60 * 60 * 24;
                var DepartAge = Math.ceil((depart.getTime() - birthday.getTime()) / (one_day));
                
                if (!(age < 2) || !(DepartAge > 8))
                    return false;
            }
            return true;
        };

        thisTravelerAge.isChild = function(date1, date2) {
            var now = new Date(date1);
            var birthday = null;
            if (date2 == null) {
                birthday = new Date(parseInt(thisTravelerAge.dateYear.val()), parseInt(thisTravelerAge.dateMonth.val()) - 1, parseInt(thisTravelerAge.dateDay.val()));
            }
            else
                birthday = new Date(date2);

            if (!isNaN(birthday) && !isNaN(now)) {
                var age = thisTravelerAge.computeAge(now, birthday);
                if (!(age < 12))
                    return false;
            }
            return true;
        };

//Added by Bryan, 16 May 2012
        thisTravelerAge.isAdult = function(date1, date2) {
            var now = new Date(date1);
            var birthday = null;
            if (date2 == null) {
                birthday = new Date(parseInt(thisTravelerAge.dateYear.val()), parseInt(thisTravelerAge.dateMonth.val()) - 1, parseInt(thisTravelerAge.dateDay.val()));
            }
            else
                birthday = new Date(date2);

            if (!isNaN(birthday) && !isNaN(now)) {
                var age = thisTravelerAge.computeAge(now, birthday);
                if (!(age >= 12))
                    return false;
            }
            return true;
        };

        thisTravelerAge.isNotFutureDate = function() {
            var birthday = new Date(parseInt(thisTravelerAge.dateYear.val()), parseInt(thisTravelerAge.dateMonth.val()) - 1, parseInt(thisTravelerAge.dateDay.val()));
            var now = new Date();

            if (!isNaN(birthday))
                return (birthday < now);

            return true;
        }

        thisTravelerAge.computeAge = function(date1, date2) {
            if (date1 > date2) {
                var age = date1.getFullYear() - date2.getFullYear();
                if (date1.getMonth() < date2.getMonth() || (date1.getMonth() <= date2.getMonth() && date1.getDate() < date2.getDate())) {
                    age--;
                }
                return age;
            }
            return null;
        };

        return thisTravelerAge;
    };

    SKYSALES.Class.TravelerAge.createObject = function(json) {
        SKYSALES.Util.createObject('TravelerAge', 'TravelerAge', json);
    };
}

/*
Name: 
Class Friend
Param:
None
Return: 
An instance of Friend
Functionality:
This class represents the contact's Friend. 
It would simply contain a possible guest information.
Notes:
Class Hierarchy:
SkySales -> Friend
*/
if (!SKYSALES.Class.Friend) {
    SKYSALES.Class.Friend = function() {
        var parent = SKYSALES.Class.SkySales(),
        thisFriend = SKYSALES.Util.extendObject(parent);

        thisFriend.customerNumber = null;
        thisFriend.firstName = null;
        thisFriend.lastName = null;
        thisFriend.bdayDay = null;
        thisFriend.bdayMonth = null;
        thisFriend.bdayYear = null;
        thisFriend.sequence = null;
        thisFriend.documentDay = null;
        thisFriend.documentMonth = null;
        thisFriend.documentYear = null;
        thisFriend.gender = null;
        thisFriend.title = null;
        thisFriend.documentCountry = null;
        thisFriend.nationality = null;
        thisFriend.documentNumber = null;
        thisFriend.paxType = null;
        thisFriend.init = function(json) {
            this.setSettingsByObject(json);
        };

        return thisFriend;
    };
    SKYSALES.Class.Friend.createObject = function(json) {
        SKYSALES.Util.createObject('Friend', 'Friend', json);
    };
}
/* End TravelerInput.js */


/* Start Addons.js */
/*!
This file is part of the Navitaire Professional Services customization.
Copyright (C) Navitaire.  All rights reserved.
*/
/*


JsLint Status:
- JsLint Edition 2008-07-04
        
+ Strict whitespace
+ Assume a browser
+ Disallow undefined variables
+ Disallow leading _ in identifiers
+ Disallow == and !=
+ Disallow ++ and --
+ Disallow bitwise operators
+ Disallow . in RegExp literals
+ Disallow global var
Indentation 4
*/

/*extern $ jQuery window SKYSALES*/


/*
Name: 
Class AddOns
Param:
None
Return: 
An instance of AddOns
Functionality:
This class represents the Link and the Div container of each component
in the traveler page
Notes:
Class Hierarchy:
SkySales -> AddOnsContainer
*/
if (!SKYSALES.Class.AddOnsContainer) {
    SKYSALES.Class.AddOnsContainer = function() {
        var parent = SKYSALES.Class.SkySales(),
        thisAddOnsContainer = SKYSALES.Util.extendObject(parent);

        thisAddOnsContainer.menuSectionId = "";
        thisAddOnsContainer.menuSection = null;
        thisAddOnsContainer.menuListId = "";
        thisAddOnsContainer.menuList = null;
        thisAddOnsContainer.AddOnsArray = null;
        thisAddOnsContainer.submitButtonId = "";
        thisAddOnsContainer.submitButton = null;
        thisAddOnsContainer.continueButtonId = "";
        thisAddOnsContainer.continueButton = null;
        thisAddOnsContainer.AddOnsArrayLinks = null;
        thisAddOnsContainer.init = function(json) {
            this.setSettingsByObject(json);
            this.setVars();
            this.addEvents();

            //initialize all traveler input
            for (i = 0; i < thisAddOnsContainer.AddOnsArray.length; i += 1) {
                thisAddOnsContainer.AddOnsArray[i].AddOnsArray = this.AddOnsArray;
                thisAddOnsContainer.AddOnsArray[i].hideContent(); //resets all

            }
            thisAddOnsContainer.AddOnsArray[0].content.show();
            thisAddOnsContainer.AddOnsArray[0].list.addClass("selected");
        };

        thisAddOnsContainer.setVars = function() {
            thisAddOnsContainer.menuSection = $('#' + this.menuSectionId);
            thisAddOnsContainer.menuList = $('#' + this.menuListId);
            thisAddOnsContainer.submitButton = $('#' + this.submitButtonId);
            thisAddOnsContainer.continueButton = $('#' + this.continueButtonId);
        };

        thisAddOnsContainer.addEvents = function() {
            thisAddOnsContainer.continueButton.click(this.continueHandler);
        };

        thisAddOnsContainer.setSettingsByObject = function(json) {
            parent.setSettingsByObject.call(this, json);

            var i = 0;
            var AddOnsArray = this.AddOnsArray || [];
            var AddOns = null;
            for (i = 0; i < AddOnsArray.length; i += 1) {
                AddOns = new SKYSALES.Class.AddOns();
                /*AddOnsArray[i].linkId = "link_" + i;
                AddOnsArray[i].contentId = "content_" + i;
                AddOnsArray[i].listId = "list_" + i;*/
                AddOnsArray[i].linkId = this.AddOnsArrayLinks[i].linkId;
                AddOnsArray[i].contentId = this.AddOnsArrayLinks[i].contentId;
                AddOnsArray[i].listId = this.AddOnsArrayLinks[i].listId;
                AddOns.init(AddOnsArray[i], i);
                AddOnsArray[i] = AddOns;
            }
            this.AddOnsArray = AddOnsArray;
        };

        thisAddOnsContainer.continueHandler = function() {
            var activeAddOnsIndex = 0;
            AddOnsArray = thisAddOnsContainer.AddOnsArray || [];
            for (i = 0; i < AddOnsArray.length; i += 1) {
                if (AddOnsArray[i].list.hasClass("selected")) {
                    activeAddOnsIndex = i;
                    break;
                }
            }

            if ((activeAddOnsIndex + 1) == AddOnsArray.length) {
                if (thisAddOnsContainer.validateAddOns()) {
                    SKYSALES.DisplayLoadingBar(); //added by Linson at 2012-01-05
                    document.getElementById(thisAddOnsContainer.submitButtonId).click();
                }
            }
            else {
                thisAddOnsContainer.AddOnsArray[activeAddOnsIndex].triggerValidate();
                if (!thisAddOnsContainer.AddOnsArray[activeAddOnsIndex].isValidated()) {
                    thisAddOnsContainer.AddOnsArray[activeAddOnsIndex].toggleMenuHandler();
                    return false;
                }
                thisAddOnsContainer.AddOnsArray[activeAddOnsIndex + 1].toggleMenuHandler();
            }

        };

        thisAddOnsContainer.isValidated = function() {
            var i = 0;
            var AddOnsArray = this.AddOnsArray || [];
            var AddOns = null;
            var validated = true;
            for (i = 0; i < AddOnsArray.length; i += 1) {
                if (!AddOnsArray[i].isValidated()) { validated = false; }
            }
            return validated;
        }

        thisAddOnsContainer.validateAddOns = function() {
            var i = 0;
            var AddOnsArray = this.AddOnsArray || [];
            var AddOns = null;
            for (i = 0; i < AddOnsArray.length; i += 1) {
                AddOnsArray[i].triggerValidate();
                if (!AddOnsArray[i].isValidated()) {
                    AddOnsArray[i].toggleMenuHandler();
                    return false;
                }
            }
            return true;
        }

        return thisAddOnsContainer;
    };

    SKYSALES.Class.AddOnsContainer.createObject = function(json) {
        SKYSALES.Util.createObject('AddOnsContainer', 'AddOnsContainer', json);
    };
}


/*
Name: 
Class AddOns
Param:
None
Return: 
An instance of AddOns
Functionality:
This class represents the Link and the Div container of each component
in the traveler page
Notes:
Class Hierarchy:
SkySales -> AddOns
*/
if (!SKYSALES.Class.AddOns) {
    SKYSALES.Class.AddOns = function() {
        var parent = SKYSALES.Class.SkySales(),
        thisAddOns = SKYSALES.Util.extendObject(parent);

        thisAddOns.linkLabel = "";
        thisAddOns.linkId = "";
        thisAddOns.link = null;
        thisAddOns.listId = "";
        thisAddOns.list = null;
        thisAddOns.contentId = "";
        thisAddOns.content = null;
        thisAddOns.AddOnsArray = null;  //so as to access the other AddOns objects
        thisAddOns.validateButtonId = "";
        thisAddOns.validateButton = null;
        thisAddOns.validateValueId = "";

        thisAddOns.init = function(json) {
            this.setSettingsByObject(json);
            this.setVars();
            this.addEvents();
            thisAddOns.link.html(thisAddOns.linkLabel);
        };

        thisAddOns.setVars = function() {
            parent.setVars.call(this);
            thisAddOns.link = $('#' + this.linkId);
            thisAddOns.list = $('#' + this.listId);
            thisAddOns.content = $('#' + this.contentId);
            thisAddOns.validateButtonId = this.validateButtonId;
            thisAddOns.validateValueId = this.validateValueId;
            if (this.validateButtonId != '') { thisAddOns.validateButton = $('#' + this.validateButtonId); }
        };

        thisAddOns.addEvents = function() {
            parent.addEvents.call(this);
            //post here association of action to showing contentDivId
            //thisAddOns.action.focus(thisAddOns.toggleDiv());
            //thisAddOns.link.focus(alert('test'));
            thisAddOns.link.click(this.toggleMenuHandler);

        };

        thisAddOns.toggleMenuHandler = function() {
            //hide other traveler input
            for (i = 0; i < thisAddOns.AddOnsArray.length; i += 1) {
                thisAddOns.AddOnsArray[i].hideContent();
            }
            //show self
            thisAddOns.list.addClass("selected");
            thisAddOns.content.show();
        }

        thisAddOns.triggerValidate = function() {
            if (this.validateButtonId != '' && $('#' + this.validateButtonId).length > 0) {
                document.getElementById(this.validateButtonId).click();
            }
        }

        thisAddOns.isValidated = function() {
            if (thisAddOns.validateValueId != '' && $('#' + thisAddOns.validateValueId).length > 0) {
                if ($('#' + thisAddOns.validateValueId).val() == 'false') { return false; } else { return true; };
            }
            return true;
        }

        thisAddOns.hideContent = function() {
            thisAddOns.list.removeClass("selected");
            thisAddOns.content.hide();
        }

        thisAddOns.setSettingsByObject = function(json) {
            parent.setSettingsByObject.call(this, json);

        };

        return thisAddOns;
    };

    SKYSALES.Class.AddOns.createObject = function(json) {
        SKYSALES.Util.createObject('AddOns', 'AddOns', json);
    };
}


/* End Addons.js */

/* Start Change Control */
//Sam modify not to prompt permission to send itinery 20120615
function confirmEmail(confirmMessage, sendItineraryLinkButton, displayConfirm) {
    var sendItinerary;

//    if (displayConfirm == true) {
//        sendItinerary = confirm(confirmMessage)
//    }
//    else {
        sendItinerary = true;
//    }

    if (sendItinerary == true) {
        // showMsg is a global variable that determines whether or not to display the navigate away alert.
        showMsg = false;
        SKYSALES.DisplayLoadingBar(); //added by Linson at 2012-03-26
        __doPostBack(sendItineraryLinkButton, '');
        return true;
    }
}

/* End Change Control */

/* Start SSR Availability Input */

/*
Name: 
Class MealLegInput
Param:
None
Return: 
An instance of MealLegInput
Functionality:
This class represents the MealLegInput control in the traveler page.
Notes:
Class Hierarchy:
SkySales -> MealLegInput
*/
SKYSALES.Class.MealLegInput = function() {
    var parent = new SKYSALES.Class.SkySales(),
    thisMealLegInput = SKYSALES.Util.extendObject(parent);

    thisMealLegInput.clientId = '';
    thisMealLegInput.ssrAvailabilityArray = null;
    thisMealLegInput.checkboxMealArray = [];
    thisMealLegInput.currencyCode = '';
    thisMealLegInput.complimentaryTextArray = null;
    thisMealLegInput.noMealText = '';
    thisMealLegInput.nextButtonId = '';
    thisMealLegInput.continueButtonId = '';
    thisMealLegInput.isFlightChange = false;
    thisMealLegInput.disableMarket = null;
    thisMealLegInput.nextButton = null;
    thisMealLegInput.continueButton = null;
    thisMealLegInput.maxMeals = '';
    thisMealLegInput.maxComplimentaryMeals = '';
    thisMealLegInput.isValidMeal = true;
    thisMealLegInput.maxComplimentaryText = '';
    thisMealLegInput.maxMealText = '';
    thisMealLegInput.mealGroupMaxTextArray = '';
    thisMealLegInput.priorityMealsText = '';
    thisMealLegInput.mealGroupArray = [];
    thisMealLegInput.showMealImage = '';
    thisMealLegInput.enablePriorityChecking = '';
    thisMealLegInput.buttonText = '';
    thisMealLegInput.passengerNumber = '';
    thisMealLegInput.mealSegmentsJson = [];
    thisMealLegInput.noComplimentaryMealText = '';
    thisMealLegInput.validComplimentaryMealClasses = [];
    thisMealLegInput.init = function(json) {
        this.setSettingsByObject(json);

        //This is to remove the hidden button submit when auto assign seats is off
        if ($("#hiddenButtonSubmit").length > 0) {
            $("#hiddenButtonSubmit").remove();
        }

        this.setVars();
        this.addEvents();
        this.initSsrAvailabilityArray();
        this.initCheckBoxMealLegInputArray();
    };

    thisMealLegInput.setVars = function() {
    };

    thisMealLegInput.addEvents = function() {
        var i = 0,
            continueButtons = $('input[@id="' + this.continueButtonId + '"]');

        if (this.passengerNumber == '0') {
            if (continueButtons.length > 1) {
                for (i = 0; i < continueButtons.length; i++) {
                    if ($(continueButtons[i]).parents('p.hidden').length == 0) {
                        break;
                    }
                }
            }
            $(continueButtons[i]).click(this.mealsEventHandler);
        }
    };

    thisMealLegInput.mealsEventHandler = function() {
        return thisMealLegInput.mealsEvent();
    };

    thisMealLegInput.mealsEvent = function() {
        return this.complimentaryMealCheck();
    };

    //This function checks if complimentary meal is offered but customer did not choose a meal
    thisMealLegInput.complimentaryMealCheck = function() {
        var i = 0,
        j = 0,
        mealSegmentsList = this.mealSegmentsJson,
        passengerNumber = 0,
        segmentIndex = 0,
        productClass = '',
        mealContainerId = '',
        del = '_',
        selectedMeals = '';

        for (i = 0; i < mealSegmentsList.length; i++) {
            passengerNumber = mealSegmentsList[i].passengerNumber;
            segmentIndex = mealSegmentsList[i].segmentIndex;
            productClass = mealSegmentsList[i].productClass;
            if (jQuery.inArray(productClass, this.validComplimentaryMealClasses) != -1) {
                mealContainerId = "mealContainer" + del + passengerNumber + del + segmentIndex;
                if ($('[id = ' + mealContainerId + ']').length > 0 && $('[id = ' + mealContainerId + '] :input').length > 0) {
                    selectedMeals = $('[id = ' + mealContainerId + '] :input[id *= "meal"]:checked').size();
                    if (selectedMeals == 0) {
                        alert(this.noComplimentaryMealText);
                        return false;
                    }
                }
            }
        }
        return true;
    };

    thisMealLegInput.initSsrAvailabilityArray = function() {
        var i = 0,
        ssrAvailabilityArrayObject = this.ssrAvailabilityArray || [],
        len = ssrAvailabilityArrayObject.length,
        ssrAvailability = null;
        for (i = 0; i < len; i += 1) {
            ssrAvailability = new SKYSALES.Class.SSRAvailability();
            ssrAvailability.init(ssrAvailabilityArrayObject[i]);
            this.ssrAvailabilityArray[i] = ssrAvailability;
        }
    };

    thisMealLegInput.initCheckBoxMealLegInputArray = function() {
        var i = 0,
        checkBoxMealArrayObject = this.checkboxMealArray || [],
        len = checkBoxMealArrayObject.length,
        checkBoxMeal = null;
        for (i = 0; i < len; i += 1) {
            checkBoxMeal = new SKYSALES.Class.CheckBoxMealLegInput();
            checkBoxMeal.complimentaryTextArray = this.complimentaryTextArray;
            checkBoxMeal.noMealText = this.noMealText;
            checkBoxMeal.isFlightChange = this.isFlightChange;
            checkBoxMeal.maxMeals = this.maxMeals;
            checkBoxMeal.maxComplimentaryMeals = this.maxComplimentaryMeals;
            checkBoxMeal.maxComplimentaryText = this.maxComplimentaryText;
            checkBoxMeal.maxMealText = this.maxMealText;
            checkBoxMeal.maxGroupMealsText = this.mealGroupMaxTextArray;
            checkBoxMeal.priorityMealsText = this.priorityMealsText;
            checkBoxMeal.groupMealArray = this.mealGroupArray;
            checkBoxMeal.showImage = this.showMealImage;
            checkBoxMeal.enablePriorityChecking = this.enablePriorityChecking;
            checkBoxMeal.segmentIndex = i + 1;
            //disable market variable's value is the index of the first segment of the active flight
            if (this.disableMarket != "none" && i < parseInt(this.disableMarket)) {
                checkBoxMeal.isDisabled = true;
            }
            checkBoxMeal.init(checkBoxMealArrayObject[i], this.ssrAvailabilityArray[0], this.currencyCode);
            this.checkboxMealArray[i] = checkBoxMeal;
        }
    };

    return thisMealLegInput;
}

/*
Creates the MealLegInput class.
*/
SKYSALES.Class.MealLegInput.createObject = function(json) {
    SKYSALES.Util.createObject('mealLegInput', 'MealLegInput', json);
};

/*
Name: 
Class DropDownMealLegInput
Param:
None
Return: 
An instance of DropDownMealLegInput
Functionality:
This class represents the dropdown meal input web control in the traveler page.
Notes:
Class Hierarchy:
SkySales -> DropDownMealLegInput
*/
SKYSALES.Class.CheckBoxMealLegInput = function() {
    var parent = new SKYSALES.Class.SkySales(),
    thisCheckBoxMealLegInput = SKYSALES.Util.extendObject(parent);

    thisCheckBoxMealLegInput.checkBoxMealIdArray = {};
    thisCheckBoxMealLegInput.checkBoxMeal = {};
    thisCheckBoxMealLegInput.quantityPrefix = '';
    thisCheckBoxMealLegInput.del = '';
    thisCheckBoxMealLegInput.passengerNumber = 0;
    thisCheckBoxMealLegInput.flightReference = '';
    thisCheckBoxMealLegInput.selectedItem = '';
    thisCheckBoxMealLegInput.currencyCode = '';
    thisCheckBoxMealLegInput.hiFlyerMealText = null;
    thisCheckBoxMealLegInput.ssrAvailability = null;
    thisCheckBoxMealLegInput.isNewBooking = true;
    thisCheckBoxMealLegInput.complimentaryTextArray = null;
    thisCheckBoxMealLegInput.noMealText = '';
    thisCheckBoxMealLegInput.noMealTextHtml = null;
    thisCheckBoxMealLegInput.isFlightChange = false;
    thisCheckBoxMealLegInput.isDisabled = false;
    thisCheckBoxMealLegInput.origDropDownId = '';
    thisCheckBoxMealLegInput.mealPriceArray = null;
    thisCheckBoxMealLegInput.maxMeals = '';
    thisCheckBoxMealLegInput.maxComplimentaryMeals = '';
    thisCheckBoxMealLegInput.maxComplimentaryText = '';
    thisCheckBoxMealLegInput.maxMealText = '';
    thisCheckBoxMealLegInput.maxGroupMealsText = '';
    thisCheckBoxMealLegInput.priorityMealsText = '';
    thisCheckBoxMealLegInput.groupMealArray = [];
    thisCheckBoxMealLegInput.showImage = '';
    thisCheckBoxMealLegInput.segmentIndex = '';
    thisCheckBoxMealLegInput.enablePriorityChecking = '';

    //var isSelectedMealAvailable = false;

    thisCheckBoxMealLegInput.init = function(json, ssrAvailability, currencyCode) {
        this.setSettingsByObject(json);
        this.addEvents();

    };

    thisCheckBoxMealLegInput.addEvents = function() {
        //Add event on every checkbox
        var i = 0,
        j = 0,
        labelMealId;
        for (i = 0; i < this.checkBoxMealIdArray.length; i = i + 1) {
            for (j = 0; j < this.checkBoxMealIdArray[i].length; j = j + 1) {
                $('#' + this.checkBoxMealIdArray[i][j].checkboxMealId + '_dropDown').change(this.mealsInputHandler);
                //No events added for showing meal images if disabled
                if (this.showImage === "true") {
                    labelMealId = this.checkBoxMealIdArray[i][j].checkboxMealId.replace("complimentaryMeal", "meal");
                    $('#' + labelMealId + '_label').mouseover(this.showMealImage);
                    $('#' + labelMealId + '_label').mouseout(this.hideMealImage);
                }
            }
        }
    };

    thisCheckBoxMealLegInput.showMealImage = function() {
        var dhtml = new SKYSALES.Dhtml(),
        mealImage,
        x = dhtml.getX(this),
        y = dhtml.getY(this);

        mealImage = $(this.nextSibling);
        $(this).css('font-weight', 'bold');
        $(mealImage).css('left', x + $(this).width() + 10 - 137); //Added by Sean on 27 Aug 2012 - 107
        $(mealImage).css('top', y - ($(mealImage).height() / 2) + 8 - 251); //Added by Sean on 27 Aug 2012 - 231
        $(mealImage).css('position', 'absolute');
        $(mealImage).show();
    };

    thisCheckBoxMealLegInput.hideMealImage = function() {
        var mealImage = $(this.nextSibling);
        $(this).css('font-weight', 'normal');
        $(mealImage).hide();
    };

    thisCheckBoxMealLegInput.getGroupValue = function(mealGroupName, mealType, value) {
        var i = 0;
        for (i = 0; i < this.groupMealArray.length; i = i + 1) {
            if (this.groupMealArray[i].groupName === mealGroupName) {
                if (mealType === "meal") {
                    return value === "maxNumber" ? this.groupMealArray[i].maxNumber : this.groupMealArray[i].priority;
                }
                else if (mealType === "complimentaryMeal") {
                    return this.groupMealArray[i].complimentaryCount;
                }
            }
        }
        return 0;
    };

    thisCheckBoxMealLegInput.mealsInputHandler = function() {
        var selectParamObj = null,
            del = '_',
            mealArray = this.id.split(del) || '',
            passengerNumberIndex = jQuery.inArray("passengerNumber", mealArray) + 1,
            segmentIndex = jQuery.inArray("meal", mealArray) + 1 || jQuery.inArray("complimentaryMeal", mealArray) + 1,
            groupNameIndex = jQuery.inArray("group", mealArray) + 1,
            mealsContainerId = "mealContainer" + del + mealArray[passengerNumberIndex] + del + mealArray[segmentIndex],
            mealsGroupContainerId = "mealGroupContainer" + del + mealArray[groupNameIndex] + del + mealArray[passengerNumberIndex] + del + mealArray[segmentIndex],
            mealsGroupMax = thisCheckBoxMealLegInput.getGroupValue(mealArray[groupNameIndex], "meal", "maxNumber"),
            mealsGroupPriority = thisCheckBoxMealLegInput.getGroupValue(mealArray[groupNameIndex], "meal", "priority"),
            mealsContainer = $('[id ^= "' + mealsContainerId + '"]'),
            mealsGroupContainer = $('[id ^= "' + mealsGroupContainerId + '"]'),
            selectedRegularMeals = $('[id = ' + mealsContainer[0].id + '] :input[id *= "meal"]:checked').size(),
            selectedMealsInGroup = $('[id = ' + mealsGroupContainerId + '] :input:checked').size(),
            selectedCheckboxDom = $('#' + this.id) || {},
            checkBoxId = this.id.replace('_dropDown', '');

        if (this.id.indexOf('_dropDown') != -1) {
            thisCheckBoxMealLegInput.dropDownMealsInputHandler(this, mealsGroupPriority, mealsContainerId, mealsGroupContainerId, mealsGroupMax, mealArray[groupNameIndex], mealArray[passengerNumberIndex], mealArray[segmentIndex], checkBoxId);
        }
    };

    thisCheckBoxMealLegInput.dropDownMealsInputHandler = function(obj, mealsGroupPriority, mealsContainerId, mealsGroupContainerId, mealsGroupMax, groupName, passengerNumber, segmentIndex, checkBoxId) {

        if ($(obj).val() !== '' && $(obj).val() > 0) {
            $('#' + checkBoxId).attr('checked', 'true');
        }
        else {
            $('#' + checkBoxId).removeAttr('checked');
        }

        if (this.isMealGroupPassedPriorityCheck(mealsGroupPriority, passengerNumber, segmentIndex, obj)) {

            var allCheckboxInGroup = $('[id = ' + mealsGroupContainerId + '] :input:checked'),
            allCheckboxInSegment = $('[id = ' + mealsContainerId + '] :input:checked'),
            i = 0,
            groupMealsCount = parseInt(i),
            segmentMealsCount = parseInt(i);

            for (i = 0; i < allCheckboxInGroup.length; i = i + 1) {
                groupMealsCount = groupMealsCount + parseInt($('#' + allCheckboxInGroup[i].id + '_dropDown').val());
            }

            if (groupMealsCount > mealsGroupMax) {
                alert(this.displayMealMaximumText(groupName, "", mealsGroupMax));
                this.resetMealsDropDown(obj.id, checkBoxId);
            }

            for (i = 0; i < allCheckboxInSegment.length; i = i + 1) {
                segmentMealsCount = segmentMealsCount + parseInt($('#' + allCheckboxInSegment[i].id + '_dropDown').val());
            }

            if (segmentMealsCount > this.maxMeals) {
                alert(this.maxMealText);
                this.resetMealsDropDown(obj.id, checkBoxId);
            }
        }
        else {
            alert(this.priorityMealsText);
            $(obj).val('');
            $('#' + checkBoxId).removeAttr('checked');
        }
    };

    thisCheckBoxMealLegInput.isMealGroupPassedPriorityCheck = function(mealGroupPriority, passengerNumber, segmentIndex, obj) {
        //A function to sort meal categories on the array
        function sortMealgroup(meal1, meal2) {
            return meal1.priority - meal2.priority;
        }

        if (this.enablePriorityChecking === "false" || (mealGroupPriority == 1 && $(obj).val() !== '')) {
            return true;
        }

        var groupMealArray = this.groupMealArray.sort(sortMealgroup),
            mealGroupIndex = this.getMealGroupIndex(groupMealArray, mealGroupPriority),
            groupPriorityContainerId = "priority" + this.del + groupMealArray[mealGroupIndex].priority + this.del + passengerNumber + this.del + segmentIndex,
            groupPriorityContainer = $($('[id = ' + groupPriorityContainerId + '] :input[id *= "meal"]:checked')),
            i = 0;

        //If dropdown was set to 0, check other categories if needed to untick checkboxes that belong to that category
        if ($(obj).val() === '') {
            //Check if no other checkox are ticked inside the category
            if (groupPriorityContainer.length - 1 < 0) {
                for (i = mealGroupIndex + 1; i < groupMealArray.length; i++) {
                    groupPriorityContainerId = "priority" + this.del + groupMealArray[i].priority + this.del + passengerNumber + this.del + segmentIndex;
                    $('[id = ' + groupPriorityContainerId + '] :input[id *= "dropDown"]').val("0");
                    $('[id = ' + groupPriorityContainerId + '] :input[type = "checkbox"]:checked').removeAttr('checked');
                }
            }
            return true;
        }
        else {
            do {
                //get higher priority of the selected category, there can be a scenario when some meal categories have the same priority
                mealGroupIndex--;
            } while (groupMealArray[mealGroupIndex].priority === mealGroupPriority);

            //Get number of ticked checboxes of the higher priority category
            groupPriorityContainerId = "priority" + this.del + groupMealArray[mealGroupIndex].priority + this.del + passengerNumber + this.del + segmentIndex;
            groupPriorityContainer = $($('[id = ' + groupPriorityContainerId + '] :input[id *= "meal"]:checked'));

            return groupPriorityContainer.length > 0 ? true : false;
        }

        return true;
    };

    thisCheckBoxMealLegInput.resetMealsDropDown = function(id, checkBoxId) {
        $('[id = "' + checkBoxId + '"]').removeAttr('checked');
        $('[id = "' + id + '"]').val("");
    };

    thisCheckBoxMealLegInput.getMealGroupIndex = function(groupMealArray, mealGroupPriority) {
        for (var i = 0; i < groupMealArray.length; i++) {
            if (groupMealArray[i].priority === mealGroupPriority) return i;
        }
    }

    thisCheckBoxMealLegInput.displayMealMaximumText = function(mealGroupName, mealType, maxCount) {

        var i = 0;
        if (mealType === "complimentary") {
            return this.maxGroupMealsText[this.maxGroupMealsText.length - 1].messagePre;
        }

        //Start with the second message
        for (i = 1; i < this.maxGroupMealsText.length - 1; i = i + 1) {
            if (this.maxGroupMealsText[i].messageId.toLowerCase().indexOf(mealGroupName.toLowerCase()) > 0) {
                return this.maxGroupMealsText[i].messagePre + maxCount + this.maxGroupMealsText[i].messagePost;
            }
        }
        //first message in the array is always the default message
        return this.maxGroupMealsText[0].messagePre;
    }

    return thisCheckBoxMealLegInput;
}

/*
Name: 
Class BaggageLegInput
Param:
None
Return: 
An instance of BaggageLegInput
Functionality:
This class represents the BaggageLegInput control in the traveler page.
Notes:
Class Hierarchy:
SkySales -> BaggageLegInput
*/
SKYSALES.Class.BaggageLegInput = function() {
    var parent = new SKYSALES.Class.SkySales(),
    thisBaggageLegInput = SKYSALES.Util.extendObject(parent);

    thisBaggageLegInput.clientId = '';
    thisBaggageLegInput.ssrAvailabilityArray = null;
    thisBaggageLegInput.dropDownBaggageArray = [];
    thisBaggageLegInput.currencyCode = '';
    thisBaggageLegInput.complimentaryTextArray = null;
    thisBaggageLegInput.isFlightChange = false;
    thisBaggageLegInput.disableMarket = null;
    thisBaggageLegInput.isNewBookingModified = null;
    thisBaggageLegInput.isBookingModified = null;
    thisBaggageLegInput.isAgent = null;

    thisBaggageLegInput.init = function(json) {
        this.setSettingsByObject(json);
        this.setVars();
        this.addEvents();
        this.initSsrAvailabilityArray();
        this.initDropDownBaggageLegInputArray();
    };

    thisBaggageLegInput.initSsrAvailabilityArray = function() {
        var i = 0,
        ssrAvailabilityArrayObject = this.ssrAvailabilityArray || [],
        len = ssrAvailabilityArrayObject.length,
        ssrAvailability = null;
        for (i = 0; i < len; i += 1) {
            ssrAvailability = new SKYSALES.Class.SSRAvailability();
            ssrAvailability.init(ssrAvailabilityArrayObject[i]);
            this.ssrAvailabilityArray[i] = ssrAvailability;
        }
    };

    thisBaggageLegInput.initDropDownBaggageLegInputArray = function() {
        var i = 0,
        dropDownBaggageArrayObject = this.dropDownBaggageArray || [],
        len = dropDownBaggageArrayObject.length,
        dropDownBaggage = null;
        for (i = 0; i < len; i += 1) {
            dropDownBaggage = new SKYSALES.Class.DropDownBaggageLegInput();
            dropDownBaggage.complimentaryTextArray = this.complimentaryTextArray;
            if (this.disableMarket != "none" && i < parseInt(this.disableMarket)) {
                    dropDownBaggage.isDisabled = true;
            }
            dropDownBaggage.isFlightChange = this.isFlightChange;
            dropDownBaggage.isNewBookingModified = this.isNewBookingModified;
            dropDownBaggage.isBookingModified = this.isBookingModified;
            dropDownBaggage.isAgent = this.isAgent;
            dropDownBaggage.init(dropDownBaggageArrayObject[i], this.ssrAvailabilityArray[0], this.currencyCode);
            this.dropDownBaggageArray[i] = dropDownBaggage;
        }
    };

    return thisBaggageLegInput;
}

/*
Creates the BaggageLegInput class.
*/
SKYSALES.Class.BaggageLegInput.createObject = function(json) {
    SKYSALES.Util.createObject('baggageLegInput', 'BaggageLegInput', json);
};

/*
Name: 
Class DropDownBaggageLegInput
Param:
None
Return: 
An instance of DropDownBaggageLegInput
Functionality:
This class represents the dropdown baggage input web control in the traveler page.
Notes:
Class Hierarchy:
SkySales -> DropDownBaggageLegInput
*/
SKYSALES.Class.DropDownBaggageLegInput = function() {
    var parent = new SKYSALES.Class.SkySales(),
    thisDropDownBaggageLegInput = SKYSALES.Util.extendObject(parent);

    thisDropDownBaggageLegInput.dropDownBaggageId = '';
    thisDropDownBaggageLegInput.dropDownBaggage = {};
    thisDropDownBaggageLegInput.quantityPrefix = '';
    thisDropDownBaggageLegInput.del = '';
    thisDropDownBaggageLegInput.passengerNumber = 0;
    thisDropDownBaggageLegInput.flightReference = '';
    thisDropDownBaggageLegInput.selectedItem = '';
    thisDropDownBaggageLegInput.currencyCode = '';
    thisDropDownBaggageLegInput.hiFlyerBaggageText = '';

    thisDropDownBaggageLegInput.isNewBooking = true;
    thisDropDownBaggageLegInput.complimentaryTextArray = null;
    thisDropDownBaggageLegInput.prevSelected = null;
    thisDropDownBaggageLegInput.isFlightChange = false;
    thisDropDownBaggageLegInput.isDisabled = false;
    thisDropDownBaggageLegInput.origDropDownId = "";
    thisDropDownBaggageLegInput.baggagePriceArray = null;
    thisDropDownBaggageLegInput.baggageAlertCodeList = '';
    thisDropDownBaggageLegInput.baggageAlertMessage = '';

    //added by Michelle to auto select baggage for connecting flights 09Nov2010 (start)
    thisDropDownBaggageLegInput.autoSelectBaggage = function() {
        if (this.selectedItem != '') {
            var ID = thisDropDownBaggageLegInput.dropDownBaggageId;
            var selectedText = this.options[this.selectedIndex].text;
            var selectedValue = this.options[this.selectedIndex].value;
            var flightRef = thisDropDownBaggageLegInput.flightReference;
            var elems = document.getElementsByTagName("select");
            var matches = [];
            if ((selectedValue.length > 0 || selectedText.length > 0) && flightRef.length > 0) {
                var ssrCode = selectedValue.substring(selectedValue.indexOf("ssrCode"), selectedValue.indexOf("flightReference") - 1);
                var passengerNum = ID.substring(ID.indexOf("passengerNumber"), ID.indexOf("flightReference") - 1);
                for (var i = 0, m = elems.length; i < m; i++) {
                    if (!elems[i].disabled) {
                        if (elems[i].id && elems[i].id != ID && elems[i].id.indexOf(passengerNum) != -1 && (elems[i].id.indexOf("BaggageLegInputViewTravelerView") != -1 || elems[i].id.indexOf("BaggageLegInputViewTravelerChangeView") != -1 || elems[i].id.indexOf("BaggageLegInputViewTravelerFlightChangeView") != -1 || elems[i].id.indexOf("BaggageLegInputViewSSRChangeView") != -1)) {
                            matches.push(elems[i]);
                        }
                    }
                }

                if (matches) {
                    for (var x = 0, y = matches.length; x < y; x++) {
                        var nextID = matches[x].id;
                        var journey1 = flightRef.substring(flightRef.lastIndexOf("-") + 1, flightRef.length);
                        var journey2 = nextID.substring(nextID.lastIndexOf("-") + 1, nextID.length);
                        var journey1from = journey1.substring(0, 2);
                        var journey1to = journey1.substring(journey1.length - 1, journey1.length - 3);
                        var journey2from = journey2.substring(0, 2);
                        var journey2to = journey2.substring(journey2.length - 1, journey2.length - 3);
                        if (!(journey1from == journey2to && journey2from == journey1to) && (journey1to == journey2from || journey1from == journey2to)) {
                            var select = document.getElementById(nextID);
                            if (ssrCode.length > 0) {
                                for (var i = 0, l = select.options.length; i < l; i++) {
                                    o = select.options[i];
                                    if (o.value.indexOf(ssrCode) != -1) {
                                        o.selected = true;
                                    }
                                }
                            }
                            else if (selectedText.length > 0) {
                                for (var i = 0, l = select.options.length; i < l; i++) {
                                    o = select.options[i];
                                    if (o.text.indexOf(selectedText) != -1) {
                                        o.selected = true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            // add by super 2012-02-29 when baggage >32kg will prompt a notice (start)
            var alertcodestr = thisDropDownBaggageLegInput.baggageAlertCodeList
            var alertcodelist = alertcodestr.split(',');
            for (var i = 0; i < alertcodelist.length; i++) {
                var tempalertcode = alertcodelist[i];
                if (selectedValue.indexOf(tempalertcode) != -1) {
                    alert(thisDropDownBaggageLegInput.baggageAlertMessage);
                    break;
                }
            }
            //add by super 2012-02-29 when baggage >32kg will prompt a notice (end)
        }
    }

    thisDropDownBaggageLegInput.addEvents = function() {
        thisDropDownBaggageLegInput.dropDownBaggage.change(this.autoSelectBaggage);
    }
    //added by Michelle to auto select baggage for connecting flights 09Nov2010 (end)

    thisDropDownBaggageLegInput.init = function(json, ssrAvailability, currencyCode) {
        this.setSettingsByObject(json);
        this.setVars();
        /*        since the baggage's price is based on weight, when there's a complimentary baggage, 
        it applies to all, so there's no need to remove the label if the value changes in the dropdown*/

        this.addEvents();

        this.currencyCode = currencyCode;

        this.initDropDownBaggageLegInput(ssrAvailability);

        if (this.dropDownBaggage) {
            this.dropDownBaggage.val(this.selectedItem);
        }

        if (this.isDisabled) {
            this.dropDownBaggage.attr('disabled', true);
            $('#' + this.origDropDownId).val(this.selectedItem);
        }
    };

    thisDropDownBaggageLegInput.setVars = function() {
        this.hiFlyerBaggageText = $('#' + this.dropDownBaggageId + "_hiflyerBaggageText");
        if (this.isDisabled) {
            this.origDropDownId = this.dropDownBaggageId;
            this.dropDownBaggageId = this.dropDownBaggageId + "_";
        }
        this.dropDownBaggage = $('#' + this.dropDownBaggageId);

    };

    thisDropDownBaggageLegInput.showLabel = function(ssrCode, value) {
        var i = 0;
        for (i = 0; i < this.complimentaryTextArray.length; i++) {
            // if the ssr is more expensive, that's the only time to change the label
            if (this.complimentaryTextArray[i].SsrCode == ssrCode && (this.prevSelected == null || i > this.prevSelected)) {
                this.prevSelected = i;
                this.hiFlyerBaggageText.text(this.complimentaryTextArray[i].labelText);

                //only set a new selected item if it's new booking or if it's on a flight change. if not, retain the original value.
                // same as the meals, i included the flight change condition here so that the new complimentary ssrs that come with the new fare/s would be selected

                if ((this.isNewBooking || this.isFlightChange) && !this.isDisabled)
                    this.selectedItem = value;
                break;
            }
        }

    };

    thisDropDownBaggageLegInput.findMCCPrice = function(ssrCode) {
        var priceCtr = 0;
        if (this.baggagePriceArray != null) {
            for (priceCtr = 0; priceCtr < this.baggagePriceArray.length; priceCtr++) {
                if (this.baggagePriceArray[priceCtr].SsrCode == ssrCode)
                    return this.baggagePriceArray[priceCtr].SsrPrice;
            }
        }
        return '';
    };

    /*
    Creates the option items of the dropdown with their corresponding values.
    */

    thisDropDownBaggageLegInput.initDropDownBaggageLegInput = function(ssrAvailability) {
        len1 = ssrAvailability.SsrAvailabilityList.length;
        SSRAvailability = null;
        flightCarrierCode = null;
        var i = 0,
        selectedItemArray = this.selectedItem ? this.selectedItem.split('_') : null,
        ssrCode = selectedItemArray ? selectedItemArray[jQuery.inArray("ssrCode", selectedItemArray) + 1] : "",
        ssrNum = selectedItemArray ? selectedItemArray[jQuery.inArray("ssrNum", selectedItemArray) + 1] : "1",
		selectBoxObj = '';

        this.selectedItem = ssrCode !== "" ? this.selectedItem : "";

        if (this.selectedItem !== '' && !this.isFlightChange) {
            this.isNewBooking = false;
        }

        for (i = 0; i < len1; i += 1) {

            SSRAvailability = ssrAvailability.SsrAvailabilityList[i];

            if (SSRAvailability.passengerNumber == this.passengerNumber) {
                len2 = SSRAvailability.flightParts.length;
                flightPart = null;

                for (j = 0; j < len2; j += 1) {
                    flightPart = new SKYSALES.Class.FlightPart();
                    flightPart.init(SSRAvailability.flightParts[j]);

                    if (flightPart.flightKey != null) {
                        flightDesignator = '';

                        if (flightPart.flightKey.flightDesignator != null) {
                            flightCarrierCode = flightPart.flightKey.flightDesignator.CarrierCode;
                            flightDesignator = flightPart.flightKey.flightDesignator.CarrierCode + flightPart.flightKey.flightDesignator.FlightNumber;
                            flightDesignator = flightDesignator.replace(' ', '-');
                            flightDesignator = flightDesignator.replace(' ', '-');
                        }


                        flightRef = flightPart.flightKey.departureDate.getDate() +
                                    '-' +
                                    flightDesignator +
                                    '-' +
                                    flightPart.flightKey.departureStation +
                                    flightPart.flightKey.arrivalStation;
                        if (this.flightReference == flightRef) {
                            len3 = flightPart.availableSsrs.length;
                            origSelectedItem = this.selectedItem;
                            hasComplimentaryBaggage = false;

                            for (k = 0; k < len3; k += 1) {
                                if (this.dropDownBaggage) {
                                    text = flightPart.availableSsrs[k].ssr.name;
                                    price = thisDropDownBaggageLegInput.findMCCPrice(flightPart.availableSsrs[k].ssr.ssrCode);
                                    text = text + ' (' + price + ')';
                                    value = this.quantityPrefix +
                                            this.del +
                                            "passengerNumber" +
                                            this.del +
                                            this.passengerNumber +
                                            this.del +
                                            "ssrCode" +
                                            this.del +
                                            flightPart.availableSsrs[k].ssr.ssrCode +
                                            this.del +
                                            "ssrNum" +
                                            this.del +
                                            ssrNum +
                                            this.del +
                                            "flightReference" +
                                            this.del +
                                            flightRef;

                                    if (flightPart.availableSsrs[k].price == 0) {
                                        this.showLabel(flightPart.availableSsrs[k].ssr.ssrCode, value);
                                        hasComplimentaryBaggage = true;
                                    }
                                    else if ((hasComplimentaryBaggage) && (value == origSelectedItem) && (flightPart.availableSsrs[k].price > 0)) {
                                        this.selectedItem = value;
                                    }
                                    else if ((this.isNewBooking == true) && (value == origSelectedItem)) {
                                        this.selectedItem = value;
                                    }

                                    else if ((this.isNewBooking == true) && (this.isNewBookingModified == "") && (this.isBookingModified == "") && (this.isAgent == "false")) {
                                    if ((flightCarrierCode == "AK") || (flightCarrierCode == "FD") || (flightCarrierCode == "QZ") || (flightCarrierCode == "PQ") || (flightCarrierCode == "JW")) {
                                            if (flightPart.availableSsrs[k].ssr.ssrCode == "PBAB") {
                                                this.selectedItem = value;
                                            }
                                        }
                                        else if (flightCarrierCode == "D7") {
                                            if (flightPart.availableSsrs[k].ssr.ssrCode == "PBAB") {
                                                this.selectedItem = value;
                                            }
                                        }
                                    }

                                    selectBoxObj = this.dropDownBaggage.get(0);
                                    if (selectBoxObj) {
                                        selectBoxObj.options[selectBoxObj.options.length] = new window.Option(text, value, false, false);
                                    }

                                }
                            }
                            //Added by Michelle 24DEC2010 - Remove 'No checked bag' when Manage My Booking with baggage purchased (start)
                            if (selectBoxObj) {
                                if (thisDropDownBaggageLegInput.isBookingModified == "True" && this.selectedItem != '') {
                                    if (this.selectedItem.indexOf("__") == -1) {
                                        selectBoxObj = this.dropDownBaggage.get(0);
                                        if (selectBoxObj) {
                                            for (var m = 0, l = 2; m < l; m++) {
                                                o = selectBoxObj.options[m];
                                                if (m == 0 && o.value == "") {
                                                    selectBoxObj.options[0] = null;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            //Added by Michelle 24DEC2010 - Remove 'No checked bag' when Manage My Booking with baggage purchased (end)     
                        }
                    }
                }
            }
        }

    };



    return thisDropDownBaggageLegInput;
}
/* add by super chen 2011-11-08
Name: 
Class sports equipment
Param:
None
Return: 
An instance of sports equipment
Functionality:
This class represents the sports equipment control in the traveler page.
Notes:
Class Hierarchy:
SkySales -> sports equipment
*/
SKYSALES.Class.SportsEquipment = function() {
    var parent = new SKYSALES.Class.SkySales(),
    thisSportsEquipment = SKYSALES.Util.extendObject(parent);


    thisSportsEquipment.clientId = '';
    thisSportsEquipment.ssrAvailabilityArray = null;
    thisSportsEquipment.dropdownsportEquipmentarray = [];
    thisSportsEquipment.currencyCode = '';
    thisSportsEquipment.disableMarket = null;
    thisSportsEquipment.isBookingModified = '';
    thisSportsEquipment.init = function(json) {
        this.setSettingsByObject(json);
        this.setVars();
        this.addEvents();
        this.initSsrAvailabilityArray();
        this.initdropdownSportEquipmentarray();
    };
    thisSportsEquipment.initSsrAvailabilityArray = function() {
        var i = 0,
        ssrAvailabilityArrayObject = this.ssrAvailabilityArray || [],
        len = ssrAvailabilityArrayObject.length,
        ssrAvailability = null;
        for (i = 0; i < len; i += 1) {
            ssrAvailability = new SKYSALES.Class.SSRAvailability();
            ssrAvailability.init(ssrAvailabilityArrayObject[i]);
            this.ssrAvailabilityArray[i] = ssrAvailability;
        }
    };
    thisSportsEquipment.initdropdownSportEquipmentarray = function() {
        var dropdownSportEquipmentObject = this.dropdownsportEquipmentarray || [],
        len = dropdownSportEquipmentObject.length;
        var dropdownSport = null;
        for (var i = 0; i < len; i++) {
            dropdownSport = new SKYSALES.Class.dropdownSportEquipment();
            if (this.disableMarket != "none" && i < parseInt(this.disableMarket)) {
                dropdownSport.isDisabled = true;
            }
            dropdownSport.isBookingModified = this.isBookingModified;
            dropdownSport.init(dropdownSportEquipmentObject[i], this.ssrAvailabilityArray[0], this.currencyCode);
            this.dropdownsportEquipmentarray[i] = dropdownSport;
        }
    };

    return thisSportsEquipment;
}
/*
Creates the SportsEquipment class.
*/
SKYSALES.Class.SportsEquipment.createObject = function(json) {
    SKYSALES.Util.createObject('sportsEquipment', 'SportsEquipment', json);
};

SKYSALES.Class.dropdownSportEquipment = function() {
    var parent = new SKYSALES.Class.SkySales(),
    thisdropdownSportEquipment = SKYSALES.Util.extendObject(parent);

    thisdropdownSportEquipment.sportsdropdown = {};
    thisdropdownSportEquipment.sportsequipmentid = '';
    thisdropdownSportEquipment.quantityPrefix = '';
    thisdropdownSportEquipment.del = '';
    thisdropdownSportEquipment.passengerNumber = 0;
    thisdropdownSportEquipment.flightreference = '';
    thisdropdownSportEquipment.currencyCode = '';
    thisdropdownSportEquipment.selectedItem = '';
    thisdropdownSportEquipment.origDropDownId = '';
    thisdropdownSportEquipment.isDisabled = false;
    thisdropdownSportEquipment.sportequipPriceArray = null;
    thisdropdownSportEquipment.clientId = '';
    thisdropdownSportEquipment.isBookingModified = '';
    thisdropdownSportEquipment.sportsAlertCodeList = '';
    thisdropdownSportEquipment.sportsAlertMessage = '';

    thisdropdownSportEquipment.init = function(json, ssrAvailability, currencyCode) {
        this.setSettingsByObject(json);
        this.setVars();
        this.addEvents();
        this.currencyCode = currencyCode;
        this.initDropDownSprotEquipment(ssrAvailability);
        if (this.sportsdropdown) {
            this.sportsdropdown.val(this.selectedItem);
        }
        if (this.isDisabled) {
            this.sportsdropdown.attr('disabled', true);
            $('#' + this.origDropDownId).val(this.selectedItem);
        }
    };

    thisdropdownSportEquipment.setVars = function() {
        if (this.isDisabled) {
            this.origDropDownId = this.sportsequipmentid;
            this.sportsequipmentid = this.sportsequipmentid + "_";
        }
        this.sportsdropdown = $('#' + this.sportsequipmentid);
    };
    thisdropdownSportEquipment.findMCCPrice = function(ssrCode) {
        var priceCtr = 0;
        if (this.sportequipPriceArray != null) {
            for (priceCtr = 0; priceCtr < this.sportequipPriceArray.length; priceCtr++) {
                if (this.sportequipPriceArray[priceCtr].SsrCode == ssrCode)
                    return this.sportequipPriceArray[priceCtr].SsrPrice;
            }
        }
        return '';
    };
    thisdropdownSportEquipment.initDropDownSprotEquipment = function(ssrAvailability) {
        len1 = ssrAvailability.SsrAvailabilityList.length;
        SSRAvailability = null;
        flightCarrierCode = null;
        var i = 0;
        var selectBoxObj = null;
        selectedItemArray = this.selectedItem ? this.selectedItem.split('_') : null,
        ssrNum = selectedItemArray ? selectedItemArray[jQuery.inArray("ssrNum", selectedItemArray) + 1] : "1"; ;
        for (i = 0; i < len1; i += 1) {

            SSRAvailability = ssrAvailability.SsrAvailabilityList[i];

            if (SSRAvailability.passengerNumber == this.passengerNumber) {
                len2 = SSRAvailability.flightParts.length;
                flightPart = null;
                for (j = 0; j < len2; j += 1) {
                    flightPart = new SKYSALES.Class.FlightPart();
                    flightPart.init(SSRAvailability.flightParts[j]);

                    if (flightPart.flightKey != null) {
                        flightDesignator = '';

                        if (flightPart.flightKey.flightDesignator != null) {
                            flightCarrierCode = flightPart.flightKey.flightDesignator.CarrierCode;
                            flightDesignator = flightPart.flightKey.flightDesignator.CarrierCode + flightPart.flightKey.flightDesignator.FlightNumber;
                            flightDesignator = flightDesignator.replace(' ', '-');
                            flightDesignator = flightDesignator.replace(' ', '-');
                        }


                        flightRef = flightPart.flightKey.departureDate.getDate() +
                                    '-' +
                                    flightDesignator +
                                    '-' +
                                    flightPart.flightKey.departureStation +
                                    flightPart.flightKey.arrivalStation;
                        if (this.flightreference == flightRef) {
                            len3 = flightPart.availableSsrs.length;
                            for (k = 0; k < len3; k += 1) {
                                if (this.sportsdropdown) {
                                    text = flightPart.availableSsrs[k].ssr.name;
                                    price = thisdropdownSportEquipment.findMCCPrice(flightPart.availableSsrs[k].ssr.ssrCode);
                                    text = text + ' (' + price + ')';
                                    value = this.quantityPrefix +
                                            this.del +
                                            "passengerNumber" +
                                            this.del +
                                            this.passengerNumber +
                                            this.del +
                                            "ssrCode" +
                                            this.del +
                                            flightPart.availableSsrs[k].ssr.ssrCode +
                                            this.del +
                                           "ssrNum" +
                                            this.del +
                                            ssrNum +
                                            this.del +
                                            "flightReference" +
                                            this.del +
                                            flightRef;

                                    selectBoxObj = this.sportsdropdown.get(0);
                                    if (selectBoxObj) {
                                        selectBoxObj.options[selectBoxObj.options.length] = new window.Option(text, value, false, false);
                                    }
                                }
                            }
                            //In MMB page,when user have selelct the sports equipments then can not remove to no sports equipment (start)
                            if (selectBoxObj) {
                                if (thisdropdownSportEquipment.isBookingModified == "True" && this.selectedItem != '') {
                                    if (this.selectedItem.indexOf("__") == -1) {
                                        selectBoxObj = this.sportsdropdown.get(0);
                                        if (selectBoxObj) {
                                            for (var m = 0, l = 2; m < l; m++) {
                                                o = selectBoxObj.options[m];
                                                if (m == 0 && o.value == "") {
                                                    selectBoxObj.options[0] = null;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            //(end)                       
                        }
                    }
                }
            }
        }
    };
    thisdropdownSportEquipment.autoselectsport = function() {
        if (this.selectedItem != '') {
            var ID = thisdropdownSportEquipment.sportsequipmentid;
            var selectedText = this.options[this.selectedIndex].text;
            var selectedValue = this.options[this.selectedIndex].value;
            var flightRef = thisdropdownSportEquipment.flightreference;
            var elems = document.getElementsByTagName("select");
            var matches = [];
            if ((selectedValue.length > 0 || selectedText.length > 0) && flightRef.length > 0) {
                var ssrCode = selectedValue.substring(selectedValue.indexOf("ssrCode"), selectedValue.indexOf("flightReference") - 1);
                var passengerNum = ID.substring(ID.indexOf("passengerNumber"), ID.indexOf("flightReference") - 1);
                var idpre = ID.substring(0, ID.lastIndexOf("flightReference") - 1);

                for (var i = 0, m = elems.length; i < m; i++) {
                    if (!elems[i].disabled) {
                        if (elems[i].id && elems[i].id != ID && elems[i].id.indexOf(passengerNum) != -1 && (elems[i].id.indexOf(idpre) != -1)) {
                            matches.push(elems[i]);
                        }
                    }
                }
                if (matches) {
                    for (var x = 0, y = matches.length; x < y; x++) {
                        var nextID = matches[x].id;
                        var select = document.getElementById(nextID);
                        var journey1 = flightRef.substring(flightRef.lastIndexOf("-") + 1, flightRef.length);
                        var journey2 = nextID.substring(nextID.lastIndexOf("-") + 1, nextID.length);
                        var journey1from = journey1.substring(0, 3);
                        var journey1to = journey1.substring(3, journey1.length);
                        var journey2from = journey2.substring(0, 3);
                        var journey2to = journey2.substring(3, journey2.length);
                        if (!(journey1from == journey2to && journey2from == journey1to) && (journey1to == journey2from || journey1from == journey2to)) {

                            if (ssrCode.length > 0) {
                                for (var i = 0, l = select.options.length; i < l; i++) {
                                    o = select.options[i];
                                    if (o.value.indexOf(ssrCode) != -1) {
                                        o.selected = true;
                                    }
                                }
                            }
                            else if (selectedText.length > 0) {
                                for (var i = 0, l = select.options.length; i < l; i++) {
                                    o = select.options[i];
                                    if (o.text.indexOf(selectedText) != -1) {
                                        o.selected = true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            // add by super 2012-03-02 when sports equipment >32kg will prompt a notice (start)
            var alertcodestr = thisdropdownSportEquipment.sportsAlertCodeList
            var alertcodelist = alertcodestr.split(',');
            for (var i = 0; i < alertcodelist.length; i++) {
                var tempalertcode = alertcodelist[i];
                if (selectedValue.indexOf(tempalertcode) != -1) {
                    alert(thisdropdownSportEquipment.sportsAlertMessage);
                    break;
                }
            }
            //add by super 2012-02-29 when baggage >32kg will prompt a notice (end)
        }
    };
    thisdropdownSportEquipment.addEvents = function() {
        thisdropdownSportEquipment.sportsdropdown.change(this.autoselectsport);
    };
    return thisdropdownSportEquipment;
}
/****end*** by super chen   */

/*
Name: 
Class SSRAvailability
Param:
None
Return: 
An instance of SSRAvailability
Functionality:
This class represents the JSON object created by the Navitaire.NewSkies.UI.Web.Rez.Controls.SSRAvailability Class.
Notes:
Class Hierarchy:
SkySales -> SSRAvailability
*/
SKYSALES.Class.SSRAvailability = function() {
    var parent = new SKYSALES.Class.SkySales(),
    thisSSRAvailability = SKYSALES.Util.extendObject(parent);

    thisSSRAvailability.SsrAvailabilityList = [];

    thisSSRAvailability.init = function(json) {
        this.setSettingsByObject(json);
        this.setVars();
        this.addEvents();
        this.initSsrAvailabilityList();
    };

    thisSSRAvailability.initSsrAvailabilityList = function() {
        var i = 0,
        SSRAvailabilityArray = this.SsrAvailabilityList || [],
        len = SSRAvailabilityArray.length,
        SSRAvailability = null;
        for (i = 0; i < len; i += 1) {
            SSRAvailability = new SKYSALES.Class.SsrAvailabilityPassenger();
            SSRAvailability.init(SSRAvailabilityArray[i]);
            this.SsrAvailabilityList[i] = SSRAvailability;
        }
    };

    return thisSSRAvailability;
}

/*
Name: 
Class SsrAvailabilityPassenger
Param:
None
Return: 
An instance of SsrAvailabilityPassenger
Functionality:
This class represents the Navitaire.NewSkies.UI.Common.Rez.Data.SsrAvailability.Passenger class.
Notes:
Class Hierarchy:
SkySales -> SsrAvailabilityPassenger
*/
SKYSALES.Class.SsrAvailabilityPassenger = function() {
    var parent = new SKYSALES.Class.SkySales(),
    thisSsrAvailabilityPassenger = SKYSALES.Util.extendObject(parent);

    thisSsrAvailabilityPassenger.passengerNumber = 0;
    thisSsrAvailabilityPassenger.flightParts = [];

    thisSsrAvailabilityPassenger.init = function(json) {
        this.setSettingsByObject(json);
        this.setVars();
        this.addEvents();
        this.initFlightPartsArray();
    };

    thisSsrAvailabilityPassenger.initFlightPartsArray = function() {
        var i = 0,
        flightPartsArray = this.flightParts || [],
        len = flightPartsArray.length,
        flightPart = null;
        for (i = 0; i < len; i += 1) {
            flightPart = new SKYSALES.Class.FlightPart();
            flightPart.init(flightPartsArray[i]);
            this.flightParts[i] = flightPart;
        }
    };

    return thisSsrAvailabilityPassenger;
}

/*
Name: 
Class FlightPart
Param:
None
Return: 
An instance of FlightPart
Functionality:
This class represents the Navitaire.NewSkies.UI.Common.Rez.Data.SsrAvailability.FlightPart class.
Notes:
Class Hierarchy:
SkySales -> FlightPart
*/
SKYSALES.Class.FlightPart = function() {
    var parent = new SKYSALES.Class.SkySales(),
    thisFlightPart = SKYSALES.Util.extendObject(parent);

    thisFlightPart.flightKey = null;
    thisFlightPart.availableSsrs = [];

    thisFlightPart.init = function(json) {
        this.setSettingsByObject(json);
        this.setVars();
        this.addEvents();
        this.initFlightKey();
        this.initAvailableSsrsArray();
    };

    thisFlightPart.initFlightKey = function() {
        var flightKeyObject = new SKYSALES.Class.FlightKey();
        flightKeyObject.init(this.flightKey);
        this.flightKey = flightKeyObject;
    };

    thisFlightPart.initAvailableSsrsArray = function() {
        var i = 0,
        availableSsrsObject = this.availableSsrs || [],
        len = availableSsrsObject.length,
        availableSsr = null;
        for (i = 0; i < len; i += 1) {
            availableSsr = new SKYSALES.Class.AvailableSsr();
            availableSsr.init(availableSsrsObject[i]);
            this.availableSsrs[i] = availableSsr;
        }
    };

    return thisFlightPart;
}

/*
Name: 
Class FlightKey
Param:
None
Return: 
An instance of FlightKey
Functionality:
This class represents the Navitaire.NewSkies.Booking.Contract.FlightKey class.
Notes:
Class Hierarchy:
SkySales -> FlightKey
*/
SKYSALES.Class.FlightKey = function() {
    var parent = new SKYSALES.Class.SkySales(),
    thisFlightKey = SKYSALES.Util.extendObject(parent);

    thisFlightKey.arrivalStation = '';
    thisFlightKey.departureDate = '';
    thisFlightKey.departureStation = '';
    thisFlightKey.flightDesignator = null;

    thisFlightKey.init = function(json) {
        this.setSettingsByObject(json);
        this.setVars();
        this.addEvents();
        this.initDepartureDate();
        this.initFlightDesignator();
    };

    thisFlightKey.initDepartureDate = function() {
        var date = SKYSALES.Class.Date();
        date.init(this.departureDate);
        this.departureDate = date;
    };

    thisFlightKey.initFlightDesignator = function() {
        var flightDesignatorObject = SKYSALES.Class.FlightDesignator();
        flightDesignatorObject.init(this.flightDesignator);
        this.flightDesignator = flightDesignatorObject;
    };

    return thisFlightKey;
}

/*
Name: 
Class FlightDesignator
Param:
None
Return: 
An instance of FlightDesignator
Functionality:
This class represents the Navitaire.NewSkies.Booking.Contract.FlightDesignator class.
Notes:
Class Hierarchy:
SkySales -> FlightDesignator
*/
SKYSALES.Class.FlightDesignator = function() {
    var parent = new SKYSALES.Class.SkySales(),
    thisFlightDesignator = SKYSALES.Util.extendObject(parent);

    thisFlightDesignator.CarrierCode = '';
    thisFlightDesignator.FlightNumber = '';

    thisFlightDesignator.init = function(json) {
        this.setSettingsByObject(json);
        this.setVars();
        this.addEvents();
    };

    return thisFlightDesignator;
}

/*
Name: 
Class AvailableSsr
Param:
None
Return: 
An instance of AvailableSsr
Functionality:
This class represents the Navitaire.NewSkies.UI.Common.Rez.Data.SsrAvailability.AvailableSsr class.
Notes:
Class Hierarchy:
SkySales -> AvailableSsr
*/
SKYSALES.Class.AvailableSsr = function() {
    var parent = new SKYSALES.Class.SkySales(),
    thisAvailableSSR = SKYSALES.Util.extendObject(parent);

    thisAvailableSSR.price = '';
    thisAvailableSSR.ssr = null;

    thisAvailableSSR.init = function(json) {
        this.setSettingsByObject(json);
        this.setVars();
        this.addEvents();
        this.initSSR();
    };

    thisAvailableSSR.initSSR = function() {
        var ssrObject = SKYSALES.Class.SSR();
        ssrObject.init(this.ssr);
        this.ssr = ssrObject;
    };

    return thisAvailableSSR;
}

/*
Name: 
Class SSR
Param:
None
Return: 
An instance of SSR
Functionality:
This class represents the Navitaire.NewSkies.Booking.Contract.SSR class.
Notes:
Class Hierarchy:
SkySales -> SSR
*/
SKYSALES.Class.SSR = function() {
    var parent = new SKYSALES.Class.SkySales(),
    thisSSR = SKYSALES.Util.extendObject(parent);

    thisSSR.name = '';
    thisSSR.ssrCode = '';

    thisSSR.init = function(json) {
        this.setSettingsByObject(json);
        this.setVars();
        this.addEvents();
    };

    return thisSSR;
}

/* End SSR Availability Input */

/* Start InsuranceInput */

if (SKYSALES.Class.InsuranceInput === undefined) {
    /*
    Class InsuranceInput
    */
    SKYSALES.Class.InsuranceInput = function() {
        var parent = new SKYSALES.Class.SkySales();
        var thisInsuranceInput = SKYSALES.Util.extendObject(parent);

        thisInsuranceInput.clientId = '';
        thisInsuranceInput.insuranceAcceptInputId = '';
        thisInsuranceInput.insuranceAcceptInput = null;
        thisInsuranceInput.insuranceAcceptMessage = '';
        thisInsuranceInput.yesInsuranceInputId = '';
        thisInsuranceInput.yesInsuranceInput = null;
        thisInsuranceInput.submitButtonId = '';
        thisInsuranceInput.submitButton = null;
        thisInsuranceInput.linkButtonDeclineId = '';
        thisInsuranceInput.linkButtonDecline = null;
        thisInsuranceInput.continueButtonId = '';
        thisInsuranceInput.continueButton = null;
        thisInsuranceInput.validateValueId = '';
        thisInsuranceInput.confirmCancelNote = '';
        thisInsuranceInput.noAUSInsuranceInputId = '';
        thisInsuranceInput.noAUSInsuranceInput = null;
        thisInsuranceInput.departCountryCode = '';
        thisInsuranceInput.AUSNoCheckedAlert = '';
        thisInsuranceInput.confirmCancelNoteOptOut = '';
        thisInsuranceInput.insuranceAcceptMessageOptOut = '';

        thisInsuranceInput.setSettingsByObject = function(jsonObject) {
            parent.setSettingsByObject.call(this, jsonObject);
            var propName = '';
            for (propName in jsonObject) {
                if (thisInsuranceInput.hasOwnProperty(propName)) {
                    thisInsuranceInput[propName] = jsonObject[propName];
                }
            }
        };

        thisInsuranceInput.addEvents = function() {
            parent.addEvents.call(this);
            if ($('#' + thisInsuranceInput.yesInsuranceInputId).length > 0) {
                thisInsuranceInput.yesInsuranceInput.click(thisInsuranceInput.clickInsuranceInput);
            }
            if ($('#' + thisInsuranceInput.insuranceAcceptInputId).length > 0) {
                if (thisInsuranceInput.departCountryCode == "AU" || thisInsuranceInput.departCountryCode == "NZ")
                {
                    thisInsuranceInput.insuranceAcceptInput.click(thisInsuranceInput.clickYesInsuranceInputAUS);
                }
                thisInsuranceInput.continueButton.click(thisInsuranceInput.validateContinue);
            }
            if ($('#' + thisInsuranceInput.linkButtonDeclineId).length > 0) {
                thisInsuranceInput.linkButtonDecline.click(thisInsuranceInput.confirmCancelInsuranceHandler);
            }
            if ($('#' + thisInsuranceInput.noAUSInsuranceInputId).length > 0) {
                thisInsuranceInput.noAUSInsuranceInput.click(thisInsuranceInput.clickNoInsuranceInputAUS);
                //if (thisInsuranceInput.departCountryCode == "AU")
                //{
                    thisInsuranceInput.continueButton.click(thisInsuranceInput.validateCancel);
                //}
                    
            }
        };

        thisInsuranceInput.setVars = function() {
            parent.setVars.call(this);
            if ($('#' + thisInsuranceInput.insuranceAcceptInputId).length > 0) { thisInsuranceInput.insuranceAcceptInput = $('#' + thisInsuranceInput.insuranceAcceptInputId); }
            if ($('#' + thisInsuranceInput.yesInsuranceInputId).length > 0) { thisInsuranceInput.yesInsuranceInput = $('#' + thisInsuranceInput.yesInsuranceInputId); }
            if ($('#' + thisInsuranceInput.submitButtonId).length > 0) { thisInsuranceInput.submitButton = $('#' + thisInsuranceInput.submitButtonId); }
            if ($('#' + thisInsuranceInput.continueButtonId).length > 0) { thisInsuranceInput.continueButton = $('#' + thisInsuranceInput.continueButtonId); }
            if ($('#' + thisInsuranceInput.linkButtonDeclineId).length > 0) { thisInsuranceInput.linkButtonDecline = $('#' + thisInsuranceInput.linkButtonDeclineId); }
            if ($('#' + thisInsuranceInput.noAUSInsuranceInputId).length > 0) { thisInsuranceInput.noAUSInsuranceInput = $('#' + thisInsuranceInput.noAUSInsuranceInputId); }
        };

        thisInsuranceInput.init = function(jsonObject) {
            this.setSettingsByObject(jsonObject);
            this.setVars();
            this.addEvents();
        };

        thisInsuranceInput.confirmCancelInsuranceHandler = function() {
            if (thisInsuranceInput.departCountryCode == "AU" || thisInsuranceInput.departCountryCode == "NZ") {
                if (thisInsuranceInput.noAUSInsuranceInput.attr('checked')) {                   
                    if (window.confirm(thisInsuranceInput.confirmCancelNoteOptOut)) {
                        SKYSALES.DisplayLoadingBar();//Added by Linson at 2012-03-26
                        __doPostBack('CONTROLGROUPADDONS$InsuranceInputAddOnsView$LinkButtonInsuranceDecline','')
                     }
                     else{
                        thisInsuranceInput.insuranceAcceptInput.attr('checked', true);
                        thisInsuranceInput.noAUSInsuranceInput.attr('checked', false);
                        return false;
                    }
                }
            }
            else {
                if (window.confirm(thisInsuranceInput.confirmCancelNote)) {
                    return false;
                }
                SKYSALES.DisplayLoadingBar();//Added by Linson at 2012-03-26
            }
        };

        thisInsuranceInput.clickInsuranceInput = function() {
            SKYSALES.DisplayLoadingBar();//Added by Linson at 2012-03-26
            document.getElementById(thisInsuranceInput.submitButtonId).click();
        }
        
        thisInsuranceInput.validateCancel = function() {
            if (thisInsuranceInput.departCountryCode == "AU" || thisInsuranceInput.departCountryCode == "NZ") {
                if (thisInsuranceInput.noAUSInsuranceInput.attr('checked')) { 
                     $('#' + thisInsuranceInput.validateValueId).val("false");                  
                     $('#' + thisInsuranceInput.linkButtonDeclineId).click();
                }
            }
        };

        thisInsuranceInput.validateContinue = function() {
            if (thisInsuranceInput.departCountryCode == "AU" || thisInsuranceInput.departCountryCode == "NZ") {
                if (thisInsuranceInput.insuranceAcceptInput.attr('checked')) {
                    $('#' + thisInsuranceInput.validateValueId).val("true");
                }
                else {
                    if (thisInsuranceInput.noAUSInsuranceInput.attr('checked')) {
                    }
                    else{
                        $('#' + thisInsuranceInput.validateValueId).val("false");
                       alert(thisInsuranceInput.insuranceAcceptMessageOptOut);
                     }
                }
            }
            else {
                if (thisInsuranceInput.insuranceAcceptInput.attr('checked')) {
                    $('#' + thisInsuranceInput.validateValueId).val("true");
                }
                else {
                    $('#' + thisInsuranceInput.validateValueId).val("false");
                    alert(thisInsuranceInput.insuranceAcceptMessage);
                }
            }
        }

        thisInsuranceInput.clickYesInsuranceInputAUS = function() {
            var bool1 = thisInsuranceInput.insuranceAcceptInput != null ? thisInsuranceInput.insuranceAcceptInput.attr('checked') : true;
            var bool2 = thisInsuranceInput.noAUSInsuranceInput != null ? thisInsuranceInput.noAUSInsuranceInput.attr('checked') : true;

            if (bool1 && bool2) {
                var checkControl = thisInsuranceInput.noAUSInsuranceInput == undefined ? thisInsuranceInput.insuranceAcceptInput : thisInsuranceInput.noAUSInsuranceInput;
                checkControl.attr('checked', false);
            }
        }

        thisInsuranceInput.clickNoInsuranceInputAUS = function() {
            if (thisInsuranceInput.insuranceAcceptInput.attr('checked') && thisInsuranceInput.noAUSInsuranceInput.attr('checked')) {
                thisInsuranceInput.insuranceAcceptInput.attr('checked', false);
            }
        }

        return thisInsuranceInput;
    };

    SKYSALES.Class.InsuranceInput.createObject = function(json) {
        SKYSALES.Util.createObject('insuranceInput', 'InsuranceInput', json);
    };
}

/* End InsuranceInput */

SKYSALES.fareRuleHandler = function(keys, markets) {
    if (keys.length > 0) {
        var infoDelimiter = '|';
        var keyDelimiter = '~';
        var carrierDelimiter = '^';
        var carrierCode = '';
        var carrierCode2 = '';
        flightInfos = keys[0].split(infoDelimiter);
        //Added by Michelle 15Dec2010 - Show fare rule message for connecting flight with different carrier codes 
        if (flightInfos[1].indexOf('^')!= -1){
                 carrierInfos = flightInfos[1].split(carrierDelimiter);
                 carrierKey1 = carrierInfos[0].split(keyDelimiter);
                 carrierKey2 = carrierInfos[1].split(keyDelimiter);
                 carrierCode = carrierKey1[0];
                 carrierCode2 = carrierKey2[0];
                 if(carrierCode != carrierCode2 && (carrierCode == 'D7' || carrierCode2 == 'D7'))
                 {
                    carrierCode = 'Connecting';
                 }
        }
        else{
        flightKeys = flightInfos[1].split(keyDelimiter);
        carrierCode = flightKeys[0];
        }

        if ($('#CheckBoxAgreementLabel').length > 0) { $('#CheckBoxAgreementLabel').hide(); }
        if ($('#CheckBoxAgreementLabelAK').length > 0) { $('#CheckBoxAgreementLabelAK').hide(); }
        if ($('#CheckBoxAgreementLabelFD').length > 0) { $('#CheckBoxAgreementLabelFD').hide(); }
        if ($('#CheckBoxAgreementLabelQZ').length > 0) { $('#CheckBoxAgreementLabelQZ').hide(); }
        if ($('#CheckBoxAgreementLabelPQ').length > 0) { $('#CheckBoxAgreementLabelPQ').hide(); }
        if ($('#CheckBoxAgreementLabelJW').length > 0) { $('#CheckBoxAgreementLabelJW').hide(); }
        if ($('#CheckBoxAgreementLabelD7').length > 0) { $('#CheckBoxAgreementLabelD7').hide(); }
        if ($('#CheckBoxAgreementLabelConnecting').length > 0) { $('#CheckBoxAgreementLabelConnecting').hide(); }
        
        if ($('#CheckBoxAgreementLabel' + carrierCode).length > 0) {
            $('#CheckBoxAgreementLabel' + carrierCode).css('display', 'inline');
        }
        else {
            if ($('#CheckBoxAgreementLabel').length > 0) { $('#CheckBoxAgreementLabel').css('display', 'inline'); }
        }
    }
    else {
        $('#agreementInput').hide();
        $('#ControlGroupSelectView_ButtonSubmit').hide();
    }
};

/* Start PaxSSRDisplayAtaGlance */

SKYSALES.Class.PaxSSRDisplayAtaGlance = function() {
    var parent = new SKYSALES.Class.SkySales();
    var thisPaxSSRDisplayAtaGlance = SKYSALES.Util.extendObject(parent);

    thisPaxSSRDisplayAtaGlance.paxNameDisplayId = '';
    thisPaxSSRDisplayAtaGlance.ssrListDisplayId = '';
    thisPaxSSRDisplayAtaGlance.paxNameDisplayObj = null;
    thisPaxSSRDisplayAtaGlance.paxNameSpanDisplayObj = null;
    thisPaxSSRDisplayAtaGlance.ssrListDisplayObj = null;

    thisPaxSSRDisplayAtaGlance.init = function(json) {
        this.setSettingsByObject(json);
        this.setVars();
        this.addEvents();
    };

    thisPaxSSRDisplayAtaGlance.setVars = function() {
        thisPaxSSRDisplayAtaGlance.paxNameSpanDisplayObj = $('#' + this.paxNameDisplayId + 'span');
        thisPaxSSRDisplayAtaGlance.paxNameDisplayObj = $('#' + this.paxNameDisplayId);
        thisPaxSSRDisplayAtaGlance.ssrListDisplayObj = $('#' + this.ssrListDisplayId);
    };

    thisPaxSSRDisplayAtaGlance.addEvents = function() {
        thisPaxSSRDisplayAtaGlance.paxNameDisplayObj.click(this.hideShowSSRListHandler);
        thisPaxSSRDisplayAtaGlance.paxNameSpanDisplayObj.click(this.hideShowSSRListHandler);
    };

    thisPaxSSRDisplayAtaGlance.hideShowSSRListHandler = function() {
        if (thisPaxSSRDisplayAtaGlance.paxNameDisplayObj.hasClass('expand_iconSml')) {
            thisPaxSSRDisplayAtaGlance.paxNameDisplayObj.removeClass('expand_iconSml');
            thisPaxSSRDisplayAtaGlance.paxNameDisplayObj.addClass('collapse_iconSml');
        }
        else {
            thisPaxSSRDisplayAtaGlance.paxNameDisplayObj.removeClass('collapse_iconSml');
            thisPaxSSRDisplayAtaGlance.paxNameDisplayObj.addClass('expand_iconSml');
        }
        thisPaxSSRDisplayAtaGlance.ssrListDisplayObj.slideToggle();
    };

    return thisPaxSSRDisplayAtaGlance;
};

SKYSALES.Class.PaxSSRDisplayAtaGlance.createObject = function(json) {
    SKYSALES.Util.createObject('paxSSRDisplayAtaGlance', 'PaxSSRDisplayAtaGlance', json);
};

/* End PaxSSRDisplayAtaGlance */

/* Start Passenger Input */

//added by Linson at 2012-02-28 begin  desc: populatePassenger for anonymous
function populatePassengerForAnonymous(clientId,object, index, nameChange){
    if(contactData){
        contactData.title=$("#" + clientId + "_DropDownListTitle").val();
        if (contactData.title === "MR") {
            contactData.gender="Male";
        }
        else if (contactData.title === "MS") {
            contactData.gender="Female";
        }
        contactData.firstName=$("#" + clientId + "_TextBoxFirstName").val();
        contactData.middleName=$("#" + clientId + "_TextBoxMiddleName").val();
        contactData.lastName=$("#" + clientId + "_TextBoxLastName").val();
        contactData.ff="";
        contactData.bdayDay="";
        contactData.bdayMonth="";
        contactData.bdayYear="";
        contactData.nationality=$("#" + clientId + "_DropDownListCountry").val();
        contactData.countryOfResidence="";                           
        populatePassenger(object, index, nameChange);
    }
}
//added by Linson at 2012-02-28 end

//Replacement of a ton of script in the passenger control
function populatePassenger(object, index, nameChange) {
    //Don't even do anything unless contactData is available
    if (contactData) {
        //get the fieldset that has all the fields for the passenger passed in
        var fieldsetIndex = index - 1;
        var passengerFields = $('#passengerInputContainer' + fieldsetIndex);
        
        var contactFname = "";
        if (contactData.firstName != "") {
            contactFname = contactData.firstName;
        }
        else {
            contactFname = contactData.lastName;
        }
        
        //these are the items that are maped to the contactData item included in the passengerFields control
        var fieldIdentifiers = [
            { 'name': 'DropDownListTitle', 'value': contactData.title },
            { 'name': 'TextBoxFirstName', 'value': contactFname },
            { 'name': 'TextBoxMiddleName', 'value': contactData.middleName },
            { 'name': 'TextBoxLastName', 'value': contactData.lastName },
            { 'name': 'TextBoxCustomerNumber', 'value': contactData.ff },
            { 'name': 'DropDownListBirthDateDay', 'value': contactData.bdayDay },
            { 'name': 'DropDownListBirthDateMonth', 'value': contactData.bdayMonth },
            { 'name': 'DropDownListBirthDateYear', 'value': contactData.bdayYear },
            { 'name': 'DropDownListGender', 'value': contactData.gender },
            { 'name': 'DropDownListNationality', 'value': contactData.nationality },
            { 'name': 'DropDownListResidentCountry', 'value': contactData.countryOfResidence }
       ];
    }

    if ($('#' + object.id + ':checked').val() == null && nameChange != "False") {
        $.map(fieldIdentifiers, function(obj) {
            if (obj)
                $(":input[@id*=" + obj.name + "]", passengerFields).val('');
        });
    }
    else if (nameChange != "False") {
        $.map(fieldIdentifiers, function(obj) {
            if (obj)
                $(":input[@id*=" + obj.name + "]", passengerFields).val(obj.value);
        });
    }
    else if ($('#' + object.id + ':checked').val() == null && nameChange == "False") {
        $.map(fieldIdentifiers, function(obj) {
            if (obj) {
                if ((obj.name != "TextBoxFirstName") && (obj.name != "TextBoxLastName"))
                    $(":input[@id*=" + obj.name + "]", passengerFields).val('');
            }
        });
    }
    else if (nameChange == "False") {
        $.map(fieldIdentifiers, function(obj) {
            if (obj) {
                if ((obj.name != "TextBoxFirstName") && (obj.name != "TextBoxLastName"))
                    $(":input[@id*=" + obj.name + "]", passengerFields).val(obj.value);
            }
        });
    }
}

/* End Passenger Input */

/* Start Booking Retrieve Input */

/*
Name: 
Class ControlGroupBookingRetrieve
Param:
None
Return: 
An instance of ControlGroupBookingRetrieve
Functionality:
Handles a ControlGroupBookingRetrieve validation
Notes:
        
Class Hierarchy:
SkySales -> ControlGroupBookingRetrieve
*/
if (!SKYSALES.Class.ControlGroupBookingRetrieve) {
    SKYSALES.Class.ControlGroupBookingRetrieve = function() {
        var parent = new SKYSALES.Class.ControlGroup();
        var thisControlGroupBookingRetrieve = SKYSALES.Util.extendObject(parent);

        thisControlGroupBookingRetrieve.bookingRetrieve = null;

        thisControlGroupBookingRetrieve.init = function(json) {
            this.setSettingsByObject(json);

            var bookingRetrieve = new SKYSALES.Class.BookingRetrieve();
            bookingRetrieve.init(json);
            thisControlGroupBookingRetrieve.bookingRetrieve = bookingRetrieve;

            this.setVars();
            this.addEvents();
        };

        thisControlGroupBookingRetrieve.validateHandler = function() {
            var retVal = thisControlGroupBookingRetrieve.validate();
            return retVal;
        };
        thisControlGroupBookingRetrieve.validate = function() {
            var retVal = false;
            var bookingRetrieve = this.bookingRetrieve;
            retVal = bookingRetrieve.isOneSectionPopulated();
            if (retVal) {
                retVal = parent.validate.call(this);
            }
            return retVal;
        };
        return thisControlGroupBookingRetrieve;
    };
}

/*
Name: 
Class BookingRetrieve
Param:
None
Return: 
An instance of BookingRetrieve
Functionality:
Handles a BookingRetrieve
Notes:
        
Class Hierarchy:
SkySales -> FlightSearch -> BookingRetrieve
*/
if (!SKYSALES.Class.BookingRetrieve) {
    SKYSALES.Class.BookingRetrieve = function() {
        var parent = new SKYSALES.Class.FlightSearch();
        var thisBookingRetrieve = SKYSALES.Util.extendObject(parent);

        thisBookingRetrieve.optionHeaderText = [];
        thisBookingRetrieve.marketArray = [];
        thisBookingRetrieve.missingInformation = '';
        thisBookingRetrieve.sectionValidation = {};

        thisBookingRetrieve.init = function(json) {
            this.setSettingsByObject(json);
            this.setVars();
            this.addEvents();
        };

        thisBookingRetrieve.setVars = function() {
            parent.setVars.call(this);
            var i = 0;
            var len = 0;
            var sectionValidation = this.sectionValidation;
            var sectionArray = [];
            var prop = '';
            var validate = null;

            for (prop in sectionValidation) {
                if (sectionValidation.hasOwnProperty(prop)) {
                    sectionArray = sectionValidation[prop] || [];
                    len = sectionArray.length;
                    for (i = 0; i < len; i += 1) {
                        validate = sectionArray[i];
                        validate.input = this.getById(validate.id);
                    }
                }
            }
        };

        thisBookingRetrieve.isOneSectionPopulated = function() {
            var i = 0;
            var len = 0;
            var sectionValidation = this.sectionValidation;
            var sectionArray = [];
            var prop = '';
            var validate = null;
            var input = null;
            var retVal = false;
            var value = '';
            var requiredempty = '';
            var sectionIsPopulated = true;

            for (prop in sectionValidation) {
                if (sectionValidation.hasOwnProperty(prop)) {
                    sectionArray = sectionValidation[prop] || [];
                    len = sectionArray.length;
                    sectionIsPopulated = true;
                    for (i = 0; i < len; i += 1) {
                        validate = sectionArray[i];
                        input = validate.input.get(0);
                        if (input) {
                            value = input.value;
                            requiredempty = value.requiredempty || '';
                            if (value === requiredempty) {
                                value = '';
                            }
                            if (!value) {
                                sectionIsPopulated = false;
                                break;
                            }
                        }
                    }
                    if (sectionIsPopulated) {
                        retVal = true;
                        break;
                    }
                }
            }
            if (!retVal) {
                //TODO: Make this trigger a validation error, instead of an alert
                alert(this.missingInformation);
                SKYSALES.RemoveLoadingBar(); //added by Linson at 2012-01-10
            }
            return retVal;
        };

        return thisBookingRetrieve;
    };
}

/* End Booking Retrieve Input */

/* Start MCC Dev Implementation */

/*
Name: 
Class MCCInput
Param:
None
Return: 
An instance of MCCInput
Functionality:
This class represents a MCCInput
Notes:
Class Hierarchy:
SkySales -> MCCInput
*/
SKYSALES.Class.MCCInput = function() {
    var parent = SKYSALES.Class.SkySales(),
        thisMCCInput = SKYSALES.Util.extendObject(parent),
        resource = SKYSALES.Util.getResource();

    thisMCCInput.clientId = "";
    thisMCCInput.contentId = "";
    thisMCCInput.bookingCurrencyCode = '';
    thisMCCInput.externalRateId = '';
    thisMCCInput.defaultCurrencyValue = '';
    thisMCCInput.dropDownListCurrencyId = '';
    thisMCCInput.dropDownListCurrency = null;
    thisMCCInput.dropDownOrigin = null;
    thisMCCInput.currencyArray = [];
    thisMCCInput.externalRateInfo = resource.externalRateInfo;
    thisMCCInput.currencyHash = resource.currencyHash;
    thisMCCInput.stationHash = SKYSALES.Util.getResource().stationHash;
    thisMCCInput.applyMCCBtnId = '';   //added by Linson at 2012-01-05
    thisMCCInput.applyMCCButton = null;   //added by Linson at 2012-01-05      
    thisMCCInput.setVars = function() {
        thisMCCInput.dropDownListCurrency = this.getById(this.dropDownListCurrencyId);
        thisMCCInput.dropDownOrigin = $('#' + this.contentId + ' :input[id*=TextBoxMarketOrigin1]');
        //added by Linson at 2012-01-05 begin
        if (thisMCCInput.applyMCCBtnId != '') {
            thisMCCInput.applyMCCButton = $("#" + thisMCCInput.applyMCCBtnId);
            if (thisMCCInput.applyMCCButton) thisMCCInput.applyMCCButton.attr('onclick', 'SKYSALES.DisplayLoadingBar()');
        }
        //added by Linson at 2012-01-05 end          
    };

    thisMCCInput.updateCurrencyHandler = function() {
        thisMCCInput.updateCurrency();
    };

    thisMCCInput.populateCurrency = function() {
        this.updateCurrency();
    };

    thisMCCInput.updateCurrency = function() {
        var externalRateList = this.externalRateInfo.ExternalRateList,
            key = '',
            selectedItem = '',
            externalRateArray = [],
            externalRate = null,
            currency = null,
            stationCurrency = null,
            defaultItem = {},
            description = '',
            origin = '',
            currencyIndex = 0,
            originCountryCode = {};

        if (this.dropDownOrigin.length > 0) {
            origin = this.dropDownOrigin.val();
            originCountryCode = this.stationHash[origin];
        }
        else {
            origin = $('#MCCOriginCountry').val();
            originCountryCode.countryCode = origin;
        }

        if (origin !== '' && origin !== undefined) {
            var currencyArray = this.currencyArray;
            for (currencyIndex in currencyArray) {
                if (currencyArray[currencyIndex].countryCode === originCountryCode.countryCode) {

                    currency = this.currencyHash[currencyArray[currencyIndex].currencyCode];
                    if (currency) {
                        description = currency.name + " (" + currencyArray[currencyIndex].currencyCode + ")";
                    }
                    else {
                        description = currencyArray[currencyIndex].currencyCode;
                    }

                    defaultItem = { "code": this.defaultCurrencyValue, "name": description };
                    externalRateArray[externalRateArray.length] = defaultItem;

                    for (key in externalRateList) {
                        if (externalRateList.hasOwnProperty(key)) {
                            externalRate = externalRateList[key];

                            if (externalRate.quotedCurrency === currencyArray[currencyIndex].currencyCode) {
                                currency = this.currencyHash[externalRate.collectedCurrency];

                                if (currency) {
                                    externalRate.name = currency.name + " (" + currency.code + ")";
                                }
                                else {
                                    externalRate.name = externalRate.collectedCurrency;
                                }

                                externalRateArray[externalRateArray.length] = externalRate;
                            }
                        }
                    }
                    break;
                }
            }

            // null is the value for the 1st option in MCC dropdown, set in the culture file
            if (!isItemInObject(this.externalRateId, externalRateArray) || !this.externalRateId || this.externalRateId === 'null') {
                selectedItem = "default";
            }
            else {
                selectedItem = this.externalRateId;
            }

            var paramObject = {
                "objectArray": externalRateArray,
                "selectBox": this.dropDownListCurrency,
                "selectedItem": selectedItem,
                "showCode": false
            };

            SKYSALES.Util.populateSelect(paramObject);
        }
    };

    var isItemInObject = function(item, objectArray) {
        if (item && item !== null) {
            for (objectIndex in objectArray) {
                if (objectArray[objectIndex].code == item) {
                    return true;
                }
            } 
        }
        return false;
    }

    thisMCCInput.addEvents = function() {
        this.dropDownOrigin.change(this.updateCurrencyHandler);
    };

    thisMCCInput.init = function(json) {
        this.setSettingsByObject(json);
        this.setVars();
        this.addEvents();

        this.populateCurrency();
    };

    return thisMCCInput;
};

/* End MCC Dev Implemetnation */

/* Start MCC NPS Implementation */

if (!SKYSALES.Class.MCCSearchInput) {
    SKYSALES.Class.MCCSearchInput = function() {
        var parent = new SKYSALES.Class.SkySales();
        var thisMCCSearchInput = SKYSALES.Util.extendObject(parent);

        thisMCCSearchInput.clientId = "";
        thisMCCSearchInput.contentId = "";
        thisMCCSearchInput.currencyArray = [];
        thisMCCSearchInput.currencyInfoArray = [];
        thisMCCSearchInput.MCCArray = [];
        thisMCCSearchInput.dropDownCurrencyId = "";
        thisMCCSearchInput.selectedMCC = "";
        thisMCCSearchInput.resetPaymentsText = "";
        thisMCCSearchInput.hasUncommittedPayment = "";
        thisMCCSearchInput.applyButtonId = "";

        thisMCCSearchInput.dropDownOrigin = null;
        thisMCCSearchInput.dropDownCurrency = null;
        thisMCCSearchInput.applyButton = null;
        thisMCCSearchInput.stationHash = SKYSALES.Util.getResource().stationHash;

        thisMCCSearchInput.setVars = function() {
            parent.setVars.call(this);
            thisMCCSearchInput.dropDownOrigin = $('#' + this.contentId + ' :input[id*=TextBoxMarketOrigin1]');
            thisMCCSearchInput.dropDownCurrency = $('#' + this.dropDownCurrencyId);
            thisMCCSearchInput.applyButton = $('#' + this.applyButtonId) || {};
        };

        thisMCCSearchInput.init = function(json) {
            this.setSettingsByObject(json);
            this.setVars();
            this.addEvents();
            this.initializeCurrency();
        };

//        thisMCCSearchInput.setSettingsByObject = function(json) {
//            parent.setSettingsByObject.call(this, json);
//        };

        thisMCCSearchInput.addEvents = function() {
//            parent.addEvents.call(this);
            this.dropDownOrigin.change(this.updateCurrencyHandler);
            this.applyButton.click(this.applyCurrencyHandler);
        };

        thisMCCSearchInput.initializeCurrency = function() {
            this.updateCurrency();
        };

        thisMCCSearchInput.getCurrencyDescription = function(code) {
            var index = 0;
            for (index in this.currencyInfoArray) {
                if (this.currencyInfoArray[index].currencyCode === code) {
                    return this.currencyInfoArray[index].description;
                }
            }
        };
        
        thisMCCSearchInput.applyCurrencyHandler = function ()
        {
            thisMCCSearchInput.applyCurrency();
        };

        thisMCCSearchInput.applyCurrency = function() {
            if (this.hasUncommittedPayment === "true") {
                return confirm(this.resetPaymentsText);
            }
        };

        thisMCCSearchInput.updateCurrencyHandler = function ()
        {
            thisMCCSearchInput.updateCurrency();
        };
        
        thisMCCSearchInput.updateCurrency = function() {
            var origin = '', currencyIndex = 0;
            var originCountryCode = {};
            if (this.dropDownOrigin.length > 0) {
                origin = this.dropDownOrigin.val();
                originCountryCode = this.stationHash[origin];
            }
            else {
                origin = $('#MCCOriginCountry').val();
                originCountryCode.countryCode = origin;
            }

            if (origin !== '' && origin !== undefined) {
                var currencyArray = this.currencyArray, arrayIndex = 0;
                var myMCCArray = [];


                for (currencyIndex in currencyArray) {
                    if (currencyArray[currencyIndex].countryCode === originCountryCode.countryCode) {

                        var myMCCItem = {};
                        myMCCItem.name = this.getCurrencyDescription(currencyArray[currencyIndex].currencyCode);
                        myMCCItem.code = currencyArray[currencyIndex].currencyCode;
                        myMCCArray.push(myMCCItem);
                        if (this.MCCArray && this.MCCArray.length > 0) {
                            for (arrayIndex in this.MCCArray) {
                                if (this.MCCArray[arrayIndex].buy === currencyArray[currencyIndex].currencyCode) {
                                    myMCCItem = {};
                                    myMCCItem.name = this.getCurrencyDescription(this.MCCArray[arrayIndex].sell);
                                    myMCCItem.code = this.MCCArray[arrayIndex].sell;
                                    myMCCArray.push(myMCCItem);
                                }
                            }
                        }

                        selectParamObj = {
                            'selectBox': this.dropDownCurrency,
                            'objectArray': myMCCArray,
                            'showCode': true
                        };

                        SKYSALES.Util.populateSelect(selectParamObj);
                        if (this.selectedMCC !== "") {
                            this.dropDownCurrency.val(this.selectedMCC);
                        }
                        else {
                            this.dropDownCurrency.val(currencyArray[currencyIndex].currencyCode);
                        }
                        break;
                    }
                }

                // Empty the select input 
                if (myMCCArray.length === 0) {
                    selectParamObj = {
                        'selectBox': this.dropDownCurrency,
                        'objectArray': [],
                        'showCode': true
                    };
                    SKYSALES.Util.populateSelect(selectParamObj);
                }


            }
        };

        return thisMCCSearchInput;
    };
        SKYSALES.Class.MCCSearchInput.createObject = function(json) {
        SKYSALES.Util.createObject('mCCSearchInput', 'MCCSearchInput', json);
    };
}

/* End MCC NPS Implementation */

if (!SKYSALES.Class.AdvancedBookingListSearchInput) {
    SKYSALES.Class.AdvancedBookingListSearchInput = function() {
        var parent = new SKYSALES.Class.SkySales();
        var thisAdvancedBookingList = SKYSALES.Util.extendObject(parent);

        thisAdvancedBookingList.HyperLinkIdArray = {};
        thisAdvancedBookingList.ReportLinkHash = {};
        thisAdvancedBookingList.AttachPromptText = '';
        thisAdvancedBookingList.ReportPromptText = '';

        thisAdvancedBookingList.init = function(json) {
            this.setSettingsByObject(json);
            this.setVars();
            this.addEvents();
        };

        thisAdvancedBookingList.setVars = function() {
            parent.setVars.call(this);

        };

        thisAdvancedBookingList.addEvents = function() {
            var prop = "";

            for (prop in this.HyperLinkIdArray) {
                if (this.HyperLinkIdArray.hasOwnProperty(prop)) {
                    if (this.HyperLinkIdArray[prop].hasOwnProperty("attachId")) {
                        $('#' + this.HyperLinkIdArray[prop].attachId).click(this.ShowAttachPrompterHandler);
                    }
                    if (this.HyperLinkIdArray[prop].hasOwnProperty("reportId")) {
                        $('#' + this.HyperLinkIdArray[prop].reportId).click(this.ShowReportPrompterHandler);
                        this.ReportLinkHash[this.HyperLinkIdArray[prop].reportId] = this.HyperLinkIdArray[prop].reportLink;
                    }
                }
            }
        };

        thisAdvancedBookingList.ShowAttachPrompterHandler = function() {
            //modified by Linson at 2012-03-26
            return thisAdvancedBookingList.ShowPrompter(thisAdvancedBookingList.AttachPromptText)? SKYSALES.DisplayLoadingBar() :false;
        };

        thisAdvancedBookingList.ShowReportPrompterHandler = function() {
            return thisAdvancedBookingList.ShowPrompter(thisAdvancedBookingList.ReportPromptText, this.id);
        };

        thisAdvancedBookingList.ShowPrompter = function(text, id) {
            var reportLink = this.ReportLinkHash[id] || "";

            if (confirm(text)) {
                if (reportLink !== "") {
                    window.open(reportLink, "FeedbackURL", "height=600,width=720,status=yes,toolbar=no,menubar=no,location=no");
                }
                else {                     
                    return true;
                }                
            }
            SKYSALES.RemoveLoadingBar(); //added by Linson at 2012-01-11
            return false;
        };

        return thisAdvancedBookingList;
    };
    
    SKYSALES.Class.AdvancedBookingListSearchInput.createObject = function(json) {

        SKYSALES.Util.createObject('advancedBookingListSearchInput', 'AdvancedBookingListSearchInput', json);

    };
}

SKYSALES.Util.itineraryDisable = function(a) {
    $("div[@class^='target']").hide();
    $("table[@class^='target']").hide();

    if (a != 0) {

        if (document.getElementById('span' + a).hasAttribute('selected')) {
            document.getElementById('span' + a).removeAttribute('selected');
            document.getElementById('span' + a).removeAttribute('class');
            document.getElementById('span' + a).setAttribute('class', 'arrow-double-down arrow-btn');
            return;
        }

        $("div[@class='target" + a + "']").show();
        $("table[@class='target" + a + "']").show();

        for (var i = 1; i <= $("span[@id^='span']").length; i++) {
            if (document.getElementById('span' + i)) {
            
            document.getElementById('span' + i).removeAttribute('class');
            document.getElementById('span' + i).setAttribute('class', 'arrow-double-down arrow-btn');
        }
    }
        for (var j = 1; j <= $("span[@id^='spanINF']").length; j++) {
            if (document.getElementById('spanINF' + i)) {
            document.getElementById('spanINF' + j).removeAttribute('class');
            document.getElementById('spanINF' + j).setAttribute('class', 'arrow-double-down arrow-btn');
        }
    }

        document.getElementById('span' + a).removeAttribute('class');
        document.getElementById('span' + a).setAttribute('class', 'arrow-double-up arrow-btn');
        document.getElementById('span' + a).setAttribute('selected', 'true');

    }
};

SKYSALES.Util.itineraryDisableINF = function(a) {
    $("div[@class^='target']").hide();
    $("table[@class^='target']").hide();

    if (a != 0) {

        if (document.getElementById('spanINF' + a).hasAttribute('selected')) {
            document.getElementById('spanINF' + a).removeAttribute('selected');
            document.getElementById('spanINF' + a).removeAttribute('class');
            document.getElementById('spanINF' + a).setAttribute('class', 'arrow-double-down arrow-btn');
            return;
        }

        $("div[@class='targetINF" + a + "']").show();
        //        $("table[@class='targetINF" + a + "']").show();


        for (var i = 1; i <= $("span[@id^='span']").length; i++) {
            if (document.getElementById('span' + i)) {
                document.getElementById('span' + i).removeAttribute('class');
                document.getElementById('span' + i).setAttribute('class', 'arrow-double-down arrow-btn');
            }
        }
        for (var j = 1; j <= $("span[@id^='spanINF']").length; j++) {
            if (document.getElementById('spanINF' + i)) {
                document.getElementById('spanINF' + j).removeAttribute('class');
                document.getElementById('spanINF' + j).setAttribute('class', 'arrow-double-down arrow-btn');
            }
        }

        document.getElementById('spanINF' + a).removeAttribute('class');
        document.getElementById('spanINF' + a).setAttribute('class', 'arrow-double-up arrow-btn');
        document.getElementById('spanINF' + a).setAttribute('selected', 'true');

    }
};

SKYSALES.Util.searchToggle = function(a) {
if (a == 'up') {
    $("div[@class^='ucSearchForm-ButtonItem']").hide(400);
    $("#SearchToggle").hide(400);
    $("#ShowSearch").show();
    $("#HideSearch").hide();
}
else {
    $("div[@class^='ucSearchForm-ButtonItem']").show(400);
    $("#SearchToggle").show(400);
    $("#ShowSearch").hide();
    $("#HideSearch").show();
}
};
/* Start .js */

/* End .js */
