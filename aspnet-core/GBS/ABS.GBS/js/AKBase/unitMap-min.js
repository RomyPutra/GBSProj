if (SKYSALES.Class.Base === undefined) {
	SKYSALES.Class.Base = function () {
		var a = this;
		a.init = SKYSALES.Class.Base.prototype.init;
		a.setSettingsByObject = SKYSALES.Class.Base.prototype.setSettingsByObject;
		a.addEvents = SKYSALES.Class.Base.prototype.addEvents;
		a.setVars = SKYSALES.Class.Base.prototype.setVars;
		return a
	};
	SKYSALES.Class.Base.prototype.init = function (a) {};
	SKYSALES.Class.Base.prototype.setSettingsByObject = function (a) {
		var b = "";
		for (b in a) {
			if (a.hasOwnProperty(b)) {
				if (this[b] !== undefined) {
					this[b] = a[b]
				}
			}
		}
	};
	SKYSALES.Class.Base.prototype.addEvents = function () {};
	SKYSALES.Class.Base.prototype.setVars = function () {}
	
}
if (SKYSALES.Class.UnitContainer === undefined) {
	SKYSALES.Class.UnitContainer = function () {
		var b = SKYSALES.Class.Base();
		var a = SKYSALES.Util.extendObject(b);
		a.containerId = "";
		a.container = null;
		a.setVars = function () {
			a.container = $("#" + this.containerId)
		};
		a.hide = function () {
			this.container.hide("slow")
		};
		a.show = function () {
			this.container.show("slow")
		};
		return a
	}
}
if (SKYSALES.Class.UnitMapContainer === undefined) {
	SKYSALES.Class.UnitMapContainer = function () {
		var a = SKYSALES.Class.Base();
		var b = SKYSALES.Util.extendObject(a);
		b.clientId = "";
		b.clientName = "";
		b.blockedSeatMessage = "";
		b.noSeatsConfirmationMessage = "";
		b.blankImageName = "";
		b.equipmentImagePath = "";
		b.genericUnitImagePath = "";
		b.propertyIconImagePath = "";
		b.propertySuperSetId = "propertyId";
		b.propertyAutoAssignId = "propertyId";
		b.iconMax = 5;
		b.iconMaxLargeSeat = 7;
		b.noUnitsMeetFilterCriteriaMessage = "";
		b.grid = 0;
		b.activeCompartmentDesignator = "";
		b.activeCompartmentDeck = -1;
		b.activeUnitInput = null;
		b.nextUnitInput = null; //added by diana 20170125
		b.activeUnitMap = null;
		b.activeCompartment = null;
		b.activeUnit = null;
		b.unitInputArray = [];
		b.ssrHash = {};
		b.ssrFeeHash = {};
		b.showDetailId = null;
		b.unitMapArray = [];
		b.ssrContainer = null;
		b.soldSsrHash = {};
		b.submitButtonId = "";
		b.skipButtonId = "";
		b.propertyFilterInputArray = null;
		b.ssrFilterInputArray = null;
		b.selectedPropertyHash = {};
		b.selectedSsrHash = {};
		b.showSsrContainerOnInit = false;
		b.ssrListJson = {};
		b.ssrFeeListJson = {};
		b.unitMapJson = [];
		b.activeUnitInputClass = "activeUnitInput";
		b.activeCompartmentClass = "carSelected";
		b.activeSelectedLinkClass = "selected";
		b.delimiter = "_";
		b.removeSegmentSeatsId = "";
		b.removeAutoSeatsMsg = "";
		b.freeLabel = "";
		b.showPremiumAlert = "";
		b.premiumAlertMsg = "";
		b.clean = function (c) {
			return c.replace(/[\$\s]/g, "_")
		};
		b.init = function (c) {
			this.setSettingsByObject(c);
			this.initAutoAssign();
			this.initSsrContainer();
			this.initSoldSsrHash();
			this.initSsrHash();
			this.initSsrFeeHash();
			this.initUnitMapArray();
			this.initUnitInputArray();
			this.initPropertyAutoAssignSet(this.activeUnitInput);
			this.initSubmitButton();
			delete b.initAutoAssign;
			delete b.initSsrContainer;
			delete b.initSoldSsrHash;
			delete b.initSsrHash;
			delete b.initSsrFeeHash;
			delete b.initUnitMapArray;
			delete b.initPropertyAutoAssignSet;
			delete b.initSubmitButton;
			delete b.init
		};
		b.initUnitInputArray = function () {
			var c = null;
			var d = 0;
            //to select seat textbox
		    //for (d = 0; d < this.unitInputArray.length; d += 1) {
			for (d = this.unitInputArray.length - 1; d >= 0; d -= 1) {
				c = SKYSALES.Class.UnitInput();
				b.unitInputArray[d].unitMapContainer = this;
				c.init(this.unitInputArray[d]);

			    //added by diana 20170525
				if (d < this.unitInputArray.length) {
				    c.nextUnitInput = b.unitInputArray[d + 1];
				    b.unitInputArray[d] = c;
				}
			    //b.unitInputArray[d].nextUnitInput = b.unitInputArray[d + 1];

				if (c.isActive === true && d == 0) {
					b.activeUnitInput = c
				}
			}
			this.updateActiveUnitInput(this.activeUnitInput);
			if (this.showSsrContainerOnInit) {
				this.activateUnitInput(this.activeUnitInput)
			}
		};
		b.initPropertyAutoAssignSet = function (e) {
			var h = $("#" + this.propertyAutoAssignId);
			var j = null;
			var g = this.unitMapArray || [];
			var d = null;
			var f = 0;
			var c = "";
			for (f = 0; f < g.length; f += 1) {
				d = g[f];
				j = this.getPropertyAutoAssignHash(d, e);
				c += '<span id="propertyAutoAssignSet_' + f + '" class="propertyAutoAssignSet propertyAutoAssignSet' + f + '" > <b>Flight: ' + (f + 1) + "</b>";
				c += this.getPropertyAutoAssignInputsHtml(j);
				c += this.getPropertyAutoAssignListsHtml(j);
				c += "</span>"
			}
			h.append(c)
		};
		b.getPropertyAutoAssignHash = function (p, s) {
			p = p || this.activeUnitMap;
			var l = null;
			var e = null;
			var h = 0;
			var r = {};
			var j = $("#propertyYesNoTemplateId").text();
			var m = $("#propertyStringTemplateId").text();
			var q = $("#propertyListTemplateId").text();
			var f = $("#propertyListOptionTemplateId").text();
			var t = "";
			var o = "";
			var k = null;
			var d = s.selectedAutoAssignPropertyArray || [];
			var c = null;
			if (p) {
				l = p.flattenedPropertyTypeList;
				for (h = 0; h < l.length; h += 1) {
					e = l[h];
					if (e.searchable === 1 && e.usedOnMap === 1) {
						o = e.propertyTypeCode;
						var n = "";
						var g = "";
						if (e.valueType === "YesNo") {
							for (i = 0; i < d.length; i += 1) {
								c = d[i];
								comparePropertyTypeCode = c.propertyTypeCode.toLowerCase();
								comparePropertyCode = c.propertyCode.toLowerCase();
								if ((comparePropertyCode === "true") && (comparePropertyTypeCode === e.propertyTypeCode.toLowerCase())) {
									n = "CHECKED"
								}
							}
							t = j;
							r[o] = {
								equipmentIndex : p.equipmentIndex,
								propertyType : e,
								html : t,
								type : "boolean",
								checked : n
							}
						} else {
							if ((e.valueType === "Numeric") || (e.valueType === "String")) {
								t = m;
								r[o] = {
									equipmentIndex : p.equipmentIndex,
									propertyType : e,
									html : t,
									type : "text"
								}
							} else {
								if (e.valueType === "List") {
									if (!r[o]) {
										t = q;
										r[o] = {
											propertyTypeName : e.propertyTypeName,
											equipmentIndex : p.equipmentIndex,
											optionArray : [],
											html : t,
											type : "list"
										}
									}
									for (i = 0; i < d.length; i += 1) {
										c = d[i];
										comparePropertyTypeCode = c.propertyTypeCode.toLowerCase();
										comparePropertyCode = c.propertyCode.toLowerCase();
										if ((comparePropertyCode === e.propertyCode.toLowerCase()) && (comparePropertyTypeCode === e.propertyTypeCode.toLowerCase())) {
											g = "SELECTED"
										}
									}
									t = f;
									k = r[o].optionArray;
									k[k.length] = {
										equipmentIndex : p.equipmentIndex,
										propertyType : e,
										html : t,
										type : "option",
										selected : g
									}
								}
							}
						}
					}
				}
			}
			return r
		};
		b.getPropertyAutoAssignInputsHtml = function (g) {
			var c = "";
			var d = "";
			var h = null;
			var f = "";
			var e = null;
			for (f in g) {
				if (g.hasOwnProperty(f)) {
					h = g[f];
					if (h.type !== "list") {
						d = h.html;
						e = h.propertyType;
						d = d.replace(/\[equipmentIndex\]/g, h.equipmentIndex);
						d = d.replace(/\[propertyTypeCode\]/g, e.propertyTypeCode);
						d = d.replace(/\[propertyCode\]/g, e.propertyCode);
						d = d.replace(/\[name\]/g, e.name);
						d = d.replace(/\[passengerNumber\]/g, h.passengerNumber);
						d = d.replace(/\[checked\]/g, h.checked);
						c += d
					}
				}
			}
			return c
		};
		b.getPropertyAutoAssignListsHtml = function (h) {
			var c = "";
			var j = "";
			var n = "";
			var l = "";
			var k = null;
			var g = null;
			var e = null;
			var m = "";
			var f = 0;
			var d = null;
			for (m in h) {
				if (h.hasOwnProperty(m)) {
					e = h[m];
					if (e.type === "list") {
						j = e.html;
						j = j.replace(/\[equipmentIndex\]/g, e.equipmentIndex);
						j = j.replace(/\[name\]/g, e.propertyTypeName);
						j = j.replace(/\[propertyTypeCode\]/g, m);
						j = j.replace(/\[propertyCode\]/g, m);
						j = j.replace(/\[passengerNumber\]/g, e.passengerNumber);
						l = "";
						g = e.optionArray || [];
						for (f = 0; f < g.length; f += 1) {
							k = g[f];
							d = k.propertyType;
							n = k.html;
							n = n.replace(/\[equipmentIndex\]/g, e.equipmentIndex);
							n = n.replace(/\[propertyTypeCode\]/g, d.propertyTypeCode);
							n = n.replace(/\[propertyCode\]/g, d.propertyCode);
							n = n.replace(/\[name\]/g, d.name);
							n = n.replace(/\[selected\]/g, k.selected);
							l += n
						}
						j = j.replace("[optionArray]", l);
						c += j
					}
				}
			}
			return c
		};
		b.initUnitMapArray = function () {
			var d = 0;
			var c = null;
			for (d = 0; d < this.unitMapJson.length; d += 1) {
				c = SKYSALES.Class.UnitMap();
				c.listItem = $("#listItem_" + d);
				c.unitMapContainer = this;
				c.equipmentIndex = d;
				c.init(this.unitMapJson[d]);
				b.unitMapArray[d] = c;
				if (c.equipment.isActive) {
					if (d == 0) {
						if (b.showPremiumAlert == "true") {
							//alert(b.premiumAlertMsg)
						}
					}
					b.activeUnitMap = c
				}
			}
			if ((!this.activeUnitMap) && (this.unitMapArray.length > 0)) {
				b.activeUnitMap = this.unitMapArray[0]
			}
			b.activeUnitMap.listItem.addClass("selected");
			delete b.unitMapJson
		};
		b.initSubmitButton = function () {
			var c = $("#" + this.submitButtonId);
			c.click(this.submitButtonHandler);
			var d = $("#" + this.removeSegmentSeatsId);
			d.click(this.removeSegmentSeats);
			$("#" + this.skipButtonId).click(function () {
				return confirm(b.noSeatsConfirmationMessage)
			})
		};
		b.submitButtonHandler = function () {
			var d = b.unitInputArray || [];
			var c = true;
			if (d.length > 0) {
				for (i = 0; i < d.length; i += 1) {
					if ($("#" + d[i].inputId).val() != "") {
						c = false
					}
				}
			}
			if (c) {
				return confirm(b.noSeatsConfirmationMessage)
			}
		};
		b.removeSegmentSeats = function () {
			var f = confirm(b.removeAutoSeatsMsg);
			if (f) {
				var e = b.activeUnitMap.equipmentIndex;
				var g = b.unitInputArray || [];
				var d = 0;
				var c = null;
				if (g.length > 0) {
					for (d = 0; d < g.length; d += 1) {
						c = g[d];
						c.removeUnitInput()
					}
				}
			}
		};
		b.initAutoAssign = function () {
			var c = $("#AutoUnitAssignAllButton");
			c.click(this.autoAssignHandler)
		};
		b.autoAssignHandler = function () {
			var c = null;
			var d = 0;
			var e = b.unitInputArray || [];
			if (e.length > 0) {
				c = e[0];
				b.activeUnitInput = c;
				b.ssrContainer.unitInput = b.unitInput;
				b.ssrContainer.unit = b.getUnitByUnitKey(b.activeUnitInput.unitKey)
			}
			for (d = 0; d < e.length; d += 1) {
				c = e[d];
				b.writeAutoAssignPropertiesToDom(c)
			}
			window.__doPostBack(b.clientName + "$LinkButtonAutoAssignUnit", "")
		};
		b.initSsrContainer = function () {
			var c = {
				containerId : "confirmSeat",
				dynamicContainerId : "confirmSeatContainer",
				ssrTemplateId : "ssrId",
				ssrCancelButtonId : "ssrCancelButton",
				ssrConfirmButtonId : "ssrConfirmButton",
				sellSsrButtonId : "sellSsrButton",
				unitMapContainer : this
			};
			b.ssrContainer = SKYSALES.Class.SsrContainer();
			b.ssrContainer.init(c)
		};
		b.initSoldSsrHash = function () {
			var f = this.soldSsrHash;
			var e = null;
			var g = null;
			var d = 0;
			var c = "";
			for (c in f) {
				if (f.hasOwnProperty(c)) {
					e = f[c].soldSsrArray;
					for (d = 0; d < e.length; d += 1) {
						g = new SKYSALES.Class.SoldSsr();
						g.init(e[d]);
						e[d] = g
					}
					f[c].soldSsrArray = e;
					f[c].lostSsrArray = []
				}
			}
			b.soldSsrHash = f
		};
		b.initSsrHash = function () {
			var c = 0;
			var e = this.ssrListJson.SsrList;
			var d = null;
			e = e || [];
			for (c = 0; c < e.length; c += 1) {
				d = SKYSALES.Class.Ssr();
				d.init(e[c]);
				if (!b.ssrHash[d.ssrCode]) {
					b.ssrHash[d.ssrCode] = d
				}
			}
			delete b.ssrListJson
		};
		b.initSsrFeeHash = function () {
			var e = 0;
			var d = this.ssrFeeListJson.SsrFeeList;
			var c = null;
			var f = "";
			d = d || [];
			for (e = 0; e < d.length; e += 1) {
				c = SKYSALES.Class.SsrFee();
				c.init(d[e]);
				f = c.getKey();
				b.ssrFeeHash[f] = c
			}
			delete b.ssrFeeListJson
		};
		b.updateActiveUnitInput = function (e) {
			var f = 0;
			var h = this.unitInputArray;
			var d = null;
			var c = 0;
			var g = null;
			if (this.activeUnitInput && e) {
				c = this.activeUnitInput.equipmentIndex;
				if (e.equipmentIndex === c) {
					for (f = 0; f < h.length; f += 1) {
						d = h[f];
						if (d.equipmentIndex !== c) {
							continue
						}
						g = this.getUnitByUnitKey(d.unitKey);
						if (d === e) {
							if (g) {
							    g.updateSeatImage("Selected");
								g.unitAvailability = "Open"
							}
							d.input.addClass(this.activeUnitInputClass);
							b.activeUnitInput = d;
							b.activeUnit = g;
						} else {
							if (g) {
								g.updateSeatImage("Selected");
								g.unitAvailability = "Occupied";
							}
							d.deactivate()
						}
					}
					this.updateFilterSsrFees();
					this.selectFilterProperties();
					this.selectFilterSsrs()
				} else {
					$("#" + this.clientId + "_passengerInput").val(e.passengerNumber);
					this.postBackForEquipment()
				}
			}
		};
		b.activateUnitInput = function (e) {
			var d = null;
			var c = -1;
			if (this.activeUnitInput && e) {
				this.ssrContainer.writeSoldSsrsToDom();
				c = this.activeUnitInput.equipmentIndex;
				if (e.equipmentIndex === c) {
					b.activeUnitInput = e;
					b.ssrContainer.showFlightList = true;
					b.ssrContainer.unitInput = b.activeUnitInput;
					b.ssrContainer.unit = b.getUnitByUnitKey(b.activeUnitInput.unitKey);
					if (b.ssrContainer.unit) {}
					
				} else {
					if (this.unitMapArray.length > e.equipmentIndex) {
						b.activeUnitInput = e;
						b.activeEquipment = b.unitMapArray[e.equipmentIndex];
						d = this.getUnitByUnitKey(this.activeUnitInput.unitKey);
						if (d) {
							b.activeCompartment = d.compartment;
							b.activeCompartmentDesignator = d.compartmentDesignator;
							b.ssrContainer.showFlightList = true;
							b.ssrContainer.unitInput = b.activeUnitInput;
							b.activeCompartmentDeck = d.compartment.deck;
							b.activeUnit = d;
							b.ssrContainer.unit = d;
							if (b.ssrContainer.unit) {}
							
						} else {
							this.postBackForUpSell()
						}
					} else {
						this.postBackForUpSell()
					}
				}
			}
		};
		b.postBackForUpSell = function () {
			b.showSsrContainerOnInit = true;
			var c = '<input type="hidden" name="' + b.clientName + '$showUpSell" value="true" />';
			$("#showUpSellInputContainer").html(c);
			window.__doPostBack(this.clientName + "$LinkButtonTripSelector", "")
		};
		b.confirmSeat = function (d) {
			var f = this.activeUnit;
			var e = this.activeUnitInput;
			var c = false;
			if ((e) && (d) && (d.unitAvailability === "Open") && !c) {
				if (f === d) {
					e.input.val("");
					e.hiddenInput.val("");
					e.unitKey = "";
					if (d.updateSeatImage) {
						d.updateSeatImage()
					}
					d = null;
					b.activeUnit = null;
					e.removeInputFee()
				} else {
					if (d.hasProperty(8)) {
						//alert(b.blockedSeatMessage)
					}
					b.assignSeat(d);
				}
			}
		};
		b.isValidAge = function (g) {
			var e = new Date();
			var f = new Date(g);
			var h = 1000 * 60 * 60 * 24 * 365;
			var c = Math.floor((e.getTime() - f.getTime()) / h);
			var d = false;
			if (c > 15 && c < 65) {
				d = true
			}
			return d
		};
		b.assignSeat = function (d) {
			
			var g = this.activeUnit;
			var f = this.activeUnitInput;
			var e = "";
			var c = d.unitFee;
			//alert("assignseat: orivalue: " + f.getOriSeat() + " to:" + d.getKey());
			if ((f) && (d) && (d.unitAvailability === "Open") && (f.validSeat(d.getKey()))) {
			    e = d.getKey();
			    //window.alert("A");
				f.setInputAndHiddenInput(e);
				f.unitKey = e;
				f.showInputFee(d.unitFee);
				f.paxSeatFee.html(c);
				if (d.updateSeatImage) {
					d.updateSeatImage("Selected")
				}
				b.activeUnit = d;

				if (g) {
					if (g.updateSeatImage) {
						g.updateSeatImage()
					}
				}
				b.activeUnit = d;

			    //begin, added by diana 20170125, focus to next textbox
				var index = f.passengerNumber;
				if (index >= 0 && index < b.unitInputArray.length - 1) {
				    this.activeUnitInput = b.unitInputArray[index].nextUnitInput;
				    f = this.activeUnitInput;
				    f.updateActiveUnitInput(this.activeUnitInput);
				}
			    //end, added by diana 20170125, focus to next textbox

			}
		};
		b.getUnitByUnitKey = function (j) {
			j = j || "";
			var c = j.split(this.delimiter);
			var g = -1;
			var e = "";
			var k = "";
			var n = "";
			var h = null;
			var m = null;
			var d = null;
			var l = null;
			var f = 0;
			if (c.length === 4) {
				g = c[0];
				e = c[1];
				k = c[2];
				k = parseInt(k, 10);
				n = c[3];
				g = parseInt(g, 10);
				if (this.unitMapArray.length > g) {
					h = this.unitMapArray[g].equipment;
					if (h && h.compartmentHash[e]) {
						m = h.compartmentHash[e];
						for (f = 0; f < m.length; f += 1) {
							d = m[f];
							if (d && d.unitHash[j]) {
								l = d.unitHash[j];
								if (l) {
									break
								}
							}
						}
					}
				}
			}
			return l
		};
		b.postBackForEquipment = function () {
			if (this.activeCompartment) {
				this.activeCompartment.compartmentContainer.html("")
			}
			window.__doPostBack(this.clientName + "$LinkButtonTripSelector", "")
		};
		b.showNoUnitsMeetFilterCriteria = function () {
			window.alert(this.noUnitsMeetFilterCriteriaMessage)
		};
		b.updateFilterSsrFees = function (n) {
			n = n || this.activeUnitInput;
			var k = this.getSsrFilterInputHash();
			var e = null;
			var h = "";
			var c = null;
			var l = null;
			var f = null;
			var m = null;
			var j = "";
			var g = "";
			var d = "";
			for (d in k) {
				if (k.hasOwnProperty(d)) {
					m = k[d];
					l = m.input;
					e = m.fee;
					h = l.attr("id");
					h = h || "";
					h = h.replace("ssr_", "");
					c = this.ssrHash[h];
					f = this.getSsrFee(c);
					j = f.supplant();
					g = e.html();
					g = g || "";
					g = g.replace("[ssrFee]", j);
					e.html(g)
				}
			}
		};
		b.getSsrFee = function (h) {
			h = h || new SKYSALES.Class.Ssr();
			var e = this.delimiter;
			var f = this.activeUnitInput;
			var d = this.ssrFeeHash;
			var c = null;
			var g = h.feeCode + e + f.journeyIndex + e + f.segmentIndex + e + f.passengerNumber;
			c = d[g];
			if (!c) {
				c = new SKYSALES.Class.SsrFee();
				c.init()
			}
			return c
		};
		b.selectFilterProperties = function (m) {
			m = m || this.activeUnitInput;
			var j = m.selectedFilterPropertyArray || [];
			var l = this.getPropertyFilterInputHash();
			var k = null;
			var h = 0;
			var f = false;
			var d = "";
			var e = null;
			var g = "";
			var c = -1;
			for (d in l) {
				if (l.hasOwnProperty(d)) {
					k = l[d].input;
					c = l[d].index;
					if (k.length > 0) {
						g = k.attr("name");
						g = g.toLowerCase();
						f = k.is(":checkbox");
						if (f) {
							k.removeAttr("checked")
						} else {
							k.val("")
						}
						for (h = 0; h < j.length; h += 1) {
							e = j[h];
							comparePropertyTypeCode = e.propertyTypeCode.toLowerCase();
							comparePropertyCode = e.propertyCode.toLowerCase();
							if ((comparePropertyCode === "true") && (comparePropertyTypeCode === g)) {
								f = k.is(":checkbox");
								if (f) {
									k.attr("checked", "checked")
								}
							} else {
								optionArray = $("option", k);
								optionArray = optionArray || [];
								for (optionIndex = 0; optionIndex < optionArray.length; optionIndex += 1) {
									filterOptionName = optionArray[optionIndex].value;
									filterOptionName = filterOptionName || "";
									filterOptionName = filterOptionName.toLowerCase();
									if (comparePropertyCode === filterOptionName) {
										k.val(e.propertyCode)
									}
								}
							}
						}
					}
				}
			}
		};
		b.selectFilterSsrs = function (l) {
			l = l || this.activeUnitInput;
			var e = l.selectedFilterSsrArray;
			var j = this.getSsrFilterInputHash();
			var k = null;
			var g = 0;
			var d = false;
			var c = "";
			var h = null;
			var f = "";
			for (c in j) {
				if (j.hasOwnProperty(c)) {
					k = j[c].input;
					if (k) {
						f = k.attr("name");
						d = k.is(":checkbox");
						if (d) {
							k.removeAttr("checked")
						} else {
							k.val("")
						}
						for (g = 0; g < e.length; g += 1) {
							h = e[g];
							if (h.ssrCode === f) {
								d = k.is(":checkbox");
								if (d) {
									k.attr("checked", "checked")
								} else {
									k.val(h.propertyCode)
								}
								break
							}
						}
					}
				}
			}
		};
		b.sortPropertyList = function (f, d) {
			var e = f.displayPriority;
			var c = d.displayPriority;
			var g = 0;
			if ((e === 0) && (c === 0)) {
				g = 0
			} else {
				if (e === 0) {
					g = 1
				} else {
					if (c === 0) {
						g = -1
					} else {
						g = e - c
					}
				}
			}
			if (g === 0) {
				g = b.sortPropertyListByName(f, d)
			}
			return g
		};
		b.sortPropertyListByName = function (g, f) {
			var c = null;
			var e = g.name.toLowerCase();
			var d = f.name.toLowerCase();
			var h = 0;
			if (e !== d) {
				c = [e, d];
				c.sort();
				if (c[0] === e) {
					h = -1
				} else {
					h = 1
				}
			}
			return h
		};
		b.getSelectedSsrFilterInputHash = function () {
			var m = {};
			var j = this.getSsrFilterInputHash();
			var n = this.activeUnitMap || {};
			var f = n.ssrCodeList || [];
			var g = "";
			var l = "";
			var k = null;
			var h = 0;
			var d = false;
			var c = false;
			var e = "";
			for (h = 0; h < f.length; h += 1) {
				e = f[h];
				if (j[e]) {
					k = j[e].input;
					if (k) {
						l = k.val();
						d = k.is(":checkbox");
						if (d) {
							c = k.attr("checked");
							if (!c) {
								l = ""
							}
						}
						if (l) {
							g = k.attr("name");
							m[g] = {
								filterName : g,
								filterValue : l,
								index : h
							}
						}
					}
				}
			}
			return m
		};
		b.getSelectedAutoAssignPropertyInputHash = function (q) {
			var r = {};
			var m = this.getAutoAssignPropertyInputHash(q);
			var c = "";
			var l = "";
			var p = "";
			var o = null;
			var h = false;
			var g = false;
			var n = false;
			var e = "";
			var d = -1;
			var f = null;
			var k = -1;
			var j = "";
			for (e in m) {
				if (m.hasOwnProperty(e)) {
					f = m[e];
					o = f.input;
					d = f.index;
					k = f.equipmentIndex;
					if (o.length > 0) {
						p = o.val();
						h = o.is(":checkbox");
						n = o.is("select");
						if (h) {
							g = o.attr("checked");
							if (!g) {
								p = ""
							}
						} else {
							if (n) {
								j = p + "_List_" + k;
								if (e !== j) {
									p = ""
								}
							}
						}
						if (p) {
							c = o.attr("id");
							l = o.attr("name");
							r[c] = {
								autoAssignName : l,
								autoAssignValue : p,
								index : d,
								equipmentIndex : k
							}
						}
					}
				}
			}
			return r
		};
		b.getSelectedPropertyFilterInputHash = function () {
			var m = {};
			var l = this.getPropertyFilterInputHash();
			var g = "";
			var j = "";
			var h = null;
			var f = false;
			var e = false;
			var k = false;
			var d = "";
			var c = -1;
			for (d in l) {
				if (l.hasOwnProperty(d)) {
					h = l[d].input;
					c = l[d].index;
					if (h.length > 0) {
						j = h.val();
						f = h.is(":checkbox");
						k = h.is("select");
						if (f) {
							e = h.attr("checked");
							if (!e) {
								j = ""
							}
						} else {
							if (k) {
								if (d !== j + "_List") {
									j = ""
								}
							}
						}
						if (j) {
							g = h.attr("name");
							m[g] = {
								filterName : g,
								filterValue : j,
								index : c
							}
						}
					}
				}
			}
			return m
		};
		b.getSsrFilterInputHash = function () {
			var j = "";
			var d = this.activeUnitMap || {};
			var k = d.ssrCodeList || [];
			var f = 0;
			var c = this.ssrHash;
			var h = null;
			var g = "";
			var e = $("#filterPanelSsrContainer");
			b.ssrFilterInputHash = {};
			for (f = 0; f < k.length; f += 1) {
				j = k[f];
				h = c[j];
				if (h) {
					g = h.ssrCode + "Container";
					b.ssrFilterInputHash[h.ssrCode] = {
						input : $("#ssr_" + h.ssrCode, e),
						index : f,
						fee : $("#ssr_" + g, e)
					}
				}
			}
			return this.ssrFilterInputHash
		};
		b.getAutoAssignPropertyInputHash = function (o) {
			var j = this.unitMapArray || [];
			var g = 0;
			var n = null;
			var d = [];
			var c = 0;
			var h = null;
			b.autoAssignPropertyInputHash = {};
			var k = "";
			var l = $("#autoAssignPropertyContainer");
			var e = -1;
			var f = "";
			var m = null;
			if (o) {
				e = o.equipmentIndex;
				e = parseInt(e, 10)
			}
			for (g = 0; g < j.length; g += 1) {
				n = j[g];
				if (e === -1 || n.equipmentIndex === e) {
					d = n.flattenedPropertyTypeList;
					for (c = 0; c < d.length; c += 1) {
						h = d[c];
						if (h.searchable) {
							k = h.propertyCode;
							if (k === "") {
								k = h.propertyTypeCode
							}
							f = "property_" + h.propertyTypeCode + "_" + h.valueType + "_" + n.equipmentIndex;
							m = $("#" + f, l);
							if (m.length > 0) {
								k = k + "_" + h.valueType + "_" + n.equipmentIndex;
								if (!this.autoAssignPropertyInputHash[k]) {
									this.autoAssignPropertyInputHash[k] = {
										input : $("#" + f, l),
										index : c,
										equipmentIndex : n.equipmentIndex
									}
								}
							}
						}
					}
				}
			}
			return this.autoAssignPropertyInputHash
		};
		b.getPropertyFilterInputHash = function () {
			var e = this.activeUnitMap || {};
			var j = e.flattenedPropertyTypeList || [];
			var h = 0;
			var f = null;
			b.propertyFilterInputHash = {};
			var g = "";
			var k = "";
			var d = null;
			var c = $("#filterPanelPropertyContainer");
			for (h = 0; h < j.length; h += 1) {
				f = j[h];
				if (f.searchable) {
					g = f.propertyCode;
					if (g === "") {
						g = f.propertyTypeCode
					}
					k = "property_" + f.propertyTypeCode + "_" + f.valueType;
					d = $("#" + k, c);
					g = g + "_" + f.valueType;
					if (!this.propertyFilterInputHash[g]) {
						this.propertyFilterInputHash[g] = {
							input : $("#" + k, c),
							index : h
						}
					}
				}
			}
			return this.propertyFilterInputHash
		};
		b.writeFilterToDom = function (c) {
			this.writeFilterPropertiesToDom(c);
			this.writeFilterSsrsToDom(c)
		};
		b.writeFilterSsrsToDom = function (q) {
			var s = "selectedFilterSsrInputContainer";
			var t = $("#" + s);
			q = q || this.activeUnitInput;
			var r = {};
			var c = "";
			var m = "";
			var u = this.getSelectedSsrFilterInputHash();
			var n = null;
			var v = [];
			var o = null;
			var d = "";
			var l = 0;
			var p = this.clientId;
			var k = this.clientName;
			var j = "";
			var f = "";
			var e = this.delimiter;
			var g = null;
			var h = "";
			if (q) {
				m = q.getKey();
				h = "selectedFilterSsrInputContainer" + e + m;
				g = [];
				if (!r[m]) {
					r[m] = []
				}
				v = r[m];
				for (c in u) {
					if (u.hasOwnProperty(c)) {
						n = u[c];
						v[v.length] = n
					}
				}
				r[m] = v;
				for (c in r) {
					if (r.hasOwnProperty(c)) {
						v = r[c];
						for (l = 0; l < v.length; l += 1) {
							o = v[l];
							j = p + e + q.equipmentIndex + e + q.passengerNumber + e + o.filterName;
							f = k + "$selectedFilterSsr_" + q.equipmentIndex + e + q.passengerNumber + e + o.filterName;
							d += '<input type="hidden" id="' + j + '" name="' + f + '" value="' + o.filterValue + '" />';
							g[g.length] = {
								ssrCode : o.filterName,
								quantity : o.filterValue
							}
						}
					}
				}
				q.selectedFilterSsrArray = g;
				$("#" + h).remove();
				t.append('<span id="' + h + '">' + d + "</span>")
			}
		};
		b.writeAutoAssignPropertiesToDom = function (u) {
			var k = "selectedAutoAssignPropertyInputContainer";
			var v = $("#" + k);
			u = u || this.activeUnitInput;
			var n = {};
			var e = "";
			var s = "";
			var d = this.getSelectedAutoAssignPropertyInputHash(u);
			var w = null;
			var f = [];
			var q = null;
			var c = "";
			var r = 0;
			var t = this.clientId;
			var o = this.clientName;
			var m = "";
			var j = "";
			var h = b.delimiter;
			var g = null;
			var l = "";
			var p = -1;
			if (u) {
				l = "selectedAutoAssignPropertyInputContainer" + h + u.getKey();
				g = u.selectedAutoAssignPropertyArray || [];
				s = u.getKey();
				if (!n[s]) {
					n[s] = []
				}
				f = n[s];
				for (e in d) {
					if (d.hasOwnProperty(e)) {
						w = d[e];
						f[f.length] = w
					}
				}
				n[s] = f;
				for (e in n) {
					if (n.hasOwnProperty(e)) {
						f = n[e];
						for (r = 0; r < f.length; r += 1) {
							q = f[r];
							p = parseInt(u.equipmentIndex, 10);
							if (p === q.equipmentIndex) {
								m = t + h + u.equipmentIndex + h + u.passengerNumber + h + q.autoAssignName;
								j = o + "$selectedFilterProperty_" + u.equipmentIndex + h + u.passengerNumber + h + q.autoAssignName;
								c += '<input type="hidden" id="' + m + '" name="' + j + '" value="' + q.autoAssignValue + '" />';
								g[g.length] = {
									propertyCode : q.autoAssignValue,
									propertyTypeCode : q.autoAssignName
								}
							}
						}
					}
				}
				u.selectedAutoAssignPropertyArray = g;
				$("#" + l).remove();
				v.append('<span id="' + l + '">' + c + "</span>")
			}
		};
		b.writeFilterPropertiesToDom = function (t) {
			var k = "selectedFilterPropertyInputContainer";
			var u = $("#" + k);
			t = t || this.activeUnitInput;
			var n = {};
			var f = "";
			var r = "";
			var d = this.getSelectedPropertyFilterInputHash();
			var v = null;
			var g = [];
			var p = null;
			var c = "";
			var q = 0;
			var s = this.clientId;
			var o = this.clientName;
			var m = "";
			var j = "";
			var h = b.delimiter;
			var e = null;
			var l = "";
			if (t) {
				l = "selectedFilterPropertyInputContainer" + h + t.getKey();
				e = [];
				r = t.getKey();
				if (!n[r]) {
					n[r] = []
				}
				g = n[r];
				for (f in d) {
					if (d.hasOwnProperty(f)) {
						v = d[f];
						g[g.length] = v
					}
				}
				n[r] = g;
				for (f in n) {
					if (n.hasOwnProperty(f)) {
						g = n[f];
						for (q = 0; q < g.length; q += 1) {
							p = g[q];
							m = s + h + t.equipmentIndex + h + t.passengerNumber + h + p.filterName;
							j = o + "$selectedFilterProperty_" + t.equipmentIndex + h + t.passengerNumber + h + p.filterName;
							c += '<input type="hidden" id="' + m + '" name="' + j + '" value="' + p.filterValue + '" />';
							e[e.length] = {
								propertyCode : p.filterValue,
								propertyTypeCode : p.filterName
							}
						}
					}
				}
				t.selectedFilterPropertyArray = e;
				$("#" + l).remove();
				u.append('<span id="' + l + '">' + c + "</span>")
			}
		};
		b.getUnfulfilledPropertyHash = function (e) {
			var c = {};
			var g = 0;
			var h = null;
			var k = null;
			var f = "";
			var j = "";
			var d = null;
			h = e.unfulfilledPropertyArray;
			for (g = 0; g < h.length; g += 1) {
				k = h[g];
				f = k.key;
				j = k.value;
				d = null;
				if (j.toLowerCase() === "true") {
					d = this.getPropertyByPropertyTypeCode(f, e)
				} else {
					d = this.getPropertyByPropertyCode(j, e)
				}
				c[f] = d
			}
			return c
		};
		b.getEquipmentByUnitInput = function (d) {
			d = d || this.activeUnitInput;
			var e = this.unitMapArray || [];
			var c = d.equipmentIndex;
			c = parseInt(c, 10);
			var f = null;
			if (e.length > c) {
				f = e[c]
			}
			return f
		};
		b.getPropertyByPropertyCode = function (f, c) {
			var g = this.getEquipmentByUnitInput(c) || {};
			var e = 0;
			var j = null;
			var d = null;
			var h = {
				name : ""
			};
			j = g.flattenedPropertyTypeList || [];
			for (e = 0; e < j.length; e += 1) {
				d = j[e];
				if (d.propertyCode === f) {
					h = d;
					break
				}
			}
			return h
		};
		b.getPropertyByPropertyTypeCode = function (f, c) {
			var g = this.getEquipmentByUnitInput(c) || {};
			var e = 0;
			var j = null;
			var d = null;
			var h = {
				name : ""
			};
			j = g.flattenedPropertyTypeList || [];
			for (e = 0; e < j.length; e += 1) {
				d = j[e];
				if (d.propertyTypeCode === f) {
					h = d;
					break
				}
			}
			return h
		};
		b.getNotSeatedTogether = function (c) {
			var d = "";
			d = c.notSeatedTogether;
			return d
		};
		b.getIconImageUri = function (d) {
			var c = "";
			if (d) {
				c = this.propertyIconImagePath + d
			} else {
				c = this.equipmentImagePath + this.blankImageName
			}
			return c
		};
		return b
	}
}
if (SKYSALES.Class.UnitMap === undefined) {
	SKYSALES.Class.UnitMap = function () {
		var b = SKYSALES.Class.Base();
		var a = SKYSALES.Util.extendObject(b);
		a.equipmentIndex = -1;
		a.equipment = null;
		a.flattenedPropertyTypeList = [];
		a.numericPropertyCodeList = [];
		a.numericPropertyHash = {};
		a.ssrCodeList = [];
		a.unitMapContainer = null;
		a.init = function (c) {
			this.setSettingsByObject(c)
		};
		a.setSettingsByObject = function (l) {
			var j = {};
			var g = [];
			var c = [];
			var k = [];
			var f = [];
			var h = 0;
			var d = null;
			var e = "";
			if (l.flattenedPropertyTypeListJson) {
				g = l.flattenedPropertyTypeListJson.FlattenedPropertyTypeList || [];
				a.flattenedPropertyTypeList = g
			}
			if (l.numericPropertyCodeListJson) {
				c = l.numericPropertyCodeListJson.PropertyCodeList || [];
				a.numericPropertyCodeList = c
			}
			if (l.numericPropertyListJson) {
				k = l.numericPropertyListJson.FlattenedPropertyTypeList || [];
				for (h = 0; h < k.length; h += 1) {
					d = k[h];
					e = d.propertyTypeCode;
					a.numericPropertyHash[e] = d
				}
			}
			if (l.ssrCodeListJson) {
				f = l.ssrCodeListJson.SsrCodeList || [];
				a.ssrCodeList = f
			}
			if (l.equipmentJson) {
				j = SKYSALES.Class.Equipment();
				j.unitMap = this;
				j.equipmentIndex = this.equipmentIndex;
				j.init(l.equipmentJson);
				a.equipment = j
			}
		};
		return a
	}
}
if (SKYSALES.Class.EquipmentBase === undefined) {
	SKYSALES.Class.EquipmentBase = function () {
		var b = SKYSALES.Class.UnitContainer();
		var a = SKYSALES.Util.extendObject(b);
		a.convertParamObject = SKYSALES.Class.EquipmentBase.prototype.convertParamObject;
		return a
	};
	SKYSALES.Class.EquipmentBase.prototype.convertParamObject = function (d, b) {
		var c = "";
		var a = "";
		for (c in d) {
			if (d.hasOwnProperty(c)) {
				a = d[c];
				if ((a !== c) && (b[c] !== undefined)) {
					b[a] = b[c];
					delete b[c]
				}
			}
		}
	}
}
if (SKYSALES.Class.Equipment === undefined) {
	SKYSALES.Class.Equipment = function () {
		var a = SKYSALES.Class.EquipmentBase();
		var b = SKYSALES.Util.extendObject(a);
		b.arrivalStation = "";
		b.equipmentCategory = "";
		b.equipmentType = "";
		b.equipmentTypeSuffix = "";
		b.marketingCode = "";
		b.availableUnits = 0;
		b.departureStation = "";
		b.propertyArray = [];
		b.compartmentHash = {};
		b.compartmentArray = [];
		b.filterButtonId = "filterEquipmentButton";
		b.filterButton = null;
		b.propertiesInUse = null;
		b.compartmentCount = 0;
		b.isStripped = true;
		b.isActive = false;
		b.unitMap = null;
		b.equipmentIndex = -1;
		b.init = function (c) {
			var d = SKYSALES.Class.Equipment.equipmentKeyHash;
			this.convertParamObject(d, c);
			this.setSettingsByObject(c);
			this.populateCompartmentHash();
			this.setVars();
			if (this.isActive) {
				this.addEvents()
			}
		};
		b.getKey = function () {
			return this.equipmentIndex
		};
		b.populateCompartmentHash = function () {
			var e = "";
			var f = null;
			var d = this.compartmentHash;
			var c = "";
			var g = null;
			for (e = 0; e < this.compartmentArray.length; e += 1) {
				f = SKYSALES.Class.Compartment();
				f.equipment = b;
				f.compartmentIndex = e;
				f.init(b.compartmentArray[e]);
				c = f.compartmentDesignator;
				g = d[c] || [];
				g[g.length] = f;
				b.compartmentHash[f.compartmentDesignator] = g
			}
			this.activateDefaultCompartment();
			delete b.compartmentArray
		};
		b.activateDefaultCompartment = function () {
			var e = null;
			var c = this.compartmentHash;
			var h = "";
			var g = null;
			var d = 0;
			var f = null;
			for (h in c) {
				if (c.hasOwnProperty(h)) {
					g = c[h];
					for (d = 0; d < g.length; d += 1) {
						e = g[d];
						e.isActive = false;
						if ((!f) || (e.availableUnits > f.availableUnits)) {
							f = e
						}
					}
				}
			}
			if (f) {
				f.updateCompartment()
			}
		};
		b.updateEquipmentFilterHandler = function () {
			b.updateEquipmentFilter()
		};
		b.updateEquipmentFilter = function () {
			var e = this.getUnitMapContainer();
			var h = "";
			var f = null;
			var c = e.activeCompartment;
			var g = null;
			var d = 0;
			c.updateCompartmentFilter();
			for (h in b.compartmentHash) {
				if (b.compartmentHash.hasOwnProperty(h)) {
					g = b.compartmentHash[h] || [];
					for (d = 0; d < g.length; d += 1) {
						f = g[d];
						if (f) {
							if (f !== c) {
								f.updateCompartmentFilter()
							}
							if (f.unitsWithSelectedCriteria) {
								if (!c.unitsWithSelectedCriteria) {
									c = f
								} else {
									if (f.availableUnits > c.availableUnits) {
										c = f
									} else {
										if ((f.availableUnits === c.availableUnits) && (f.unitsWithSelectedCriteria > c.unitsWithSelectedCriteria)) {
											c = f
										}
									}
								}
							}
						}
					}
				}
			}
			if (c.unitsWithSelectedCriteria === 0) {
				e.showNoUnitsMeetFilterCriteria()
			} else {
				c.updateCompartment()
			}
			e.writeFilterToDom()
		};
		b.addEvents = function () {
			a.addEvents.call(this);
			b.filterButton.click(this.updateEquipmentFilterHandler)
		};
		b.setVars = function () {
			a.setVars.call(this);
			b.filterButton = $("#" + this.filterButtonId);
			delete b.filterButtonId
		};
		b.getUnitMapContainer = function () {
			return this.unitMap.unitMapContainer
		};
		b.getFlattenedPropertyTypeList = function () {
			return this.unitMap.flattenedPropertyTypeList
		};
		b.getNumericPropertyCodeList = function () {
			return this.unitMap.numericPropertyCodeList
		};
		b.getSsrCodeList = function () {
			return this.unitMap.ssrCodeList
		};
		b.getNumericPropertyHash = function () {
			return this.unitMap.numericPropertyHash
		};
		return b
	};
	SKYSALES.Class.Equipment.equipmentKeyHash = {
		as : "arrivalStation",
		ec : "equipmentCategory",
		et : "equipmentType",
		ets : "equipmentTypeSuffix",
		mc : "marketingCode",
		au : "availableUnits",
		ds : "departureStation",
		p : "properties",
		c : "compartmentArray",
		piu : "propertiesInUse",
		is : "isStripped",
		ia : "isActive"
	}
}
if (SKYSALES.Class.Compartment === undefined) {
	SKYSALES.Class.Compartment = function () {
		var a = SKYSALES.Class.EquipmentBase();
		var b = SKYSALES.Util.extendObject(a);
		b.equipment = null;
		b.compartmentIndex = -1;
		b.deck = -1;
		b.compartmentDesignator = "";
		b.len = "";
		b.availableUnits = 0;
		b.sequence = "";
		b.width = "";
		b.unitHash = {};
		b.unitArray = [];
		b.isActive = false;
		b.unitsAvailableSpan = null;
		b.compartmentContainerId = "deck";
		b.compartmentContainer = null;
		b.unitHtmlArray = null;
		b.inputId = "";
		b.input = null;
		b.availableSeatsId = "";
		b.deckTabsId = "deckTabs";
		b.deckTabs = null;
		b.deckTabId = "deckTab_";
		b.deckTab = null;
		b.deckTabLabelId = "deckTabLabel_";
		b.deckTabLabel = null;
		b.deckTabAId = "deckTabA_";
		b.deckTabA = null;
		b.propertyFilterInputHash = null;
		b.ssrFilterInputHash = null;
		b.unitsWithSelectedCriteria = 0;
		b.delimiter = "_";
		b.init = function (c) {
			var d = SKYSALES.Class.Compartment.compartmentKeyHash;
			this.convertParamObject(d, c);
			this.setSettingsByObject(c);
			this.populateUnitHash();
			this.setVars();
			this.addEvents();
			this.unitsAvailableSpan.text(this.availableUnits)
		};
		b.getUnitMapContainer = function () {
			return this.equipment.getUnitMapContainer()
		};
		b.setSettingsByObject = function (e) {
			a.setSettingsByObject.call(this, e);
			var c = b.delimiter;
			var d = this.getUnitMapContainer();
			b.compartmentDesignator = d.clean(this.compartmentDesignator);
			b.containerId = "compartment" + c + this.compartmentDesignator + c + this.deck;
			b.unitContainerId = this.compartmentDesignator + c + this.deck;
			b.availableSeatsId = "availableSeats_compartment" + c + this.compartmentDesignator + c + "deck" + c + this.deck
		};
		b.updateCompartmentFilter = function () {
			var d = this.getUnitMapContainer();
			var e = "";
			var c = "";
			var h = null;
			var j = true;
			var l = false;
			var f = false;
			var k = d.activeUnitInput;
			var m = d.getSelectedPropertyFilterInputHash(k);
			var g = d.getSelectedSsrFilterInputHash();
			b.unitsWithSelectedCriteria = 0;
			this.draw();
			for (c in b.unitHash) {
				if (this.unitHash.hasOwnProperty(c)) {
					j = true;
					h = this.unitHash[c];
					if (h.hasProperty && h.unitAvailability === "Open") {
						h.updateSeatImage();
						for (c in m) {
							if (m.hasOwnProperty(c)) {
								e = m[c];
								if (e.index > -1) {
									l = h.hasProperty(e.index);
									if (!l) {
										j = false;
										break
									}
								}
							}
						}
						if (j === true) {
							for (c in g) {
								if (g.hasOwnProperty(c)) {
									e = g[c];
									if (e.index > -1) {
										f = h.hasSsr(e.index);
										if (!f) {
											j = false;
											break
										}
									}
								}
							}
						}
						if (j === true) {
							h.updateSeatImage("Filtered");
							b.unitsWithSelectedCriteria += 1
						}
					}
				}
			}
		};
		b.draw = function () {
			var l = this.getUnitMapContainer();
			var d = "";
			var k = null;
			var q = 0;
			var p = "";
			var c = parseFloat(l.grid, 10);
			var t = this.equipment.equipmentCategory;
			var m = 120;
			var e = 316;
			var j = 246;
			var n = 158;
			var o = b.width;
			var f = b.width;
			var r = b.len;
			var u = (o * c) / e;
			var g = "images/AKBase/equipment/";
			var s = this.equipment.compartmentCount;
			var h = 0;
			if (this.unitHtmlArray === null) {
				b.unitHtmlArray = [];
				q = 0;
				for (d in this.unitHash) {
					if (this.unitHash.hasOwnProperty(d)) {
						k = this.unitHash[d];
						b.unitHtmlArray[q] = k.getUnitHtml();
						q += 1
					}
				}
				p += '<div class="floor" style="top:' + c + "px;left:0px;width:" + c * o + "px; height:" + c * (r - 1) + 'px;"></div>';
				if (t === "Train") {
					f = (f * c) + 32;
					h = m * u - c;
					p += '<div id="nose" style="top:' + (m) * -1 + "px; left:0px; height:" + m + "px; width:" + f + 'px">';
					p += '<img src="' + g + 'train-nose.png" height="' + m + 'px;" width="' + f + 'px;" alt=""/>';
					p += "</div>";
					p += '<div class="wall" style="top:0; left:0px; width:' + c * o + "px; height:" + c + 'px;"></div>';
					p += '<div class="wall" style="top:' + c * r + "px; left:0px; width:" + c * o + "px; height:" + c + 'px;"></div>';
					p += '<div class="wall" style="top:' + c + "px; left:0; width:" + c + "px; height:" + c * (r - 1) + 'px;"></div>';
					p += '<div class="wall" style="top:' + c + "px; left:" + c * (o - 1) + "px; width:" + c + "px; height:" + c * (r - 1) + 'px;"></div>';
					p += '<div id="tail" style="top:' + r * c + "px; left:0px; height:" + n * u + "px; width:" + f + 'px">';
					p += '<img src="' + g + 'train-tail.png" width="' + f + 'px;" alt=""/>';
					p += "</div>"
				} else {
					if (this.compartmentIndex === 0) {
						h = j * u - c;
						p += '<div id="nose" style="top:' + (j * u - c) * -1 + "px; left:0px; height:" + j * u + "px; width:" + o * c + 'px">';
						p += '<img src="' + g + 'nose-m.png" width="' + e * u + 'px;" alt=""/>';
						p += "</div>"
					}
					if (this.compartmentIndex === (s - 1)) {
						p += '<div id="tail" style="top:' + r * c + "px; left:0px; height:" + n * u + "px; width:" + o * c + 'px">';
						p += '<img src="' + g + 'tail.gif" width="' + e * u + 'px;" alt=""/>';
						p += "</div>"
					}
					p += '<div class="wallRight" style="top:' + c + "px; left:0px; width:" + c + "px; height:" + c * (r - 1) + 'px;"></div>';
					p += '<div class="wallLeft" style="top:' + c + "px;left:" + c * (o - 1) + "px;width:" + c + "px; height:" + c * (r - 1) + 'px;"></div>'
				}
				this.compartmentContainer.css("top", h);
				this.compartmentContainer.height(c * r);
				this.compartmentContainer.width((c * o) + 50);
				this.compartmentContainer.append('<div id="' + this.unitContainerId + '" class="hiddens" >' + p + this.unitHtmlArray.join("\n") + "</div>");
				b.unitContainer = $("#" + this.unitContainerId);
				for (d in this.unitHash) {
					if (this.unitHash.hasOwnProperty(d)) {
						k = this.unitHash[d];
						k.draw()
					}
				}
			}
			b.unitHtmlArray.length = 0
		};
		b.populateUnitHash = function () {
			var f = null;
			var e = 0;
			var c = "";
			for (e = 0; e < this.unitArray.length; e += 1) {
				b.unitArray[e].compartment = this;
				c = this.unitArray[e].ut;
				switch (c) {
				case "NS":
					f = SKYSALES.Class.Seat();
					break;
				case "LS":
					f = SKYSALES.Class.LargeSeat();
					break;
				case "BH":
					f = SKYSALES.Class.BulkHead();
					break;
				case "B1":
					f = SKYSALES.Class.BedOneOfThree();
					break;
				case "B2":
					f = SKYSALES.Class.BedTwoOfThree();
					break;
				case "B3":
					f = SKYSALES.Class.BedThreeOfThree();
					break;
				case "B4":
					f = SKYSALES.Class.BedOneOfTwo();
					break;
				case "B5":
					f = SKYSALES.Class.BedTwoOfTwo();
					break;
				case "B6":
					f = SKYSALES.Class.BedOneOfOne();
					break;
				case "CO":
					f = SKYSALES.Class.SeatCompartment();
					break;
				case "TB":
					f = SKYSALES.Class.Table();
					break;
				case "WL":
					f = SKYSALES.Class.Wall();
					break;
				case "WI":
					f = SKYSALES.Class.Window();
					break;
				case "DR":
					f = SKYSALES.Class.Door();
					break;
				case "ST":
					f = SKYSALES.Class.Stairs();
					break;
				case "WG":
					if (this.unitArray[e].an === 0) {
						f = SKYSALES.Class.WingLeft()
					} else {
						if (this.unitArray[e].an === 180) {
							f = SKYSALES.Class.WingRight()
						} else {
							f = SKYSALES.Class.Wing()
						}
					}
					break;
				case "EX":
					f = SKYSALES.Class.Exit();
					break;
				case "LR":
					f = SKYSALES.Class.LabelRuler();
					break;
				case "GR":
					f = SKYSALES.Class.GenericUnit();
					break;
				case "DR":
					f = SKYSALES.Class.GenericUnit();
					break;
				case "LV":
					f = SKYSALES.Class.Lavatory();
					break;
				case "LG":
					f = SKYSALES.Class.Luggage();
					break;
				default:
					f = SKYSALES.Class.Unit()
				}
				f.compartment = this;
				f.init(this.unitArray[e]);
				var d = f.getKey();
				b.unitHash[d] = f
			}
			delete b.unitArray
		};
		b.updateDeckTabCss = function () {
			var f = null;
			var c = null;
			var e = null;
			var d = 0;
			if (b.equipment) {
				f = this.equipment.compartmentHash[b.compartmentDesignator]
			}
			f = f || [];
			if (f.length < 2) {
				this.deckTabs.hide()
			} else {
				this.deckTabs.show();
				for (d = 0; d < f.length; d += 1) {
					c = f[d];
					e = c.deckTab;
					if (this.deck === c.deck) {
						$("label", e).show();
						$("a", e).hide()
					} else {
						$("label", e).hide();
						$("a", e).show()
					}
				}
			}
		};
		b.addDeckTabEvents = function () {
			var d = 0;
			var e = null;
			var c = null;
			if (this.equipment) {
				e = this.equipment.compartmentHash[this.compartmentDesignator]
			}
			e = e || [];
			for (d = 0; d < e.length; d += 1) {
				c = e[d];
				c.deckTab.unbind("click");
				c.deckTab.click(c.activateDeckHandler)
			}
		};
		b.updateDeckTabs = function () {
			this.updateDeckTabCss();
			this.addDeckTabEvents()
		};
		b.activate = function () {
			var d = this.getUnitMapContainer();
			var c = d.activeCompartmentClass;
			var e = this.equipment.isStripped;
			var f = null;
			if (!e) {
				this.draw();
				this.unitContainer.show();
				f = this.equipment.compartmentHash[this.compartmentDesignator] || [];
				if (f.length > 0) {
					f[0].container.removeClass(c);
					f[0].container.addClass(c)
				}
				this.updateDeckTabs()
			}
			d.activeCompartment = this;
			d.activeCompartmentDeck = this.deck;
			b.isActive = true;
			d.writeFilterToDom(d.activeUnitInput);
			d.updateActiveUnitInput(d.activeUnitInput)
		};
		b.deactivate = function () {
			var e = this.getUnitMapContainer();
			var g = e.activeCompartment;
			var d = e.activeCompartmentClass;
			var f = [];
			var c = 0;
			if (g) {
				f = g.equipment.compartmentHash[g.compartmentDesignator] || [];
				for (c = 0; c < f.length; c += 1) {
					f[c].container.removeClass(d)
				}
				if (g.unitContainer) {
					g.unitContainer.hide()
				}
				g.isActive = false
			}
		};
		b.activateDeckHandler = function () {
			var c = b.getUnitMapContainer();
			if (c.activeCompartmentDeck !== this.deck) {
				c.activeCompartment.deactivate();
				b.activate()
			}
		};
		b.getKey = function () {
			var c = b.delimiter;
			var d = this.equipment.getKey() + c + this.compartmentDesignator + c + this.deck;
			return d
		};
		b.updateCompartmentHandler = function () {
			b.updateCompartment()
		};
		b.updateCompartment = function () {
			if (!this.isActive) {
				this.input.val(b.compartmentDesignator);
				this.deactivate();
				this.activate()
			}
		};
		b.addEvents = function () {
			a.addEvents.call(this);
			this.container.click(this.updateCompartmentHandler);
			this.compartmentContainer.mouseover(this.showDetailHandler);
			this.compartmentContainer.mouseout(this.hideDetailHandler);
			this.compartmentContainer.click(this.confirmSeatHandler)
		};
		b.confirmSeatHandler = function (d) {
			var e = null;
			d = d || window.event;
			e = d.target || d.srcElement;
			var f = e.id;
			if (f === "") {
				f = $(e).parent("div.aUnit").attr("id") || ""
			}
			var c = b.unitHash[f];
			if (c && c.confirmSeat) {
				c.confirmSeat()
			}
		};
		b.showDetailHandler = function (d) {
			var e = null;
			d = d || window.event;
			e = d.target || d.srcElement;
			var f = e.id;
			if (f === "") {
				f = $(e).parent("div.aUnit").attr("id") || ""
			}
			var c = b.unitHash[f];
			if (c && c.setTimeoutDetail) {
				c.setTimeoutDetail()
			}
		};
		b.hideDetailHandler = function (d) {
			var c = b.getUnitMapContainer();
			window.clearTimeout(c.showDetailId);
			if (SKYSALES.Class.Seat.prototype.unitDetail) {
				SKYSALES.Class.Seat.prototype.unitDetail.hide()
			}
		};
		b.getFlattenedPropertyTypeList = function () {
			return this.equipment.getFlattenedPropertyTypeList()
		};
		b.getNumericPropertyCodeList = function () {
			return this.equipment.getNumericPropertyCodeList()
		};
		b.getSsrCodeList = function () {
			return this.equipment.getSsrCodeList()
		};
		b.getNumericPropertyHash = function () {
			return this.equipment.getNumericPropertyHash()
		};
		b.setVars = function () {
			a.setVars.call(this);
			var c = b.delimiter;
			b.template = $("#" + this.templateId);
			b.compartmentContainer = $("#" + this.compartmentContainerId);
			b.input = $("#" + this.inputId);
			b.unitsAvailableSpan = $("#availableSeats" + c + "compartment" + c + this.compartmentDesignator + c + "deck" + c + this.deck);
			b.deckTabs = $("#" + this.deckTabsId);
			b.deckTab = $("#" + this.deckTabId + this.deck, this.deckTabs);
			b.deckTabLabel = $("#" + this.deckTabLabelId + this.deck, this.deckTab);
			b.deckTabA = $("#" + this.deckTabAId + this.deck, this.deckTab);
			delete b.templateId;
			delete b.compartmentContainerId;
			delete b.inputId;
			delete b.deckTabsId
		};
		return b
	};
	SKYSALES.Class.Compartment.compartmentKeyHash = {
		d : "deck",
		cd : "compartmentDesignator",
		l : "len",
		au : "availableUnits",
		s : "sequence",
		w : "width",
		u : "unitArray"
	}
}
if (SKYSALES.Class.Unit === undefined) {
	SKYSALES.Class.Unit = function () {
		var b = SKYSALES.Class.EquipmentBase();
		var a = SKYSALES.Util.extendObject(b);
		a.compartment = null;
		a.assignable = "";
		a.propertyArray = [];
		a.height = 0;
		a.compartmentDesignator = "";
		a.seatSet = "";
		a.propertyInts = [];
		a.ssrPermissionArray = [];
		a.travelClassCode = "";
		a.unitAngle = "";
		a.unitAvailability = "";
		a.unitDesignator = "";
		a.unitGroup = "";
		a.unitType = "";
		a.width = 0;
		a.x = "";
		a.y = "";
		a.zone = "";
		a.unitFee = "";
		a.unitKey = "";
		a.text = "";
		a.iconContentName = "";
		a.unitKey = null;
		a.unitTemplateId = "";
		a.unitTemplate = null;
		a.unitTemplateHtml = "";
		SKYSALES.Class.Unit.prototype.timeOut = 500;
		SKYSALES.Class.Unit.prototype.offset = 30;
		SKYSALES.Class.Unit.prototype.delimiter = "_";
		a.unitMapContainer = null;
		a.init = function (c) {
			var d = SKYSALES.Class.Unit.prototype.unitKeyHash;
			this.convertParamObject(d, c);
			this.setSettingsByObject(c);
			this.setVars();
			this.setUnitAvailability();
			delete a.init
		};
		a.setSettingsByObject = function (d) {
			b.setSettingsByObject.call(this, d);
			var c = this.getUnitMapContainer();
			a.unitFee = SKYSALES.Class.UnitFee.getUnitFee(this.unitGroup);
			a.compartmentDesignator = c.clean(this.compartmentDesignator);
			a.unitDesignator = c.clean(this.unitDesignator);
			this.compartmentDesignator = a.compartmentDesignator;
			this.unitDesignator = a.unitDesignator;
			a.unitKey = this.getKey();
			delete a.setSettingsByObject
		};
		a.setVars = function () {
			b.setVars.call(this);
			if (this.unitTemplateId) {
				a.unitTemplate = this.getById(this.unitTemplateId);
				a.unitTemplateHtml = this.unitTemplate.text()
			}
			delete a.unitTemplateId;
			delete a.setVars
		};
		a.getUnitHtml = function () {
			var f = this.getUnitMapContainer();
			var e = f.iconMax;
			var d = {
				templateHtml : this.unitTemplateHtml,
				propertyHtml : this.booleanPropertyUnitTemplateHtml,
				iconMax : e
			};
			var c = this.supplant(d);
			delete a.unitTemplateHtml;
			delete a.booleanPropertyUnitTemplateHtml;
			delete a.getUnitHtml;
			return c
		};
		a.setUnitAvailability = function () {
			if (!this.assignable) {
				this.unitAvailability = "Open"
			}
			if ((this.assignable) && (this.unitAvailability !== "Open")) {
				this.unitAvailability = "Reserved"
			}
			delete a.setUnitAvailability
		};
		a.draw = SKYSALES.Class.Unit.prototype.draw;
		a.getImagePath = SKYSALES.Class.Unit.prototype.getImagePath;
		a.updateSeatImage = SKYSALES.Class.Unit.prototype.updateSeatImage;
		a.getById = SKYSALES.Class.Unit.prototype.getById;
		a.getUnitMapContainer = SKYSALES.Class.Unit.prototype.getUnitMapContainer;
		a.getKey = SKYSALES.Class.Unit.prototype.getKey;
		a.getFlattenedPropertyTypeList = SKYSALES.Class.Unit.prototype.getFlattenedPropertyTypeList;
		a.getNumericPropertyCodeList = SKYSALES.Class.Unit.prototype.getNumericPropertyCodeList;
		a.getSsrCodeList = SKYSALES.Class.Unit.prototype.getSsrCodeList;
		a.getNumericPropertyHash = SKYSALES.Class.Unit.prototype.getNumericPropertyHash;
		a.supplant = SKYSALES.Class.Unit.prototype.supplant;
		return a
	};
	SKYSALES.Class.Unit.prototype.draw = function () {
		var a = this.getKey();
		this.container = this.getById(a);
		this.updateSeatImage()
	};
	SKYSALES.Class.Unit.prototype.getImagePath = function (e) {
		e = e || this.unitAvailability;
		var b = SKYSALES.Class.Unit.prototype.delimiter;
		var d = "JetAircraft";
		if (this.compartment && this.compartment.equipment) {
			d = this.compartment.equipment.equipmentCategory
		}
		var c = this.getUnitMapContainer();
		var a = "";
		if ((this.unitGroup == 2 || this.unitGroup == 22) && e != "Reserved") {
		    a = c.equipmentImagePath + d + b + this.unitType + b + e + b + this.unitAngle + b + "HS.gif"
		} else if ((this.unitGroup == 7 || this.unitGroup == 21) && e != "Reserved") {
		    a = c.equipmentImagePath + d + b + this.unitType + b + e + b + this.unitAngle + b + "AB.gif"
		} else if ((this.unitGroup == 8 || this.unitGroup == 25) && e != "Reserved") {
		    a = c.equipmentImagePath + d + b + this.unitType + b + e + b + this.unitAngle + b + "AW.gif"
		} else {
		    a = c.equipmentImagePath + d + b + this.unitType + b + e + b + this.unitAngle + ".gif"
		}
		//if (this.unitGroup != 2 || e == "Reserved") {
		//	a = c.equipmentImagePath + d + b + this.unitType + b + e + b + this.unitAngle + ".gif"
		//} else {
		//	a = c.equipmentImagePath + d + b + this.unitType + b + e + b + this.unitAngle + b + "HS.gif"
		//}
		return a
	};
	SKYSALES.Class.Unit.prototype.updateSeatImage = function (b) {
		b = b || this.unitAvailability;
		var a = this.getImagePath(b);
		this.container.css("background-image", "url(" + a + ")")
	};
	SKYSALES.Class.Unit.prototype.getById = function (b) {
		var a = null;
		if (b) {
			a = window.document.getElementById(b)
		}
		if (!a) {
			a = $([])
		}
		return $(a)
	};
	SKYSALES.Class.Unit.prototype.getUnitMapContainer = function () {
		if ((!this.unitMapContainer) && this.compartment) {
			this.unitMapContainer = this.compartment.getUnitMapContainer()
		}
		return this.unitMapContainer
	};
	SKYSALES.Class.Unit.prototype.getKey = function () {
		var a = SKYSALES.Class.Unit.prototype.delimiter;
		if (!this.unitKey) {
			this.unitKey = this.compartment.getKey() + a + this.unitDesignator
		}
		return this.unitKey
	};
	SKYSALES.Class.Unit.prototype.getFlattenedPropertyTypeList = function () {
		return this.compartment.getFlattenedPropertyTypeList()
	};
	SKYSALES.Class.Unit.prototype.getNumericPropertyCodeList = function () {
		return this.compartment.getNumericPropertyCodeList()
	};
	SKYSALES.Class.Unit.prototype.getSsrCodeList = function () {
		return this.compartment.getSsrCodeList()
	};
	SKYSALES.Class.Unit.prototype.getNumericPropertyHash = function () {
		return this.compartment.getNumericPropertyHash()
	};
	SKYSALES.Class.Unit.prototype.supplant = function (g) {
	    //var seatArray = [15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 29, 30, 31];
	    var seatNo = this.unitDesignator.substr(0, this.unitDesignator.length - 1);
	    var splitFee = this.unitFee.toString().split(" ");
	    var seatFee = "";
	    var seatCurr = "";

        if (splitFee.length >= 1) seatFee = splitFee[0] * 2;
        if (splitFee.length >= 2) seatCurr = splitFee[1];

		var c = g.templateHtml || "";
		var e = this.getUnitMapContainer() || {};
		var d = e.grid || 1;
		c = c.replace("[unitKey]", this.getKey());
		if (e.activeUnitMap) {
			var f = e.activeUnitMap.equipment.equipmentCategory;
			var b = new SKYSALES.Class.ComparmentUnitDesignator();
			b.equipmentCategory = f;
			b.delimiter = "_";
			b.delimiterDisplay = "-";
			b.parseDesignator(this.getKey());
			b.init();
			c = c.replace(/\[compartmentUnitDesignator\]/g, b.display)
		}
		c = c.replace(/\[unitDesignator\]/g, this.unitDesignator);
		c = c.replace("[unitAvailability]", this.unitAvailability);
		c = c.replace("[unitGroup]", this.unitGroup);

		c = c.replace("[unitFee]", this.unitFee);

	    //added by diana 20170125, to put discounted fee, add condition is international flight
		var IsIntFlight = document.getElementById("ctl00_ContentPlaceHolder2_hfIsInternational");
		//if (IsIntFlight != null && IsIntFlight != "undefined" && IsIntFlight.value == "TRUE" && this.unitGroup == 1) //!= 2 && seatArray.indexOf(parseInt(seatNo)) >= 0)
		//    c = c.replace("[unitOriginFee]", "<strike>" + (parseFloat(seatFee).toFixed(2) + " " + seatCurr) + "</strike>");
		//else
		//    c = c.replace("[unitOriginFee]", "");

		var unitArray = [1, 2, 7, 8];
		if (IsIntFlight != null && IsIntFlight != "undefined" && IsIntFlight.value == "TRUE" && unitArray.indexOf(parseInt(this.unitGroup)) < 0)
		{
		    c = c.replace("[unitRemark]", "If you do not agree with this <br />price, kindly contact Group Desk <br /> for enquiries");
		}
		else {
		    c = c.replace("[unitRemark]", "");
		    c = c.replace("[unitRemark2]", "");
		}

		c = c.replace("[unitAngle]", this.unitAngle);
		c = c.replace("[text]", this.text);
		c = c.replace("[width]", this.width * d);
		c = c.replace("[height]", this.height * d);
		c = c.replace("[x]", this.x * d);
		c = c.replace("[y]", this.y * d);
		var a = e.getIconImageUri(this.iconContentName);
		c = c.replace("[IconContentName]", a);
		return c
	};
	SKYSALES.Class.Unit.prototype.unitKeyHash = {
		a : "assignable",
		h : "height",
		cl : "cabotageLevel",
		cau : "carAvailableUnits",
		cd : "compartmentDesignator",
		ss : "seatSet",
		pi : "propertyInts",
		cw : "criterionWeight",
		pri : "priority",
		pb : "propertyArray",
		ssau : "seatSetAvailableUnits",
		spb : "ssrPermissionArray",
		ssmc : "ssrSeatMapCode",
		tcc : "travelClassCode",
		an : "unitAngle",
		ua : "unitAvailability",
		ud : "unitDesignator",
		ug : "unitGroup",
		ut : "unitType",
		w : "width",
		x : "x",
		y : "y",
		z : "zone",
		t : "text",
		icn : "iconContentName"
	}
}
if (SKYSALES.Class.Seat === undefined) {
	SKYSALES.Class.Seat = function () {
		var a = SKYSALES.Class.Unit();
		var b = SKYSALES.Util.extendObject(a);
		b.unitTemplateId = "seatId";
		b.booleanPropertyHash = {};
		b.getUnitHtml = function () {
			var f = this.getUnitMapContainer();
			var e = f.iconMax;
			var d = {
				templateHtml : this.unitTemplateHtml,
				propertyHtml : SKYSALES.Class.Seat.prototype.booleanPropertyUnitTemplateHtml,
				iconMax : e
			};
			var c = this.supplant(d);
			delete b.unitTemplateHtml;
			delete b.getUnitHtml;
			return c
		};
		b.showDetailHandler = function () {
			b.setTimeoutDetail()
		};
		b.hideDetailHandler = function () {
			b.hideDetail()
		};
		b.showDetailTimeoutHandler = function () {
			b.showDetail()
		};
		b.confirmSeat = function () {
			var c = b.getUnitMapContainer();
			c.confirmSeat(b)
		};
		b.setVars = function () {
			a.setVars.call(this);
			if (!SKYSALES.Class.Seat.prototype.ranSetVars) {
				SKYSALES.Class.Seat.prototype.ranSetVars = true;
				SKYSALES.Class.Seat.prototype.unitDetail = this.getById("unitTipId");
				SKYSALES.Class.Seat.prototype.positionContainer = this.getById("unitMapView");
				SKYSALES.Class.Seat.prototype.booleanPropertyUnitTemplate = this.getById("booleanPropertyUnitId");
				SKYSALES.Class.Seat.prototype.booleanPropertyUnitTipTemplate = this.getById("booleanPropertyUnitTipId");
				SKYSALES.Class.Seat.prototype.numericPropertyUnitTipTemplate = this.getById("numericPropertyUnitTipId");
				SKYSALES.Class.Seat.prototype.unitDetailHtml = SKYSALES.Class.Seat.prototype.unitDetail.text();
				SKYSALES.Class.Seat.prototype.booleanPropertyUnitTemplateHtml = SKYSALES.Class.Seat.prototype.booleanPropertyUnitTemplate.text();
				SKYSALES.Class.Seat.prototype.booleanPropertyUnitTipTemplateHtml = SKYSALES.Class.Seat.prototype.booleanPropertyUnitTipTemplate.text();
				SKYSALES.Class.Seat.prototype.numericPropertyUnitTipTemplateHtml = SKYSALES.Class.Seat.prototype.numericPropertyUnitTipTemplate.text()
			}
			delete b.setVars
		};
		b.supplant = SKYSALES.Class.Seat.prototype.supplant;
		b.getSsrPermissionHtml = SKYSALES.Class.Seat.prototype.getSsrPermissionHtml;
		b.getPropertyHtml = SKYSALES.Class.Seat.prototype.getPropertyHtml;
		b.getNumericPropertyHtml = SKYSALES.Class.Seat.prototype.getNumericPropertyHtml;
		b.showDetail = SKYSALES.Class.Seat.prototype.showDetail;
		b.hasProperty = SKYSALES.Class.Seat.prototype.hasProperty;
		b.hasNumericProperty = SKYSALES.Class.Seat.prototype.hasNumericProperty;
		b.hasSsr = SKYSALES.Class.Seat.prototype.hasSsr;
		b.setTimeoutDetail = SKYSALES.Class.Seat.prototype.setTimeoutDetail;
		return b
	};
	SKYSALES.Class.Seat.prototype.supplant = function (h) {
		var b = h.templateHtml || "";
		var d = h.propertyHtml || "";
		var g = h.numericPropertyHtml || "";
		var c = h.iconMax || -1;
		var a = "";
		var f = "";
		var e = "";
		b = SKYSALES.Class.Unit.prototype.supplant.call(this, h);
		if (b.indexOf("[propertyArray]") > -1) {
			f = this.getPropertyHtml(d, c);
			b = b.replace("[propertyArray]", f)
		}
		if (b.indexOf("[numericPropertyArray]") > -1) {
			e = this.getNumericPropertyHtml(g);
			b = b.replace("[numericPropertyArray]", e)
		}
		if (b.indexOf("[ssrPermissionArray]") > -1) {
			a = this.getSsrPermissionHtml(d);
			b = b.replace("[ssrPermissionArray]", a)
		}
		return b
	};
	SKYSALES.Class.Seat.prototype.getPropertyHtml = function (n, g) {
		var a = this.getUnitMapContainer();
		var c = this.getFlattenedPropertyTypeList() || [];
		var o = 0;
		var j = this.unitType;
		var e = "property_" + j + "_";
		var m = "";
		var h = "";
		var b = 0;
		var f = [];
		var k = null;
		var l = false;
		for (b = 0; b < c.length; b += 1) {
			k = c[b];
			l = this.hasProperty(b);
			if (l) {
				f[f.length] = k
			}
		}
		f.sort(a.sortPropertyList);
		for (b = 0; b < f.length; b += 1) {
			k = f[b];
			m = n;
			m = window.decodeURI(m);
			m = m.replace("[name]", k.name);
			var d = a.getIconImageUri(k.iconContentName);
			if (d != a.equipmentImagePath + a.blankImageName) {
				m = m.replace("[IconContentName]", d);
				m = m.replace("[className]", e + o)
			} else {
				m = ""
			}
			h += m;
			o += 1;
			if (o === g) {
				break
			}
		}
		return h
	};
	SKYSALES.Class.Seat.prototype.getNumericPropertyHtml = function (h) {
		var e = 0;
		var a = this.getNumericPropertyCodeList() || [];
		var g = this.getNumericPropertyHash() || {};
		var d = "";
		var b = null;
		var f = "";
		var c = "";
		var j = false;
		for (e = 0; e < a.length; e += 1) {
			j = this.hasNumericProperty(e);
			if (j) {
				d = a[e];
				b = g[d];
				if (b) {
					f = h;
					f = window.decodeURI(f);
					f = f.replace("[name]", b.name);
					f = f.replace("[value]", this.propertyInts[e]);
					c += f
				}
			}
		}
		return c
	};
	SKYSALES.Class.Seat.prototype.getSsrPermissionHtml = function (k) {
		var f = this.getUnitMapContainer();
		var e = this.getSsrCodeList() || [];
		var g = 0;
		var j = f.ssrHash || {};
		var l = "";
		var b = "";
		var d = "";
		var c = false;
		var a = null;
		var h = f.getIconImageUri();
		for (g = 0; g < e.length; g += 1) {
			c = this.hasSsr(g);
			if (c) {
				d = e[g];
				a = j[d];
				if (a && a.ssrCode) {
					l = k;
					l = window.decodeURI(l);
					l = l.replace(/\[IconContentName\]/, h);
					l = l.replace(/\[name\]/g, a.name);
					b += l
				}
			}
		}
		return b
	};
	SKYSALES.Class.Seat.prototype.showDetail = function () {
		var c = {
			templateHtml : SKYSALES.Class.Seat.prototype.unitDetailHtml,
			propertyHtml : SKYSALES.Class.Seat.prototype.booleanPropertyUnitTipTemplateHtml,
			numericPropertyHtml : SKYSALES.Class.Seat.prototype.numericPropertyUnitTipTemplateHtml
		};
		var a = this.supplant(c);
		var b = SKYSALES.Class.Seat.prototype.unitDetail;
		b.html(a);
		var g = SKYSALES.Dhtml();
		var f = g.getX(this.container.get(0));
		var e = g.getY(this.container.get(0));
		var d = SKYSALES.Class.Seat.prototype.positionContainer.get(0);
		f -= d.scrollLeft;
		e -= d.scrollTop;
		b.css("left", f + SKYSALES.Class.Unit.prototype.offset);
		b.css("top", e + SKYSALES.Class.Unit.prototype.offset);
		b.show()
	};
	SKYSALES.Class.Seat.prototype.hasProperty = function (b) {
		var a = false;
		if (b === null) {
			b = -1
		}
		if (b > -1) {
			if (((this.propertyArray[b >> 5] >> (b % 32)) & 1) === 1) {
				a = true
			}
		}
		return a
	};
	SKYSALES.Class.Seat.prototype.hasNumericProperty = function (c) {
		var a = true;
		var b = -32769;
		if (c === null) {
			c = -1
		}
		if (c > -1) {
			if (this.propertyInts[c] === b) {
				a = false
			}
		}
		return a
	};
	SKYSALES.Class.Seat.prototype.hasSsr = function (a) {
		var b = false;
		if (a === null) {
			a = -1
		}
		if (a > -1) {
			if (((this.ssrPermissionArray[a >> 5] >> (a % 32)) & 1) === 1) {
				b = true
			}
		}
		return b
	};
	SKYSALES.Class.Seat.prototype.setTimeoutDetail = function () {
		var a = this.getUnitMapContainer();
		if (a.showDetailId) {
			window.clearTimeout(a.showDetailId)
		}
		a.showDetailId = window.setTimeout(this.showDetailTimeoutHandler, SKYSALES.Class.Unit.prototype.timeOut)
	}
}
if (SKYSALES.Class.LargeSeat === undefined) {
	SKYSALES.Class.LargeSeat = function () {
		var b = SKYSALES.Class.Seat();
		var a = SKYSALES.Util.extendObject(b);
		a.unitTemplateId = "largeSeatId";
		a.confirmSeat = function () {
			var c = a.getUnitMapContainer();
			c.confirmSeat(a)
		};
		a.showDetailHandler = function () {
			a.setTimeoutDetail()
		};
		a.showDetailTimeoutHandler = function () {
			b.showDetail.call(a)
		};
		a.hideDetailHandler = function () {
			b.hideDetail.call(a)
		};
		a.getUnitHtml = function () {
			var f = this.getUnitMapContainer();
			var e = f.iconMaxLargeSeat;
			var d = {
				templateHtml : this.unitTemplateHtml,
				propertyHtml : SKYSALES.Class.Seat.prototype.booleanPropertyUnitTemplateHtml,
				iconMax : e
			};
			var c = this.supplant(d);
			delete a.getUnitHtml;
			return c
		};
		return a
	}
}
if (SKYSALES.Class.Chouchette === undefined) {
	SKYSALES.Class.Chouchette = function () {
		var b = SKYSALES.Class.Seat();
		var a = SKYSALES.Util.extendObject(b);
		a.unitTemplateId = "chouchetteId";
		a.confirmSeat = function () {
			var c = a.getUnitMapContainer();
			c.confirmSeat(a)
		};
		a.showDetailHandler = function () {
			a.setTimeoutDetail()
		};
		a.showDetailTimeoutHandler = function () {
			b.showDetail.call(a)
		};
		a.hideDetailHandler = function () {
			b.hideDetail.call(a)
		};
		return a
	}
}
if (SKYSALES.Class.BedOneOfOne === undefined) {
	SKYSALES.Class.BedOneOfOne = function () {
		var a = SKYSALES.Class.Seat();
		var b = SKYSALES.Util.extendObject(a);
		b.unitTemplateId = "bedOneOfOneId";
		b.confirmSeat = function () {
			var c = b.getUnitMapContainer();
			c.confirmSeat(b)
		};
		b.showDetailHandler = function () {
			b.setTimeoutDetail()
		};
		b.showDetailTimeoutHandler = function () {
			a.showDetail.call(b)
		};
		b.hideDetailHandler = function () {
			a.hideDetail.call(b)
		};
		return b
	}
}
if (SKYSALES.Class.BedOneOfTwo === undefined) {
	SKYSALES.Class.BedOneOfTwo = function () {
		var a = SKYSALES.Class.Seat();
		var b = SKYSALES.Util.extendObject(a);
		b.unitTemplateId = "bedOneOfTwoId";
		b.confirmSeat = function () {
			var c = b.getUnitMapContainer();
			c.confirmSeat(b)
		};
		b.showDetailHandler = function () {
			b.setTimeoutDetail()
		};
		b.showDetailTimeoutHandler = function () {
			a.showDetail.call(b)
		};
		b.hideDetailHandler = function () {
			a.hideDetail.call(b)
		};
		return b
	}
}
if (SKYSALES.Class.BedTwoOfTwo === undefined) {
	SKYSALES.Class.BedTwoOfTwo = function () {
		var b = SKYSALES.Class.Seat();
		var a = SKYSALES.Util.extendObject(b);
		a.unitTemplateId = "bedTwoOfTwoId";
		a.confirmSeat = function () {
			var c = a.getUnitMapContainer();
			c.confirmSeat(a)
		};
		a.showDetailHandler = function () {
			a.setTimeoutDetail()
		};
		a.showDetailTimeoutHandler = function () {
			b.showDetail.call(a)
		};
		a.hideDetailHandler = function () {
			b.hideDetail.call(a)
		};
		return a
	}
}
if (SKYSALES.Class.BedOneOfThree === undefined) {
	SKYSALES.Class.BedOneOfThree = function () {
		var a = SKYSALES.Class.Seat();
		var b = SKYSALES.Util.extendObject(a);
		b.unitTemplateId = "bedOneOfThreeId";
		b.confirmSeat = function () {
			var c = b.getUnitMapContainer();
			c.confirmSeat(b)
		};
		b.showDetailHandler = function () {
			b.setTimeoutDetail()
		};
		b.showDetailTimeoutHandler = function () {
			a.showDetail.call(b)
		};
		b.hideDetailHandler = function () {
			a.hideDetail.call(b)
		};
		return b
	}
}
if (SKYSALES.Class.BedTwoOfThree === undefined) {
	SKYSALES.Class.BedTwoOfThree = function () {
		var b = SKYSALES.Class.Seat();
		var a = SKYSALES.Util.extendObject(b);
		a.unitTemplateId = "bedTwoOfThreeId";
		a.confirmSeat = function () {
			var c = a.getUnitMapContainer();
			c.confirmSeat(a)
		};
		a.showDetailHandler = function () {
			a.setTimeoutDetail()
		};
		a.showDetailTimeoutHandler = function () {
			b.showDetail.call(a)
		};
		a.hideDetailHandler = function () {
			b.hideDetail.call(a)
		};
		return a
	}
}
if (SKYSALES.Class.BedThreeOfThree === undefined) {
	SKYSALES.Class.BedThreeOfThree = function () {
		var a = SKYSALES.Class.Seat();
		var b = SKYSALES.Util.extendObject(a);
		b.unitTemplateId = "bedThreeOfThreeId";
		b.confirmSeat = function () {
			var c = b.getUnitMapContainer();
			c.confirmSeat(b)
		};
		b.showDetailHandler = function () {
			b.setTimeoutDetail()
		};
		b.showDetailTimeoutHandler = function () {
			a.showDetail.call(b)
		};
		b.hideDetailHandler = function () {
			a.hideDetail.call(b)
		};
		return b
	}
}
if (SKYSALES.Class.GenericUnit === undefined) {
	SKYSALES.Class.GenericUnit = function () {
		var a = SKYSALES.Class.Unit();
		var b = SKYSALES.Util.extendObject(a);
		b.unitTemplateId = "genericUnitId";
		b.getImagePath = function () {
			var d = this.getUnitMapContainer();
			var c = d.genericUnitImagePath + this.iconContentName + ".gif";
			return c
		};
		return b
	}
}
if (SKYSALES.Class.Window === undefined) {
	SKYSALES.Class.Window = function () {
		var b = SKYSALES.Class.Unit();
		var a = SKYSALES.Util.extendObject(b);
		a.unitTemplateId = "windowId";
		return a
	}
}
if (SKYSALES.Class.LabelRuler === undefined) {
	SKYSALES.Class.LabelRuler = function () {
		var a = SKYSALES.Class.Unit();
		var b = SKYSALES.Util.extendObject(a);
		b.unitTemplateId = "labelRulerId";
		return b
	}
}
if (SKYSALES.Class.Table === undefined) {
	SKYSALES.Class.Table = function () {
		var b = SKYSALES.Class.Unit();
		var a = SKYSALES.Util.extendObject(b);
		a.unitTemplateId = "tableId";
		return a
	}
}
if (SKYSALES.Class.Wall === undefined) {
	SKYSALES.Class.Wall = function () {
		var a = SKYSALES.Class.Unit();
		var b = SKYSALES.Util.extendObject(a);
		b.unitTemplateId = "wallId";
		return b
	}
}
if (SKYSALES.Class.Exit === undefined) {
	SKYSALES.Class.Exit = function () {
		var b = SKYSALES.Class.Unit();
		var a = SKYSALES.Util.extendObject(b);
		a.unitTemplateId = "exitId";
		return a
	}
}
if (SKYSALES.Class.ComparmentUnitDesignator === undefined) {
	SKYSALES.Class.ComparmentUnitDesignator = function () {
		var a = this;
		a.equipmentId = "";
		a.compartmentDesignator = "";
		a.unitDesignator = "";
		a.deck = "";
		a.compartmentUnitDesignator = "";
		a.display = "";
		a.equipmentCategory = "";
		a.delimiter = "_";
		a.delimiterDisplay = "-";
		a.parseDesignator = function (b) {
			var c = b.split(a.delimiter);
			if (4 === c.length) {
				a.equipmentId = c[0];
				a.compartmentDesignator = c[1];
				a.deck = c[2];
				a.unitDesignator = c[3]
			}
		};
		a.init = function () {
			a.compartmentUnitDesignator = a.compartmentDesignator + a.delimiterDisplay + a.unitDesignator;
			if ("Train" === a.equipmentCategory) {
				a.display = a.compartmentUnitDesignator
			} else {
				a.display = a.unitDesignator
			}
		}
	}
}
if (SKYSALES.Class.Door === undefined) {
	SKYSALES.Class.Door = function () {
		var a = SKYSALES.Class.Unit();
		var b = SKYSALES.Util.extendObject(a);
		b.unitTemplateId = "doorId";
		return b
	}
}
if (SKYSALES.Class.Stairs === undefined) {
	SKYSALES.Class.Stairs = function () {
		var b = SKYSALES.Class.Unit();
		var a = SKYSALES.Util.extendObject(b);
		a.unitTemplateId = "stairsId";
		return a
	}
}
if (SKYSALES.Class.BulkHead === undefined) {
	SKYSALES.Class.BulkHead = function () {
		var b = SKYSALES.Class.Unit();
		var a = SKYSALES.Util.extendObject(b);
		a.unitTemplateId = "bulkHeadId";
		return a
	}
}
if (SKYSALES.Class.Wing === undefined) {
	SKYSALES.Class.Wing = function () {
		var a = SKYSALES.Class.Unit();
		var b = SKYSALES.Util.extendObject(a);
		b.unitTemplateId = "wingId";
		return b
	}
}
if (SKYSALES.Class.WingLeft === undefined) {
	SKYSALES.Class.WingLeft = function () {
		var b = SKYSALES.Class.Unit();
		var a = SKYSALES.Util.extendObject(b);
		a.unitTemplateId = "wingLeftId";
		a.setSettingsByObject = function (e) {
			b.setSettingsByObject.call(this, e);
			var d = this.getUnitMapContainer();
			var c = d.grid;
			a.x = (((a.x * c) - 120) / c)
		};
		a.updateSeatImage = function (c) {};
		return a
	}
}
if (SKYSALES.Class.WingRight === undefined) {
	SKYSALES.Class.WingRight = function () {
		var b = SKYSALES.Class.Unit();
		var a = SKYSALES.Util.extendObject(b);
		a.unitTemplateId = "wingRightId";
		a.setSettingsByObject = function (e) {
			b.setSettingsByObject.call(this, e);
			var d = this.getUnitMapContainer();
			var c = d.grid;
			a.x = (((a.x * c) + 10) / c)
		};
		a.updateSeatImage = function (c) {};
		return a
	}
}
if (SKYSALES.Class.SeatCompartment === undefined) {
	SKYSALES.Class.SeatCompartment = function () {
		var b = SKYSALES.Class.Unit();
		var a = SKYSALES.Util.extendObject(b);
		a.unitTemplateId = "seatCompartmentId";
		a.updateSeatImage = function (c) {};
		return a
	}
}
if (SKYSALES.Class.Lavatory === undefined) {
	SKYSALES.Class.Lavatory = function () {
		var a = SKYSALES.Class.Unit();
		var b = SKYSALES.Util.extendObject(a);
		b.unitTemplateId = "lavatoryId";
		return b
	}
}
if (SKYSALES.Class.Luggage === undefined) {
	SKYSALES.Class.Luggage = function () {
		var a = SKYSALES.Class.Unit();
		var b = SKYSALES.Util.extendObject(a);
		b.unitTemplateId = "luggageId";
		return b
	}
}
if (SKYSALES.Class.UnitInput === undefined) {
    SKYSALES.Class.UnitInput = function () {
        var b = SKYSALES.Class.UnitContainer();
        var a = SKYSALES.Util.extendObject(b);
        a.inputId = "";
        a.hiddenInputId = "";
        a.input = null;
        a.hiddenInput = null;
        a.tripInputId = "";
        a.tripInput = null;
        a.passengerNumber = -1;
        a.unitKey = "";
        a.equipmentIndex = "";
        a.passengerName = "";
        a.isActive = false;
        a.journeyIndex = -1;
        a.segmentIndex = -1;
        a.departureStation = "";
        a.arrivalStation = "";
        a.segmentDepartureStation = "";
        a.segmentArrivalStation = "";
        a.selectedFilterPropertyArray = null;
        a.selectedAutoAssignPropertyArray = null;
        a.selectedFilterSsrArray = null;
        a.nextUnitInput = null;
        a.paxSeatFeeId = "";
        a.paxSeatFee = null;
        a.activateId = "";
        a.activate = null;
        a.delimiter = "_";
        a.delimiterDisplay = "-";
        a.unitMapContainer = null;
        a.unfulfilledPropertyArray = null;
        a.notSeatedTogether = "";
        a.paxBday = "";
        a.blockedSSRExists = false;
        a.inputFee = null;
        a.removeSeat = null;
        a.reselectSeat = null;
        a.seatFeeLabel = null;
		a.hiddenIsHot = "";
        a.init = function (c) {
            this.setSettingsByObject(c);
            this.setVars();
            this.makeReadOnly();
            this.writePaxSeatFee();
            this.initInput();
            this.addEvents()
        };
        a.writePaxSeatFee = function () {
            var c = a.unitMapContainer.getUnitByUnitKey(a.unitKey);
            if (c) {
                if (parseFloat(c.unitFee) > 0) {
                    this.inputFee.text(c.unitFee)
                }
                this.paxSeatFee.html(c.unitFee)
            }
            if (this.seatFeeLabel.is(":hidden")) {
                this.removeSeat.addClass("hidden")
            }
        };
        a.showInputFee = function (c) {
            if (parseFloat(c) > 0) {
                this.inputFee.text(c);
                $("#" + this.inputId + "_HiddenFee").val(c);
                $("#" + this.inputId + "_Fee").val(c);
            } else {
                this.inputFee.text(this.unitMapContainer.freeLabel);
                $("#" + this.inputId + "_HiddenFee").val("");
                $("#" + this.inputId + "_Fee").val("");
            }
            UpdateTotalAmount(1, this.inputFee.text());
            if (this.seatFeeLabel.is(":hidden")) {
                this.seatFeeLabel.removeClass("hidden");
                this.inputFee.removeClass("hidden");
                this.removeSeat.removeClass("hidden")
            }
        };
        a.removeInputFee = function () {
            this.inputFee.addClass("hidden");
            UpdateTotalAmount(0, this.inputFee.text());
            $("#" + this.inputId + "_HiddenFee").val("");
            this.seatFeeLabel.addClass("hidden");
            this.removeSeat.addClass("hidden")
        };
        a.makeReadOnly = function () {
            this.input.attr("readonly", "readonly")
        };
        a.updateActiveUnitInput = function () {
            a.tripInput.val(a.equipmentIndex);
            a.unitMapContainer.writeFilterToDom(a.UnitMapContainer.activeUnitInput);

            // when click textbox, call here
            a.unitMapContainer.updateActiveUnitInput(a)
        };
        a.deactivate = function () {
            var c = a.unitMapContainer.activeUnitInputClass;
            if (this.input) {
                this.input.removeClass(c)
            }
        };
        a.addEvents = function () {
            b.addEvents.call(this);
            this.input.click(this.updateActiveUnitInput);
            var c = a.unitMapContainer.getUnitByUnitKey(this.unitKey);
            this.activate.click(this.activateUnitInputHandler);
            this.removeSeat.click(this.removeUnitInput);
            this.reselectSeat.click(this.updateActiveUnitInput)
        };
        a.removeUnitInput = function () {
            var c = a.unitMapContainer.getUnitByUnitKey(a.unitKey);
            var d = a.unitMapContainer.activeUnit;
            if (c != null && d == c) {
                a.unitMapContainer.confirmSeat(d)
            } else {
                a.input.val("");
                a.hiddenInput.val("");
                a.removeInputFee();
                a.unitKey = "";
                if (c != null && c.updateSeatImage) {
                    c.unitAvailability = "Open";
                    c.updateSeatImage()
                }
            }
        };
        a.activateUnitInputHandler = function () {
            a.tripInput.val(a.equipmentIndex);
            a.unitMapContainer.activateUnitInput(a)
        };
        a.setInput = function (d) {
			
            var e = this.unitMapContainer.unitMapArray[this.equipmentIndex].equipmentCategory;
            var c = new SKYSALES.Class.ComparmentUnitDesignator();
            c.equipmentCategory = e;
            c.delimiter = a.delimiter;
            c.delimiterDisplay = a.delimiterDisplay;
            c.parseDesignator(d);
            c.init();
            this.input.val(c.display)
        };
        a.initInput = function () {
			var c = this.hiddenInput.val();
            if (undefined !== c) {
                this.setInput(c)
            }
        };
        a.setVars = function () {
			//test alert 
			//alert(this.inputId + this.hiddenInputId + this.tripInputId + " hot seat:" + this.hiddenIsHot);
            b.setVars.call(this);
            a.input = $("#" + this.inputId);
            a.inputFee = $("#" + this.inputId + "_Fee");
            a.reselectSeat = $("#" + this.hiddenInputId + "_Reselect");
            a.removeSeat = $("#" + this.hiddenInputId + "_Remove");
            a.seatFeeLabel = $("#" + this.inputId + "_SeatFeeLabel");
            a.hiddenInput = $("#" + this.hiddenInputId);
            a.tripInput = $("#" + this.tripInputId);
            a.paxSeatFee = $("#" + this.paxSeatFeeId);
            a.activate = $("#" + this.activateId);
			//added new
			a.hiddenIsHot = $("#" + this.hiddenIsHot)
        };

        Array.prototype.inArray = function (value) {
            // Returns true if the passed value is found in the
            // array. Returns false if it is not.
            var i;
            for (i = 0; i < this.length; i++) {
                if (this[i] == value) {
                    return true;
                }
            }
            return false;
        };

        function calseat(xx) {
            var arr = new Array(); //"12A","12B","12C","12D","12E","12F","14A","14B","14C","14D","14E","14F","1A","1B","1C","1D","1E","1F","2A","2B","2C","2D","2E","2F","3A","3B","3C","3D","3E","3F","4A","4B","4C","4D","4E","4F","5A","5B","5C","5D","5E","5F");
            if (arr.inArray(xx)) {
                return true;
            }
            else {
                return false;
            }
        }

        a.setInputAndHiddenInput = function (c) {
			this.hiddenInput.val(c);
			this.setInput(c)
        };
        a.supplant = function (c) {
            c = window.decodeURI(c);
            c = c.replace("[passengerName]", this.passengerName);
            c = c.replace("[segmentDepartureStation]", this.segmentDepartureStation);
            c = c.replace("[segmentArrivalStation]", this.segmentArrivalStation);
            return c
        };
        a.getKey = function () {
            var c = this.delimiter;
            var d = this.equipmentIndex + c + this.passengerNumber;
			
            return d
        };
        a.getSegmentKey = function () {
            var c = this.delimiter;
            var d = this.journeyIndex + c + this.segmentIndex + c + this.passengerNumber;
            return d
        };
		//added new function
		a.getOriSeat = function () {
            var c = this.hiddenInput.val();
			var e = this.unitMapContainer.unitMapArray[this.equipmentIndex].equipmentCategory;
            var cc = new SKYSALES.Class.ComparmentUnitDesignator();
            cc.equipmentCategory = e;
            cc.delimiter = a.delimiter;
            cc.delimiterDisplay = a.delimiterDisplay;
            cc.parseDesignator(c);
            cc.init();
			
            return cc.display
        };
		a.validSeat = function (c) {
			var h = this.hiddenIsHot.val();
			if (h == 1) {
				return true;
			} else {
				var e = this.unitMapContainer.unitMapArray[this.equipmentIndex].equipmentCategory;
				var cc = new SKYSALES.Class.ComparmentUnitDesignator();
				cc.equipmentCategory = e;
				cc.delimiter = a.delimiter;
				cc.delimiterDisplay = a.delimiterDisplay;
				cc.parseDesignator(c);
				cc.init();
				//validateion added
				if (calseat(cc.display)) {
					return false;
				} else {
					return true;
				}
			}
			return false;
		}
        return a
    }
}
if (SKYSALES.Class.SoldSsr === undefined) {
	SKYSALES.Class.SoldSsr = function () {
		var a = SKYSALES.Class.Base();
		var b = SKYSALES.Util.extendObject(a);
		b.ssrCode = "";
		b.journeyIndex = -1;
		b.segmentIndex = -1;
		b.passengerNumber = -1;
		b.quantity = 1;
		b.delimiter = "_";
		b.init = function (c) {
			this.setSettingsByObject(c)
		};
		b.isSelected = function () {
			var c = b.getInput();
			return c.is(":checked")
		};
		b.getInput = function () {
			return $("#" + b.ssrCode + this.delimiter + "checkbox")
		};
		b.getKey = function () {
			var c = this.delimiter;
			var d = b.ssrCode + c + b.journeyIndex + c + b.segmentIndex + c + b.passengerNumber;
			return d
		};
		return b
	}
}
if (SKYSALES.Class.Ssr === undefined) {
	SKYSALES.Class.Ssr = function () {
		var a = SKYSALES.Class.Base();
		var b = SKYSALES.Util.extendObject(a);
		b.allowed = 1;
		b.boardingZone = 0;
		b.feeCode = "";
		b.limitPerPassenger = 0;
		b.name = 0;
		b.seatMapCode = "";
		b.seatRestriction = "";
		b.ssrCode = "";
		b.ssrNestCode = "";
		b.ssrType = "";
		b.traceQueueCode = "";
		b.unitValue = 0;
		b.init = function (c) {
			this.setSettingsByObject(c)
		};
		b.supplant = function (c) {
			c = window.decodeURI(c);
			c = c.replace(/\[ssrKey\]/g, b.ssrCode);
			c = c.replace(/\[ssrCode\]/g, b.ssrCode);
			c = c.replace(/\[name\]/g, b.name);
			return c
		};
		return b
	}
}
if (SKYSALES.Class.SsrFee === undefined) {
	SKYSALES.Class.SsrFee = function () {
		var a = SKYSALES.Class.Base();
		var b = SKYSALES.Util.extendObject(a);
		b.feeCode = "";
		b.journeyNumber = -1;
		b.segmentNumber = -1;
		b.passengerFee = 0;
		b.passengerNumber = -1;
		b.travelComponent = "";
		b.delimiter = "_";
		b.templateId = "ssrFeeId";
		b.template = null;
		b.templateHtml = "";
		b.init = function (c) {
			this.setSettingsByObject(c);
			this.setVars();
			this.addEvents()
		};
		b.setVars = function () {
			a.setVars.call(this);
			b.template = $("#" + this.templateId);
			b.templateHtml = b.template.text()
		};
		b.getKey = function () {
			var c = this.delimiter;
			var d = this.feeCode + c + this.journeyNumber + c + this.segmentNumber + c + this.passengerNumber;
			return d
		};
		b.supplant = function (c) {
			c = c || b.templateHtml;
			c = window.decodeURI(c);
			c = c.replace(/\[feeCode\]/g, this.feeCode);
			var d = SKYSALES.Util.convertToLocaleCurrency(this.passengerFee);
			c = c.replace(/\[passengerFee\]/g, d);
			return c
		};
		return b
	}
}
if (SKYSALES.Class.SsrContainer === undefined) {
	SKYSALES.Class.SsrContainer = function () {
		var c = SKYSALES.Class.UnitContainer();
		var d = SKYSALES.Util.extendObject(c);
		d.dynamicContainerId = "";
		d.dynamicContainer = null;
		d.ssrTemplateId = "";
		d.lostSsrTemplateHtml = "";
		d.lostSsrTemplate = null;
		d.lostSsrTemplateId = "lostSsrId";
		d.ssrTemplate = null;
		d.ssrCancelButtonId = "";
		d.ssrCancelButton = null;
		d.ssrConfirmButtonId = "";
		d.ssrConfirmButton = null;
		d.sellSsrButtonId = "";
		d.sellSsrButton = null;
		d.unit = null;
		d.unitInput = null;
		d.soldSsrInputContainerId = "soldSsrInputContainer";
		d.soldSsrInputContainer = null;
		d.flightListId = "confirmUnitInputContainer";
		d.flightList = null;
		d.showFlightList = false;
		d.delimiter = "_";
		d.unitMapContainer = null;
		d.iframe = null;
		d.unfulfilledPropertyTemplateId = "unfulfilledPropertyId";
		d.unfulfilledPropertyTemplate = null;
		d.unfulfilledPropertyTemplateHtml = "";
		var a = "";
		var b = "";
		d.init = function (e) {
			this.setSettingsByObject(e);
			this.setVars();
			this.addEvents()
		};
		d.sellSsrs = function () {
			d.populateSoldSsrHash();
			d.writeSoldSsrsToDom();
			window.__doPostBack(d.unitMapContainer.clientName + "$LinkButtonSellSsrs", "")
		};
		d.populateSoldSsrHash = function () {
			var l = this.unitMapContainer;
			var f = "";
			var h = null;
			var m = l.ssrHash;
			var e = null;
			var o = null;
			var p = d.unitInput;
			var k = l.soldSsrHash;
			var j = null;
			var g = false;
			var q = l.activeUnitInput;
			var n = q.getSegmentKey();
			k[n] = {
				soldSsrArray : [],
				lostSsrArray : []
			};
			for (f in m) {
				if (m.hasOwnProperty(f)) {
					e = m[f];
					o = {
						journeyIndex : p.journeyIndex,
						segmentIndex : p.segmentIndex,
						passengerNumber : p.passengerNumber,
						ssrCode : e.ssrCode
					};
					h = SKYSALES.Class.SoldSsr();
					h.init(o);
					g = h.isSelected();
					if (g) {
						j = k[n].soldSsrArray;
						j[j.length] = h;
						k[n].soldSsrArray = j
					}
				}
			}
		};
		d.updateConfirm = function () {
			d.populateSoldSsrHash();
			d.hide();
			d.unitMapContainer.assignSeat(d.unit);
			d.writeSoldSsrsToDom()
		};
		d.writeSoldSsrsToDom = function () {
			var k = this.unitMapContainer.soldSsrHash;
			var f = 0;
			var g = "";
			var h = null;
			var e = this.unitMapContainer.clientId;
			var m = this.unitMapContainer.clientName;
			var o = "";
			var n = "";
			var p = this.delimiter;
			var j = null;
			var l = 0;
			for (f in k) {
				if (k.hasOwnProperty(f)) {
					j = k[f].soldSsrArray || [];
					for (l = 0; l < j.length; l += 1) {
						h = j[l];
						if (h.ssrCode) {
							o = e + p + h.ssrCode + p + h.journeyIndex + p + h.segmentIndex + p + h.passengerNumber;
							n = m + "$" + h.ssrCode + p + h.journeyIndex + p + h.segmentIndex + p + h.passengerNumber;
							g += '<input type="hidden" id="' + o + '" name="' + n + '" value="' + h.quantity + '" />'
						}
					}
				}
			}
			d.soldSsrInputContainer.html(g)
		};
		d.hide = function () {
			d.dynamicContainer.html("");
			if (d.iframe) {
				d.iframe.remove()
			}
			c.hide.call(d)
		};
		d.addEvents = function () {
			c.addEvents.call(this);
			d.ssrCancelButton.click(d.hide);
			d.ssrConfirmButton.click(d.updateConfirm);
			d.sellSsrButton.click(d.sellSsrs)
		};
		d.setVars = function () {
			c.setVars.call(this);
			d.dynamicContainer = $("#" + d.dynamicContainerId);
			d.ssrTemplate = $("#" + d.ssrTemplateId);
			d.ssrCancelButton = $("#" + d.ssrCancelButtonId);
			d.ssrConfirmButton = $("#" + d.ssrConfirmButtonId);
			d.soldSsrInputContainer = $("#" + d.soldSsrInputContainerId);
			d.lostSsrTemplate = $("#" + d.lostSsrTemplateId);
			d.flightList = $("#" + d.flightListId);
			d.sellSsrButton = $("#" + d.sellSsrButtonId);
			d.unfulfilledPropertyTemplate = $("#" + d.unfulfilledPropertyTemplateId);
			d.lostSsrTemplateHtml = d.lostSsrTemplate.text();
			b = d.ssrTemplate.text();
			a = d.dynamicContainer.text();
			d.unfulfilledPropertyTemplateHtml = d.unfulfilledPropertyTemplate.text()
		};
		d.getSsrFee = function (f) {
			var e = this.unitMapContainer.getSsrFee(f);
			return e
		};
		d.supplant = function () {
			var t = a;
			var w = "";
			var u = this.unit;
			var D = this.unitInput;
			var q = this.unitMapContainer.ssrHash;
			var l = this.unitMapContainer.soldSsrHash;
			var g = "";
			var r = null;
			var s = "";
			var e = null;
			var f = false;
			var y = 0;
			var A = [];
			var z = D.getSegmentKey();
			var C = [];
			var B = 0;
			var h = "";
			var k = "";
			var p = null;
			var v = this.unitMapContainer.getUnfulfilledPropertyHash(D);
			var n = null;
			var F = this.unitMapContainer.getNotSeatedTogether(D);
			var x = "";
			var j = "";
			var E = "";
			var m = {
				templateHtml : ""
			};
			var o = this.unit.getSsrCodeList();
			if (!l[z]) {
				l[z] = {
					soldSsrArray : [],
					lostSsrArray : []
				}
			}
			if (!u) {
				u = SKYSALES.Class.Unit()
			}
			m.templateHtml = t;
			t = u.supplant(m);
			if (D) {
				t = D.supplant(t, "")
			}
			if (u.hasSsr) {
				y = 0;
				for (ssrIndex in o) {
					if (q.hasOwnProperty(o[ssrIndex])) {
						r = q[o[ssrIndex]];
						if (r.ssrCode) {
							f = u.hasSsr(y);
							if (f) {
								w = b;
								w = r.supplant(w);
								e = d.getSsrFee(r);
								w = e.supplant(w);
								s += w
							} else {
								C = l[z].soldSsrArray;
								for (B = 0; B < C.length; B += 1) {
									p = C[B];
									if (p.ssrCode === r.ssrCode) {
										A[A.length] = r;
										break
									}
								}
							}
						}
						y += 1
					}
				}
				l[z].lostSsrArray = A;
				for (y = 0; y < A.length; y += 1) {
					r = A[y];
					if (r.ssrCode) {
						h = d.lostSsrTemplateHtml;
						h = r.supplant(h);
						k += h
					}
				}
			}
			for (E in v) {
				if (v.hasOwnProperty(E)) {
					n = v[E];
					x = d.unfulfilledPropertyTemplateHtml;
					x = x.replace("[key]", E);
					x = x.replace("[value]", n.name);
					j += x
				}
			}
			t = t.replace(/\[notSeatedTogether\]/, F);
			t = t.replace(/\[unfulfilledPropertyArray\]/, j);
			t = t.replace(/\[lostSsrArray\]/g, k);
			t = t.replace(/\[ssrArray\]/g, s);
			return t
		};
		d.checkSoldSsrInput = function () {
			var f = 0;
			var h = this.unitMapContainer.soldSsrHash;
			var e = d.unitInput;
			var j = null;
			var k = null;
			var l = e.getSegmentKey();
			var g = h[l].soldSsrArray || [];
			for (f = 0; f < g.length; f += 1) {
				j = g[f];
				k = j.getInput();
				k.attr("checked", "checked")
			}
		};
		d.sellSelectedFilterSsrs = function (q) {
			var m = this.unitMapContainer;
			q = q || m.activeUnitInput;
			var n = m.getSelectedSsrFilterInputHash();
			var k = "";
			var e = null;
			var f = "";
			var g = null;
			var p = null;
			var o = q.getSegmentKey();
			var l = m.soldSsrHash || {};
			var j = l[o].soldSsrArray || [];
			var h = false;
			for (f in n) {
				if (n.hasOwnProperty(f)) {
					e = n[f];
					k = e.filterName;
					p = {
						ssrCode : k,
						journeyIndex : q.journeyIndex,
						segmentIndex : q.segmentIndex,
						passengerNumber : q.passengerNumber
					};
					g = new SKYSALES.Class.SoldSsr();
					g.init(p);
					o = g.getKey();
					h = this.isSoldSsrAlreadySold(g, q);
					if (!h) {
						j[j.length] = g;
						if (!l[o]) {
							l[o] = {
								soldSsrArray : [],
								lostSsrArray : []
							}
						}
						m.soldSsrHash[o].soldSsrArray = j
					}
				}
			}
		};
		d.isSoldSsrAlreadySold = function (e, o) {
			var k = this.unitMapContainer;
			var g = false;
			var l = 0;
			var n = o.getSegmentKey();
			var j = k.soldSsrHash || {};
			var f = j[n].soldSsrArray || [];
			var m = null;
			var h = "";
			o = o || k.activeUnitInput;
			for (l = 0; l < f.length; l += 1) {
				m = f[l];
				h = m.getKey();
				if (h === n) {
					g = true;
					break
				}
			}
			return g
		};
		d.show = function () {
			var j = d.supplant();
			this.container.css("position", "absolute");
			this.container.css("left", "200px");
			this.container.css("top", "200px");
			this.dynamicContainer.html(j);
			this.sellSelectedFilterSsrs();
			this.checkSoldSsrInput();
			var k = 0;
			var e = 0;
			var g = 0;
			var l = 0;
			var m = "";
			var h = null;
			var f = null;
			if ($.browser.msie) {
				k = this.container.height();
				e = this.container.width();
				g = this.container.css("left");
				l = this.container.css("top");
				m = '<iframe src="#"></iframe>';
				f = this.container.parent();
				f.prepend(m);
				h = $("iframe", f);
				h.css("position", "absolute");
				h.css("left", g);
				h.css("top", l);
				h.width(e);
				h.height(k);
				d.iframe = h
			}
			if (this.showFlightList) {
				this.flightList.show()
			}
			c.show.call(this)
		};
		return d
	}
}
SKYSALES.Class.UnitFee = function () {};
SKYSALES.Class.UnitFee.UnitFeeHash = {};
SKYSALES.Class.UnitFee.getUnitFee = function (b) {
    var a = SKYSALES.Class.UnitFee.UnitFeeHash[b];

	if (!a) {
		a = 0
	}
    
	return a
};

function UpdateTotalAmount(mode, val) {
    var SeatTotAmt = document.getElementById("SeatTotAmt").value;
    if (SeatTotAmt == "") SeatTotAmt = 0;
    SeatTotAmt = parseFloat(SeatTotAmt);
    if (mode == 0)
        SeatTotAmt = SeatTotAmt - parseFloat(val);
    else if (mode == 1)
        SeatTotAmt = SeatTotAmt + parseFloat(val);
    document.getElementById("SeatTotAmt").value = SeatTotAmt;
}