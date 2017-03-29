$('#backToReferrerPage').attr('href', document.referrer);

//Action Confirm
(function () {
    function getArtistToAddToAuction() {
        var artistChecked = [];
        $("#PartsArtists  :checked").map(function () {
            artistChecked.push($(this).val());
        });
        return artistChecked;
    }

    $('#checkAll').on('change', function () {
        if (this.checked) {
            $('#PartsArtists :checkbox').prop('checked', true);
        } else {
            $('#PartsArtists :checkbox').prop('checked', false);
        }
    });

    $('#confirmSubmitBtn').on('click', function () {
        var checkedArt = getArtistToAddToAuction();
        var form = $('#confirmAuctionSubmit')
        for (index in checkedArt) {
            form.append('<input autocomplete="off" type="hidden" name="Participants[' + index + '].User.UserName" value=' + checkedArt[index] + '>')
        }
        
        form.submit();
    });

    
    function AddNewParticipant(artistName, index) {
        var table = $('#PartsArtists');
        table.append('<div class="col-lg-6" id=' + artistName + '>' +
                            '<div class="input-group" style="direction: ltr;text-align:left">' +
                                '<input autocomplete="off" type="text" class="form-control" disabled value="' + artistName + '">' +
                                //'<input autocomplete="off" type="hidden" class="form-control" name="Participants[' + index + '].User.UserName" value="' + artistName + '">' +
                                '<span class="input-group-addon">' +
                                    '<input autocomplete="off" type="checkbox" value=' + artistName + '>' +
                                '</span>' +
                            '</div>' +
                        '</td>' +
                    '</div>');
    }

    $("#loadArtists").on('click', function () {
        var table = $('#PartsArtists');
        table.empty();
        $.ajax({
            url: '/UserManager/GetAllArtists',
            success: function (data) {
                if (data) {
                    for (i in data) {
                        AddNewParticipant(data[i], i);
                    }
                }
            }
        });
    });

    $("#searchNewArtist").autocomplete({
        change: function (event, ui) {
            if (!ui.item) {
                $(this).val('');
            }
        },
        source: function (req, res) {
            var finalObj = {};
            var checkedArt = [];
            var checkedArt = getArtistToAddToAuction();

            finalObj.name = req.term;
            finalObj.pickedArtists = checkedArt

            $.ajax({
                url: '/Admin/UserManager/GetNewArtist',
                data: finalObj,
                success: function (data) {
                    res(data);
                }
            });
        }
    });
})();
var isUserReportView = !!$('.user-status-report-select').length;
if (isUserReportView) {
    var HideShowDate = function () {
        if ($('[name=usersStatus] option:selected').text() == "רשומים") {
            $('[name*="Date"]').attr("disabled", true);

        }
        else {
            $('[name*="Date"]').attr("disabled", false);
        }
    };

    HideShowDate();

    $('[name=usersStatus]').on('change', function () {
        HideShowDate()
    });
}
// ReportView
(function () {
    'use strict';
    if ($('#reportsContainer').length) {
        var startInput = $("#StartDate");
        var endInput = $("#EndDate");

        $('.select-music-type-report').on('change',hideShowMusicType);
        $('#WeekDate').on('click', setWeekAgo);
        $('#MonthDate').on('click', setMonthAgo);
        $('#YearDate').on('click', setYearAgo);

        function hideShowMusicType(evt) {
            if (evt.currentTarget.value === '0') {
                $('[name*="Date"]').attr("disabled", true);
            } else {
                $('[name*="Date"]').attr("disabled", false);
            }
        }

        function setWeekAgo() {
            var weekAgo = new Date();
            var today = new Date();
            weekAgo.setDate(weekAgo.getDate() - 7);
            today.setDate(today.getDate());
            SetRangeTime(weekAgo, today);
        }

        function setMonthAgo() {
            var mothAgo = new Date();
            var today = new Date();
            mothAgo.setMonth(mothAgo.getMonth() - 1)
            today.setDate(today.getDate());
            SetRangeTime(mothAgo, today);
        }

        function setYearAgo() {
            var yearAgo = new Date();
            var today = new Date();
            yearAgo.setYear(yearAgo.getFullYear() - 1);
            today.setDate(today.getDate());
            SetRangeTime(yearAgo, today);
        }

        
        function SetRangeTime(startDate,endDate) {
            startInput.val(dateToString(startDate));
            endInput.val(dateToString(endDate));
        }

        function dateToString(date) {
            return date.getFullYear() + '-' + padNumber(date.getMonth() + 1,2) + '-' + padNumber(date.getDate(),2)
        }

        function padNumber(number, length) {
            var str = number.toString();
            return str.length < length ? padNumber('0' + str, length) : str;
        }

        //$('#leadMusicTable').DataTable({
        //    "lengthMenu": [[50, 10, 25 ], [50, 10, 25]]
        //});


        $('.laed-music-row').on('dblclick', getMusicPurchase);
        function getMusicPurchase(evt) {
            var musicId = $(evt.currentTarget).attr('data-music-id');
            $.get('MusicPurchase?MusicId=' + musicId).then(function success(res) {
                $('#modelMusicPurchasesBody').html(res);
                $('#modelMusicPurchases').modal('show');
            }, function failed(res) {
                console.error(res);
                alert('הבאת נתוני המכירות נכשלה');
            })
        }
        
    }
})();

