var app = app || {};


// Modul for manage a word
app.word = function () {
    var catchedLetterId,
        $htmlWord;

    var $loadEvents = function () {
        var $words = $('.word');
        $words.on('dragstart', drag);
        $words.on('mousedown', 'span', catchLetter);
        $words.on('click', rotate);
    }();

    function catchLetter(ev) {
        catchedLetterId = Number($(this).attr('data-position'));
    }

    function drag(ev) {
        ev.originalEvent.dataTransfer.setData('text/plain', 'anything');
        $htmlWord = $(this).parent();
    }

    function rotate(ev) {
        var $word = $(this);
        if ($word.hasClass('vertical')) {
            $word.removeClass('vertical');
        } else {
            $word.addClass('vertical');
        }
    }

    function getCatchedLetterId() {
        return catchedLetterId;
    }

    function getWord() {
        var word = $($htmlWord).children(".word").text();
        word = word.replace(/[ \n\r]+/g, '');
        return word;
    }

    function getHtmlWord() {
        return $htmlWord;
    }

    function isVertical() {
        return $htmlWord.children(".word").hasClass('vertical');
    }

    function clear() {
        catchedLetterId = null;
        $htmlWord = null;
    }

    return {
        getCatchedLetterId: getCatchedLetterId,
        getWord: getWord,
        getHtmlWord: getHtmlWord,
        isVertical: isVertical,
        clear: clear
    };
}();


// Modul for manage a board
app.board = function (word) {
    var boardName = $('#boarName').text(),
        words = [],
        dropCellId,
        size;

    function setSize(boardSize) {
        size = boardSize;
    }

    function getName() {
        return boardName;
    }

    function allowDrop(ev) {
        ev.preventDefault();
    }

    function getStartPosition() {
        var start,
            end;

        if (word.isVertical()) {
            start = dropCellId - word.getCatchedLetterId() * size;
            end = start + (word.getWord().length - 1) * size;
            if (start < 0 || end >= size * size) {
                return -1;
            }
        } else {
            start = dropCellId - word.getCatchedLetterId();
            end = start + word.getWord().length - 1;

            if (parseInt(start / size) != parseInt(end / size)) {
                return -1;
            }
        }

        return start;
    }

    function hasWord() {
        var index = words.indexOf(word.getWord());
        return index >= 0;
    }

    function pushWordInBoard() {
        var s = size,
            $cells = $('.board span'),
            wordAsText = word.getWord(),
            i;

        var startPosition = getStartPosition();
        if (startPosition < 0 || hasWord()) {
            return;
        }

        if (!word.isVertical()) {
            s = 1;
        }

        var currentCell;
        for (i in wordAsText) {
            currentCell = $cells[startPosition + Number(i) * s];
            currentCell.innerHTML = currentCell.innerHTML.replace(/[ \n\r]+/g, '');
            if (currentCell.innerHTML !== wordAsText[i] && currentCell.innerHTML !== '&nbsp;') {
                return;
            }
        }

        for (i in wordAsText) {
            currentCell = $cells[startPosition + Number(i) * s];
            currentCell.innerHTML = wordAsText[i];
        }

        words.push(word.getWord());
        console.log(getBoardAsJson());
        word.getHtmlWord().remove();
        word.clear();
    }

    function drop(ev) {
        ev.preventDefault();
        dropCellId = Number($(ev.target).attr('data-position'));
        app.boardHub.addWordToBoard(boardName, word.getWord(), word.getCatchedLetterId(), word.isVertical(), dropCellId);
        pushWordInBoard();
    }

    function getCellId(ev) {
        dropCellId = Number($(this).attr('data-position'));
    }

    function getBoardAsJson() {
        var boardContent = $('.board').text();
        boardContent = boardContent.replace(/[ \n\r]+/g, '');
        var json = JSON.stringify({
            content: boardContent,
            words: words
        });

        return json;
    }

    function loadBoard(boardAsJson) {
        var board = JSON.parse(boardAsJson),
            content = board.content;
        words = board.words;
        var $cells = $('.board span'),
            i;
        for (i in content) {
            $cells[i].innerHTML = content[i].replace(' ', '&nbsp;');
        }
    }

    var $loadEvents = function () {
        var $board = $('.board');
        $board.on('dragover', allowDrop);
        $board.on('drop', drop);
        $board.on('mouseup', 'span', getCellId);
    }();

    return {
        name: getName(),
        setSize: setSize,
        getBoardAsJson: getBoardAsJson,
        loadBoard: loadBoard
    };
}(app.word);

// SignalR Modul for adding word to board
app.boardHub = function (board) {
    var hub = $.connection.boardsHub;

    $.connection.hub.start().done(function () {
        hub.server.joinBoard(app.board.name);
    });

    hub.client.loadBoard = function (content) {
        board.loadBoard(content);
    };

    function updatePage(result) {
        if (result.message) {
            $("#message").text(result.message);
        }

        $("#points").text(result.points);
    }

    function addWordToBoard(boardName, word, catchedLetterId, isVertical, dropCellId) {
        hub.server.addWordToBoard(boardName, word, catchedLetterId, isVertical, dropCellId)
            .done(function (result) {
                updatePage(result);
            });
    }

    return {
        addWordToBoard: addWordToBoard
    };
}(app.board);

//Module shearch 
app.search = function () {
    var $words = $(".word");

    function getWordText($word) {
        var word = $word.text().replace(/[ \n\r]+/g, '');
        return word;
    }
    
    function toggleVisibility($word, condition) {
        if (condition) {
            $word.parent().show();
        } else {
            $word.parent().hide();
        }
    }

    function showAll() {
        $words.each(function() {
            $(this).parent().show();
        });
    }

    function byLength(length) {
        $words.each(function () {
            var $word = $(this);
            var word = getWordText($word);
            toggleVisibility($word, word.length == length);
        });
    }

    function toBegin(letters) {
        $words.each(function () {
            var $word = $(this); 
            var word = getWordText($word);
            toggleVisibility($word, word.indexOf(letters) == 0);
        });
    }

    function toContain(letters) {
        $words.each(function () {
            var $word = $(this);
            var word = getWordText($word);
            toggleVisibility($word, word.indexOf(letters) >= 0);
        });
    }

    var $loadEvents = function () {
        var $searchInput = $('#letters');
        $searchInput.on('keyup', function (ev) {
            if ($(ev.target).val() == "") {
                showAll();
                return;
            }

            var radioVal = $('#radio-buttons input:checked').val();
            switch (radioVal) {
                case "1":
                    byLength($(ev.target).val());
                    break;
                case "2":
                    toBegin($(ev.target).val());
                    break;
                case "3":
                    toContain($(ev.target).val());
                    break;
            default:
            }
        });
    }();
}();


