��    N      �  k   �      �  6  �  0  �    
  %    k  E     �     �  ,   �       %   0  ,   V  -   �      �  &   �     �          9     ;     L  �   d  Q   \  *   �  [   �  G   5     }     �     �     �  $   �     
       <   7  <   t     �     �     �     �  5   �  1   4  :   f  #   �     �  3   �  N        c  P   k  (   �  ,   �       &   %     L     Y     f     s     �     �     �     �     �  (   �  �        �  q   �     Z     y     �     �     �     �     �                :     Q     l     }     �  �  �    e     q$  $  �&  �  �2  �  �4  4   )7  ,   ^7  K   �7  3   �7  G   8  K   S8  L   �8  6   �8  A   #9  3   e9  3   �9     �9  !   �9  0   �9  e  ":  �   �;  Q   <  �   p<  �   =  6   �=  =   �=  8   >  5   A>  ]   w>  6   �>  @   ?  o   M?  o   �?     -@     M@  G   i@  (   �@  �   �@  w   vA  �   �A  ]   wB  E   �B  b   C  �   ~C  !   D  �   5D  5   �D  Y   E  J   kE  m   �E  &   $F  &   KF  &   rF  A   �F  6   �F     G     /G     LG     iG  P   }G     �G     �I  �   �I  6   �J  V   �J  ?   9K  S   yK  O   �K     L  #   =L  D   aL  *   �L  ;   �L  ;   M  %   IM     oM  *   �M         C      *   I         !   (           &                    4   5   <   '      -             "       B   =   %   $              K   H   #      :                     J              @   L   /            .   A            0       3   9   ,   F   ;   8                            +   
         7   N   6       E       ?       G                         D   	       )   2       >   1       M           
Context control:
  -B, --before-context=NUM  print NUM lines of leading context
  -A, --after-context=NUM   print NUM lines of trailing context
  -C, --context=NUM         print NUM lines of output context
  -NUM                      same as --context=NUM
      --color[=WHEN],
      --colour[=WHEN]       use markers to highlight the matching strings;
                            WHEN is `always', `never', or `auto'
  -U, --binary              do not strip CR characters at EOL (MSDOS)
  -u, --unix-byte-offsets   report offsets as if CRs were not there (MSDOS)

 
Miscellaneous:
  -s, --no-messages         suppress error messages
  -v, --invert-match        select non-matching lines
  -V, --version             print version information and exit
      --help                display this help and exit
      --mmap                use memory-mapped input if possible
 
Output control:
  -m, --max-count=NUM       stop after NUM matches
  -b, --byte-offset         print the byte offset with output lines
  -n, --line-number         print line number with output lines
      --line-buffered       flush output on every line
  -H, --with-filename       print the filename for each match
  -h, --no-filename         suppress the prefixing filename on output
      --label=LABEL         print LABEL as filename for standard input
  -o, --only-matching       show only the part of a line matching PATTERN
  -q, --quiet, --silent     suppress all normal output
      --binary-files=TYPE   assume that binary files are TYPE;
                            TYPE is `binary', `text', or `without-match'
  -a, --text                equivalent to --binary-files=text
  -I                        equivalent to --binary-files=without-match
  -d, --directories=ACTION  how to handle directories;
                            ACTION is `read', `recurse', or `skip'
  -D, --devices=ACTION      how to handle devices, FIFOs and sockets;
                            ACTION is `read' or `skip'
  -R, -r, --recursive       equivalent to --directories=recurse
      --include=FILE_PATTERN  search only files that match FILE_PATTERN
      --exclude=FILE_PATTERN  skip files and directories matching FILE_PATTERN
      --exclude-from=FILE   skip files matching any file pattern from FILE
      --exclude-dir=PATTERN directories that match PATTERN will be skipped.
  -L, --files-without-match print only names of FILEs containing no match
  -l, --files-with-matches  print only names of FILEs containing matches
  -c, --count               print only a count of matching lines per FILE
  -T, --initial-tab         make tabs line up (if needed)
  -Z, --null                print 0 byte after FILE name
   -E, --extended-regexp     PATTERN is an extended regular expression (ERE)
  -F, --fixed-strings       PATTERN is a set of newline-separated fixed strings
  -G, --basic-regexp        PATTERN is a basic regular expression (BRE)
  -P, --perl-regexp         PATTERN is a Perl regular expression
   -e, --regexp=PATTERN      use PATTERN for matching
  -f, --file=FILE           obtain PATTERN from FILE
  -i, --ignore-case         ignore case distinctions
  -w, --word-regexp         force PATTERN to match only whole words
  -x, --line-regexp         force PATTERN to match only whole lines
  -z, --null-data           a data line ends in 0 byte, not newline
 %s: illegal option -- %c
 %s: invalid option -- %c
 %s: option `%c%s' doesn't allow an argument
 %s: option `%s' is ambiguous
 %s: option `%s' requires an argument
 %s: option `--%s' doesn't allow an argument
 %s: option `-W %s' doesn't allow an argument
 %s: option `-W %s' is ambiguous
 %s: option requires an argument -- %c
 %s: unrecognized option `%c%s'
 %s: unrecognized option `--%s'
 ' (standard input) Binary file %s matches
 Copyright (C) %s Free Software Foundation, Inc.
