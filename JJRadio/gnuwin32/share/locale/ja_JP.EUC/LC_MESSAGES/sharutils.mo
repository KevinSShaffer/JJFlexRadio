��    n      �  �   �      P	  �   Q	  �  �	  O  �  q   7  �  �  "   E  -   h     �     �     �     �     �     �  (        .     ?     Y  ,   s     �  %   �  ,   �  &        8     X     x     �     �     �  	   �     �  &   �     �     �  !   
     ,     @  %   `     �     �     �  &   �     �     �     �     �  )         *     H     X     u     �  I   �  #  �  �     !   �          .     =  2   J     }     �  
   �     �     �     �     �     �       "     )   9  &   c     �     �     �      �  ,   �  ,   !  8   N     �     �     �     �  
   �     �     �     �               (  
   .     9     L     S     s     �  =   �     �     �     �     �       	             %  
   *     5     D     I     \  e  l  �   �   .  g!  S  �$  �   �&  O  n'  /   �*  2   �*     !+     6+     Q+  %   j+     �+     �+  $   �+     �+     �+     ,  2   :,      m,  $   �,  )   �,  0   �,  )   -  '   8-     `-     m-     �-      �-     �-  
   �-     �-     �-     .  $   .      :.  =   [.  G   �.     �.     �.     /  )   !/     K/     R/     _/     h/  2   y/  '   �/     �/  &   �/     0     00  E   K0    �0  �   �2  $   �3  "   �3      �3     �3  <   
