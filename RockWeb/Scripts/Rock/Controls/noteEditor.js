﻿(function (Sys) {
    'use strict';
    Sys.Application.add_load(function () {

        // Initialize NoteEditor and NoteContainer events
        $('.js-notecontainer .js-addnote,.js-editnote,.js-replynote').click(function (e) {
            var addNote = $(this).hasClass('js-addnote');
            var editNote = $(this).hasClass('js-editnote');
            var replyNote = $(this).hasClass('js-replynote');
            var cancelNote = $(this).hasClass('js-editnote-cancel');
            var deleteNote = $(this).hasClass('js-removenote');

            var $noteContainer = $(this).closest('.js-notecontainer');
            var $noteEditor = $noteContainer.find('.js-note-editor');
            $noteEditor.detach();

            // clear out any previously entered stuff
            $noteEditor.find('.js-parentnoteid').val('');
            $noteEditor.find('textarea').val('');
            $noteEditor.find('input:checkbox').prop('checked', false);
            $noteEditor.find('.js-notesecurity').hide();

            if (addNote) {
                // display the 'noteEditor' as the first note in the list
                var $noteList = $noteContainer.find('.js-notelist').first();
                $noteList.prepend($noteEditor);
            }
            else {
                var $currentNote = $(this).closest('.js-noteviewitem');
                var currentNoteId = $currentNote.attr("data-note-id");

                if (replyNote) {
                    // display the 'noteEditor' as a reply to the current note
                    $noteEditor.find('.js-parentnoteid').val(currentNoteId);
                    var $childNotesList = $currentNote.find('.js-childnotes').first();
                    $childNotesList.append($noteEditor)
                    //$noteEditor.insertAfter($currentNote);
                }
                else if (editNote) {
                    // display the 'noteEditor' in place of the currentNote
                    $.get(Rock.settings.get('baseUrl') + 'api/notes/' + currentNoteId, function (noteData) {
                        $noteEditor.find('.js-parentnoteid').val(noteData.ParentNoteId);
                        $noteEditor.find('.js-notetext').val(noteData.Text);
                        $noteEditor.find('.js-noteprivate').prop('checked', noteData.IsPrivateNote);
                        $noteEditor.find('.js-notealert').prop('checked', noteData.IsAlert);

                        $noteEditor.find('.js-noteid').val(currentNoteId);

                        var $securityBtn = $noteEditor.find('.js-notesecurity');
                        $securityBtn.attr('data-entity-id', currentNoteId);
                        $securityBtn.show();
                        
                        e.preventDefault();
                        $noteEditor.insertBefore($currentNote);
                    });
                }
            }
            
            if (editNote) {
                // hide the readonly details of the note that we are editing then show the editor
                $currentNote.find('.js-notedetails').first().hide();
                $noteEditor.show();
            }
            else {
                // slide the noteeditor into view
                $noteEditor.slideDown();
            }
        });

        $('.js-notecontainer .js-notesecurity').click(function (e) {
            var $securityBtn = $(this);
            var entityTypeId = $securityBtn.attr('data-entitytype-id');
            var title = $securityBtn.attr('data-title');
            var currentNoteId = $securityBtn.attr('data-entity-id');
            var securityUrl = Rock.settings.get('baseUrl') + "Secure/" + entityTypeId + "/" + currentNoteId + "?t=" + title + "&pb=&sb=Done";
            Rock.controls.modal.show($securityBtn, securityUrl);
        });

        $('.js-notecontainer .js-editnote-cancel').click(function (e) {
            var $noteContainer = $(this).closest('.js-notecontainer');
            var $noteEditor = $noteContainer.find('.js-note-editor');
            $noteEditor.slideUp();

            // show any notedetails that might have been hidden when doing the editing
            $noteEditor.siblings().find('.js-notedetails').show()
        });

        $('.js-notecontainer .js-removenote').click(function (e) {
            var $currentNote = $(this).closest('.js-noteviewitem');
            var currentNoteId = $currentNote.attr("data-note-id");
            var $noteContainer = $(this).closest('.js-notecontainer');
            $noteContainer.find(".js-currentnoteid").val(currentNoteId);

            e.preventDefault();
            e.stopImmediatePropagation();
            var postbackJs = $noteContainer.find(".js-delete-postback").attr('href');
            return Rock.dialogs.confirm('Are you sure you want to delete this note?', function () {
                window.location = postbackJs;
            });
        });
    });
}(Sys));