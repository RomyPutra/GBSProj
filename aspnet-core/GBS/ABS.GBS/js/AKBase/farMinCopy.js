if (!this.JSON) {
    JSON = function() {
        function f(n) {
            return n < 10 ? "0" + n : n
        }
        Date.prototype.toJSON = function() {
            return this.getUTCFullYear() + "-" + f(this.getUTCMonth() + 1) + "-" + f(this.getUTCDate()) + "T" + f(this.getUTCHours()) + ":" + f(this.getUTCMinutes()) + ":" + f(this.getUTCSeconds()) + "Z"
        };
        var m = {
            "\b": "\\b",
            "\t": "\\t",
            "\n": "\\n",
            "\f": "\\f",
            "\r": "\\r",
            '"': '\\"',
            "\\": "\\\\"
        };

        function stringify(value, whitelist) {
            var a, i, k, l, r = /["\\\x00-\x1f\x7f-\x9f]/g,
                v;
            switch (typeof value) {
                case "string":
                    return r.test(value) ? '"' + value.replace(r, function(a) {
                        var c = m[a];
                        if (c) {
                            return c
                        }
                        c = a.charCodeAt();
                        return "\\u00" + Math.floor(c / 16).toString(16) + (c % 16).toString(16)
                    }) + '"' : '"' + value + '"';
                case "number":
                    return isFinite(value) ? String(value) : "null";
                case "boolean":
                case "null":
                    return String(value);
                case "object":
                    if (!value) {
                        return "null"
                    }
                    if (typeof value.toJSON === "function") {
                        return stringify(value.toJSON())
                    }
                    a = [];
                    if (typeof value.length === "number" && !(value.propertyIsEnumerable("length"))) {
                        l = value.length;
                        for (i = 0; i < l; i += 1) {
                            a.push(stringify(value[i], whitelist) || "null")
                        }
                        return "[" + a.join(",") + "]"
                    }
                    if (whitelist) {
                        l = whitelist.length;
                        for (i = 0; i < l; i += 1) {
                            k = whitelist[i];
                            if (typeof k === "string") {
                                v = stringify(value[k], whitelist);
                                if (v) {
                                    a.push(stringify(k) + ":" + v)
                                }
                            }
                        }
                    } else {
                        for (k in value) {
                            if (typeof k === "string") {
                                v = stringify(value[k], whitelist);
                                if (v) {
                                    a.push(stringify(k) + ":" + v)
                                }
                            }
                        }
                    }
                    return "{" + a.join(",") + "}"
            }
        }
        return {
            stringify: stringify,
            parse: function(text, filter) {
                var j;

                function walk(k, v) {
                    var i, n;
                    if (v && typeof v === "object") {
                        for (i in v) {
                            if (Object.prototype.hasOwnProperty.apply(v, [i])) {
                                n = walk(i, v[i]);
                                if (n !== undefined) {
                                    v[i] = n
                                }
                            }
                        }
                    }
                    return filter(k, v)
                }
                if (/^[\],:{}\s]*$/.test(text.replace(/\\./g, "@").replace(/"[^"\\\n\r]*"|true|false|null|-?\d+(?:\.\d*)?(:?[eE][+\-]?\d+)?/g, "]").replace(/(?:^|:|,)(?:\s*\[)+/g, ""))) {
                    j = eval("(" + text + ")");
                    return typeof filter === "function" ? walk("", j) : j
                }
                throw new SyntaxError("parseJSON")
            }
        }
    }()
}(function() {
    if (typeof jQuery != "undefined") {
        var _jQuery = jQuery
    }
    var jQuery = window.jQuery = function(selector, context) {
        return this instanceof jQuery ? this.init(selector, context) : new jQuery(selector, context)
    };
    if (typeof $ != "undefined") {
        var _$ = $
    }
    window.$ = jQuery;
    var quickExpr = /^[^<]*(<(.|\s)+>)[^>]*$|^#(\w+)$/;
    jQuery.fn = jQuery.prototype = {
        init: function(selector, context) {
            selector = selector || document;
            if (typeof selector == "string") {
                var m = quickExpr.exec(selector);
                if (m && (m[1] || !context)) {
                    if (m[1]) {
                        selector = jQuery.clean([m[1]], context)
                    } else {
                        var tmp = document.getElementById(m[3]);
                        if (tmp) {
                            if (tmp.id != m[3]) {
                                return jQuery().find(selector)
                            } else {
                                this[0] = tmp;
                                this.length = 1;
                                return this
                            }
                        } else {
                            selector = []
                        }
                    }
                } else {
                    return new jQuery(context).find(selector)
                }
            } else {
                if (jQuery.isFunction(selector)) {
                    return new jQuery(document)[jQuery.fn.ready ? "ready" : "load"](selector)
                }
            }
            return this.setArray(selector.constructor == Array && selector || (selector.jquery || selector.length && selector != window && !selector.nodeType && selector[0] != undefined && selector[0].nodeType) && jQuery.makeArray(selector) || [selector])
        },
        jquery: "1.2.1",
        size: function() {
            return this.length
        },
        length: 0,
        get: function(num) {
            return num == undefined ? jQuery.makeArray(this) : this[num]
        },
        pushStack: function(a) {
            var ret = jQuery(a);
            ret.prevObject = this;
            return ret
        },
        setArray: function(a) {
            this.length = 0;
            Array.prototype.push.apply(this, a);
            return this
        },
        each: function(fn, args) {
            return jQuery.each(this, fn, args)
        },
        index: function(obj) {
            var pos = -1;
            this.each(function(i) {
                if (this == obj) {
                    pos = i
                }
            });
            return pos
        },
        attr: function(key, value, type) {
            var obj = key;
            if (key.constructor == String) {
                if (value == undefined) {
                    return this.length && jQuery[type || "attr"](this[0], key) || undefined
                } else {
                    obj = {};
                    obj[key] = value
                }
            }
            return this.each(function(index) {
                for (var prop in obj) {
                    jQuery.attr(type ? this.style : this, prop, jQuery.prop(this, obj[prop], type, index, prop))
                }
            })
        },
        css: function(key, value) {
            return this.attr(key, value, "curCSS")
        },
        text: function(e) {
            if (typeof e != "object" && e != null) {
                return this.empty().append(document.createTextNode(e))
            }
            var t = "";
            jQuery.each(e || this, function() {
                jQuery.each(this.childNodes, function() {
                    if (this.nodeType != 8) {
                        t += this.nodeType != 1 ? this.nodeValue : jQuery.fn.text([this])
                    }
                })
            });
            return t
        },
        wrapAll: function(html) {
            if (this[0]) {
                jQuery(html, this[0].ownerDocument).clone().insertBefore(this[0]).map(function() {
                    var elem = this;
                    while (elem.firstChild) {
                        elem = elem.firstChild
                    }
                    return elem
                }).append(this)
            }
            return this
        },
        wrapInner: function(html) {
            return this.each(function() {
                jQuery(this).contents().wrapAll(html)
            })
        },
        wrap: function(html) {
            return this.each(function() {
                jQuery(this).wrapAll(html)
            })
        },
        append: function() {
            return this.domManip(arguments, true, 1, function(a) {
                this.appendChild(a)
            })
        },
        prepend: function() {
            return this.domManip(arguments, true, -1, function(a) {
                this.insertBefore(a, this.firstChild)
            })
        },
        before: function() {
            return this.domManip(arguments, false, 1, function(a) {
                this.parentNode.insertBefore(a, this)
            })
        },
        after: function() {
            return this.domManip(arguments, false, -1, function(a) {
                this.parentNode.insertBefore(a, this.nextSibling)
            })
        },
        end: function() {
            return this.prevObject || jQuery([])
        },
        find: function(t) {
            var data = jQuery.map(this, function(a) {
                return jQuery.find(t, a)
            });
            return this.pushStack(/[^+>] [^+>]/.test(t) || t.indexOf("..") > -1 ? jQuery.unique(data) : data)
        },
        clone: function(events) {
            var ret = this.map(function() {
                return this.outerHTML ? jQuery(this.outerHTML)[0] : this.cloneNode(true)
            });
            var clone = ret.find("*").andSelf().each(function() {
                if (this[expando] != undefined) {
                    this[expando] = null
                }
            });
            if (events === true) {
                this.find("*").andSelf().each(function(i) {
                    var events = jQuery.data(this, "events");
                    for (var type in events) {
                        for (var handler in events[type]) {
                            jQuery.event.add(clone[i], type, events[type][handler], events[type][handler].data)
                        }
                    }
                })
            }
            return ret
        },
        filter: function(t) {
            return this.pushStack(jQuery.isFunction(t) && jQuery.grep(this, function(el, index) {
                return t.apply(el, [index])
            }) || jQuery.multiFilter(t, this))
        },
        not: function(t) {
            return this.pushStack(t.constructor == String && jQuery.multiFilter(t, this, true) || jQuery.grep(this, function(a) {
                return (t.constructor == Array || t.jquery) ? jQuery.inArray(a, t) < 0 : a != t
            }))
        },
        add: function(t) {
            return this.pushStack(jQuery.merge(this.get(), t.constructor == String ? jQuery(t).get() : t.length != undefined && (!t.nodeName || jQuery.nodeName(t, "form")) ? t : [t]))
        },
        is: function(expr) {
            return expr ? jQuery.multiFilter(expr, this).length > 0 : false
        },
        hasClass: function(expr) {
            return this.is("." + expr)
        },
        val: function(val) {
            if (val == undefined) {
                if (this.length) {
                    var elem = this[0];
                    if (jQuery.nodeName(elem, "select")) {
                        var index = elem.selectedIndex,
                            a = [],
                            options = elem.options,
                            one = elem.type == "select-one";
                        if (index < 0) {
                            return null
                        }
                        for (var i = one ? index : 0, max = one ? index + 1 : options.length; i < max; i++) {
                            var option = options[i];
                            if (option.selected) {
                                var val = jQuery.browser.msie && !option.attributes.value.specified ? option.text : option.value;
                                if (one) {
                                    return val
                                }
                                a.push(val)
                            }
                        }
                        return a
                    } else {
                        return this[0].value.replace(/\r/g, "")
                    }
                }
            } else {
                return this.each(function() {
                    if (val.constructor == Array && /radio|checkbox/.test(this.type)) {
                        this.checked = (jQuery.inArray(this.value, val) >= 0 || jQuery.inArray(this.name, val) >= 0)
                    } else {
                        if (jQuery.nodeName(this, "select")) {
                            var tmp = val.constructor == Array ? val : [val];
                            jQuery("option", this).each(function() {
                                this.selected = (jQuery.inArray(this.value, tmp) >= 0 || jQuery.inArray(this.text, tmp) >= 0)
                            });
                            if (!tmp.length) {
                                this.selectedIndex = -1
                            }
                        } else {
                            this.value = val
                        }
                    }
                })
            }
        },
        html: function(val) {
            return val == undefined ? (this.length ? this[0].innerHTML : null) : this.empty().append(val)
        },
        replaceWith: function(val) {
            return this.after(val).remove()
        },
        eq: function(i) {
            return this.slice(i, i + 1)
        },
        slice: function() {
            return this.pushStack(Array.prototype.slice.apply(this, arguments))
        },
        map: function(fn) {
            return this.pushStack(jQuery.map(this, function(elem, i) {
                return fn.call(elem, i, elem)
            }))
        },
        andSelf: function() {
            return this.add(this.prevObject)
        },
        domManip: function(args, table, dir, fn) {
            var clone = this.length > 1,
                a;
            return this.each(function() {
                if (!a) {
                    a = jQuery.clean(args, this.ownerDocument);
                    if (dir < 0) {
                        a.reverse()
                    }
                }
                var obj = this;
                if (table && jQuery.nodeName(this, "table") && jQuery.nodeName(a[0], "tr")) {
                    obj = this.getElementsByTagName("tbody")[0] || this.appendChild(document.createElement("tbody"))
                }
                jQuery.each(a, function() {
                    var elem = clone ? this.cloneNode(true) : this;
                    if (!evalScript(0, elem)) {
                        fn.call(obj, elem)
                    }
                })
            })
        }
    };

    function evalScript(i, elem) {
        var script = jQuery.nodeName(elem, "script");
        if (script) {
            if (elem.src) {
                jQuery.ajax({
                    url: elem.src,
                    async: false,
                    dataType: "script"
                })
            } else {
                jQuery.globalEval(elem.text || elem.textContent || elem.innerHTML || "")
            }
            if (elem.parentNode) {
                elem.parentNode.removeChild(elem)
            }
        } else {
            if (elem.nodeType == 1) {
                jQuery("script", elem).each(evalScript)
            }
        }
        return script
    }
    jQuery.extend = jQuery.fn.extend = function() {
        var target = arguments[0] || {},
            a = 1,
            al = arguments.length,
            deep = false;
        if (target.constructor == Boolean) {
            deep = target;
            target = arguments[1] || {}
        }
        if (al == 1) {
            target = this;
            a = 0
        }
        var prop;
        for (; a < al; a++) {
            if ((prop = arguments[a]) != null) {
                for (var i in prop) {
                    if (target == prop[i]) {
                        continue
                    }
                    if (deep && typeof prop[i] == "object" && target[i]) {
                        jQuery.extend(target[i], prop[i])
                    } else {
                        if (prop[i] != undefined) {
                            target[i] = prop[i]
                        }
                    }
                }
            }
        }
        return target
    };
    var expando = "jQuery" + (new Date()).getTime(),
        uuid = 0,
        win = {};
    jQuery.extend({
        noConflict: function(deep) {
            window.$ = _$;
            if (deep) {
                window.jQuery = _jQuery
            }
            return jQuery
        },
        isFunction: function(fn) {
            return !!fn && typeof fn != "string" && !fn.nodeName && fn.constructor != Array && /function/i.test(fn + "")
        },
        isXMLDoc: function(elem) {
            return elem.documentElement && !elem.body || elem.tagName && elem.ownerDocument && !elem.ownerDocument.body
        },
        globalEval: function(data) {
            data = jQuery.trim(data);
            if (data) {
                if (window.execScript) {
                    window.execScript(data)
                } else {
                    if (jQuery.browser.safari) {
                        window.setTimeout(data, 0)
                    } else {
                        eval.call(window, data)
                    }
                }
            }
        },
        nodeName: function(elem, name) {
            return elem.nodeName && elem.nodeName.toUpperCase() == name.toUpperCase()
        },
        cache: {},
        data: function(elem, name, data) {
            elem = elem == window ? win : elem;
            var id = elem[expando];
            if (!id) {
                id = elem[expando] = ++uuid
            }
            if (name && !jQuery.cache[id]) {
                jQuery.cache[id] = {}
            }
            if (data != undefined) {
                jQuery.cache[id][name] = data
            }
            return name ? jQuery.cache[id][name] : id
        },
        removeData: function(elem, name) {
            elem = elem == window ? win : elem;
            var id = elem[expando];
            if (name) {
                if (jQuery.cache[id]) {
                    delete jQuery.cache[id][name];
                    name = "";
                    for (name in jQuery.cache[id]) {
                        break
                    }
                    if (!name) {
                        jQuery.removeData(elem)
                    }
                }
            } else {
                try {
                    delete elem[expando]
                } catch (e) {
                    if (elem.removeAttribute) {
                        elem.removeAttribute(expando)
                    }
                }
                delete jQuery.cache[id]
            }
        },
        each: function(obj, fn, args) {
            if (args) {
                if (obj.length == undefined) {
                    for (var i in obj) {
                        fn.apply(obj[i], args)
                    }
                } else {
                    for (var i = 0, ol = obj.length; i < ol; i++) {
                        if (fn.apply(obj[i], args) === false) {
                            break
                        }
                    }
                }
            } else {
                if (obj.length == undefined) {
                    for (var i in obj) {
                        fn.call(obj[i], i, obj[i])
                    }
                } else {
                    for (var i = 0, ol = obj.length, val = obj[0]; i < ol && fn.call(val, i, val) !== false; val = obj[++i]) {}
                }
            }
            return obj
        },
        prop: function(elem, value, type, index, prop) {
            if (jQuery.isFunction(value)) {
                value = value.call(elem, [index])
            }
            var exclude = /z-?index|font-?weight|opacity|zoom|line-?height/i;
            return value && value.constructor == Number && type == "curCSS" && !exclude.test(prop) ? value + "px" : value
        },
        className: {
            add: function(elem, c) {
                jQuery.each((c || "").split(/\s+/), function(i, cur) {
                    if (!jQuery.className.has(elem.className, cur)) {
                        elem.className += (elem.className ? " " : "") + cur
                    }
                })
            },
            remove: function(elem, c) {
                elem.className = c != undefined ? jQuery.grep(elem.className.split(/\s+/), function(cur) {
                    return !jQuery.className.has(c, cur)
                }).join(" ") : ""
            },
            has: function(t, c) {
                return jQuery.inArray(c, (t.className || t).toString().split(/\s+/)) > -1
            }
        },
        swap: function(e, o, f) {
            for (var i in o) {
                e.style["old" + i] = e.style[i];
                e.style[i] = o[i]
            }
            f.apply(e, []);
            for (var i in o) {
                e.style[i] = e.style["old" + i]
            }
        },
        css: function(e, p) {
            if (p == "height" || p == "width") {
                var old = {},
                    oHeight, oWidth, d = ["Top", "Bottom", "Right", "Left"];
                jQuery.each(d, function() {
                    old["padding" + this] = 0;
                    old["border" + this + "Width"] = 0
                });
                jQuery.swap(e, old, function() {
                    if (jQuery(e).is(":visible")) {
                        oHeight = e.offsetHeight;
                        oWidth = e.offsetWidth
                    } else {
                        e = jQuery(e.cloneNode(true)).find(":radio").removeAttr("checked").end().css({
                            visibility: "hidden",
                            position: "absolute",
                            display: "block",
                            right: "0",
                            left: "0"
                        }).appendTo(e.parentNode)[0];
                        var parPos = jQuery.css(e.parentNode, "position") || "static";
                        if (parPos == "static") {
                            e.parentNode.style.position = "relative"
                        }
                        oHeight = e.clientHeight;
                        oWidth = e.clientWidth;
                        if (parPos == "static") {
                            e.parentNode.style.position = "static"
                        }
                        e.parentNode.removeChild(e)
                    }
                });
                return p == "height" ? oHeight : oWidth
            }
            return jQuery.curCSS(e, p)
        },
        curCSS: function(elem, prop, force) {
            var ret, stack = [],
                swap = [];

            function color(a) {
                if (!jQuery.browser.safari) {
                    return false
                }
                var ret = document.defaultView.getComputedStyle(a, null);
                return !ret || ret.getPropertyValue("color") == ""
            }
            if (prop == "opacity" && jQuery.browser.msie) {
                ret = jQuery.attr(elem.style, "opacity");
                return ret == "" ? "1" : ret
            }
            if (prop.match(/float/i)) {
                prop = styleFloat
            }
            if (!force && elem.style[prop]) {
                ret = elem.style[prop]
            } else {
                if (document.defaultView && document.defaultView.getComputedStyle) {
                    if (prop.match(/float/i)) {
                        prop = "float"
                    }
                    prop = prop.replace(/([A-Z])/g, "-$1").toLowerCase();
                    var cur = document.defaultView.getComputedStyle(elem, null);
                    if (cur && !color(elem)) {
                        ret = cur.getPropertyValue(prop)
                    } else {
                        for (var a = elem; a && color(a); a = a.parentNode) {
                            stack.unshift(a)
                        }
                        for (a = 0; a < stack.length; a++) {
                            if (color(stack[a])) {
                                swap[a] = stack[a].style.display;
                                stack[a].style.display = "block"
                            }
                        }
                        ret = prop == "display" && swap[stack.length - 1] != null ? "none" : document.defaultView.getComputedStyle(elem, null).getPropertyValue(prop) || "";
                        for (a = 0; a < swap.length; a++) {
                            if (swap[a] != null) {
                                stack[a].style.display = swap[a]
                            }
                        }
                    }
                    if (prop == "opacity" && ret == "") {
                        ret = "1"
                    }
                } else {
                    if (elem.currentStyle) {
                        var newProp = prop.replace(/\-(\w)/g, function(m, c) {
                            return c.toUpperCase()
                        });
                        ret = elem.currentStyle[prop] || elem.currentStyle[newProp];
                        if (!/^\d+(px)?$/i.test(ret) && /^\d/.test(ret)) {
                            var style = elem.style.left;
                            var runtimeStyle = elem.runtimeStyle.left;
                            elem.runtimeStyle.left = elem.currentStyle.left;
                            elem.style.left = ret || 0;
                            ret = elem.style.pixelLeft + "px";
                            elem.style.left = style;
                            elem.runtimeStyle.left = runtimeStyle
                        }
                    }
                }
            }
            return ret
        },
        clean: function(a, doc) {
            var r = [];
            doc = doc || document;
            jQuery.each(a, function(i, arg) {
                if (!arg) {
                    return
                }
                if (arg.constructor == Number) {
                    arg = arg.toString()
                }
                if (typeof arg == "string") {
                    arg = arg.replace(/(<(\w+)[^>]*?)\/>/g, function(m, all, tag) {
                        return tag.match(/^(abbr|br|col|img|input|link|meta|param|hr|area)$/i) ? m : all + "></" + tag + ">"
                    });
                    var s = jQuery.trim(arg).toLowerCase(),
                        div = doc.createElement("div"),
                        tb = [];
                    var wrap = !s.indexOf("<opt") && [1, "<select>", "</select>"] || !s.indexOf("<leg") && [1, "<fieldset>", "</fieldset>"] || s.match(/^<(thead|tbody|tfoot|colg|cap)/) && [1, "<table>", "</table>"] || !s.indexOf("<tr") && [2, "<table><tbody>", "</tbody></table>"] || (!s.indexOf("<td") || !s.indexOf("<th")) && [3, "<table><tbody><tr>", "</tr></tbody></table>"] || !s.indexOf("<col") && [2, "<table><tbody></tbody><colgroup>", "</colgroup></table>"] || jQuery.browser.msie && [1, "div<div>", "</div>"] || [0, "", ""];
                    div.innerHTML = wrap[1] + arg + wrap[2];
                    while (wrap[0]--) {
                        div = div.lastChild
                    }
                    if (jQuery.browser.msie) {
                        if (!s.indexOf("<table") && s.indexOf("<tbody") < 0) {
                            tb = div.firstChild && div.firstChild.childNodes
                        } else {
                            if (wrap[1] == "<table>" && s.indexOf("<tbody") < 0) {
                                tb = div.childNodes
                            }
                        }
                        for (var n = tb.length - 1; n >= 0; --n) {
                            if (jQuery.nodeName(tb[n], "tbody") && !tb[n].childNodes.length) {
                                tb[n].parentNode.removeChild(tb[n])
                            }
                        }
                        if (/^\s/.test(arg)) {
                            div.insertBefore(doc.createTextNode(arg.match(/^\s*/)[0]), div.firstChild)
                        }
                    }
                    arg = jQuery.makeArray(div.childNodes)
                }
                if (0 === arg.length && (!jQuery.nodeName(arg, "form") && !jQuery.nodeName(arg, "select"))) {
                    return
                }
                if (arg[0] == undefined || jQuery.nodeName(arg, "form") || arg.options) {
                    r.push(arg)
                } else {
                    r = jQuery.merge(r, arg)
                }
            });
            return r
        },
        attr: function(elem, name, value) {
            var fix = jQuery.isXMLDoc(elem) ? {} : jQuery.props;
            if (name == "selected" && jQuery.browser.safari) {
                elem.parentNode.selectedIndex
            }
            if (fix[name]) {
                if (value != undefined) {
                    elem[fix[name]] = value
                }
                return elem[fix[name]]
            } else {
                if (jQuery.browser.msie && name == "style") {
                    return jQuery.attr(elem.style, "cssText", value)
                } else {
                    if (value == undefined && jQuery.browser.msie && jQuery.nodeName(elem, "form") && (name == "action" || name == "method")) {
                        return elem.getAttributeNode(name).nodeValue
                    } else {
                        if (elem.tagName) {
                            if (value != undefined) {
                                if (name == "type" && jQuery.nodeName(elem, "input") && elem.parentNode) {
                                    throw "type property can't be changed"
                                }
                                elem.setAttribute(name, value)
                            }
                            if (jQuery.browser.msie && /href|src/.test(name) && !jQuery.isXMLDoc(elem)) {
                                return elem.getAttribute(name, 2)
                            }
                            return elem.getAttribute(name)
                        } else {
                            if (name == "opacity" && jQuery.browser.msie) {
                                if (value != undefined) {
                                    elem.zoom = 1;
                                    elem.filter = (elem.filter || "").replace(/alpha\([^)]*\)/, "") + (parseFloat(value).toString() == "NaN" ? "" : "alpha(opacity=" + value * 100 + ")")
                                }
                                return elem.filter ? (parseFloat(elem.filter.match(/opacity=([^)]*)/)[1]) / 100).toString() : ""
                            }
                            name = name.replace(/-([a-z])/ig, function(z, b) {
                                return b.toUpperCase()
                            });
                            if (value != undefined) {
                                elem[name] = value
                            }
                            return elem[name]
                        }
                    }
                }
            }
        },
        trim: function(t) {
            return (t || "").replace(/^\s+|\s+$/g, "")
        },
        makeArray: function(a) {
            var r = [];
            if (typeof a != "array") {
                for (var i = 0, al = a.length; i < al; i++) {
                    r.push(a[i])
                }
            } else {
                r = a.slice(0)
            }
            return r
        },
        inArray: function(b, a) {
            for (var i = 0, al = a.length; i < al; i++) {
                if (a[i] == b) {
                    return i
                }
            }
            return -1
        },
        merge: function(first, second) {
            if (jQuery.browser.msie) {
                for (var i = 0; second[i]; i++) {
                    if (second[i].nodeType != 8) {
                        first.push(second[i])
                    }
                }
            } else {
                for (var i = 0; second[i]; i++) {
                    first.push(second[i])
                }
            }
            return first
        },
        unique: function(first) {
            var r = [],
                done = {};
            try {
                for (var i = 0, fl = first.length; i < fl; i++) {
                    var id = jQuery.data(first[i]);
                    if (!done[id]) {
                        done[id] = true;
                        r.push(first[i])
                    }
                }
            } catch (e) {
                r = first
            }
            return r
        },
        grep: function(elems, fn, inv) {
            if (typeof fn == "string") {
                fn = eval("false||function(a,i){return " + fn + "}")
            }
            var result = [];
            for (var i = 0, el = elems.length; i < el; i++) {
                if (!inv && fn(elems[i], i) || inv && !fn(elems[i], i)) {
                    result.push(elems[i])
                }
            }
            return result
        },
        map: function(elems, fn) {
            if (typeof fn == "string") {
                fn = eval("false||function(a){return " + fn + "}")
            }
            var result = [];
            for (var i = 0, el = elems.length; i < el; i++) {
                var val = fn(elems[i], i);
                if (val !== null && val != undefined) {
                    if (val.constructor != Array) {
                        val = [val]
                    }
                    result = result.concat(val)
                }
            }
            return result
        }
    });
    var userAgent = navigator.userAgent.toLowerCase();
    jQuery.browser = {
        version: (userAgent.match(/.+(?:rv|it|ra|ie)[\/: ]([\d.]+)/) || [])[1],
        safari: /webkit/.test(userAgent),
        opera: /opera/.test(userAgent),
        msie: /msie/.test(userAgent) && !/opera/.test(userAgent),
        mozilla: /mozilla/.test(userAgent) && !/(compatible|webkit)/.test(userAgent)
    };
    var styleFloat = jQuery.browser.msie ? "styleFloat" : "cssFloat";
    jQuery.extend({
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
            if (a && typeof a == "string") {
                ret = jQuery.multiFilter(a, ret)
            }
            return this.pushStack(jQuery.unique(ret))
        }
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
                for (var j = 0, al = a.length; j < al; j++) {
                    jQuery(a[j])[n](this)
                }
            })
        }
    });
    jQuery.each({
        removeAttr: function(key) {
            jQuery.attr(this, key, "");
            this.removeAttribute(key)
        },
        addClass: function(c) {
            jQuery.className.add(this, c)
        },
        removeClass: function(c) {
            jQuery.className.remove(this, c)
        },
        toggleClass: function(c) {
            jQuery.className[jQuery.className.has(this, c) ? "remove" : "add"](this, c)
        },
        remove: function(a) {
            if (!a || jQuery.filter(a, [this]).r.length) {
                jQuery.removeData(this);
                this.parentNode.removeChild(this)
            }
        },
        empty: function() {
            jQuery("*", this).each(function() {
                jQuery.removeData(this)
            });
            while (this.firstChild) {
                this.removeChild(this.firstChild)
            }
        }
    }, function(i, n) {
        jQuery.fn[i] = function() {
            return this.each(n, arguments)
        }
    });
    jQuery.each(["Height", "Width"], function(i, name) {
        var n = name.toLowerCase();
        jQuery.fn[n] = function(h) {
            return this[0] == window ? jQuery.browser.safari && self["inner" + name] || jQuery.boxModel && Math.max(document.documentElement["client" + name], document.body["client" + name]) || document.body["client" + name] : this[0] == document ? Math.max(document.body["scroll" + name], document.body["offset" + name]) : h == undefined ? (this.length ? jQuery.css(this[0], n) : null) : this.css(n, h.constructor == String ? h : h + "px")
        }
    });
    var chars = jQuery.browser.safari && parseInt(jQuery.browser.version) < 417 ? "(?:[\\w*_-]|\\\\.)" : "(?:[\\w\u0128-\uFFFF*_-]|\\\\.)",
        quickChild = new RegExp("^>\\s*(" + chars + "+)"),
        quickID = new RegExp("^(" + chars + "+)(#)(" + chars + "+)"),
        quickClass = new RegExp("^([#.]?)(" + chars + "*)");
    jQuery.extend({
        expr: {
            "": "m[2]=='*'||jQuery.nodeName(a,m[2])",
            "#": "a.getAttribute('id')==m[2]",
            ":": {
                lt: "i<m[3]-0",
                gt: "i>m[3]-0",
                nth: "m[3]-0==i",
                eq: "m[3]-0==i",
                first: "i==0",
                last: "i==r.length-1",
                even: "i%2==0",
                odd: "i%2",
                "first-child": "a.parentNode.getElementsByTagName('*')[0]==a",
                "last-child": "jQuery.nth(a.parentNode.lastChild,1,'previousSibling')==a",
                "only-child": "!jQuery.nth(a.parentNode.lastChild,2,'previousSibling')",
                parent: "a.firstChild",
                empty: "!a.firstChild",
                contains: "(a.textContent||a.innerText||jQuery(a).text()||'').indexOf(m[3])>=0",
                visible: '"hidden"!=a.type&&jQuery.css(a,"display")!="none"&&jQuery.css(a,"visibility")!="hidden"',
                hidden: '"hidden"==a.type||jQuery.css(a,"display")=="none"||jQuery.css(a,"visibility")=="hidden"',
                enabled: "!a.disabled",
                disabled: "a.disabled",
                checked: "a.checked",
                selected: "a.selected||jQuery.attr(a,'selected')",
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
                has: "jQuery.find(m[3],a).length",
                header: "/h\\d/i.test(a.nodeName)",
                animated: "jQuery.grep(jQuery.timers,function(fn){return a==fn.elem;}).length"
            }
        },
        parse: [/^(\[) *@?([\w-]+) *([!*$^~=]*) *('?"?)(.*?)\4 *\]/, /^(:)([\w-]+)\("?'?(.*?(\(.*?\))?[^(]*?)"?'?\)/, new RegExp("^([:.#]*)(" + chars + "+)")],
        multiFilter: function(expr, elems, not) {
            var old, cur = [];
            while (expr && expr != old) {
                old = expr;
                var f = jQuery.filter(expr, elems, not);
                expr = f.t.replace(/^\s*,\s*/, "");
                cur = not ? elems = f.r : jQuery.merge(cur, f.r)
            }
            return cur
        },
        find: function(t, context) {
            if (typeof t != "string") {
                return [t]
            }
            if (context && !context.nodeType) {
                context = null
            }
            context = context || document;
            var ret = [context],
                done = [],
                last;
            while (t && last != t) {
                var r = [];
                last = t;
                t = jQuery.trim(t);
                var foundToken = false;
                var re = quickChild;
                var m = re.exec(t);
                if (m) {
                    var nodeName = m[1].toUpperCase();
                    for (var i = 0; ret[i]; i++) {
                        for (var c = ret[i].firstChild; c; c = c.nextSibling) {
                            if (c.nodeType == 1 && (nodeName == "*" || c.nodeName.toUpperCase() == nodeName.toUpperCase())) {
                                r.push(c)
                            }
                        }
                    }
                    ret = r;
                    t = t.replace(re, "");
                    if (t.indexOf(" ") == 0) {
                        continue
                    }
                    foundToken = true
                } else {
                    re = /^([>+~])\s*(\w*)/i;
                    if ((m = re.exec(t)) != null) {
                        r = [];
                        var nodeName = m[2],
                            merge = {};
                        m = m[1];
                        for (var j = 0, rl = ret.length; j < rl; j++) {
                            var n = m == "~" || m == "+" ? ret[j].nextSibling : ret[j].firstChild;
                            for (; n; n = n.nextSibling) {
                                if (n.nodeType == 1) {
                                    var id = jQuery.data(n);
                                    if (m == "~" && merge[id]) {
                                        break
                                    }
                                    if (!nodeName || n.nodeName.toUpperCase() == nodeName.toUpperCase()) {
                                        if (m == "~") {
                                            merge[id] = true
                                        }
                                        r.push(n)
                                    }
                                    if (m == "+") {
                                        break
                                    }
                                }
                            }
                        }
                        ret = r;
                        t = jQuery.trim(t.replace(re, ""));
                        foundToken = true
                    }
                }
                if (t && !foundToken) {
                    if (!t.indexOf(",")) {
                        if (context == ret[0]) {
                            ret.shift()
                        }
                        done = jQuery.merge(done, ret);
                        r = ret = [context];
                        t = " " + t.substr(1, t.length)
                    } else {
                        var re2 = quickID;
                        var m = re2.exec(t);
                        if (m) {
                            m = [0, m[2], m[3], m[1]]
                        } else {
                            re2 = quickClass;
                            m = re2.exec(t)
                        }
                        m[2] = m[2].replace(/\\/g, "");
                        var elem = ret[ret.length - 1];
                        if (m[1] == "#" && elem && elem.getElementById && !jQuery.isXMLDoc(elem)) {
                            var oid = elem.getElementById(m[2]);
                            if ((jQuery.browser.msie || jQuery.browser.opera) && oid && typeof oid.id == "string" && oid.id != m[2]) {
                                oid = jQuery('[@id="' + m[2] + '"]', elem)[0]
                            }
                            ret = r = oid && (!m[3] || jQuery.nodeName(oid, m[3])) ? [oid] : []
                        } else {
                            for (var i = 0; ret[i]; i++) {
                                var tag = m[1] == "#" && m[3] ? m[3] : m[1] != "" || m[0] == "" ? "*" : m[2];
                                if (tag == "*" && ret[i].nodeName.toLowerCase() == "object") {
                                    tag = "param"
                                }
                                r = jQuery.merge(r, ret[i].getElementsByTagName(tag))
                            }
                            if (m[1] == ".") {
                                r = jQuery.classFilter(r, m[2])
                            }
                            if (m[1] == "#") {
                                var tmp = [];
                                for (var i = 0; r[i]; i++) {
                                    if (r[i].getAttribute("id") == m[2]) {
                                        tmp = [r[i]];
                                        break
                                    }
                                }
                                r = tmp
                            }
                            ret = r
                        }
                        t = t.replace(re2, "")
                    }
                }
                if (t) {
                    var val = jQuery.filter(t, r);
                    ret = r = val.r;
                    t = jQuery.trim(val.t)
                }
            }
            if (t) {
                ret = []
            }
            if (ret && context == ret[0]) {
                ret.shift()
            }
            done = jQuery.merge(done, ret);
            return done
        },
        classFilter: function(r, m, not) {
            m = " " + m + " ";
            var tmp = [];
            for (var i = 0; r[i]; i++) {
                var pass = (" " + r[i].className + " ").indexOf(m) >= 0;
                if (!not && pass || not && !pass) {
                    tmp.push(r[i])
                }
            }
            return tmp
        },
        filter: function(t, r, not) {
            var last;
            while (t && t != last) {
                last = t;
                var p = jQuery.parse,
                    m;
                for (var i = 0; p[i]; i++) {
                    m = p[i].exec(t);
                    if (m) {
                        t = t.substring(m[0].length);
                        m[2] = m[2].replace(/\\/g, "");
                        break
                    }
                }
                if (!m) {
                    break
                }
                if (m[1] == ":" && m[2] == "not") {
                    r = jQuery.filter(m[3], r, true).r
                } else {
                    if (m[1] == ".") {
                        r = jQuery.classFilter(r, m[2], not)
                    } else {
                        if (m[1] == "[") {
                            var tmp = [],
                                type = m[3];
                            for (var i = 0, rl = r.length; i < rl; i++) {
                                var a = r[i],
                                    z = a[jQuery.props[m[2]] || m[2]];
                                if (z == null || /href|src|selected/.test(m[2])) {
                                    z = jQuery.attr(a, m[2]) || ""
                                }
                                if ((type == "" && !!z || type == "=" && z == m[5] || type == "!=" && z != m[5] || type == "^=" && z && !z.indexOf(m[5]) || type == "$=" && z.substr(z.length - m[5].length) == m[5] || (type == "*=" || type == "~=") && z.indexOf(m[5]) >= 0) ^ not) {
                                    tmp.push(a)
                                }
                            }
                            r = tmp
                        } else {
                            if (m[1] == ":" && m[2] == "nth-child") {
                                var merge = {},
                                    tmp = [],
                                    test = /(\d*)n\+?(\d*)/.exec(m[3] == "even" && "2n" || m[3] == "odd" && "2n+1" || !/\D/.test(m[3]) && "n+" + m[3] || m[3]),
                                    first = (test[1] || 1) - 0,
                                    last = test[2] - 0;
                                for (var i = 0, rl = r.length; i < rl; i++) {
                                    var node = r[i],
                                        parentNode = node.parentNode,
                                        id = jQuery.data(parentNode);
                                    if (!merge[id]) {
                                        var c = 1;
                                        for (var n = parentNode.firstChild; n; n = n.nextSibling) {
                                            if (n.nodeType == 1) {
                                                n.nodeIndex = c++
                                            }
                                        }
                                        merge[id] = true
                                    }
                                    var add = false;
                                    if (first == 1) {
                                        if (last == 0 || node.nodeIndex == last) {
                                            add = true
                                        }
                                    } else {
                                        if ((node.nodeIndex + last) % first == 0) {
                                            add = true
                                        }
                                    }
                                    if (add ^ not) {
                                        tmp.push(node)
                                    }
                                }
                                r = tmp
                            } else {
                                var f = jQuery.expr[m[1]];
                                if (typeof f != "string") {
                                    f = jQuery.expr[m[1]][m[2]]
                                }
                                f = eval("false||function(a,i){return " + f + "}");
                                r = jQuery.grep(r, f, not)
                            }
                        }
                    }
                }
            }
            return {
                r: r,
                t: t
            }
        },
        dir: function(elem, dir) {
            var matched = [];
            var cur = elem[dir];
            while (cur && cur != document) {
                if (cur.nodeType == 1) {
                    matched.push(cur)
                }
                cur = cur[dir]
            }
            return matched
        },
        nth: function(cur, result, dir, elem) {
            result = result || 1;
            var num = 0;
            for (; cur; cur = cur[dir]) {
                if (cur.nodeType == 1 && ++num == result) {
                    break
                }
            }
            return cur
        },
        sibling: function(n, elem) {
            var r = [];
            for (; n; n = n.nextSibling) {
                if (n.nodeType == 1 && (!elem || n != elem)) {
                    r.push(n)
                }
            }
            return r
        }
    });
    jQuery.event = {
        add: function(element, type, handler, data) {
            if (jQuery.browser.msie && element.setInterval != undefined) {
                element = window
            }
            if (!handler.guid) {
                handler.guid = this.guid++
            }
            if (data != undefined) {
                var fn = handler;
                handler = function() {
                    return fn.apply(this, arguments)
                };
                handler.data = data;
                handler.guid = fn.guid
            }
            var parts = type.split(".");
            type = parts[0];
            handler.type = parts[1];
            var events = jQuery.data(element, "events") || jQuery.data(element, "events", {});
            var handle = jQuery.data(element, "handle", function() {
                var val;
                if (typeof jQuery == "undefined" || jQuery.event.triggered) {
                    return val
                }
                val = jQuery.event.handle.apply(element, arguments);
                return val
            });
            var handlers = events[type];
            if (!handlers) {
                handlers = events[type] = {};
                if (element.addEventListener) {
                    element.addEventListener(type, handle, false)
                } else {
                    element.attachEvent("on" + type, handle)
                }
            }
            handlers[handler.guid] = handler;
            this.global[type] = true
        },
        guid: 1,
        global: {},
        remove: function(element, type, handler) {
            var events = jQuery.data(element, "events"),
                ret, index;
            if (typeof type == "string") {
                var parts = type.split(".");
                type = parts[0]
            }
            if (events) {
                if (type && type.type) {
                    handler = type.handler;
                    type = type.type
                }
                if (!type) {
                    for (type in events) {
                        this.remove(element, type)
                    }
                } else {
                    if (events[type]) {
                        if (handler) {
                            delete events[type][handler.guid]
                        } else {
                            for (handler in events[type]) {
                                if (!parts[1] || events[type][handler].type == parts[1]) {
                                    delete events[type][handler]
                                }
                            }
                        }
                        for (ret in events[type]) {
                            break
                        }
                        if (!ret) {
                            if (element.removeEventListener) {
                                element.removeEventListener(type, jQuery.data(element, "handle"), false)
                            } else {
                                element.detachEvent("on" + type, jQuery.data(element, "handle"))
                            }
                            ret = null;
                            delete events[type]
                        }
                    }
                }
                for (ret in events) {
                    break
                }
                if (!ret) {
                    jQuery.removeData(element, "events");
                    jQuery.removeData(element, "handle")
                }
            }
        },
        trigger: function(type, data, element, donative, extra) {
            data = jQuery.makeArray(data || []);
            if (!element) {
                if (this.global[type]) {
                    jQuery("*").add([window, document]).trigger(type, data)
                }
            } else {
                var val, ret, fn = jQuery.isFunction(element[type] || null),
                    evt = !data[0] || !data[0].preventDefault;
                if (evt) {
                    data.unshift(this.fix({
                        type: type,
                        target: element
                    }))
                }
                data[0].type = type;
                if (jQuery.isFunction(jQuery.data(element, "handle"))) {
                    val = jQuery.data(element, "handle").apply(element, data)
                }
                if (!fn && element["on" + type] && element["on" + type].apply(element, data) === false) {
                    val = false
                }
                if (evt) {
                    data.shift()
                }
                if (extra && extra.apply(element, data) === false) {
                    val = false
                }
                if (fn && donative !== false && val !== false && !(jQuery.nodeName(element, "a") && type == "click")) {
                    this.triggered = true;
                    element[type]()
                }
                this.triggered = false
            }
            return val
        },
        handle: function(event) {
            var val;
            event = jQuery.event.fix(event || window.event || {});
            var parts = event.type.split(".");
            event.type = parts[0];
            var c = jQuery.data(this, "events") && jQuery.data(this, "events")[event.type],
                args = Array.prototype.slice.call(arguments, 1);
            args.unshift(event);
            for (var j in c) {
                args[0].handler = c[j];
                args[0].data = c[j].data;
                if (!parts[1] || c[j].type == parts[1]) {
                    var tmp = c[j].apply(this, args);
                    if (val !== false) {
                        val = tmp
                    }
                    if (tmp === false) {
                        event.preventDefault();
                        event.stopPropagation()
                    }
                }
            }
            if (jQuery.browser.msie) {
                event.target = event.preventDefault = event.stopPropagation = event.handler = event.data = null
            }
            return val
        },
        fix: function(event) {
            var originalEvent = event;
            event = jQuery.extend({}, originalEvent);
            event.preventDefault = function() {
                if (originalEvent.preventDefault) {
                    originalEvent.preventDefault()
                }
                originalEvent.returnValue = false
            };
            event.stopPropagation = function() {
                if (originalEvent.stopPropagation) {
                    originalEvent.stopPropagation()
                }
                originalEvent.cancelBubble = true
            };
            if (!event.target && event.srcElement) {
                event.target = event.srcElement
            }
            if (jQuery.browser.safari && event.target.nodeType == 3) {
                event.target = originalEvent.target.parentNode
            }
            if (!event.relatedTarget && event.fromElement) {
                event.relatedTarget = event.fromElement == event.target ? event.toElement : event.fromElement
            }
            if (event.pageX == null && event.clientX != null) {
                var e = document.documentElement,
                    b = document.body;
                event.pageX = event.clientX + (e && e.scrollLeft || b.scrollLeft || 0);
                event.pageY = event.clientY + (e && e.scrollTop || b.scrollTop || 0)
            }
            if (!event.which && (event.charCode || event.keyCode)) {
                event.which = event.charCode || event.keyCode
            }
            if (!event.metaKey && event.ctrlKey) {
                event.metaKey = event.ctrlKey
            }
            if (!event.which && event.button) {
                event.which = (event.button & 1 ? 1 : (event.button & 2 ? 3 : (event.button & 4 ? 2 : 0)))
            }
            return event
        }
    };
    jQuery.fn.extend({
        bind: function(type, data, fn) {
            return type == "unload" ? this.one(type, data, fn) : this.each(function() {
                jQuery.event.add(this, type, fn || data, fn && data)
            })
        },
        one: function(type, data, fn) {
            return this.each(function() {
                jQuery.event.add(this, type, function(event) {
                    jQuery(this).unbind(event);
                    return (fn || data).apply(this, arguments)
                }, fn && data)
            })
        },
        unbind: function(type, fn) {
            return this.each(function() {
                jQuery.event.remove(this, type, fn)
            })
        },
        trigger: function(type, data, fn) {
            return this.each(function() {
                jQuery.event.trigger(type, data, this, true, fn)
            })
        },
        triggerHandler: function(type, data, fn) {
            if (this[0]) {
                return jQuery.event.trigger(type, data, this[0], false, fn)
            }
        },
        toggle: function() {
            var a = arguments;
            return this.click(function(e) {
                this.lastToggle = 0 == this.lastToggle ? 1 : 0;
                e.preventDefault();
                return a[this.lastToggle].apply(this, [e]) || false
            })
        },
        hover: function(f, g) {
            function handleHover(e) {
                var p = e.relatedTarget;
                while (p && p != this) {
                    try {
                        p = p.parentNode
                    } catch (e) {
                        p = this
                    }
                }
                if (p == this) {
                    return false
                }
                return (e.type == "mouseover" ? f : g).apply(this, [e])
            }
            return this.mouseover(handleHover).mouseout(handleHover)
        },
        ready: function(f) {
            bindReady();
            if (jQuery.isReady) {
                f.apply(document, [jQuery])
            } else {
                jQuery.readyList.push(function() {
                    return f.apply(this, [jQuery])
                })
            }
            return this
        }
    });
    jQuery.extend({
        isReady: false,
        readyList: [],
        ready: function() {
            if (!jQuery.isReady) {
                jQuery.isReady = true;
                if (jQuery.readyList) {
                    jQuery.each(jQuery.readyList, function() {
                        this.apply(document)
                    });
                    jQuery.readyList = null
                }
                if (jQuery.browser.mozilla || jQuery.browser.opera) {
                    document.removeEventListener("DOMContentLoaded", jQuery.ready, false)
                }
                if (!window.frames.length) {
                    jQuery(window).load(function() {
                        jQuery("#__ie_init").remove()
                    })
                }
            }
        }
    });
    jQuery.each(("blur,focus,load,resize,scroll,unload,click,dblclick,mousedown,mouseup,mousemove,mouseover,mouseout,change,select,submit,keydown,keypress,keyup,error").split(","), function(i, o) {
        jQuery.fn[o] = function(f) {
            return f ? this.bind(o, f) : this.trigger(o)
        }
    });
    var readyBound = false;

    function bindReady() {
        if (readyBound) {
            return
        }
        readyBound = true;
        if (jQuery.browser.mozilla || jQuery.browser.opera) {
            document.addEventListener("DOMContentLoaded", jQuery.ready, false)
        } else {
            if (jQuery.browser.msie) {
                document.write("<script id=__ie_init defer=true src=//:><\/script>");
                var script = document.getElementById("__ie_init");
                if (script) {
                    script.onreadystatechange = function() {
                        if (this.readyState != "complete") {
                            return
                        }
                        jQuery.ready()
                    }
                }
                script = null
            } else {
                if (jQuery.browser.safari) {
                    jQuery.safariTimer = setInterval(function() {
                        if (document.readyState == "loaded" || document.readyState == "complete") {
                            clearInterval(jQuery.safariTimer);
                            jQuery.safariTimer = null;
                            jQuery.ready()
                        }
                    }, 10)
                }
            }
        }
        jQuery.event.add(window, "load", jQuery.ready)
    }
    jQuery.fn.extend({
        load: function(url, params, callback) {
            if (jQuery.isFunction(url)) {
                return this.bind("load", url)
            }
            var off = url.indexOf(" ");
            if (off >= 0) {
                var selector = url.slice(off, url.length);
                url = url.slice(0, off)
            }
            callback = callback || function() {};
            var type = "GET";
            if (params) {
                if (jQuery.isFunction(params)) {
                    callback = params;
                    params = null
                } else {
                    params = jQuery.param(params);
                    type = "POST"
                }
            }
            var self = this;
            jQuery.ajax({
                url: url,
                type: type,
                data: params,
                complete: function(res, status) {
                    if (status == "success" || status == "notmodified") {
                        self.html(selector ? jQuery("<div/>").append(res.responseText.replace(/<script(.|\s)*?\/script>/g, "")).find(selector) : res.responseText)
                    }
                    setTimeout(function() {
                        self.each(callback, [res.responseText, status, res])
                    }, 13)
                }
            });
            return this
        },
        serialize: function() {
            return jQuery.param(this.serializeArray())
        },
        serializeArray: function() {
            return this.map(function() {
                return jQuery.nodeName(this, "form") ? jQuery.makeArray(this.elements) : this
            }).filter(function() {
                return this.name && !this.disabled && (this.checked || /select|textarea/i.test(this.nodeName) || /text|hidden|password/i.test(this.type))
            }).map(function(i, elem) {
                var val = jQuery(this).val();
                return val == null ? null : val.constructor == Array ? jQuery.map(val, function(val, i) {
                    return {
                        name: elem.name,
                        value: val
                    }
                }) : {
                    name: elem.name,
                    value: val
                }
            }).get()
        }
    });
    jQuery.each("ajaxStart,ajaxStop,ajaxComplete,ajaxError,ajaxSuccess,ajaxSend".split(","), function(i, o) {
        jQuery.fn[o] = function(f) {
            return this.bind(o, f)
        }
    });
    var jsc = (new Date).getTime();
    jQuery.extend({
        get: function(url, data, callback, type) {
            if (jQuery.isFunction(data)) {
                callback = data;
                data = null
            }
            return jQuery.ajax({
                type: "GET",
                url: url,
                data: data,
                success: callback,
                dataType: type
            })
        },
        getScript: function(url, callback) {
            return jQuery.get(url, null, callback, "script")
        },
        getJSON: function(url, data, callback) {
            return jQuery.get(url, data, callback, "json")
        },
        post: function(url, data, callback, type) {
            if (jQuery.isFunction(data)) {
                callback = data;
                data = {}
            }
            return jQuery.ajax({
                type: "POST",
                url: url,
                data: data,
                success: callback,
                dataType: type
            })
        },
        ajaxSetup: function(settings) {
            jQuery.extend(jQuery.ajaxSettings, settings)
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
        lastModified: {},
        ajax: function(s) {
            var jsonp, jsre = /=(\?|%3F)/g,
                status, data;
            s = jQuery.extend(true, s, jQuery.extend(true, {}, jQuery.ajaxSettings, s));
            if (s.data && s.processData && typeof s.data != "string") {
                s.data = jQuery.param(s.data)
            }
            if (s.dataType == "jsonp") {
                if (s.type.toLowerCase() == "get") {
                    if (!s.url.match(jsre)) {
                        s.url += (s.url.match(/\?/) ? "&" : "?") + (s.jsonp || "callback") + "=?"
                    }
                } else {
                    if (!s.data || !s.data.match(jsre)) {
                        s.data = (s.data ? s.data + "&" : "") + (s.jsonp || "callback") + "=?"
                    }
                }
                s.dataType = "json"
            }
            if (s.dataType == "json" && (s.data && s.data.match(jsre) || s.url.match(jsre))) {
                jsonp = "jsonp" + jsc++;
                if (s.data) {
                    s.data = s.data.replace(jsre, "=" + jsonp)
                }
                s.url = s.url.replace(jsre, "=" + jsonp);
                s.dataType = "script";
                window[jsonp] = function(tmp) {
                    data = tmp;
                    success();
                    complete();
                    window[jsonp] = undefined;
                    try {
                        delete window[jsonp]
                    } catch (e) {}
                }
            }
            if (s.dataType == "script" && s.cache == null) {
                s.cache = false
            }
            if (s.cache === false && s.type.toLowerCase() == "get") {
                s.url += (s.url.match(/\?/) ? "&" : "?") + "_=" + (new Date()).getTime()
            }
            if (s.data && s.type.toLowerCase() == "get") {
                s.url += (s.url.match(/\?/) ? "&" : "?") + s.data;
                s.data = null
            }
            if (s.global && !jQuery.active++) {
                jQuery.event.trigger("ajaxStart")
            }
            if (!s.url.indexOf("http") && s.dataType == "script") {
                var head = document.getElementsByTagName("head")[0];
                var script = document.createElement("script");
                script.src = s.url;
                if (!jsonp && (s.success || s.complete)) {
                    var done = false;
                    script.onload = script.onreadystatechange = function() {
                        if (!done && (!this.readyState || this.readyState == "loaded" || this.readyState == "complete")) {
                            done = true;
                            success();
                            complete();
                            head.removeChild(script)
                        }
                    }
                }
                head.appendChild(script);
                return
            }
            var requestDone = false;
            var xml = window.ActiveXObject ? new ActiveXObject("Microsoft.XMLHTTP") : new XMLHttpRequest();
            xml.open(s.type, s.url, s.async);
            if (s.data) {
                xml.setRequestHeader("Content-Type", s.contentType)
            }
            if (s.ifModified) {
                xml.setRequestHeader("If-Modified-Since", jQuery.lastModified[s.url] || "Thu, 01 Jan 1970 00:00:00 GMT")
            }
            xml.setRequestHeader("X-Requested-With", "XMLHttpRequest");
            if (s.beforeSend) {
                s.beforeSend(xml)
            }
            if (s.global) {
                jQuery.event.trigger("ajaxSend", [xml, s])
            }
            var onreadystatechange = function(isTimeout) {
                if (!requestDone && xml && (xml.readyState == 4 || isTimeout == "timeout")) {
                    requestDone = true;
                    if (ival) {
                        clearInterval(ival);
                        ival = null
                    }
                    status = isTimeout == "timeout" && "timeout" || !jQuery.httpSuccess(xml) && "error" || s.ifModified && jQuery.httpNotModified(xml, s.url) && "notmodified" || "success";
                    if (status == "success") {
                        try {
                            data = jQuery.httpData(xml, s.dataType)
                        } catch (e) {
                            status = "parsererror"
                        }
                    }
                    if (status == "success") {
                        var modRes;
                        try {
                            modRes = xml.getResponseHeader("Last-Modified")
                        } catch (e) {}
                        if (s.ifModified && modRes) {
                            jQuery.lastModified[s.url] = modRes
                        }
                        if (!jsonp) {
                            success()
                        }
                    } else {
                        jQuery.handleError(s, xml, status)
                    }
                    complete();
                    if (s.async) {
                        xml = null
                    }
                }
            };
            if (s.async) {
                var ival = setInterval(onreadystatechange, 13);
                if (s.timeout > 0) {
                    setTimeout(function() {
                        if (xml) {
                            xml.abort();
                            if (!requestDone) {
                                onreadystatechange("timeout")
                            }
                        }
                    }, s.timeout)
                }
            }
            try {
                xml.send(s.data)
            } catch (e) {
                jQuery.handleError(s, xml, null, e)
            }
            if (!s.async) {
                onreadystatechange()
            }
            return xml;

            function success() {
                if (s.success) {
                    s.success(data, status)
                }
                if (s.global) {
                    jQuery.event.trigger("ajaxSuccess", [xml, s])
                }
            }

            function complete() {
                if (s.complete) {
                    s.complete(xml, status)
                }
                if (s.global) {
                    jQuery.event.trigger("ajaxComplete", [xml, s])
                }
                if (s.global && !--jQuery.active) {
                    jQuery.event.trigger("ajaxStop")
                }
            }
        },
        handleError: function(s, xml, status, e) {
            if (s.error) {
                s.error(xml, status, e)
            }
            if (s.global) {
                jQuery.event.trigger("ajaxError", [xml, s, e])
            }
        },
        active: 0,
        httpSuccess: function(r) {
            try {
                return !r.status && location.protocol == "file:" || (r.status >= 200 && r.status < 300) || r.status == 304 || jQuery.browser.safari && r.status == undefined
            } catch (e) {}
            return false
        },
        httpNotModified: function(xml, url) {
            try {
                var xmlRes = xml.getResponseHeader("Last-Modified");
                return xml.status == 304 || xmlRes == jQuery.lastModified[url] || jQuery.browser.safari && xml.status == undefined
            } catch (e) {}
            return false
        },
        httpData: function(r, type) {
            var ct = r.getResponseHeader("content-type");
            var xml = type == "xml" || !type && ct && ct.indexOf("xml") >= 0;
            var data = xml ? r.responseXML : r.responseText;
            if (xml && data.documentElement.tagName == "parsererror") {
                throw "parsererror"
            }
            if (type == "script") {
                jQuery.globalEval(data)
            }
            if (type == "json") {
                data = eval("(" + data + ")")
            }
            return data
        },
        param: function(a) {
            var s = [];
            if (a.constructor == Array || a.jquery) {
                jQuery.each(a, function() {
                    s.push(encodeURIComponent(this.name) + "=" + encodeURIComponent(this.value))
                })
            } else {
                for (var j in a) {
                    if (a[j] && a[j].constructor == Array) {
                        jQuery.each(a[j], function() {
                            s.push(encodeURIComponent(j) + "=" + encodeURIComponent(this))
                        })
                    } else {
                        s.push(encodeURIComponent(j) + "=" + encodeURIComponent(a[j]))
                    }
                }
            }
            return s.join("&").replace(/%20/g, "+")
        }
    });
    jQuery.fn.extend({
        show: function(speed, callback) {
            return speed ? this.animate({
                height: "show",
                width: "show",
                opacity: "show"
            }, speed, callback) : this.filter(":hidden").each(function() {
                this.style.display = this.oldblock ? this.oldblock : "";
                if (jQuery.css(this, "display") == "none") {
                    this.style.display = "block"
                }
            }).end()
        },
        hide: function(speed, callback) {
            return speed ? this.animate({
                height: "hide",
                width: "hide",
                opacity: "hide"
            }, speed, callback) : this.filter(":visible").each(function() {
                this.oldblock = this.oldblock || jQuery.css(this, "display");
                if (this.oldblock == "none") {
                    this.oldblock = "block"
                }
                this.style.display = "none"
            }).end()
        },
        _toggle: jQuery.fn.toggle,
        toggle: function(fn, fn2) {
            return jQuery.isFunction(fn) && jQuery.isFunction(fn2) ? this._toggle(fn, fn2) : fn ? this.animate({
                height: "toggle",
                width: "toggle",
                opacity: "toggle"
            }, fn, fn2) : this.each(function() {
                jQuery(this)[jQuery(this).is(":hidden") ? "show" : "hide"]()
            })
        },
        slideDown: function(speed, callback) {
            return this.animate({
                height: "show"
            }, speed, callback)
        },
        slideUp: function(speed, callback) {
            return this.animate({
                height: "hide"
            }, speed, callback)
        },
        slideToggle: function(speed, callback) {
            return this.animate({
                height: "toggle"
            }, speed, callback)
        },
        fadeIn: function(speed, callback) {
            return this.animate({
                opacity: "show"
            }, speed, callback)
        },
        fadeOut: function(speed, callback) {
            return this.animate({
                opacity: "hide"
            }, speed, callback)
        },
        fadeTo: function(speed, to, callback) {
            return this.animate({
                opacity: to
            }, speed, callback)
        },
        animate: function(prop, speed, easing, callback) {
            var opt = jQuery.speed(speed, easing, callback);
            return this[opt.queue === false ? "each" : "queue"](function() {
                opt = jQuery.extend({}, opt);
                var hidden = jQuery(this).is(":hidden"),
                    self = this;
                for (var p in prop) {
                    if (prop[p] == "hide" && hidden || prop[p] == "show" && !hidden) {
                        return jQuery.isFunction(opt.complete) && opt.complete.apply(this)
                    }
                    if (p == "height" || p == "width") {
                        opt.display = jQuery.css(this, "display");
                        opt.overflow = this.style.overflow
                    }
                }
                if (opt.overflow != null) {
                    this.style.overflow = "hidden"
                }
                opt.curAnim = jQuery.extend({}, prop);
                jQuery.each(prop, function(name, val) {
                    var e = new jQuery.fx(self, opt, name);
                    if (/toggle|show|hide/.test(val)) {
                        e[val == "toggle" ? hidden ? "show" : "hide" : val](prop)
                    } else {
                        var parts = val.toString().match(/^([+-]=)?([\d+-.]+)(.*)$/),
                            start = e.cur(true) || 0;
                        if (parts) {
                            var end = parseFloat(parts[2]),
                                unit = parts[3] || "px";
                            if (unit != "px") {
                                self.style[name] = (end || 1) + unit;
                                start = ((end || 1) / e.cur(true)) * start;
                                self.style[name] = start + unit
                            }
                            if (parts[1]) {
                                end = ((parts[1] == "-=" ? -1 : 1) * end) + start
                            }
                            e.custom(start, end, unit)
                        } else {
                            e.custom(start, val, "")
                        }
                    }
                });
                return true
            })
        },
        queue: function(type, fn) {
            if (jQuery.isFunction(type)) {
                fn = type;
                type = "fx"
            }
            if (!type || (typeof type == "string" && !fn)) {
                return queue(this[0], type)
            }
            return this.each(function() {
                if (fn.constructor == Array) {
                    queue(this, type, fn)
                } else {
                    queue(this, type).push(fn);
                    if (queue(this, type).length == 1) {
                        fn.apply(this)
                    }
                }
            })
        },
        stop: function() {
            var timers = jQuery.timers;
            return this.each(function() {
                for (var i = 0; i < timers.length; i++) {
                    if (timers[i].elem == this) {
                        timers.splice(i--, 1)
                    }
                }
            }).dequeue()
        }
    });
    var queue = function(elem, type, array) {
        if (!elem) {
            return
        }
        var q = jQuery.data(elem, type + "queue");
        if (!q || array) {
            q = jQuery.data(elem, type + "queue", array ? jQuery.makeArray(array) : [])
        }
        return q
    };
    jQuery.fn.dequeue = function(type) {
        type = type || "fx";
        return this.each(function() {
            var q = queue(this, type);
            q.shift();
            if (q.length) {
                q[0].apply(this)
            }
        })
    };
    jQuery.extend({
        speed: function(speed, easing, fn) {
            var opt = speed && speed.constructor == Object ? speed : {
                complete: fn || !fn && easing || jQuery.isFunction(speed) && speed,
                duration: speed,
                easing: fn && easing || easing && easing.constructor != Function && easing
            };
            opt.duration = (opt.duration && opt.duration.constructor == Number ? opt.duration : {
                slow: 600,
                fast: 200
            }[opt.duration]) || 400;
            opt.old = opt.complete;
            opt.complete = function() {
                jQuery(this).dequeue();
                if (jQuery.isFunction(opt.old)) {
                    opt.old.apply(this)
                }
            };
            return opt
        },
        easing: {
            linear: function(p, n, firstNum, diff) {
                return firstNum + diff * p
            },
            swing: function(p, n, firstNum, diff) {
                return ((-Math.cos(p * Math.PI) / 2) + 0.5) * diff + firstNum
            }
        },
        timers: [],
        fx: function(elem, options, prop) {
            this.options = options;
            this.elem = elem;
            this.prop = prop;
            if (!options.orig) {
                options.orig = {}
            }
        }
    });
    jQuery.fx.prototype = {
        update: function() {
            if (this.options.step) {
                this.options.step.apply(this.elem, [this.now, this])
            }(jQuery.fx.step[this.prop] || jQuery.fx.step._default)(this);
            if (this.prop == "height" || this.prop == "width") {
                this.elem.style.display = "block"
            }
        },
        cur: function(force) {
            if (this.elem[this.prop] != null && this.elem.style[this.prop] == null) {
                return this.elem[this.prop]
            }
            var r = parseFloat(jQuery.curCSS(this.elem, this.prop, force));
            return r && r > -10000 ? r : parseFloat(jQuery.css(this.elem, this.prop)) || 0
        },
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
                return self.step()
            }
            t.elem = this.elem;
            jQuery.timers.push(t);
            if (jQuery.timers.length == 1) {
                var timer = setInterval(function() {
                    var timers = jQuery.timers;
                    for (var i = 0; i < timers.length; i++) {
                        if (!timers[i]()) {
                            timers.splice(i--, 1)
                        }
                    }
                    if (!timers.length) {
                        clearInterval(timer)
                    }
                }, 13)
            }
        },
        show: function() {
            this.options.orig[this.prop] = jQuery.attr(this.elem.style, this.prop);
            this.options.show = true;
            this.custom(0, this.cur());
            if (this.prop == "width" || this.prop == "height") {
                this.elem.style[this.prop] = "1px"
            }
            jQuery(this.elem).show()
        },
        hide: function() {
            this.options.orig[this.prop] = jQuery.attr(this.elem.style, this.prop);
            this.options.hide = true;
            this.custom(this.cur(), 0)
        },
        step: function() {
            var t = (new Date()).getTime();
            if (t > this.options.duration + this.startTime) {
                this.now = this.end;
                this.pos = this.state = 1;
                this.update();
                this.options.curAnim[this.prop] = true;
                var done = true;
                for (var i in this.options.curAnim) {
                    if (this.options.curAnim[i] !== true) {
                        done = false
                    }
                }
                if (done) {
                    if (this.options.display != null) {
                        this.elem.style.overflow = this.options.overflow;
                        this.elem.style.display = this.options.display;
                        if (jQuery.css(this.elem, "display") == "none") {
                            this.elem.style.display = "block"
                        }
                    }
                    if (this.options.hide) {
                        this.elem.style.display = "none"
                    }
                    if (this.options.hide || this.options.show) {
                        for (var p in this.options.curAnim) {
                            jQuery.attr(this.elem.style, p, this.options.orig[p])
                        }
                    }
                }
                if (done && jQuery.isFunction(this.options.complete)) {
                    this.options.complete.apply(this.elem)
                }
                return false
            } else {
                var n = t - this.startTime;
                this.state = n / this.options.duration;
                this.pos = jQuery.easing[this.options.easing || (jQuery.easing.swing ? "swing" : "linear")](this.state, n, 0, 1, this.options.duration);
                this.now = this.start + ((this.end - this.start) * this.pos);
                this.update()
            }
            return true
        }
    };
    jQuery.fx.step = {
        scrollLeft: function(fx) {
            fx.elem.scrollLeft = fx.now
        },
        scrollTop: function(fx) {
            fx.elem.scrollTop = fx.now
        },
        opacity: function(fx) {
            jQuery.attr(fx.elem.style, "opacity", fx.now)
        },
        _default: function(fx) {
            fx.elem.style[fx.prop] = fx.now + fx.unit
        }
    };
    jQuery.fn.offset = function() {
        var left = 0,
            top = 0,
            elem = this[0],
            results;
        if (elem) {
            with(jQuery.browser) {
                var absolute = jQuery.css(elem, "position") == "absolute",
                    parent = elem.parentNode,
                    offsetParent = elem.offsetParent,
                    doc = elem.ownerDocument,
                    safari2 = safari && parseInt(version) < 522;
                if (elem.getBoundingClientRect) {
                    box = elem.getBoundingClientRect();
                    add(box.left + Math.max(doc.documentElement.scrollLeft, doc.body.scrollLeft), box.top + Math.max(doc.documentElement.scrollTop, doc.body.scrollTop));
                    if (msie) {
                        var border = jQuery("html").css("borderWidth");
                        border = (border == "medium" || jQuery.boxModel && parseInt(version) >= 7) && 2 || border;
                        add(-border, -border)
                    }
                } else {
                    add(elem.offsetLeft, elem.offsetTop);
                    while (offsetParent) {
                        add(offsetParent.offsetLeft, offsetParent.offsetTop);
                        if (mozilla && /^t[d|h]$/i.test(parent.tagName) || !safari2) {
                            border(offsetParent)
                        }
                        if (safari2 && !absolute && jQuery.css(offsetParent, "position") == "absolute") {
                            absolute = true
                        }
                        offsetParent = offsetParent.offsetParent
                    }
                    while (parent.tagName && !/^body|html$/i.test(parent.tagName)) {
                        if (!/^inline|table-row.*$/i.test(jQuery.css(parent, "display"))) {
                            add(-parent.scrollLeft, -parent.scrollTop)
                        }
                        if (mozilla && jQuery.css(parent, "overflow") != "visible") {
                            border(parent)
                        }
                        parent = parent.parentNode
                    }
                    if (safari2 && absolute) {
                        add(-doc.body.offsetLeft, -doc.body.offsetTop)
                    }
                }
                results = {
                    top: top,
                    left: left
                }
            }
        }
        return results;

        function border(elem) {
            add(jQuery.css(elem, "borderLeftWidth"), jQuery.css(elem, "borderTopWidth"))
        }

        function add(l, t) {
            left += parseInt(l) || 0;
            top += parseInt(t) || 0
        }
    }
})();
(function($) {
    function Datepicker() {
        this.debug = false;
        this._nextId = 0;
        this._inst = [];
        this._curInst = null;
        this._disabledInputs = [];
        this._datepickerShowing = false;
        this._inDialog = false;
        this.regional = [];
        this.regional[""] = {
            clearText: "Clear",
            clearStatus: "Erase the current date",
            closeText: "Close",
            closeStatus: "Close without change",
            prevText: "&#x3c;Prev",
            prevStatus: "Show the previous month",
            nextText: "Next&#x3e;",
            nextStatus: "Show the next month",
            currentText: "Today",
            currentStatus: "Show the current month",
            monthNames: ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"],
            monthNamesShort: ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"],
            monthStatus: "Show a different month",
            yearStatus: "Show a different year",
            weekHeader: "Wk",
            weekStatus: "Week of the year",
            dayNames: ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"],
            dayNamesShort: ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"],
            dayNamesMin: ["Su", "Mo", "Tu", "We", "Th", "Fr", "Sa"],
            dayStatus: "Set DD as first week day",
            dateStatus: "Select DD, M d",
            dateFormat: "mm/dd/yy",
            firstDay: 0,
            initStatus: "Select a date",
            isRTL: false
        };
        this._defaults = {
            showOn: "focus",
            showAnim: "show",
            defaultDate: null,
            appendText: "",
            buttonText: "...",
            buttonImage: "",
            buttonImageOnly: false,
            closeAtTop: true,
            mandatory: false,
            hideIfNoPrevNext: false,
            changeMonth: true,
            changeYear: true,
            yearRange: "-10:+10",
            changeFirstDay: false,
            showOtherMonths: false,
            showWeeks: false,
            calculateWeek: this.iso8601Week,
            shortYearCutoff: "+10",
            showStatus: false,
            statusForDate: this.dateStatus,
            minDate: null,
            maxDate: null,
            speed: "normal",
            beforeShowDay: null,
            beforeShow: null,
            onSelect: null,
            onClose: null,
            numberOfMonths: 1,
            stepMonths: 1,
            rangeSelect: false,
            rangeSeparator: " - "
        };
        $.extend(this._defaults, this.regional[""]);
        this._datepickerDiv = $('<div id="datepicker_div">')
    }
    $.extend(Datepicker.prototype, {
        markerClassName: "hasDatepicker",
        log: function() {
            if (this.debug) {
                console.log.apply("", arguments)
            }
        },
        _register: function(inst) {
            var id = this._nextId++;
            this._inst[id] = inst;
            return id
        },
        _getInst: function(id) {
            return this._inst[id] || id
        },
        setDefaults: function(settings) {
            extendRemove(this._defaults, settings || {});
            return this
        },
        _attachDatepicker: function(target, settings) {
            var inlineSettings = null;
            for (attrName in this._defaults) {
                var attrValue = target.getAttribute("date:" + attrName);
                if (attrValue) {
                    inlineSettings = inlineSettings || {};
                    try {
                        inlineSettings[attrName] = eval(attrValue)
                    } catch (err) {
                        inlineSettings[attrName] = attrValue
                    }
                }
            }
            var nodeName = target.nodeName.toLowerCase();
            var instSettings = (inlineSettings ? $.extend(settings || {}, inlineSettings || {}) : settings);
            if (nodeName == "input") {
                var inst = (inst && !inlineSettings ? inst : new DatepickerInstance(instSettings, false));
                this._connectDatepicker(target, inst)
            } else {
                if (nodeName == "div" || nodeName == "span") {
                    var inst = new DatepickerInstance(instSettings, true);
                    this._inlineDatepicker(target, inst)
                }
            }
        },
        _destroyDatepicker: function(target) {
            var nodeName = target.nodeName.toLowerCase();
            var calId = target._calId;
            target._calId = null;
            var $target = $(target);
            if (nodeName == "input") {
                $target.siblings(".datepicker_append").replaceWith("").end().siblings(".datepicker_trigger").replaceWith("").end().removeClass(this.markerClassName).unbind("focus", this._showDatepicker).unbind("keydown", this._doKeyDown).unbind("keypress", this._doKeyPress);
                var wrapper = $target.parents(".datepicker_wrap");
                if (wrapper) {
                    wrapper.replaceWith(wrapper.html())
                }
            } else {
                if (nodeName == "div" || nodeName == "span") {
                    $target.removeClass(this.markerClassName).empty()
                }
            }
            if ($("input[_calId=" + calId + "]").length == 0) {
                this._inst[calId] = null
            }
        },
        _enableDatepicker: function(target) {
            target.disabled = false;
            $(target).siblings("button.datepicker_trigger").each(function() {
                this.disabled = false
            }).end().siblings("img.datepicker_trigger").css({
                opacity: "1.0",
                cursor: ""
            });
            this._disabledInputs = $.map(this._disabledInputs, function(value) {
                return (value == target ? null : value)
            })
        },
        _disableDatepicker: function(target) {
            target.disabled = true;
            $(target).siblings("button.datepicker_trigger").each(function() {
                this.disabled = true
            }).end().siblings("img.datepicker_trigger").css({
                opacity: "0.5",
                cursor: "default"
            });
            this._disabledInputs = $.map($.datepicker._disabledInputs, function(value) {
                return (value == target ? null : value)
            });
            this._disabledInputs[$.datepicker._disabledInputs.length] = target
        },
        _isDisabledDatepicker: function(target) {
            if (!target) {
                return false
            }
            for (var i = 0; i < this._disabledInputs.length; i++) {
                if (this._disabledInputs[i] == target) {
                    return true
                }
            }
            return false
        },
        _changeDatepicker: function(target, name, value) {
            var settings = name || {};
            if (typeof name == "string") {
                settings = {};
                settings[name] = value
            }
            if (inst = this._getInst(target._calId)) {
                extendRemove(inst._settings, settings);
                this._updateDatepicker(inst)
            }
        },
        _setDateDatepicker: function(target, date, endDate) {
            if (inst = this._getInst(target._calId)) {
                inst._setDate(date, endDate);
                this._updateDatepicker(inst)
            }
        },
        _getDateDatepicker: function(target) {
            var inst = this._getInst(target._calId);
            return (inst ? inst._getDate() : null)
        },
        _doKeyDown: function(e) {
            var inst = $.datepicker._getInst(this._calId);
            if ($.datepicker._datepickerShowing) {
                switch (e.keyCode) {
                    case 9:
                        $.datepicker._hideDatepicker(null, "");
                        break;
                    case 13:
                        $.datepicker._selectDay(inst, inst._selectedMonth, inst._selectedYear, $("td.datepicker_daysCellOver", inst._datepickerDiv)[0]);
                        return false;
                        break;
                    case 27:
                        $.datepicker._hideDatepicker(null, inst._get("speed"));
                        break;
                    case 33:
                        $.datepicker._adjustDate(inst, (e.ctrlKey ? -1 : -inst._get("stepMonths")), (e.ctrlKey ? "Y" : "M"));
                        break;
                    case 34:
                        $.datepicker._adjustDate(inst, (e.ctrlKey ? +1 : +inst._get("stepMonths")), (e.ctrlKey ? "Y" : "M"));
                        break;
                    case 35:
                        if (e.ctrlKey) {
                            $.datepicker._clearDate(inst)
                        }
                        break;
                    case 36:
                        if (e.ctrlKey) {
                            $.datepicker._gotoToday(inst)
                        }
                        break;
                    case 37:
                        if (e.ctrlKey) {
                            $.datepicker._adjustDate(inst, -1, "D")
                        }
                        break;
                    case 38:
                        if (e.ctrlKey) {
                            $.datepicker._adjustDate(inst, -7, "D")
                        }
                        break;
                    case 39:
                        if (e.ctrlKey) {
                            $.datepicker._adjustDate(inst, +1, "D")
                        }
                        break;
                    case 40:
                        if (e.ctrlKey) {
                            $.datepicker._adjustDate(inst, +7, "D")
                        }
                        break
                }
            } else {
                if (e.keyCode == 36 && e.ctrlKey) {
                    $.datepicker._showDatepicker(this)
                }
            }
        },
        _doKeyPress: function(e) {
            var inst = $.datepicker._getInst(this._calId);
            var chars = $.datepicker._possibleChars(inst._get("dateFormat"));
            var chr = String.fromCharCode(e.charCode == undefined ? e.keyCode : e.charCode);
            return e.ctrlKey || (chr < " " || !chars || chars.indexOf(chr) > -1)
        },
        _connectDatepicker: function(target, inst) {
            var input = $(target);
            if (input.is("." + this.markerClassName)) {
                return
            }
            var appendText = inst._get("appendText");
            var isRTL = inst._get("isRTL");
            if (appendText) {
                if (isRTL) {
                    input.before('<span class="datepicker_append">' + appendText)
                } else {
                    input.after('<span class="datepicker_append">' + appendText)
                }
            }
            var showOn = inst._get("showOn");
            if (showOn == "focus" || showOn == "both") {
                input.focus(this._showDatepicker)
            }
            if (showOn == "button" || showOn == "both") {
                input.wrap('<span id="calDiv" class="datepicker_wrap">');
                var buttonText = inst._get("buttonText");
                var buttonImage = inst._get("buttonImage");
                var trigger = $(inst._get("buttonImageOnly") ? $("<img>").addClass("datepicker_trigger").attr({
                    src: buttonImage,
                    alt: buttonText,
                    title: buttonText
                }) : $("<button>").addClass("datepicker_trigger").attr({
                    type: "button"
                }).html(buttonImage != "" ? $("<img>").attr({
                    src: buttonImage,
                    alt: buttonText,
                    title: buttonText
                }) : buttonText));
                if (isRTL) {
                    input.before(trigger)
                } else {
                    input.after(trigger)
                }
                trigger.click(function() {
                    if ($.datepicker._datepickerShowing && $.datepicker._lastInput == target) {
                        $.datepicker._hideDatepicker()
                    } else {
                        $.datepicker._showDatepicker(target)
                    }
                })
            }
            input.addClass(this.markerClassName).keydown(this._doKeyDown).keypress(this._doKeyPress).bind("setData.datepicker", function(event, key, value) {
                inst._settings[key] = value
            }).bind("getData.datepicker", function(event, key) {
                return inst._get(key)
            });
            input[0]._calId = inst._id
        },
        _inlineDatepicker: function(target, inst) {
            var input = $(target);
            if (input.is("." + this.markerClassName)) {
                return
            }
            input.addClass(this.markerClassName).append(inst._datepickerDiv).bind("setData.datepicker", function(event, key, value) {
                inst._settings[key] = value
            }).bind("getData.datepicker", function(event, key) {
                return inst._get(key)
            });
            input[0]._calId = inst._id;
            this._updateDatepicker(inst)
        },
        _inlineShow: function(inst) {
            var numMonths = inst._getNumberOfMonths()
        },
        _dialogDatepicker: function(input, dateText, onSelect, settings, pos) {
            var inst = this._dialogInst;
            if (!inst) {
                inst = this._dialogInst = new DatepickerInstance({}, false);
                this._dialogInput = $('<input type="text" size="1" style="position: absolute; top: -100px;"/>');
                this._dialogInput.keydown(this._doKeyDown);
                $("body").append(this._dialogInput);
                this._dialogInput[0]._calId = inst._id
            }
            extendRemove(inst._settings, settings || {});
            this._dialogInput.val(dateText);
            this._pos = (pos ? (pos.length ? pos : [pos.pageX, pos.pageY]) : null);
            if (!this._pos) {
                var browserWidth = window.innerWidth || document.documentElement.clientWidth || document.body.clientWidth;
                var browserHeight = window.innerHeight || document.documentElement.clientHeight || document.body.clientHeight;
                var scrollX = document.documentElement.scrollLeft || document.body.scrollLeft;
                var scrollY = document.documentElement.scrollTop || document.body.scrollTop;
                this._pos = [(browserWidth / 2) - 100 + scrollX, (browserHeight / 2) - 150 + scrollY]
            }
            this._dialogInput.css("left", this._pos[0] + "px").css("top", this._pos[1] + "px");
            inst._settings.onSelect = onSelect;
            this._inDialog = true;
            this._datepickerDiv.addClass("datepicker_dialog");
            this._showDatepicker(this._dialogInput[0]);
            if ($.blockUI) {
                $.blockUI(this._datepickerDiv)
            }
            return this
        },
        _showDatepicker: function(input) {
            input = input.target || input;
            if (input.nodeName.toLowerCase() != "input") {
                input = $("input", input.parentNode)[0]
            }
            if ($.datepicker._isDisabledDatepicker(input) || $.datepicker._lastInput == input) {
                return
            }
            var inst = $.datepicker._getInst(input._calId);
            var beforeShow = inst._get("beforeShow");
            extendRemove(inst._settings, (beforeShow ? beforeShow.apply(input, [input, inst]) : {}));
            $.datepicker._hideDatepicker(null, "");
            $.datepicker._lastInput = input;
            inst._setDateFromField(input);
            if ($.datepicker._inDialog) {
                input.value = ""
            }
            if (!$.datepicker._pos) {
                $.datepicker._pos = $.datepicker._findPos(input);
                $.datepicker._pos[1] += input.offsetHeight
            }
            var isFixed = false;
            $(input).parents().each(function() {
                isFixed |= $(this).css("position") == "fixed"
            });
            if (isFixed && $.browser.opera) {
                $.datepicker._pos[0] -= document.documentElement.scrollLeft;
                $.datepicker._pos[1] -= document.documentElement.scrollTop
            }
            inst._datepickerDiv.css("position", ($.datepicker._inDialog && $.blockUI ? "static" : (isFixed ? "fixed" : "absolute"))).css({
                left: $.datepicker._pos[0] + "px",
                top: $.datepicker._pos[1] + "px"
            });
            $.datepicker._pos = null;
            inst._rangeStart = null;
            $.datepicker._updateDatepicker(inst);
            if (!inst._inline) {
                var speed = inst._get("speed");
                var postProcess = function() {
                    $.datepicker._datepickerShowing = true;
                    $.datepicker._afterShow(inst)
                };
                var showAnim = inst._get("showAnim") || "show";
                inst._datepickerDiv[showAnim](speed, postProcess);
                if (speed == "") {
                    postProcess()
                }
                var isVisible = $(":visible", $(inst._input[0])).length > 0;
                if (isVisible) {
                    inst._input[0].focus()
                }
                $.datepicker._curInst = inst
            }
        },
        _updateDatepicker: function(inst) {
            inst._datepickerDiv.empty().append(inst._generateDatepicker());
            var numMonths = inst._getNumberOfMonths();
            if (numMonths[0] != 1 || numMonths[1] != 1) {
                inst._datepickerDiv.addClass("datepicker_multi")
            } else {
                inst._datepickerDiv.removeClass("datepicker_multi")
            }
            if (inst._get("isRTL")) {
                inst._datepickerDiv.addClass("datepicker_rtl")
            } else {
                inst._datepickerDiv.removeClass("datepicker_rtl")
            }
            if (inst._input) {
                var isVisible = $(":visible", $(inst._input[0])).length > 0;
                if (isVisible) {
                    inst._input[0].focus()
                }
            }
        },
        _afterShow: function(inst) {
            var numMonths = inst._getNumberOfMonths();
            if ($.browser.msie && parseInt($.browser.version) < 7) {
                $("#datepicker_cover").css({
                    width: inst._datepickerDiv.width() + 4,
                    height: inst._datepickerDiv.height() + 4
                })
            }
            var isFixed = inst._datepickerDiv.css("position") == "fixed";
            var pos = inst._input ? $.datepicker._findPos(inst._input[0]) : null;
            var browserWidth = window.innerWidth || document.documentElement.clientWidth || document.body.clientWidth;
            var browserHeight = window.innerHeight || document.documentElement.clientHeight || document.body.clientHeight;
            var scrollX = (isFixed ? 0 : document.documentElement.scrollLeft || document.body.scrollLeft);
            var scrollY = (isFixed ? 0 : document.documentElement.scrollTop || document.body.scrollTop);
            if ((inst._datepickerDiv.offset().left + inst._datepickerDiv.width() - (isFixed && $.browser.msie ? document.documentElement.scrollLeft : 0)) > (browserWidth + scrollX)) {
                inst._datepickerDiv.css("left", Math.max(scrollX, pos[0] + (inst._input ? $(inst._input[0]).width() : null) - inst._datepickerDiv.width() - (isFixed && $.browser.opera ? document.documentElement.scrollLeft : 0)) + "px")
            }
            if ((inst._datepickerDiv.offset().top + inst._datepickerDiv.height() - (isFixed && $.browser.msie ? document.documentElement.scrollTop : 0)) > (browserHeight + scrollY)) {
                inst._datepickerDiv.css("top", Math.max(scrollY, pos[1] - (this._inDialog ? 0 : inst._datepickerDiv.height()) - (isFixed && $.browser.opera ? document.documentElement.scrollTop : 0)) + "px")
            }
        },
        _findPos: function(obj) {
            var notTypeHidden = (obj.type != "hidden");
            while (obj && (obj.type == "hidden" || obj.nodeType != 1)) {
                obj = obj.nextSibling
            }
            var jObj = $(obj);
            if (notTypeHidden) {
                jObj.show()
            }
            var position = jObj.offset();
            if (notTypeHidden) {
                jObj.hide()
            }
            return [position.left, position.top]
        },
        _hideDatepicker: function(input, speed) {
            var inst = this._curInst;
            if (!inst) {
                return
            }
            var rangeSelect = inst._get("rangeSelect");
            if (rangeSelect && this._stayOpen) {
                this._selectDate(inst, inst._formatDate(inst._currentDay, inst._currentMonth, inst._currentYear))
            }
            this._stayOpen = false;
            if (this._datepickerShowing) {
                speed = (speed != null ? speed : inst._get("speed"));
                var showAnim = inst._get("showAnim");
                inst._datepickerDiv[(showAnim == "slideDown" ? "slideUp" : (showAnim == "fadeIn" ? "fadeOut" : "hide"))](speed, function() {
                    $.datepicker._tidyDialog(inst)
                });
                if (speed == "") {
                    this._tidyDialog(inst)
                }
                var onClose = inst._get("onClose");
                if (onClose) {
                    onClose.apply((inst._input ? inst._input[0] : null), [inst._getDate(), inst])
                }
                this._datepickerShowing = false;
                this._lastInput = null;
                inst._settings.prompt = null;
                if (this._inDialog) {
                    this._dialogInput.css({
                        position: "absolute",
                        left: "0",
                        top: "-100px"
                    });
                    if ($.blockUI) {
                        $.unblockUI();
                        $("body").append(this._datepickerDiv)
                    }
                }
                this._inDialog = false
            }
            this._curInst = null
        },
        _tidyDialog: function(inst) {
            inst._datepickerDiv.removeClass("datepicker_dialog").unbind(".datepicker");
            $(".datepicker_prompt", inst._datepickerDiv).remove()
        },
        _checkExternalClick: function(event) {
            if (!$.datepicker._curInst) {
                return
            }
            var $target = $(event.target);
            if (($target.parents("#datepicker_div").length == 0) && ($target.attr("class") != "datepicker_trigger") && $.datepicker._datepickerShowing && !($.datepicker._inDialog && $.blockUI)) {
                $.datepicker._hideDatepicker(null, "")
            }
        },
        _adjustDate: function(id, offset, period) {
            var inst = this._getInst(id);
            inst._adjustDate(offset, period);
            this._updateDatepicker(inst)
        },
        _gotoToday: function(id) {
            var date = new Date();
            var inst = this._getInst(id);
            inst._selectedDay = date.getDate();
            inst._drawMonth = inst._selectedMonth = date.getMonth();
            inst._drawYear = inst._selectedYear = date.getFullYear();
            this._adjustDate(inst)
        },
        _selectMonthYear: function(id, select, period) {
            var inst = this._getInst(id);
            inst._selectingMonthYear = false;
            inst[period == "M" ? "_drawMonth" : "_drawYear"] = select.options[select.selectedIndex].value - 0;
            this._adjustDate(inst)
        },
        _clickMonthYear: function(id) {
            var inst = this._getInst(id);
            if (inst._input && inst._selectingMonthYear && !$.browser.msie) {
                inst._input[0].focus()
            }
            inst._selectingMonthYear = !inst._selectingMonthYear
        },
        _changeFirstDay: function(id, day) {
            var inst = this._getInst(id);
            inst._settings.firstDay = day;
            this._updateDatepicker(inst)
        },
        _selectDay: function(id, month, year, td) {
            if ($(td).is(".datepicker_unselectable")) {
                return
            }
            var inst = this._getInst(id);
            var rangeSelect = inst._get("rangeSelect");
            if (rangeSelect) {
                if (!this._stayOpen) {
                    $(".datepicker td").removeClass("datepicker_currentDay");
                    $(td).addClass("datepicker_currentDay")
                }
                this._stayOpen = !this._stayOpen
            }
            inst._selectedDay = inst._currentDay = $("a", td).html();
            inst._selectedMonth = inst._currentMonth = month;
            inst._selectedYear = inst._currentYear = year;
            this._selectDate(id, inst._formatDate(inst._currentDay, inst._currentMonth, inst._currentYear));
            if (this._stayOpen) {
                inst._endDay = inst._endMonth = inst._endYear = null;
                inst._rangeStart = new Date(inst._currentYear, inst._currentMonth, inst._currentDay);
                this._updateDatepicker(inst)
            } else {
                if (rangeSelect) {
                    inst._endDay = inst._currentDay;
                    inst._endMonth = inst._currentMonth;
                    inst._endYear = inst._currentYear;
                    inst._selectedDay = inst._currentDay = inst._rangeStart.getDate();
                    inst._selectedMonth = inst._currentMonth = inst._rangeStart.getMonth();
                    inst._selectedYear = inst._currentYear = inst._rangeStart.getFullYear();
                    inst._rangeStart = null;
                    if (inst._inline) {
                        this._updateDatepicker(inst)
                    }
                }
            }
        },
        _clearDate: function(id) {
            var inst = this._getInst(id);
            if (inst._get("mandatory")) {
                return
            }
            this._stayOpen = false;
            inst._endDay = inst._endMonth = inst._endYear = inst._rangeStart = null;
            this._selectDate(inst, "")
        },
        _selectDate: function(id, dateStr) {
            var inst = this._getInst(id);
            dateStr = (dateStr != null ? dateStr : inst._formatDate());
            if (inst._rangeStart) {
                dateStr = inst._formatDate(inst._rangeStart) + inst._get("rangeSeparator") + dateStr
            }
            if (inst._input) {
                inst._input.val(dateStr)
            }
            var onSelect = inst._get("onSelect");
            if (onSelect) {
                onSelect.apply((inst._input ? inst._input[0] : null), [dateStr, inst])
            } else {
                if (inst._input) {
                    inst._input.trigger("change")
                }
            }
            if (inst._inline) {
                this._updateDatepicker(inst)
            } else {
                if (!this._stayOpen) {
                    this._hideDatepicker(null, inst._get("speed"));
                    this._lastInput = inst._input[0];
                    if (typeof(inst._input[0]) != "object") {
                        inst._input[0].focus()
                    }
                    this._lastInput = null
                }
            }
        },
        noWeekends: function(date) {
            var day = date.getDay();
            return [(day > 0 && day < 6), ""]
        },
        iso8601Week: function(date) {
            var checkDate = new Date(date.getFullYear(), date.getMonth(), date.getDate(), (date.getTimezoneOffset() / -60));
            var firstMon = new Date(checkDate.getFullYear(), 1 - 1, 4);
            var firstDay = firstMon.getDay() || 7;
            firstMon.setDate(firstMon.getDate() + 1 - firstDay);
            if (firstDay < 4 && checkDate < firstMon) {
                checkDate.setDate(checkDate.getDate() - 3);
                return $.datepicker.iso8601Week(checkDate)
            } else {
                if (checkDate > new Date(checkDate.getFullYear(), 12 - 1, 28)) {
                    firstDay = new Date(checkDate.getFullYear() + 1, 1 - 1, 4).getDay() || 7;
                    if (firstDay > 4 && (checkDate.getDay() || 7) < firstDay - 3) {
                        checkDate.setDate(checkDate.getDate() + 3);
                        return $.datepicker.iso8601Week(checkDate)
                    }
                }
            }
            return Math.floor(((checkDate - firstMon) / 86400000) / 7) + 1
        },
        dateStatus: function(date, inst) {
            return $.datepicker.formatDate(inst._get("dateStatus"), date, inst._getFormatConfig())
        },
        parseDate: function(format, value, settings) {
            if (format == null || value == null) {
                throw "Invalid arguments"
            }
            value = (typeof value == "object" ? value.toString() : value + "");
            if (value == "") {
                return null
            }
            var shortYearCutoff = (settings ? settings.shortYearCutoff : null) || this._defaults.shortYearCutoff;
            var dayNamesShort = (settings ? settings.dayNamesShort : null) || this._defaults.dayNamesShort;
            var dayNames = (settings ? settings.dayNames : null) || this._defaults.dayNames;
            var monthNamesShort = (settings ? settings.monthNamesShort : null) || this._defaults.monthNamesShort;
            var monthNames = (settings ? settings.monthNames : null) || this._defaults.monthNames;
            var year = -1;
            var month = -1;
            var day = -1;
            var literal = false;
            var lookAhead = function(match) {
                var matches = (iFormat + 1 < format.length && format.charAt(iFormat + 1) == match);
                if (matches) {
                    iFormat++
                }
                return matches
            };
            var getNumber = function(match) {
                lookAhead(match);
                var size = (match == "y" ? 4 : 2);
                var num = 0;
                while (size > 0 && iValue < value.length && value.charAt(iValue) >= "0" && value.charAt(iValue) <= "9") {
                    num = num * 10 + (value.charAt(iValue++) - 0);
                    size--
                }
                if (size == (match == "y" ? 4 : 2)) {
                    throw "Missing number at position " + iValue
                }
                return num
            };
            var getName = function(match, shortNames, longNames) {
                var names = (lookAhead(match) ? longNames : shortNames);
                var size = 0;
                for (var j = 0; j < names.length; j++) {
                    size = Math.max(size, names[j].length)
                }
                var name = "";
                var iInit = iValue;
                while (size > 0 && iValue < value.length) {
                    name += value.charAt(iValue++);
                    for (var i = 0; i < names.length; i++) {
                        if (name == names[i]) {
                            return i + 1
                        }
                    }
                    size--
                }
                throw "Unknown name at position " + iInit
            };
            var checkLiteral = function() {
                if (value.charAt(iValue) != format.charAt(iFormat)) {
                    throw "Unexpected literal at position " + iValue
                }
                iValue++
            };
            var iValue = 0;
            for (var iFormat = 0; iFormat < format.length; iFormat++) {
                if (literal) {
                    if (format.charAt(iFormat) == "'" && !lookAhead("'")) {
                        literal = false
                    } else {
                        checkLiteral()
                    }
                } else {
                    switch (format.charAt(iFormat)) {
                        case "d":
                            day = getNumber("d");
                            break;
                        case "D":
                            getName("D", dayNamesShort, dayNames);
                            break;
                        case "m":
                            month = getNumber("m");
                            break;
                        case "M":
                            month = getName("M", monthNamesShort, monthNames);
                            break;
                        case "y":
                            year = getNumber("y");
                            break;
                        case "'":
                            if (lookAhead("'")) {
                                checkLiteral()
                            } else {
                                literal = true
                            }
                            break;
                        default:
                            checkLiteral()
                    }
                }
            }
            if (year < 100) {
                year += new Date().getFullYear() - new Date().getFullYear() % 100 + (year <= shortYearCutoff ? 0 : -100)
            }
            var date = new Date(year, month - 1, day);
            if (date.getFullYear() != year || date.getMonth() + 1 != month || date.getDate() != day) {
                throw "Invalid date"
            }
            return date
        },
        formatDate: function(format, date, settings) {
            if (!date) {
                return ""
            }
            var dayNamesShort = (settings ? settings.dayNamesShort : null) || this._defaults.dayNamesShort;
            var dayNames = (settings ? settings.dayNames : null) || this._defaults.dayNames;
            var monthNamesShort = (settings ? settings.monthNamesShort : null) || this._defaults.monthNamesShort;
            var monthNames = (settings ? settings.monthNames : null) || this._defaults.monthNames;
            var lookAhead = function(match) {
                var matches = (iFormat + 1 < format.length && format.charAt(iFormat + 1) == match);
                if (matches) {
                    iFormat++
                }
                return matches
            };
            var formatNumber = function(match, value) {
                return (lookAhead(match) && value < 10 ? "0" : "") + value
            };
            var formatName = function(match, value, shortNames, longNames) {
                return (lookAhead(match) ? longNames[value] : shortNames[value])
            };
            var output = "";
            var literal = false;
            if (date) {
                for (var iFormat = 0; iFormat < format.length; iFormat++) {
                    if (literal) {
                        if (format.charAt(iFormat) == "'" && !lookAhead("'")) {
                            literal = false
                        } else {
                            output += format.charAt(iFormat)
                        }
                    } else {
                        switch (format.charAt(iFormat)) {
                            case "d":
                                output += formatNumber("d", date.getDate());
                                break;
                            case "D":
                                output += formatName("D", date.getDay(), dayNamesShort, dayNames);
                                break;
                            case "m":
                                output += formatNumber("m", date.getMonth() + 1);
                                break;
                            case "M":
                                output += formatName("M", date.getMonth(), monthNamesShort, monthNames);
                                break;
                            case "y":
                                output += (lookAhead("y") ? date.getFullYear() : (date.getYear() % 100 < 10 ? "0" : "") + date.getYear() % 100);
                                break;
                            case "'":
                                if (lookAhead("'")) {
                                    output += "'"
                                } else {
                                    literal = true
                                }
                                break;
                            default:
                                output += format.charAt(iFormat)
                        }
                    }
                }
            }
            return output
        },
        _possibleChars: function(format) {
            var chars = "";
            var literal = false;
            for (var iFormat = 0; iFormat < format.length; iFormat++) {
                if (literal) {
                    if (format.charAt(iFormat) == "'" && !lookAhead("'")) {
                        literal = false
                    } else {
                        chars += format.charAt(iFormat)
                    }
                } else {
                    switch (format.charAt(iFormat)) {
                        case "d" || "m" || "y":
                            chars += "0123456789";
                            break;
                        case "D" || "M":
                            return null;
                        case "'":
                            if (lookAhead("'")) {
                                chars += "'"
                            } else {
                                literal = true
                            }
                            break;
                        default:
                            chars += format.charAt(iFormat)
                    }
                }
            }
            return chars
        }
    });

    function DatepickerInstance(settings, inline) {
        this._id = $.datepicker._register(this);
        this._selectedDay = 0;
        this._selectedMonth = 0;
        this._selectedYear = 0;
        this._drawMonth = 0;
        this._drawYear = 0;
        this._input = null;
        this._inline = inline;
        this._datepickerDiv = (!inline ? $.datepicker._datepickerDiv : $('<div id="datepicker_div_' + this._id + '" class="datepicker_inline">'));
        this._settings = extendRemove(settings || {});
        if (inline) {
            this._setDate(this._getDefaultDate())
        }
    }
    $.extend(DatepickerInstance.prototype, {
        _get: function(name) {
            return this._settings[name] || $.datepicker._defaults[name]
        },
        _setDateFromField: function(input) {
            this._input = $(input);
            var dateFormat = this._get("dateFormat");
            var dates = this._input ? this._input.val().split(this._get("rangeSeparator")) : null;
            this._endDay = this._endMonth = this._endYear = null;
            var date = defaultDate = this._getDefaultDate();
            if (dates.length > 0) {
                var settings = this._getFormatConfig();
                if (dates.length > 1) {
                    date = $.datepicker.parseDate(dateFormat, dates[1], settings) || defaultDate;
                    this._endDay = date.getDate();
                    this._endMonth = date.getMonth();
                    this._endYear = date.getFullYear()
                }
                try {
                    date = $.datepicker.parseDate(dateFormat, dates[0], settings) || defaultDate
                } catch (e) {
                    $.datepicker.log(e);
                    date = defaultDate
                }
            }
            this._selectedDay = date.getDate();
            this._drawMonth = this._selectedMonth = date.getMonth();
            this._drawYear = this._selectedYear = date.getFullYear();
            this._currentDay = (dates[0] ? date.getDate() : 0);
            this._currentMonth = (dates[0] ? date.getMonth() : 0);
            this._currentYear = (dates[0] ? date.getFullYear() : 0);
            this._adjustDate()
        },
        _getDefaultDate: function() {
            var date = this._determineDate("defaultDate", new Date());
            var minDate = this._getMinMaxDate("min", true);
            var maxDate = this._getMinMaxDate("max");
            date = (minDate && date < minDate ? minDate : date);
            date = (maxDate && date > maxDate ? maxDate : date);
            return date
        },
        _determineDate: function(name, defaultDate) {
            var offsetNumeric = function(offset) {
                var date = new Date();
                date.setDate(date.getDate() + offset);
                return date
            };
            var offsetString = function(offset, getDaysInMonth) {
                var date = new Date();
                var matches = /^([+-]?[0-9]+)\s*(d|D|w|W|m|M|y|Y)?$/.exec(offset);
                if (matches) {
                    var year = date.getFullYear();
                    var month = date.getMonth();
                    var day = date.getDate();
                    switch (matches[2] || "d") {
                        case "d":
                        case "D":
                            day += (matches[1] - 0);
                            break;
                        case "w":
                        case "W":
                            day += (matches[1] * 7);
                            break;
                        case "m":
                        case "M":
                            month += (matches[1] - 0);
                            day = Math.min(day, getDaysInMonth(year, month));
                            break;
                        case "y":
                        case "Y":
                            year += (matches[1] - 0);
                            day = Math.min(day, getDaysInMonth(year, month));
                            break
                    }
                    date = new Date(year, month, day)
                }
                return date
            };
            var date = this._get(name);
            return (date == null ? defaultDate : (typeof date == "string" ? offsetString(date, this._getDaysInMonth) : (typeof date == "number" ? offsetNumeric(date) : date)))
        },
        _setDate: function(date, endDate) {
            this._selectedDay = this._currentDay = date.getDate();
            this._drawMonth = this._selectedMonth = this._currentMonth = date.getMonth();
            this._drawYear = this._selectedYear = this._currentYear = date.getFullYear();
            if (this._get("rangeSelect")) {
                if (endDate) {
                    this._endDay = endDate.getDate();
                    this._endMonth = endDate.getMonth();
                    this._endYear = endDate.getFullYear()
                } else {
                    this._endDay = this._currentDay;
                    this._endMonth = this._currentMonth;
                    this._endYear = this._currentYear
                }
            }
            this._adjustDate()
        },
        _getDate: function() {
            var startDate = (!this._currentYear || (this._input && this._input.val() == "") ? null : new Date(this._currentYear, this._currentMonth, this._currentDay));
            if (this._get("rangeSelect")) {
                return [startDate, (!this._endYear ? null : new Date(this._endYear, this._endMonth, this._endDay))]
            } else {
                return startDate
            }
        },
        _generateDatepicker: function() {
            var today = new Date();
            today = new Date(today.getFullYear(), today.getMonth(), today.getDate());
            var showStatus = this._get("showStatus");
            var isRTL = this._get("isRTL");
            var clear = (this._get("mandatory") ? "" : '<div class="datepicker_clear"><a onclick="jQuery.datepicker._clearDate(' + this._id + ');"' + (showStatus ? this._addStatus(this._get("clearStatus") || "&#xa0;") : "") + ">" + this._get("clearText") + "</a></div>");
            var controls = '<div class="datepicker_control">' + (isRTL ? "" : clear) + '<div class="datepicker_close"><a onclick="jQuery.datepicker._hideDatepicker();"' + (showStatus ? this._addStatus(this._get("closeStatus") || "&#xa0;") : "") + ">" + this._get("closeText") + "</a></div>" + (isRTL ? clear : "") + "</div>";
            var prompt = this._get("prompt");
            var closeAtTop = this._get("closeAtTop");
            var hideIfNoPrevNext = this._get("hideIfNoPrevNext");
            var numMonths = this._getNumberOfMonths();
            var stepMonths = this._get("stepMonths");
            var isMultiMonth = (numMonths[0] != 1 || numMonths[1] != 1);
            var minDate = this._getMinMaxDate("min", true);
            var maxDate = this._getMinMaxDate("max");
            var drawMonth = this._drawMonth;
            var drawYear = this._drawYear;
            if (maxDate) {
                var maxDraw = new Date(maxDate.getFullYear(), maxDate.getMonth() - numMonths[1] + 1, maxDate.getDate());
                maxDraw = (minDate && maxDraw < minDate ? minDate : maxDraw);
                while (new Date(drawYear, drawMonth, 1) > maxDraw) {
                    drawMonth--;
                    if (drawMonth < 0) {
                        drawMonth = 11;
                        drawYear--
                    }
                }
            }
            var prev = '<div class="datepicker_prev">' + (this._canAdjustMonth(-1, drawYear, drawMonth) ? '<a onclick="jQuery.datepicker._adjustDate(' + this._id + ", -" + stepMonths + ", 'M');\"" + (showStatus ? this._addStatus(this._get("prevStatus") || "&#xa0;") : "") + ">" + this._get("prevText") + "</a>" : (hideIfNoPrevNext ? "" : "<label>" + this._get("prevText") + "</label>")) + "</div>";
            var next = '<div class="datepicker_next">' + (this._canAdjustMonth(+1, drawYear, drawMonth) ? '<a onclick="jQuery.datepicker._adjustDate(' + this._id + ", +" + stepMonths + ", 'M');\"" + (showStatus ? this._addStatus(this._get("nextStatus") || "&#xa0;") : "") + ">" + this._get("nextText") + "</a>" : (hideIfNoPrevNext ? ">" : "<label>" + this._get("nextText") + "</label>")) + "</div>";
            var html = (prompt ? '<div class="datepicker_prompt">' + prompt + "</div>" : "") + (closeAtTop && !this._inline ? controls : "") + '<div class="datepicker_links">' + (isRTL ? next : prev) + (this._isInRange(today) ? '<div class="datepicker_current"><a onclick="jQuery.datepicker._gotoToday(' + this._id + ');"' + (showStatus ? this._addStatus(this._get("currentStatus") || "&#xa0;") : "") + ">" + this._get("currentText") + "</a></div>" : "") + (isRTL ? prev : next) + "</div>";
            var showWeeks = this._get("showWeeks");
            for (var row = 0; row < numMonths[0]; row++) {
                for (var col = 0; col < numMonths[1]; col++) {
                    var selectedDate = new Date(drawYear, drawMonth, this._selectedDay);
                    html += '<div class="datepicker_oneMonth' + (col == 0 ? " datepicker_newRow" : "") + '">' + this._generateMonthYearHeader(drawMonth, drawYear, minDate, maxDate, selectedDate, row > 0 || col > 0) + '<table class="datepicker" cellpadding="0" cellspacing="0"><thead><tr class="datepicker_titleRow">' + (showWeeks ? "<td>" + this._get("weekHeader") + "</td>" : "");
                    var firstDay = this._get("firstDay");
                    var changeFirstDay = this._get("changeFirstDay");
                    var dayNames = this._get("dayNames");
                    var dayNamesShort = this._get("dayNamesShort");
                    var dayNamesMin = this._get("dayNamesMin");
                    for (var dow = 0; dow < 7; dow++) {
                        var day = (dow + firstDay) % 7;
                        var status = this._get("dayStatus") || "&#xa0;";
                        status = (status.indexOf("DD") > -1 ? status.replace(/DD/, dayNames[day]) : status.replace(/D/, dayNamesShort[day]));
                        html += "<td" + ((dow + firstDay + 6) % 7 >= 5 ? ' class="datepicker_weekEndCell"' : "") + ">" + (!changeFirstDay ? "<span" : '<a onclick="jQuery.datepicker._changeFirstDay(' + this._id + ", " + day + ');"') + (showStatus ? this._addStatus(status) : "") + ' title="' + dayNames[day] + '">' + dayNamesMin[day] + (changeFirstDay ? "</a>" : "</span>") + "</td>"
                    }
                    html += "</tr></thead><tbody>";
                    var daysInMonth = this._getDaysInMonth(drawYear, drawMonth);
                    if (drawYear == this._selectedYear && drawMonth == this._selectedMonth) {
                        this._selectedDay = Math.min(this._selectedDay, daysInMonth)
                    }
                    var leadDays = (this._getFirstDayOfMonth(drawYear, drawMonth) - firstDay + 7) % 7;
                    var currentDate = (!this._currentDay ? new Date(9999, 9, 9) : new Date(this._currentYear, this._currentMonth, this._currentDay));
                    var endDate = this._endDay ? new Date(this._endYear, this._endMonth, this._endDay) : currentDate;
                    var printDate = new Date(drawYear, drawMonth, 1 - leadDays);
                    var numRows = (isMultiMonth ? 6 : Math.ceil((leadDays + daysInMonth) / 7));
                    var beforeShowDay = this._get("beforeShowDay");
                    var showOtherMonths = this._get("showOtherMonths");
                    var calculateWeek = this._get("calculateWeek") || $.datepicker.iso8601Week;
                    var dateStatus = this._get("statusForDate") || $.datepicker.dateStatus;
                    for (var dRow = 0; dRow < numRows; dRow++) {
                        html += '<tr class="datepicker_daysRow">' + (showWeeks ? '<td class="datepicker_weekCol">' + calculateWeek(printDate) + "</td>" : "");
                        for (var dow = 0; dow < 7; dow++) {
                            var daySettings = (beforeShowDay ? beforeShowDay.apply((this._input ? this._input[0] : null), [printDate]) : [true, ""]);
                            var otherMonth = (printDate.getMonth() != drawMonth);
                            var unselectable = otherMonth || !daySettings[0] || (minDate && printDate < minDate) || (maxDate && printDate > maxDate);
                            html += '<td class="datepicker_daysCell' + ((dow + firstDay + 6) % 7 >= 5 ? " datepicker_weekEndCell" : "") + (otherMonth ? " datepicker_otherMonth" : "") + (printDate.getTime() == selectedDate.getTime() && drawMonth == this._selectedMonth ? " datepicker_daysCellOver" : "") + (unselectable ? " datepicker_unselectable" : "") + (otherMonth && !showOtherMonths ? "" : " " + daySettings[1] + (printDate.getTime() >= currentDate.getTime() && printDate.getTime() <= endDate.getTime() ? " datepicker_currentDay" : "") + (printDate.getTime() == today.getTime() ? " datepicker_today" : "")) + '"' + (unselectable ? "" : " onmouseover=\"jQuery(this).addClass('datepicker_daysCellOver');" + (!showStatus || (otherMonth && !showOtherMonths) ? "" : "jQuery('#datepicker_status_" + this._id + "').html('" + (dateStatus.apply((this._input ? this._input[0] : null), [printDate, this]) || "&#xa0;") + "');") + "\" onmouseout=\"jQuery(this).removeClass('datepicker_daysCellOver');" + (!showStatus || (otherMonth && !showOtherMonths) ? "" : "jQuery('#datepicker_status_" + this._id + "').html('&#xa0;');") + '" onclick="jQuery.datepicker._selectDay(' + this._id + "," + drawMonth + "," + drawYear + ', this);"') + ">" + (otherMonth ? (showOtherMonths ? printDate.getDate() : "&#xa0;") : (unselectable ? printDate.getDate() : "<a>" + printDate.getDate() + "</a>")) + "</td>";
                            printDate.setDate(printDate.getDate() + 1)
                        }
                        html += "</tr>"
                    }
                    drawMonth++;
                    if (drawMonth > 11) {
                        drawMonth = 0;
                        drawYear++
                    }
                    html += "</tbody></table></div>"
                }
            }
            html += (showStatus ? '<div id="datepicker_status_' + this._id + '" class="datepicker_status">' + (this._get("initStatus") || "&#xa0;") + "</div>" : "") + (!closeAtTop && !this._inline ? controls : "") + '<div style="clear: both;"></div>' + ($.browser.msie && parseInt($.browser.version) < 7 && !this._inline ? '<iframe src="javascript:false;" class="datepicker_cover"></iframe>' : "");
            return html
        },
        _generateMonthYearHeader: function(drawMonth, drawYear, minDate, maxDate, selectedDate, secondary) {
            minDate = (this._rangeStart && minDate && selectedDate < minDate ? selectedDate : minDate);
            var showStatus = this._get("showStatus");
            var html = '<div class="datepicker_header">';
            var monthNames = this._get("monthNames");
            if (secondary || !this._get("changeMonth")) {
                html += monthNames[drawMonth] + "&#xa0;"
            } else {
                var inMinYear = (minDate && minDate.getFullYear() == drawYear);
                var inMaxYear = (maxDate && maxDate.getFullYear() == drawYear);
                html += '<select class="datepicker_newMonth" onchange="jQuery.datepicker._selectMonthYear(' + this._id + ", this, 'M');\" onclick=\"jQuery.datepicker._clickMonthYear(" + this._id + ');"' + (showStatus ? this._addStatus(this._get("monthStatus") || "&#xa0;") : "") + ">";
                for (var month = 0; month < 12; month++) {
                    if ((!inMinYear || month >= minDate.getMonth()) && (!inMaxYear || month <= maxDate.getMonth())) {
                        html += '<option value="' + month + '"' + (month == drawMonth ? ' selected="selected"' : "") + ">" + monthNames[month] + "</option>"
                    }
                }
                html += "</select>"
            }
            if (secondary || !this._get("changeYear")) {
                html += drawYear
            } else {
                var years = this._get("yearRange").split(":");
                var year = 0;
                var endYear = 0;
                if (years.length != 2) {
                    year = drawYear - 10;
                    endYear = drawYear + 10
                } else {
                    if (years[0].charAt(0) == "+" || years[0].charAt(0) == "-") {
                        year = drawYear + parseInt(years[0], 10);
                        endYear = drawYear + parseInt(years[1], 10)
                    } else {
                        year = parseInt(years[0], 10);
                        endYear = parseInt(years[1], 10)
                    }
                }
                year = (minDate ? Math.max(year, minDate.getFullYear()) : year);
                endYear = (maxDate ? Math.min(endYear, maxDate.getFullYear()) : endYear);
                html += '<select class="datepicker_newYear" onchange="jQuery.datepicker._selectMonthYear(' + this._id + ", this, 'Y');\" onclick=\"jQuery.datepicker._clickMonthYear(" + this._id + ');"' + (showStatus ? this._addStatus(this._get("yearStatus") || "&#xa0;") : "") + ">";
                for (; year <= endYear; year++) {
                    html += '<option value="' + year + '"' + (year == drawYear ? ' selected="selected"' : "") + ">" + year + "</option>"
                }
                html += "</select>"
            }
            html += "</div>";
            return html
        },
        _addStatus: function(text) {
            return " onmouseover=\"jQuery('#datepicker_status_" + this._id + "').html('" + text + "');\" onmouseout=\"jQuery('#datepicker_status_" + this._id + "').html('&#xa0;');\""
        },
        _adjustDate: function(offset, period) {
            var year = this._drawYear + (period == "Y" ? offset : 0);
            var month = this._drawMonth + (period == "M" ? offset : 0);
            var day = Math.min(this._selectedDay, this._getDaysInMonth(year, month)) + (period == "D" ? offset : 0);
            var date = new Date(year, month, day);
            var minDate = this._getMinMaxDate("min", true);
            var maxDate = this._getMinMaxDate("max");
            date = (minDate && date < minDate ? minDate : date);
            date = (maxDate && date > maxDate ? maxDate : date);
            this._selectedDay = date.getDate();
            this._drawMonth = this._selectedMonth = date.getMonth();
            this._drawYear = this._selectedYear = date.getFullYear()
        },
        _getNumberOfMonths: function() {
            var numMonths = this._get("numberOfMonths");
            return (numMonths == null ? [1, 1] : (typeof numMonths == "number" ? [1, numMonths] : numMonths))
        },
        _getMinMaxDate: function(minMax, checkRange) {
            var date = this._determineDate(minMax + "Date", null);
            if (date) {
                date.setHours(0);
                date.setMinutes(0);
                date.setSeconds(0);
                date.setMilliseconds(0)
            }
            return date || (checkRange ? this._rangeStart : null)
        },
        _getDaysInMonth: function(year, month) {
            return 32 - new Date(year, month, 32).getDate()
        },
        _getFirstDayOfMonth: function(year, month) {
            return new Date(year, month, 1).getDay()
        },
        _canAdjustMonth: function(offset, curYear, curMonth) {
            var numMonths = this._getNumberOfMonths();
            var date = new Date(curYear, curMonth + (offset < 0 ? offset : numMonths[1]), 1);
            if (offset < 0) {
                date.setDate(this._getDaysInMonth(date.getFullYear(), date.getMonth()))
            }
            return this._isInRange(date)
        },
        _isInRange: function(date) {
            var newMinDate = (!this._rangeStart ? null : new Date(this._selectedYear, this._selectedMonth, this._selectedDay));
            newMinDate = (newMinDate && this._rangeStart < newMinDate ? this._rangeStart : newMinDate);
            var minDate = newMinDate || this._getMinMaxDate("min");
            var maxDate = this._getMinMaxDate("max");
            return ((!minDate || date >= minDate) && (!maxDate || date <= maxDate))
        },
        _getFormatConfig: function() {
            var shortYearCutoff = this._get("shortYearCutoff");
            shortYearCutoff = (typeof shortYearCutoff != "string" ? shortYearCutoff : new Date().getFullYear() % 100 + parseInt(shortYearCutoff, 10));
            return {
                shortYearCutoff: shortYearCutoff,
                dayNamesShort: this._get("dayNamesShort"),
                dayNames: this._get("dayNames"),
                monthNamesShort: this._get("monthNamesShort"),
                monthNames: this._get("monthNames")
            }
        },
        _formatDate: function(day, month, year) {
            if (!day) {
                this._currentDay = this._selectedDay;
                this._currentMonth = this._selectedMonth;
                this._currentYear = this._selectedYear
            }
            var date = (day ? (typeof day == "object" ? day : new Date(year, month, day)) : new Date(this._currentYear, this._currentMonth, this._currentDay));
            return $.datepicker.formatDate(this._get("dateFormat"), date, this._getFormatConfig())
        }
    });

    function extendRemove(target, props) {
        $.extend(target, props);
        for (var name in props) {
            if (props[name] == null) {
                target[name] = null
            }
        }
        return target
    }
    $.fn.datepicker = function(options) {
        var otherArgs = Array.prototype.slice.call(arguments, 1);
        if (typeof options == "string" && (options == "isDisabled" || options == "getDate")) {
            return $.datepicker["_" + options + "Datepicker"].apply($.datepicker, [this[0]].concat(otherArgs))
        }
        return this.each(function() {
            typeof options == "string" ? $.datepicker["_" + options + "Datepicker"].apply($.datepicker, [this].concat(otherArgs)) : $.datepicker._attachDatepicker(this, options)
        })
    };
    $(document).ready(function() {
        $(document.body).append($.datepicker._datepickerDiv).mousedown($.datepicker._checkExternalClick)
    });
    $.datepicker = new Datepicker()
})(jQuery);
(function($) {
    $.metaobjects = function(options) {
        options = $.extend({
            context: document,
            clean: true,
            selector: "object.metaobject"
        }, options);

        function jsValue(value, name) {
            if (name == "regex") {
                value = escapeRegex(value)
            } else {
                value = escapeValue(value)
            }
            eval("value = " + value + ";");
            return value
        }

        function escapeValue(value) {
            if (value.match(/^'.*'$/)) {
                value = value.replace(/'/g, "\\'");
                value = value.replace(/^\\\'/, "'");
                value = value.replace(/\\\'$/, "'")
            }
            return value
        }

        function escapeRegex(value) {
            if (value.match(/^'.*'$/)) {
                value = value.replace(/^'/, "/");
                value = value.replace(/'$/, "/")
            }
            return value
        }
        return $(options.selector, options.context).each(function() {
            var settings = {
                target: this.parentNode
            };
            $("> param[@name=metaparam]", this).each(function() {
                $.extend(settings, jsValue(this.value))
            });
            $("> param", this).not("[@name=metaparam]").each(function() {
                var type = $(this).attr("type");
                var name = this.name;
                var value = jsValue(this.value, name);
                $(settings.target).each(function() {
                    this[name] = value
                })
            });
            if (options.clean) {
                $(this).remove()
            }
        })
    }
})(jQuery); /*! This file is part of the Navitaire NewSkies application. Copyright (C) Navitaire. All rights reserved. */
SKYSALES = {};
SKYSALES.Json = window.JSON;
SKYSALES.Resource = {};
SKYSALES.Util = {};
SKYSALES.Class = {};
SKYSALES.Instance = {};
SKYSALES.Instance.index = 0;
SKYSALES.Instance.getNextIndex = function() {
    SKYSALES.Instance.index += 1;
    return SKYSALES.Instance.index
};
if (!SKYSALES.Class.LocaleCurrency) {
    SKYSALES.Class.LocaleCurrency = function() {
        var t = new SKYSALES.Class.SkySales();
        var e = SKYSALES.Util.extendObject(t);
        e.num = null;
        e.localeCurrency = null;
        var f = SKYSALES.Util.getResource();
        var c = f.currencyCultureInfo;
        var s = 0;
        var q = "";
        var b = "";
        var l = "";
        var n = true;
        var a = function() {
            var v = "n";
            if (!n) {
                v = "(n)"
            }
            return v
        };
        var m = function(B) {
            var y = c.groupSizes || [];
            var w = c.groupSeparator;
            var z = 0;
            var C = 0;
            var A = 3;
            if (z > y.length) {
                A = y[z]
            }
            var G = A - 1;
            s = Math.floor(B);
            var H = s.toString();
            var D = H.split("");
            var E = D.reverse();
            var v = [];
            var F = function() {
                var I = 3;
                if (z <= y.length - 2) {
                    z += 1;
                    I = y[z]
                } else {
                    I = A
                }
                G += I;
                return I
            };
            for (C = 0; C < E.length; C += 1) {
                if (C > G) {
                    A = F();
                    v.push(w)
                }
                v.push(E[C])
            }
            D = v.reverse();
            var x = D.join("");
            return x
        };
        var p = function(y) {
            var w = y - s;
            var v = w.toFixed(c.decimalDigits);
            var x = v.substring(2);
            return x
        };
        var u = function() {
            var w = a() || "";
            var v = w.replace("n", l);
            return v
        };
        var r = function() {
            e.currency = e.num.toString();
            n = e.num >= 0;
            e.num = Math.abs(e.num);
            q = m(e.num);
            b = p(e.num);
            l = q;
            if (0 < c.decimalDigits) {
                l += c.decimalSeparator
            }
            l += b;
            e.currency = u()
        };
        e.init = function(v) {
            this.setSettingsByObject(v);
            if (null !== this.num) {
                r()
            }
        };
        return e
    }
}
SKYSALES.Class.Resource = function() {
    var b = new SKYSALES.Class.SkySales();
    var a = SKYSALES.Util.extendObject(b);
    a.locationInfo = {};
    a.countryInfo = {};
    a.titleInfo = {};
    a.stateInfo = {};
    a.stationInfo = {};
    a.macInfo = {};
    a.marketInfo = {};
    a.macHash = {};
    a.stationHash = {};
    a.macStationHash = {};
    a.countryStationHash = {};
    a.countryHash = {};
    a.marketHash = {};
    a.sourceInfo = {};
    a.clientHash = {};
    a.dateCultureInfo = {};
    a.currencyCultureInfo = {};
    a.populateMacHash = function() {
        var c = 0;
        var e = [];
        var f = {};
        var l = null;
        if (a.macInfo && a.macInfo.MacList) {
            e = a.macInfo.MacList;
            for (c = 0; c < e.length; c += 1) {
                l = e[c];
                f[l.code] = l
            }
        }
        a.macHash = f
    };
    a.populateStationHash = function() {
        var e = 0;
        var l = [];
        var n = {};
        var f = null;
        var m = {};
        var c = a.countryHash;
        if (a.stationInfo && a.stationInfo.StationList) {
            l = a.stationInfo.StationList;
            for (e = 0; e < l.length; e += 1) {
                f = l[e];
                n[f.code] = f;
                stationCountryCode = f.countryCode || "";
                if (stationCountryCode !== "") {
                    m[stationCountryCode] = m[stationCountryCode] || [];
                    m[stationCountryCode][m[stationCountryCode].length] = f;
                    m[stationCountryCode].name = c[stationCountryCode]
                }
            }
        }
        a.stationHash = n;
        a.countryStationHash = m
    };
    a.populateCountryHash = function() {
        var e = 0;
        var c = {};
        var f = [];
        var l = null;
        if (a.countryInfo && a.countryInfo.CountryList) {
            f = a.countryInfo.CountryList;
            for (e = 0; e < f.length; e += 1) {
                l = f[e];
                c[l.code] = l.name
            }
        }
        a.countryHash = c
    };
    a.populateMarketHash = function() {
        var f = 0;
        var q = {};
        var c = [];
        var e = {};
        var n = 0;
        var p = [];
        var r = "";
        var l = {};
        var m = {};
        if (a.marketInfo && a.marketInfo.MarketList) {
            c = a.marketInfo.MarketList;
            for (f = 0; f < c.length; f += 1) {
                e = c[f];
                p = e.Value;
                if (p) {
                    q[e.Key] = p;
                    for (n = 0; n < p.length; n += 1) {
                        l = p[n];
                        r = l.code;
                        l.name = "";
                        m = a.stationHash[r];
                        if (m) {
                            l.name = m.name
                        }
                    }
                }
            }
            a.marketHash = q
        }
    };
    a.populateClientHash = function() {
        var m = window.document.cookie;
        var e = [];
        var l = 0;
        var c = "";
        var f = "";
        var n = "";
        var p = -1;
        if (m) {
            e = document.cookie.split("; ");
            for (l = 0; l < e.length; l += 1) {
                c = e[l];
                p = c.indexOf("=");
                if (p > -1) {
                    f = c.substring(0, p);
                    n = c.substring(p + 1, c.length);
                    if (f) {
                        n = SKYSALES.Util.decodeUriComponent(n);
                        a.clientHash[f] = n
                    }
                }
            }
        }
    };
    a.sortCountryStationHash = function(e, c) {
        return (e.name > c.name)
    };
    a.joinMacStations = function() {
        if (SKYSALES.joinMacStations != "false") {
            for (sIdx in a.stationHash) {
                if (a.stationHash[sIdx].macCode != "") {
                    if (!a.stationHash[a.stationHash[sIdx].macCode]) {
                        macStation = {
                            countryCode: a.stationHash[sIdx].countryCode,
                            macCode: "",
                            name: a.macHash[a.stationHash[sIdx].macCode].name,
                            shortName: a.macHash[a.stationHash[sIdx].macCode].name,
                            code: a.macHash[a.stationHash[sIdx].macCode].code
                        };
                        a.stationHash[a.stationHash[sIdx].macCode] = macStation;
                        a.countryStationHash[a.stationHash[sIdx].countryCode].push(macStation);
                        a.countryStationHash[a.stationHash[sIdx].countryCode].sort(a.sortCountryStationHash);
                        a.marketHash[a.stationHash[sIdx].macCode] = []
                    }
                    myMarketHash = a.marketHash[sIdx];
                    macMarketHash = a.marketHash[a.stationHash[sIdx].macCode];
                    for (dIdx in myMarketHash) {
                        alreadyInMarketHash = false;
                        for (mdIdx in macMarketHash) {
                            if (myMarketHash[dIdx].code == macMarketHash[mdIdx].code) {
                                alreadyInMarketHash = true;
                                break
                            }
                        }
                        if (alreadyInMarketHash == false) {
                            a.marketHash[a.stationHash[sIdx].macCode].push(myMarketHash[dIdx])
                        }
                    }
                    macStationMarket = {
                        code: a.stationHash[sIdx].macCode,
                        name: a.macHash[a.stationHash[sIdx].macCode].name
                    };
                    for (mIndex in a.marketHash) {
                        for (dIdx in a.marketHash[mIndex]) {
                            if (a.marketHash[mIndex][dIdx].code == a.stationHash[sIdx].code) {
                                a.marketHash[mIndex].push(macStationMarket);
                                delete a.marketHash[mIndex][dIdx]
                            }
                        }
                    }
                    a.macStationHash[sIdx] = a.stationHash[sIdx];
                    delete a.stationHash[sIdx]
                }
            }
        }
    };
    a.setSettingsByObject = function(c) {
        b.setSettingsByObject.call(this, c);
        a.populateCountryHash();
        a.populateStationHash();
        a.populateMacHash();
        a.populateMarketHash();
        a.joinMacStations();
        a.populateClientHash()
    };
    return a
};
SKYSALES.Util.createObjectArray = [];
SKYSALES.Util.createObject = function(a, e, c) {
    var b = SKYSALES.Util.createObjectArray;
    b[b.length] = {
        objNameBase: a,
        objType: e,
        json: c
    }
};
SKYSALES.Util.initObjects = function() {
    var f = 0;
    var c = SKYSALES.Util.createObjectArray;
    var l = "";
    var b = "";
    var e = null;
    var a = null;
    for (f = 0; f < c.length; f += 1) {
        a = c[f];
        l = a.objNameBase + SKYSALES.Instance.getNextIndex();
        b = a.objType;
        e = a.json || {};
        if (SKYSALES.Class[b]) {
            SKYSALES.Instance[l] = new SKYSALES.Class[b]();
            SKYSALES.Instance[l].init(e)
        }
    }
    SKYSALES.Util.createObjectArray = []
};
SKYSALES.Util.decodeUriComponent = function(a) {
    a = a || "";
    if (window.decodeURIComponent) {
        a = window.decodeURIComponent(a)
    }
    a = a.replace(/\+/g, " ");
    return a
};
SKYSALES.Util.encodeUriComponent = function(a) {
    a = a || "";
    if (window.encodeURIComponent) {
        a = window.encodeURIComponent(a)
    }
    return a
};
SKYSALES.Util.getResource = function() {
    return SKYSALES.Resource
};
SKYSALES.Util.extendObject = function(b) {
    var a = function() {};
    a.prototype = b;
    return new a()
};
SKYSALES.Util.initializeNewObject = function(l) {
    var f = "";
    var b = {
        objNameBase: "",
        objType: "",
        selector: ""
    };
    var e = function() {
        var p = true;
        $().extend(b, l);
        var n = null;
        for (n in b) {
            if (b.hasOwnProperty(n)) {
                if (b[n] === undefined) {
                    p = false;
                    break
                }
            }
        }
        return p
    };
    var c = function(s) {
        var u = $(this).val();
        var p = SKYSALES.Json.parse(u);
        var t = null;
        var q = "";
        var x = [];
        var r = 0;
        var w = 0;
        var n = /^([a-zA-Z0-9]+)\[(\d+)\]$/;
        var v = [];
        if (p.method !== undefined) {
            t = SKYSALES.Instance[f];
            if (p.method.name.indexOf(".") > -1) {
                x = p.method.name.split(".");
                for (r = 0; r < x.length; r += 1) {
                    q = x[r];
                    v = q.match(n);
                    if ((v) && (v.length > 0)) {
                        q = v[1];
                        w = v[2];
                        w = parseInt(w, 10);
                        t = t[q][w]
                    } else {
                        t = t[q]
                    }
                }
            } else {
                t = t[p.method.name]
            }
            if (t) {
                t(p.method.paramJsonObject)
            }
        }
    };
    var m = function() {
        f = l.objNameBase + SKYSALES.Instance.getNextIndex();
        if (SKYSALES.Class[l.objType]) {
            SKYSALES.Instance[f] = new SKYSALES.Class[l.objType]();
            $("object.jsObject > param", this).each(c)
        } else {
            alert("Object Type Not Found: " + l.objType)
        }
    };
    var a = function() {
        var n = e();
        if (n) {
            $(l.selector).each(m)
        } else {
            alert("\nthere has been an error")
        }
    };
    a();
    return false
};
SKYSALES.Util.populateSelect = function(b) {
    var e = b.selectedItem || null;
    var l = b.objectArray || null;
    var q = b.selectBox || null;
    var n = b.showCode || false;
    var r = b.clearOptions || false;
    var p = "";
    var m = "";
    var c = null;
    var f = null;
    var a = "";
    if (q) {
        c = q.get(0);
        if (c && c.options) {
            if (r) {
                c.options.length = 0
            } else {
                if (!c.originalOptionLength) {
                    c.originalOptionLength = c.options.length
                }
                c.options.length = c.originalOptionLength
            }
            if (l) {
                for (a in l) {
                    if (l.hasOwnProperty(a)) {
                        f = l[a];
                        if (n) {
                            p = f.name + " (" + f.code + ")"
                        } else {
                            p = f.name || f.Name
                        }
                        m = f.code || f.ProvinceStateCode || "";
                        c.options[c.options.length] = new window.Option(p, m, false, false)
                    }
                }
                if (e !== null) {
                    q.val(e)
                }
            }
        }
    }
};
SKYSALES.Util.populateSelectWithGroups = function(c) {
    var a = c.selectedItem || null;
    var p = c.objectArray || null;
    var l = c.groupArray || null;
    var n = c.selectBox || null;
    var x = c.showCode || false;
    var I = c.clearOptions || false;
    var H = c.promoStationsArray || [];
    var m = c.restrictedStationCategory || [];
    var u = c.restrictedPairStationCategory || [];
    var z = c.originObject || null;
    var v = "";
    var B = null;
    var t = null;
    var A = null;
    var s = null;
    var f = null;
    var b = null;
    var y = null;
    var D = [];
    var w = true;
    var r = true;
    var q = 0;
    var E = false;
    var G = false;
    var J = c.optionHeaderText || ["Origin", "Destination"];
    if (n) {
        t = n.get(0);
        if (t && t.options) {
            if (I) {
                t.options.length = 0
            } else {
                if (!t.originalOptionLength) {
                    t.originalOptionLength = t.options.length
                }
                try {
                    t.options.length = t.originalOptionLength
                } catch (F) {}
                while (t.hasChildNodes()) {
                    t.removeChild(t.firstChild)
                }
            }
            if (p && l) {
                for (A in l) {
                    if (l.hasOwnProperty(A)) {
                        for (f in l[A]) {
                            if (l[A].hasOwnProperty(f) && f !== "name") {
                                s = l[A][f] || {};
                                w = true;
                                if (s.stationCategories) {
                                    for (q = 0; q < m.length; q = q + 1) {
                                        if (s.stationCategories.indexOf(m[q]) > -1) {
                                            w = false;
                                            break
                                        }
                                    }
                                }
                                if (w) {
                                    for (B in p) {
                                        r = true;
                                        var C = true;
                                        if (H.length > 0) {
                                            C = false;
                                            for (arrayIndex in H) {
                                                if (H[arrayIndex] == B || H[arrayIndex] == p[B].code) {
                                                    C = true;
                                                    break
                                                }
                                            }
                                        }
                                        if (z && s.stationCategories && z.stationCategories) {
                                            for (q = 0; q < u.length; q = q + 1) {
                                                if (s.stationCategories.indexOf(u[q]) > -1 && z.stationCategories.indexOf(u[q]) > -1 && s.stationCategories[s.stationCategories.indexOf(u[q])] == z.stationCategories[z.stationCategories.indexOf(u[q])]) {
                                                    r = false;
                                                    break
                                                }
                                            }
                                            if (!r) {
                                                break
                                            }
                                        }
                                        if ((B == s.code || p[B].code == s.code) && C) {
                                            E = true;
                                            G = true;
                                            break
                                        }
                                    }
                                }
                                l[A][f].show = (E) ? true : false;
                                E = false
                            }
                        }
                        l[A].show = (G) ? true : false;
                        G = false
                    }
                }
                b = document.createElement("option");
                b.value = "";
                selectBoxName = t.name.toLowerCase();
                b.appendChild(document.createTextNode(J[(selectBoxName.indexOf("origin") > 0) ? 0 : 1]));
                t.appendChild(b);
                for (A in l) {
                    if (l.hasOwnProperty(A) && l[A].show && A !== "show") {
                        if (l[A].show) {
                            y = document.createElement("optgroup");
                            y.label = l[A].name || A
                        }
                        for (f in l[A]) {
                            if (l[A].hasOwnProperty(f) && f !== "show" && f !== "name" && l[A][f].show) {
                                s = l[A][f];
                                b = document.createElement("option");
                                b.value = s.code;
                                v = (x) ? s.name + " (" + s.code + ")" : s.name;
                                b.appendChild(document.createTextNode(v));
                                y.appendChild(b)
                            }
                        }
                        t.appendChild(y)
                    }
                }
                if (a !== null) {
                    n.val(a)
                }
            }
        }
    }
};
SKYSALES.Util.cloneArray = function(a) {
    return a.concat()
};
SKYSALES.Util.convertToLocaleCurrency = function(a) {
    var b = {
        num: a
    };
    var c = new SKYSALES.Class.LocaleCurrency();
    c.init(b);
    return c.currency
};
SKYSALES.Util.getTime = function(a, b, e) {
    var c = "";
    a = Number(a);
    b = Number(b);
    e = Number(e);
    if (isNaN(a) === false) {
        a += 1;
        if (a > 12) {
            a = a - 12
        }
        if (a.toString().length === 1) {
            a = "0" + a
        }
        c = a;
        if (isNaN(b) === false) {
            if (b.toString().length === 1) {
                b = "0" + b
            }
            c = c + ":" + b;
            if (isNaN(e) === false) {
                if (e.toString().length === 1) {
                    e = "0" + e
                }
                c = c + ":" + e
            }
        }
        if (a > 12) {
            c = c + " PM"
        } else {
            c = c + " AM"
        }
    }
    return c
};
SKYSALES.Util.getDate = function(c, e, a) {
    var b = "";
    c = Number(c);
    e = Number(e);
    a = Number(a);
    if (isNaN(c) === false) {
        b = c;
        if (isNaN(e) === false) {
            if (e.toString().length === 1) {
                e = "0" + e
            }
            b = b.toString() + e.toString();
            if (isNaN(a) === false) {
                if (a.toString().length === 1) {
                    a = "0" + a
                }
                b = b.toString() + a.toString()
            }
        }
    }
    return b
};
if (!SKYSALES.Class.SkySales) {
    SKYSALES.Class.SkySales = function() {
        var a = this;
        a.containerId = "";
        a.container = null;
        a.init = SKYSALES.Class.SkySales.prototype.init;
        a.getById = SKYSALES.Class.SkySales.prototype.getById;
        a.setSettingsByObject = SKYSALES.Class.SkySales.prototype.setSettingsByObject;
        a.addEvents = SKYSALES.Class.SkySales.prototype.addEvents;
        a.setVars = SKYSALES.Class.SkySales.prototype.setVars;
        a.hide = SKYSALES.Class.SkySales.prototype.hide;
        a.show = SKYSALES.Class.SkySales.prototype.show;
        return a
    };
    SKYSALES.Class.SkySales.prototype.init = function(a) {
        this.setSettingsByObject(a);
        this.setVars()
    };
    SKYSALES.Class.SkySales.prototype.getById = function(b) {
        var a = null;
        if (b) {
            a = window.document.getElementById(b)
        }
        if (a) {
            a = $(a)
        } else {
            a = $([])
        }
        return a
    };
    SKYSALES.Class.SkySales.prototype.setSettingsByObject = function(a) {
        var b = "";
        for (b in a) {
            if (a.hasOwnProperty(b)) {
                if (this[b] !== undefined) {
                    this[b] = a[b]
                }
            }
        }
    };
    SKYSALES.Class.SkySales.prototype.addEvents = function() {};
    SKYSALES.Class.SkySales.prototype.setVars = function() {
        this.container = $("#" + this.containerId)
    };
    SKYSALES.Class.SkySales.prototype.hide = function() {
        this.container.hide("slow")
    };
    SKYSALES.Class.SkySales.prototype.show = function() {
        this.container.show("slow")
    }
}
if (!SKYSALES.Class.BaseToggleView) {
    SKYSALES.Class.BaseToggleView = function() {
        var a = SKYSALES.Class.SkySales();
        var b = SKYSALES.Util.extendObject(a);
        b.toggleViewIdArray = [];
        b.toggleViewArray = [];
        b.addToggleView = function(l) {
            if (l.toggleViewIdArray) {
                l = l.toggleViewIdArray
            }
            var e = l || [];
            var c = null;
            var f = 0;
            var m = null;
            if (e.length === undefined) {
                e = [];
                e[0] = l
            }
            for (f = 0; f < e.length; f += 1) {
                c = e[f];
                m = new SKYSALES.Class.ToggleView();
                m.init(c);
                b.toggleViewArray[b.toggleViewArray.length] = m
            }
        };
        return b
    }
}
SKYSALES.Class.Date = function() {
    var b = new SKYSALES.Class.SkySales(),
        a = SKYSALES.Util.extendObject(b);
    a.Day = "";
    a.Month = "";
    a.Hour = "";
    a.Minute = "";
    a.Second = "";
    a.Year = "";
    a.date = null;
    a.init = function(c) {
        this.setSettingsByObject(c);
        this.initDateTime()
    };
    a.initDateTime = function() {
        this.date = new Date();
        this.date.setHours(this.Hour, this.Minute, this.Second, 0);
        this.date.setFullYear(this.Year, this.Month - 1, this.Day)
    };
    a.getTime = function() {
        var c = SKYSALES.Util.getTime(this.Hour, this.Minute);
        return c
    };
    a.getDate = function() {
        var c = SKYSALES.Util.getDate(this.Year, this.Month, this.Day);
        return c
    };
    return a
};
if (!SKYSALES.Class.BaggagePrompt) {
    SKYSALES.Class.BaggagePrompt = function() {
        var b = new SKYSALES.Class.SkySales(),
            a = SKYSALES.Util.extendObject(b);
        a.baggagePrompt = "";
        a.init = function(c) {
            this.setSettingsByObject(c);
            a.displayPrompt()
        };
        a.displayPrompt = function() {
            $(window).load(function() {
                if (a.baggagePrompt != "") {
                    alert(a.baggagePrompt)
                }
            })
        };
        return a
    };
    SKYSALES.Class.BaggagePrompt.createObject = function(a) {
        SKYSALES.Util.createObject("baggagePrompt", "BaggagePrompt", a)
    }
}
if (!SKYSALES.Class.BookingDetail) {
    SKYSALES.Class.BookingDetail = function() {
        var b = new SKYSALES.Class.SkySales(),
            a = SKYSALES.Util.extendObject(b);
        a.ssrChangePrompt = "";
        a.showSsrPrompter = "";
        a.confirmButtonId = "";
        a.init = function(c) {
            this.setSettingsByObject(c);
            this.addEvent()
        };
        a.addEvent = function() {
            var c = "";
            $("#" + this.confirmButtonId).click(this.displayPromptHandler);
            c = this.confirmButtonId.split("_");
            $("#" + c[0] + "_" + c[2]).click(this.displayPromptHandler)
        };
        a.displayPromptHandler = function() {
            return a.displayPrompt()
        };
        a.displayPrompt = function() {
            if (this.showSsrPrompter === "true") {
                return confirm(this.ssrChangePrompt)
            }
        };
        return a
    };
    SKYSALES.Class.BookingDetail.createObject = function(a) {
        SKYSALES.Util.createObject("bookingDetail", "BookingDetail", a)
    }
}
if (!SKYSALES.Class.FlightSearch) {
    SKYSALES.Class.FlightSearch = function() {
        var c = new SKYSALES.Class.SkySales();
        var a = SKYSALES.Util.extendObject(c);
        a.optionHeaderText = null;
        a.promoStationsArray = null;
        a.marketArray = null;
        a.flightTypeInputIdArray = null;
        a.countryInputIdArray = null;
        a.confirmTextArray = null;
        a.actionId = "";
        a.action = null;
        a.dropDownAdtId = "";
        a.dropDownInfId = "";
        a.dropDownChdId = "";
        a.tooManyInfantsText = "";
        a.tooManyPassengersTextPre = "";
        a.tooManyPassengersTextPost = "";
        a.infantArray = null;
        a.infantAgeText = "";
        a.updateButtonId = "";
        a.updateButton = null;
        a.updateButtonClicked = false;
        a.dateRangeError = "";
        a.dateSameText = "";
        a.originalDate = [];
        a.disableInputId = "";
        a.insuranceControlId = "";
        a.checkboxNotification = "";
        a.maxpaxnum = "";
        a.autoMacArray = [];
        a.autoMacMessage = "";
        a.autoMacShown = " ";
        a.restrictedStationCategories = [];
        a.restrictedPairStationCategory = [];
        a.localizedMac = [];
        var e = [];
        var b = [];
        a.init = function(f) {
            this.setSettingsByObject(f);
            this.setVars();
            this.addEvents();
            this.initFlightTypeInputIdArray();
            this.initCountryInputIdArray();
            this.populateFlightType();
            this.initAutoMacArray()
        };
        a.initAutoMacArray = function() {
            for (amIdx in a.autoMacArray) {
                if (SKYSALES.Resource.macHash[a.autoMacArray[amIdx].MacStation]) {
                    a.autoMacArray[amIdx].Stations = SKYSALES.Resource.macHash[a.autoMacArray[amIdx].MacStation].stations
                }
            }
        };
        a.setSettingsByObject = function(l) {
            c.setSettingsByObject.call(this, l);
            var f = 0;
            var m = this.marketArray || [];
            var n = null;
            for (f = 0; f < m.length; f += 1) {
                n = new SKYSALES.Class.FlightSearchMarket();
                n.optionHeaderText = this.optionHeaderText;
                n.flightSearch = this;
                n.index = f;
                n.autoMacArray = a.autoMacArray;
                n.restrictedStationCategories = this.restrictedStationCategories;
                n.restrictedPairStationCategory = this.restrictedPairStationCategory;
                n.localizedMac = a.localizedMac;
                n.init(m[f]);
                this.marketArray[f] = n
            }
        };
        a.setVars = function() {
            c.setVars.call(this);
            a.action = $("#" + this.actionId);
            a.updateButton = $("input[type='submit'][id^='" + this.updateButtonId + "']")
        };
        a.addEvents = function() {
            c.addEvents.call(this);
            if (a.actionId != null) {
                a.action.click(a.searchHandler)
            }
            if (a.updateButtonId != null) {
                a.updateButton.click(a.searchUpdateHandler)
            }
        };
        a.searchUpdateHandler = function() {
            a.updateButtonClicked = true;
            a.searchHandler()
        };
        a.searchHandler = function() {
            a.checkAutoMac();
            a.updateButtonClicked = false;
            if (/SearchView/i.test(a.actionId) || /SelectView/i.test(a.actionId) || /CompactView/i.test(a.actionId)) {
                if (a.tooManyInfantsText != "") {
                    if (a.checkInfantPax() == false) {
                        return false
                    }
                }
                if (a.tooManyPassengersTextPre != "") {
                    if (a.checkNumPax() == false) {
                        return false
                    }
                }
                if (a.checkSameDate() == false) {
                    return false
                }
            } else {
                if (/SearchChangeView/i.test(a.actionId)) {
                    return a.searchChangeHandler()
                }
            }
        };
        a.checkAutoMac = function() {
            for (j = 0; j < a.marketArray.length; j += 1) {
                isAutoMac = false;
                autoMacStations = "";
                for (i = 0; i < a.autoMacArray.length; i += 1) {
                    if (a.autoMacShown.indexOf(a.autoMacArray[i].MacStation) < 1) {
                        for (h = 0; h < a.autoMacArray[i].Stations.length; h += 1) {
                            if ($("#" + a.marketArray[j].marketInputIdArray[0].originId).val() == a.autoMacArray[i].Stations[h] || $("#" + a.marketArray[j].marketInputIdArray[0].destinationId).val() == a.autoMacArray[i].Stations[h] || $("#" + a.marketArray[j].marketInputIdArray[0].originId).val() == a.autoMacArray[i].MacStation || $("#" + a.marketArray[j].marketInputIdArray[0].destinationId).val() == a.autoMacArray[i].MacStation) {
                                for (g = 0; g < a.autoMacArray[i].Stations.length; g += 1) {
                                    stationCode = a.autoMacArray[i].Stations[g];
                                    if (SKYSALES.Resource.macStationHash[stationCode]) {
                                        autoMacStations += "\n" + SKYSALES.Resource.macStationHash[stationCode].name + " (" + stationCode + ")"
                                    } else {
                                        if (SKYSALES.Resource.stationHash[stationCode]) {
                                            autoMacStations += "\n" + SKYSALES.Resource.stationHash[stationCode].name + " (" + stationCode + ")"
                                        }
                                    }
                                }
                                a.autoMacShown += " " + a.autoMacArray[i].MacStation + " ";
                                a.autoMacMessage = a.autoMacArray[i].MacPrompter;
                                isAutoMac = true;
                                break
                            }
                        }
                    }
                    if (isAutoMac == true) {
                        break
                    }
                }
                if ((autoMacStations != "") && (a.autoMacMessage)) {
                    alert(a.autoMacMessage)
                }
            }
        };
        a.checkSameDate = function() {
            if (a.dateSameText != "" && $("#" + a.flightTypeInputIdArray[1].checkBoxId).is(":checked") == false) {
                if ($("#" + a.marketArray[0].marketDateIdArray[0].marketDateId).val() == $("#" + a.marketArray[1].marketDateIdArray[0].marketDateId).val()) {
                    return confirm(a.dateSameText)
                }
            }
        };
        a.searchChangeHandler = function() {
            a = this;
            var l = 0;
            var n = null;
            var f = $("#" + this.insuranceControlId).is(":checked");
            if ($("#" + this.insuranceControlId).length == 0) {
                f = true
            }
            if (a.marketArray != null && f == true) {
                if (a.marketArray.length > 1) {
                    var m = a.checkDateRange();
                    if (!m) {
                        return false
                    }
                } else {
                    if ($("#" + this.disableInputId).is(":checked") == false) {
                        alert(this.checkboxNotification);
                        return false
                    }
                }
                for (l = 0; l < a.marketArray.length; l++) {
                    n = a.marketArray[l];
                    if (n.isToChange() && n.compareDates() && !confirm(a.confirmTextArray[l])) {
                        return false
                    }
                }
            }
            return a.checkInfantAge()
        };
        a.checkInfantAge = function() {
            if (a.marketArray != null) {
                var p = a.marketArray[a.marketArray.length - 1];
                if (p.isToChange()) {
                    var f = p.getSearchDate();
                    var m = new SKYSALES.Class.TravelerAge();
                    var l = 0;
                    var n = null;
                    for (l = 0; l < a.infantArray.length; l += 1) {
                        n = a.infantArray[l];
                        if (!m.isInfant(f, n)) {
                            alert(a.infantAgeText);
                            return false
                        }
                    }
                }
            }
        };
        a.checkDateRange = function() {
            var r, n, l, q;
            r = new Array();
            n = new Array();
            l = new Array();
            q = new Array();
            var f = (this.originalDate[0]).split("/");
            var p = (this.originalDate[1]).split("/");
            for (var m = 0; m < a.marketArray.length; m++) {
                r[m] = this.marketArray[m].marketDateIdArray[0].marketDateId;
                n[m] = $("#" + r[m]).val().split("/");
                l[m] = new Date(n[m][2], n[m][0], n[m][1]);
                q[m] = $("#" + this.disableInputId[m]).is(":checked")
            }
            if (q[0] == false && q[1] == false) {
                alert(this.checkboxNotification);
                return false
            }
            if (q[0] == false) {
                l[0] = new Date(f[2], f[0], f[1])
            }
            if (q[1] == false) {
                l[1] = new Date(p[2], p[0], p[1])
            }
            if (l[1] < l[0]) {
                alert(this.dateRangeError);
                return false
            }
            if (l[1].toString() == l[0].toString()) {
                if (!confirm(this.dateSameText)) {
                    return false
                }
            }
            return true
        };
        a.checkInfantPax = function() {
            var f = $("#" + a.dropDownAdtId);
            var l = $("#" + a.dropDownInfId);
            if (f.val() < l.val()) {
                alert(a.tooManyInfantsText);
                return false
            }
        };
        a.checkNumPax = function() {
            var f = $("#" + a.dropDownAdtId);
            var l = $("#" + a.dropDownChdId);
            var m = parseInt(f.val()) + parseInt(l.val());
            if (m > a.maxpaxnum) {
                alert(a.tooManyPassengersTextPre + a.maxpaxnum + a.tooManyPassengersTextPost);
                return false
            }
        };
        a.initCountryInputIdArray = function() {
            var l = 0;
            var f = null;
            var n = {};
            var m = this.countryInputIdArray || [];
            for (l = 0; l < m.length; l += 1) {
                f = m[l];
                n = new SKYSALES.Class.CountryInput();
                n.init(f);
                e[e.length] = n
            }
        };
        a.initFlightTypeInputIdArray = function() {
            var l = 0;
            var n = null;
            var f = {};
            var m = this.flightTypeInputIdArray || [];
            for (l = 0; l < m.length; l += 1) {
                n = m[l];
                f = new SKYSALES.Class.FlightTypeInput();
                f.flightSearch = this;
                f.index = l;
                f.checkBoxId = n.checkBoxId;
                f.init(n);
                b[b.length] = f
            }
        };
        a.populateFlightType = function() {
            var f = 0;
            var l = null;
            for (f = 0; f < b.length; f += 1) {
                l = b[f];
                if (l.input.attr("checked")) {
                    l.input.click();
                    if ($("#" + l.checkBoxId).length > 0) {
                        $("#" + l.checkBoxId).attr("checked", true)
                    }
                    break
                }
            }
        };
        a.updateFlightTypeShow = function(l) {
            var f = 0;
            var m = null;
            for (f = 0; f < b.length; f += 1) {
                m = b[f];
                m.hideInputArray.show()
            }
        };
        a.updateFlightTypeHide = function(f) {
            this.updateFlightTypeShow();
            f.hideInputArray.hide()
        };
        return a
    };
    SKYSALES.Class.FlightSearch.createObject = function(a) {
        SKYSALES.Util.createObject("flightSearch", "FlightSearch", a)
    }
}
if (!SKYSALES.Class.FlightSearchMarket) {
    SKYSALES.Class.FlightSearchMarket = function() {
        var c = new SKYSALES.Class.SkySales();
        var b = SKYSALES.Util.extendObject(c);
        b.flightSearch = null;
        b.index = -1;
        b.validationMessageObject = {};
        b.validationObjectIdArray = [];
        b.stationInputIdArray = [];
        b.stationDropDownIdArray = [];
        b.marketInputIdArray = [];
        b.macInputIdArray = [];
        b.marketDateIdArray = [];
        b.autoMacArray = [];
        b.disableMarket = false;
        b.localizedMac = [];
        b.restrictedStationCategories = [];
        b.restrictedPairStationCategory = [];
        b.optionHeaderText = null;
        var a = [];
        var e = [];
        var l = [];
        var f = [];
        var m = [];
        b.init = function(n) {
            this.setSettingsByObject(n);
            this.setVars();
            this.addEvents();
            this.initMarketInputIdArray();
            this.initStationInputIdArray();
            this.initStationDropDownIdArray();
            this.initMacInputIdArray();
            this.initMarketDateIdArray();
            this.initValidationObjectRedirect()
        };
        b.initMacInputIdArray = function() {
            var p = 0;
            var r = null;
            var n = {};
            var q = this.macInputIdArray || [];
            for (p = 0; p < q.length; p += 1) {
                r = q[p];
                n = new SKYSALES.Class.MacInput();
                n.init(r);
                n.autoMacArray = b.autoMacArray;
                f[f.length] = n;
                n.showMac.call(n.stationInput)
            }
        };
        b.initMarketDateIdArray = function() {
            var n = 0;
            var q = null;
            var r = {};
            var p = this.marketDateIdArray || [];
            for (n = 0; n < p.length; n += 1) {
                q = p[n];
                r = new SKYSALES.Class.MarketDate();
                r.marketDateCount = this.flightSearch.marketArray.length;
                r.init(q);
                m[m.length] = r
            }
        };
        b.compareDates = function() {
            return m[0].compareDates()
        };
        b.getSearchDate = function() {
            return m[0].marketDate.val()
        };
        b.initMarketInputIdArray = function() {
            var p = 0;
            var n = null;
            var r = {};
            var q = this.marketInputIdArray || [];
            for (p = 0; p < q.length; p += 1) {
                n = q[p];
                r = new SKYSALES.Class.MarketInput();
                marketOriginalId = this.flightSearch.marketArray[0].stationDropDownIdArray[0].inputId;
                r.promoStationsArray = this.flightSearch.promoStationsArray;
                r.optionHeaderText = b.optionHeaderText;
                r.restrictedStationCategories = this.restrictedStationCategories;
                r.restrictedPairStationCategory = this.restrictedPairStationCategory;
                r.localizedMac = b.localizedMac;
                r.init(n);
                a[a.length] = r;
                if (this.disableMarket) {
                    r.disableInput.attr("disabled", true)
                }
            }
        };
        b.isToChange = function() {
            if (a[1].disableInput.attr("checked")) {
                return true
            }
            return false
        };
        b.initStationInputIdArray = function() {
            var n = 0;
            var p = null;
            var r = {};
            var q = this.stationInputIdArray;
            for (n = 0; n < q.length; n += 1) {
                p = q[n];
                r = new SKYSALES.Class.StationInput();
                r.init(p);
                e[e.length] = r
            }
        };
        b.initStationDropDownIdArray = function() {
            var p = 0;
            var r = null;
            var n = {};
            var q = this.stationDropDownIdArray;
            for (p = 0; p < q.length; p += 1) {
                r = q[p];
                n = new SKYSALES.Class.StationDropDown();
                n.init(r);
                l[l.length] = n;
                if (p == 0) {
                    n.selectBox.change()
                }
            }
        };
        b.initValidationObjectRedirect = function() {
            var w = this.validationObjectIdArray || [];
            var n = 0;
            var v = "";
            var u = "";
            var t = "";
            var p = null;
            var s = null;
            var q = null;
            for (n = 0; n < w.length; n += 1) {
                v = w[n];
                u = v.key || "";
                t = v.value || "";
                p = $("object.metaobject>param[@value*='" + u + "']");
                if (p.length > 0) {
                    s = $(":input#" + t);
                    if (s.length > 0) {
                        q = p[0];
                        if ("value" in q) {
                            var r = q.value;
                            r = r.replace(u, t);
                            q.value = r
                        }
                    }
                }
            }
        };
        return b
    }
}
if (!SKYSALES.Class.MacInput) {
    SKYSALES.Class.MacInput = function() {
        var a = new SKYSALES.Class.SkySales();
        var b = SKYSALES.Util.extendObject(a);
        b.macHash = SKYSALES.Util.getResource().macHash;
        b.stationHash = SKYSALES.Util.getResource().stationHash;
        b.stationInputId = "";
        b.macContainerId = "";
        b.macLabelId = "";
        b.macInputId = "";
        b.macContainer = {};
        b.stationInput = {};
        b.macInput = {};
        b.macLabel = {};
        b.autoMacArray = [];
        b.showMac = function() {
            var f = $(this).val();
            f = f || "";
            f = f.toUpperCase();
            var c = null;
            var e = "";
            var m = "";
            var l = null;
            b.macInput.removeAttr("checked");
            b.macContainer.hide();
            c = b.stationHash[f];
            if (c) {
                e = c.macCode;
                isAutoMac = false;
                if (b.autoMacArray.length > 0) {
                    for (i = 0; i < b.autoMacArray.length; i += 1) {
                        if (b.autoMacArray[i].MacStation == e) {
                            isAutoMac = true
                        }
                    }
                }
                l = b.macHash[e];
                if ((l) && (l.stations.length > 0)) {
                    m = l.stations.join();
                    b.macLabel.html(m);
                    if (isAutoMac) {
                        b.macInput.attr("checked", true)
                    } else {
                        b.macContainer.show()
                    }
                }
            }
        };
        b.addEvents = function() {
            b.stationInput.change(b.showMac)
        };
        b.setVars = function() {
            b.stationInput = $("#" + b.stationInputId);
            b.macContainer = $("#" + b.macContainerId);
            b.macLabel = $("#" + b.macLabelId);
            b.macInput = $("#" + b.macInputId)
        };
        b.init = function(c) {
            a.init.call(this, c);
            b.macContainer.hide();
            this.addEvents()
        };
        return b
    }
}
if (!SKYSALES.Class.MarketDate) {
    SKYSALES.Class.MarketDate = function() {
        var b = new SKYSALES.Class.SkySales();
        var a = SKYSALES.Util.extendObject(b);
        a.dateFormat = SKYSALES.datepicker.datePickerFormat;
        a.dateDelimiter = SKYSALES.datepicker.datePickerDelimiter;
        a.marketDateId = "";
        a.marketDate = null;
        a.marketDayId = "";
        a.marketDay = null;
        a.marketMonthYearId = "";
        a.marketMonthYear = null;
        a.origDate = null;
        a.marketDateCount = "";
        a.setSettingsByObject = function(c) {
            b.setSettingsByObject.call(this, c);
            var e = "";
            for (e in c) {
                if (a.hasOwnProperty(e)) {
                    a[e] = c[e]
                }
            }
        };
        a.parseDate = function(f) {
            var q = "";
            var n = "";
            var p = "";
            var e = new Date();
            var c = "";
            var r = "";
            var l = [];
            var m = 0;
            if (f.indexOf(a.dateDelimiter) > -1) {
                l = f.split(a.dateDelimiter);
                for (m = 0; m < a.dateFormat.length; m += 1) {
                    c = l[m];
                    if (c.charAt(0) === "0") {
                        c = c.substring(1)
                    }
                    r = a.dateFormat.charAt(m);
                    switch (r) {
                        case "m":
                            n = c;
                            break;
                        case "d":
                            q = c;
                            break;
                        case "y":
                            p = c;
                            break
                    }
                }
                e = new Date(p, n - 1, q)
            }
            return e
        };
        a.addEvents = function() {
            var c = new SKYSALES.Class.DatePickerManager();
            c.marketCount = a.marketDateCount;
            c.isAOS = false;
            c.yearMonth = a.marketMonthYear;
            c.day = a.marketDay;
            c.linkedDate = a.marketDate;
            c.init()
        };
        a.setVars = function() {
            a.marketDate = $("#" + a.marketDateId);
            a.marketDay = $("#" + a.marketDayId);
            a.marketMonthYear = $("#" + a.marketMonthYearId)
        };
        a.init = function(c) {
            b.init.call(this, c);
            this.addEvents();
            a.origDate = a.marketDate.val()
        };
        a.compareDates = function() {
            if (a.origDate != a.marketDate.val()) {
                return false
            }
            return true
        };
        a.datesInOrder = function(f) {
            var l = true;
            var e = null;
            var c = null;
            e = this.parseDate(f[0]);
            c = this.parseDate(f[1]);
            if (e > c) {
                l = false
            }
            return l
        };
        a.isOneDayOnly = function(f) {
            var l = true;
            var e = null;
            var c = null;
            e = this.parseDate(f[0]);
            c = this.parseDate(f[1]);
            if (e > c || e < c) {
                l = false
            }
            return l
        };
        return a
    }
}
if (!SKYSALES.Class.CountryInput) {
    SKYSALES.Class.CountryInput = function() {
        var b = new SKYSALES.Class.SkySales();
        var a = SKYSALES.Util.extendObject(b);
        a.countryInfo = SKYSALES.Util.getResource().countryInfo;
        a.countryInputId = "";
        a.input = {};
        a.defaultCountry = "";
        a.countryArray = [];
        a.populateCountryInput = function() {
            var c = {
                selectBox: a.input,
                objectArray: a.countryArray,
                selectedItem: a.defaultCountry,
                showCode: true
            };
            SKYSALES.Util.populateSelect(c)
        };
        a.addEvents = function() {};
        a.setVars = function() {
            a.input = $("#" + a.countryInputId);
            var c = a.countryInfo;
            if (c) {
                if (c.CountryList) {
                    a.countryArray = c.CountryList
                }
                if (c.DefaultValue) {
                    a.defaultCountry = c.DefaultValue
                }
            }
        };
        a.init = function(c) {
            b.init.call(this, c);
            a.populateCountryInput();
            this.addEvents()
        };
        return a
    }
}
if (!SKYSALES.Class.FlightTypeInput) {
    SKYSALES.Class.FlightTypeInput = function() {
        var b = new SKYSALES.Class.SkySales();
        var c = SKYSALES.Util.extendObject(b);
        c.flightSearch = null;
        c.index = -1;
        c.flightTypeId = "";
        c.hideInputIdArray = [];
        c.hideInputArray = [];
        c.input = {};
        var a = 1;
        c.updateFlightTypeHandler = function() {
            c.flightSearch.updateFlightTypeHide(c)
        };
        c.addEvents = function() {
            b.addEvents.call(this);
            this.input.click(this.updateFlightTypeHandler)
        };
        c.getById = function(f) {
            var e = null;
            if (f) {
                e = window.document.getElementById(f)
            }
            return e
        };
        c.setVars = function() {
            b.setVars.call(this);
            var e = 0;
            var f = null;
            var l = [];
            c.input = $("#" + this.flightTypeId);
            for (e = 0; e < this.hideInputIdArray.length; e += 1) {
                f = c.getById(this.hideInputIdArray[e]);
                if (f) {
                    l[l.length] = f
                }
            }
            c.hideInputArray = $(l)
        };
        c.init = function(e) {
            this.setSettingsByObject(e);
            this.setVars();
            this.addEvents()
        };
        return c
    }
}
if (!SKYSALES.Class.MarketInput) {
    SKYSALES.Class.MarketInput = function() {
        var b = new SKYSALES.Class.SkySales();
        var a = SKYSALES.Util.extendObject(b);
        a.marketHash = SKYSALES.Util.getResource().marketHash;
        a.stationHash = SKYSALES.Util.getResource().stationHash;
        a.countryStationHash = SKYSALES.Util.getResource().countryStationHash;
        a.optionHeaderText = null;
        a.containerId = "";
        a.container = null;
        a.containerTextId = "";
        a.containerText = null;
        a.disableInputId = "";
        a.disableInput = null;
        a.originId = "";
        a.origin = null;
        a.destinationId = "";
        a.destination = null;
        a.toggleMarketCount = 0;
        a.marketOriginalId = "";
        a.promoStationsArray = [];
        a.localizedMac = [];
        a.initializeMacStationNames = function() {
            if (this.localizedMac) {
                for (mac in this.localizedMac) {
                    if (this.stationHash[this.localizedMac[mac].StationCode]) {
                        this.stationHash[this.localizedMac[mac].StationCode].name = this.localizedMac[mac].StationName
                    }
                }
            }
        };
        a.restrictedStationCategories = [];
        a.restrictedPairStationCategory = [];
        a.toggleMarket = function() {
            if ((a.toggleMarketCount % 2) === 0) {
                $(a.container).hide();
                $(a.containerText).show()
            } else {
                $(a.container).show();
                $(a.containerText).hide()
            }
            a.toggleMarketCount += 1
        };
        a.useComboBox = function(c) {
            var e = true;
            if (c && c.get(0) && c.get(0).options) {
                e = false
            }
            return e
        };
        a.updateMarketOrigin = function() {
            var e = $(this).val();
            if (e == "") {
                var e = $("#" + marketOriginalId).val()
            }
            e = e.toUpperCase();
            var n = a.marketHash[e];
            n = n || [];
            var f = null;
            var m = true;
            var c = a.stationHash[e];
            m = a.useComboBox(a.destination);
            if (m) {
                f = {
                    input: a.destination,
                    options: n,
                    optionHeaderText: a.optionHeaderText
                };
                SKYSALES.Class.DropDown.getDropDown(f)
            } else {
                var l = [];
                if (a.promoStationsArray && a.promoStationsArray.length > 0) {
                    for (stationArrayIndex in a.promoStationsArray) {
                        if (e == a.promoStationsArray[stationArrayIndex].originStation) {
                            l = a.promoStationsArray[stationArrayIndex].destStationArray;
                            break
                        }
                    }
                }
                f = {
                    selectBox: a.destination,
                    promoStationsArray: l,
                    objectArray: n,
                    groupArray: a.countryStationHash,
                    showCode: true,
                    optionHeaderText: a.optionHeaderText,
                    restrictedStationCategory: a.restrictedStationCategories,
                    restrictedPairStationCategory: a.restrictedPairStationCategory,
                    originObject: c
                };
                SKYSALES.Util.populateSelectWithGroups(f)
            }
        };
        a.addEvents = function() {
            b.addEvents.call(this);
            a.origin.change(a.updateMarketOrigin);
            a.disableInput.click(a.toggleMarket)
        };
        a.setVars = function() {
            b.setVars.call(this);
            a.container = $("#" + a.containerId);
            a.containerText = $("#" + a.containerTextId);
            a.disableInput = $("#" + a.disableInputId);
            a.origin = $("#" + a.originId);
            a.destination = $("#" + a.destinationId)
        };
        a.populateMarketInput = function(c) {
            var l = true;
            var e = {};
            if ((c) && (c.length > 0)) {
                l = a.useComboBox(c);
                if (l) {
                    e = {
                        input: c,
                        options: this.stationHash,
                        optionHeaderText: a.optionHeaderText,
                        restrictedStationCategory: this.restrictedStationCategories
                    };
                    SKYSALES.Class.DropDown.getDropDown(e)
                } else {
                    var f = [];
                    if (a.promoStationsArray && a.promoStationsArray.length > 0) {
                        for (stationArrayIndex in a.promoStationsArray) {
                            f.push(a.promoStationsArray[stationArrayIndex].originStation)
                        }
                    }
                    e = {
                        selectBox: c,
                        promoStationsArray: f,
                        objectArray: this.stationHash,
                        groupArray: a.countryStationHash,
                        showCode: true,
                        optionHeaderText: a.optionHeaderText,
                        restrictedStationCategory: this.restrictedStationCategories
                    };
                    SKYSALES.Util.populateSelectWithGroups(e)
                }
            }
        };
        a.init = function(c) {
            b.init.call(this, c);
            this.addEvents();
            a.initializeMacStationNames();
            a.populateMarketInput(a.origin);
            a.populateMarketInput(a.destination);
            a.disableInput.click();
            a.disableInput.removeAttr("checked")
        };
        return a
    }
}
if (!SKYSALES.Class.StationInput) {
    SKYSALES.Class.StationInput = function() {
        var b = new SKYSALES.Class.SkySales();
        var a = SKYSALES.Util.extendObject(b);
        a.stationInputId = "";
        a.stationInput = null;
        a.setVars = function() {
            b.setVars.call(this);
            a.stationInput = $("#" + this.stationInputId)
        };
        a.init = function(c) {
            b.init.call(this, c);
            this.addEvents()
        };
        return a
    }
}
if (!SKYSALES.Class.StationDropDown) {
    SKYSALES.Class.StationDropDown = function() {
        var b = new SKYSALES.Class.SkySales();
        var a = SKYSALES.Util.extendObject(b);
        a.selectBoxId = "";
        a.selectBox = null;
        a.inputId = "";
        a.input = null;
        a.updateStationDropDown = function() {
            var f = $(this).val();
            if (f != "") {
                try {
                    a.selectBox.val(f)
                } catch (c) {}
            } else {
                $(this).val(this.defaultValue)
            }
        };
        a.updateStationInput = function() {
            var c = $(this).val();
            a.input.val(c);
            a.input.change()
        };
        a.addEvents = function() {
            a.input.change(a.updateStationDropDown);
            a.selectBox.change(a.updateStationInput)
        };
        a.setVars = function() {
            a.selectBox = $("#" + a.selectBoxId);
            a.input = $("#" + a.inputId)
        };
        a.init = function(c) {
            b.init.call(this, c);
            this.addEvents();
            a.input.change()
        };
        return a
    }
}
if (!SKYSALES.Class.TravelDocumentInput) {
    SKYSALES.Class.TravelDocumentInput = function() {
        var b = new SKYSALES.Class.SkySales();
        var a = SKYSALES.Util.extendObject(b);
        a.instanceName = "";
        a.delimiter = "_";
        a.travelDocumentInfoId = "";
        a.travelDocumentInfo = null;
        a.documentNumberId = "";
        a.documentNumber = null;
        a.documentTypeId = "";
        a.documentType = null;
        a.documentIssuingCountryId = "";
        a.documentIssuingCountry = null;
        a.documentExpYearId = "";
        a.documentExpYear = null;
        a.documentExpMonthId = "";
        a.documentExpMonth = null;
        a.documentExpDayId = "";
        a.documentExpDay = null;
        a.actionId = "";
        a.action = null;
        a.travelDocumentKey = "";
        a.missingDocumentText = "";
        a.missingDocumentTypeText = "";
        a.invalidExpDateText = "";
        a.emptyExpDateText = "";
        a.invalidDaysOfMonthTextPre = "";
        a.invalidDaysOfMonthTextMid = "";
        a.invalidDaysOfMonthTextPost = "";
        a.missingDocumentNumberText = "";
        a.missingDocumentCountryText = "";
        a.init = function(c) {
            this.setSettingsByObject(c);
            this.setVars();
            this.addEvents()
        };
        a.setVars = function() {
            a.travelDocumentInfo = this.getById(this.travelDocumentInfoId);
            a.documentType = this.getById(this.documentTypeId);
            a.documentNumber = this.getById(this.documentNumberId);
            a.documentIssuingCountry = this.getById(this.documentIssuingCountryId);
            a.documentExpYear = this.getById(this.documentExpYearId);
            a.documentExpMonth = this.getById(this.documentExpMonthId);
            a.documentExpDay = this.getById(this.documentExpDayId);
            a.action = this.getById(this.actionId)
        };
        a.setTravelDocumentInfo = function() {
            var l = "";
            var c = this.documentType.val();
            var e = this.documentNumber.val();
            var f = this.documentIssuingCountry.val();
            if (c && e && f) {
                l = this.delimiter + c + this.delimiter + e + this.delimiter + f;
                this.travelDocumentInfo.val(l)
            }
            return true
        };
        a.validateTravelDocumentHandler = function() {
            var c = a.validateTravelDocument();
            return c
        };
        a.validateTravelDocument = function() {
            this.setTravelDocumentInfo();
            var e = this.action.get(0);
            var c = window.validate(e) && this.validateInput();
            return c
        };
        a.addEvents = function() {
            this.action.click(this.validateTravelDocumentHandler)
        };
        a.validateInput = function() {
            var c = true;
            var e = "";
            var q = "";
            var u = this.documentNumber.val() || "";
            var p = this.documentExpYear.val() || "";
            var f = this.documentExpMonth.val() || "";
            var l = this.documentExpDay.val() || "";
            var t = this.documentType.val() || "";
            var r = this.documentIssuingCountry.val() || "";
            var m = false;
            var n = false;
            var s = "";
            if (u || t || r || p || f || l) {
                if (!u) {
                    e = e + this.missingDocumentNumberText + "\n"
                }
                if (!t) {
                    e = e + this.missingDocumentTypeText + "\n"
                }
                if (!r) {
                    e = e + this.missingDocumentCountryText + "\n"
                }
                n = this.checkDaysOfMonth(l, f, p);
                m = this.isPastDate(l, f, p);
                if (l && f && p) {
                    if (!n) {
                        s = this.documentExpMonth.find(":selected").text();
                        q = this.invalidDaysOfMonthTextPre + l;
                        q += this.invalidDaysOfMonthTextMid + s + this.invalidDaysOfMonthTextPost;
                        e = e + q + "\n"
                    } else {
                        if (!m) {
                            e = e + this.invalidExpDateText + "\n"
                        }
                    }
                } else {
                    e = e + this.emptyExpDateText + "\n"
                }
                if (e) {
                    window.alert(this.missingDocumentText + "\n" + e);
                    c = false
                }
            }
            return c
        };
        a.checkDaysOfMonth = function(f, p, l) {
            l = window.parseInt(l, 10);
            p = window.parseInt(p, 10);
            f = window.parseInt(f, 10);
            var n = false;
            var m = null;
            var c = -1;
            var e = null;
            if (l && p && f) {
                p -= 1;
                m = new Date();
                m.setMonth(2);
                m.setDate(1);
                m.setDate(m.getDate() - 1);
                c = m.getDate();
                e = [31, c, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];
                if (f <= e[p]) {
                    n = true
                }
            }
            return n
        };
        a.isPastDate = function(e, n, l) {
            l = window.parseInt(l, 10);
            n = window.parseInt(n, 10);
            e = window.parseInt(e, 10);
            var m = false;
            var c = null;
            var f = null;
            if (l && n && e) {
                n -= 1;
                c = new Date();
                f = new Date(l, n, e);
                if (f > c) {
                    m = true
                }
            }
            return m
        };
        return a
    }
}
if (!SKYSALES.Class.ControlGroup) {
    SKYSALES.Class.ControlGroup = function() {
        var b = new SKYSALES.Class.SkySales();
        var a = SKYSALES.Util.extendObject(b);
        a.actionId = "SkySales";
        a.action = null;
        a.init = function(c) {
            this.setSettingsByObject(c);
            this.setVars();
            this.addEvents()
        };
        a.setVars = function() {
            b.setVars.call(this);
            a.action = $("#" + this.actionId)
        };
        a.addEvents = function() {
            b.addEvents.call(this);
            this.action.click(this.validateHandler)
        };
        a.validateHandler = function() {
            var c = a.validate();
            return c
        };
        a.validate = function() {
            var c = this.action.get(0);
            var e = window.validate(c);
            return e
        };
        return a
    };
    SKYSALES.Class.ControlGroup.createObject = function(a) {
        SKYSALES.Util.createObject("controlGroup", "ControlGroup", a)
    }
}
if (!SKYSALES.Class.ControlGroupRegister) {
    SKYSALES.Class.ControlGroupRegister = function() {
        var b = new SKYSALES.Class.ControlGroup();
        var a = SKYSALES.Util.extendObject(b);
        a.travelDocumentInput = null;
        a.setSettingsByObject = function(c) {
            b.setSettingsByObject.call(this, c);
            var e = new SKYSALES.Class.TravelDocumentInput();
            e.init(this.travelDocumentInput);
            a.travelDocumentInput = e
        };
        a.validateHandler = function() {
            var c = a.validate();
            return c
        };
        a.validate = function() {
            var c = false;
            c = (this.travelDocumentInput.setTravelDocumentInfo() && this.travelDocumentInput.validateExpDate());
            if (c) {
                c = b.validate.call(this)
            }
            return c
        };
        return a
    };
    SKYSALES.Class.ControlGroupRegister.createObject = function(a) {
        SKYSALES.Util.createObject("controlGroupRegister", "ControlGroupRegister", a)
    }
}
if (!SKYSALES.Class.ContactInput) {
    SKYSALES.Class.ContactInput = function() {
        var a = new SKYSALES.Class.SkySales();
        var b = SKYSALES.Util.extendObject(a);
        b.clientId = "";
        b.keyIdArray = [];
        b.keyArray = [];
        b.clientStoreIdHash = null;
        b.countryInputId = "";
        b.countryInput = null;
        b.stateInputId = "";
        b.stateInput = null;
        b.countryStateHash = null;
        b.imContactId = "";
        b.imContact = null;
        b.currentContactData = {};
        b.logOutButton = null;
        b.homePhone = null;
        b.workPhone = null;
        b.otherPhone = null;
        b.faxPhone = null;
        b.homePhoneId = "";
        b.workPhoneId = "";
        b.otherPhoneId = "";
        b.faxPhoneId = "";
        b.dropdownTitleId = "";
        b.dropdownGenderId = "";
        b.dropdownTitle = null;
        b.dropdownGender = null;
        b.selectCountryText = "";
        b.selectStateText = "";
        b.stateArray = [];
        b.clientHash = SKYSALES.Util.getResource().clientHash;
        b.setSettingsByObject = function(c) {
            a.setSettingsByObject.call(this, c);
            var e = "";
            for (e in c) {
                if (b.hasOwnProperty(e)) {
                    b[e] = c[e]
                }
            }
        };
        b.clearCurrentContact = function() {
            $("#" + b.clientId + "_DropDownListTitle").val("");
            $("#" + b.clientId + "_TextBoxAddressLine1").val("");
            $("#" + b.clientId + "_TextBoxAddressLine2").val("");
            $("#" + b.clientId + "_TextBoxAddressLine3").val("");
            $("#" + b.clientId + "_TextBoxCity").val("");
            $("#" + b.clientId + "_DropDownListStateProvince").val("");
            $("#" + b.clientId + "_DropDownListCountry").val(b.selectCountryText);
            $("#" + b.clientId + "_TextBoxPostalCode").val("");
            $("#" + b.clientId + "_TextBoxHomePhone").val("");
            $("#" + b.clientId + "_TextBoxWorkPhone").val("");
            $("#" + b.clientId + "_TextBoxOtherPhone").val("");
            $("#" + b.clientId + "_TextBoxFax").val("");
            $("#" + b.clientId + "_TextBoxEmailAddress").val("")
        };
        b.populateCurrentContact = function() {
            if (b.currentContactData) {
                if (b.imContact.attr("checked") === true) {
                    $("#" + b.clientId + "_DropDownListTitle").val(b.currentContactData.title);
                    if (b.currentContactData.firstName != "") {
                        $("#" + b.clientId + "_TextBoxFirstName").val(b.currentContactData.firstName)
                    } else {
                        $("#" + b.clientId + "_TextBoxFirstName").val(b.currentContactData.lastName)
                    }
                    $("#" + b.clientId + "_TextBoxMiddleName").val(b.currentContactData.middleName);
                    $("#" + b.clientId + "_TextBoxLastName").val(b.currentContactData.lastName);
                    $("#" + b.clientId + "_TextBoxAddressLine1").val(b.currentContactData.streetAddressOne);
                    $("#" + b.clientId + "_TextBoxAddressLine2").val(b.currentContactData.streetAddressTwo);
                    $("#" + b.clientId + "_TextBoxAddressLine3").val(b.currentContactData.streetAddressThree);
                    $("#" + b.clientId + "_TextBoxCity").val(b.currentContactData.city);
                    $("#" + b.clientId + "_DropDownListCountry").val(b.currentContactData.country);
                    b.updateState();
                    $("#" + b.clientId + "_DropDownListStateProvince").val(b.currentContactData.stateProvince);
                    $("#" + b.clientId + "_TextBoxPostalCode").val(b.currentContactData.postalCode);
                    $("#" + b.clientId + "_TextBoxHomePhone").val(b.currentContactData.eveningPhone);
                    $("#" + b.clientId + "_TextBoxWorkPhone").val(b.currentContactData.dayPhone);
                    $("#" + b.clientId + "_TextBoxOtherPhone").val(b.currentContactData.mobilePhone);
                    $("#" + b.clientId + "_TextBoxFax").val(b.currentContactData.faxPhone);
                    $("#" + b.clientId + "_TextBoxEmailAddress").val(b.currentContactData.email)
                } else {
                    b.clearCurrentContact()
                }
            }
        };
        b.populateCountrySelect = function() {
            var f = "";
            if ($(b.imContact).is(":checked") || (!$(b.imContact).is(":visible") && $("#" + b.countryInputId + "_value").val() != "")) {
                f = $("#" + b.countryInputId + "_value").val()
            } else {
                f = $("#" + b.countryInputId).val()
            }
            var c = SKYSALES.Util.getResource().countryInfo.CountryList;
            c.unshift({
                InternationalDialCode: "",
                code: "",
                name: b.selectCountryText
            });
            var e = {
                selectBox: $("#" + b.countryInputId),
                objectArray: c,
                selectedItem: f,
                showCode: false
            };
            SKYSALES.Util.populateSelect(e)
        };
        b.updateCountry = function() {
            if (b.stateArray.length == 0) {
                var c = SKYSALES.Util.getResource().stateInfo.StateList;
                var e = b.stateInput.val();
                for (stateIndex in c) {
                    if (c[stateIndex].ProvinceStateCode == e) {
                        b.countryInput.val(c[stateIndex].CountryCode);
                        break
                    }
                }
            }
        };
        b.updateStateAndPhoneNumbers = function() {
            b.updatePhoneNumbers();
            b.updateState()
        };
        b.updatePhoneNumbers = function() {
            var e = b.countryInput.val();
            var c = SKYSALES.Util.getResource().countryInfo.CountryList;
            for (i = 0; i < c.length; i += 1) {
                country = c[i];
                if (e == country.code) {
                    b.homePhone.val(country.InternationalDialCode);
                    b.workPhone.val(country.InternationalDialCode);
                    b.otherPhone.val(country.InternationalDialCode);
                    b.faxPhone.val(country.InternationalDialCode);
                    break
                }
            }
        };
        b.updateState = function() {
            var c = SKYSALES.Util.getResource().stateInfo.StateList;
            var n = b.countryInput.val();
            var e = [];
            var l = {};
            var p = [];
            if (n !== "") {
                var f = 0;
                for (stateIndex in c) {
                    if (c[stateIndex].CountryCode == n) {
                        e.push(c[stateIndex])
                    }
                }
                e = e || [];
                b.stateArray = e;
                if (e.length === 0) {
                    e = c
                }
            }
            if (e.length == 0 || e[0].code != "") {
                e.unshift({
                    InternationalDialCode: "",
                    code: "",
                    name: b.selectStateText || "Select State"
                })
            }
            var m = {
                objectArray: e,
                selectBox: b.stateInput,
                showCode: false,
                clearOptions: true
            };
            SKYSALES.Util.populateSelect(m)
        };
        b.getKey = function() {
            var l = 0;
            var c = b.keyArray;
            var e = null;
            var f = "";
            for (l = 0; l < c.length; l += 1) {
                e = c[l];
                f += e.val()
            }
            f = f.replace(/\s+/g, "+");
            f = b.clientId + "_" + (f.toLowerCase());
            return f
        };
        b.populateClientStoreIdHash = function() {
            var q = b.clientHash;
            var m = 0;
            var l = "";
            var e = [];
            var c = "";
            var p = -1;
            var f = b.getKey();
            var n = null;
            b.clientStoreIdHash = {};
            if (f && q && q[f]) {
                b.clientStoreIdHash = b.clientStoreIdHash || {};
                l = q[f];
                e = l.split("&");
                for (m = 0; m < e.length; m += 1) {
                    c = e[m];
                    p = c.indexOf("=");
                    if (p > -1) {
                        f = c.substring(0, p);
                        n = c.substring(p + 1, c.length);
                        if (f) {
                            b.clientStoreIdHash[f] = n
                        }
                    }
                }
            }
        };
        b.autoPopulateForm = function() {
            b.populateClientStoreIdHash();
            var c = b.clientStoreIdHash;
            var e = "";
            var f = "";
            for (e in c) {
                if (c.hasOwnProperty(e)) {
                    f = c[e];
                    $("#" + e).val(f)
                }
            }
            b.initializeState()
        };
        b.initializeState = function() {
            if ($("#" + b.countryInputId + " option").length < 1) {
                b.populateCountrySelect()
            }
            if (this.countryInput.val() !== "") {
                var e = "";
                var c = [];
                if ($(b.imContact).is(":checked") || (!$(b.imContact).is(":visible") && $("#" + b.stateInputId + "_value").val() != "") && $("#" + b.stateInputId + "_value").val() != undefined) {
                    e = $("#" + b.stateInputId + "_value").val() || ""
                } else {
                    e = $("#" + b.stateInputId).val() || "";
                    if (e == "" && b.clientStoreIdHash) {
                        e = b.clientStoreIdHash[b.stateInputId]
                    }
                }
                if (e) {
                    c = e.split("|");
                    if (c.length == 2) {
                        e = c[1]
                    }
                }
                b.updateState();
                if (e != "") {
                    b.stateInput.val(e)
                }
            } else {
                b.updateState()
            }
        };
        b.updateGender = function() {
            var c = b.dropdownTitle.val();
            var e = SKYSALES.Util.getResource().titleInfo.TitleList;
            for (i = 0; i < e.length; i += 1) {
                title = e[i];
                if (c == title.TitleKey) {
                    b.dropdownGender.val(title.GenderCode);
                    break
                }
            }
        };
        b.updateTitle = function() {
            var c = b.dropdownGender.val();
            var e = b.dropdownTitle.val();
            var f = SKYSALES.Util.getResource().titleInfo.TitleList;
            for (i = 0; i < f.length; i += 1) {
                title = f[i];
                if (e != "CHD" && c == title.GenderCode) {
                    b.dropdownTitle.val(title.TitleKey);
                    break
                }
            }
        };
        b.addEvents = function() {
            a.addEvents.call(this);
            var f = 0;
            var c = b.keyArray;
            var e = null;
            for (f = 0; f < c.length; f += 1) {
                e = c[f];
                e.change(b.autoPopulateForm)
            }
            b.countryInput.change(b.updateStateAndPhoneNumbers);
            b.stateInput.change(b.updateCountry);
            b.imContact.click(b.populateCurrentContact);
            b.logOutButton.click(b.clearCurrentContact);
            b.dropdownTitle.change(this.updateGender);
            b.dropdownGender.change(this.updateTitle)
        };
        b.setVars = function() {
            a.setVars.call(this);
            var e = 0;
            var f = b.keyIdArray;
            var c = b.keyArray;
            var l = "";
            for (e = 0; e < f.length; e += 1) {
                l = f[e];
                c[c.length] = $("#" + l)
            }
            b.countryInput = $("#" + b.countryInputId);
            b.stateInput = $("#" + b.stateInputId);
            b.imContact = $("#" + b.imContactId);
            b.logOutButton = $("#MemberLoginContactView_ButtonLogOut");
            b.homePhone = $("#" + b.homePhoneId);
            b.workPhone = $("#" + b.workPhoneId);
            b.otherPhone = $("#" + b.otherPhoneId);
            b.faxPhone = $("#" + b.faxPhoneId);
            b.dropdownTitle = $("#" + this.dropdownTitleId);
            b.dropdownGender = $("#" + this.dropdownGenderId);
            if ($("#" + b.clientId + "_TextBoxFirstName").val() == "") {
                $("#" + b.clientId + "_TextBoxFirstName").val(b.currentContactData.lastName)
            }
        };
        b.init = function(c) {
            this.setSettingsByObject(c);
            this.setVars();
            this.addEvents();
            this.initializeState()
        };
        return b
    };
    SKYSALES.Class.ContactInput.createObject = function(a) {
        SKYSALES.Util.createObject("contactInput", "ContactInput", a)
    }
}
if (!SKYSALES.Class.ToggleView) {
    SKYSALES.Class.ToggleView = function() {
        var b = new SKYSALES.Class.SkySales();
        var a = SKYSALES.Util.extendObject(b);
        a.showId = "";
        a.hideId = "";
        a.elementId = "";
        a.show = null;
        a.hide = null;
        a.element = null;
        a.setVars = function() {
            b.setVars.call(this);
            a.show = $("#" + a.showId);
            a.hide = $("#" + a.hideId);
            a.element = $("#" + a.elementId)
        };
        a.init = function(c) {
            this.setSettingsByObject(c);
            this.setVars();
            this.addEvents()
        };
        a.updateShowHandler = function() {
            a.element.show("slow")
        };
        a.updateHideHandler = function() {
            a.element.hide()
        };
        a.addEvents = function() {
            b.addEvents.call(this);
            a.show.click(a.updateShowHandler);
            a.hide.click(a.updateHideHandler)
        };
        return a
    }
}
if (!SKYSALES.Class.PaymentInputContainer) {
    SKYSALES.Class.PaymentInputContainer = function() {
        var a = SKYSALES.Class.SkySales();
        thisPaymentInputContainer = SKYSALES.Util.extendObject(a);
        thisPaymentInputContainer.labelConvenienceFeeId = "";
        thisPaymentInputContainer.voucherNumberTextBoxId = "";
        thisPaymentInputContainer.voucherNumberTextBox = "";
        thisPaymentInputContainer.labelConvenienceFee = null;
        thisPaymentInputContainer.validBinRange = "";
        thisPaymentInputContainer.invalidBinRangeMsg = "";
        thisPaymentInputContainer.buttonSubmitProxyId = "ButtonSubmitProxy";
        thisPaymentInputContainer.buttonSubmitProxy = null;
        thisPaymentInputContainer.buttonSubmitId = "CONTROLGROUPPAYMENTBOTTOM_ButtonSubmit";
        thisPaymentInputContainer.defaultCreditCardsArray = null;
        thisPaymentInputContainer.dropDownListPaymentMethodId = "";
        thisPaymentInputContainer.dropDownListPaymentMethod = null;
        thisPaymentInputContainer.registeredCCArray = null;
        thisPaymentInputContainer.ccPaymentFeesArray = null;
        thisPaymentInputContainer.ccPaymentControlName = "";
        thisPaymentInputContainer.ccPaymentFeeLabel = "";
        thisPaymentInputContainer.ccPaymentFeePaxLabel = "";
        thisPaymentInputContainer.ccPaymentFeeWayLabel = "";
        thisPaymentInputContainer.ccPaymentFeeSegmentCount = "";
        thisPaymentInputContainer.ccPaymentFeePaxCount = "";
        thisPaymentInputContainer.selectedCreditCard = "";
        thisPaymentInputContainer.dropDownMCCId = "";
        thisPaymentInputContainer.buttonApplyMCCId = "";
        thisPaymentInputContainer.hiddenUpdatedMCCId = "";
        thisPaymentInputContainer.dropDownMCC = null;
        thisPaymentInputContainer.buttonApplyMCC = null;
        thisPaymentInputContainer.hiddenUpdatedMCC = null;
        thisPaymentInputContainer.upSellPrompt = "";
        thisPaymentInputContainer.mccNoticePrompt = "";
        thisPaymentInputContainer.mccCurrencyCode = "";
        thisPaymentInputContainer.mccEnabled = "";
        thisPaymentInputContainer.issuingCountrySelectId = "";
        thisPaymentInputContainer.countryCurrencyHash = {};
        thisPaymentInputContainer.isModifiedBooking = "";
        thisPaymentInputContainer.manageMyBookingPmtAlert = "";
        var b = 0;
        thisPaymentInputContainer.setSettingsByObject = function(c) {
            a.setSettingsByObject.call(this, c)
        };
        thisPaymentInputContainer.setVars = function() {
            thisPaymentInputContainer.labelConvenienceFee = this.getById(this.labelConvenienceFeeId);
            thisPaymentInputContainer.voucherNumberTextBox = this.getById(this.voucherNumberTextBoxId);
            thisPaymentInputContainer.buttonSubmitProxy = this.getById(this.buttonSubmitProxyId);
            thisPaymentInputContainer.dropDownListPaymentMethod = this.getById(this.dropDownListPaymentMethodId);
            thisPaymentInputContainer.dropDownMCC = this.getById(this.dropDownMCCId);
            thisPaymentInputContainer.buttonApplyMCC = this.getById(this.buttonApplyMCCId);
            thisPaymentInputContainer.hiddenUpdatedMCC = this.getById(this.hiddenUpdatedMCCId)
        };
        thisPaymentInputContainer.previewConvenienceFeeDivHandler = function() {
            $("#convenienceFeePreview").show()
        };
        thisPaymentInputContainer.hideConvenienceFeeDivHandler = function() {
            $("#convenienceFeePreview").hide()
        };
        thisPaymentInputContainer.addEvents = function() {
            this.labelConvenienceFee.hover(thisPaymentInputContainer.previewConvenienceFeeDivHandler, thisPaymentInputContainer.hideConvenienceFeeDivHandler);
            this.buttonSubmitProxy.click(thisPaymentInputContainer.validateCC);
            this.dropDownListPaymentMethod.change(thisPaymentInputContainer.showSelectedCC);
            $("#" + this.buttonSubmitId).click(this.prompterHandler);
            $("input:radio[name='" + this.ccPaymentControlName + "']").click(function() {
                var c = $(this).val();
                thisPaymentInputContainer.selectCC(c)
            });
            if (this.hiddenUpdatedMCC.length > 0) {
                this.buttonApplyMCC.click(thisPaymentInputContainer.updateCurrency)
            }
        };
        thisPaymentInputContainer.prompterHandler = function() {
            return thisPaymentInputContainer.showCurrencyPrompter()
        };
        thisPaymentInputContainer.showCurrencyPrompter = function() {
            if (this.defaultCreditCardsArray.length > 0) {
                var f = this.defaultCreditCardsArray[this.getActivePayment()],
                    l = $("#" + f.issuingCountrySelectId).val() || "",
                    n = this.defaultCreditCardsArray[b].billingContentId,
                    e = $("input:radio[name='" + this.ccPaymentControlName + "']:checked").val(),
                    c = validateBySelector("td[id='" + this.selectedCreditCard + "_CVV_" + e + "']") && validateBySelector("div[id='" + n + "']"),
                    m = this.countryCurrencyHash[l] || null;
                if (c && this.mccEnabled === "true" && this.selectedCreditCard === "") {
                    if (this.mccCurrencyCode != "") {
                        return confirm(this.mccNoticePrompt)
                    } else {
                        if (m != null && f.currencyCode != m.currencyCode) {
                            return confirm(this.upSellPrompt)
                        }
                    }
                }
            }
            return true
        };
        thisPaymentInputContainer.updateCurrency = function() {
            var c = thisPaymentInputContainer.dropDownMCC.val();
            thisPaymentInputContainer.hiddenUpdatedMCC.val(c)
        };
        thisPaymentInputContainer.init = function(l) {
            this.setSettingsByObject(l);
            this.setVars();
            this.addEvents();
            if (this.voucherNumberTextBox != null) {
                this.voucherNumberTextBox.val("")
            }
            var f = 0;
            var e = this.defaultCreditCardsArray || [];
            var p = null;
            for (f = 0; f < e.length; f += 1) {
                p = new SKYSALES.Class.PaymentInput();
                p.init(e[f]);
                this.defaultCreditCardsArray[f] = p;
                p.hideContent()
            }
            if (this.defaultCreditCardsArray.length > 0) {
                var n = this.getActivePayment();
                this.defaultCreditCardsArray[n].showContent();
                this.displayConvenienceFee(this.defaultCreditCardsArray[n].ccId)
            }
            var c = SKYSALES.Util.getResource().countryInfo.CountryList;
            for (f = 0; f < c.length; f = f + 1) {
                this.countryCurrencyHash[c[f].code] = {
                    currencyCode: c[f].currency
                }
            }
            var m = $("input:radio[name='" + thisPaymentInputContainer.ccPaymentControlName + "']:enabled");
            if (m.length > 0) {
                if (m.length > 1) {
                    thisPaymentInputContainer.selectedCreditCard = thisPaymentInputContainer.registeredCCArray[0].brand;
                    thisPaymentInputContainer.displayConvenienceFee(thisPaymentInputContainer.selectedCreditCard);
                    $("#" + thisPaymentInputContainer.registeredCCArray[0].CVV).removeAttr("disabled");
                    thisPaymentInputContainer.showBillingContent(thisPaymentInputContainer.registeredCCArray[0].brand);
                    thisPaymentInputContainer.hideDefaultCCs()
                } else {
                    if (m.length == 1) {
                        thisPaymentInputContainer.showDefaultCCs();
                        thisPaymentInputContainer.displayConvenienceFee(thisPaymentInputContainer.defaultCreditCardsArray[thisPaymentInputContainer.getActivePayment()].ccId);
                        thisPaymentInputContainer.showBillingContent(thisPaymentInputContainer.defaultCreditCardsArray[thisPaymentInputContainer.getActivePayment()].ccId)
                    }
                }
                m[0].checked = true
            }
        };
        thisPaymentInputContainer.getActivePayment = function() {
            var e = 0;
            var c = 0;
            for (e = 0; e < thisPaymentInputContainer.defaultCreditCardsArray.length; e++) {
                if (thisPaymentInputContainer.defaultCreditCardsArray[e].contentId == "content_" + thisPaymentInputContainer.dropDownListPaymentMethod.val()) {
                    c = e;
                    break
                }
            }
            return c
        };
        thisPaymentInputContainer.showSelectedCC = function() {
            var e = 0;
            var c = thisPaymentInputContainer.getActivePayment();
            for (e = 0; e < thisPaymentInputContainer.defaultCreditCardsArray.length; e++) {
                thisPaymentInputContainer.defaultCreditCardsArray[e].hideContent()
            }
            thisPaymentInputContainer.defaultCreditCardsArray[c].showContent();
            thisPaymentInputContainer.displayConvenienceFee(thisPaymentInputContainer.defaultCreditCardsArray[c].ccId)
        };
        thisPaymentInputContainer.hideDefaultCCs = function() {
            thisPaymentInputContainer.defaultCreditCardsArray[thisPaymentInputContainer.getActivePayment()].clearCCForm();
            $("#" + thisPaymentInputContainer.registeredCCArray[thisPaymentInputContainer.registeredCCArray.length - 1].brand + "_div").hide()
        };
        thisPaymentInputContainer.showDefaultCCs = function() {
            $("#" + thisPaymentInputContainer.registeredCCArray[thisPaymentInputContainer.registeredCCArray.length - 1].brand + "_div").show()
        };
        thisPaymentInputContainer.validateCC = function() {
            $.post("SessionRefresh.aspx");
            var n = true;
            if (thisPaymentInputContainer.selectedCreditCard == "" && thisPaymentInputContainer.defaultCreditCardsArray.length > 0) {
                var q = thisPaymentInputContainer.defaultCreditCardsArray[thisPaymentInputContainer.getActivePayment()];
                var f = $("#" + q.accountNumberInputId).val();
                if (thisPaymentInputContainer.validBinRange != "" && f != "") {
                    var s = false;
                    var m = thisPaymentInputContainer.validBinRange.split(",");
                    var l = m.length;
                    for (i = 0; i < l; i++) {
                        var e = m[i];
                        if (e == f.substring(0, e.length)) {
                            s = true;
                            break
                        }
                    }
                    if (s == false) {
                        alert(thisPaymentInputContainer.invalidBinRangeMsg);
                        n = false
                    }
                }
                n = n && validateBySelector("div[id='" + q.contentId + "']") && validateBySelector("div[id='" + q.billingContentId + "']")
            } else {
                if (thisPaymentInputContainer.selectedCreditCard != "") {
                    var r = thisPaymentInputContainer.defaultCreditCardsArray[b].billingContentId;
                    var p = $("input:radio[name='" + thisPaymentInputContainer.ccPaymentControlName + "']:checked").val();
                    n = validateBySelector("td[id='" + thisPaymentInputContainer.selectedCreditCard + "_CVV_" + p + "']") && validateBySelector("div[id='" + r + "']")
                } else {
                    if (!thisPaymentInputContainer.defaultCreditCardsArray.length > 0) {
                        var c = ($("#" + thisPaymentInputContainer.buttonSubmitId)).get(0);
                        n = validate(c)
                    }
                }
            }
            if (n == true) {
                if (thisPaymentInputContainer.isModifiedBooking == "True") {
                    alert(thisPaymentInputContainer.manageMyBookingPmtAlert)
                }
                document.getElementById(thisPaymentInputContainer.buttonSubmitId).click()
            }
            return false
        };
        thisPaymentInputContainer.selectCC = function(e) {
            thisPaymentInputContainer.selectedCreditCard = "";
            var l = null;
            var c = 0;
            var f = 0;
            for (f = 0; f < thisPaymentInputContainer.registeredCCArray.length; f++) {
                l = $("#" + thisPaymentInputContainer.registeredCCArray[f].CVV);
                if (l != null && l.length > 0) {
                    l.val("");
                    if (thisPaymentInputContainer.registeredCCArray[f].virtualNo == e) {
                        selecteddIndex = f;
                        thisPaymentInputContainer.selectedCreditCard = thisPaymentInputContainer.registeredCCArray[f].brand
                    }
                    l.attr("disabled", "disabled");
                    l.removeClass("validationError")
                }
            }
            if (thisPaymentInputContainer.selectedCreditCard != "") {
                thisPaymentInputContainer.hideDefaultCCs();
                l = $("#" + thisPaymentInputContainer.registeredCCArray[selecteddIndex].CVV);
                l.removeAttr("disabled");
                l.select();
                thisPaymentInputContainer.displayConvenienceFee(thisPaymentInputContainer.selectedCreditCard);
                thisPaymentInputContainer.showBillingContent(thisPaymentInputContainer.registeredCCArray[selecteddIndex].brand)
            } else {
                thisPaymentInputContainer.showDefaultCCs();
                thisPaymentInputContainer.displayConvenienceFee(thisPaymentInputContainer.defaultCreditCardsArray[thisPaymentInputContainer.getActivePayment()].ccId);
                thisPaymentInputContainer.showBillingContent(thisPaymentInputContainer.defaultCreditCardsArray[thisPaymentInputContainer.getActivePayment()].ccId)
            }
        };
        thisPaymentInputContainer.displayConvenienceFee = function(e) {
            var f = 0;
            var c = false;
            var l = thisPaymentInputContainer.ccPaymentFeeLabel;
            for (f = 0; f < thisPaymentInputContainer.ccPaymentFeesArray.length; f++) {
                if (e == thisPaymentInputContainer.ccPaymentFeesArray[f].brand) {
                    c = true;
                    $("#paymentFee_Amount").text(thisPaymentInputContainer.ccPaymentFeesArray[f].convenienceFee);
                    $("#totalWithPaymentFee_Amount").text(thisPaymentInputContainer.ccPaymentFeesArray[f].paymentTotal);
                    l = l.concat(thisPaymentInputContainer.ccPaymentFeesArray[f].feeBase, " x ", thisPaymentInputContainer.ccPaymentFeeSegmentCount, thisPaymentInputContainer.ccPaymentFeeWayLabel, " x ", thisPaymentInputContainer.ccPaymentFeePaxCount, thisPaymentInputContainer.ccPaymentFeePaxLabel, thisPaymentInputContainer.ccPaymentFeesArray[f].convenienceFee);
                    $("#convenienceFeePreview").text(l)
                }
            }
            if (c == false) {
                $("#convenienceFee_Display").hide()
            } else {
                $("#convenienceFee_Display").show()
            }
        };
        thisPaymentInputContainer.showBillingContent = function(c) {
            var e = 0;
            b = 0;
            for (e = 0; e < thisPaymentInputContainer.defaultCreditCardsArray.length; e++) {
                thisPaymentInputContainer.defaultCreditCardsArray[e].billingContent.addClass("hidden");
                if (c == thisPaymentInputContainer.defaultCreditCardsArray[e].ccId) {
                    b = e
                }
            }
            thisPaymentInputContainer.defaultCreditCardsArray[b].billingContent.removeClass("hidden")
        };
        return thisPaymentInputContainer
    };
    SKYSALES.Class.PaymentInputContainer.createObject = function(a) {
        SKYSALES.Util.createObject("paymentInputContainer", "PaymentInputContainer", a)
    }
}
if (!SKYSALES.Class.PaymentInput) {
    SKYSALES.Class.PaymentInput = function() {
        var a = SKYSALES.Class.SkySales(),
            b = SKYSALES.Util.extendObject(a);
        b.dccOfferInfoId = "";
        b.foreignAmountId = "";
        b.foreignCurrencyId = "";
        b.foreignCurrencySymbolId = "";
        b.ownCurrencyAmountId = "";
        b.ownCurrencyId = "";
        b.ownCurrencySymbolId = "";
        b.rejectRadioBtnIdId = "";
        b.acceptRadioBtnIdId = "";
        b.doubleOptOutId = "";
        b.inlineDCCAjaxSucceededId = "";
        b.dccId = "";
        b.inlineDCCConversionLabelId = "";
        b.amountInputId = "";
        b.accountNumberInputId = "";
        b.inlineDCCOffer = null;
        b.currencyCode = null;
        b.feeAmt = null;
        b.issuingCountryTextBoxId = "";
        b.issuingCountrySelectId = "";
        b.billingCountryTextBoxId = "";
        b.billingCountrySelectId = "";
        b.billingStateTextBoxId = "";
        b.billingStateSelectId = "";
        b.billingStateSelectBoxInput = "";
        b.billingStateTextBoxInput = "";
        b.billingCountryInput = "";
        b.billingCountryTextBox = null;
        b.voucherNumberTextBoxId = "";
        b.voucherNumberTextBox = "";
        b.stateInfo = SKYSALES.Util.getResource().stateInfo;
        b.contentId = "";
        b.content = null;
        b.billingContentId = "";
        b.billingContent = null;
        b.cvvTextBoxId = "";
        b.cvvTextBox = null;
        b.acctHolderNameTextBoxId = "";
        b.acctHolderNameTextBox = null;
        b.bankNameTextBoxId = "";
        b.bankNameTextBox = null;
        b.ccId = "";
        b.setSettingsByObject = function(c) {
            a.setSettingsByObject.call(this, c)
        };
        b.setVars = function() {
            b.dcc = $("#" + this.dccId);
            b.inlineDCCConversionLabel = $("#" + this.inlineDCCConversionLabelId);
            b.accountNoTextBox = $("#" + this.accountNumberInputId);
            b.amountTextBox = $("#" + this.amountInputId);
            b.inlineDCCAjaxSucceeded = $("#" + this.inlineDCCAjaxSucceededId);
            b.billingStateSelectBoxInput = this.getById(this.billingStateSelectId);
            b.billingStateTextBoxInput = this.getById(this.billingStateTextBoxId);
            b.billingCountryInput = this.getById(this.billingCountrySelectId);
            b.content = this.getById(this.contentId);
            b.billingContent = this.getById(this.billingContentId);
            b.billingCountryTextBox = this.getById(this.billingCountryTextBoxId);
            b.cvvTextBox = $("#" + this.cvvTextBoxId);
            b.acctHolderNameTextBox = $("#" + this.acctHolderNameTextBoxId);
            b.bankNameTextBox = $("#" + this.bankNameTextBoxId)
        };
        b.hideTextBoxRef = function() {
            b.billingStateTextBoxInput.addClass("hidden");
            b.billingCountryTextBox.addClass("hidden")
        };
        b.hideContent = function() {
            this.content.addClass("hidden");
            this.clearCCForm();
            this.billingContent.addClass("hidden")
        };
        b.clearCCForm = function() {
            this.cvvTextBox.val("");
            this.accountNoTextBox.val("");
            this.acctHolderNameTextBox.val("");
            this.bankNameTextBox.val("");
            $("#" + this.issuingCountryTextBoxId).val("");
            $("#" + this.issuingCountrySelectId).val("")
        };
        b.showContent = function() {
            this.content.removeClass("hidden");
            this.billingContent.removeClass("hidden")
        };
        b.inlineDCCAjaxRequestHandler = function() {
            b.getInlineDCC()
        };
        b.addEvents = function() {
            this.amountTextBox.change(this.inlineDCCAjaxRequestHandler);
            this.accountNoTextBox.change(this.inlineDCCAjaxRequestHandler);
            this.billingCountryInput.change(this.updateState);
            this.billingStateSelectBoxInput.change(this.updateStateTextBoxValue)
        };
        b.init = function(c) {
            this.setSettingsByObject(c);
            this.setVars();
            this.addEvents();
            this.hideTextBoxRef();
            this.initDropDowns();
            this.billingStateSelectBoxInput.val(this.billingStateTextBoxInput.val())
        };
        b.initDropDowns = function() {
            if (b.issuingCountryTextBoxId !== "") {
                var c = {
                    TextBoxId: b.issuingCountryTextBoxId,
                    SelectBoxId: b.issuingCountrySelectId
                };
                countryDropDown = new SKYSALES.Class.CountryDropDown();
                countryDropDown.init(c)
            }
            if (b.billingCountryTextBoxId !== "") {
                var c = {
                    TextBoxId: b.billingCountryTextBoxId,
                    SelectBoxId: b.billingCountrySelectId
                };
                countryDropDown = new SKYSALES.Class.CountryDropDown();
                countryDropDown.init(c)
            }
            b.initStateDropDown()
        };
        b.updateState = function() {
            b.initStateDropDown();
            b.billingStateTextBoxInput.val("")
        };
        b.initStateDropDown = function() {
            var c = b.billingCountryInput.val();
            var e = b.stateInfo.StateList;
            var f = b.billingStateSelectBoxInput.get(0);
            if (!(f === undefined)) {
                f.options.length = 1;
                for (i = 0; i < e.length; i += 1) {
                    state = e[i];
                    if (c == state.CountryCode) {
                        f.options[f.options.length] = new window.Option(state.Name, state.ProvinceStateCode, false, false)
                    }
                }
            }
        };
        b.updateStateTextBoxValue = function() {
            var c = $(this).val();
            b.billingStateTextBoxInput.val(c)
        };
        b.getInlineDCC = function(c, f) {
            var e = {};
            if ("True" === this.inlineDCCOffer) {
                if (!f) {
                    f = this.accountNoTextBox.val()
                }
                if (!c) {
                    c = this.amountTextBox.val()
                }
                e = {
                    amount: c,
                    paymentFee: this.feeAmt,
                    currencyCode: this.currencyCode,
                    accountNumber: f
                };
                if (this.currencyCode && c && f && (0 < parseFloat(c)) && (12 <= f.length)) {
                    this.inlineDCCAjaxSucceeded.val("false");
                    $.get("DCCOfferAjax-Resource.aspx", e, this.inlineDCCResponseHandler)
                }
            }
        };
        b.setVarsAfterAjaxResponse = function(c) {
            var e = $("#" + this.dccOfferInfoId, c);
            b.foreignAmount = $("#" + this.foreignAmountId, e).text();
            b.foreignCurrency = $("#" + this.foreignCurrencyId, e).text();
            b.foreignCurrencySymbol = $("#" + this.foreignCurrencySymbolId, e).text();
            b.ownCurrencyAmount = $("#" + this.ownCurrencyAmountId, e).text();
            b.ownCurrency = $("#" + this.ownCurrencyId, e).text();
            b.ownCurrencySymbol = $("#" + this.ownCurrencySymbolId, e).text();
            b.acceptRadioBtnID = $("#" + this.acceptRadioBtnIdId, e).text();
            b.rejectRadioBtnID = $("#" + this.rejectRadioBtnIdId, e).text();
            b.acceptRadioBtn = $("#" + this.acceptRadioBtnID);
            b.doubleOptOut = $("#" + this.doubleOptOutId, e).text();
            b.radioButtonInlineDccStatusOfferAccept = $("#" + this.acceptRadioBtnID);
            b.radioButtonInlineDccStatusOfferReject = $("#" + this.rejectRadioBtnID)
        };
        b.foreignUpdateConversionLabel = function() {
            this.inlineDCCConversionLabel.text("( " + this.foreignAmount + " " + this.foreignCurrency + ")")
        };
        b.ownUpdateConversionLabel = function() {
            this.inlineDCCConversionLabel.text("")
        };
        b.noThanks = function() {
            $("#dccCont").show("slow")
        };
        b.noShowThanks = function() {
            $("#dccCont").hide("slow")
        };
        b.inlineDccStatusOfferAccept = function() {
            this.foreignUpdateConversionLabel();
            this.noShowThanks()
        };
        b.inlineDccStatusOfferReject = function() {
            this.ownUpdateConversionLabel();
            this.noThanks()
        };
        b.inlineDccStatusOfferAcceptHandler = function() {
            b.inlineDccStatusOfferAccept()
        };
        b.inlineDccStatusOfferRejectHandler = function() {
            b.inlineDccStatusOfferReject()
        };
        b.addEventsAfterAjaxResponse = function() {
            this.radioButtonInlineDccStatusOfferAccept.click(this.inlineDccStatusOfferAcceptHandler);
            this.radioButtonInlineDccStatusOfferReject.click(this.inlineDccStatusOfferRejectHandler)
        };
        b.updateAcceptRadioBtn = function() {
            var c = this.acceptRadioBtn.attr("checked");
            if (c) {
                this.foreignUpdateConversionLabel()
            }
        };
        b.updateInlineDCCOffer = function(f) {
            this.inlineDCCAjaxSucceeded.val("true");
            var c = null;
            if (f) {
                this.dcc.empty();
                var e = $(f);
                c = $("#" + this.dccId, e);
                if (c && c.length) {
                    this.dcc.prepend(c.children())
                }
                this.setVarsAfterAjaxResponse(e);
                this.addEventsAfterAjaxResponse();
                this.updateAcceptRadioBtn()
            }
        };
        b.inlineDCCResponseHandler = function(c) {
            b.updateInlineDCCOffer(c)
        };
        return b
    };
    SKYSALES.Class.PaymentInput.createObject = function(a) {
        SKYSALES.Util.createObject("paymentInput", "PaymentInput", a)
    }
}
if (!SKYSALES.Class.CountryDropDown) {
    SKYSALES.Class.CountryDropDown = function() {
        var b = SKYSALES.Class.SkySales();
        var a = SKYSALES.Util.extendObject(b);
        a.SelectBox = null;
        a.SelectBoxId = "";
        a.TextBox = null;
        a.TextBoxId = null;
        a.countryInfo = SKYSALES.Util.getResource().countryInfo;
        a.init = function(c) {
            this.setSettingsByObject(c);
            this.setVars();
            this.addEvents();
            a.populateDropDown();
            a.TextBox.val(this.TextBox.val())
        };
        a.setVars = function() {
            a.TextBox = this.getById(a.TextBoxId);
            a.Select = this.getById(a.SelectBoxId)
        };
        a.addEvents = function() {
            a.Select.change(a.updateTextBoxValue)
        };
        a.updateTextBoxValue = function() {
            var c = $(this).val();
            a.TextBox.val(c)
        };
        a.populateDropDown = function() {
            var c = {
                selectBox: this.Select,
                objectArray: this.countryInfo.CountryList,
                selectedItem: this.TextBox.val(),
                showCode: false
            };
            SKYSALES.Util.populateSelect(c)
        };
        return a
    }
}
if (!SKYSALES.Class.PriceDisplay) {
    SKYSALES.Class.PriceDisplay = function() {
        var a = new SKYSALES.Class.SkySales();
        var b = SKYSALES.Util.extendObject(a);
        b.toggleViewIdArray = null;
        b.init = function(f) {
            this.setSettingsByObject(f);
            var c = this.toggleViewIdArray || [];
            var e = 0;
            var l = null;
            for (e = 0; e < c.length; e += 1) {
                l = new SKYSALES.Class.ToggleView();
                l.init(c[e]);
                b.toggleViewIdArray[e] = l
            }
        };
        return b
    };
    SKYSALES.Class.PriceDisplay.createObject = function(a) {
        SKYSALES.Util.createObject("priceDisplay", "PriceDisplay", a)
    }
}
if (!SKYSALES.Class.FlightDisplay) {
    SKYSALES.Class.FlightDisplay = function() {
        var b = new SKYSALES.Class.SkySales();
        var a = SKYSALES.Util.extendObject(b);
        a.toggleViewIdArray = null;
        a.init = function(f) {
            this.setSettingsByObject(f);
            var c = this.toggleViewIdArray || [];
            var e = 0;
            var l = null;
            for (e = 0; e < c.length; e += 1) {
                l = new SKYSALES.Class.ToggleView();
                l.init(c[e]);
                a.toggleViewIdArray[e] = l
            }
        };
        return a
    };
    SKYSALES.Class.FlightDisplay.createObject = function(a) {
        SKYSALES.Util.createObject("flightDisplay", "FlightDisplay", a)
    }
}
if (!SKYSALES.Class.RandomImage) {
    SKYSALES.Class.RandomImage = function() {
        var a = new SKYSALES.Class.SkySales();
        var b = SKYSALES.Util.extendObject(a);
        b.imageUriArray = [];
        b.init = function(c) {
            this.setSettingsByObject(c);
            this.setVars();
            this.setAsBackground()
        };
        b.getRandomNumber = function() {
            var c = this.imageUriArray.length;
            var e = Math.floor(Math.random() * c);
            return e
        };
        b.setAsBackground = function() {
            var e = this.getRandomNumber();
            var c = "url(" + this.imageUriArray[e] + ")";
            this.container.css("background-image", c)
        };
        return b
    };
    SKYSALES.Class.RandomImage.createObject = function(a) {
        SKYSALES.Util.createObject("randomImage", "RandomImage", a)
    }
}
SKYSALES.Class.DropDown = function(b) {
    b = b || {};
    var a = this;
    a.container = {};
    a.name = "";
    a.options = [];
    a.dropDownContainer = null;
    a.dropDownContainerInput = null;
    a.document = null;
    a.optionList = null;
    a.optionActiveClass = "optionActive";
    a.timeOutObj = null;
    a.timeOut = 225;
    a.minCharLength = 2;
    a.optionMax = 100;
    a.html = '<div></div><div class="dropDownContainer"></div>';
    a.autoComplete = true;
    a.setSettingsByObject = function(c) {
        var e = null;
        for (e in c) {
            if (a.hasOwnProperty(e)) {
                a[e] = c[e]
            }
        }
    };
    a.getOptionHtml = function(c) {
        c = c || "";
        var l = {};
        var p = "";
        var f = "";
        var n = 0;
        var m = a.options;
        var e = new RegExp("^" + c, "i");
        if (c.length < a.minCharLength) {
            f = ""
        } else {
            for (p in m) {
                if (m.hasOwnProperty(p)) {
                    l = m[p];
                    l.name = l.name || "";
                    l.code = l.code || "";
                    if (l.name.match(e) || l.code.match(e)) {
                        f += "<div><span>" + l.code + "</span>" + l.name + " (" + l.code + ")</div>";
                        n += 1
                    }
                    if (n >= a.optionMax) {
                        break
                    }
                }
            }
        }
        return f
    };
    a.close = function() {
        if (a.timeOutObj) {
            window.clearTimeout(a.timeOutObj)
        }
        a.document.unbind("click", a.close);
        if (a.optionList) {
            a.optionList.unbind("hover");
            a.optionList.unbind("click")
        }
        a.optionList = null;
        a.dropDownContainer.html("")
    };
    a.getActiveOptionIndex = function() {
        var e = -1;
        var c = $("." + a.optionActiveClass, a.dropDownContainer);
        if (a.optionList && (c.length > 0)) {
            e = a.optionList.index(c[0])
        }
        return e
    };
    a.arrowDown = function() {
        var c = a.getActiveOptionIndex();
        if (a.optionList) {
            if ((c === -1) && (a.optionList.length > 0)) {
                a.optionActive.call(a.optionList[0])
            } else {
                if (a.optionList.length > c + 1) {
                    a.optionInActive.call(a.optionList[c]);
                    a.optionActive.call(a.optionList[c + 1])
                } else {
                    a.arrowDownOpen()
                }
            }
        } else {
            a.arrowDownOpen()
        }
    };
    a.arrowDownOpen = function() {
        var c = a.minCharLength;
        a.minCharLength = 0;
        a.open();
        a.minCharLength = c
    };
    a.arrowUp = function() {
        var c = a.getActiveOptionIndex();
        if (a.optionList) {
            if ((c === -1) && (a.optionList.length > 0)) {
                a.optionActive.call(a.optionList[0])
            } else {
                if ((c > 0) && (a.optionList.length > 0)) {
                    a.optionInActive.call(a.optionList[c]);
                    a.optionActive.call(a.optionList[c - 1])
                }
            }
        }
    };
    a.selectButton = function() {
        var e = a.getActiveOptionIndex();
        var c = a.optionMax;
        if (e > -1) {
            a.selectOption.call(a.optionList[e])
        } else {
            if (a.autoComplete === true) {
                a.optionMax = 1;
                a.open();
                if (a.optionList && (a.optionList.length > 0)) {
                    a.selectOption.call(a.optionList[0])
                }
                a.optionMax = c
            }
        }
    };
    a.keyEvent = function(c) {
        var f = true;
        var e = c.which;
        if (e === 40) {
            a.arrowDown();
            a.autoComplete = true;
            f = false
        } else {
            if (e === 38) {
                a.arrowUp();
                a.autoComplete = true;
                f = false
            } else {
                if (e === 9) {
                    a.selectButton();
                    a.inputBlur()
                } else {
                    if (e === 13) {
                        a.selectButton();
                        a.autoComplete = false;
                        f = false
                    } else {
                        a.autoComplete = true
                    }
                }
            }
        }
        return f
    };
    a.inputKeyEvent = function(c) {
        var f = true;
        var e = c.which;
        if ((e !== 40) && (e !== 38) && (e !== 9) && (e !== 13)) {
            if (a.timeOutObj) {
                window.clearTimeout(a.timeOutObj)
            }
            a.timeOutObj = window.setTimeout(a.open, a.timeOut);
            f = false
        }
        return f
    };
    a.catchEvent = function() {
        return false
    };
    a.open = function() {
        var n = "";
        var l = null;
        var e = a.dropDownContainerInput.val();
        var f = a.getOptionHtml(e);
        var c = 0;
        var m = 0;
        a.dropDownContainer.html(f);
        a.addOptionEvents();
        a.dropDownContainer.click(a.catchEvent);
        a.document.click(a.close);
        a.dropDownContainer.show();
        if (a.optionList && (a.optionList.length > 0) && a.optionActive) {
            a.optionActive.call(a.optionList[0])
        }
        m = a.dropDownContainer.width();
        if ($.browser.msie) {
            c = a.dropDownContainer.height();
            n = '<iframe src="#"></iframe>';
            a.dropDownContainer.prepend(n);
            l = $("iframe", a.dropDownContainer);
            l.width(m);
            l.height(c)
        }
    };
    a.optionActive = function() {
        var c = $(this);
        a.optionList.removeClass(a.optionActiveClass);
        c.addClass(a.optionActiveClass)
    };
    a.optionInActive = function() {
        var c = $(this);
        c.removeClass(a.optionActiveClass)
    };
    a.selectOption = function() {
        var c = $("span", this).text();
        a.dropDownContainerInput.val(c);
        a.close();
        a.dropDownContainerInput.change()
    };
    a.addOptionEvents = function() {
        a.optionList = $("div", a.dropDownContainer);
        a.optionList.hover(a.optionActive, a.optionInActive);
        a.optionList.click(a.selectOption)
    };
    a.inputBlur = function() {
        a.close()
    };
    a.addEvents = function(c) {
        a.dropDownContainerInput = c.input;
        a.dropDownContainer = $("div.dropDownContainer", a.container);
        a.document = $(document);
        a.dropDownContainerInput.keyup(a.inputKeyEvent);
        a.dropDownContainerInput.keydown(a.keyEvent)
    };
    a.init = function(e) {
        a.setSettingsByObject(e);
        var c = a.html;
        e.input.attr("autocomplete", "off");
        e.input.wrap('<span class="dropDownOuterContainer"></span>');
        e.input.after(c);
        a.container = e.input.parent("span.dropDownOuterContainer");
        a.addEvents(e);
        SKYSALES.Class.DropDown.dropDownArray[SKYSALES.Class.DropDown.dropDownArray.length] = a
    };
    a.init(b);
    return a
};
SKYSALES.Class.DropDown.dropDownArray = [];
SKYSALES.Class.DropDown.getDropDown = function(e) {
    var l = null;
    var c = 0;
    var f = null;
    var m = SKYSALES.Class.DropDown.dropDownArray;
    var b = null;
    var a = e.input.get(0);
    for (c = 0; c < m.length; c += 1) {
        f = m[c];
        b = f.dropDownContainerInput.get(0);
        if ((b) && (a) && (b === a)) {
            l = m[c];
            if (e.options) {
                l.options = e.options
            }
        }
    }
    if (!l) {
        l = new SKYSALES.Class.DropDown(e)
    }
    return l
};
if (!SKYSALES.Class.DatePickerManager) {
    SKYSALES.Class.DatePickerManager = function() {
        var s = this;
        s.isAOS = false;
        s.yearMonth = null;
        s.day = null;
        s.linkedDate = null;
        s.marketCount = null;
        var p = [];
        var l = "-";
        var n = "yy-mm";
        var r = "first";
        var t = "mm/dd/yy";
        var u = new RegExp("\\d{4}-\\d{2}");
        var b = function(B) {
            var C = new Date(B.getFullYear(), B.getMonth(), 32);
            var A = C.getDate();
            return 32 - A
        };
        var c = function(A) {
            return A.match(/\d{2}/)
        };
        var a = function(A) {
            A = A || "";
            return A.match(u)
        };
        var m = function(D, A) {
            var G = new Date();
            var E = D.split(l);
            var F = 0;
            var H = 1;
            if (true === s.isAOS) {
                F = 1;
                H = 0
            }
            var C = E[F];
            var B = E[H] - 1;
            G = new Date(C, B, A);
            return G
        };
        var z = function(E, A) {
            var G = new Date();
            var B = c(A);
            var H = a(E);
            if (B && H) {
                var D = m(E, A);
                var C = b(D);
                var F = A;
                if (A > C) {
                    F = C
                }
                G = new Date(D.getFullYear(), D.getMonth(), F)
            } else {
                G = new Date()
            }
            return G
        };
        var y = function() {
            var A = z(s.yearMonth.val(), s.day.val());
            var B = $.datepicker.formatDate(t, A);
            s.linkedDate.val(B);
            return {}
        };
        var x = function(F, N) {
            var E = new Date();
            var K = E.getDate();
            var J = E.getFullYear() + "-" + E.getMonth();
            var M = F.getFullYear() + "-" + F.getMonth();
            var D = J === M;
            var O = (2 < K);
            var H = O && D;
            var G = F.getDate();
            var B = b(F);
            var I = 31 - B;
            var C = SKYSALES.Util.cloneArray(p);
            var A = 31;
            if (I > 0) {
                A = 31 - I;
                C.splice(A, I)
            }
            if (H) {
                C.splice(0, K - 2)
            }
            var L = {
                selectedItem: G,
                objectArray: C,
                selectBox: N,
                clearOptions: true
            };
            SKYSALES.Util.populateSelect(L)
        };
        var w = function() {
            var I = s.day.val();
            var J = s.yearMonth.val();
            var E = m(s.yearMonth.val(), 1);
            var B = b(E);
            if (I > B) {
                I = B
            }
            E = new Date(E.getFullYear(), E.getMonth(), I);
            x(E, s.day);
            s.linkedDate.val($.datepicker.formatDate(t, E));
            if (this.tagName == "SELECT" && s.marketCount > 1) {
                if (this.id.indexOf("DropDownListMarketMonth1") != -1) {
                    var A = this.id.replace("DropDownListMarketMonth1", "");
                    var G = $("#" + A + "DropDownListMarketDay2");
                    var H = document.getElementById(A + "DropDownListMarketMonth2");
                    var F = document.getElementById("date_picker_id_2")
                } else {
                    if (this.id.indexOf("DropDownListMarketMonth2") != -1) {
                        var A = this.id.replace("DropDownListMarketMonth2", "");
                        var G = $("#" + A + "DropDownListMarketDay1");
                        var H = document.getElementById(A + "DropDownListMarketMonth1");
                        var F = document.getElementById("date_picker_id_1")
                    }
                }
                var D = m(H.value, 1);
                D = new Date(D.getFullYear(), D.getMonth(), G.val());
                x(D, G);
                var I = G.val();
                var C = m(H.value, 1);
                var B = b(C);
                if (I > B) {
                    I = B
                }
                C = new Date(C.getFullYear(), C.getMonth(), I);
                F.value = $.datepicker.formatDate(t, C)
            }
        };
        var f = function() {
            var J = s.yearMonth.val();
            var I = s.day.val();
            var D = z(J, I);
            var F = $.datepicker.formatDate(t, D);
            s.linkedDate.val(F);
            if (this.id != null && s.marketCount > 1) {
                if (this.id.indexOf("DropDownListMarketDay1") != -1 && this.tagName == "SELECT") {
                    var A = this.id.replace("DropDownListMarketDay1", "");
                    var G = document.getElementById(A + "DropDownListMarketDay2");
                    var H = document.getElementById(A + "DropDownListMarketMonth2");
                    var E = document.getElementById("date_picker_id_2");
                    var C = document.getElementById(A + "CheckBoxChangeMarket_2");
                    if (C == null || C.checked == true) {}
                } else {
                    if (this.id.indexOf("DropDownListMarketDay2") != -1 && this.tagName == "SELECT") {
                        var A = this.id.replace("DropDownListMarketDay2", "");
                        var G = document.getElementById(A + "DropDownListMarketDay1");
                        var H = document.getElementById(A + "DropDownListMarketMonth1");
                        var E = document.getElementById("date_picker_id_1");
                        var C = document.getElementById(A + "CheckBoxChangeMarket_1");
                        if (C == null || C.checked == true) {}
                    }
                }
                var I = G.value;
                var D = m(H.value, 1);
                var B = b(D);
                D = new Date(D.getFullYear(), D.getMonth(), I);
                E.value = $.datepicker.formatDate(t, D)
            }
        };
        var q = function() {
            var A = [];
            var C = 1;
            var B = {};
            for (C = 1; C <= 31; C += 1) {
                B = {};
                B.name = C;
                if (C <= 9) {
                    B.code = "0" + C
                } else {
                    B.code = C
                }
                A[C - 1] = B
            }
            return A
        };
        var e = function(G) {
            var C = G.match(/\d{2}\/\d{2}\/\d{4}/);
            var B = new Date();
            var H = "";
            if (C) {
                B = new Date(G);
                H = $.datepicker.formatDate(n, B);
                s.yearMonth.val(H);
                x(B, s.day);
                var A = s.day[0].id;
                var E = s.yearMonth[0].id;
                var F = $("#" + A);
                var D = $("#" + E);
                D.change();
                F.change()
            }
        };
        s.setSettingsByObject = function(B) {
            var A = "";
            for (A in B) {
                if (s.hasOwnProperty(A)) {
                    s[A] = B[A]
                }
            }
        };
        s.setVars = function() {
            if (true === s.isAOS) {
                l = "/";
                n = "m/yy";
                u = new RegExp("\\d{1,2}\\/\\d{4}");
                r = "eq(1)"
            }
        };
        var v = function() {
            if (!s.isAOS) {
                f()
            }
        };
        s.addEvents = function() {
            s.yearMonth.change(w);
            s.day.change(f);
            var G = new Date();
            var B = new Date();
            var K = new Date();
            B.setFullYear(B.getFullYear() + 1);
            var A = $("option:" + r, s.yearMonth).val();
            var F = $("option:last", s.yearMonth).val();
            p = q();
            var J = s.linkedDate;
            if (a(A)) {
                G.setDate(G.getDate() - 1);
                if (s.isAOS) {
                    K = new Date(s.linkedDate.val())
                } else {
                    K = m(s.yearMonth.val(), s.day.val())
                }
                x(K, s.day)
            }
            if (a(F)) {
                B = m(F, 1);
                var C = b(B);
                B = new Date(B.getFullYear(), B.getMonth(), C)
            }
            var E = SKYSALES.Util.getResource();
            var I = E.dateCultureInfo;
            var D = SKYSALES.datepicker;
            v();
            var H = {
                beforeShow: y,
                onSelect: e,
                minDate: G,
                maxDate: B,
                showOn: "both",
                buttonImageOnly: true,
                buttonImage: "./images/AKBase/be_calendar.gif",
                buttonText: "Calendar",
                numberOfMonths: 1,
                mandatory: true,
                monthNames: I.monthNames,
                monthNamesShort: I.monthNamesShort,
                dayNames: I.dayNames,
                dayNamesShort: I.dayNamesShort,
                dayNamesMin: I.dayNamesMin,
                closeText: D.closeText,
                prevText: D.prevText,
                nextText: D.nextText,
                currentText: D.currentText
            };
            J.datepicker(H)
        };
        s.init = function(A) {
            this.setSettingsByObject(A);
            this.setVars();
            this.addEvents()
        }
    }
}
SKYSALES.initializeSkySalesForm = function() {
    document.SkySales = document.forms.SkySales
};
SKYSALES.getSkySalesForm = function() {
    var a = $("SkySales").get(0);
    return a
};
SKYSALES.Common = function() {
    var b = this;
    var a = null;
    b.allInputObjects = null;
    b.initializeCommon = function() {
        var e = new SKYSALES.Hint();
        var c = new SKYSALES.InputLabel();
        b.addKeyDownEvents();
        b.addSetAndEraseEvents();
        b.setValues();
        e.addHintEvents();
        c.formatInputLabel();
        b.stripeTables()
    };
    b.setValues = function() {
        var c = function(e) {
            if ((this.jsvalue !== null) && (this.jsvalue !== undefined)) {
                this.value = this.jsvalue
            }
        };
        b.getAllInputObjects().each(c)
    };
    b.stopSubmit = function() {
        $("form").unbind("submit", b.stopSubmit);
        return false
    };
    b.addKeyDownEvents = function() {
        var c = function(f) {
            if (f.keyCode === 13) {
                $("form").submit(b.stopSubmit);
                return false
            }
            return true
        };
        $(":input").keydown(c)
    };
    b.getAllInputObjects = function() {
        if (b.allInputObjects === null) {
            b.allInputObjects = $(":input")
        }
        return b.allInputObjects
    };
    b.addSetAndEraseEvents = function() {
        var f = function() {
            b.eraseElement(this, this.requiredempty)
        };
        var c = function() {
            b.setElement(this, this.requiredempty);
            $(this).change()
        };
        var e = function(m) {
            var l = $(this);
            if ((this.requiredempty !== null) && (this.requiredempty !== undefined)) {
                if (l.is(":text") && (l.is(":hidden") === false)) {
                    l.focus(f);
                    l.blur(c)
                }
            }
        };
        b.getAllInputObjects().each(e)
    };
    b.eraseElement = function(e, c) {
        if (e.value === c) {
            e.value = ""
        }
    };
    b.setElement = function(e, c) {
        if (e.value === "") {
            e.value = c
        }
    };
    b.getCountryInfo = function() {
        if (a === null) {
            a = window.countryInfo
        }
        return a
    };
    b.setCountryInfo = function(c) {
        a = c;
        return b
    };
    b.isEmpty = function(e, c) {
        var l = null;
        var f = false;
        if ((e) && (c === undefined)) {
            if (e.requiredempty) {
                c = e.requiredempty
            } else {
                c = ""
            }
        }
        l = SKYSALES.Common.getValue(e);
        if ((l === null) || (l === undefined) || (l.length === 0) || (l === c)) {
            f = true
        }
        return f
    };
    b.stripeTables = function() {
        $(".stripeMe tr:even").addClass("alt");
        return b
    }
};
SKYSALES.Common.addEvent = function(c, a, b) {
    $(c).bind(a, b)
};
SKYSALES.Common.getValue = function(a) {
    var b = null;
    if (a) {
        b = $(a).val();
        return b
    }
    return null
};
SKYSALES.InputLabel = function() {
    var a = this;
    a.getInputLabelRequiredFlag = function() {
        return "*"
    };
    a.getInputLabelSuffix = function() {
        return ":"
    };
    a.formatInputLabel = function() {
        var b = a.getInputLabelRequiredFlag();
        var e = a.getInputLabelSuffix();
        var c = function(l) {
            var f = $("label[@for=" + this.id + "]").eq(0);
            var n = $(f).text();
            var m = "";
            var p = null;
            if (n !== "") {
                m = $(this).attr("type");
                if ((m !== "checkbox") && (m !== "radio")) {
                    n = n
                }
                p = this.required;
                if (p === undefined) {
                    p = null
                }
                if (p === null) {
                    p = this.getAttribute("required")
                }
                if (p !== null) {
                    p = p.toString().toLowerCase();
                    if (p === "true") {
                        n = b + n
                    }
                }
                $(f).text(n)
            }
        };
        SKYSALES.common.getAllInputObjects().each(c)
    }
};
SKYSALES.Dhtml = function() {
    var a = this;
    a.getX = function(b) {
        var c = 0;
        if (b.x) {
            c += b.x
        } else {
            if (b.offsetParent) {
                while (b.offsetParent) {
                    c += b.offsetLeft;
                    b = b.offsetParent
                }
            }
        }
        return c
    };
    a.getY = function(b) {
        var c = 0;
        if (b.y) {
            c += b.y
        } else {
            if (b) {
                while (b) {
                    c += b.offsetTop;
                    b = b.offsetParent
                }
            }
        }
        return c
    };
    return a
};
SKYSALES.Hint = function() {
    var a = this;
    a.addHintEvents = function() {
        var b = function(c) {
            if ((this.hint !== null) && (this.hint !== undefined)) {
                if (this.tagName && (this.tagName.toString().toLowerCase() === "input")) {
                    a.addHintFocusEvents(this)
                } else {
                    a.addHintHoverEvents(this)
                }
            }
        };
        SKYSALES.common.getAllInputObjects().each(b)
    };
    a.addHintFocusEvents = function(f, c) {
        var e = function() {
            a.showHint(f, c)
        };
        var b = function() {
            a.hideHint(f, c)
        };
        if ($(f).is(":hidden") === false) {
            $(f).focus(e);
            $(f).blur(b)
        }
    };
    a.addHintHoverEvents = function(e, b) {
        var c = function() {
            a.showHint(e, b)
        };
        var f = function() {
            a.hideHint(e, b)
        };
        $(e).hover(c, f)
    };
    a.getHintDivId = function() {
        return "cssHint"
    };
    a.showHint = function(l, t, n, f, b) {
        var w = a.getHintDivId();
        var m = $("#" + w);
        var s = 0;
        var q = 0;
        var v = 0;
        var p = 0;
        if (n === undefined) {
            n = l.hintxoffset
        }
        if (f === undefined) {
            f = l.hintyoffset
        }
        if (b === undefined) {
            b = l.hintReferenceid
        }
        var u = $("#" + b).get(0);
        var r = new SKYSALES.Dhtml();
        if (!u) {
            s = r.getX(l);
            q = r.getY(l);
            if (n === undefined) {
                s += l.offsetWidth + 5
            }
        } else {
            s = r.getX(u);
            q = r.getY(u);
            if (n === undefined) {
                s += u.offsetWidth + 5
            }
        }
        if (t === undefined) {
            if (l.hint !== undefined) {
                t = l.hint
            }
        }
        m.html(t);
        m.show();
        n = (n !== undefined) ? n : v;
        f = (f !== undefined) ? f : p;
        var e = parseInt(n, 10) + parseInt(s, 10);
        var c = parseInt(f, 10) + parseInt(q, 10);
        m.css("left", e + "px");
        m.css("top", c + "px")
    };
    a.hideHint = function(c) {
        var b = a.getHintDivId();
        $("#" + b).hide()
    }
};
SKYSALES.ValidationErrorReadAlong = function() {
    var a = this;
    a.objId = "";
    a.obj = null;
    a.errorMessage = "";
    a.isError = false;
    a.hasBeenFixed = false;
    a.hasValidationEvents = false;
    a.getValidationErrorHtml = function() {
        var b = '<span id="validationErrorContainerReadAlongIFrame" class="hidden" ></span> <div id="validationErrorContainerReadAlong" > <p class="close"> <a id="validationErrorContainerReadAlongCloseButton" /> </p> <div id="validationErrorContainerReadAlongContent" > <h3 class="error">ERROR</h3> <div id="validationErrorContainerReadAlongList" > </div> </div> </div>';
        return b
    };
    a.getValidationErrorCloseId = function() {
        return "validationErrorContainerReadAlongCloseButton"
    };
    a.getValidationErrorListId = function() {
        return "validationErrorContainerReadAlongList"
    };
    a.getValidationErrorIFrameId = function() {
        return "validationErrorContainerReadAlongIFrame"
    };
    a.getValidationErrorDivId = function() {
        return "validationErrorContainerReadAlong"
    };
    a.getFixedClass = function() {
        return "fixedValidationError"
    };
    a.addCloseEvent = function() {
        var c = a.getValidationErrorCloseId();
        var b = function() {
            a.hide()
        };
        $("#" + c).click(b)
    };
    a.addValidationErrorDiv = function() {
        $("#mainContent").append(a.getValidationErrorHtml())
    };
    a.hide = function() {
        var c = a.getValidationErrorIFrameId();
        var b = a.getValidationErrorDivId();
        $("#" + c).hide();
        $("#" + b).hide()
    };
    a.addFocusEvent = function(b) {
        var e = {
            obj: this
        };
        var c = function(f) {
            var n = f.data.obj;
            var l = null;
            var u = null;
            var m = 0;
            var s = 0;
            var r = 0;
            var p = 0;
            var q = null;
            var t = null;
            if (n.isError === true) {
                l = new SKYSALES.Hint();
                l.hideHint();
                u = $("#" + a.getValidationErrorDivId());
                m = parseInt(u.width(), 10) + 5;
                s = parseInt(u.height(), 10) + 5;
                q = new SKYSALES.Dhtml();
                r = q.getX(n.obj);
                p = q.getY(n.obj);
                r = r + this.offsetWidth + 5;
                p = p - 72;
                if ($.browser.msie) {
                    t = $("#" + a.getValidationErrorIFrameId());
                    t.css("position", "absolute");
                    t.show();
                    if (m > 25) {
                        t.width(m - 25)
                    }
                    t.height(s - 5);
                    t.css("left", r + 16);
                    t.css("top", p)
                }
                u.css("left", r);
                u.css("top", p);
                u.css("position", "absolute");
                u.show("slow");
                return false
            }
        };
        if ($(this.obj).is(":hidden") === false) {
            $(this.obj).bind("focus", e, c)
        }
    };
    a.addBlurEvent = function(b) {
        var e = {
            obj: this
        };
        var c = function(f) {
            var q = f.data.obj;
            var u = new SKYSALES.Validate(null, "", "", null);
            u.validateSingleElement(this);
            var s = u.errors;
            var n = false;
            var p = true;
            if (u.validationErrorArray.length > 0) {
                if (u.validationErrorArray[0].isError === false) {
                    n = true
                }
            }
            var r = q.getValidationErrorListId();
            var l = $("#" + r).find("li").eq(b);
            var m = q.getFixedClass();
            var t = function() {
                if ((p === true) && ($(this).attr("class").indexOf("hidden") === -1) && ($(this).attr("class").indexOf(m) === -1)) {
                    p = false
                }
            };
            if (n === true) {
                q.hasBeenFixed = true;
                l.addClass(m);
                p = true;
                $("#" + r).find("li").each(t);
                if (p === true) {
                    a.hide()
                }
            } else {
                q.hasBeenFixed = false;
                l.removeClass(m);
                l.removeClass("hidden");
                q.isError = true;
                q.errorMessage = s;
                l.text(s)
            }
            return false
        };
        $(this.obj).bind("blur", e, c)
    }
};
SKYSALES.errorsHeader = "Please correct the following.\n\n";
SKYSALES.Validate = function(e, c, a, l) {
    var b = this;
    if (a === undefined) {
        a = SKYSALES.errorsHeader
    }
    b.form = e;
    b.namespace = c;
    b.errors = "";
    b.validationErrorArray = [];
    b.setfocus = null;
    b.clickedObj = null;
    b.errorDisplayMethod = "read_along";
    b.errorsHeader = a;
    b.namedErrors = [];
    b.dateRangeArray = [];
    if (l) {
        b.regexElementIdFilter = l
    }
    b.requiredAttribute = "required";
    b.requiredEmptyAttribute = "requiredempty";
    b.validationTypeAttribute = "validationtype";
    b.regexAttribute = "regex";
    b.minLengthAttribute = "minlength";
    b.numericMinLengthAttribute = "numericminlength";
    b.maxLengthAttribute = "maxlength";
    b.numericMaxLengthAttribute = "numericmaxlength";
    b.minValueAttribute = "minvalue";
    b.maxValueAttribute = "maxvalue";
    b.equalsAttribute = "equals";
    b.dateRangeAttribute = "daterange";
    b.dateRange1HiddenIdAttribute = "date1hiddenid";
    b.dateRange2HiddenIdAttribute = "date2hiddenid";
    b.defaultErrorAttribute = "error";
    b.requiredErrorAttribute = "requirederror";
    b.validationTypeErrorAttribute = "validationtypeerror";
    b.regexErrorAttribute = "regexerror";
    b.minLengthErrorAttribute = "minlengtherror";
    b.maxLengthErrorAttribute = "maxlengtherror";
    b.minValueErrorAttribute = "minvalueerror";
    b.maxValueErrorAttribute = "maxvalueerror";
    b.equalsErrorAttribute = "equalserror";
    b.dateRangeErrorAttribute = "daterangeerror";
    b.defaultError = "{label} is invalid.";
    b.defaultRequiredError = "{label} is required.";
    b.defaultValidationTypeError = "{label} is invalid.";
    b.defaultRegexError = "{label} is invalid.";
    b.defaultMinLengthError = "{label} is too short in length.";
    b.defaultMaxLengthError = "{label} is too long in length.";
    b.defaultMinValueError = "{label} must be greater than {minValue}.";
    b.defaultMaxValueError = "{label} must be less than {maxValue}.";
    b.defaultEqualsError = "{label} is not equal to {equals}";
    b.defaultNotEqualsError = "{label} cannot equal {equals}";
    b.defaultValidationErrorClass = "validationError";
    b.defaultValidationErrorLabelClass = "validationErrorLabel";
    b.sameDate = false;
    b.run = function() {
        var n = $(":input", SKYSALES.getSkySalesForm()).get();
        var p = null;
        for (var m = 0; m < n.length; m += 1) {
            p = n[m];
            if (!this.isExemptFromValidation(p)) {
                b.validateSingleElement(p)
            }
        }
        return b.outputErrors()
    };
    b.runBySelector = function(q) {
        var p = $(q).find(":input").get();
        var n = null;
        var m = 0;
        for (m = 0; m < p.length; m += 1) {
            n = p[m];
            b.validateSingleElement(n)
        }
        return false
    };
    b.validateSingleElement = function(p) {
        $(p).removeClass(b.defaultValidationErrorClass);
        $("label[@for=" + p.id + "]").eq(0).removeClass(this.defaultValidationErrorLabelClass);
        var n = new SKYSALES.ValidationErrorReadAlong();
        n.objId = p.id;
        n.obj = p;
        this.validationErrorArray[b.validationErrorArray.length] = n;
        this.validateRequired(p);
        var m = b.getValue(p);
        if ((b.errors.length < 1) && (m !== null) && (m !== "")) {
            b.validateType(p);
            b.validateRegex(p);
            b.validateMinLength(p);
            b.validateMaxLength(p);
            b.validateMinValue(p);
            b.validateMaxValue(p);
            b.validateEquals(p);
            b.validateDateRange(p)
        }
    };
    b.outputErrors = function() {
        var m = this.errorDisplayMethod.toString().toLowerCase();
        var r = "";
        var n = [];
        var q = 0;
        var s = true;
        if (this.errors) {
            n = b.errors.split("\n");
            r += '<ul class="validationErrorList" >';
            for (q = 0; q < n.length; q += 1) {
                if (n[q] !== "") {
                    r += '<li class="validationErrorListItem" >' + n[q] + "</li>"
                }
            }
            r += "</ul>";
            if (m.indexOf("read_along") > -1) {
                b.outputErrorsReadAlong(r);
                s = false
            }
            if (m.indexOf("alert") > -1) {
                alert(b.errorsHeader + b.errors)
            }
            if (s === true) {
                alert(b.errorsHeader + b.errors)
            }
            if (b.setfocus) {
                if ($(b.setfocus).is(":hidden") === false) {
                    try {
                        b.setfocus.blur();
                        b.setfocus.focus()
                    } catch (p) {}
                }
            }
            return false
        } else {
            return !b.sameDate
        }
    };
    b.outputErrorsReadAlong = function(p) {
        var n = 0;
        var m = "";
        var q = null;
        var s = this;
        var r = function(t) {
            this.hasValidationEvents = true;
            this.addFocusEvent(t);
            this.addBlurEvent(t)
        };
        s.validationErrorReadAlong = new SKYSALES.ValidationErrorReadAlong();
        s.readAlongDivId = $("#" + this.validationErrorReadAlong.getValidationErrorDivId()).attr("id");
        if (s.readAlongDivId === undefined) {
            s.validationErrorReadAlong.addValidationErrorDiv();
            s.validationErrorReadAlong.addCloseEvent()
        }
        m += '<ul class="validationErrorList" >';
        for (n = 0; n < s.validationErrorArray.length; n += 1) {
            q = this.validationErrorArray[n];
            if (q.isError === true) {
                m += '<li class="validationErrorListItem" >' + q.errorMessage + "</li>"
            } else {
                m += '<li class="validationErrorListItem hidden" >' + q.errorMessage + "</li>"
            }
        }
        $("#" + s.validationErrorReadAlong.getValidationErrorListId()).html(m);
        $(s.validationErrorArray).each(r)
    };
    b.checkFocus = function(m) {
        if (!b.setfocus) {
            b.setfocus = m
        }
    };
    b.setError = function(v, q, p) {
        var n = "";
        var w = "";
        var u = "";
        var t = 0;
        var m = null;
        if (v.type === "radio") {
            n = v.getAttribute("name");
            if (n.length > 0) {
                if (b.namedErrors[n] !== undefined) {
                    return
                }
                b.namedErrors[n] = n
            }
        }
        w = v[q];
        if (!w) {
            if (v.getAttribute(q)) {
                w = v.getAttribute(q)
            } else {
                if (v[b.defaultErrorAttribute]) {
                    w = v[b.defaultErrorAttribute]
                } else {
                    if (p) {
                        w = p
                    } else {
                        w = b.defaultError
                    }
                }
            }
        }
        var s = w.match(/\{\s*(\w+)\s*\}/g);
        if (s) {
            for (t = 0; t < s.length; t += 1) {
                u = s[t].replace(/\{\s*(\w+)\s*\}/, "$1");
                w = w.replace(/\{\s*\w+\s*\}/, b.cleanAttributeForErrorDisplay(v, u))
            }
        }
        $(v).addClass(this.defaultValidationErrorClass);
        $("label[@for=" + v.id + "]").eq(0).addClass(b.defaultValidationErrorLabelClass);
        this.errors += w + "\n";
        var r = v.id;
        for (t = 0; t < b.validationErrorArray.length; t += 1) {
            m = b.validationErrorArray[t];
            if (m.objId === r) {
                m.errorMessage = w;
                m.isError = true;
                break
            }
        }
        this.checkFocus(v)
    };
    b.cleanAttributeForErrorDisplay = function(r, m) {
        var n = null;
        var q = "";
        if (m === undefined) {
            m = ""
        }
        m = m.toLowerCase();
        var p = "";
        if (m === "label") {
            p = $("label[@for=" + r.id + "]").eq(0).text();
            n = new SKYSALES.InputLabel();
            q = n.getInputLabelRequiredFlag();
            p = p.replace(q, "")
        }
        if (!p) {
            p = r.id
        }
        if (!p) {
            return m
        }
        if (m.match(/^(minvalue|maxvalue)$/i)) {
            return p.replace(/[^\d.,]/g, "")
        }
        return p
    };
    b.validateRequired = function(r) {
        var u = b.requiredAttribute;
        var p = b.requiredEmptyAttribute;
        var t = r[u] ? r[u] : r.getAttribute(u);
        var s = r[p];
        var q = null;
        b.radioGroupHash = {};
        var n = "";
        var m = false;
        if (t !== undefined && t != null) {
            t = t.toString().toLowerCase();
            if (s) {
                s = s.toString().toLowerCase()
            }
            if (t === "true") {
                q = b.getValue(r);
                if ((r.type === "checkbox") && (r.checked === false)) {
                    q = ""
                } else {
                    if (r.type === "radio") {
                        n = r.getAttribute("name");
                        if (b.radioGroupHash[n] === undefined) {
                            b.radioGroupHash[n] = $("input[@name='" + n + "']")
                        }
                        m = b.radioGroupHash[n].is(":checked");
                        if (!m) {
                            q = ""
                        }
                    }
                }
                if ((q === undefined) || (q === "none") || (q === null) || (q === "") || (q.toLowerCase() === s)) {
                    b.setError(r, b.requiredErrorAttribute, b.defaultRequiredError)
                }
            }
        }
    };
    b.validateType = function(p) {
        var m = p[this.validationTypeAttribute];
        var n = this.getValue(p);
        if ((m) && (n !== null)) {
            m = m.toLowerCase();
            if ((m === "address") && (!n.match(b.stringPattern))) {
                b.setValidateTypeError(p)
            } else {
                if ((m === "alphanumeric") && (!n.match(b.alphaNumericPattern))) {
                    b.setValidateTypeError(p)
                } else {
                    if ((m === "amount") && (!b.validateAmount(n))) {
                        b.setValidateTypeError(p)
                    } else {
                        if ((m === "country") && (!n.match(b.stringPattern))) {
                            b.setValidateTypeError(p)
                        } else {
                            if ((m === "email") && (!n.match(b.emailPattern))) {
                                b.setValidateTypeError(p)
                            } else {
                                if ((m === "mod10") && (!b.validateMod10(n))) {
                                    b.setValidateTypeError(p)
                                } else {
                                    if ((m === "name") && (!n.match(b.stringPattern))) {
                                        b.setValidateTypeError(p)
                                    } else {
                                        if ((m === "numeric") && (!b.validateNumeric(n))) {
                                            b.setValidateTypeError(p)
                                        } else {
                                            if ((m.indexOf("date") === 0) && (!b.validateDate(p, m, n))) {
                                                b.setValidateTypeError(p)
                                            } else {
                                                if ((m === "state") && (!n.match(b.stringPattern))) {
                                                    b.setValidateTypeError(p)
                                                } else {
                                                    if ((m === "string") && (!n.match(b.stringPattern))) {
                                                        b.setValidateTypeError(p)
                                                    } else {
                                                        if ((m === "uppercasestring") && (!n.match(b.upperCaseStringPattern))) {
                                                            b.setValidateTypeError(p)
                                                        } else {
                                                            if ((m === "zip") && (!n.match(b.stringPattern))) {
                                                                b.setValidateTypeError(p)
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    };
    b.validateRegex = function(p) {
        var m = p[b.regexAttribute] != undefined ? p[b.regexAttribute] : p.getAttribute(b.regexAttribute);
        var n = b.getValue(p);
        if ((n !== null) && (m) && (!n.match(m))) {
            this.setError(p, b.regexErrorAttribute, b.defaultRegexError)
        }
    };
    b.validateMinLength = function(q) {
        var m = q[b.minLengthAttribute] != undefined ? q[b.minLengthAttribute] : q.getAttribute(b.minLengthAttribute);
        var p = q[b.numericMinLengthAttribute];
        var n = this.getValue(q);
        if ((0 < m) && (n !== null) && (n.length < m)) {
            b.setError(q, b.minLengthErrorAttribute, b.defaultMinLengthError)
        } else {
            if ((0 < p) && (0 < n.length) && (n.replace(b.numericStripper, "").length < p)) {
                b.setError(q, b.minLengthErrorAttribute, b.defaultMinLengthError)
            }
        }
    };
    b.validateMaxLength = function(q) {
        var m = q[b.maxLengthAttribute] != undefined ? q[b.maxLengthAttribute] : q.getAttribute(b.maxLengthAttribute);
        var p = q[b.numericMaxLengthAttribute];
        var n = this.getValue(q);
        if ((0 < m) && (n !== null) && (m < n.length)) {
            b.setError(q, b.maxLengthErrorAttribute, b.defaultMaxLengthError)
        } else {
            if ((0 < p) && (0 < n.length) && (p < n.replace(b.numericStripper, "").length)) {
                b.setError(q, b.maxLengthErrorAttribute, b.defaultMaxLengthError)
            }
        }
    };
    b.validateMinValue = function(p) {
        var m = p[b.minValueAttribute];
        var n = b.getValue(p);
        if ((n !== null) && (m !== undefined) && (0 < m.length)) {
            if ((5 < m.length) && (m.substring(0, 5) === "&gt;=")) {
                if (n < parseFloat(m.substring(5, m.length))) {
                    b.setError(p, b.minValueErrorAttribute, b.defaultMinValueError)
                }
            } else {
                if ((4 < m.length) && (m.substring(0, 4) === "&gt;")) {
                    if (n <= parseFloat(m.substring(4, m.length))) {
                        b.setError(p, b.minValueErrorAttribute, b.defaultMinValueError)
                    }
                } else {
                    if (n < parseFloat(m)) {
                        b.setError(p, b.minValueErrorAttribute, b.defaultMinValueError)
                    }
                }
            }
        }
    };
    b.validateMaxValue = function(p) {
        var m = p[this.maxValueAttribute];
        var n = this.getValue(p);
        if ((n !== null) && (m !== undefined) && (0 < m.length)) {
            if ((5 < m.length) && (m.substring(0, 5) === "&lt;=")) {
                if (n > parseFloat(m.substring(5, m.length))) {
                    b.setError(p, b.maxValueErrorAttribute, b.defaultMaxValueError)
                }
            } else {
                if ((4 < m.length) && (m.substring(0, 4) === "&lt;")) {
                    if (n >= parseFloat(m.substring(4, m.length))) {
                        b.setError(p, b.maxValueErrorAttribute, b.defaultMaxValueError)
                    }
                } else {
                    if (parseFloat(n) > m) {
                        b.setError(p, b.maxValueErrorAttribute, b.defaultMaxValueError)
                    }
                }
            }
        }
    };
    b.validateEquals = function(p) {
        var m = p[b.equalsAttribute];
        var n = b.getValue(p);
        if ((n !== null) && (m !== undefined) && (0 < m.length)) {
            if ((2 < m.length) && (m.substring(0, 2) === "!=")) {
                if (n === m.substring(2, m.length)) {
                    b.setError(p, b.equalsErrorAttribute, b.defaultEqualsError)
                }
            }
        }
    };
    var f = function(t) {
        var r = t.parent();
        var n = r.parent();
        var q = n.is(":hidden");
        var m = r.is(":hidden");
        var p = (n.parent()).is(":hidden");
        var s = !(q || (p || m));
        return s
    };
    b.checkIfValidateDateRangeNeeded = function(t) {
        var p = t[b.dateRangeAttribute];
        var n = t[b.dateRange1HiddenIdAttribute];
        var u = t[b.dateRange2HiddenIdAttribute];
        var x = "";
        var q = "";
        var m = t.id;
        var w = false;
        var r = false;
        var v = null;
        var s = null;
        if ((p !== undefined) && (0 < p.length)) {
            x = m.charAt(m.length - 1);
            if (this.validateNumeric(x)) {
                q = x
            }
            if (("1" === q) || ("" === q)) {
                s = $("#" + u);
                r = f(s);
                if (r) {
                    w = true;
                    v = $("#" + n);
                    b.dateRangeArray[0] = v.val();
                    b.dateRangeArray[1] = s.val()
                }
            }
        }
        return w
    };
    b.validateDateRange = function(q) {
        var r = null;
        var p = false;
        var n = false;
        var m = b.checkIfValidateDateRangeNeeded(q);
        if (m) {
            r = new SKYSALES.Class.MarketDate();
            p = r.datesInOrder(this.dateRangeArray);
            if (!p) {
                this.setError(q, this.dateRangeErrorAttribute, this.defaultError)
            }
        }
    };
    b.isExemptFromValidation = function(m) {
        if (m.id.indexOf(this.namespace) !== 0) {
            return true
        }
        if (this.regexElementIdFilter && (!m.id.match(this.regexElementIdFilter))) {
            return true
        }
        return false
    };
    b.setValidateTypeError = function(m) {
        this.setError(m, this.validationTypeErrorAttribute, this.defaultValidationTypeError)
    };
    b.validateAmount = function(m) {
        if ((!m.match(this.amountPattern)) || (m === 0)) {
            return false
        }
        return true
    };
    b.validateDate = function(r, n, p) {
        var q = "";
        var m = new Date();
        if (n) {
            q = n.toLowerCase()
        }
        if ((q === "dateyear") && ((p < m.getFullYear()) || (!p.match(b.dateYearPattern)))) {
            return false
        } else {
            if ((q === "datemonth") && (!p.match(b.dateMonthPattern))) {
                return false
            } else {
                if ((q === "dateday") && (!p.match(b.DateDayPattern))) {
                    return false
                }
            }
        }
        return true
    };
    b.validateMod10 = function(s) {
        var r = /\D/;
        var p = s.replace(/ /g, "");
        var q;
        var n = 0;
        var t = 0;
        var m = 0;
        if (!r.test(p)) {
            while (p.length < 16) {
                p = "0" + p
            }
            for (m = p.length - 1; 0 <= m; m -= 2) {
                n += parseInt(p.charAt(m), 10);
                q = String((p.charAt(m - 1) * 2));
                for (t = 0; t < q.length; t += 1) {
                    n += parseInt(q.charAt(t), 10)
                }
            }
            return (n % 10 === 0)
        }
        return false
    };
    b.validateNumeric = function(m) {
        m = m.replace(/\s/g, "");
        if (!m.match(b.numericPattern)) {
            return false
        }
        return true
    };
    b.getValue = function(m) {
        return SKYSALES.Common.getValue(m)
    };
    b.stringPattern = /^.+$/;
    b.upperCaseStringPattern = /^[A-Z]([A-Z|\s])*$/;
    b.numericPattern = /^\d+$/;
    b.numericStripper = /\D/g;
    b.alphaNumericPattern = /^\w+$/;
    b.amountPattern = /^(\d+((\.|,|\s|\xA0)\d+)*)$/;
    b.dateYearPattern = /^\d{4}$/;
    b.dateMonthPattern = /^\d{2}$/;
    b.dateDayPattern = /^\d{2}$/;
    b.emailPattern = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,8}$/
};
var validateBySelector = function(b) {
    var c = null;
    var a = null;
    if (b !== undefined) {
        c = new SKYSALES.Validate(null, "", SKYSALES.errorsHeader, null);
        c.clickedObj = a;
        c.runBySelector(b);
        return c.outputErrors()
    }
    return true
};
var validate = function(f, a, c) {
    var b = null;
    var m = null;
    var l = null;
    if (document.getElementById && document.createTextNode) {
        if (f.getAttribute) {
            b = f;
            f = f.getAttribute("id").replace(/_\w+$/, "")
        }
        m = new SKYSALES.Validate(SKYSALES.getSkySalesForm(), f + "_", SKYSALES.errorsHeader, c);
        m.clickedObj = b;
        if (a) {
            l = a;
            if (!a.getAttribute) {
                l = document.getElementById(f + "_" + a)
            }
            m.validateSingleElement(l);
            return m.outputErrors()
        }
        return m.run()
    }
    return true
};
var preventDoubleClick = function() {
    return true
};
var events = [];
var register = function(a, b) {
    if (events[a] === undefined) {
        events[a] = []
    }
    events[a][events[a].length] = b
};
var raise = function(eventName, eventArgs) {
    var ix = 0;
    if (events[eventName] !== undefined) {
        for (ix = 0; ix < events[eventName].length; ix += 1) {
            if (eval(events[eventName][ix] + "(eventArgs)") === false) {
                return false
            }
        }
    }
    return true
};
var WindowInitialize = function() {
    var a = window.onload;
    var b = function() {
        raise("WindowLoad", {});
        if (a) {
            a()
        }
    };
    $(window).ready(b)
};
SKYSALES.Util.displayPopUpConverter = function() {
    var a = "CurrencyConverter.aspx";
    var b = window.converterWindow;
    if (!window.converterWindow || b.closed) {
        b = window.open(a, "converter", "width=360,height=220,toolbar=0,status=0,location=0,menubar=0,scrollbars=0,resizable=0")
    } else {
        b.open(a, "converter", "width=360,height=220,toolbar=0,status=0,location=0,menubar=0,scrollbars=0,resizable=0");
        if ($(b).is(":hidden") === false) {
            b.focus()
        }
    }
};
var hideShow = function(a, c) {
    var e = a;
    var b = c;
    if (document.getElementById && document.getElementById(a)) {
        if (document.getElementById(b).checked === true) {
            document.getElementById(e).style.display = "inline"
        } else {
            document.getElementById(e).style.display = "none"
        }
    }
};
var jsLoadedCommon = true;
SKYSALES.toggleAtAGlanceEvent = function() {
    $(this).next().slideToggle()
};
SKYSALES.toggleAtAGlance = function() {
    $("div.atAGlanceDivHeader").click(SKYSALES.toggleAtAGlanceEvent)
};
SKYSALES.initializeTime = function() {
    var a = 0;
    var b = "";
    for (a = 0; a < 23; a += 1) {
        b += "<option value=" + a + ">" + a + "</option>"
    }
    if (b !== "") {
        $("select.Time").append(b)
    }
};
$("a.animateMe").animate({
    height: "toggle",
    opacity: "toggle"
}, "slow");
SKYSALES.aosAvailabilityShow = function() {
    $(this).parent().find("div.hideShow").show("slow");
    return false
};
SKYSALES.aosAvailabilityHide = function() {
    $(this).parent().parent(".hideShow").hide("slow");
    return false
};
SKYSALES.dropDownMenuEvent = function() {
    $("div.slideDownUp").toggle("fast");
    return false
};
SKYSALES.faqHideShow = function() {
    $(this).parent("dt").next(".accordianSlideContent").slideToggle("slow")
};
SKYSALES.equipHideShow = function() {
    $("div#moreSearchOptions").slideToggle("slow");
    return false
};
SKYSALES.initializeAosAvailability = function() {
    $(".hideShow").hide();
    $("a.showContent").click(SKYSALES.aosAvailabilityShow);
    $("a.hideContent").click(SKYSALES.aosAvailabilityHide);
    $("a.toggleSlideContent").click(SKYSALES.dropDownMenuEvent);
    $("a.accordian").click(SKYSALES.faqHideShow);
    $("a.showEquipOpt").click(SKYSALES.equipHideShow);
    $("a.hideEquipOpt").click(SKYSALES.equipHideShow)
};
SKYSALES.initializeMetaObjects = function() {
    $.metaobjects({
        clean: false
    })
};
SKYSALES.common = new SKYSALES.Common();

function formatCurrency(c) {
    var a = 0;
    c = c.toString();
    if (isNaN(c)) {
        c = "0"
    }
    if (c.indexOf(".") > -1) {
        a = c.substring(c.indexOf(".") + 1, c.length);
        c = c * 100
    }
    a = c % 100;
    if (a > 0) {
        c = Math.floor(c / 100).toString()
    }
    if (a < 10) {
        a = "0" + a
    }
    for (var b = 0; b < Math.floor((c.length - (1 + b)) / 3); b += 1) {
        c = c.substring(0, c.length - (4 * b + 3)) + "," + c.substring(c.length - (4 * b + 3))
    }
    return (c + "." + a)
}
SKYSALES.Util.sendAspFormFields = function() {
    var b = null;
    var e = window.document.getElementById("eventTarget");
    var c = window.document.getElementById("eventArgument");
    var a = window.document.getElementById("viewState");
    return true; //JM var f=window.theForm; if(!f.onsubmit||(f.onsubmit()!==false)){e.name="__EVENTTARGET";c.name="__EVENTARGUMENT";a.name="__VIEWSTATE";if(f.checkValidity){b=function(){$(this).removeAttr("required")};SKYSALES.common.getAllInputObjects().each(b)}}return true};SKYSALES.Util.initStripeTable=function(){$(".hotelResult").hide();var a=function(){$(".stripeMe tr").removeClass("over");$(this).parent().parent().addClass("over")};$(".stripeMe input").click(a)};SKYSALES.Util.ready=function(){$("form").submit(SKYSALES.Util.sendAspFormFields);SKYSALES.initializeMetaObjects();SKYSALES.common.initializeCommon();SKYSALES.Util.initObjects();SKYSALES.initializeSkySalesForm();SKYSALES.toggleAtAGlance();SKYSALES.Util.initStripeTable();SKYSALES.initializeAosAvailability()};$(document).ready(SKYSALES.Util.ready);SKYSALES.openNewWindow=function(b,a,c){var e=window.open(b,a,c)};SKYSALES.tooltip=function(){var e="tt";var w=0;var l=23;var v=700;var m=10;var f=20;var q=95;var n=0;var r,x,s,u,p;var a=document.all?true:false;return{show:function(c,b){if(r==null){r=document.createElement("div");r.setAttribute("id",e);x=document.createElement("div");x.setAttribute("id",e+"left");s=document.createElement("div");s.setAttribute("id",e+"cont");u=document.createElement("div");u.setAttribute("id",e+"right");r.appendChild(x);r.appendChild(s);r.appendChild(u);document.body.appendChild(r);r.style.opacity=0;r.style.filter="alpha(opacity=0)";document.onmousemove=this.pos}r.style.display="block";s.innerHTML=c;r.style.width=b?b+"px":"auto";s.style.width=b?(b-60)+"px":"auto";if(!b&&a){x.style.display="none";u.style.display="none";r.style.width=r.offsetWidth;s.style.width=(r.offsetWidth-60);x.style.display="block";u.style.display="block"}if(r.offsetWidth>v){r.style.width=v+"px";s.style.width=(v-60)+"px"}p=(parseInt(r.offsetHeight)+w)/2;clearInterval(r.timer);r.timer=setInterval(function(){SKYSALES.tooltip.fade(1)},f)},pos:function(t){var c=a?event.clientY+document.documentElement.scrollTop:t.pageY;var b=a?event.clientX+document.documentElement.scrollLeft:t.pageX;r.style.top=(c-p)+"px";r.style.left=(b+l)+"px"},fade:function(t){var b=n;if((b!=q&&t==1)||(b!=0&&t==-1)){var c=m;if(q-b<m&&t==1){c=q-b}else{if(n<m&&t==-1){c=b}}n=b+(c*t);r.style.opacity=n*0.01;r.style.filter="alpha(opacity="+n+")"}else{clearInterval(r.timer);if(t==-1){r.style.display="none"}}},hide:function(){clearInterval(r.timer);r.timer=setInterval(function(){SKYSALES.tooltip.fade(-1)},f)}}}();$(document).ready(function(){SKYSALES.startSessionTracking()});window.onbeforeunload=function(){if(SKYSALES.sessionTracking){SKYSALES.sessionTracking.sessionExpiry=0}};SKYSALES.startSessionTracking=function(){if(SKYSALES.sessionTracking&&parseInt(SKYSALES.sessionTracking.sessionExpiry)>0){setTimeout("SKYSALES.launchSessionNotice();",(((parseInt(SKYSALES.sessionTracking.sessionExpiry)*60)-30-SKYSALES.sessionTracking.sessionExpiryCountdown)*1000))}};$(window).resize(function(){if($("#dimmer").length>0){$("#dimmer").css("width",($(document).width())+"px");$("#dimmer").css("height",($(document).height())+"px")}});SKYSALES.launchSessionNotice=function(){if(SKYSALES.sessionTracking&&parseInt(SKYSALES.sessionTracking.sessionExpiry)>0){if($("#dimmer").length>0){$("#dimmer").remove();$("#sessionNoticeBox").remove()}$("body").append('<div id="dimmer"></div><div id="sessionNoticeBox"><span id="sessionNoticeText">'+SKYSALES.sessionTracking.title+"<br/>"+SKYSALES.sessionTracking.noticeText1+'<span id="sessionNoticeCount">'+SKYSALES.sessionTracking.sessionExpiryCountdown+"</span>"+SKYSALES.sessionTracking.noticeText2+"<br/>"+SKYSALES.sessionTracking.noticeText3+'</span><br/><br/><input id="sessionNoticeOK" type="button" value=" OK " /></div>');$("#dimmer").show();$("#sessionNoticeBox").show();$("#dimmer").css("width",($(document).width())+"px");$("#dimmer").css("height",($(document).height())+"px");SKYSALES.sessionNoticeCountdownTick();window.focus();$("#sessionNoticeOK").focus();SKYSALES.windowTitle=document.title;document.title=" ";$("#sessionNoticeOK").click(function(){sCount=0;if($("#sessionNoticeCount").length>0){sCount=parseInt($("#sessionNoticeCount").html())}$("#dimmer").remove();$("#sessionNoticeBox").remove();document.title=SKYSALES.windowTitle;if(sCount>0){$.post("SessionRefresh.aspx");SKYSALES.startSessionTracking()}else{showMsg=false;window.location="Search.aspx"}})}};SKYSALES.sessionNoticeCountdown=function(){sCount=0;if($("#sessionNoticeCount").length>0){sCount=parseInt($("#sessionNoticeCount").html())}if(sCount>0){sCount=sCount-1;$("#sessionNoticeCount").html(sCount);if(sCount>0){SKYSALES.sessionNoticeCountdownTick();if(document.title!=SKYSALES.windowTitle){document.title=SKYSALES.windowTitle}else{document.title=SKYSALES.sessionTracking.noticeWindowTitle}window.focus();$("#sessionNoticeOK").focus()}}if(sCount==0&&$("#sessionNoticeText").length>0){$("#sessionNoticeText").html(SKYSALES.sessionTracking.expiredText)}};SKYSALES.sessionNoticeCountdownTick=function(){setTimeout("SKYSALES.sessionNoticeCountdown();",1000)};SKYSALES.loginDisp="show";SKYSALES.toggleLogin=function(){if(SKYSALES.loginDisp=="show"){$("#loginControl").removeClass("login_expand_icon");$("#loginControl").addClass("login_collapse_icon");$("#loginContainer").show("slide");SKYSALES.loginDisp="hide"}else{$("#loginControl").removeClass("login_collapse_icon");$("#loginControl").addClass("login_expand_icon");$("#loginContainer").hide("slide");SKYSALES.loginDisp="show"}};function toggleOneWayOnly(c,a,b){if($("#"+c).length>0){if($("#"+c).attr("checked")){$("#"+a).click()}else{$("#"+b).click()}}}SKYSALES.selectedFareCellId=new Array();SKYSALES.selectedFareRowId=new Array();SKYSALES.selectedFareColumnId=new Array();SKYSALES.selectedFareCellId[1]="";SKYSALES.selectedFareCellId[2]="";SKYSALES.selectedFareRowId[1]="";SKYSALES.selectedFareRowId[2]="";SKYSALES.selectedFareColumnId[1]="";SKYSALES.selectedFareColumnId[2]="";SKYSALES.hiliteFare=function(l,c,f,b,a,e){if($("#"+c).length>0){$("#"+c).attr("checked",true);$("#"+c).click()}fareCellId=c+"CellId";if($("#"+fareCellId).length>0){if($("#"+SKYSALES.selectedFareCellId[l])){$("#"+SKYSALES.selectedFareCellId[l]).removeClass("resultFareHilite"+e);$("#"+SKYSALES.selectedFareCellId[l]).addClass("resultFare"+e)}$("#"+fareCellId).removeClass("resultFare"+e);$("#"+fareCellId).addClass("resultFareHilite"+e);SKYSALES.selectedFareCellId[l]=fareCellId}if($("#"+f).length>0){if($("#"+SKYSALES.selectedFareRowId[l])){$("#"+SKYSALES.selectedFareRowId[l]).removeClass("selectedAirlineCodeHilite");$("#"+SKYSALES.selectedFareRowId[l]).addClass("selectedAirlineCode")}$("#"+f).removeClass("selectedAirlineCode");$("#"+f).addClass("selectedAirlineCodeHilite");SKYSALES.selectedFareRowId[l]=f}if($("#"+b).length>0){if($("#"+SKYSALES.selectedFareColumnId[l])){$("#"+SKYSALES.selectedFareColumnId[l]).removeClass("resultClassHilite");$("#"+SKYSALES.selectedFareColumnId[l]).addClass("resultClass")}$("#"+b).removeClass("resultClass");$("#"+b).addClass("resultClassHilite");SKYSALES.selectedFareColumnId[l]=b}};SKYSALES.changeTandCLink=function(a){if($("#termsAndConditionsLink")){$("#termsAndConditionsLink")[0].href="tandc_"+a+".html"}};SKYSALES.openTermsAndConditions=function(b,f){var e=SKYSALES.AK_defineLanguagePath(f);var a="http://www.airasia.com/"+e+"/tnc_main.html";var c=window.open(a,"TermsAndConditions","width=600,height=500,toolbar=0,status=0,menubar=0,scrollbars=1,resizable=No")};SKYSALES.openFareRules=function(e){var c=SKYSALES.AK_defineLanguagePath(e);var a="http://www.airasia.com/"+c+"/farerules.html ";var b=window.open(a,"FareRules","width=600,height=500,toolbar=0,status=0,menubar=0,scrollbars=1,resizable=No")};SKYSALES.AK_defineLanguagePath=function(a){switch(a){case"bm":language_path="my/bm";break;case"tw":language_path="tw/tw";break;case"ae":language_path="ae/ae";break;case"ch":language_path="hk/tc";break;case"cn":language_path="ch/ch";break;case"en":language_path="my/en";break;case"id":language_path="id/id";break;case"th":language_path="th/th";break;case"vn":language_path="vn/vn";break;case"ko":language_path="kr/ko";break;case"ja":language_path="jp/ja";break;case"fa":language_path="ir/fa";break;default:language_path="my/en"}return language_path};SKYSALES.togglePriceSummary=function(b){var a="";if(b=="OtherTaxesAndFees"){a="#OtherTaxesAndFees"}else{if(b=="PriceMarket1"){a="#PriceMarket1"}else{if(b=="PriceMarket2"){a="#PriceMarket2"}}}if($(a).hasClass("expand_icon")){$(a).removeClass("expand_icon");$(a).addClass("collapse_icon")}else{$(a).removeClass("collapse_icon");$(a).addClass("expand_icon")}};SKYSALES.openAAGo=function(f){var c="";var n="";var e="";var p="";var b=window.location.href;if(b!=null){var l=b.split("/");if(l[3]!=null){e=l[3];e=e.toUpperCase()}if(l[4]!=null){p=l[4];p=p.toUpperCase()}}switch(e){case"AU":c="AUSTRALIA";break;case"BN":c="BRUNEI";break;case"CN":c="CHINA";break;case"HK":c="HONGKONG";break;case"IN":c="INDIA";break;case"ID":c="INDONESIA";break;case"MO":c="MACAU";break;case"MY":c="MALAYSIA";break;case"PH":c="PHILLIPPINES";break;case"SG":c="SINGAPORE";break;case"KR":c="KOREA";break;case"TW":c="TAIWAN";break;case"TH":c="THAILAND";break;case"GB":c="UK";break;case"JP":c="JAPAN";break;case"FR":c="FRANCE";break;case"NZ":c="NEWZEALAND";break;default:c="ROW"}switch(p){case"EN":n="en";break;case"ZH":n="zz";break;case"ID":n="id";break;case"MS":n="ms";break;case"KO":n="kr";break;case"TH":n="th";break;case"JA":n="jp";break;default:n="en"}if((e=="CN")&&(p=="ZH")){n="zh"}var a=f+n+c;var m=window.open(a,"_self","")};SKYSALES.get_url_param=function(b){b=b.replace(/[\[]/,"\\[").replace(/[\]]/,"\\]");var a="[\\?&]"+b+"=([^&#]*)";var e=new RegExp(a);var c=e.exec(window.location.href);if(c==null){return""}else{return c[1]}};SKYSALES.ShowContent=function(a){document.getElementById(d).style.display="block"} /*! This file is part of the Navitaire NewSkies application. Copyright (C) Navitaire. All rights reserved. */ ;if(SKYSALES.Class.AvailabilityInput===undefined){SKYSALES.Class.AvailabilityInput=function(){var a=this;a.detailsLinks=null;a.journeyInfoArray=[];a.productClasses=[];a.fareClasses=[];a.fareCategories=[];a.departureDates=[null,null];a.submitButtonId="";a.restrictSameFareCategory="";a.sameCategoryMessage="";a.agreement24HrMessage="";a.incompleteFareSelection="";a.isNewBooking="true";a.isWithin24Hours=[false,false];a.WithinTimeLimit=120;a.isWithinTimeLimit=[false,false];a.agreement24HrsId="";a.agreement24HrsCheckboxId="";a.agreement24HrsNoteId="";a.taxAndFeeProgressMessage="";a.submitButton=null;a.isEligibleFor24Hrs=null;a.hideMarketIndex="";a.marketCount=null;a.preselectFares=null;a.clientId="";a.hiflyerFares=null;a.isFlightChange=false;a.addEvents=function(){a.addGetPriceItineraryInfoEvents();a.addEquipmentPropertiesAjaxEvents();a.submitButton.click(a.validateEvent);a.addFareCategoryDisplayEvents();a.agreementCheckBoxEvents()};a.setVars=function(){var b=(a.clientId).split("_");a.submitButtonId=b[0]+"_ButtonSubmit";a.detailsLinks=$(".showContent");a.submitButton=$("#"+a.submitButtonId)};a.agreementCheckBoxEvents=function(){if($("#ControlGroupSelectView_AgreementInputSelectView_CheckBoxAgreement").length>0){$("#ControlGroupSelectView_AgreementInputSelectView_CheckBoxAgreement").click(function(){$("#validationErrorContainerReadAlong").hide();$("#validationErrorContainerReadAlongIFrame").hide()})}if($("#ControlGroupSelectChangeView_AgreementInputSelectChangeView_CheckBoxAgreement").length>0){$("#ControlGroupSelectChangeView_AgreementInputSelectChangeView_CheckBoxAgreement").click(function(){$("#validationErrorContainerReadAlong").hide();$("#validationErrorContainerReadAlongIFrame").hide()})}};a.addFareCategoryDisplayEvents=function(){for(j=1;j<=2;j++){for(i=0;i<this.fareCategories.length;i++){if($("#icon"+this.fareCategories[i].fareCategoryName+j).length>0&&$("#description"+this.fareCategories[i].fareCategoryName).length>0){$("#icon"+this.fareCategories[i].fareCategoryName+j).addClass("pointer");$("#icon"+this.fareCategories[i].fareCategoryName+j).mouseover(function(){a.FareCategoryDisplayMouseover(this)});$("#icon"+this.fareCategories[i].fareCategoryName+j).mouseout(function(){a.FareCategoryDisplayMouseout(this)})}}}};a.FareCategoryDisplayMouseover=function(c){if(!c.classList){c.classList=c.className.split(" ")}if($("#description"+c.classList[0]).length>0){var b=parseInt($("#icon"+c.classList[1]).offset().top+35)+"px";var e=parseInt($("#icon"+c.classList[1]).offset().left-70)+"px";$("#description"+c.classList[0]).css({top:b,left:e});$("#description"+c.classList[0]).show()}};a.FareCategoryDisplayMouseout=function(b){if(!b.classList){b.classList=b.className.split(" ")}if(b.classList){if($("#description"+b.classList[0]).length>0){$("#description"+b.classList[0]).hide()}}};a.initJourneyInfoContainers=function(){var b=0;var c=this.journeyInfoList;var e=null;for(b=0;b<c.length;b+=1){e=new SKYSALES.Class.JourneyInfo();e.init(c[b]);a.journeyInfoArray[a.journeyInfoArray.length]=e}};a.getPriceItineraryInfo=function(){if(undefined!==SKYSALES.taxAndFeeInclusiveDisplayDataRequestHandler){var b=$("#selectMainBody div[@class^='availabilityInputContent'] .fareTable tr td[@class^='resultFareCell'] div[@class^='resultFare'] div[@class^='fareRadio'] :radio[@checked]");var c=[];$(b).each(function(e){var f=$(this).val().split("@");c[e]=f[0]});SKYSALES.taxAndFeeInclusiveDisplayDataRequestHandler(c,b.length,a.taxAndFeeProgressMessage);SKYSALES.fareRuleHandler(c,b.length)}};a.updateFareInfo=function(){var b=$("#selectMainBody div[@class^='availabilityInputContent'] .fareTable tr td[@class^='resultFareCell'] div[@class^='resultFare'] div[@class^='fareRadio'] :radio[@checked]");if($("#agreementInput").is(":hidden")===true){$("#agreementInput").show();if($("#ControlGroupSelectView_AgreementInputSelectView_CheckBoxAgreement").length>0){$("#ControlGroupSelectView_AgreementInputSelectView_CheckBoxAgreement").attr("required","true")}else{$("#ControlGroupSelectChangeView_AgreementInputSelectChangeView_CheckBoxAgreement").attr("required","true")}}$(b).each(function(f){var n=$(this).val().split("@");var m=n[0].split("~");var e=new Date();var l=new Date();a.fareClasses[f]=this.getAttribute("productClass");a.departureDates[f]=new Date(n[1]);l.setHours(e.getHours()+4+(e.getTimezoneOffset()/60));a.isWithin24Hours[f]=false;if((a.departureDates[f]<l)&&((a.hideMarketIndex=="")||(a.hideMarketIndex!=f))){a.isWithin24Hours[f]=true}var c=a.departureDates[f].getTime()-e.getTime();c=Math.ceil(c/(1000*60));a.isWithinTimeLimit[f]=false;if(c!=NaN){if((c<=a.WithinTimeLimit)&&((a.hideMarketIndex=="")||(a.hideMarketIndex!=f))){a.isWithinTimeLimit[f]=true}}});a.update24HrCheckbox();a.update24HrNote()};a.update24HrCheckbox=function(){if(a.isNewBooking=="false"){var b=true;b=(!a.isWithin24Hours[0])&&(!a.isWithin24Hours[1]);if(b==true){$("#"+a.agreement24HrsId).hide()}else{$("#"+a.agreement24HrsId).show()}}};checkIfHiFlyer=function(e){var b=a.isFlightChange==true?(a.hiflyerFares!=null&&a.hiflyerFares.length>0):false;if(!b){return true}for(var c=0;c<a.hiflyerFares.length;c++){if(a.hiflyerFares[c]==e){return true}}return false};a.update24HrNote=function(){var e=true;var c=false;if(a.hideMarketIndex=="0"){e=(!a.isWithin24Hours[1]);c=checkIfHiFlyer(1)}else{if(a.hideMarketIndex=="1"){e=(!a.isWithin24Hours[0]);c=checkIfHiFlyer(0)}else{e=(!a.isWithin24Hours[0])&&(!a.isWithin24Hours[1]);c=checkIfHiFlyer(0)&&checkIfHiFlyer(1)}}var b=a.isEligibleFor24Hrs&&c;if(!e&&b==false){$("#"+a.submitButtonId).hide();$("#agreementInputCheckbox").hide();$("#SpecialNeedssection").hide();$("#"+a.agreement24HrsNoteId).show()}else{$("#"+a.agreement24HrsNoteId).hide();$("#agreementInput").show();$("#agreementInputCheckbox").show();$("#SpecialNeedssection").show();$("#"+a.submitButtonId).show()}};a.showPreselectedFares=function(b){for(var c=0;c<b.length;c+=1){if(b[c]!==null){$("#"+b[c]).click()}}};a.setPreselectedFares=function(){var n=$("#selectMainBody div[@class^='availabilityInputContent'] .fareTable tr td[@class^='resultFareCell'] div[@class^='resultFare'] div[@class^='fareRadio']");var e=n.find(":radio[@checked]").length;if(e===0){$("#taxAndFeeInclusiveDivBody").remove()}else{if(e===1){$("#taxesAndFeesInclusiveDisplay_2").hide()}}var f=-1;var p=-1;var l="";var c="";var m=-1;$("td[@class^='resultFareCell']",$("#fareTable1_1")).each(function(){var r=$(this);var q=r.find(":radio[@name$='market1']").attr("id");if(q){m=$.trim(r.find("div[@class='paxPriceDisplay']").find("span").text());m=parseFloat(m.replace(/[^0-9\.]+/,""));if(!isNaN(m)){if(m<f||f<0){f=m;l=q}}}});$("#"+l).attr("checked","checked");$("td[@class^='resultFareCell']",$("#fareTable2_1")).each(function(){var r=$(this);var q=r.find(":radio[@name$='market2']").attr("id");if(q){m=$.trim(r.find("div[@class='paxPriceDisplay']").find("span").text());m=parseFloat(m.replace(/[^0-9\.]+/,""));if(!isNaN(m)){if(m<p||p<0){p=m;c=q}}}});$("#"+c).attr("checked","checked");a.getPriceItineraryInfo();a.updateFareInfo();var b=[];b[0]=l;b[1]=c;a.showPreselectedFares(b)};a.addGetPriceItineraryInfoEvents=function(){$("#selectMainBody div[@class^='availabilityInputContent'] table[@id^='fareTable1'] tr td[@class^='resultFareCell'] div[@class^='resultFare'] div[@class^='fareRadio'] :radio").each(function(){$(this).click(function(){a.getPriceItineraryInfo();a.updateFareInfo()})});$("#selectMainBody div[@class^='availabilityInputContent'] table[@id^='fareTable2'] tr td[@class^='resultFareCell'] div[@class^='resultFare'] div[@class^='fareRadio'] :radio").each(function(){$(this).click(function(){a.getPriceItineraryInfo();a.updateFareInfo()})})};a.getFareCategory=function(b){var c="";for(i=0;i<a.fareCategories.length;i++){if(a.fareCategories[i].productClasses.indexOf("["+b+"]")>-1){c=a.fareCategories[i].fareCategoryName}}return c};a.validateEvent=function(){var b=true;var c=false;var f=false;c=((a.isWithin24Hours[0])||(a.isWithin24Hours[1]));f=((a.isWithinTimeLimit[0])||(a.isWithinTimeLimit[1]));var e=$("#selectMainBody div[@class^='availabilityInputContent'] .fareTable tr td[@class^='resultFareCell'] div[@class^='resultFare'] div[@class^='fareRadio'] :radio[@checked]").length;if(e<a.marketCount){alert(a.incompleteFareSelection);return false}if(a.isEligibleFor24Hrs==false){if(a.isNewBooking=="true"){if(c){return false}}if(a.isNewBooking=="false"){if(f){return false}if(c){if($("#"+a.agreement24HrsCheckboxId+":checked").length==0){alert(a.agreement24HrMessage);return false}}}}if(b==true){if(a.restrictSameFareCategory=="True"){if(a.fareClasses.length>1){b=false;if(a.fareClasses[0]==a.fareClasses[1]){b=true}else{if(a.getFareCategory(a.fareClasses[0])==a.getFareCategory(a.fareClasses[1])){b=true}}}if(b==false){alert(a.sameCategoryMessage);return false}}}};a.ajaxEquipmentProperties=function(){};a.addEquipmentPropertiesAjaxEvent=function(){$(this).click(a.ajaxEquipmentProperties)};a.addEquipmentPropertiesAjaxEvents=function(){a.detailsLinks.each(a.addEquipmentPropertiesAjaxEvent)};a.init=function(){a.initJourneyInfoContainers();if(undefined!==SKYSALES.taxAndFeeInclusiveDisplayDataRequestHandler){a.setVars();if(a.preselectFares=="true"){a.setPreselectedFares();if($("#ControlGroupSelectView_AgreementInputSelectView_CheckBoxAgreement").length>0){$("#ControlGroupSelectView_AgreementInputSelectView_CheckBoxAgreement").attr("required","true")}else{$("#ControlGroupSelectChangeView_AgreementInputSelectChangeView_CheckBoxAgreement").attr("required","true")}}else{a.updateFareInfo();$("#agreementInput").hide();if($("#ControlGroupSelectView_AgreementInputSelectView_CheckBoxAgreement").length>0&&$("#ControlGroupSelectView_AgreementInputSelectView_CheckBoxAgreement")[0].required==="true"){$("#ControlGroupSelectView_AgreementInputSelectView_CheckBoxAgreement").attr("required","false")}else{if($("#ControlGroupSelectChangeView_AgreementInputSelectChangeView_CheckBoxAgreement").length>0&&$("#ControlGroupSelectChangeView_AgreementInputSelectChangeView_CheckBoxAgreement")[0].required==="true"){$("#ControlGroupSelectChangeView_AgreementInputSelectChangeView_CheckBoxAgreement").attr("required","false")}}}a.addEvents()}}}}if(SKYSALES.Class.JourneyInfo===undefined){SKYSALES.Class.JourneyInfo=function(){var a=this;a.equipmentInfoUri="EquipmentPropertiesDisplayAjax-resource.aspx";a.key="";a.journeyContainerId="";a.activateJourneyId="";a.activateJourney=null;a.deactivateJourneyId="";a.deactivateJourney=null;a.journeyContainer=null;a.legInfoArray=[];a.clientName="EquipmentPropertiesDisplayControlAjax";a.init=function(b){this.setSettingsByObject(b);this.setVars();this.addEvents()};a.setVars=function(){a.journeyContainer=$("#"+a.journeyContainerId);a.activateJourney=$("#"+a.activateJourneyId);a.deactivateJourney=$("#"+a.deactivateJourneyId)};a.addEvents=function(){a.activateJourney.click(a.show);a.deactivateJourney.click(a.hide)};a.setSettingsByObject=function(b){var c="";for(c in b){if(a.hasOwnProperty(c)){a[c]=b[c]}}};a.showWithData=function(m){var p=$(m).html();var e=SKYSALES.Json.parse(p);var n=e.legInfo;var f=null;var c="";var s=null;var q="";var b=null;var l=0;var r=null;for(c in n){if(n.hasOwnProperty(c)){q="";f=n[c];if(f.legIndex!==undefined){s=$("#propertyContainer_"+a.key);b=f.equipmentPropertyArray;for(l=0;l<b.length;l+=1){r=b[l];q+="<div>"+r.name+": "+r.value+"</div>"}s.html(q)}}}a.journeyContainer.show("slow")};a.show=function(){var c=a.legInfoArray;var f=null;var b={};var m="";var e=0;var l=a.clientName;for(e=0;e<c.length;e+=1){f=c[e];for(m in f){if(f.hasOwnProperty(m)){b[l+"$legInfo_"+m+"_"+e]=f[m]}}}$.post(a.equipmentInfoUri,b,a.showWithData)};a.hide=function(){a.journeyContainer.hide()};return a}; /*! This file is part of the Navitaire NewSkies application. Copyright (C) Navitaire. All rights reserved. */ }if(!SKYSALES.Class.SsrPassengerInput){SKYSALES.Class.SsrPassengerInput=function(){var b=SKYSALES.Class.SkySales();var a=SKYSALES.Util.extendObject(b);a.ssrFormArray=null;a.ssrFeeArray=null;a.errorMsgOverMaxPerPassenger="There has been an error";a.ssrButtonIdArray=null;a.ssrButtonArray=null;a.buttonTrackId="";a.buttonTrack=null;a.init=function(c){this.setSettingsByObject(c);this.setVars();this.addEvents();$("table.ssrSoldContainer :input",this.container).attr("disabled","disabled")};a.setVars=function(){a.buttonTrack=$("#"+this.buttonTrackId);a.ssrButtonIdArray=this.ssrButtonIdArray||[];var l=[];var e=0;var c=null;var f="";for(e=0;e<this.ssrButtonIdArray.length;e+=1){f=this.ssrButtonIdArray[e];c=$("#"+f);if(c.length>0){l[l.length]=c}}a.ssrButtonArray=l};a.addEvents=function(){this.addButtonClickedEvents()};a.addButtonClickedEvents=function(){var e=0;var c=null;for(e=0;e<this.ssrButtonArray.length;e+=1){c=this.ssrButtonArray[e];c.click(this.updateButtonTrackHandler)}};a.updateButtonTrackHandler=function(){a.buttonTrack.val(this.id)};a.setSettingsByObject=function(m){b.setSettingsByObject.call(this,m);var l=0;var f=this.ssrFormArray||[];var n=null;for(l=0;l<f.length;l+=1){n=new SKYSALES.Class.SsrForm();n.index=l;n.ssrPassengerInput=this;n.init(f[l]);f[l]=n}var e=this.ssrFeeArray||[];var c=null;for(l=0;l<e.length;l+=1){c=new SKYSALES.Class.SsrFormFee();c.index=l;c.ssrPassengerInput=this;c.init(e[l]);e[l]=c}};a.deactivateSsrFormNotes=function(){var e=0;var c=this.ssrFormArray;var f=null;for(e=0;e<c.length;e+=1){f=c[e];f.deactivateNoteDiv()}};return a};SKYSALES.Class.SsrPassengerInput.createObject=function(a){SKYSALES.Util.createObject("ssrPassengerInput","SsrPassengerInput",a)}}SKYSALES.Class.SsrForm=function(){var a=SKYSALES.Class.SkySales();var b=SKYSALES.Util.extendObject(a);b.maximumDropDownLimit=0;b.ssrPassengerId="";b.ssrPassenger=null;b.ssrCodeId="";b.ssrCode=null;b.ssrQuantityId="";b.ssrQuantity=null;b.ssrNoteId="";b.ssrNote=null;b.ssrNoteIframeId="";b.ssrNoteIframe=null;b.ssrNoteCloseId="";b.ssrNoteClose=null;b.ssrNoteDivId="";b.ssrNoteDiv=null;b.ssrNoteImageId="";b.ssrNoteImage=null;b.ssrNoteCancelId="";b.ssrNoteCancel=null;b.ssrFlightId="";b.ssrFlight=null;b.ssrAmountId="";b.ssrAmount=null;b.ssrCurrencyId="";b.ssrCurrency=null;b.index=-1;b.ssrPassengerInput=null;b.init=function(c){this.setSettingsByObject(c);this.setVars();this.addEvents();this.updateSsrAmount()};b.setVars=function(){b.ssrNote=$("#"+this.ssrNoteId);b.ssrNoteDiv=$("#"+this.ssrNoteDivId);b.ssrNoteClose=$("#"+this.ssrNoteCloseId);b.ssrNoteCancel=$("#"+this.ssrNoteCancelId);b.ssrNoteImage=$("#"+this.ssrNoteImageId);b.ssrNoteIframe=$("#"+this.ssrNoteIframeId);b.ssrQuantity=$("#"+this.ssrQuantityId);b.ssrPassenger=$("#"+this.ssrPassengerId);b.ssrCode=$("#"+this.ssrCodeId);b.ssrCurrency=$("#"+this.ssrCurrencyId);b.ssrFlight=$("#"+this.ssrFlightId);b.ssrAmount=$("#"+this.ssrAmountId)};b.addEvents=function(){this.addNoteEvents();this.addQuantityEvents();this.addSSRCodeEvents();this.addFlightEvents()};b.addFlightEvents=function(){this.ssrFlight.change(this.updateSsrAmountHandler)};b.updateSsrQuantityHandler=function(){b.updateSsrQuantity()};b.updateSsrAmountHandler=function(){b.updateSsrAmount()};b.addSSRCodeEvents=function(){this.ssrCode.change(this.updateSsrQuantityHandler);this.ssrCode.change(this.updateSsrAmountHandler)};b.addQuantityEvents=function(){this.ssrQuantity.change(this.updateSsrAmountHandler);this.ssrQuantity.blur(this.updateSsrAmountHandler)};b.updateSsrAmount=function(){var q=this.ssrAmount;var t=SKYSALES.Util.convertToLocaleCurrency("0.00");q.val(t);var s=this.ssrPassenger.val();var n=this.ssrCode.val();var l=this.ssrQuantity.val();l=$.trim(l);var u=/^[0-9]+$/;var m=0;var r=this.ssrPassengerInput;var c=r.ssrFeeArray;var e=null;var f="";var p=0;if(u.test(l)){f=this.ssrFlight.val();for(m=0;m<c.length;m+=1){e=c[m];if((f==="all")||(f===e.segmentKey)){if((s===e.passengerNumber)&&(n===e.ssrCode)){p+=(e.amount*l)}}}t=SKYSALES.Util.convertToLocaleCurrency(p);q.val(t)}else{this.ssrQuantity.val(0)}};b.updateSsrQuantity=function(){var u=this.maximumDropDownLimit;u=window.parseInt(u,10);var t=this.ssrPassenger.val();var q=this.ssrCode.val();var n=this.ssrFlight.val();var r=0;var p=0;var s=this.ssrPassengerInput;var e=s.ssrFeeArray;var f=null;var m=this.ssrQuantity.val();m=parseInt(m,10);var v=0;var c=null;var l=this.ssrQuantity.get(0);for(r=0;r<e.length;r+=1){f=e[r];if((n==="all")||(n===f.segmentKey)){if((t===f.passengerNumber)&&(q===f.ssrCode)){v=parseInt(f.maxPerPassenger,10);if(v===0){v=u;v=parseInt(v,10);if(m>=v){v=m;v=v+1}}if(l.options){while(l.options.length>0){l.options[0]=null}for(p=0;p<=v;p+=1){c=new window.Option(p,p);l.options[p]=c;if(m===p){this.ssrQuantity.val(p)}}}if(m>v){this.ssrQuantity.val(v);alert(this.getErrorMsgOverMaxPerPassenger())}else{this.ssrQuantity.val(m)}}}}};b.getErrorMsgOverMaxPerPassenger=function(){var c="";c=this.ssrPassengerInput.errorMsgOverMaxPerPassenger;return c};b.clearAndDeactivateNoteDiv=function(){var e=this.ssrNote;var c=e.is(":disabled");if(c===false){e.val("")}this.deactivateNoteDiv()};b.deactivateNoteDiv=function(){this.ssrNoteDiv.hide();this.ssrNoteIframe.hide()};b.activateNoteDiv=function(){this.ssrPassengerInput.deactivateSsrFormNotes();var l=this.ssrNoteImage.get(0);var m=SKYSALES.Dhtml();var f=m.getX(l);var e=m.getY(l);this.ssrNoteDiv.css("left",f+"px");this.ssrNoteDiv.css("top",e+"px");this.ssrNoteDiv.show();this.ssrNoteIframe.css("left",f+"px");this.ssrNoteIframe.css("top",e+"px");this.ssrNoteIframe.show();var c=this.ssrNote.is(":disabled");if(c===false){this.ssrNote.click()}};b.ssrNoteCancelHandler=function(){b.clearAndDeactivateNoteDiv()};b.ssrNoteCloseHandler=function(){b.deactivateNoteDiv()};b.ssrNoteImageHandler=function(){b.activateNoteDiv()};b.addNoteEvents=function(){this.ssrNoteCancel.mouseup(this.ssrNoteCancelHandler);this.ssrNoteClose.mouseup(this.ssrNoteCloseHandler);this.ssrNoteImage.mouseup(this.ssrNoteImageHandler)};return b};SKYSALES.Class.SsrFormFee=function(){var b=SKYSALES.Class.SkySales();var a=SKYSALES.Util.extendObject(b);a.journeyIndex=-1;a.segmentIndex=-1;a.segmentKey="";a.passengerNumber=-1;a.ssrCode="";a.feeCode="";a.amount=0;a.currencyCode="";a.maxPerPassenger=0;a.index=-1;a.ssrPassengerInput=null;return a};SKYSALES.taxAndFeeInclusiveDisplayDataRequestHandler=function taxAndFeeInclusiveDisplayDataRequestHandler(c,b,a){var l=",";var f={flightKeys:c.join(l),numberOfMarkets:b,keyDelimeter:l};var m=function(){var r=$("taxAndFeeInclusiveTotal");var p=$("allUpPricing");if(SKYSALES.common){SKYSALES.common.stripeTables(p)}var n=new SKYSALES.Class.ToggleView();var q={elementId:"allUpPricing",hideId:"closeTotalPrice",showId:"taxAndFeeInclusiveTotal"};n.init(q);var s=function(){p.toggle()};if(r&&p){r.click(s)}};var e=function(n){n="<div>"+n+"</div>";if(window.$){$("#taxAndFeeInclusiveDivHeader").before("<div id='tempDiv'></div>");$("#taxAndFeeInclusiveDivHeader").remove();$("#taxAndFeeInclusiveDivBody").remove();$("#tempDiv").after($(n).find("#taxAndFeeInclusiveDivHeader"));$("#taxAndFeeInclusiveDivHeader").after($(n).find("#taxAndFeeInclusiveDivBody"));$("#tempDiv").remove();if(b===1){$("table#taxesAndFeesInclusiveDisplay_2").hide()}}m()};$.get("TaxAndFeeInclusiveDisplayAjax-resource.aspx",f,e);inProcessHTMLString='<div id="taxAndFeeInclusiveDivBody" class="atAGlanceDivBody">'+a+'<div id="waitImg"></div></div>';$("#taxAndFeeInclusiveDivBody").remove();$("#taxAndFeeInclusiveDivHeader").after(inProcessHTMLString)}; /*! This file is part of the Navitaire Professional Services customization. Copyright (C) Navitaire. All rights reserved. */ if(!SKYSALES.Class.TravelerInputContainer){SKYSALES.Class.TravelerInputContainer=function(){var f=SKYSALES.Class.SkySales(),a=SKYSALES.Util.extendObject(f);a.menuSectionId="";a.menuSection=null;a.menuListId="";a.menuList=null;a.mainTravelerId="";a.mainTraveler=null;a.travelerInputArray=null;a.submitButtonId="";a.submitButton=null;a.continueButtonId="";a.continueButton=null;a.confirmText="";a.nextButtonId="";a.nextButton=null;a.nextButtons=null;a.disableNames=null;a.disableContactNames=null;a.infantText="";a.infantPreText="";a.childText="";a.childPreText="";a.infantFutureBirthDateErrorPreText="";a.infantFutureBirthDateErrorPostText="";a.returnDate="";a.passportExpDays="";a.adultPassportMsgText="";a.adultPassportMsgPreText="";a.childPassportMsgText="";a.childPassportMsgPreText="";a.infantPassportMsgText="";a.infantPassportMsgPreText="";a.duplicateAdultTextPre="";a.duplicateAdultText="";a.friendsArray=null;a.addFriendsButtonId="";a.refreshFriendsButtonId="";a.addFriendsButton=null;a.refreshFriendsButton=null;a.addFriendsDiv="";a.addFriendsErrorText="";a.noTickedFriendsText="";a.continueButtonText="";a.submitButtonText="";a.paxTypesArray=null;a.changeCurrencyPrompter="";a.unmatchedPaxTypeErrorText="";a.passportNumber="";a.passportExpDate=null;a.passportExpMonth=null;a.passportExpYear=null;var m=2;a.init=function(n){this.setSettingsByObject(n);this.setVars();this.addEvents();if(a.travelerInputArray.length>0){for(i=0;i<a.travelerInputArray.length;i+=1){a.travelerInputArray[i].travelerInputArray=this.travelerInputArray;a.travelerInputArray[i].hideContent();if(a.travelerInputArray[i].contentId=="content_0"||(a.addFriendsDiv!=null&&a.travelerInputArray[i].contentId==a.addFriendsDiv)){if(this.disableContactNames!=null&&this.disableContactNames!=""&&a.travelerInputArray[i].contentId=="content_0"){a.travelerInputArray[i].disableTextFields()}else{continue}}else{if(this.disableNames!=null&&this.disableNames!=""){a.travelerInputArray[i].disableTextFields()}}}a.travelerInputArray[0].showContent()}if(this.changeCurrencyPrompter!=""){alert(this.changeCurrencyPrompter)}};a.setVars=function(){a.menuSection=$("#"+this.menuSectionId);a.menuList=$("#"+this.menuListId);a.mainTraveler=$("#"+this.mainTravelerId);a.submitButton=$("input[id='"+this.submitButtonId+"']");if(a.submitButtonText!=""){a.submitButton.attr("value",a.submitButtonText)}a.addFriendsButton=$("input[id='"+this.addFriendsButtonId+"']");a.continueButton=$("#"+this.continueButtonId);a.nextButton=$("input[type='button'][id='"+this.nextButtonId+"']");a.refreshFriendsButton=$("input[id='"+this.refreshFriendsButtonId+"']");a.passportNumber=$("#"+this.passportNumber+" input[id*=TextBoxDocumentNumber]");a.passportExpDate=$("#"+this.passportExpDate+" select[id*=DropDownListDocumentDateDay]");a.passportExpMonth=$("#"+this.passportExpMonth+" select[id*=DropDownListDocumentDateMonth]");a.passportExpYear=$("#"+this.passportExpYear+" select[id*=DropDownListDocumentDateYear]")};a.addEvents=function(){a.submitButton.click(a.pickSeatHandler);a.nextButton.click(a.nextTabHandler);a.addFriendsButton.click(a.addFriends);a.refreshFriendsButton.click(a.clearAllFriends);if(a.confirmText==null||a.confirmText==""){a.continueButton.attr("value",a.continueButtonText);a.continueButton.click(a.pickSeatHandler)}else{a.continueButton.click(a.confirmPageHandler)}};a.pickSeatHandler=function(){var n=a.validateForms();return n};a.addFriends=function(){var r=$("#"+a.addFriendsDiv+" :checked").length;var p=0;for(p=0;p<a.paxTypesArray.length;p++){a.paxTypesArray[p].remainingTabs=a.countPaxType(a.paxTypesArray[p].paxType)}var n=a.checkPaxTypeCount();if(r>0&&n){if($("#"+a.addFriendsDiv+" :checked").length>0){a.clearAllFriends();var q=0;$("#"+a.addFriendsDiv+" :checked").each(function(){var s=parseInt($(this).val());var t=a.uploadFriend(s);m=q==0?t:(m>t?t:m);q++});a.travelerInputArray[m].toggleMenuHandler()}}else{if(r==0){alert(a.noTickedFriendsText)}else{if(r>(a.travelerInputArray.length-2)){alert(a.addFriendsErrorText)}else{if(!n){alert(a.unmatchedPaxTypeErrorText)}}}}};a.checkPaxTypeCount=function(){var p=0;for(p=0;p<a.paxTypesArray.length;p++){var t=a.paxTypesArray[p].paxType;var s=0;var u=false;var n=$("input[paxType="+t+"]:checked").length;if(n>0){var r=a.paxTypesArray[p].codesMatchArray;for(s=0;s<r.length;s++){var q=a.searchPaxTypeIndex(r[s].typeCode);if(n<=a.paxTypesArray[q].remainingTabs){u=true;a.paxTypesArray[q].remainingTabs-=n;break}else{if(a.paxTypesArray[q].remainingTabs>0&&n>a.paxTypesArray[q].remainingTabs){n-=a.paxTypesArray[q].remainingTabs;a.paxTypesArray[q].remainingTabs=0}}}if(u==false){return false}}}return true};a.searchPaxTypeIndex=function(n){var p=0;for(p=0;p<a.paxTypesArray.length;p++){if(a.paxTypesArray[p].paxType==n){return p}}};var c=function(s,n){var r=0;for(r=0;r<a.paxTypesArray.length;r++){if(a.paxTypesArray[r].paxType==s){var q=0;var p=a.paxTypesArray[r].codesMatchArray;for(q=0;q<p.length;q++){var t=a.findPaxType(p[q].typeCode,n);if(t!=null){return t}}break}}return a.findPaxType(a.paxTypesArray[0].paxType,n)};var b=function(q){var n=0;var p=0;for(n=0;n<a.friendsArray.length;n++){if(a.friendsArray[n].sequence==q){p=n;break}}return p};a.clearAllFriends=function(){var n=0;for(n=2;n<a.travelerInputArray.length;n++){a.travelerInputArray[n].clearFriend()}};a.uploadFriend=function(q){var r=q>0?this.friendsArray[b(q)]:contactData;var p=q>0?c(r.paxType,false):c("ADT",false);var s=this.travelerInputArray[p];var n=q>0?r.customerNumber:r.ff;s.hasFriend=true;s.travelerName.firstName.val(r.firstName);s.travelerName.lastName.val(r.lastName);s.dropdownTitle.val(r.title);s.travelerName.writeNameHandler();s.dropdownNationality.val(r.nationality);s.dropdownGender.val(r.gender);s.dropdownBirthDateMonth.val(r.bdayMonth);s.dropdownBirthDateDay.val(r.bdayDay);s.dropdownBirthDateYear.val(r.bdayYear);s.passportNumber.val(r.documentNumber);s.passportCountry.val(r.documentCountry);s.passportExpDate.val(r.documentDay);s.passportExpMonth.val(r.documentMonth);s.passportExpYear.val(r.documentYear);return p};a.validateForms=function(){var p=this.continueButton.get(0);if(p==null){p=this.submitButton.get(0)}if(a.validateInfants()){var n=validate(p);if(!n){this.customValidateHandler()}if(a.validatePassportExps()){return n}else{return false}}return false};a.validateInfants=function(){var n=0;var p=0;for(n=0;n<a.travelerInputArray.length;n++){if((a.travelerInputArray[n].paxType=="INF"||a.travelerInputArray[n].paxType=="CHD")&&!l(n)){return false}}return true};a.validatePassportExps=function(){var n=0;for(n=0;n<a.travelerInputArray.length;n++){if(!e(n)){return false}}return true};a.confirmPageHandler=function(){var n=a.confirmPage();return n};a.confirmPage=function(){var p=window.confirm(this.confirmText);if(p){document.getElementById(this.submitButtonId).click();return false}else{var n=this.validateForms();return n}};a.customValidateHandler=function(){var n=null;for(i=0;i<a.travelerInputArray.length;i+=1){n=$("#"+a.travelerInputArray[i].contentId+" :input.validationError, :select.validationError, :radio.validationError");if(n!=null&&n.length>0){a.travelerInputArray[i].toggleMenuHandler();n[0].focus();break}}};a.nextTabHandler=function(){var p=null;var n=0;for(n=0;n<a.travelerInputArray.length;n+=1){p=a.travelerInputArray[n];if(p.selectedTab==0){if(l(n)&&validateBySelector("div[id='"+p.contentId+"']")){p.travelerInputArray[n].hideContent();a.travelerInputArray[n+1].showContent()}break}}return false};var e=function(r){if(a.travelerInputArray[r].passportNumber.val()!=""&&a.travelerInputArray[r].passportNumber.length>0){var q=a.returnDate.split("/");var s=new Date(q[2],q[0]-1,q[1]);s.setDate(s.getDate()+parseInt(a.passportExpDays));var n=a.findPaxType("ADT",null);var u=a.findPaxType("CHD",null);var t=a.findPaxType("INF",null);if(a.travelerInputArray[r].paxType=="ADT"){if(a.travelerInputArray[r].passportExpYear.val()!=""&&a.travelerInputArray[r].passportExpMonth.val()!=""&&a.travelerInputArray[r].passportExpDate.val()!=""){var p=new Date();p.setFullYear(a.travelerInputArray[r].passportExpYear.val(),(a.travelerInputArray[r].passportExpMonth.val()-1),a.travelerInputArray[r].passportExpDate.val());if(s>p){alert(a.adultPassportMsgPreText+((r-n)+1)+a.adultPassportMsgText);return true}return true}return true}else{if(a.travelerInputArray[r].paxType=="CHD"){if(a.travelerInputArray[r].passportExpYear.val()!=""&&a.travelerInputArray[r].passportExpMonth.val()!=""&&a.travelerInputArray[r].passportExpDate.val()!=""){var p=new Date();p.setFullYear(a.travelerInputArray[r].passportExpYear.val(),(a.travelerInputArray[r].passportExpMonth.val()-1),a.travelerInputArray[r].passportExpDate.val());if(s>p){alert(a.childPassportMsgPreText+((r-u)+1)+a.childPassportMsgText);return true}return true}return true}else{if(a.travelerInputArray[r].paxType=="INF"){if(a.travelerInputArray[r].passportExpYear.val()!=""&&a.travelerInputArray[r].passportExpMonth.val()!=""&&a.travelerInputArray[r].passportExpDate.val()!=""){var p=new Date();p.setFullYear(a.travelerInputArray[r].passportExpYear.val(),(a.travelerInputArray[r].passportExpMonth.val()-1),a.travelerInputArray[r].passportExpDate.val());if(s>p){alert(a.infantPassportMsgPreText+((r-t)+1)+a.infantPassportMsgText);return true}return true}return true}}}return true}return true};var l=function(q){var u=0;var p=0;var t=a.findPaxType("INF",null);var s=a.findPaxType("CHD",null);if(a.travelerInputArray[q].paxType=="INF"){if(!a.travelerInputArray[q].travelerAge.isInfant(a.returnDate,null)){a.travelerInputArray[q].toggleMenuHandler();u=(q-t)+1;alert(a.infantPreText+u+a.infantText);return false}else{if(!a.travelerInputArray[q].travelerAge.isNotFutureDate()){a.travelerInputArray[q].toggleMenuHandler();u=(q-t)+1;alert(a.infantFutureBirthDateErrorPreText+u+a.infantFutureBirthDateErrorPostText);return false}}for(i=0;i<a.travelerInputArray.length;i++){if((a.travelerInputArray[i].paxType=="INF")&&(i!=q)&&(a.travelerInputArray[i].travelerCompany.val()==a.travelerInputArray[q].travelerCompany.val())){alert(a.duplicateAdultTextPre+(i-t+1)+a.duplicateAdultText);return false}}}else{if(a.travelerInputArray[q].paxType=="CHD"){var r="";var n=true;if(!a.travelerInputArray[q].travelerAge.isChild(a.returnDate,null)){a.travelerInputArray[q].toggleMenuHandler();p=(q-s)+1;alert(a.childPreText+p+a.childText);return false}}}return true};a.findPaxType=function(r,p){var n=null;var s=0;for(s=0;s<a.travelerInputArray.length;s++){var q=p!=null?a.travelerInputArray[s].paxType==r&&a.travelerInputArray[s].hasFriend==p:a.travelerInputArray[s].paxType==r;if(q==true){n=s;break}}return n};a.countPaxType=function(p){var n=0;var q=0;for(q=2;q<a.travelerInputArray.length;q++){if(a.travelerInputArray[q].paxType==p){n++}}return n};a.setSettingsByObject=function(q){f.setSettingsByObject.call(this,q);var p=0;var s=this.travelerInputArray||[];var n=null;for(p=0;p<s.length;p+=1){n=new SKYSALES.Class.TravelerInput();n.init(s[p]);s[p]=n}if(this.friendsArray!=null){p=0;var r=null;for(p=0;p<this.friendsArray.length;p+=1){r=new SKYSALES.Class.Friend();r.init(this.friendsArray[p])}}a.travelerInputArray=s};return a};SKYSALES.Class.TravelerInputContainer.createObject=function(a){SKYSALES.Util.createObject("travelerInputContainer","TravelerInputContainer",a)}}if(!SKYSALES.Class.TravelerInput){SKYSALES.Class.TravelerInput=function(){var b=SKYSALES.Class.SkySales(),a=SKYSALES.Util.extendObject(b);a.linkId="";a.link=null;a.listId="";a.list=null;a.contentId="";a.content=null;a.tabId="";a.tab=null;a.tabStart=0;a.travelerInputArray=null;a.travelerName=null;a.travelerAge=null;a.travelerCompany=null;a.selectedTab=0;a.imThisTravellerCheckboxId="";a.imThisTravellerCheckbox=null;a.paxType="";a.docType=null;a.passportNumber=null;a.passportCountry=null;a.passportExpDate=null;a.passportExpMonth=null;a.passportExpYear=null;a.dropdownTitleId="";a.dropdownGenderId="";a.dropdownTitle=null;a.dropdownGender=null;a.dropdownNationalityId="";a.dropdownNationality=null;a.dropdownBirthDateMonthId="";a.dropdownBirthDateMonth=null;a.dropdownBirthDateDayId="";a.dropdownBirthDateDay=null;a.dropdownBirthDateYearId="";a.dropdownBirthDateYear=null;a.customerNumberId="";a.customerNumber=null;a.hasFriend=false;a.setTabIndexes=function(){a.link.attr("tabindex",a.tabStart);var c=$("#"+a.contentId+" :input:visible, :select:visible, :radio:visible");for(i=0;i<c.length;i++){$(c[i]).attr("tabindex",a.tabStart+i+1);c[i].setAttribute("tabindex",a.tabStart+i+1)}};a.init=function(s){this.setSettingsByObject(s);this.setVars();this.addEvents();setTimeout(a.setTabIndexes,2000);var e=null,l=$("#"+a.contentId+" :input[id*=DropDownListBirthDateMonth]"),p=$("#"+a.contentId+" :input[id*=DropDownListBirthDateDay]"),m=$("#"+a.contentId+" :input[id*=DropDownListBirthDateYear]");e=new SKYSALES.Class.TravelerAge();e.init(l,p,m);a.travelerAge=e;if(this.paxType=="INF"){a.travelerCompany=$("#"+a.contentId+" :input[id*=DropDownListAssign]")}if($("#"+a.contentId+" :input:text[id*=TextBoxFirstName]").val()==""){$("#"+a.contentId+" :input:text[id*=TextBoxFirstName]").val($("#"+a.contentId+" :input:text[id*=TextBoxLastName]").val())}var n=null,q=$("#"+a.contentId+" :input:text[id*=TextBoxFirstName]"),r=$("#"+a.contentId+" :input:text[id*=TextBoxLastName]"),f=a.tab;if(q.length>0||r.length>0){n=new SKYSALES.Class.TravelerName();n.init(q,r,f);a.travelerName=n}var c=$("#"+a.contentId+" select[id*=DropDownListNationality]");if(c.length>0){a.populateCountryInput(c)}a.passportNumber=$("#"+a.contentId+" :input:text[id*=TextBoxDocumentNumber]");a.docType=$("#"+a.contentId+" :input:hidden[id*=DropDownListDocumentType]");if(a.passportNumber.length>0){a.passportNumber.keyup(this.addPassportHandler)}a.passportCountry=$("#"+a.contentId+" select[id*=DropDownListDocumentCountry]");if(a.passportCountry.length>0){a.populateCountryInput(a.passportCountry)}a.passportExpDate=$("#"+a.contentId+" select[id*=DropDownListDocumentDateDay]");a.passportExpMonth=$("#"+a.contentId+" select[id*=DropDownListDocumentDateMonth]");a.passportExpYear=$("#"+a.contentId+" select[id*=DropDownListDocumentDateYear]")};a.disableTextFields=function(){a.travelerName.disable()};a.populateCountryInput=function(f){var l=$("#"+f[0].id+"_value").val();var c=SKYSALES.Util.getResource().countryInfo.CountryList;var e={selectBox:f,objectArray:c,selectedItem:l,showCode:false};SKYSALES.Util.populateSelect(e)};a.setVars=function(){a.link=$("#"+this.linkId);a.list=$("#"+this.listId);a.content=$("#"+this.contentId);a.tab=$("#"+this.tabId);a.imThisTravellerCheckbox=$("#"+this.imThisTravellerCheckboxId);a.dropdownTitle=$("#"+this.dropdownTitleId);a.dropdownGender=$("#"+this.dropdownGenderId);a.dropdownBirthDateMonth=$("#"+this.dropdownBirthDateMonthId);a.dropdownBirthDateDay=$("#"+this.dropdownBirthDateDayId);a.dropdownBirthDateYear=$("#"+this.dropdownBirthDateYearId);a.dropdownNationality=$("#"+this.dropdownNationalityId);a.customerNumber=$("#"+this.customerNumberId)};a.addEvents=function(){a.link.focus(this.toggleMenuHandler);a.link.click(this.toggleMenuHandler);a.imThisTravellerCheckbox.click(this.populateTravelerTabHandler);a.dropdownTitle.change(this.updateGender);a.dropdownGender.change(this.updateTitle);a.dropdownBirthDateMonth.change(this.updateDays);a.dropdownBirthDateYear.change(this.updateDays)};a.updateGender=function(){var c=a.dropdownTitle.val();var e=SKYSALES.Util.getResource().titleInfo.TitleList;for(i=0;i<e.length;i+=1){title=e[i];if(c==title.TitleKey){a.dropdownGender.val(title.GenderCode);break}}};a.clearFriend=function(){a.customerNumber.val("");a.travelerName.firstName.val("");a.travelerName.lastName.val("");a.dropdownTitle.val("");a.travelerName.writeNameHandler();a.dropdownNationality.val("");a.dropdownGender.val("");a.dropdownBirthDateMonth.val("");a.dropdownBirthDateDay.val("");a.dropdownBirthDateYear.val("");a.passportNumber.val("");a.passportCountry.val("");a.passportExpDate.val("");a.passportExpMonth.val("");a.passportExpYear.val("");a.hasFriend=false};a.updateTitle=function(){var c=a.dropdownGender.val();var e=a.dropdownTitle.val();var f=SKYSALES.Util.getResource().titleInfo.TitleList;for(i=0;i<f.length;i+=1){title=f[i];if(e!="CHD"&&c==title.GenderCode){a.dropdownTitle.val(title.TitleKey);break}}};a.updateDays=function(){var f=a.dropdownBirthDateMonth.val()-1;var e=a.dropdownBirthDateYear.val();var c=32-new Date(e,f,32).getDate();var m=a.dropdownBirthDateDay.val();var l=a.dropdownBirthDateDay[0].length-1;if(l<c){for(i=l+1;i<=c;i+=1){a.dropdownBirthDateDay.append('<option value="'+i+'">'+i+"</option>")}}else{if(l>c){for(i=l;i>c;i-=1){$("#"+a.dropdownBirthDateDayId+" option:eq("+i+")").remove()}}}if(m!=""&&m<=c){a.dropdownBirthDateDay.val(m)}else{a.dropdownBirthDateDay.val("")}};a.addPassportHandler=function(){if(a.passportNumber!=null&&a.passportNumber.length>0){if(a.docType!=null&&a.docType.length>0){var e=a.passportNumber.val()+"";var c=e.replace(/^\s+|\s+$/g,"");if(c.length>0){a.docType.val("P")}else{a.docType.val("")}}}};a.populateTravelerTabHandler=function(){a.travelerName.writeNameHandler()};a.toggleMenuHandler=function(){if(a.travelerInputArray!==null){for(i=0;i<a.travelerInputArray.length;i+=1){a.travelerInputArray[i].hideContent()}a.showContent()}};a.showContent=function(){a.list.addClass("selected");a.content.show();a.selectedTab=0};a.hideContent=function(){a.list.removeClass("selected");a.content.hide();a.selectedTab=1};a.setSettingsByObject=function(c){b.setSettingsByObject.call(this,c)};return a};SKYSALES.Class.TravelerInput.createObject=function(a){SKYSALES.Util.createObject("travelerInput","TravelerInput",a)}}if(!SKYSALES.Class.TravelerName){SKYSALES.Class.TravelerName=function(){var a=SKYSALES.Class.SkySales(),b=SKYSALES.Util.extendObject(a);b.firstName=null;b.lastName=null;b.nameLink=null;b.init=function(e,c,f){b.firstName=e;b.lastName=c;b.nameLink=f;this.addEvents();this.writeNameHandler()};b.disable=function(){b.firstName.attr("readonly",true);b.lastName.attr("readonly",true)};b.addEvents=function(){b.firstName.change(this.writeNameHandler);b.lastName.change(this.writeNameHandler)};b.writeNameHandler=function(){var c=", ";if(b.lastName.val()===""||b.firstName.val()===""){c=""}b.nameLink.text(b.lastName.val()+c+b.firstName.val())};return b};SKYSALES.Class.TravelerName.createObject=function(a){SKYSALES.Util.createObject("travelerName","TravelerName",a)}}if(!SKYSALES.Class.TravelerAge){SKYSALES.Class.TravelerAge=function(){var a=SKYSALES.Class.SkySales(),b=SKYSALES.Util.extendObject(a);b.dateMonth=null;b.dateDay=null;b.dateYear=null;b.init=function(e,f,c){b.dateMonth=e;b.dateDay=f;b.dateYear=c};b.isInfant=function(m,l){var e=new Date(m);var f=null;if(l==null){f=new Date(parseInt(b.dateYear.val()),parseInt(b.dateMonth.val())-1,parseInt(b.dateDay.val()))}else{f=new Date(l)}if(!isNaN(f)&&!isNaN(e)){var c=b.computeAge(e,f);if(!(c<2)){return false}}return true};b.isChild=function(m,l){var e=new Date(m);var f=null;if(l==null){f=new Date(parseInt(b.dateYear.val()),parseInt(b.dateMonth.val())-1,parseInt(b.dateDay.val()))}else{f=new Date(l)}if(!isNaN(f)&&!isNaN(e)){var c=b.computeAge(e,f);if(!(c<12)){return false}}return true};b.isNotFutureDate=function(){var e=new Date(parseInt(b.dateYear.val()),parseInt(b.dateMonth.val())-1,parseInt(b.dateDay.val()));var c=new Date();if(!isNaN(e)){return(e<c)}return true};b.computeAge=function(f,e){if(f>e){var c=f.getFullYear()-e.getFullYear();if(f.getMonth()<e.getMonth()||(f.getMonth()<=e.getMonth()&&f.getDate()<e.getDate())){c--}return c}return null};return b};SKYSALES.Class.TravelerAge.createObject=function(a){SKYSALES.Util.createObject("TravelerAge","TravelerAge",a)}}if(!SKYSALES.Class.Friend){SKYSALES.Class.Friend=function(){var b=SKYSALES.Class.SkySales(),a=SKYSALES.Util.extendObject(b);a.customerNumber=null;a.firstName=null;a.lastName=null;a.bdayDay=null;a.bdayMonth=null;a.bdayYear=null;a.sequence=null;a.documentDay=null;a.documentMonth=null;a.documentYear=null;a.gender=null;a.title=null;a.documentCountry=null;a.nationality=null;a.documentNumber=null;a.paxType=null;a.init=function(c){this.setSettingsByObject(c)};return a};SKYSALES.Class.Friend.createObject=function(a){SKYSALES.Util.createObject("Friend","Friend",a)}; /*! This file is part of the Navitaire Professional Services customization. Copyright (C) Navitaire. All rights reserved. */ }if(!SKYSALES.Class.AddOnsContainer){SKYSALES.Class.AddOnsContainer=function(){var a=SKYSALES.Class.SkySales(),b=SKYSALES.Util.extendObject(a);b.menuSectionId="";b.menuSection=null;b.menuListId="";b.menuList=null;b.AddOnsArray=null;b.submitButtonId="";b.submitButton=null;b.continueButtonId="";b.continueButton=null;b.AddOnsArrayLinks=null;b.init=function(c){this.setSettingsByObject(c);this.setVars();this.addEvents();for(i=0;i<b.AddOnsArray.length;i+=1){b.AddOnsArray[i].AddOnsArray=this.AddOnsArray;b.AddOnsArray[i].hideContent()}b.AddOnsArray[0].content.show();b.AddOnsArray[0].list.addClass("selected")};b.setVars=function(){b.menuSection=$("#"+this.menuSectionId);b.menuList=$("#"+this.menuListId);b.submitButton=$("#"+this.submitButtonId);b.continueButton=$("#"+this.continueButtonId)};b.addEvents=function(){b.continueButton.click(this.continueHandler)};b.setSettingsByObject=function(f){a.setSettingsByObject.call(this,f);var e=0;var l=this.AddOnsArray||[];var c=null;for(e=0;e<l.length;e+=1){c=new SKYSALES.Class.AddOns();l[e].linkId=this.AddOnsArrayLinks[e].linkId;l[e].contentId=this.AddOnsArrayLinks[e].contentId;l[e].listId=this.AddOnsArrayLinks[e].listId;c.init(l[e],e);l[e]=c}this.AddOnsArray=l};b.continueHandler=function(){var c=0;AddOnsArray=b.AddOnsArray||[];for(i=0;i<AddOnsArray.length;i+=1){if(AddOnsArray[i].list.hasClass("selected")){c=i;break}}if((c+1)==AddOnsArray.length){if(b.validateAddOns()){document.getElementById(b.submitButtonId).click()}}else{b.AddOnsArray[c].triggerValidate();if(!b.AddOnsArray[c].isValidated()){b.AddOnsArray[c].toggleMenuHandler();return false}b.AddOnsArray[c+1].toggleMenuHandler()}};b.isValidated=function(){var e=0;var f=this.AddOnsArray||[];var c=null;var l=true;for(e=0;e<f.length;e+=1){if(!f[e].isValidated()){l=false}}return l};b.validateAddOns=function(){var e=0;var f=this.AddOnsArray||[];var c=null;for(e=0;e<f.length;e+=1){f[e].triggerValidate();if(!f[e].isValidated()){f[e].toggleMenuHandler();return false}}return true};return b};SKYSALES.Class.AddOnsContainer.createObject=function(a){SKYSALES.Util.createObject("AddOnsContainer","AddOnsContainer",a)}}if(!SKYSALES.Class.AddOns){SKYSALES.Class.AddOns=function(){var b=SKYSALES.Class.SkySales(),a=SKYSALES.Util.extendObject(b);a.linkLabel="";a.linkId="";a.link=null;a.listId="";a.list=null;a.contentId="";a.content=null;a.AddOnsArray=null;a.validateButtonId="";a.validateButton=null;a.validateValueId="";a.init=function(c){this.setSettingsByObject(c);this.setVars();this.addEvents();a.link.html(a.linkLabel)};a.setVars=function(){b.setVars.call(this);a.link=$("#"+this.linkId);a.list=$("#"+this.listId);a.content=$("#"+this.contentId);a.validateButtonId=this.validateButtonId;a.validateValueId=this.validateValueId;if(this.validateButtonId!=""){a.validateButton=$("#"+this.validateButtonId)}};a.addEvents=function(){b.addEvents.call(this);a.link.click(this.toggleMenuHandler)};a.toggleMenuHandler=function(){for(i=0;i<a.AddOnsArray.length;i+=1){a.AddOnsArray[i].hideContent()}a.list.addClass("selected");a.content.show()};a.triggerValidate=function(){if(this.validateButtonId!=""&&$("#"+this.validateButtonId).length>0){document.getElementById(this.validateButtonId).click()}};a.isValidated=function(){if(a.validateValueId!=""&&$("#"+a.validateValueId).length>0){if($("#"+a.validateValueId).val()=="false"){return false}else{return true}}return true};a.hideContent=function(){a.list.removeClass("selected");a.content.hide()};a.setSettingsByObject=function(c){b.setSettingsByObject.call(this,c)};return a};SKYSALES.Class.AddOns.createObject=function(a){SKYSALES.Util.createObject("AddOns","AddOns",a)}}function confirmEmail(e,a,c){var b;if(c==true){b=confirm(e)}else{b=true}if(b==true){showMsg=false;__doPostBack(a,"");return true}}SKYSALES.Class.MealLegInput=function(){var b=new SKYSALES.Class.SkySales(),a=SKYSALES.Util.extendObject(b);a.clientId="";a.ssrAvailabilityArray=null;a.dropDownMealArray=[];a.currencyCode="";a.complimentaryTextArray=null;a.noMealText="";a.isFlightChange=false;a.disableMarket=null;a.init=function(c){this.setSettingsByObject(c);this.setVars();this.addEvents();this.initSsrAvailabilityArray();this.initDropDownMealLegInputArray()};a.initSsrAvailabilityArray=function(){var f=0,l=this.ssrAvailabilityArray||[],c=l.length,e=null;for(f=0;f<c;f+=1){e=new SKYSALES.Class.SSRAvailability();e.init(l[f]);this.ssrAvailabilityArray[f]=e}};a.initDropDownMealLegInputArray=function(){var f=0,e=this.dropDownMealArray||[],c=e.length,l=null;for(f=0;f<c;f+=1){l=new SKYSALES.Class.DropDownMealLegInput();l.complimentaryTextArray=this.complimentaryTextArray;l.noMealText=this.noMealText;l.isFlightChange=this.isFlightChange;if(this.disableMarket!="none"&&f<parseInt(this.disableMarket)){l.isDisabled=true}l.init(e[f],this.ssrAvailabilityArray[0],this.currencyCode);this.dropDownMealArray[f]=l}};return a};SKYSALES.Class.MealLegInput.createObject=function(a){SKYSALES.Util.createObject("mealLegInput","MealLegInput",a)};SKYSALES.Class.DropDownMealLegInput=function(){var b=new SKYSALES.Class.SkySales(),a=SKYSALES.Util.extendObject(b);a.dropDownMealId="";a.dropDownMeal={};a.quantityPrefix="";a.del="";a.passengerNumber=0;a.flightReference="";a.selectedItem="";a.currencyCode="";a.hiFlyerMealText=null;a.ssrAvailability=null;a.isNewBooking=true;a.complimentaryTextArray=null;a.noMealText="";a.noMealTextHtml=null;a.isFlightChange=false;a.isDisabled=false;a.origDropDownId="";a.mealPriceArray=null;var c=false;a.init=function(l,f,e){this.setSettingsByObject(l);this.setVars();this.addEvents();this.currencyCode=e;if(this.selectedItem!=""){this.isNewBooking=false}this.initDropDownMealLegInput(f);if(this.dropDownMeal){this.dropDownMeal.val(this.selectedItem).change()}if(this.isDisabled){this.dropDownMeal.attr("disabled",true);$("#"+this.origDropDownId).val(this.selectedItem)}};a.setVars=function(){this.hiFlyerMealText=$("#"+this.dropDownMealId+"_hiflyerMealText");this.noMealTextHtml=$("#"+this.dropDownMealId+"_noMealText");if(this.isDisabled){this.origDropDownId=this.dropDownMealId;this.dropDownMealId=this.dropDownMealId+"_"}this.dropDownMeal=this.getById(this.dropDownMealId)};a.addEvents=function(){a.dropDownMeal.change(a.updateMealText)};a.updateMealText=function(){var e=$("#"+a.dropDownMealId).val();a.prevSelected=null;if(a.processSsrs(false,e)){a.hiFlyerMealText.show()}else{a.hiFlyerMealText.hide()}};a.showLabel=function(l,f){var e=0;for(e=0;e<this.complimentaryTextArray.length;e++){if(this.complimentaryTextArray[e].SsrCode==l&&(this.prevSelected==null||e>this.prevSelected)){this.prevSelected=e;this.hiFlyerMealText.text(this.complimentaryTextArray[e].labelText);this.selectedItem=f;break}}};a.initDropDownMealLegInput=function(e){this.ssrAvailability=e;a.processSsrs(true,null)};a.processSsrs=function(q,p){ssrAvailability=this.ssrAvailability;len1=ssrAvailability.SsrAvailabilityList.length;SSRAvailability=null;var n=0;for(n=0;n<len1;n+=1){SSRAvailability=ssrAvailability.SsrAvailabilityList[n];if(SSRAvailability.passengerNumber==this.passengerNumber){len2=SSRAvailability.flightParts.length;flightPart=null;for(j=0;j<len2;j+=1){c=false;flightPart=new SKYSALES.Class.FlightPart();flightPart.init(SSRAvailability.flightParts[j]);if(flightPart.flightKey!=null){flightDesignator="";if(flightPart.flightKey.flightDesignator!=null){flightDesignator=flightPart.flightKey.flightDesignator.CarrierCode+flightPart.flightKey.flightDesignator.FlightNumber;flightDesignator=flightDesignator.replace(/\s/g,"-")}flightRef=flightPart.flightKey.departureDate.getDate()+"-"+flightDesignator+"-"+flightPart.flightKey.departureStation+flightPart.flightKey.arrivalStation;if(this.flightReference==flightRef){len3=flightPart.availableSsrs.length;for(k=0;k<len3;k+=1){value=this.quantityPrefix+this.del+"passengerNumber"+this.del+this.passengerNumber+this.del+"ssrCode"+this.del+flightPart.availableSsrs[k].ssr.ssrCode+this.del+"flightReference"+this.del+flightRef;if(q){if(this.isFlightChange&&this.selectedItem!=""){c=(value==this.selectedItem)?true:c;if(!c&&!(k+1<len3)){this.selectedItem=""}}a.outputOptions(flightPart.availableSsrs[k],value)}else{if(value==p&&flightPart.availableSsrs[k].price==0){this.showLabel(flightPart.availableSsrs[k].ssr.ssrCode,value);return true}}}selectBoxObj=this.dropDownMeal.get(0);if(selectBoxObj){var r="";if(!this.isNewBooking&&this.selectedItem!=""){for(var e=0,f=2;e<f;e++){r=selectBoxObj.options[e];if(r!==null&&r!==undefined&&e==0&&r.value==""){selectBoxObj.options[0]=null;break}}}}}}}}}if(this.dropDownMeal.get(0)!=null&&this.dropDownMeal.get(0).options.length==1){this.noMealTextHtml.text(this.noMealText)}};a.outputOptions=function(e,f){if(this.dropDownMeal){text=e.ssr.name;price=a.findMCCPrice(e.ssr.ssrCode);text=text+" ("+price+")";if(e.price==0&&(this.isNewBooking||this.isFlightChange&&this.selectedItem=="")){this.showLabel(e.ssr.ssrCode,f)}selectBoxObj=this.dropDownMeal.get(0);if(selectBoxObj){selectBoxObj.options[selectBoxObj.options.length]=new window.Option(text,f,false,false)}}};a.findMCCPrice=function(f){var e=0;if(this.mealPriceArray!=null){for(e=0;e<this.mealPriceArray.length;e++){if(this.mealPriceArray[e].SsrCode==f){return this.mealPriceArray[e].SsrPrice}}}return""};return a};SKYSALES.Class.BaggageLegInput=function(){var a=new SKYSALES.Class.SkySales(),b=SKYSALES.Util.extendObject(a);b.clientId="";b.ssrAvailabilityArray=null;b.dropDownBaggageArray=[];b.currencyCode="";b.complimentaryTextArray=null;b.isFlightChange=false;b.disableMarket=null;b.isNewBookingModified=null;b.isBookingModified=null;b.isAgent=null;b.init=function(c){this.setSettingsByObject(c);this.setVars();this.addEvents();this.initSsrAvailabilityArray();this.initDropDownBaggageLegInputArray()};b.initSsrAvailabilityArray=function(){var f=0,l=this.ssrAvailabilityArray||[],c=l.length,e=null;for(f=0;f<c;f+=1){e=new SKYSALES.Class.SSRAvailability();e.init(l[f]);this.ssrAvailabilityArray[f]=e}};b.initDropDownBaggageLegInputArray=function(){var e=0,l=this.dropDownBaggageArray||[],c=l.length,f=null;for(e=0;e<c;e+=1){f=new SKYSALES.Class.DropDownBaggageLegInput();f.complimentaryTextArray=this.complimentaryTextArray;if(this.disableMarket!="none"&&e<parseInt(this.disableMarket)){f.isDisabled=true}f.isFlightChange=this.isFlightChange;f.isNewBookingModified=this.isNewBookingModified;f.isBookingModified=this.isBookingModified;f.isAgent=this.isAgent;f.init(l[e],this.ssrAvailabilityArray[0],this.currencyCode);this.dropDownBaggageArray[e]=f}};return b};SKYSALES.Class.BaggageLegInput.createObject=function(a){SKYSALES.Util.createObject("baggageLegInput","BaggageLegInput",a)};SKYSALES.Class.DropDownBaggageLegInput=function(){var b=new SKYSALES.Class.SkySales(),a=SKYSALES.Util.extendObject(b);a.dropDownBaggageId="";a.dropDownBaggage={};a.quantityPrefix="";a.del="";a.passengerNumber=0;a.flightReference="";a.selectedItem="";a.currencyCode="";a.hiFlyerBaggageText="";a.isNewBooking=true;a.complimentaryTextArray=null;a.prevSelected=null;a.isFlightChange=false;a.isDisabled=false;a.origDropDownId="";a.baggagePriceArray=null;a.autoSelectBaggage=function(){if(this.selectedItem!=""){var s=a.dropDownBaggageId;var B=this.options[this.selectedIndex].text;var w=this.options[this.selectedIndex].value;var q=a.flightReference;var H=document.getElementsByTagName("select");var e=[];if((w.length>0||B.length>0)&&q.length>0){var p=w.substring(w.indexOf("ssrCode"),w.indexOf("flightReference")-1);var f=s.substring(s.indexOf("passengerNumber"),s.indexOf("flightReference")-1);for(var D=0,A=H.length;D<A;D++){if(!H[D].disabled){if(H[D].id&&H[D].id!=s&&H[D].id.indexOf(f)!=-1&&(H[D].id.indexOf("BaggageLegInputViewTravelerView")!=-1||H[D].id.indexOf("BaggageLegInputViewTravelerChangeView")!=-1||H[D].id.indexOf("BaggageLegInputViewTravelerFlightChangeView")!=-1)){e.push(H[D])}}}if(e){for(var v=0,t=e.length;v<t;v++){var n=e[v].id;var F=q.substring(q.lastIndexOf("-")+1,q.length);var E=n.substring(n.lastIndexOf("-")+1,n.length);var u=F.substring(0,2);var r=F.substring(F.length-1,F.length-3);var c=E.substring(0,2);var G=E.substring(E.length-1,E.length-3);if(!(u==G&&c==r)&&(r==c||u==G)){var z=document.getElementById(n);if(p.length>0){for(var D=0,C=z.options.length;D<C;D++){o=z.options[D];if(o.value.indexOf(p)!=-1){o.selected=true}}}else{if(B.length>0){for(var D=0,C=z.options.length;D<C;D++){o=z.options[D];if(o.text.indexOf(B)!=-1){o.selected=true}}}}}}}}}};a.addEvents=function(){a.dropDownBaggage.change(this.autoSelectBaggage)};a.init=function(f,e,c){this.setSettingsByObject(f);this.setVars();this.addEvents();this.currencyCode=c;if(this.selectedItem!=""&&!this.isFlightChange){a.isNewBooking=false}this.initDropDownBaggageLegInput(e);if(this.dropDownBaggage){this.dropDownBaggage.val(this.selectedItem)}if(this.isDisabled){this.dropDownBaggage.attr("disabled",true);$("#"+this.origDropDownId).val(this.selectedItem)}};a.setVars=function(){this.hiFlyerBaggageText=$("#"+this.dropDownBaggageId+"_hiflyerBaggageText");if(this.isDisabled){this.origDropDownId=this.dropDownBaggageId;this.dropDownBaggageId=this.dropDownBaggageId+"_"}this.dropDownBaggage=$("#"+this.dropDownBaggageId)};a.showLabel=function(f,e){var c=0;for(c=0;c<this.complimentaryTextArray.length;c++){if(this.complimentaryTextArray[c].SsrCode==f&&(this.prevSelected==null||c>this.prevSelected)){this.prevSelected=c;this.hiFlyerBaggageText.text(this.complimentaryTextArray[c].labelText);if((this.isNewBooking||this.isFlightChange)&&!this.isDisabled){this.selectedItem=e}break}}};a.findMCCPrice=function(e){var c=0;if(this.baggagePriceArray!=null){for(c=0;c<this.baggagePriceArray.length;c++){if(this.baggagePriceArray[c].SsrCode==e){return this.baggagePriceArray[c].SsrPrice}}}return""};a.initDropDownBaggageLegInput=function(f){len1=f.SsrAvailabilityList.length;SSRAvailability=null;flightCarrierCode=null;var n=0;for(n=0;n<len1;n+=1){SSRAvailability=f.SsrAvailabilityList[n];if(SSRAvailability.passengerNumber==this.passengerNumber){len2=SSRAvailability.flightParts.length;flightPart=null;for(j=0;j<len2;j+=1){flightPart=new SKYSALES.Class.FlightPart();flightPart.init(SSRAvailability.flightParts[j]);if(flightPart.flightKey!=null){flightDesignator="";if(flightPart.flightKey.flightDesignator!=null){flightCarrierCode=flightPart.flightKey.flightDesignator.CarrierCode;flightDesignator=flightPart.flightKey.flightDesignator.CarrierCode+flightPart.flightKey.flightDesignator.FlightNumber;flightDesignator=flightDesignator.replace(" ","-");flightDesignator=flightDesignator.replace(" ","-")}flightRef=flightPart.flightKey.departureDate.getDate()+"-"+flightDesignator+"-"+flightPart.flightKey.departureStation+flightPart.flightKey.arrivalStation;if(this.flightReference==flightRef){len3=flightPart.availableSsrs.length;origSelectedItem=this.selectedItem;hasComplimentaryBaggage=false;for(k=0;k<len3;k+=1){if(this.dropDownBaggage){text=flightPart.availableSsrs[k].ssr.name;price=a.findMCCPrice(flightPart.availableSsrs[k].ssr.ssrCode);text=text+" ("+price+")";value=this.quantityPrefix+this.del+"passengerNumber"+this.del+this.passengerNumber+this.del+"ssrCode"+this.del+flightPart.availableSsrs[k].ssr.ssrCode+this.del+"flightReference"+this.del+flightRef;if(flightPart.availableSsrs[k].price==0){this.showLabel(flightPart.availableSsrs[k].ssr.ssrCode,value);hasComplimentaryBaggage=true}else{if((hasComplimentaryBaggage)&&(value==origSelectedItem)&&(flightPart.availableSsrs[k].price>0)){this.selectedItem=value}else{if((this.isNewBooking==true)&&(value==origSelectedItem)){this.selectedItem=value}else{if((this.isNewBooking==true)&&(this.isNewBookingModified=="")&&(this.isBookingModified=="")&&(this.isAgent=="false")){if((flightCarrierCode=="AK")||(flightCarrierCode=="FD")||(flightCarrierCode=="QZ")){if(flightPart.availableSsrs[k].ssr.ssrCode=="PBAB"){this.selectedItem=value}}else{if(flightCarrierCode=="D7"){if(flightPart.availableSsrs[k].ssr.ssrCode=="PBAB"){this.selectedItem=value}}}}}}}selectBoxObj=this.dropDownBaggage.get(0);if(selectBoxObj){selectBoxObj.options[selectBoxObj.options.length]=new window.Option(text,value,false,false)}}}if(selectBoxObj){if(a.isBookingModified=="True"&&this.selectedItem!=""){if(this.selectedItem.indexOf("__")==-1){selectBoxObj=this.dropDownBaggage.get(0);if(selectBoxObj){for(var c=0,e=2;c<e;c++){o=selectBoxObj.options[c];if(c==0&&o.value==""){selectBoxObj.options[0]=null;break}}}}}}}}}}}};return a};SKYSALES.Class.SSRAvailability=function(){var a=new SKYSALES.Class.SkySales(),b=SKYSALES.Util.extendObject(a);b.SsrAvailabilityList=[];b.init=function(c){this.setSettingsByObject(c);this.setVars();this.addEvents();this.initSsrAvailabilityList()};b.initSsrAvailabilityList=function(){var l=0,f=this.SsrAvailabilityList||[],c=f.length,e=null;for(l=0;l<c;l+=1){e=new SKYSALES.Class.SsrAvailabilityPassenger();e.init(f[l]);this.SsrAvailabilityList[l]=e}};return b};SKYSALES.Class.SsrAvailabilityPassenger=function(){var b=new SKYSALES.Class.SkySales(),a=SKYSALES.Util.extendObject(b);a.passengerNumber=0;a.flightParts=[];a.init=function(c){this.setSettingsByObject(c);this.setVars();this.addEvents();this.initFlightPartsArray()};a.initFlightPartsArray=function(){var e=0,l=this.flightParts||[],c=l.length,f=null;for(e=0;e<c;e+=1){f=new SKYSALES.Class.FlightPart();f.init(l[e]);this.flightParts[e]=f}};return a};SKYSALES.Class.FlightPart=function(){var b=new SKYSALES.Class.SkySales(),a=SKYSALES.Util.extendObject(b);a.flightKey=null;a.availableSsrs=[];a.init=function(c){this.setSettingsByObject(c);this.setVars();this.addEvents();this.initFlightKey();this.initAvailableSsrsArray()};a.initFlightKey=function(){var c=new SKYSALES.Class.FlightKey();c.init(this.flightKey);this.flightKey=c};a.initAvailableSsrsArray=function(){var f=0,e=this.availableSsrs||[],c=e.length,l=null;for(f=0;f<c;f+=1){l=new SKYSALES.Class.AvailableSsr();l.init(e[f]);this.availableSsrs[f]=l}};return a};SKYSALES.Class.FlightKey=function(){var b=new SKYSALES.Class.SkySales(),a=SKYSALES.Util.extendObject(b);a.arrivalStation="";a.departureDate="";a.departureStation="";a.flightDesignator=null;a.init=function(c){this.setSettingsByObject(c);this.setVars();this.addEvents();this.initDepartureDate();this.initFlightDesignator()};a.initDepartureDate=function(){var c=SKYSALES.Class.Date();c.init(this.departureDate);this.departureDate=c};a.initFlightDesignator=function(){var c=SKYSALES.Class.FlightDesignator();c.init(this.flightDesignator);this.flightDesignator=c};return a};SKYSALES.Class.FlightDesignator=function(){var b=new SKYSALES.Class.SkySales(),a=SKYSALES.Util.extendObject(b);a.CarrierCode="";a.FlightNumber="";a.init=function(c){this.setSettingsByObject(c);this.setVars();this.addEvents()};return a};SKYSALES.Class.AvailableSsr=function(){var b=new SKYSALES.Class.SkySales(),a=SKYSALES.Util.extendObject(b);a.price="";a.ssr=null;a.init=function(c){this.setSettingsByObject(c);this.setVars();this.addEvents();this.initSSR()};a.initSSR=function(){var c=SKYSALES.Class.SSR();c.init(this.ssr);this.ssr=c};return a};SKYSALES.Class.SSR=function(){var a=new SKYSALES.Class.SkySales(),b=SKYSALES.Util.extendObject(a);b.name="";b.ssrCode="";b.init=function(c){this.setSettingsByObject(c);this.setVars();this.addEvents()};return b};if(SKYSALES.Class.InsuranceInput===undefined){SKYSALES.Class.InsuranceInput=function(){var a=new SKYSALES.Class.SkySales();var b=SKYSALES.Util.extendObject(a);b.clientId="";b.insuranceAcceptInputId="";b.insuranceAcceptInput=null;b.insuranceAcceptMessage="";b.yesInsuranceInputId="";b.yesInsuranceInput=null;b.submitButtonId="";b.submitButton=null;b.linkButtonDeclineId="";b.linkButtonDecline=null;b.continueButtonId="";b.continueButton=null;b.validateValueId="";b.confirmCancelNote="";b.noAUSInsuranceInputId="";b.noAUSInsuranceInput=null;b.departCountryCode="";b.AUSNoCheckedAlert="";b.confirmCancelNoteOptOut="";b.insuranceAcceptMessageOptOut="";b.setSettingsByObject=function(c){a.setSettingsByObject.call(this,c);var e="";for(e in c){if(b.hasOwnProperty(e)){b[e]=c[e]}}};b.addEvents=function(){a.addEvents.call(this);if($("#"+b.yesInsuranceInputId).length>0){b.yesInsuranceInput.click(b.clickInsuranceInput)}if($("#"+b.insuranceAcceptInputId).length>0){if(b.departCountryCode=="AU"||b.departCountryCode=="NZ"){b.insuranceAcceptInput.click(b.clickYesInsuranceInputAUS)}b.continueButton.click(b.validateContinue)}if($("#"+b.linkButtonDeclineId).length>0){b.linkButtonDecline.click(b.confirmCancelInsuranceHandler)}if($("#"+b.noAUSInsuranceInputId).length>0){b.noAUSInsuranceInput.click(b.clickNoInsuranceInputAUS);b.continueButton.click(b.validateCancel)}};b.setVars=function(){a.setVars.call(this);if($("#"+b.insuranceAcceptInputId).length>0){b.insuranceAcceptInput=$("#"+b.insuranceAcceptInputId)}if($("#"+b.yesInsuranceInputId).length>0){b.yesInsuranceInput=$("#"+b.yesInsuranceInputId)}if($("#"+b.submitButtonId).length>0){b.submitButton=$("#"+b.submitButtonId)}if($("#"+b.continueButtonId).length>0){b.continueButton=$("#"+b.continueButtonId)}if($("#"+b.linkButtonDeclineId).length>0){b.linkButtonDecline=$("#"+b.linkButtonDeclineId)}if($("#"+b.noAUSInsuranceInputId).length>0){b.noAUSInsuranceInput=$("#"+b.noAUSInsuranceInputId)}};b.init=function(c){this.setSettingsByObject(c);this.setVars();this.addEvents()};b.confirmCancelInsuranceHandler=function(){if(b.departCountryCode=="AU"||b.departCountryCode=="NZ"){if(b.noAUSInsuranceInput.attr("checked")){if(window.confirm(b.confirmCancelNoteOptOut)){__doPostBack("CONTROLGROUPADDONS$InsuranceInputAddOnsView$LinkButtonInsuranceDecline","")}else{b.insuranceAcceptInput.attr("checked",true);b.noAUSInsuranceInput.attr("checked",false);return false}}}else{if(window.confirm(b.confirmCancelNote)){return false}}};b.clickInsuranceInput=function(){document.getElementById(b.submitButtonId).click()};b.validateCancel=function(){if(b.departCountryCode=="AU"||b.departCountryCode=="NZ"){if(b.noAUSInsuranceInput.attr("checked")){$("#"+b.validateValueId).val("false");$("#"+b.linkButtonDeclineId).click()}}};b.validateContinue=function(){if(b.departCountryCode=="AU"||b.departCountryCode=="NZ"){if(b.insuranceAcceptInput.attr("checked")){$("#"+b.validateValueId).val("true")}else{if(b.noAUSInsuranceInput.attr("checked")){}else{$("#"+b.validateValueId).val("false");alert(b.insuranceAcceptMessageOptOut)}}}else{if(b.insuranceAcceptInput.attr("checked")){$("#"+b.validateValueId).val("true")}else{$("#"+b.validateValueId).val("false");alert(b.insuranceAcceptMessage)}}};b.clickYesInsuranceInputAUS=function(){var f=b.insuranceAcceptInput!=null?b.insuranceAcceptInput.attr("checked"):true;var e=b.noAUSInsuranceInput!=null?b.noAUSInsuranceInput.attr("checked"):true;if(f&&e){var c=b.noAUSInsuranceInput==undefined?b.insuranceAcceptInput:b.noAUSInsuranceInput;c.attr("checked",false)}};b.clickNoInsuranceInputAUS=function(){if(b.insuranceAcceptInput.attr("checked")&&b.noAUSInsuranceInput.attr("checked")){b.insuranceAcceptInput.attr("checked",false)}};return b};SKYSALES.Class.InsuranceInput.createObject=function(a){SKYSALES.Util.createObject("insuranceInput","InsuranceInput",a)}}SKYSALES.fareRuleHandler=function(f,a){if(f.length>0){var l="|";var m="~";var b="^";var e="";var c="";flightInfos=f[0].split(l);if(flightInfos[1].indexOf("^")!=-1){carrierInfos=flightInfos[1].split(b);carrierKey1=carrierInfos[0].split(m);carrierKey2=carrierInfos[1].split(m);e=carrierKey1[0];c=carrierKey2[0];if(e!=c&&(e=="D7"||c=="D7")){e="Connecting"}}else{flightKeys=flightInfos[1].split(m);e=flightKeys[0]}if($("#CheckBoxAgreementLabel").length>0){$("#CheckBoxAgreementLabel").hide()}if($("#CheckBoxAgreementLabelAK").length>0){$("#CheckBoxAgreementLabelAK").hide()}if($("#CheckBoxAgreementLabelFD").length>0){$("#CheckBoxAgreementLabelFD").hide()}if($("#CheckBoxAgreementLabelQZ").length>0){$("#CheckBoxAgreementLabelQZ").hide()}if($("#CheckBoxAgreementLabelD7").length>0){$("#CheckBoxAgreementLabelD7").hide()}if($("#CheckBoxAgreementLabelConnecting").length>0){$("#CheckBoxAgreementLabelConnecting").hide()}if($("#CheckBoxAgreementLabel"+e).length>0){$("#CheckBoxAgreementLabel"+e).css("display","inline")}else{if($("#CheckBoxAgreementLabel").length>0){$("#CheckBoxAgreementLabel").css("display","inline")}}}else{$("#agreementInput").hide();$("#ControlGroupSelectView_ButtonSubmit").hide()}};SKYSALES.Class.PaxSSRDisplayAtaGlance=function(){var a=new SKYSALES.Class.SkySales();var b=SKYSALES.Util.extendObject(a);b.paxNameDisplayId="";b.ssrListDisplayId="";b.paxNameDisplayObj=null;b.ssrListDisplayObj=null;b.init=function(c){this.setSettingsByObject(c);this.setVars();this.addEvents()};b.setVars=function(){b.paxNameDisplayObj=$("#"+this.paxNameDisplayId);b.ssrListDisplayObj=$("#"+this.ssrListDisplayId)};b.addEvents=function(){b.paxNameDisplayObj.click(this.hideShowSSRListHandler)};b.hideShowSSRListHandler=function(){if(b.paxNameDisplayObj.hasClass("expand_iconSml")){b.paxNameDisplayObj.removeClass("expand_iconSml");b.paxNameDisplayObj.addClass("collapse_iconSml")}else{b.paxNameDisplayObj.removeClass("collapse_iconSml");b.paxNameDisplayObj.addClass("expand_iconSml")}b.ssrListDisplayObj.slideToggle()};return b};SKYSALES.Class.PaxSSRDisplayAtaGlance.createObject=function(a){SKYSALES.Util.createObject("paxSSRDisplayAtaGlance","PaxSSRDisplayAtaGlance",a)};function populatePassenger(c,b,a){if(contactData){var l=b-1;var f=$("#passengerInputContainer"+l);var m="";if(contactData.firstName!=""){m=contactData.firstName}else{m=contactData.lastName}var e=[{name:"DropDownListTitle",value:contactData.title},{name:"TextBoxFirstName",value:m},{name:"TextBoxMiddleName",value:contactData.middleName},{name:"TextBoxLastName",value:contactData.lastName},{name:"TextBoxCustomerNumber",value:contactData.ff},{name:"DropDownListBirthDateDay",value:contactData.bdayDay},{name:"DropDownListBirthDateMonth",value:contactData.bdayMonth},{name:"DropDownListBirthDateYear",value:contactData.bdayYear},{name:"DropDownListGender",value:contactData.gender},{name:"DropDownListNationality",value:contactData.nationality},{name:"DropDownListResidentCountry",value:contactData.countryOfResidence}]}if($("#"+c.id+":checked").val()==null&&a!="False"){$.map(e,function(n){if(n){$(":input[@id*="+n.name+"]",f).val("")}})}else{if(a!="False"){$.map(e,function(n){if(n){$(":input[@id*="+n.name+"]",f).val(n.value)}})}else{if($("#"+c.id+":checked").val()==null&&a=="False"){$.map(e,function(n){if(n){if((n.name!="TextBoxFirstName")&&(n.name!="TextBoxLastName")){$(":input[@id*="+n.name+"]",f).val("")}}})}else{if(a=="False"){$.map(e,function(n){if(n){if((n.name!="TextBoxFirstName")&&(n.name!="TextBoxLastName")){$(":input[@id*="+n.name+"]",f).val(n.value)}}})}}}}}if(!SKYSALES.Class.ControlGroupBookingRetrieve){SKYSALES.Class.ControlGroupBookingRetrieve=function(){var b=new SKYSALES.Class.ControlGroup();var a=SKYSALES.Util.extendObject(b);a.bookingRetrieve=null;a.init=function(c){this.setSettingsByObject(c);var e=new SKYSALES.Class.BookingRetrieve();e.init(c);a.bookingRetrieve=e;this.setVars();this.addEvents()};a.validateHandler=function(){var c=a.validate();return c};a.validate=function(){var e=false;var c=this.bookingRetrieve;e=c.isOneSectionPopulated();if(e){e=b.validate.call(this)}return e};return a}}if(!SKYSALES.Class.BookingRetrieve){SKYSALES.Class.BookingRetrieve=function(){var b=new SKYSALES.Class.FlightSearch();var a=SKYSALES.Util.extendObject(b);a.optionHeaderText=[];a.marketArray=[];a.missingInformation="";a.sectionValidation={};a.init=function(c){this.setSettingsByObject(c);this.setVars();this.addEvents()};a.setVars=function(){b.setVars.call(this);var f=0;var c=0;var m=this.sectionValidation;var e=[];var n="";var l=null;for(n in m){if(m.hasOwnProperty(n)){e=m[n]||[];c=e.length;for(f=0;f<c;f+=1){l=e[f];l.input=this.getById(l.id)}}}};a.isOneSectionPopulated=function(){var m=0;var n=0;var t=this.sectionValidation;var q=[];var c="";var p=null;var r=null;var f=false;var s="";var l="";var e=true;for(c in t){if(t.hasOwnProperty(c)){q=t[c]||[];n=q.length;e=true;for(m=0;m<n;m+=1){p=q[m];r=p.input.get(0);if(r){s=r.value;l=s.requiredempty||"";if(s===l){s=""}if(!s){e=false;break}}}if(e){f=true;break}}}if(!f){alert(this.missingInformation)}return f};return a}}if(!SKYSALES.Class.MCCSearchInput){SKYSALES.Class.MCCSearchInput=function(){var b=new SKYSALES.Class.SkySales();var a=SKYSALES.Util.extendObject(b);a.clientId="";a.contentId="";a.currencyArray=[];a.currencyInfoArray=[];a.MCCArray=[];a.dropDownCurrencyId="";a.selectedMCC="";a.resetPaymentsText="";a.hasUncommittedPayment="";a.applyButtonId="";a.dropDownOrigin=null;a.dropDownCurrency=null;a.applyButton=null;a.stationHash=SKYSALES.Util.getResource().stationHash;a.setVars=function(){b.setVars.call(this);a.dropDownOrigin=$("#"+this.contentId+" :input[id*=TextBoxMarketOrigin1]");a.dropDownCurrency=$("#"+this.dropDownCurrencyId);a.applyButton=$("#"+this.applyButtonId)||{}};a.init=function(c){this.setSettingsByObject(c);this.setVars();this.addEvents();this.initializeCurrency()};a.addEvents=function(){this.dropDownOrigin.change(this.updateCurrencyHandler);this.applyButton.click(this.applyCurrencyHandler)};a.initializeCurrency=function(){this.updateCurrency()};a.getCurrencyDescription=function(e){var c=0;for(c in this.currencyInfoArray){if(this.currencyInfoArray[c].currencyCode===e){return this.currencyInfoArray[c].description}}};a.applyCurrencyHandler=function(){a.applyCurrency()};a.applyCurrency=function(){if(this.hasUncommittedPayment==="true"){return confirm(this.resetPaymentsText)}};a.updateCurrencyHandler=function(){a.updateCurrency()};a.updateCurrency=function(){var f="",p=0;var e={};if(this.dropDownOrigin.length>0){f=this.dropDownOrigin.val();e=this.stationHash[f]}else{f=$("#MCCOriginCountry").val();e.countryCode=f}if(f!==""&&f!==undefined){var l=this.currencyArray,c=0;var m=[];for(p in l){if(l[p].countryCode===e.countryCode){var n={};n.name=this.getCurrencyDescription(l[p].currencyCode);n.code=l[p].currencyCode;m.push(n);if(this.MCCArray&&this.MCCArray.length>0){for(c in this.MCCArray){if(this.MCCArray[c].buy===l[p].currencyCode){n={};n.name=this.getCurrencyDescription(this.MCCArray[c].sell);n.code=this.MCCArray[c].sell;m.push(n)}}}selectParamObj={selectBox:this.dropDownCurrency,objectArray:m,showCode:true};SKYSALES.Util.populateSelect(selectParamObj);if(this.selectedMCC!==""){this.dropDownCurrency.val(this.selectedMCC)}else{this.dropDownCurrency.val(l[p].currencyCode)}break}}if(m.length===0){selectParamObj={selectBox:this.dropDownCurrency,objectArray:[],showCode:true};SKYSALES.Util.populateSelect(selectParamObj)}}};return a};SKYSALES.Class.MCCSearchInput.createObject=function(a){SKYSALES.Util.createObject("mCCSearchInput","MCCSearchInput",a)}}if(!SKYSALES.Class.AdvancedBookingListSearchInput){SKYSALES.Class.AdvancedBookingListSearchInput=function(){var b=new SKYSALES.Class.SkySales();var a=SKYSALES.Util.extendObject(b);a.HyperLinkIdArray={};a.ReportLinkHash={};a.AttachPromptText="";a.ReportPromptText="";a.init=function(c){this.setSettingsByObject(c);this.setVars();this.addEvents()};a.setVars=function(){b.setVars.call(this)};a.addEvents=function(){var c="";for(c in this.HyperLinkIdArray){if(this.HyperLinkIdArray.hasOwnProperty(c)){if(this.HyperLinkIdArray[c].hasOwnProperty("attachId")){$("#"+this.HyperLinkIdArray[c].attachId).click(this.ShowAttachPrompterHandler)}if(this.HyperLinkIdArray[c].hasOwnProperty("reportId")){$("#"+this.HyperLinkIdArray[c].reportId).click(this.ShowReportPrompterHandler);this.ReportLinkHash[this.HyperLinkIdArray[c].reportId]=this.HyperLinkIdArray[c].reportLink}}}};a.ShowAttachPrompterHandler=function(){return a.ShowPrompter(a.AttachPromptText)};a.ShowReportPrompterHandler=function(){return a.ShowPrompter(a.ReportPromptText,this.id)};a.ShowPrompter=function(e,f){var c=this.ReportLinkHash[f]||"";if(confirm(e)){if(c!==""){window.open(c,"FeedbackURL","height=600,width=720,status=yes,toolbar=no,menubar=no,location=no")}else{return true}}return false};return a};SKYSALES.Class.AdvancedBookingListSearchInput.createObject=fu
    nction(a) {
        SKYSALES.Util.createObject("advancedBookingListSearchInput", "AdvancedBookingListSearchInput", a)
    }
};