4     G4  #   d4     �4     �4     �4     �4     �4     �4     5     (5  ,   G5  (   t5     �5     �5  '   �5  )   �5  4   &6  4   [6  H   �6     �6      �6  
   	7  
   7     7     57     F7     Y7  $   v7     �7     �7     �7     �7     �7  (   �7     �7     8  L   08     }8     �8     �8     �8     �8     �8     �8     �8     �8     �8      9      	9     *9     f   ;      I   +      3   U   &   P                  a   \           F   J      k   Y   ?       8       *      0   d      <              1   /         7              m       '           ]       9             !   #   (          X   S           "      M   D   [       K              h      -   ^         B   i   g       5   V   E      
      Q      j       6   $   	              @   n   ,          G             _   %   N       W   H   b      :   c   )   .       =   Z       L              4   e   l       `           O   2   >       A       T          R       C    
  -h, --help      display this help and exit
  -m, --base64    use base64 encoding as of RFC1521
  -v, --version   output version information and exit
 
Controlling the shar headers:
  -n, --archive-name=NAME   use NAME to document the archive
  -s, --submitter=ADDRESS   override the submitter name
  -a, --net-headers         output Submitted-by: & Archive-name: headers
  -c, --cut-mark            start the shar with a cut line

Selecting how files are stocked:
  -M, --mixed-uuencode         dynamically decide uuencoding (default)
  -T, --text-files             treat all files as text
  -B, --uuencode               treat all files as binary, use uuencode
  -z, --gzip                   gzip and uuencode all files
  -g, --level-for-gzip=LEVEL   pass -LEVEL (default 9) to gzip
  -Z, --compress               compress and uuencode all files
  -b, --bits-per-code=BITS     pass -bBITS (default 12) to compress
 
Giving feedback:
      --help              display this help and exit
      --version           output version information and exit
  -q, --quiet, --silent   do not output verbose messages locally

Selecting files:
  -p, --intermix-type     allow -[BTzZ] in file lists to change mode
  -S, --stdin-file-list   read file list from standard input

Splitting output:
  -o, --output-prefix=PREFIX    output to file PREFIX.01 through PREFIX.NN
  -l, --whole-size-limit=SIZE   split archive, not files, to SIZE kilobytes
  -L, --split-size-limit=SIZE   split archive, or files, to SIZE kilobytes
 
Option -o is required with -l or -L, option -n is required with -a.
Option -g implies -z, option -b implies -Z.
 
Protecting against transmission:
  -w, --no-character-count      do not use `wc -c' to check size
  -D, --no-md5-digest           do not use `md5sum' digest to verify
  -F, --force-prefix            force the prefix character on every line
  -d, --here-delimiter=STRING   use STRING to delimit the files in the shar

Producing different kinds of shars:
  -V, --vanilla-operation   produce very simple and undemanding shars
  -P, --no-piping           exclusively use temporary files at unshar time
  -x, --no-check-existing   blindly overwrite existing files
  -X, --query-user          ask user before overwriting files (not for Net)
  -m, --no-timestamp        do not restore file modification dates & times
  -Q, --quiet-unshar        avoid verbose messages at unshar time
  -f, --basename            restore in one directory, despite hierarchy
      --no-i18n             do not produce internationalized shell script
 %s is probably not a shell archive %s looks like raw C code, not a shell archive %s: Illegal ~user %s: No `begin' line %s: No `end' line %s: No user `%s' %s: Not a regular file %s: Short file %s: data following `=' padding character %s: illegal line %s: illegal option -- %c
 %s: invalid option -- %c
 %s: option `%c%s' doesn't allow an argument
 %s: option `%s' is ambiguous
 %s: option `%s' requires an argument
 %s: option `--%s' doesn't allow an argument
 %s: option requires an argument -- %c
 %s: unrecognized option `%c%s'
 %s: unrecognized option `--%s'
 (binary) (compressed) (empty) (file already exists) (gzipped) (text) -C is being deprecated, use -Z instead Cannot access %s Cannot chdir to `%s' Cannot get current directory name Cannot open file %s Cannot use -a option without -n Cannot use -l or -L option without -o Closing `%s' Could not fork Created %d files
 DEBUG was not selected at compile time End of End of part File File %s (%s) Found no shell commands after `cut' in %s Found no shell commands in %s Hard limit %dk
 In shar: remaining size %ld
 Limit still %d
 MD5 check failed Mandatory arguments to long options are mandatory for short options too.
 Mandatory arguments to long options are mandatory for short options too.

  -d, --directory=DIRECTORY   change to DIRECTORY before unpacking
  -c, --overwrite             pass -c to shar script for overwriting files
  -e, --exit-0                same as `--split-at="exit 0"'
  -E, --split-at=STRING       split concatenated shars after STRING
  -f, --force                 same as `-c'
      --help                  display this help and exit
      --version               output version information and exit

If no FILE, standard input is read.
 Mandatory arguments to long options are mandatory to short options too.
  -h, --help               display this help and exit
  -v, --version            output version information and exit
  -o, --output-file=FILE   direct output to FILE
 Must unpack archives in sequence! Newfile, remaining %ld,  No input files Opening `%s' PLEASE avoid -X shars on Usenet or public networks Please unpack part Please unpack part 1 first! Read error SKIPPING STILL SKIPPING Saving %s (%s) Soft limit %dk
 Starting `sh' process Starting file %s
 The `cut' line was followed by: %s Too many directories for mkdir generation Try `%s --help' for more information.
 Unknown system error Usage: %s [FILE]...
 Usage: %s [INFILE] REMOTEFILE
 Usage: %s [OPTION]... [FILE]...
 WARNING: No user interaction in vanilla mode WARNING: Non-text storage options overridden WARNING: not restoring timestamps.  Consider getting and Write error You have unpacked the last part archive binary compressed continue with part continuing file creating directory creating lock directory current size empty extracting extraction aborted failed failed to create lock directory gunzipping file gzipped installing GNU \`touch', distributed in GNU File Utilities... is complete is continued in part memory exhausted next! original size overwrite overwriting part restore of standard input text uncompressing file uudecoding file Date: 1995-11-14 13:00:00+0900
From: SAKAI Kiyotaka <ksakai@netwk.ntt-at.co.jp>
Xgettext-Options: --default-domain=sharutils --output-dir=. --add-comments --keyword=_
Files: ../../po/../lib/error.c ../../po/../lib/getopt.c
	 ../../po/../lib/xmalloc.c ../../po/../src/shar.c
	 ../../po/../src/unshar.c ../../po/../src/uudecode.c
	 ../../po/../src/uuencode.c
 
  -h, --help      ���Υإ�פ�ɽ�����ƽ�λ
  -m, --base64    RFC1521 ���������Ƥ��� base64 �����
  -v, --version   �С����������ɽ�����ƽ�λ
 
shar �Υإå��򥳥�ȥ����뤹�륪�ץ����Ǥ�:
  -n, --archive-name=NAME   ����������̾�Ȥ��� NAME �����
  -s, --submitter=ADRESSE   ������̾����
  -a, --net-headers         Submitted-by: �� Archive-name: �����
  -c, --cut-mark            cut line �� shar ��Ϥ��

�ɤΤ褦�˳�Ǽ���뤫������:
  -M, --mixed-uuencode          uuencode ��ưŪ�˷���(�ǥե����)
  -T, --text-files              ���٤ƤΥե������ƥ����ȤȤ��ư���
  -B, --uuencode                ���٤ƤΥե������Х��ʥ꡼�Ȥ��ư�����uuencode �����
  -z, --gzip                    ���٤ƤΥե������ gzip ���� uuencode
  -g, --level-for-gzip=LEVEL    gzip �Υ�٥����� (�ǥե���ȤǤ� 9)
  -Z, --compress                ���٤ƤΥե������ compress ���� uuencode
  -b, --bits-per-code=BITS      compress ���Ϥ� -bBITS (�ǥե���ȤǤ� 12)
 
�����ɽ��:
      --help              ���Υإ�פ�ɽ�����ƽ�λ
      --version           �С����������ɽ�����ƽ�λ
  -q, --quiet, --silent   ��ǧ�Τ���Υ�å�������ɽ�����ޤ���

�ե����������:
  -p, --intermix-type     �⡼�ɤ��ѹ����뤿��˥ե�����ꥹ�Ȥ� -[BTzZ] �����
  -S, --stdin-file-list   �ե����롦�ꥹ�Ȥ�ɸ�����Ϥ����ɤ߹���

���Ϥ�ʬ��:
  -o, --output-prefix=PREFIXE       PREFIX.01 ���� PREFIX.NN �˽���
  -l, --whole-size-limit=GRANDEUR   �ե�����ʳ��Υ��������֤� SIZE �����Х��Ȥ�ʬ��
  -L, --split-size-limit=GRANDEUR   ���������֤ޤ��ϥե������ SIZE �����Х��Ȥ�ʬ��
 
���ץ���� -o �� -l �� -L ��ɬ�פǡ����ץ���� -n �� -a ��ɬ�פǤ���
���ץ���� -g �� -z ��ޤߡ����ץ���� -b �� -Z ��ޤߤޤ���
 
�Ѵ����Ф����ݸ�:
  -w, --no-character-count      �������Υ����å��� `wc -c' ����Ѥ��ʤ�
  -D, --no-md5-digest           �����������ȤΥ����å��� `md5sum' ����Ѥ��ʤ�
  -F, --force-prefix            ����ʸ����ƹԤ������֤�
  -d, --here-delimiter=STRING   shar �ǥե������ʬ�䤹�뼱�̻Ҥ� STRING �����

�����फ�� shar ������:
  -V, --vanilla-operation   �ʷ�� shar ������
  -P, --no-piping           unshar ������¾Ū�ʰ���ե���������
  -x, --no-check-existing   �ե����뤬¸�ߤ��Ƥ�ɬ�����
  -X, --query-user          �ե�����ξ�񤭻��˥桼�����䤤��碌�� (not for Net)
  -m, --no-timestamp        �ե�����ι���������ᤵ�ʤ�
  -Q, --quiet-unshar        unshar ���˳�ǧ��å�������ɽ�����ʤ�
  -f, --basename            ���ز������ˡ�1�ĤΥǥ��쥯�ȥ���᤹
      --no-i18n             ��ݲ����줿�����롦������ץȤ��������ʤ�
 %s �ϡ������餯�����롦���������֤ǤϤ���ޤ��� %s �� C �Υ����ɤǡ�����륢�������֤ǤϤ���ޤ��� %s: ~user �������Ǥ� %s: `begin' �Ԥ�����ޤ��� %s: `end' �Ԥ�����ޤ��� %s: `%s' �Ȥ����桼������¸�ߤ��ޤ��� %s: �����ե�����ǤϤ���ޤ��� %s: �ե����뤬û���Ǥ� %s: `=' ��³���ǡ�����ʸ���ͤᤷ�ޤ� %s: �����ʹԤǤ� %s: %c �������ʥ��ץ����Ǥ�
 %s: %c �������ʥ��ץ����Ǥ�
 %s: `%c%s' ���ץ����ϰ������뤳�Ȥ��Ǥ��ޤ���
 %s: `%s' ���ץ����������ƤǤ�
 %s: `%s' ���ץ����ϰ�����ɬ�פǤ�
 %s: ���ץ���� `--%s' �ϰ�������ޤ���
 %s: %c ���ץ����ϰ�������ꤹ��ɬ�פ�����ޤ�
 %s: ���ץ���� ��`%c%s' ��ǧ���Ǥ��ޤ���
 %s: ���ץ���� `--%s' ��ǧ���Ǥ��ޤ���
 (�Х��ʥ꡼) (compress ����Ƥ��ޤ�) (��) (�ե�����Ϥ��Ǥ�¸�ߤ��Ƥ��ޤ�) (gzip ����Ƥ��ޤ�) (�ƥ�����) -C ��ȿ�Фǡ�-Z ��ȤäƲ����� %s �˥��������Ǥ��ޤ��� %s �˰�ư�Ǥ��ޤ��� ���ߤΥǥ��쥯�ȥ�̾������Ǥ��ޤ��� �ե����� %s �򥪡��ץ�Ǥ��ޤ��� -n ���ץ�������ꤻ���� -a ���ץ�����Ȥ����ȤϤǤ��ޤ��� -o ���ץ�������ꤻ���� -l �ޤ��� -L ���ץ�����Ȥ����ȤϤǤ��ޤ��� %s ���Ĥ��ޤ� �ե������Ǥ��ޤ��� %d �ĤΥե������������ޤ���
 ����ѥ������ DEBUG �����ꤵ��Ƥ��ޤ��� �Ǹ�� �Ǹ�Υѡ��� �ե����� �ե����� %s (%s) %s �� `cut' �θ�˥����륳�ޥ�ɤ����Ĥ���ޤ��� %s �ˤϥ���롦���ޥ�ɤ����դ���ޤ��� �ϡ��ɡ���ߥåȤ� %dk �Ǥ�
 shar: ����ǥ����� %ld ���ĤäƤ��ޤ�
 �ޤ���ߥåȤ� %d �Ǥ�
 MD5 �����å������Ԥ��ޤ��� ���󥰡����ץ�����ɬ�פʰ����ϡ����硼�ȡ����ץ����Ǥ�ɬ�פǤ���
 ���󥰡����ץ�����ɬ�פʰ����ϡ����硼�ȡ����ץ����Ǥ�ɬ�פǤ�
  -d, --directory=DIRECTORY   ����ѥå������� DIRECTORY �˰�ư����
  -c, --overwrite             �ե�������񤭤���褦�� -c �� shar ���Ϥ�
  -e, --exit-0                `--split-at="exit 0"' ��Ʊ��
  -E, --split-at=STRING       STRING �θ���ǷҤ��ä� shar ��ʬ��
  -f, --force                 `-c' ��Ʊ��
      --help                  ���Υإ�פ�ɽ�����ƽ�λ
      --version               �С����������ɽ�����ƽ�λ

�ե��������ꤷ�ʤ���С�ɸ�����Ϥ��Ȥ��ޤ�
 ���󥰡����ץ�����ɬ�פʰ����ϡ����硼�ȡ����ץ����Ǥ�ɬ�פǤ�
  -h, --help               ���Υإ�פ�ɽ�����ƽ�λ
  -v, --version            �С����������ɽ�����ƽ�λ
  -o, --output-file=FILE   ľ�� FILE �˽���
 ���֤˥���ѥå�����ɬ�פ�����ޤ��� �����ե�����ǡ�%ld ���ĤäƤ��ơ� ���ϥե����뤬���ꤵ��Ƥ��ޤ��� %s �򥪡��ץ󤷤Ƥ��ޤ� Usenet ������Υͥåȥ���Ǥ� -X shar ����Ѥ��ʤ��ǲ����� �ѡ��Ȥ򥢥�ѥå����Ʋ����� �ѡ���1��ǽ�˥���ѥå����Ʋ����� Erreur en lecture ���Ф��Ƥ��ޤ� �ޤ����Ф��Ƥ��ޤ� %s (%s) �򥻡��֤��Ƥ��ޤ� ���եȥ�ߥåȤ� %dk �Ǥ�
 `sh' �Υץ������򳫻Ϥ��ޤ� �ե����� %s ����Ϥ�ޤ�
 %s ��³���� `cut' �Ԥ�����ޤ� mkdir ����������ǥ��쥯�ȥ�ο���¿�����ޤ� �ܤ����� `%s --help' ��¹Ԥ��Ʋ�������
 ��̣�����Υ����ƥ२�顼�Ǥ� ����ˡ: %s [�ե�����]...
 ����ˡ: %s [���ϥե�����] ���ϥե�����
 ����ˡ: %s [���ץ����]... [�ե�����]...
 �ٹ�: �桼������ߺ��Ѥ����Υ⡼�ɤΤ�ΤϤ���ޤ��� �ٹ�: �ƥ����ȤǤʤ���Ǽ���ץ���󤬾�񤭤���ޤ��� �ٹ�: �����ࡦ������פ��᤹���Ȥ��Ǥ��ޤ������ꤹ�뤳�Ȥ�ͤ��Ʋ����� �񤭹��ߥ��顼 �Ǹ�Υѡ��Ȥ򥢥�ѥå����ޤ��� ���������� �Х��ʥ꡼ compress ����Ƥ��ޤ� �ѡ��Ȥ�³���ޤ� �ե������³���ޤ� �ǥ��쥯�ȥ��������Ƥ��ޤ� ���å����ǥ��쥯�ȥ��������Ƥ��ޤ� ���ߤΥ����� ���� ����� ��Ф����Ǥ��ޤ� ���Ԥ��ޤ��� ���å����ǥ��쥯�ȥ�κ����˼��Ԥ��ޤ��� �ե������ gunzip ���Ƥ��ޤ� gzip ���줿�ե�����Ǥ� GNU �� File Utilities �����ۤ���Ƥ��� GNU \`touch' �򥤥󥹥ȡ��뤷�Ƥ��ޤ� ��λ���ޤ��� ���Υѡ��Ȥ�³���ޤ� ���꡼��Ȥ��̤����ޤ��� ��! ���ꥸ�ʥ롦������ ��� ��񤭤��Ƥ��ޤ� �ѡ��� �β��� ɸ������ �ƥ����� �ե������ uncompress ���Ƥ��ޤ� �ե������ uudecode ���Ƥ��ޤ� 