��       �     D   �  
l      �   ~  �  p  h   �  �   F  �   I  �        0  9     j     |     �   ,  �     �   %  �   ,  !   -  N      |   &  �     �     �   L     J  Q   -  �   5  �   (      L  )     v   D  �   ?  �   B     D  X     �   I  �   =     A  E   J  �   =  �   8     9  I   C  �   F  �   (     @  7   B  x   M  �   K  	   8  U   ,  �   J  �   9     /  @   0  p   K  �   $  �   G     )  Z  V  �   9  �        G  4   A  |   <  �   .  �   C  *   ,  n   ?  �   <  �   E     B  ^   %  �   5  �   F  �   .   D   >   s   >   �   A   �   8  !3   3  !l   #  !�   /  !�   D  !�   /  "9   4  "i   �  "�   !  #�     #�   !  #�     #�   I  $   &  $O     $v     $�   I  $�   1  $�   &  %)     %P     %g     %�     %�   $  %�     %�     %�     &
     &     &$     &>     &]   #  &m     &�   '  &�   �  &�   =  '�   '  '�     '�     '�   %  (     (-     (B     (T     (f     (x   "  (�   4  (�     (�   .  (�   "  ))   (  )L   *  )u   )  )�   !  )�   '  )�   )  *     *>      *N      *o     *�     *�  A  *�     ,     ,     ,0   1  ,B   2  ,t   0  ,�     ,�   #  ,�     -   )  -3   1  -]   -  -�     -�     -�     -�     .     .   "  .-   %  .P     .v     .�     .�     .�     .�   !  .�     .�     /  \  /   e  0v  7  0�   �  2   G  2�   4  2�   #  3   1  3A     3s     3�     3�   -  3�     3�   '  4   -  48   0  4f   "  4�   '  4�   '  4�   '  5
   @  52   =  5s   (  5�   7  5�   *  6   I  6=     6�   8  6�   9  6�   4  7   <  7H     7�   ?  7�   =  7�   +  8"   G  8N   A  8�   5  8�   ,  9   2  9;   F  9n   6  9�   0  9�   6  :   C  :T   E  :�   :  :�   0  ;   K  ;J   =  ;�   &  ;�   $  ;�   J  <    &  <k   -  <�   '  <�  .  <�   ;  >     >S   E  >r   7  >�   5  >�   2  ?&   @  ?Y   ,  ?�   )  ?�   *  ?�   5  @   H  @R      @�   0  @�   6  @�   1  A$   =  AV   5  A�   :  A�   1  B   +  B7   %  Bc   &  B�   9  B�   (  B�   4  C   �  CH   !  C�   #  D   #  D?   '  Dc   E  D�     D�     D�     E   =  E!   5  E_   +  E�     E�     E�     E�     E�     F	     F     F+     F>     F[     Fh     F�     F�     F�     F�   ;  F�   �  G   B  G�     H      H     H   5  H,     Hb     H}     H�     H�     H�   /  H�   I  H�   !  I<   3  I^   2  I�   =  I�   )  J      J-   %  JN   (  Jt      J�     J�   &  J�   "  J�   "  K     KB   �  Ka     LZ     Lq     L�   (  L�   &  L�   &  L�     M      M*     MK     M`      M      M�     M�     M�     M�     M�   "  N     N4   ,  NS     N�   
  N�     N�     N�     N�   $  N�   (  O    
  O)          �   v          �             �                 0   y      1      F   P       o      S                  #       a              j   8   A   x   <       \   D           g   �   Y       T       K      �   E   )   @   U   d   �   �       ^   c   2   R   �   �   �      /      *       b       	   |   �          `   i         4          Q   }       �   f   :   �      r   �       L   
          +       q       �          �       �   �   �       $   �       ?       X   �   u   .   �   �   n       6   �   �   [   M       J   '          O   �          "   Z   7       �   �       (   W      G       V       z       C   {   �   �   _   B   5   %   �   �          m   �   s           N   �   h               ,           �       p   �   ;   =       t             I   ~              �   �       l       3   &   !   e         9       H           >   ]   -   k   w   Either GFMT or LFMT may contain:
    %%  %
    %c'C'  the single character C
    %c'\OOO'  the character with octal code OOO   GFMT may contain:
    %<  lines from FILE1
    %>  lines from FILE2
    %=  lines common to FILE1 and FILE2
    %[-][WIDTH][.[PREC]]{doxX}LETTER  printf-style spec for LETTER
      LETTERs are as follows for new group, lower case for old group:
        F  first line number
        L  last line number
        N  number of lines = L-F+1
        E  F-1
        M  L+1   LFMT may contain:
    %L  contents of line
    %l  contents of line, excluding any trailing newline
    %[-][WIDTH][.[PREC]]{doxX}n  printf-style spec for input line number   LTYPE is `old', `new', or `unchanged'.  GTYPE is LTYPE or `changed'.   Skip the first SKIP1 bytes of FILE1 and the first SKIP2 bytes of FILE2. %s %s differ: byte %s, line %s
 %s %s differ: byte %s, line %s is %3o %s %3o %s
 %s: diff failed:  %s: illegal option -- %c
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
 --GTYPE-group-format=GFMT  Similar, but format GTYPE input groups with GFMT. --LTYPE-line-format=LFMT  Similar, but format LTYPE input lines with LFMT. --binary  Read and write data in binary mode. --diff-program=PROGRAM  Use PROGRAM to compare files. --from-file and --to-file both specified --from-file=FILE1  Compare FILE1 to all operands.  FILE1 can be a directory. --help  Output this help. --horizon-lines=NUM  Keep NUM lines of the common prefix and suffix. --ignore-file-name-case  Ignore case when comparing file names. --line-format=LFMT  Similar, but format all input lines with LFMT. --no-ignore-file-name-case  Consider case when comparing file names. --normal  Output a normal diff. --speed-large-files  Assume large files and many scattered small changes. --strip-trailing-cr  Strip trailing carriage return on input. --tabsize=NUM  Tab stops are every NUM (default 8) print columns. --to-file=FILE2  Compare all operands to FILE2.  FILE2 can be a directory. --unidirectional-new-file  Treat absent first files as empty. -3  --easy-only  Output unmerged nonoverlapping changes. -A  --show-all  Output all changes, bracketing conflicts. -B  --ignore-blank-lines  Ignore changes whose lines are all blank. -D NAME  --ifdef=NAME  Output merged file to show `#ifdef NAME' diffs. -D option not supported with directories -E  --ignore-tab-expansion  Ignore changes due to tab expansion. -E  --show-overlap  Output unmerged changes, bracketing conflicts. -H  --speed-large-files  Assume large files and many scattered small changes. -I RE  --ignore-matching-lines=RE  Ignore changes whose lines all match RE. -L LABEL  --label=LABEL  Use LABEL instead of file name. -N  --new-file  Treat absent files as empty. -S FILE  --starting-file=FILE  Start with FILE when comparing directories. -T  --initial-tab  Make tabs line up by prepending a tab. -W  --ignore-all-space  Ignore all white space. -X  Output overlapping changes, bracketing them. -X FILE  --exclude-from=FILE  Exclude files that match any pattern in FILE. -a  --text  Treat all files as text. -b  --ignore-space-change  Ignore changes in the amount of white space. -b  --print-bytes  Print differing bytes. -c  -C NUM  --context[=NUM]  Output NUM (default 3) lines of copied context.
-u  -U NUM  --unified[=NUM]  Output NUM (default 3) lines of unified context.
  --label LABEL  Use LABEL instead of file name.
  -p  --show-c-function  Show which C function each change is in.
  -F RE  --show-function-line=RE  Show the most recent line matching RE. -d  --minimal  Try hard to find a smaller set of changes. -e  --ed  Output an ed script. -e  --ed  Output unmerged changes from OLDFILE to YOURFILE into MYFILE. -i  --ignore-case  Consider upper- and lower-case to be the same. -i  --ignore-case  Ignore case differences in file contents. -i  Append `w' and `q' commands to ed scripts. -i SKIP  --ignore-initial=SKIP  Skip the first SKIP bytes of input. -i SKIP1:SKIP2  --ignore-initial=SKIP1:SKIP2 -l  --left-column  Output only the left column of common lines. -l  --paginate  Pass the output through `pr' to paginate it. -l  --verbose  Output byte numbers and values of all differing bytes. -m  --merge  Output merged file instead of ed script (default -A). -n  --rcs  Output an RCS format diff. -n LIMIT  --bytes=LIMIT  Compare at most LIMIT bytes. -o FILE  --output=FILE  Operate interactively, sending output to FILE. -q  --brief  Output only whether files differ. -r  --recursive  Recursively compare any subdirectories found. -s  --quiet  --silent  Output nothing; yield exit status only. -s  --report-identical-files  Report when two files are the same. -s  --suppress-common-lines  Do not output common lines. -t  --expand-tabs  Expand tabs to spaces in output. -v  --version  Output version info. -w  --ignore-all-space  Ignore all white space. -w NUM  --width=NUM  Output at most NUM (default 130) print columns. -x  --overlap-only  Output overlapping changes. -x PAT  --exclude=PAT  Exclude files that match PAT. -y  --side-by-side  Output in two columns.
  -W NUM  --width=NUM  Output at most NUM (default 130) print columns.
  --left-column  Output only the left column of common lines.
  --suppress-common-lines  Do not output common lines. Common subdirectories: %s and %s
 Compare files line by line. Compare three files line by line. Compare two files byte by byte. FILES are `FILE1 FILE2' or `DIR1 DIR2' or `DIR FILE...' or `FILE... DIR'. File %s is a %s while file %s is a %s
 Files %s and %s are identical
 Files %s and %s differ
 If --from-file or --to-file is given, there are no restrictions on FILES. If a FILE is `-' or missing, read standard input. If a FILE is `-', read standard input. Invalid back reference Invalid character class name Invalid collation character Invalid content of \{\} Invalid preceding regular expression Invalid range end Invalid regular expression Memory exhausted No match No newline at end of file No previous regular expression Only in %s: %s
 Premature end of regular expression Regular expression too big Report bugs to <bug-gnu-utils@gnu.org>. SKIP values may be followed by the following multiplicative suffixes:
kB 1000, K 1024, MB 1,000,000, M 1,048,576,
GB 1,000,000,000, G 1,073,741,824, and so on for T, P, E, Z, Y. SKIP1 and SKIP2 are the number of bytes to skip in each file. Side-by-side merge of file differences. Success Trailing backslash Try `%s --help' for more information. Unknown system error Unmatched ( or \( Unmatched ) or \) Unmatched [ or [^ Unmatched \{ Usage: %s [OPTION]... FILE1 FILE2
 Usage: %s [OPTION]... FILE1 [FILE2 [SKIP1 [SKIP2]]]
 Usage: %s [OPTION]... FILES
 Usage: %s [OPTION]... MYFILE OLDFILE YOURFILE
 `-%ld' option is obsolete; omit it `-%ld' option is obsolete; use `-%c %ld' `-' specified for more than one input file both files to be compared are directories cannot compare `-' to a directory cannot compare file names `%s' and `%s' cannot interactively merge standard input cmp: EOF on %s
 conflicting %s option value `%s' conflicting output style options conflicting tabsize options conflicting width options ed:	Edit then use both versions, each decorated with a header.
eb:	Edit then use both versions.
el:	Edit then use the left version.
er:	Edit then use the right version.
e:	Edit a new version.
l:	Use the left version.
r:	Use the right version.
s:	Silently include common lines.
v:	Verbosely include common lines.
q:	Quit.
 extra operand `%s' incompatible options input file shrank internal error: invalid diff type in process_diff internal error: invalid diff type passed to output internal error: screwup in format of diff blocks invalid --bytes value `%s' invalid --ignore-initial value `%s' invalid context length `%s' invalid diff format; incomplete last line invalid diff format; incorrect leading line chars invalid diff format; invalid change separator invalid horizon length `%s' invalid tabsize `%s' invalid width `%s' memory exhausted missing operand after `%s' options -l and -s are incompatible pagination not supported on this host program error read failed stack overflow standard output subsidiary program `%s' failed subsidiary program `%s' not found too many file label options write failed Project-Id-Version: GNU diffutils 2.8.2
Report-Msgid-Bugs-To: 
POT-Creation-Date: 2004-04-13 00:07-0700
PO-Revision-Date: 2002-06-21 07:56+0900
Last-Translator: IIDA Yosiaki <iida@gnu.org>
Language-Team: Japanese <translation-team-ja@lists.sourceforge.net>
MIME-Version: 1.0
Content-Type: text/plain; charset=EUC-JP
Content-Transfer-Encoding: 8bit
   GFMT��LFMT�ˤϰʲ������Ǥ��ޤ�:
    %%  %
    %c'C'  ʸ��C��ʸ��
    %c'\OOO'  8�ʥ�����OOO��ʸ��   GFMT�ˤϰʲ������Ǥ��ޤ�:
    %<  FILE1�ι�
    %>  FILE2�ι�
    %=  FILE1��FILE2�ζ��̹�
    %[-][WIDTH][.[PREC]]{doxX}LETTER  printf������Ǥ�LETTER
      LETTER�ϡ��ʲ���ʸ���ǿ�����������ʸ���ǸŤ���:
        F  �ǽ�ι��ֹ�
        L  �Ǹ�ι��ֹ�
        N  �Կ� = L-F+1
        E  F-1
        M  L+1   LFMT�ˤϰʲ������Ǥ��ޤ�:
    %L  ������
    %l  ��³������Ԥ�Τ�����������
    %[-][WIDTH][.[PREC]]{doxX}n  printf������Ǥ����ϹԿ�   ������LTYPE�ϡ�old�ס�new�ס�unchanged�ס�GTYPE��LTYPE����changed�ס�   FILE1��SKIP1�Х��Ȥ�FILE2��SKIP2�Х��Ȥ򥹥��åס� %s %s �ۤʤ�ޤ�: �Х��� %s���� %s
 %s %s �ۤʤ�ޤ�: �Х��� %s���� %s %3o %s %3o %s
 %s: diff����:  %s: �����ʥ��ץ����Ǥ� -- %c
 %s: ̵���ʥ��ץ����Ǥ� -- %c
 %s: ���ץ�����%c%s�פϰ���������դ��ޤ���
 %s: ���ץ�����%s�פ�ۣ��Ǥ�
 %s: ���ץ�����%s�פˤϰ�����ɬ�פǤ�
 %s: ���ץ�����--%s�פϰ���������դ��ޤ���
 %s: ���ץ�����-W %s�פϡ�����������դ��ޤ���
 %s: ���ץ�����-W %s�פ�ۣ��Ǥ�
 %s: ���ץ����ϰ������׵ᤷ�ޤ� -- %c
 %s: ǧ���Ǥ��ʤ����ץ�����%c%s�פǤ�
 %s: ǧ���Ǥ��ʤ����ץ�����--%s�פǤ�
 --GTYPE-group-format=GFMT  Ʊ�ͤ�����GTYPE�����Ϸ���GFMT�������� --LTYPE-line-format=LFMT  Ʊ�ͤ�����LTYPE���ϹԤ�LFMT�������� --binary  �Х��ʥ꡼���⡼�ɤ��ɤ߽񤭡� --diff-program=PROGRAM  �ե��������Ӥ�PROGRAM����ѡ� --from-file��--to-file��ξ������ꤷ�ޤ��� --from-file=FILE1  �����ڥ��ɤ�FILE1����ӡ�FILE1�ϥǥ��쥯�ȥ꡼��ġ� --help  ������������ϡ� --horizon-lines=NUM  ��Ƭ�������ˤ��붦�̤�NUM�Ԥ���ݡ� --ignore-file-name-case  �ե�����̾����ʸ����ʸ����̵�롣 --line-format=LFMT  Ʊ�ͤ����������ϹԤ�LFMT�������� --no-ignore-file-name-case  �ե�����̾����ʸ����ʸ������̡� --normal  ɸ��Ū�ʺ�ʬ����ϡ� --speed-large-files  ����ʥե������ʬ�������ѹ�������Ȳ��ꡣ --strip-trailing-cr  ���Ϥ�����������å����꥿��������� --tabsize=NUM  ���֤�NUM (�����8) ����ࡣ --to-file=FILE2  FILE2�������ڥ��ɤ���ӡ�FILE2�ϥǥ��쥯�ȥ꡼��ġ� --unidirectional-new-file  ¸�ߤ��ʤ����ԤΥե�����϶��Ȥߤʤ��� -3  --easy-only  ���ͤ��Ƥ��ʤ���̤ʻ����ѹ�����ϡ� -A  --show-all  ���ͤ򤯤��ꡢ���ѹ�����ϡ� -B  --ignore-blank-lines  ��������ιԤκ���̵�롣 -D NAME  --ifdef=NAME  ��ʬ���#ifdef NAME�פǼ���ʻ��ե��������ϡ� -D���ץ����ϥǥ��쥯�ȥ꡼�ǤΥ��ݡ��Ȥ򤷤Ƥ��ޤ��� -E  --ignore-tab-expansion  ����Ÿ���κ���̵�롣 -E  --show-overlap  ���ͤ򤯤��ꡢ̤ʻ����ѹ�����ϡ� -H  --speed-large-files  ����ʥե������ʬ�������ѹ�������Ȳ��ꡣ -i RE  --ignore-matching-lines=RE  RE�˰��פ��뤹�٤ƤιԤκ���̵�롣 -L LABEL  --label=LABEL  �ե�����̾�������LABEL����ѡ� -N  --new-file  ¸�ߤ��ʤ��ե�����϶��Ȥߤʤ��� -S FILE  --starting-file=FILE  �ǥ��쥯�ȥ꡼����Ӥ���ݡ�FILE����Ϥ�롣 -T  --initial-tab  ���֤ǻϤޤ�Ԥϡ����˥��֤��֤���·���롣 -W  --ignore-all-space  �������̵�롣 -X  ���ͤ򤯤���ʤ��顢�ѹ�����ϡ� -X FILE  --exclude-from=FILE  FILE��γƥѥ�����Ȱ��פ���ե����������� -a  --text  ���٤ƥƥ����ȤȤ��ƽ����� -b  --ignore-space-change  ������κ���̵�롣 -b  --print-bytes  �ۤʤ�Х��Ȥ�ɽ���� -c  -C NUM  --context[=NUM]  ���ԡ���������NUM�� (�����3) ����ϡ�
-u  -U NUM  --unified[=NUM]  ���礷������NUM�� (�����3) ����ϡ�
  --label LABEL  �ե�����̾�������LABEL����ѡ�
  -p  --show-c-function  ���ѹ���ޤ�C�δؿ�̾��ɽ����
  -F RE  --show-function-line=RE  RE�Ȱ��פ���ľ��ιԤ�ɽ���� -d  --minimal  �Ǥ����������Фäơ���������ʬ�򸫤Ĥ��롣 -e  --ed  ed������ץȤ���ϡ� -e  --ed  OLDFILE����YOURFILE�ؤ�MYFILE�ˤ�������̤ʻ����ѹ�����ϡ� -i  --ignore-case  �ե��������Ƥ���ʸ����ʸ����Ʊ��롣 -i  --ignore-case  �ե��������Ƥ���ʸ����ʸ����̵�롣 -i  ed������ץȤθ�ˡ�w�פȡ�q�ץ��ޥ�ɤ��ɲá� -i SKIP  --ignore-initial=SKIP  ���Ϥ���ƬSKIP�Х��Ȥ򥹥��åס� -i SKIP1:SKIP2  --ignore-initial=SKIP1:SKIP2 -l  --left-column  ���̹ԤϺ���������ϡ� -l  --paginate  ���Ϥ��pr�פǥڡ���ʬ�䡣 -l  --verbose  ���٤Ƥΰۤʤ�Х��Ȥλؿ����ͤ���ϡ� -m  --merge  ed������ץȤ�����ˡ�ʻ�礷���ե��������� (�����-A)�� -n  --rcs  RCS�����κ�ʬ����ϡ� -n LIMIT  --bytes=LIMIT  �⡹LIMIT�Х��Ȥ���ӡ� -o FILE  --output=FILE  ����Ū���������Ϥ�FILE�ء� -q  --brief  �ե����뤬�ۤʤ뤫�ɤ�����������ϡ� -r  --recursive  ���Ĥ��ä����̥ǥ��쥯�ȥ꡼��Ƶ�Ū����ӡ� -s  --quiet  --silent  ���Ϥʤ�����λ���ơ������Τߡ� -s  --report-identical-files  ξ�ե����뤬Ʊ���Ȥ������ -s  --suppress-common-lines  ���̹Ԥν��Ϥ��޻ߡ� -t  --expand-tabs  ���ϤΥ��֤�����Ÿ���� -v  --version  �С������������ϡ� -w  --ignore-all-space  �������̵�롣 -w NUM  --width=NUM  ������⡹NUM (�����130) ��ǽ��ϡ� -x  --overlap-only  ��ʣ�����ѹ�����ϡ� -x PAT  --exclude=PAT  PAT�Ȱ��פ���ե����������� -y  --side-by-side  ������ϡ�
  -W NUM  --width=NUM  �⡹NUM (�����130) ������ǽ��ϡ�
  --left-column  ���̹ԤϺ���������ϡ�
  --suppress-common-lines  ���̹Ԥν��Ϥ��޻ߡ� ���̤β��̥ǥ��쥯�ȥ꡼: %s��%s
 2�ĤΥե������Ԥ��Ȥ���Ӥ��ޤ��� 3�ĤΥե������Ԥ��Ȥ���Ӥ��ޤ��� 2�ĤΥե������Х��Ȥ��Ȥ���Ӥ��ޤ��� FILES�ϡ���FILE1 FILE2�ס�DIR1 DIR2�ס�DIR FILE...�ס�FILE... DIR�ס� �ե�����%s��%s���ե�����%s��%s
 �ե�����%s��%s��Ʊ��
 �ե�����%s��%s�ϰ㤤�ޤ�
 --from-file��--to-file����ꤹ��С�FILES�����¤Ϥ���ޤ��� �⤷�ե����뤬�ʤ�����-�פΤȤ���ɸ�����Ϥ��ɤߤޤ��� �ե����뤬��-�פΤȤ���ɸ�����Ϥ��ɤߤޤ��� ̵���ʵջ��� ̵����ʸ�����饹̾�Ǥ� ̵��������ʸ���Ǥ� ̵����\{\}������ ̵�����������ɽ�� ̵�����ϰϽ�λ ̵��������ɽ���Ǥ� ���ۥ����Ȥ��̤����ޤ��� ���פ��ޤ��� �ե����������˲��Ԥ�����ޤ��� ��������ɽ��������ޤ��� %s������ȯ��: %s
 ͽ����������ɽ���ν�λ �礭����������ɽ�� �Х��򸫤Ĥ�����<bug-gnu-utils@gnu.org>����𤷤Ƥ��������� �����åפ��ͤˤϡ����η�����³���뤳�Ȥ�Ǥ��ޤ���
kB 1000��K 1024��MB 1,000,000��M 1,048,576��
GB 1,000,000,000��G 1,073,741,824���ޤ�T��P��E��Z��Y�ˤĤ��Ƥ�Ʊ�͡� �֥����å�1�פȡ֥����å�2�פϡ��ƥե�����򥹥��åפ���Х��ȿ��� �ե����뺹ʬ��2����ʻ�� ���� ��³����ռ��� ��%s --help�פǡ����ܺ٤ʾ����Ĵ�٤ƤߤƤ��������� ̤�ΤΥ����ƥࡦ���顼�Ǥ� �����ʤ�(��\( �����ʤ�)��\) �����ʤ�[��[^ �����ʤ�\{ ����ˡ: %s [���ץ����]... �ե�����1 �ե�����2
 ����ˡ: %s [���ץ����]... �ե�����1 [�ե�����2 [�����å�1 [�����å�2]]]
 ����ˡ: %s [���ץ����]... FILES
 ����ˡ: %s [���ץ����]... MYFILE OLDFILE YOURFILE
 ��-%ld�ץ��ץ����ϡ������٤�Ǥ�����ά���ޤ��礦 ��-%ld�ץ��ץ����ϡ������٤�Ǥ�����-%c %ld�פ�Ȥ��ޤ��礦 ���ϥե�����ˡ�-�פ�ʣ������ꤵ��ޤ��� ����оݤ�ξ���Ȥ�ǥ��쥯�ȥ꡼ ��-�פϥǥ��쥯�ȥ꡼����ӤǤ��ޤ��� �ե�����̾��%s�פȡ�%s�פ���ӤǤ��ޤ��� ɸ����Ϥ�����Ū��ʻ��Ǥ��ޤ��� cmp: �ե�����%s������
 %s���ץ������͡�%s�פ����ͤ��Ƥ��ޤ� ���ϻ��ꥪ�ץ���󤬾��ͤ��Ƥ��ޤ� �������Υ��ץ���󤬾��ͤ��Ƥ��ޤ� ���Υ��ץ���󤬾��ͤ��Ƥ��ޤ� ed:	ξ�����Ǥ˥إå����Ǿ��äơ����Ѥ����Խ���
eb:	ξ¦���Ǥ���Ѥ����Խ���
el:	��¦���Ǥ���Ѥ����Խ���
er:	��¦���Ǥ���Ѥ����Խ���
e:	���Ǥ��Խ���
l:	��¦���Ǥ���ѡ�
r:	��¦���Ǥ���ѡ�
s:	���̹Ԥ���ۤ˴ޤࡣ
v:	���̹Ԥ�����˴ޤࡣ
q:	��λ��
 ;�פʥ��ڥ��ɡ�%s�� ξΩ���ʤ����ץ���� ���ϥե����뤬�̤�� �������顼: process_diff���̵���ʺ�ʬ�� �������顼: ���Ϥ��Ϥ����̵���ʺ�ʬ�� �������顼: ��ʬ�֥��å��η����˥ϥޤ� ̵����--bytes���͡�%s�� ̵����--ignore-initial���͡�%s�� ̵�������������%s�� ̵���ʺ�ʬ����; �ǽ��Ԥ�̤��ü ̵���ʺ�ʬ����; �����ʹ�Ƭʸ���� ̵���ʺ�ʬ����; ̵�����ѹ����ڤ� ̵���ʲ�����%s�� ̵���ʥ�������%s�� ̵��������%s�� ���ۥ����Ȥ��̤����ޤ��� ��%s�פθ�Υ��ڥ��ɤ�����ޤ��� ���ץ����-l��-s��ξΩ���ޤ��� ���Υۥ��ȤǤϥڡ������դ��򥵥ݡ��Ȥ��ޤ��� �ץ�����ࡦ���顼 �ɹ��߼��� �����å��������С��ե��� ɸ����� ���̥ץ�������%s�פ����� ���̥ץ�������%s�פ����Ĥ���ޤ��� �ե����롦��٥롦���ץ����¿�����ޤ� ����߼��� 