License GPLv3+: GNU GPL version 3 or later <http://gnu.org/licenses/gpl.html>
This is free software: you are free to change and redistribute it.
There is NO WARRANTY, to the extent permitted by law.
 Example: %s -i 'hello world' menu.h main.c

Regexp selection and interpretation:
 In GREP_COLORS="%s", the "%s" capacity %s. In GREP_COLORS="%s", the "%s" capacity is boolean and cannot take a value ("=%s"); skipped. In GREP_COLORS="%s", the "%s" capacity needs a value ("=..."); skipped. Invalid back reference Invalid character class name Invalid collation character Invalid content of \{\} Invalid preceding regular expression Invalid range end Invalid regular expression Invocation as `egrep' is deprecated; use `grep -E' instead.
 Invocation as `fgrep' is deprecated; use `grep -F' instead.
 Memory exhausted No match No previous regular expression No syntax specified PATTERN is a set of newline-separated fixed strings.
 PATTERN is an extended regular expression (ERE).
 PATTERN is, by default, a basic regular expression (BRE).
 Premature end of regular expression Regular expression too big Search for PATTERN in each FILE or standard input.
 Stopped processing of ill-formed GREP_COLORS="%s" at remaining substring "%s". Success Support for the -P option is not compiled into this --disable-perl-regexp binary The -P and -z options cannot be combined The -P option only supports a single pattern Trailing backslash Try `%s --help' for more information.
 Unbalanced ( Unbalanced ) Unbalanced [ Unfinished \ escape Unknown system error Unmatched ( or \( Unmatched ) or \) Unmatched [ or [^ Unmatched \{ Usage: %s [OPTION]... PATTERN [FILE]...
 With no FILE, or when FILE is -, read standard input.  If less than two FILEs
are given, assume -h.  Exit status is 0 if any line was selected, 1 otherwise;
if any error occurs and -q was not given, the exit status is 2.
 ` `egrep' means `grep -E'.  `fgrep' means `grep -F'.
Direct invocation as either `egrep' or `fgrep' is deprecated.
 conflicting matchers specified input is too large to count invalid context length argument invalid max count malformed repeat count memory exhausted recursive directory loop unfinished repeat count unknown binary-files type unknown devices method unknown directories method warning: %s: %s
 write error writing output Project-Id-Version: grep 2.5.4-pre4
Report-Msgid-Bugs-To: 
POT-Creation-Date: 2009-02-03 14:51-0400
PO-Revision-Date: 2009-01-23 18:46+0300
Last-Translator: Yuri Kozlov <yuray@komyakino.ru>
Language-Team: Russian <gnu@mx.ru>
MIME-Version: 1.0
Content-Type: text/plain; charset=UTF-8
Content-Transfer-Encoding: 8bit
X-Generator: KBabel 1.11.4
Plural-Forms:  nplurals=3; plural=(n%10==1 && n%100!=11 ? 0 : n%10>=2 && n%10<=4 && (n%100<10 || n%100>=20) ? 1 : 2);
 
