��    N      �  k   �      �  6  �  0  �    
  %    k  E     �     �  ,   �       %   0  ,   V  -   �      �  &   �     �          9     ;     L  �   d  Q   \  *   �  [   �  G   5     }     �     �     �  $   �     
       <   7  <   t     �     �     �     �  5   �  1   4  :   f  #   �     �  3   �  N        c  P   k  (   �  ,   �       &   %     L     Y     f     s     �     �     �     �     �  (   �  �        �  q   �     Z     y     �     �     �     �     �                :     Q     l     }     �  E  �  c  �  >  B"  �  �#  .  W+  �  �,     X.     v.  +   �.     �.  )   �.  +   	/  /   5/  !   e/  *   �/  $   �/  $   �/     �/     �/     0    /0  U   D1  *   �1  c   �1  M   )2  #   w2  $   �2  "   �2     �2  *    3     +3     E3  :   e3  :   �3     �3     �3  '   4     +4  A   H4  1   �4  H   �4  (   5  "   .5  6   Q5  Y   �5     �5  S   �5  /   ?6  %   o6     �6  .   �6     �6     �6     �6     7     7     :7     V7     r7     �7  '   �7    �7     �8  u   �8  '   T9  +   |9  1   �9     �9      �9     :     &:      C:      d:     �:  #   �:     �:     �:     �:         C      *   I         !   (           &                    4   5   <   '      -             "       B   =   %   $              K   H   #      :                     J              @   L   /            .   A            0       3   9   ,   F   ;   8                            +   
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
 write error writing output Project-Id-Version: grep-2.5.4
Report-Msgid-Bugs-To: 
POT-Creation-Date: 2009-02-03 14:51-0400
PO-Revision-Date: 2009-01-30 18:22+0100
Last-Translator: Milo Casagrande <milo@casagrande.name>
Language-Team: Italian <tp@lists.linux.it>
MIME-Version: 1.0
Content-Type: text/plain; charset=UTF-8
Content-Transfer-Encoding: 8-bit
 
Controllo del contesto:
  -B, --before-context=NUM  Stampa NUM righe di contesto precedente
  -A, --after-context=NUM   Stampa NUM righe di contesto seguente
  -C, --context=NUM         Stampa NUM righe di contesto dell'output
  -NUM                      Come --context=NUM
      --color[=QUANDO]
      --colour[=QUANDO]     Usa i colori per distinguere le stringhe corrispondenti.
                            QUANDO può essere "always", "never" o "auto".
  -U, --binary              Non rimuove i caratteri CR all'EOL (MSDOS)
  -u, --unix-byte-offsets   Segnala gli offset come se non ci fossero CR (MSDOS)

 
Varie:
  -s, --no-messages         Elimina i messaggi di errore
  -v, --invert-match        Seleziona le righe che non corrispondono
  -V, --version             Stampa la versione ed esce
      --help                Visualizza questo aiuto ed esce
      --mmap                Se possibile mappa l'input nella memoria
 
Controllo dell'output:
  -m, --max-count=NUM       Si ferma dopo NUM corrispondenze
  -b, --byte-offset         Stampa l'offset del byte con le righe di output
  -n, --line-number         Stampa il numero della riga con le righe di output
      --line-buffered       Fa il flush dell'output con ogni riga
  -H, --with-filename       Stampa il nome del file per ogni corrispondenza
  -h, --no-filename         Elimina il nome del file davanti all'output
      --label=ETICH         Stampa ETICH al posto del nome del file per lo standard
                            input
  -o, --only-matching       Mostra solo la parte della riga corrispondente al
                            MODELLO
  -q, --quiet, --silent     Elimina tutto l'output normale
      --binary-files=TIPO   Suppone che i file binari siano del TIPO "binary",
                            "text" oppure "without-match"
  -a, --text                Equivale a --binary-files=text
  -I                        Equivale a --binary-files=without-match
  -d, --directories=AZIONE  Come gestire le directory: AZIONE è "read", "recurse"
                            o "skip"
  -D, --devices=AZIONE      Come gestire device, FIFO e socket: AZIONE è "read"
                            o "skip" 
  -R, -r, --recursive       Equivale a --directories=recurse
      --include=MODELLO     Esamina i file corrispondenti al MODELLO
      --exclude=MODELLO     Salta i file corrispondenti al MODELLO
      --exclude-from=FILE   Salta i file corrispondenti ai modelli nel FILE
      --exclude-dir=MODELLO Salta le directory corrispondenti al MODELLO
  -L, --files-without-match Stampa solo i nomi dei FILE senza corrispondenze
  -l, --files-with-matches  Stampa solo i nomi dei FILE contenenti corrispondenze
  -c, --count               Stampa solo il conteggio delle righe occorrenti in ogni
                            FILE
  -T, --initial-tab         Allinea le tabulazioni (se necessario)
  -Z, --null                Stampa il byte 0 dopo ogni nome di FILE
   -E, --extended-regexp     MODELLO è un'espressione regolare estesa
  -F, --fixed-strings       MODELLO è un insieme di stringhe letterali separate da newline
  -G, --basic-regexp        MODELLO è un'espressione regolare semplice
  -P, --perl-regexp         MODELLO è un'espressione regolare Perl
   -e, --regexp=MODELLO      Usa MODELLO per la corrispondenza
  -f, --file=FILE           Ottiene il MODELLO dal FILE
  -i, --ignore-case         Ignora la distinzione tra maiuscole e minuscole
  -w, --word-regexp         Forza MODELLO a corrispondere solo a parole intere
  -x, --line-regexp         Forza MODELLO a corrispondere solo a righe intere
  -z, --null-data           Una riga di dati termina con il byte 0 invece che
                            newline
 %s: opzione non lecita -- %c
 %s: opzione non valida -- %c
 %s: l'opzione "%c%s" non accetta argomenti
 %s: l'opzione "%s" è ambigua
 %s: l'opzione "%s" richiede un argomento
 %s: l'opzione "--%s" non accetta argomenti
 %s: l'opzione "-W %s" non accetta un argomento
 %s: l'opzione "-W %s" è ambigua
 %s: l'opzione richiede un argomento -- %c
 %s: opzione "%c%s" non riconosciuta
 %s: opzione "--%s" non riconosciuta
 " (standard input) Il file binario %s corrisponde
 Copyright © %s Free Software Foundation, Inc.