$("#SearchArtistAgentArtists").autocomplete({
    source: function(req, res){
        $.ajax({
            url: '/Admin/UserManager/GetArtistsForArtistAgent?StartWith=' + req.term,
            success: function (data) {
                res(data);
            }
        });
    }
});

$("#searchName").autocomplete({
    source: function (req, res) {
        $.ajax({
            url: '/Admin/UserManager/GetUserByName?StartWith=' + req.term,
            success: function (data) {
                res(data);
            }
        });
    }
});

$("#searchContactManName").autocomplete({
    source: function (req, res) {
        $.ajax({
            url: '/Admin/UserManager/GetUserByContactManName?StartWith=' + req.term,
            success: function (data) {
                res(data);
            }
        });
    }
});
$("#searchMusicName").autocomplete({
    source: function (req, res) {
        $.ajax({
            url: '/Admin/Music/SearchMusicsNames?StartWith=' + req.term,
            success: function (data) {
                res(data);
            }
        });
    }
});
    
$("#searchArtistName").autocomplete({
    source: function (req, res) {
        $.ajax({
            url: '/Admin/UserBase/GetUserByName?StartWith=' + req.term + '&usrType=Artist',
            success: function (data) {
                res(data);
            }
        });
    }
});
/********************
*    Music Edit     *
********************/
(function MusicEdit() {
    'use strict';
    var searchTagInput = $("#searchTagName");
    if (searchTagInput.length) {
        var allTag = [];
        var musicTags = $('#musicTags');
        var searchData = [];
        var currTagIds = [];
        var sumCopyrightsPercentsInput = $('#sumCopyrightsPercents');
        var updateCopyrightsPercentsBtn = $('#updateCopyrightsPercents');
        var addCopyrightsOwnerBtn = $('#addCopyrightsOwner');
        var addTagBtn = $('#addTagBtn');
        var tagsCount = $('#musicTagsCount').val();
        var isFirstSelect = true;
        var currSelectValue = '';
        var musicCopyrightsCount = $('#musicCopyrightsCount').val();
        var newCopyrightOwnerType = $('#newCopyrightOwnerType');
        var newCopyrightOwnerName = $('#newCopyrightOwnerName');
        var newCopyrightPercents = $('#newCopyrightPercents');

        addTagBtn.on('click', addTagBtnClick);

        // init
        updateCopyrightsPercents();
        getCurrentTags();
        getAvailbleTags();
        // functions
        searchTagInput.select2();
        function getAvailbleTags() {
            $.post('/search/Tags').then(function (data) {
                var parseData =[];
                if (data.length) {
                    searchData = data
                        .filter(function (tg) {
                            return !currTagIds.find(function (id) { return id === tg.ID });
                        })
                        .map(function (tg) {
                            tg.hirarchyString = tg.TagHirarchi.reverse().join(' ← ' + String.fromCharCode(8207));
                            return tg;
                        });
                    parseData = searchData.map(function (tg) {
                        return { id: tg.ID, text: tg.hirarchyString };
                    });
                }
                searchTagInput.select2({
                    data:parseData,
                });
                 searchTagInput.val(null).trigger('change');
            });
        }
        function getCurrentTags() {
            $('.tag-id-value').each(function (index, elem) {
                currTagIds.push(elem.value);
            });
        }

        function addTagBtnClick() {
            currSelectValue = searchTagInput.val();
            if (!currTagIds.find(function (id) { return id === currSelectValue })) {
                var tg = searchData.find(function (tg) {
                    return tg.ID == currSelectValue;
                });
                if (tg) {
                    addNewTag(tagsCount++, tg)
                }
            }
        };

         function addNewTag(index, tag) {
            currTagIds.push(tag.ID);
            var tgElement = getTagElement(index, tag);
            musicTags.append(tgElement);
        }
        function getTagElement(index, tag){
            return $('<div class="form-group-sm">' +
                '<div class="input-group" style="direction: ltr;text-align:left">' +
                    '<span class="input-group-addon">' +
                        '<input autocomplete="off" type="hidden" name="TagViews[' + index + '].Name" value="' + tag.Name + '">' +
                        '<input autocomplete="off" type="hidden" name="TagViews[' + index + '].ViewOrder" value="' + tag.ViewOrder + '">' +
                        '<input autocomplete="off" type="hidden" name="TagViews[' + index + '].ID" value="' + tag.ID + '">' +
                        '<input autocomplete="off" type="hidden" name="TagViews[' + index + '].IsCanDelete" value="'+tag.IsCanDelete+'">' +
                        '<input autocomplete="off" type="hidden" name="TagViews[' + index + '].IsDynamic" value="' + tag.IsDynamic + '">' +
                        '<input autocomplete="off" type="hidden" name="TagViews[' + index + '].IsPublicTag" value="'+tag.IsPublicTag+'">' +
                        '<input autocomplete="off" type="hidden" name="TagViews[' + index + '].Level" value="'+tag.Level+'">' +
                        '<input autocomplete="off" type="hidden" name="TagViews[' + index + '].ParentTagID" value="'+(tag.ParentTagID || '')+'">' +
                        '<input autocomplete="off" type="checkbox" checked="checked" name="TagViews[' + index + '].IsNotDeleted" value="true">' +
                    '</span>' +
                    '<input autocomplete="off" type="text" disabled class="form-control" placeholder="' + tag.TagHirarchi.join(' ← ') + '" />' +
                '</div>' +
            '</div>');
        }
        updateCopyrightsPercentsBtn.on('click', updateCopyrightsPercents);

        function updateCopyrightsPercents() {
            sumCopyrightsPercentsInput.val(getSumCopyrightPercents());
            $('#sumCopyrightsPercentsMessage').text('');
        };
        function getSumCopyrightPercents() {
            var sum = 0;
            $('.copy-right-percents').each(function (index, element) {
                sum += Number(element.value);
            });
            return sum;
        }

        $('#searchMuiscArtistName').autocomplete({
            source: function (req, res) {
                $.ajax({
                    url: '/Admin/UserBase/GetUserByName?StartWith=' + req.term + '&usrType=Artist',
                    success: function (data) {
                        res(data);
                    }
                });
            }
        });

        $('#newCopyrightRow').on('click', '#addCopyrightsOwner', function addCopyrightsOwner(evt) {
            var type = newCopyrightOwnerType.val();
            var name = newCopyrightOwnerName.val();
            var percents = newCopyrightPercents.val();

            if (!name) {
                $('#newCopyrightOwnerNameMessage').text('יש להזין שם בעל זכויות');
                newCopyrightOwnerName.focus();
            } else {
                newCopyrightOwnerName.val('');
                newCopyrightPercents.val(0);
                $('#newCopyrightOwnerNameMessage').text('');
                var rowAdd = getNewCopyrightElement(type, name, percents, musicCopyrightsCount++);
                rowAdd.on('click', '.remove-copyright-btn', removeCopyright);
                rowAdd.insertBefore(evt.originalEvent.currentTarget);
                updateCopyrightsPercents();
            }
        });
        $('tr').on('click', '.remove-copyright-btn', removeCopyright);
        function removeCopyright(evt) {
            var row = $(evt.originalEvent.currentTarget);
            row.css('display','none');
            row.find('.is-copyright-deleted').val('true');
            row.find('.copy-right-percents').val(0);
            updateCopyrightsPercents();
        };

        $('#saveEditMusicBtn').on('click', function myfunction() {
            updateCopyrightsPercents();
            var isPercentsValid = getSumCopyrightPercents() === 100;
            var isMusicLengthValid = Number($('#Seconds').val()) > 0 || Number($('#Minutes').val()) > 0;
            if (!isPercentsValid) {
                $('#sumCopyrightsPercentsMessage').text('זכויות היוצרים צריכות להיות 100%');
                newCopyrightPercents.focus();
            }
            if (!isMusicLengthValid) {
                $('#musicLengthDangerMessage').text('יש להזין אורך יצירה');
                $('#Minutes').focus();
            }
            return isPercentsValid && isMusicLengthValid;
        });
        $('.file-form-container').on('change', '.upload-file-input', function (evt) {
            var fileInput = evt.target;
            var file = fileInput.files[0];
            $(evt.originalEvent.currentTarget).find('.file-danger-message').text('');
            $(evt.originalEvent.currentTarget).find('.upload-file-input').focus();
            if (file && fileInput.accept.toLowerCase() == file.type.toLowerCase()) {
                $(evt.originalEvent.currentTarget).find('.file-path-input').val(file.name);
            } else {
                if (file) {
                    $(evt.originalEvent.currentTarget).find('.file-danger-message').text('יש להעלות קובץ מסוג ' + fileInput.accept + ' בלבד');
                }

                $(evt.originalEvent.currentTarget).find('.file-path-input').val('');
                fileInput.files[0] = undefined;
                fileInput.value = '';
            }
        });

        function getNewCopyrightElement(type, name, percents,index) {
            var element = $('<tr>'+
	                            '<td>'+
		                            '<input autocomplete="off" class="is-copyright-deleted" name="Copyrights[' + index + '].IsDeleted" type="hidden" value="False">'+
		                            '<select data-val="true" id="Copyrights_' + index + '__Type" name="Copyrights[' + index + '].Type">' +
                                        '<option value="0">מו"ל</option>' +
			                            '<option value="1">כותב</option>'+
			                            '<option value="2">מלחין</option>'+
			                            '<option value="3">מעבד</option>'+
		                            '</select>'+
                                '</td>'+
                                '<td>'+
                                    '<input autocomplete="off" name="Copyrights[' + index + '].CopyrightAuthorName" class="form-inline" type="text" value="'+name+'">'+
                                    '<span class="field-validation-valid text-danger" data-valmsg-for="Copyrights[' + index + '].CopyrightAuthorName" data-valmsg-replace="true"></span>'+
                                '</td>'+
                                '<td>'+
                                    '<input autocomplete="off" name="Copyrights[' + index + '].Percents" class="form-inline copy-right-percents" type="number" min="0" max="100" value="'+percents+'">'+
                                    '<span class="field-validation-valid text-danger" data-valmsg-for="Copyrights[' + index + '].Percents" data-valmsg-replace="true"></span>' +
                                '</td>'+
                                '<td>'+
                                    '<input autocomplete="off" type="button" value="הסר" class="btn btn-default remove-copyright-btn">'+
                                '</td>'+
                            '</tr>');
            element.find('#Copyrights_' + index + '__Type').val(type);
            return element;
        }
    }
})();