Управление контекстом:
  -B, --before-context=ЧИС  печатать ЧИСЛО строк предшествующего контекста
  -A, --after-context=ЧИС   печатать ЧИСЛО строк последующего контекста
  -C, --context[=ЧИС]       печатать ЧИСЛО строк контекста,
      --color[=КОГДА],
      --colour[=КОГДА]      использовать маркеры для различия совпадающих
                            строк; КОГДА может быть always (всегда),
                            never (никогда), или auto (автоматически)
      --color, --colour     использовать маркеры для различия совпадающих строк
  -U, --binary              не удалять символы CR в конце строки (MSDOS)
  -u, --unix-byte-offsets   выдавать смещение, как-будто нет CR-ов (MSDOS)

 
Разное:
  -s, --no-messages         подавлять сообщения об ошибках
  -v, --revert-match        выбирать не подходящие строки
  -V, --version             напечатать информацию о версии и выйти
      --help                показать эту справку и выйти
      --mmap                при возможности, использовать ввод,
                            спроецированный в память
 
Управление выводом:
  -m, --max-count=ЧИСЛО     остановиться после указанного ЧИСЛА совпадений
  -b, --byte-offset         печатать вместе с выходными строками смещение в
                            байтах
  -n, --line-number         печатать номер строки вместе с выходными строками
      --line-buffered       сбрасывать буфер после каждой строки
  -H, --with-filename       печатать имя файла для каждого совпадения
  -h, --no-filename         не начинать вывод с имени файла
      --label=МЕТКА         выводить МЕТКУ в качестве имени файла для
                            стандартного ввода
  -o, --only-matching       показывать только часть строки, совпадающей с ШАБЛОНОМ
  -q, --quiet, --silent     подавить весь обычный вывод
      --binary-files=ТИП    считать, что двоичный файл ТИПА:
                            binary, text или without-match.
  -a, --text                то же что и --binary-files=text
  -I                        то же, что и --binary-files=without-match
  -d, --directories=ДЕЙСТВ  как обрабатывать каталоги
                            ДЕЙСТВИЕ может быть read (читать),
                            recurse (рекурсивно), или skip (пропускать).
  -D, --devices=ДЕЙСТВ      как обрабатывать устройства, FIFO и сокеты
                            ДЕЙСТВИЕ может быть 'read' или 'skip'
  -R, -r, --recursive       то же, что и --directories=recurse
      --include=Ф_ШАБЛОН    обработать только файлы, подпадающие под Ф_ШАБЛОН
      --exclude=Ф_ШАБЛОН    пропустить файлы и каталоги,
                            подпадающие под Ф_ШАБЛОН
      --exclude-from=ФАЙЛ   пропустить файлы, подпадающие под шаблон
                            файлов из ФАЙЛА
      --exclude-dir=ШАБЛОН  каталоги, подпадающие под ШАБЛОН,
                            будут пропущены
  -L, --files-without-match печатать только имена ФАЙЛОВ без совпадений
  -l, --files-with-matches  печатать только имена ФАЙЛОВ с совпадениями
  -c, --count               печатать только количество совпадающих
                            строк на ФАЙЛ
  -T, --initial-tab         выравнивать табуляцией (если нужно)
  -Z, --null                печатать байт 0 после имени ФАЙЛА
   -E, --extended-regexp     ШАБЛОН - расширенное регулярное выражение (ERE)
  -F, --fixed-regexp        ШАБЛОН - строки фиксированной длины, разделённые
                            символом новой строки
  -G, --basic-regexp        ШАБЛОН - простое регулярное выражение (BRE)
  -P, --perl-regexp         ШАБЛОН - регулярное выражения языка Perl
   -e, --regexp=ШАБЛОН       использовать ШАБЛОН для поиска
  -f, --file=ФАЙЛ           брать ШАБЛОН из ФАЙЛа
  -i, --ignore-case         игнорировать различие регистра
  -w, --word-regexp         ШАБЛОН должен подходить ко всем словам
  -x, --line-regexp         ШАБЛОН должен подходить ко всей строке
  -z, --null-data           строки разделяются байтом с нулевым значением, а не
                            символом конца строки
 %s: недопустимый параметр -- %c
 %s: неверный параметр -- %c
 %s: у параметра `%c%s' не может быть значения
 %s: параметр '%s' неоднозначен
 %s: для параметра `%s' требуется значение
 %s: у параметра `--%s' не может быть значения
 %s: у параметра `-W %s' не может быть значения
 %s: параметр `-W %s' неоднозначен
 %s: параметру требуется значение -- %c
 %s: неизвестный параметр `%c%s'
 %s: неизвестный параметр `--%s'
 " (стандартный ввод) Двоичный файл %s совпадает
 Copyright (C) %s Free Software Foundation, Inc.
Лицензия GPLv3+: GNU GPL версии 3 или новее <http://gnu.org/licenses/gpl.html>
Это свободное ПО: вы можете продавать и распространять его.
Нет НИКАКИХ ГАРАНТИЙ до степени, разрешённой законом.
 Пример: %s -i 'hello world' menu.h main.c

Выбор типа регулярного выражения и его интерпретация:
 В GREP_COLORS="%s" значение "%s" присвоено свойству %s. В GREP_COLORS="%s" свойство "%s" является логическим и не требует значения ("=%s"); пропускается. В GREP_COLORS="%s" для свойства "%s" требуется указать значение ("=..."); пропускается. Неправильная обратная ссылка Неправильный символ имени класса Неправильный символ сравнения Неправильное содержимое в \{\} Неправильное предшествующее регулярное выражение Неправильный конец диапазона Неправильное регулярное выражение Запуск под именем egrep устарел; вместо этого используйте grep -E.
 Запуск под именем fgrep устарел; вместо этого используйте grep -F.
 Память исчерпана Нет совпадений Нет предыдущего регулярного выражения Не указано синтаксиса ШАБЛОН представляет строки фиксированной длины, разделённые символом новой строки.
 ШАБЛОН представляет собой расширенное регулярное выражение (ERE).
 По умолчанию, ШАБЛОН представляет собой простое регулярное выражение (BRE).
 Преждевременное завершение регулярного выражения Регулярное выражение слишком большое Поиск ШАБЛОНА в каждом ФАЙЛЕ или в стандартном вводе.
 Прекращение обработки неправильной GREP_COLORS="%s", начиная с оставшейся подстроки "%s". Выполнено успешно Поддержка параметра -P не включена при компиляции данного исполняемого файла (--disable-perl-regexp) Параметры -P и -z не совместимы Параметр -P поддерживает только одиночный шаблон Завершающий символ обратной косой черты Попробуйте `%s --help' для получения более подробного описания.
 Несбалансированная ( Несбалансированная ) Несбалансированная [ Незавершённая \ последовательность Неизвестная системная ошибка Непарная ( или \( Непарная ) или \) Непарная [ или [^ Непарная \{ Использование: %s [ПАРАМЕТР]... ШАБЛОН [ФАЙЛ]...
 Когда не задан ФАЙЛ, или когда ФАЙЛ это -, то читается стандартный ввод.
Если указано меньше, чем два файла, то предполагает -h. При нахождении
совпадений кодом завершения программы будет 0, и 1, если нет.При возникновении
ошибок, или если не указан параметр -q, кодом завершения будет 2.
 " Вместо egrep предполагается запуск grep -E. Вместо fgrep предполагается grep -F.
Запуск под именами egrep или fgrep лучше не выполнять.
 заданы конфликтующие образцы входные данные слишком велики, чтобы сосчитать неверный аргумент длины контекста неверное максимальное количество совпадений некорректно указано количество повторений память исчерпана каталоги зациклены незавершённое количество повторений неизвестный тип binary-files неизвестный метод для устройств неизвестный метод для каталогов предупреждение: %s: %s
 ошибка записи запись выходных данных 