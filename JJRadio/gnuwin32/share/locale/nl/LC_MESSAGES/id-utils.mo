��    W      �     �      �     �     �     �  ,   �     �  %     ,   <  &   i     �     �     �  A   �  e  '	    �
     �  �   �     �     �    �     �     �  
               |  ;  9  �  >  �     1     =     P  �   ]       &     (   ?  !   h      �  #   �     �     �  %   �  .        G     V  9   ]  6   �  .   �     �          /     A  (   [     �  
   �     �     �     �     �  0         9      K   !   [   )   }   (   �   !   �      �      !  	   !     &!     9!     >!     Z!  
   y!     �!  2   �!     �!     �!  '   �!  (    "  3   I"     }"     �"     �"     �"  "   �"  #   �"  D   #  O  ]#     �$     �$     �$  +   �$     %  $   ?%  +   d%  %   �%     �%     �%     �%  H   &  �  L&    �'     �+  a  �+     ]-     v-  '  �-     �.     �.  
   �.     �.  )   �.  |  /  _  �1  >  �2     ::     P:     f:  �   x:     4;  *   C;  1   n;  "   �;  $   �;  $   �;     <      <  .   5<  0   d<     �<  	   �<  =   �<  A   �<  =   2=     p=  $   �=     �=     �=  4   �=     >     4>  ,   S>     �>     �>  8   �>  6   �>     %?     >?      S?  &   t?  (   �?     �?  $   �?  0   	@     :@     >@     T@     \@      z@     �@     �@  )   �@     �@     �@  .   A  -   IA  H   wA     �A     �A     �A     �A  +   B  ,   BB  ]   oB     I                D   =   J   V       '      S   7       (       M          :       0   )   %           
       G   -       K              E   T             9         8       1                  A   O                      B       #   *      ?   R          5       L   	   /   <   H   4      ,          U                        3   $       2          6   C   W   +       F                     &   N   "   >   ;   P           .             Q               !                @         new = %d/%d %s: illegal option -- %c
 %s: invalid option -- %c
 %s: option `%c%s' doesn't allow an argument
 %s: option `%s' is ambiguous
 %s: option `%s' requires an argument
 %s: option `--%s' doesn't allow an argument
 %s: option requires an argument -- %c
 %s: unrecognized option `%c%s'
 %s: unrecognized option `--%s'
 , Freq=%ld/%ld=%.2f
 All identifiers are non-ambiguous within the first %d characters
 Assembly language:
  -c,--comment=CHARS     Any of CHARS starts a comment until end-of-line
  -k,--keep=CHARS        Allow CHARS in tokens, and keep the result
  -i,--ignore=CHARS      Allow CHARS in tokens, and toss the result
  -u,--strip-underscore  Strip a leading underscore from tokens
  -n,--no-cpp            Don't handle C pre-processor directives
 Build an identifier database.
  -o, --output=OUTFILE    file name of ID database output
  -f, --file=OUTFILE      synonym for --output
  -i, --include=LANGS     include languages in LANGS (default: "C C++ asm")
  -x, --exclude=LANGS     exclude languages in LANGS
  -l, --lang-option=L:OPT pass OPT as a default for language L (see below)
  -m, --lang-map=MAPFILE  use MAPFILE to map file names onto source language
  -d, --default-lang=LANG make LANG the default source language
  -p, --prune=NAMES       exclude the named files and/or directories
  -v, --verbose           report per file statistics
  -s, --statistics        report statistics at end of run

      --help              display this help and exit
      --version           output version information and exit

FILE may be a file name, or a directory name to recursively search.
If no FILE is given, the current directory is searched by default.
Note that the `--include' and `--exclude' options are mutually-exclusive.

The following arguments apply to the language-specific scanners:
 Bytes=%ld Kb,  C language:
  -k,--keep=CHARS        Allow CHARS in single-token strings, keep the result
  -i,--ignore=CHARS      Allow CHARS in single-token strings, toss the result
  -u,--strip-underscore  Strip a leading underscore from single-token strings
 Collisions=%ld/%ld=%.0f%% Comment=%ld
 List identifiers that occur in FILENAME, or if FILENAME2 is