/*****************************
*    Permission Property     *
*****************************/
(function () {
    'use strict';
    var valuesLength = $('#PropertyValuesCount').val();
    var propertyId = $('#ID').val();
    var CategoryId = $('#PermissionsCategoryID').val();
    $("#rowAddNewValue").on("click", "#addValueBtn", function (evt) {
        var newVal = getNewValueTemplate(valuesLength++, propertyId, CategoryId);
        var valElem = $(newVal).insertBefore(evt.originalEvent.currentTarget);
        valElem.on("click", ".remove-value-btn", removeValue);
    });
    $("#propertyValuesTable  tr").on("click", ".remove-value-btn", removeValue);

    function removeValue(evt) {
        evt.delegateTarget.style.display = 'none';
        $(evt.delegateTarget).find('.is-row-deleted').val('true');
    }

    function getNewValueTemplate(index,propId,catId) {
        return '<tr>' +
	        '<td>' +
		        '<input data-val="true" data-val-required="השדה PermissionPropertyId נדרש." id="PropertyValueViews_' + index + '__PermissionPropertyId" name="PropertyValueViews[' + index + '].PermissionPropertyId" type="hidden" value="' + propId + '">' +
		        '<input data-val="true" data-val-required="השדה PermissionsCategoryID נדרש." id="PropertyValueViews_' + index + '__PermissionsCategoryID" name="PropertyValueViews[' + index + '].PermissionsCategoryID" type="hidden" value="' + catId + '">' +
		        '<input class="form-control text-box single-line" data-val="true" data-val-required="יש להזין שם." id="PropertyValueViews_' + index + '__Name" name="PropertyValueViews[' + index + '].Name" type="text" placeholder="שם הערך">' +
		        '<span class="field-validation-valid text-danger" data-valmsg-for="PropertyValueViews[' + index + '].Name" data-valmsg-replace="true"></span>' +
	        '</td>' +
	        '<td>' +
		        '<input autocomplete="off" class="form-control text-box single-line" data-val="true" data-val-number="The field סדר תצוגה must be a number." data-val-range="יש להזין סדר תצוגה גדול מ-0" data-val-range-max="2147483647" data-val-range-min="1" data-val-required="השדה סדר תצוגה נדרש." id="PropertyValueViews_' + index + '__ViewOrder" name="PropertyValueViews[' + index + '].ViewOrder" type="number" value="' + (index + 1) + '">' +
		        '<span class="field-validation-valid text-danger" data-valmsg-for="PropertyValueViews[' + index + '].ViewOrder" data-valmsg-replace="true"></span>' +
	        '</td>' +
	        '<td>' +
		        '<input autocomplete="off" checked="checked" class="form-control check-box" data-val="true" data-val-required="השדה סטטוס נדרש." id="PropertyValueViews_' + index + '__IsActive" name="PropertyValueViews[' + index + '].IsActive" type="checkbox" value="true"><input autocomplete="off" name="PropertyValueViews[' + index + '].IsActive" type="hidden" value="true">' +
		        '<span class="field-validation-valid text-danger" data-valmsg-for="PropertyValueViews[' + index + '].IsActive" data-valmsg-replace="true"></span>' +
	        '</td>' +
	        '<td>' +
                '<input autocomplete="off" type="hidden" name="PropertyValueViews['+index+'].IsDeleted" class="is-row-deleted" value="false" />' +
		        '<a class="btn btn-default remove-value-btn">הסר</a>' +
	        '</td>' +
        '</tr>';
    }
})();