Licenza GPLv3+: GNU GPL versione 3 o successive <http://gnu.org/licenses/gpl.html>
Questo è software libero: siete liberi di modificarlo e ridistribuirlo.
Non c'è ALCUNA GARANZIA, per quanto consentito dalle vigenti normative.
 Esempio: %s -i "ciao mondo" menu.h main.c

Selezione e interpretazione delle regexp:
 In GREP_COLORS="%s", la capacità "%s" %s. In GREP_COLORS="%s", la capacità "%s" è booleana e non può accettare un valore ("=%s"); saltato. In GREP_COLORS="%s", la capacità "%s" necessita un valore ("=..."); saltato. Riferimento all'indietro non valido Nome classe del carattere non valido Carattere di collazione non valido Contenuto di \{\} non valido Espressione regolare precedente non valida Limite massimo non valido Espressione regolare non valida L'invocazione come "egrep" è deprecata, usare "grep -E".
 L'invocazione come "fgrep" è deprecata, usare "grep -F".
 Memoria esaurita Nessuna corrispondenza Nessuna espressione regolare precedente Nessuna sintassi specificata MODELLO è un insieme di stringhe letterali separate da newline.
 MODELLO è un'espressione regolare estesa (ERE).
 MODELLO è, in modo predefinito, un'espressione regolare di base (BRE).
 Fine prematura dell'espressione regolare Espressione regolare troppo grande Cerca il MODELLO in ogni FILE o nello standard input.
 Elaborazione dei GREP_COLORS="%s" mal formati nelle sottostringhe "%s" rimanenti fermata. Successo Il supporto all'opzione -P non è compilato in questo binario --disable-perl-regexp Le opzioni -P e -z non possono essere combinate L'opzione -P supporta un solo modello Backslash finale Usare "%s --help" per ulteriori informazioni.
 ( non bilanciata ) non bilanciata [ non bilanciata Escape \ incompleto Errore di sistema sconosciuto ( o \( senza corrispondenza ) o \) senza corrispondenza [ o [^ senza corrispondenza \{ senza corrispondenza Uso: %s [OPZIONE]... MODELLO [FILE]...
 Se non c'è alcun FILE o il FILE è -, legge lo standard input. Se sono stati
specificati meno di due FILE presume -h. Esce con lo stato 0 se è stata
selezionata almeno una riga, 1 altrimenti. Se si verifica un errore e l'opzione
-q non è stata usata, lo stato è 2.
 " "egrep" significa "grep -E", "fgrep" significa "grep -F".
L'invocazione diretta come "egrep" o "fgrep" è deprecata.
 specificate corrispondenze in conflitto l'input è troppo grande per essere contato argomento della lunghezza del contesto non valido numero massimo non valido conteggio ripetizioni malformato memoria esaurita ciclo ricorsivo di directory conteggio ripetizioni incompleto tipo di file binario sconosciuto metodo per i device sconosciuto metodo per le directory sconosciuto attenzione: %s: %s
 errore di scrittura scrittura dell'output 