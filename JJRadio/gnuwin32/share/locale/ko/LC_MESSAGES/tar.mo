��       �     �   �  �      �   ?  �  -  �  �  '  W  �       -  	  �  7   $       >   !  V   7  x   +  �   �  �     �     �   �  �     H   "  a     �   2  �     �     �   $       3     N     f     ~     �     �     �   )  �   #      "   *      M      j      �      �      �      �      �      �   2  !     !E   $  !\   "  !�     !�     !�   "  !�   5  !�     "      "8     "K     "h     "�     "�     "�     "�     "�     "�   <  #     #I     #U     #b     #q     #�     #�     #�     #�     #�   "  #�   $  $     $:     $N     $_     $v     $�   !  $�   (  $�   +  $�   !  %   #  %6   "  %Z     %}   #  %�     %�     %�     %�   "  %�     &   0  &>     &o   #  &�   "  &�   (  &�     &�     '      '      'A     'X     'v     '�     '�   !  '�     '�     '�   ,  '�     (+     (?     (W     (p   ,  (�     (�   )  (�  Y  (�     *=     *V     *t     *�     *�     *�     *�     *�     +
     +   +  +3     +_     +u     +�     +�     +�     +�   1  +�   )  ,   /  ,>   2  ,n   4  ,�   (  ,�   1  ,�   %  -1     -W   0  -c   +  -�   1  -�     -�     .     .     .,   %  .8   -  .^   (  .�   <  .�     .�     /     /     /-     /E     /U   %  /h     /�     /�     /�   &  /�     0     0     0*     0I     0^   -  0s     0�     0�     0�     0�     0�   !  1     19     1S   3  1g   .  1�     1�   #  1�     2     2(     2=     2C     2J     2V  %  2g   <  3�  Z  3�  �  5%  |  8    9�  .  ;�  �  =�   .  @�     A      A   ;  A:   -  Av   �  A�     B\     Bk   �  Bw     C   %  C7   "  C]   ,  C�     C�     C�   +  C�     D     D3     DO     Dk     D�     D�     D�   ,  D�   '  D�   "  E   &  E8     E_     E     E�     E�     E�     E�     F   -  F&     FT   0  Fe   +  F�     F�     F�   "  F�   )  G      GA     Gb     G      G�      G�     G�     G�     H     H1     HL   D  Hi     H�     H�     H�     H�     I     I#     I<     IT     Is      I�   *  I�     I�     I�   !  J     J(      J@   $  Ja   4  J�   .  J�   $  J�   '  K   )  K7     Ka     Ku     K�     K�   "  K�     K�     L   3  L     LS   "  Lr   &  L�   .  L�     L�     M   !  M     M3   !  MH     Mj     M�     M�   &  M�     M�     M�   '  N     N-     NF     Nc     N�   .  N�     N�   +  N�  7  N�     P1     PI     Pg     P�     P�     P�     P�     P�     P�     Q   +  Q,   #  QX     Q|     Q�     Q�     Q�     Q�   1  Q�   +  R#   /  RO   /  R   4  R�   (  R�   -  S   %  S;   	  Sa   0  Sk   ,  S�   1  S�     S�     T
     T     T.   (  T<   3  Te   &  T�   9  T�     T�     U     U     U7     UK     U_   '  Uz     U�     U�     U�   ,  U�     V     V-     VH     Vh     V}   (  V�     V�     V�     V�   $  V�     W   $  W1     WV     Wk   1  W|   +  W�     W�   %  W�     X     X4     XF     XO   
  XX   
  Xc      �       �   2              p       �   '   G   |       T           �   :   M      �   ^       <   �   5       �           O      #         n   .          B       �   %   9   k      �   >   �   i   4      �   +   �   �   �       �   \   �   �   3   )   F   0          D       �   �   o   t   �   �   �   E   ?   �       �   L   �   N      h   �   W              �   g           �   �   =           &   l   ]       a   �   �   
   m   P   �   s   8       e   H       C      �   �   "       ;       {       -   R   �       u       @       �   x                                �   �   �      �   !   �             �      1   b   /   6           Y   c   �   K           �   j   �           �   A       	   }   �      _   �       v   �       �       7       r             w   [   �   �           �   Z       �   ~   �   y   �   �       *   X              �   I   U                                           ,   �   S   $   V   q   z      �   d              `   Q   (   J   f 
Copyright (C) 1995, 1996, 1997 Free Software Foundation, Inc.
 
Device blocking:
  -b, --blocking-factor=BLOCKS   BLOCKS x 512 bytes per record
      --record-size=SIZE         SIZE bytes per record, multiple of 512
  -i, --ignore-zeros             ignore zeroed blocks in archive (means EOF)
  -B, --read-full-records        reblock as we read (for 4.2BSD pipes)
 
Device selection and switching:
  -f, --file=ARCHIVE             use archive file or device ARCHIVE
      --force-local              archive file is local even if has a colon
      --rsh-command=COMMAND      use remote COMMAND instead of rsh
  -[0-7][lmh]                    specify drive and density
  -M, --multi-volume             create/list/extract multi-volume archive
  -L, --tape-length=NUM          change tape after writing NUM x 1024 bytes
  -F, --info-script=FILE         run script at end of each tape (implies -M)
      --new-volume-script=FILE   same as -F FILE
      --volno-file=FILE          use/update the volume number in FILE
 
GNU tar cannot read nor produce `--posix' archives.  If POSIXLY_CORRECT
is set in the environment, GNU extensions are disallowed with `--posix'.
Support for POSIX is only partially implemented, don't count on it yet.
ARCHIVE may be FILE, HOST:FILE or USER@HOST:FILE; and FILE may be a file
or a device.  *This* `tar' defaults to `-f%s -b%d'.
 
Informative output:
      --help            print this help, then exit
      --version         print tar program version number, then exit
  -v, --verbose         verbosely list files processed
      --checkpoint      print directory names while reading the archive
      --totals          print total bytes written while creating archive
  -R, --block-number    show block number within archive with each message
  -w, --interactive     ask for confirmation for every action
      --confirmation    same as -w
 
Main operation mode:
  -t, --list              list the contents of an archive
  -x, --extract, --get    extract files from an archive
  -c, --create            create a new archive
  -d, --diff, --compare   find differences between archive and file system
  -r, --append            append files to the end of an archive
  -u, --update            only append files newer than copy in archive
  -A, --catenate          append tar files to an archive
      --concatenate       same as -A
      --delete            delete from the archive (not on mag tapes!)
 
Operation modifiers:
  -W, --verify               attempt to verify the archive after writing it
      --remove-files         remove files after adding them to the archive
  -k, --keep-old-files       don't overwrite existing files when extracting
  -U, --unlink-first         remove each file prior to extracting over it
      --recursive-unlink     empty hierarchies prior to extracting directory
  -S, --sparse               handle sparse files efficiently
  -O, --to-stdout            extract files to standard output
  -G, --incremental          handle old GNU-format incremental backup
  -g, --listed-incremental   handle new GNU-format incremental backup
      --ignore-failed-read   do not exit with nonzero on unreadable files
 
Report bugs to <tar-bugs@gnu.org>.
 
Usage: %s [OPTION]...
 
Usage: %s [OPTION]... [FILE]...
 
Written by Fran�ois Pinard <pinard@iro.umontreal.ca>.
 
Written by John Gilmore and Jay Fenlason.
   -N, --newer=DATE             only store files newer than DATE
      --newer-mtime            compare date and time when data changed only
      --after-date=DATE        same as -N
  (core dumped)  link to %s
  n [name]   Give a new file name for the next (and subsequent) volume(s)
 q          Abort tar
 !          Spawn a subshell
 ?          Print this list
  unknown file type `%c'
 %s is not continued on this volume %s is the archive; not dumped %s: Could not change access and modification times %s: Could not create directory %s: Could not create file %s: Could not create symlink to `%s' %s: Could not link to `%s' %s: Could not make fifo %s: Could not make node %s: Could not write to file %s: Deleting %s
 %s: Error while closing %s: Not found in archive %s: On a different filesystem; not dumped %s: Unknown file type; file ignored %s: Was unable to backup this file %s: is unchanged; not dumped ((child)) Pipe to stdin ((child)) Pipe to stdout (child) Pipe to stdin (child) Pipe to stdout (grandchild) Pipe to stdin (grandchild) Pipe to stdout --Volume Header--
 Added write and execute permission to directory %s Ambiguous pattern `%s' Archive %s EOF not on block boundary Archive not labelled to match `%s' Archive to stdin Archive to stdout At beginning of tape, quitting now Attempting extraction of symbolic links as hard links Cannot add directory %s Cannot add file %s Cannot allocate buffer space Cannot change to directory %s Cannot chdir to %s Cannot close descriptor %d Cannot close file #%d Cannot exec %s Cannot exec a shell %s Cannot execute remote shell Cannot extract `%s' -- file is continued from another volume Cannot fork Cannot fork! Cannot open %s Cannot open archive %s Cannot open directory %s Cannot open file %s Cannot open pipe Cannot properly duplicate %s Cannot read %s Cannot read confirmation from user Cannot read from compression program Cannot read link %s Cannot remove %s Cannot rename %s to %s Cannot stat %s Cannot symlink %s to %s Cannot update compressed archives Cannot use compressed or remote archives Cannot use multi-volume compressed archives Cannot verify compressed archives Cannot verify multi-volume archives Cannot verify stdin/stdout archive Cannot write to %s Cannot write to compression program Child cannot fork Child died with signal %d%s Child returned status %d Conflicting archive format options Conflicting compression options Could not allocate memory for blocking factor %d Could not get current directory Could not get current directory: %s Could not re-position archive file Could not rewind archive file for verify Creating directory: Data differs Deleting non-header from archive Device numbers changed Directory %s has been renamed Directory %s is new Does not exist EOF in archive file EOF where user reply was expected Error while closing %s Error while deleting %s Extracting contiguous files as regular files File does not exist File name %s%s too long File name %s/%s too long Garbage command Generate data files for GNU tar test suite.
 Gid differs Hmm, this doesn't look like a tar archive If a long option shows an argument as mandatory, then it is mandatory
for the equivalent short option also.

  -l, --file-length=LENGTH   LENGTH of generated file
  -p, --pattern=PATTERN      PATTERN is `default' or `zeros'
      --help                 display this help and exit
      --version              output version information and exit
 Invalid date format `%s' Invalid group given on option Invalid mode given on option Invalid owner given on option Invalid value for record_size Memory exhausted Missing file name after -C Mod time differs Mode differs Mode or device-type changed Multiple archive files requires `-M' option No archive name given No longer a directory No new volume; exiting.
 No such file or directory Not a regular file Not linked to %s Obsolete option name replaced by --absolute-names Obsolete option name replaced by --backup Obsolete option name replaced by --block-number Obsolete option name replaced by --blocking-factor Obsolete option name replaced by --read-full-records Obsolete option name replaced by --touch Obsolete option, now implied by --blocking-factor Old option `%c' requires an argument. Omitting %s Options `-%s' and `-%s' both want standard input Options `-Aru' are incompatible with `-f -' Options `-[0-7][lmh]' not supported by *this* tar Premature end of file Read checkpoint %d Read error on %s Reading %s
 Record size must be a multiple of %d. Removing drive spec from names in the archive Removing leading `/' from absolute links Removing leading `/' from absolute path names in the archive Renamed %s to %s Size differs Skipping to next file header Skipping to next header Symlink differs Symlinked %s to %s This does not look like a tar archive This volume is out of sequence Too many errors, quitting Total bytes written:  Try `%s --help' for more information.
 Uid differs Unexpected EOF in archive Unexpected EOF on archive file Unknown pattern `%s' Unknown system error VERIFY FAILURE: %d invalid header(s) detected Verify  Visible long name error Visible longname error Volume `%s' does not match `%s' WARNING: Archive is incomplete WARNING: Cannot close %s (%d, %d) WARNING: No volume header Write checkpoint %d You may not specify more than one `-Acdtrux' option You must specify one of the `-Acdtrux' options exec/tcp: Service not available rmtd: Cannot allocate buffer space
 rmtd: Garbage command %c
 rmtd: Premature eof
 stdin stdout tar (child) tar (grandchild) Project-Id-Version: GNU tar 1.12
POT-Creation-Date: 1999-07-04 23:46-0700
PO-Revision-Date: 1997-05-30 22:55+0900
Last-Translator: Bang Jun-Young <bangjy@nownuri.net>
Language-Team: Korean <ko@li.org>
MIME-Version: 1.0
Content-Type: text/plain; charset=EUC-KR
Content-Transfer-Encoding: 8-bit
 
���۱� (C) 1995, 1996, 1997 Free Software Foundation, Inc.
 
��ġ ���� ����:
  -b, --blocking-factor=BLOCK    ���ڵ�� BLOCK x 512 ����Ʈ
      --record-size=SIZE         ���ڵ�� SIZE ����Ʈ, 512�� ���
  -i, --ignore-zeros             ��ī�̺꿡�� ������ �� ������ �����մϴ�
                                 (EOF�� �ǹ���)
  -B, --read-full-records        ���� ���� �����ȭ�մϴ� (4.2BSD ������������)
 
��ġ ���ð� ��ȯ:
  -f, --file=ARCHIVE             ��ī�̺� ���� �Ǵ� ARCHIVE ��ġ�� ����մϴ�
      --force-local              �̸��� �ݷ��� �ִ� ��ī�̺� ���ϵ� ���� ���Ϸ�
                                 �ν��մϴ�
      --rsh-command=COMMAND      rsh ��� ���� COMMAND�� ����մϴ�
  -[0-7][lmh]                    ����̺�� ��� �е��� �����մϴ�
  -M, --multi-volume             ���� ���� ��ī�̺긦 ����/���/�����մϴ�
  -L, --tape-length=NUM          NUM x 1024 ����Ʈ�� �� �ڿ� �������� �ٲߴϴ�
  -F, --info-script=FILE         �� �������� ������ ��ũ��Ʈ�� �����մϴ�
                                 (-M�� ������)
      --new-volume-script=FILE   -F FILE�� ����
      --volno-file=FILE          FILE �ȿ� �ִ� ���� ��ȣ�� ���/�����մϴ�
 
GNU tar�� `--posix' ��ī�̺긦 �аų� ����� �� �� �����ϴ�.  ����
POSIXLY_CORRECT�� ȯ�濡�� �����Ǿ� �ִٸ�, GNU Ȯ���� `--posix'�� ����
��Ȱ��ȭ�˴ϴ�.  POSIX ������ �ܿ� �κ������θ� �����Ǿ����Ƿ� ���� �ŷ�
������ ���ʽÿ�.  ARCHIVE�� FILE, HOST:FILE, �Ǵ� USER@HOST:FILE�� �� ��
������, ���⼭ FILE�� �����̳� ��ġ�� �� �� �ֽ��ϴ�.  �� `tar'��
�������� `-f%s -b%d'�Դϴ�.
 
���� ��¿� ���� �ɼ�:
      --help            �� ������ �μ��ϰ� �����ϴ�
      --version         tar ���α׷��� ���� ��ȣ�� �μ��ϰ� �����ϴ�
  -v, --verbose         ó���Ǵ� ������ ������� ����մϴ�
      --checkpoint      ��ī�̺긦 ���� ���� ���丮 �̸��� �μ��մϴ�
      --totals          ��ī�̺긦 ���� ���� ������ �� ����Ʈ ���� �μ��մϴ�
  -R, --block-number    �� �޽������� ��ī�̺곻�� ���� ��ȣ�� ǥ���մϴ�
  -w, --interactive     ��� �ൿ�� ���� Ȯ���� �䱸�մϴ�
      --confirmation    -w�� ����
 
�ֿ� ���� ���:
  -t, --list              ��ī�̺��� ���빰�� ����մϴ�
  -x, --extract, --get    ��ī�̺꿡�� ������ �����մϴ�
  -c, --create            ���ο� ��ī�̺긦 ����ϴ�
  -d, --diff, --compare   ��ī�̺�� ���� �ý��۰��� �������� ���մϴ�
  -r, --append            ��ī�̺� ���� ������ �߰��մϴ�
  -u, --update            ��ī�̺� ���� �ͺ��� ���ο� ���ϸ� �߰��մϴ�
  -A, --catenate          ��ī�̺꿡 tar ������ �߰��մϴ�
      --concatenate       -A�� ����
      --delete            ��ī�̺�κ��� �����մϴ� (�ڱ� ���������� �ȵ�!)
 
���� ������:
  -W, --verify               ��ī�̺긦 ����� ���� �����ϵ��� �մϴ�
      --remove-files         ��ī�̺꿡 ������ �߰��� ���� ����ϴ�
  -k, --keep-old-files       ������ �� �̹� �����ϴ� ������ ����� �ʽ��ϴ�
  -U, --unlink-first         �����ϱ⿡ �ռ� ��� ������ ����ϴ�
      --recursive-unlink     ���丮�� �����ϱ⿡ �ռ� �� ü�踦 ���ϴ�
  -S, --sparse               ���Ľ� ������ ȿ�������� ó���մϴ�
  -O, --to-stdout            ǥ�� ������� ������ �����մϴ�
  -G, --incremental          ������ GNU ������ ������ ��� ������ ó���մϴ�
  -g, --listed-incremental   ���ο� GNU ������ ������ ��� ������ ó���մϴ�
      --ignore-failed-read   ���� �� ���� ���Ͽ� ���� �� �ƴ� ������ ��������
                             �ʽ��ϴ�
 
<tar-bugs@gnu.org>�� ���׸� ������ �ֽʽÿ�.
 
����: %s [�ɼ�]...

 
����: %s [�ɼ�]... [����]...
 
Fran�ois Pinard <pinard@iro.umontreal.ca>�� ��������ϴ�.
 
John Gilmore�� Jay Fenlason�� ��������ϴ�.
   -N, --newer=DATE             DATE ������ ���ϵ鸸 �����մϴ�
      --newer-mtime            �����Ͱ� �ٲ���� ���� ��¥�� �ð��� ���մϴ�
      --after-date=DATE        -N�� ����
  (�ھ� ��µ�)  %s�� ��ũ
  n [�̸�]   ����(�� �� ������) ������ ���� �� ���� �̸��� �����մϴ�
 q          tar�� �ߴ��մϴ�
 !          ������� �����մϴ�
 ?          �� ����� �μ��մϴ�
  �� �� ���� ���� Ÿ�� `%c'
 %s�� �� ������ ���ӵǾ� ���� �ʽ��ϴ� %s�� ��ī�̺��Դϴ�; �������� ���� %s: ���� �ð��� ���� �ð��� �ٲ� �� �����ϴ� %s: ���丮�� ���� �� �����ϴ� %s: ������ ���� �� �����ϴ� %s: `%s'�� ���� ��ȣ��ũ�� ���� �� �����ϴ� %s: `%s'�� ��ũ�� �� �����ϴ� %s: fifo�� ���� �� �����ϴ� %s: ��带 ���� �� �����ϴ� %s: ���Ͽ� �� �� �����ϴ� %s: %s�� ����
 %s: �ݴ� ���� ���� �߻� %s: ��ī�̺꿡 ���� %s: �ٸ� ���Ͻý��� �� ����; �������� ���� %s: �� �� ���� ���� Ÿ��; ������ ���õ� %s: �� ������ ����� �� �������ϴ� %s: ������� �ʾҽ��ϴ�; �������� ���� ((�ڽ�)) ǥ���Է¿� ���� ������ ((�ڽ�)) ǥ����¿� ���� ������ (�ڽ�) ǥ���Է¿� ���� ������ (�ڽ�) ǥ����¿� ���� ������ (����) ǥ���Է¿� ���� ������ (����) ǥ����¿� ���� ������ --���� ���--
 %s ���丮�� ����� ���� ������ �ΰ��߽��ϴ� ��ȣ�� ���� `%s' ���� ��谡 �ƴ� �κп��� ��ī�̺� %s�� EOF ���� `%s'�� ��ġ�ϵ��� ���� ���� ���� ��ī�̺� ǥ���Է¿� ���� ��ī�̺� ǥ����¿� ���� ��ī�̺� �������� ���� �κп��� ���� ������ ��ȣ ��ũ�� �ϵ� ��ũ�� �����ϰ� �ֽ��ϴ� %s ���丮�� �߰��� �� �����ϴ� ���� %s�� �߰��� �� �����ϴ� ���� ������ �Ҵ��� �� �����ϴ� %s�� ���丮�� �ٲ� �� �����ϴ� %s�� ���丮�� �ٲ� �� �����ϴ� ����� %d�� ���� �� �����ϴ� ���� #%d�� ���� �� �����ϴ� %s�� ������ �� �����ϴ� %s ���� ������ �� �����ϴ� ���� ���� ������ �� �����ϴ� `%s'�� ������ �� �����ϴ� -- �� ������ �ٸ� ������ ���ӵǾ� �ֽ��ϴ� fork�� �� �����ϴ� fork�� �� �����ϴ�! %s�� �� �� �����ϴ� %s ��ī�̺긦 �� �� �����ϴ� %s ���丮�� �� �� �����ϴ� %s ������ �� �� �����ϴ� �������� �� �� �����ϴ� %s�� ����� ������ �� �����ϴ� %s�� ���� �� �����ϴ� ������� Ȯ���� ���� �� �����ϴ� ���� ���α׷����κ��� �о���� �� �����ϴ� ��ũ %s�� ���� �� �����ϴ� %s�� ���� �� �����ϴ� %s�� %s�� �̸��� �ٲ� �� �����ϴ� %s�� stat�� �� �����ϴ� %s�� %s�� ��ȣ��ũ�� �� �����ϴ� ����� ��ī�̺긦 ������ �� �����ϴ� ����� ��ī�̺곪 ���� ��ī�̺긦 ����� �� �����ϴ� ����� ����-���� ��ī�̺긦 ����� �� �����ϴ� ����� ��ī�̺긦 ������ �� �����ϴ� ����-���� ��ī�̺긦 ������ �� �����ϴ� ǥ����/��� ��ī�̺긦 ������ �� �����ϴ� %s�� �� �� �����ϴ� ���� ���α׷��� �� �� �����ϴ� �ڽ��� fork�� �� �����ϴ� �ڽ��� ��ȣ %d%s�� �Բ� �׾��� �ڽ��� ���� %d�� �ǵ��� �־����ϴ� �򰥸��� ��ī�̺� ���� �ɼ� �򰥸��� ���� �ɼ� ���� ��� %d�� �����ϴ� �޸𸮸� �Ҵ��� �� �����ϴ� ���� ���丮�� �� �� �����ϴ� ���� ���丮�� �� �� �����ϴ�: %s ��ī�̺� ������ ����ġ��ų �� �����ϴ� ������ ���� ��ī�̺� ������ �ǰ��� �� �����ϴ� ���丮�� ����� ��: �ڷᰡ �ٸ��ϴ� ��ī�̺꿡�� ����� �κ��� ������ ��ġ ��ȣ�� �ٲ���� %s ���丮�� �̸��� �ٲ�����ϴ� %s�� �� ���丮�Դϴ� �� �������� �ʽ��ϴ� ��ī�̺� ���Ͽ� EOF ������� ������ �ʿ��� ���� EOF�� ���� %s�� �ݴ� ���� ���� �߻� %s�� ����� ���� ���� �߻� ���ӵǾ� �ִ� ������ �Ϲ� ���Ϸ� ������ ������ �������� �ʽ��ϴ� ���� �̸� %s%s�� �ʹ� ��ϴ� ���� �̸� %s/%s�� �ʹ� ��ϴ� ������� ���� GNU tar ���� ������ ������ ������ �����մϴ�.
 gid�� �ٸ��ϴ� ��, �̰��� tar ��ī�̺�ó�� ������ �ʴ±��� �� �ɼǿ� �ΰ��Ǵ� �μ��� ���� ��, �̴� ������ �ǹ��� ª�� �ɼǿ���
����˴ϴ�.

  -l, --file-length=����     �����Ǵ� ������ ����
  -p, --pattern=����         ������ `default'�� `zeros'�Դϴ�
      --help                 �� ������ �����ְ� ��Ĩ�ϴ�
      --version              ���� ������ ����ϰ� ��Ĩ�ϴ�
 �������� ��¥ ���� `%s' �ɼǿ� �������� �׷��� �־��� �ɼǿ� �������� ��尡 �־��� �ɼǿ� �������� �����ڰ� �־��� record_size�� �������� �� �޸𸮰� �ٴڳ� -C �ڿ� ���� �̸��� ������ ���� �ð��� �ٸ��ϴ� ��尡 �ٸ��ϴ� ��� �Ǵ� ��ġ Ÿ���� ����� ���� ��ī�̺� ������ `-M' �ɼ��� �ʿ��մϴ� ��ī�̺� �̸��� �־����� �ʾҽ��ϴ� �� �̻� ���丮�� �ƴ� �� ������ �ƴ�; ����.
 �׷� �����̳� ���丮�� ���� �Ϲ����� ������ �ƴ� %s�� ������� ���� --absolute-names�� ��ü�Ǿ� ������� �� �ɼ� �̸� --backup���� ��ü�Ǿ� ������� �� �ɼ� �̸� --block-number�� ��ü�Ǿ� ������� �� �ɼ� �̸� --block-factor�� ��ü�Ǿ� ������� �� �ɼ� �̸� --read-full-records�� ��ü�Ǿ� ������� �� �ɼ� �̸� --touch�� ��ü�Ǿ� ������� �� �ɼ� �̸� --blocking-factor�� ���ԵǾ� ������� �� �ɼ� ������ �ɼ� `%c'�� �μ��� �ʿ��մϴ�. %s�� ���� `-%s'�� `-%s' �ɼ��� ��� ǥ�� �Է��� �ʿ��մϴ� `-Aru' �ɼ��� `-f -'�� ���ÿ� �� �� �����ϴ� `-[0-7][lmh]' �ɼ��� �� tar���� �������� �ʽ��ϴ� �߸��� ���� �� �˻����� %d�� ���� %s���� �б� ���� %s�� �д� ��
 ���ڵ� ũ��� %d�� ����� �Ǿ�� �մϴ�. ��ī�̺꿡 �ִ� �̸��鿡�� ����̺� �������� ������ ���� ��ũ�鿡�� �տ� ���� `/'�� ������ ��ī�̺� �ȿ� �ִ� ���� ��θ����� �տ� ���� `/'�� ������ %s�� %s�� �̸� �ٲ� ũ�Ⱑ �ٸ��ϴ� ���� ���� ����� �ǳ� �� ���� ����� �ǳ� �� ��ȣ��ũ�� �ٸ��ϴ� %s���� %s�� ��ȣ��ũ�Ǿ��� �̰��� tar ��ī�̺�ó�� ������ �ʽ��ϴ� �� ������ ������ ������ϴ� ������ �ʹ� ���Ƽ� �����մϴ� �� ������ ����Ʈ:  �� ���� ������ ������ `%s --help' �Ͻʽÿ�.
 uid�� �ٸ��ϴ� ��ī�̺꿡 ����ġ ���� EOF ��ī�̺� ���Ͽ� ����ġ ���� EOF �� �� ���� ���� `%s' �� �� ���� �ý��� ���� ���� ����: %d���� �������� ����� ����� ����  �������� �� �̸� ���� �������� ���̸� ���� ���� `%s'�� `%s'�� ��ġ���� �ʽ��ϴ� ���: ��ī�̺갡 �ҿ����մϴ� ���: %s�� ���� �� �����ϴ� (%d, %d) ���: ���� ��� ���� �˻����� %d�� �� `-Acdtrux' �ɼ� �� �ϳ� �̻��� �����ϸ� �� �˴ϴ� `-Acdtrux' �ɼǵ� �� �ϳ��� �����ؾ� �մϴ� exec/tcp: �� �� ���� ���� rmtd: ���� ������ �Ҵ��� �� �����ϴ�
 rmtd: ������� ���� %c
 rmtd: �߸��� eof
 ǥ���Է� ǥ����� tar (�ڽ�) tar (����) 