also given list the identifiers that occur in both files.

  -f, --file=FILE  file name of ID database
      --help       display this help and exit
      --version    output version information and exit
 Literal=%ld,  Load=%ld/%ld=%.0f%%,  Name=%ld,  Number=%ld,  Output=%ld (%ld tok, %ld hit)
 Print all tokens found in a source file.
  -i, --include=LANGS     include languages in LANGS (default: "C C++ asm")
  -x, --exclude=LANGS     exclude languages in LANGS
  -l, --lang-option=L:OPT pass OPT as a default for language L (see below)
  -m, --lang-map=MAPFILE  use MAPFILE to map file names onto source language
  -d, --default-lang=LANG make LANG the default source language
  -p, --prune=NAMES       exclude the named files and/or directories
      --help              display this help and exit
      --version           output version information and exit

The following arguments apply to the language-specific scanners:
 Print constituent file names that match PATTERN,
using shell-style wildcards.
  -f, --file=FILE        file name of ID database
  -S, --separator=STYLE  STYLE is one of `braces', `space' or `newline'
      --help             display this help and exit
      --version          output version information and exit
 Query ID database and report results.
By default, output consists of multiple lines, each line containing the
matched identifier followed by the list of file names in which it occurs.

  -f, --file=FILE       file name of ID database

  -i, --ignore-case     match PATTERN case insensitively
  -l, --literal         match PATTERN as a literal string
  -r, --regexp          match PATTERN as a regular expression
  -w, --word            match PATTERN as a delimited word
  -s, --substring       match PATTERN as a substring
            Note: If PATTERN contains extended regular expression meta-
            characters, it is interpreted as a regular expression substring.
            Otherwise, PATTERN is interpreted as a literal word.

  -k, --key=STYLE       STYLE is one of `token', `pattern' or `none'
  -R, --result=STYLE    STYLE is one of `filenames', `grep', `edit' or `none'
  -S, --separator=STYLE STYLE is one of `braces', `space' or `newline' and
                        only applies to file names when `--result=filenames'
            The above STYLE options control how query results are presented.
            Defaults are --key=token --result=filenames --separator=%s

  -F, --frequency=FREQ  find tokens that occur FREQ times, where FREQ
                        is a range expressed as `N..M'.  If N is omitted, it
                        defaults to 1, if M is omitted it defaults to MAX_USHRT
  -a, --ambiguous=LEN   find tokens whose names are ambiguous for LEN chars

  -x, --hex             only find numbers expressed as hexadecimal
  -d, --decimal         only find numbers expressed as decimal
  -o, --octal           only find numbers expressed as octal
            By default, searches match numbers of any radix.

      --help            display this help and exit
      --version         output version information and exit
 Rehash=%d,  Sorting tokens...
 String=%ld,  Text language:
  -i,--include=CHAR-CLASS  Treat characters of CHAR-CLASS as token constituents
  -x,--exclude=CHAR-CLASS  Treat characters of CHAR-CLASS as token delimiters
 Tokens=%ld,  Try `%s --help' for more information.
 Usage: %s [OPTION] FILENAME [FILENAME2]
 Usage: %s [OPTION]... PATTERN...
 Usage: %s [OPTION]... [FILE]...
 Usage: %s [OPTION]... [PATTERN]...
 Writing `%s'...
 `%s' is ambiguous `%s' is not an ID file! (bad magic #) `%s' is version %d, but I only grok version %d `%s' not found braces can't allocate %ld bytes for hash table: memory exhausted can't allocate language args obstack: memory exhausted can't allocate language args: memory exhausted can't chdir to `%s' can't chdir to `%s' from `%s' can't create `%s' can't create `%s' in `%s' can't determine the io_size of a string! can't exec `%s' can't fork can't get size of map file `%s' can't get working directory can't locate `ID' can't lstat `%s' from `%s' can't match regular-expression: memory exhausted can't modify `%s' can't open `%s' can't open language map file `%s' can't read directory `%s' (`.' from `%s') can't read entire language map file `%s' can't read language map file `%s' can't stat `%s' can't stat `%s' from `%s' directory edit? [y1-9^S/nq]  file invalid `--key' style: `%s' invalid `--result' style: `%s' junk: `%c' junk: `\%03o' language name expected following `%s' in file `%s' level %d: %ld/%ld = %.0f%%
 no file name arguments notice: `%s' was a %s, but is now a %s! notice: scan parameters changed for `%s' notice: use of `-e' is deprecated, use `-r' instead space too many file name arguments unknown I/O type: %d unrecognized language: `%s' unsupported size in io_read (): %d unsupported size in io_write (): %d warning: `%s' and `%s' are the same file, but yield different scans! Project-Id-Version: id-utils 3.2
Report-Msgid-Bugs-To: bug-id-utils@gnu.org
POT-Creation-Date: 2005-12-15 16:37+0100
PO-Revision-Date: 1998-09-30 13:31+02:00
Last-Translator: Ivo Timmermans <zarq@iname.com>
Language-Team: Dutch <nl@li.org>
MIME-Version: 1.0
Content-Type: text/plain; charset=ISO-8859-1
Content-Transfer-Encoding: 8bit
   nieuw = %d/%d %s: ongeldige optie -- %c
 %s: ongeldige optie -- %c
 %s: optie `%c%s' staat geen argumenten toe
 %s: optie `%s' is dubbelzinnig
 %s: optie `%s' vereist een argument
 %s: optie `--%s' staat geen argumenten toe
 %s: optie vereist een argument -- %c
 %s: onbekende optie `%c%s'
 %s: onbekende optie `--%s'
 , Freq=%ld/%ld=%.2f
 Alle namen van variabelen zijn niet dubbelzinnig in de eerste %d tekens
 Assembeertaal:
  -c,--comment=TEKENS    Een van TEKENS begint een commentaar tot einde van de regel
  -k,--keep=TEKENS       Sta TEKENS in eenheden toe, behoud resultaat
  -i,--ignore=TEKENS     Sta TEKENS in eenheden toe, gooi resultaat weg
  -u,--strip-underscore  Verwijder een voorgaande underscore `_' uit eenheden
  -n,--no-cpp            Behandel geen C pre-processor aanwijzingen
 Maak een namendatabase aan
  -o, --output=UITBESTAND bestandsnaam van databaseuitvoer
  -f, --file=OUTFILE      synonym for --output
  -i, --include=LANGS     include languages in LANGS (default: "C C++ asm")
  -x, --exclude=LANGS     exclude languages in LANGS
  -l, --lang-option=L:OPT pass OPT as a default for language L (see below)
  -m, --lang-map=MAPFILE  use MAPFILE to map file names onto source language
  -d, --default-lang=LANG make LANG the default source language
  -p, --prune=NAMES       exclude the named files and/or directories
  -v, --verbose           report per file statistics
  -s, --statistics        report statistics at end of run

      --help              display this help and exit
      --version           output version information and exit

FILE may be a file name, or a directory name to recursively search.
If no FILE is given, the current directory is searched by default.
Note that the `--include' and `--exclude' options are mutually-exclusive.

The following arguments apply to the language-specific scanners:
 Bytes=%ld Kb,  C taal:
  -k,--keep=TEKENS      Sta TEKENS toe in reeksen van een eenheid, behoud
                        resultaat
  -i,--ignore=TEKENS    Sta TEKENS toe in reeksen van een eenheid, gooi
                        resultaat weg
  -u,--strip-underscore Haal een voorgaand laag liggend streepje (`_')
                        weg van reeksen van een eenheid
 Botsingen=%ld/%ld=%.0f%% Commentaar=%ld
 Geef een lijst van namen die in BESTANDSNAAM voorkomen, of,
als BESTANDSNAAM2 ook gegeven is, de namen die in beide
bestanden voorkomen.

  -f, --file=BESTAND  bestandsnaam van ID database
      --help          geef deze hulp en be�indig
      --version       geef versie informatie en be�indig
 Letterlijk=%ld,  Belasting=%ld/%ld=%.0f%%,  Naam=%ld,  Nummer = %ld,  Uitvoer=%ld (%ld eenheden, %ld gevonden)
 Print all tokens found in a source file.
  -i, --include=LANGS     include languages in LANGS (default: "C C++ asm")
  -x, --exclude=LANGS     exclude languages in LANGS
  -l, --lang-option=L:OPT pass OPT as a default for language L (see below)
  -m, --lang-map=MAPFILE  use MAPFILE to map file names onto source language
  -d, --default-lang=LANG make LANG the default source language
  -p, --prune=NAMES       exclude the named files and/or directories
      --help              display this help and exit
      --version           output version information and exit

The following arguments apply to the language-specific scanners:
 Geef opbouwende bestandsnamen die overeenkomen met PATROON, gebruik
makend van jokertekens zoals van de shell.

  -f, --file=BESTAND     bestandsnaam van ID database
  -S, --separator=STIJL  STIJL is een uit `braces', `space' of `newline'
      --help             geef deze hulp en be�indig
      --version          geef versie informatie en be�indig
 Query ID database and report results.
By default, output consists of multiple lines, each line containing the
matched identifier followed by the list of file names in which it occurs.

  -f, --file=FILE       file name of ID database

  -i, --ignore-case     match PATTERN case insensitively
  -l, --literal         match PATTERN as a literal string
  -r, --regexp          match PATTERN as a regular expression
  -w, --word            match PATTERN as a delimited word
  -s, --substring       match PATTERN as a substring
            Note: If PATTERN contains extended regular expression meta-
            characters, it is interpreted as a regular expression substring.
            Otherwise, PATTERN is interpreted as a literal word.

  -k, --key=STYLE       STYLE is one of `token', `pattern' or `none'
  -R, --result=STYLE    STYLE is one of `filenames', `grep', `edit' or `none'
  -S, --separator=STYLE STYLE is one of `braces', `space' or `newline' and
                        only applies to file names when `--result=filenames'
            The above STYLE options control how query results are presented.
            Defaults are --key=token --result=filenames --separator=%s

  -F, --frequency=FREQ  find tokens that occur FREQ times, where FREQ
                        is a range expressed as `N..M'.  If N is omitted, it
                        defaults to 1, if M is omitted it defaults to MAX_USHRT
  -a, --ambiguous=LEN   find tokens whose names are ambiguous for LEN chars

  -x, --hex             only find numbers expressed as hexadecimal
  -d, --decimal         only find numbers expressed as decimal
  -o, --octal           only find numbers expressed as octal
            By default, searches match numbers of any radix.

      --help            display this help and exit
      --version         output version information and exit
 herverfrommeling=%d,  Eenheden sorteren...
 Letterreeks=%ld,  Tekst taal:
  -i,--include=TEKEN-KLASSE  Behandel tekens van TEKEN-KLASSE als onderdeel van een eenheid
  -x,--exclude=TEKEN-KLASSE  Behandel tekens van TEKEN-KLASSE als eenheidscheiding
 Eenheden=%ld,  Probeer `%s --help' voor meer informatie.
 Aanroep: %s [OPTIE] BESTANDSNAAM [BESTANDSNAAM2]
 Aanroep: %s [OPTIE]... PATROON...
 Aanroep: %s [OPTIE]... [BESTAND]...
 Aanroep: %s [OPTIE]... [PATROON]...
 `%s' schrijven...
 `%s' is dubbelzinnig `%s' is geen ID bestand! (fout magisch nummer) `%s' is versie %d, maar ik lust alleen versie %d `%s' niet gevonden accolades kan geen %ld bytes voor frommeltabel reserveren: geheugen vol kan geen objectstapel voor taalargumenten vrijmaken: geheugen vol kan geen geheugen vrijmaken voor taalargumenten: geheugen vol kan niet naar map `%s' gaan kan niet van map `%s' naar `%s' gaan kan `%s' niet aanmaken kan `%s' niet aanmaken in `%s' kan de io_size van een letterreeks niet vaststellen! kan `%s' niet uitvoeren kan geen nieuw proces beginnen kan grootte van mapbestand `%s' niet krijgen kan huidige map niet verkrijgen kan `ID' niet vinden kan de status van de link `%s' niet opvragen vanuit `%s' kan reguliere expressie niet vergelijken: geheugen vol kan `%s' niet veranderen kan `%s' niet openen kan taalbestand `%s' niet openen kan map `%s' niet lezen (`.' van `%s') kan niet het hele taalbestand `%s' lezen kan taalbestand `%s' niet lezen kan de status van `%s' niet opvragen kan de status van `%s' niet opvragen vanuit `%s' map bewerken? [j1-9^S/nq] bestand ongeldige `--key' stijl: `%s' ongeldige `--result' stijl: `%s' onzin: `%c' onzin: `\%03o' taalnaam verwacht na `%s' in bestand `%s' niveau %d: %ld/%ld = %.0f%%
 geen bestandsnaam argumenten opmerking: `%s' was een %s, maar is nu een %s! opmerking: leesparameters veranderd voor `%s' opmerking: gebruik van `-e' is verouderd, gebruik `-r' in plaats daarvan spatie te veel bestandsnaam argumenten onbekend I/O type: %d niet herkende taal: `%s' niet-ondersteunde afmeting in io_read(): %d niet-ondersteunde afmeting in io_write(): %d waarschuwing: `%s' en `%s' zijn het zelfde bestand, maar leveren verschillende resultaten op! 