/*************************
*    Permission Cost     *
**************************/
(function () {
    'use strict';
    var dataChanged = false;
    var currCategoryID = $('#CurrCategory_ID').val();
    if (currCategoryID) {
        window.onbeforeunload = function () {
            if (dataChanged) {
                return "יש מידע שלא נשמר,";
            }
        }
        var table = $('#tblPermissionCost');
        table.on('change', function () {
            dataChanged = true;
        });
        $('.frmCostPriceSaveBtn').on('click', function () {
            dataChanged = false;
            console.log(table);
        });
    }
})();

/*************************
*    Admin Managment     *
**************************/
(function () {
    if ($('.admin-managmant-role-container').length) {
        adminManagmentModule();
    }

    function adminManagmentModule() {
        // Dom Data
        var sysAdminCheckBox = $('.admin-managmant-role-checkbox[value="SystemAdmin"]');
        var adminsCheckBox = $('.admin-managmant-role-checkbox:not(.admin-managmant-role-checkbox[value="SystemAdmin"])');
        // Events
        sysAdminCheckBox.on('change', sysAdminCheckBox_changed);

        // init
        if (sysAdminCheckBox[0].checked) {
            adminsCheckBox.attr('disabled', 'disabled');
        }

        // Methods Implement
        function sysAdminCheckBox_changed(evt) {
            if (evt.target.checked) {
                adminsCheckBox.attr('disabled', 'disabled');
            } else {
                adminsCheckBox.removeAttr('disabled');
            }
        }
    }
})();